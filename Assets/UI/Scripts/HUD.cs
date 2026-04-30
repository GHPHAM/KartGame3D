using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class HUDController : MonoBehaviour
{
    // -- Raised by VehicleController.damage() so the HUD can react ----------
    // Subscribe to this from VehicleController instead of polling every frame.
    public static event System.Action<int, int> OnPlayerDamaged; // (currentHP, maxHP)

    // -- Internals ----------------------------------------------------------
    private VisualElement _healthBar;   // the green "Health" fill element
    private Label _scoreLabel;          // the "Score:" label in ScoreWrapper

    private int _score = 0;

    // -- Unity event: called once the UI document is fully built ------------
    void OnEnable()
    {
        UIDocument doc = GetComponent<UIDocument>();
        VisualElement root = doc.rootVisualElement;

        // Grab the fill bar by the name defined in HUD.uxml
        _healthBar = root.Q<VisualElement>("Health");

        if (_healthBar == null)
        {
            Debug.LogError("[HUDController] Could not find 'Health' element in HUD.uxml.");
            return;
        }

        // Grab the score label by the name defined in HUD.uxml
        _scoreLabel = root.Q<Label>("Score");

        if (_scoreLabel == null)
        {
            Debug.LogError("[HUDController] Could not find 'Score' label in HUD.uxml.");
        }

        // Listen for damage events fired by VehicleController
        OnPlayerDamaged += HandlePlayerDamaged;

        // Listen for kill events fired through ScoreManager
        ScoreManager.OnEnemyKilled += HandleEnemyKilled;

        // Initialize the bar using the player's current stats
        InitializeHealthBar();

        // Initialize score display
        UpdateScoreLabel();
    }

    void OnDisable()
    {
        OnPlayerDamaged -= HandlePlayerDamaged;
        ScoreManager.OnEnemyKilled -= HandleEnemyKilled;
    }

    // -- Initialize bar as currentHealth / maxHealth ratio ------------------
    void InitializeHealthBar()
    {
        VehicleController player = VehicleController.singleton;

        if (player == null)
        {
            Debug.LogWarning("[HUDController] VehicleController.singleton is null on init. " +
                             "Make sure the player exists before this GameObject.");
            SetHealthBarRatio(1f);  // default to full until player is ready
            return;
        }

        float ratio = Mathf.Clamp01((float)player.health / player.currentStats.maxHealth);
        SetHealthBarRatio(ratio);
    }

    // -- Called whenever VehicleController.damage() fires the event ---------
    void HandlePlayerDamaged(int currentHP, int maxHP)
    {
        float ratio = Mathf.Clamp01((float)currentHP / maxHP);
        SetHealthBarRatio(ratio);
    }

    // -- Called whenever an enemy dies; awards its scoreValue ---------------
    void HandleEnemyKilled(int scoreValue)
    {
        _score += scoreValue;
        UpdateScoreLabel();
    }

    // -- Set the green bar's width as a percentage of its parent ------------
    void SetHealthBarRatio(float ratio)
    {
        if (_healthBar == null) return;

        // Width is expressed as a percentage of the grey background parent
        _healthBar.style.width = new StyleLength(new Length(ratio * 100f, LengthUnit.Percent));
    }

    // -- Refresh the score text shown to the player -------------------------
    void UpdateScoreLabel()
    {
        if (_scoreLabel == null) return;
        _scoreLabel.text = $"Score: {_score}";
    }

    // -- Static helper so VehicleController can fire the event easily -------
    public static void NotifyDamage(int currentHP, int maxHP)
    {
        OnPlayerDamaged?.Invoke(currentHP, maxHP);
    }
}