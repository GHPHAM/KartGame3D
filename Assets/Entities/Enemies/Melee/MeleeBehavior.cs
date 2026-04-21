using UnityEngine;

public class MeleeBehavior : EnemyBase
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float attackRange = 1.2f;

    // Cached refs
    private Rigidbody _rb;
    private Transform _player;

    // -------------------------------------------------------
    // Setup
    // -------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody>();

        // Freeze rotation so the enemy doesn't tip over from physics
        _rb.freezeRotation = true;

        // Find player by tag – make sure your player GameObject is tagged "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
        else
            Debug.LogWarning("MeleeBehavior: No GameObject tagged 'Player' found!");
    }

    // -------------------------------------------------------
    // Per-physics-frame update
    // -------------------------------------------------------

    protected new void FixedUpdate()
    {
        // Let EnemyBase handle the attack cooldown timer
        base.FixedUpdate();

        MoveTowardPlayer();
    }

    // -------------------------------------------------------
    // Movement
    // -------------------------------------------------------

    private void MoveTowardPlayer()
    {
        if (_player == null) return;

        Vector3 toPlayer = _player.position - transform.position;

        // Ignore vertical distance so the enemy doesn't fly/sink
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance <= attackRange)
        {
            // Close enough – kill horizontal velocity so we don't keep pushing
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            return;
        }

        // Move toward player at moveSpeed, preserving Y velocity (gravity)
        Vector3 moveDir = toPlayer.normalized;
        _rb.linearVelocity = new Vector3(
            moveDir.x * moveSpeed,
            _rb.linearVelocity.y,
            moveDir.z * moveSpeed
        );

        // Face the player
        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    // -------------------------------------------------------
    // Attack (called by EnemyBase cooldown)
    // -------------------------------------------------------

    protected override void attack()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(_player.position.x,   0, _player.position.z)
        );

        if (distance > attackRange) return;

        // Deal damage if the player exposes an EntityStats (or similar) component
        EntityStats playerStats = _player.GetComponent<EntityStats>();
        if (playerStats != null)
        {
            playerStats.damage(attackDamage);
        }
    }
}
