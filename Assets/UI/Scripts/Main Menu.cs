using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;  // add this

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string playSceneName = "Game";

    private VisualElement _creditsMenu;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _creditsMenu = root.Q<VisualElement>("Menu-Credits");

        root.Q<Button>("Play").clicked += () =>
            SceneManager.LoadScene(playSceneName);

        root.Q<Button>("Credits").clicked += OpenCredits;

        root.Q<Button>("Quit").clicked += () =>
            Application.Quit();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseCredits();
    }

    private void OpenCredits()
    {
        _creditsMenu.style.translate = new StyleTranslate(new Translate(new Length(0), new Length(0)));
    }

    private void CloseCredits()
    {
        _creditsMenu.style.translate = new StyleTranslate(new Translate(new Length(100, LengthUnit.Percent), new Length(0)));
    }
}