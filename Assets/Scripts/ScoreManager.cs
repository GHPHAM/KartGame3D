public static class ScoreManager
{
    // Fired by AttackerStats.die() with the killer's scoreValue.
    // HUDController subscribes to this to update the score display.
    public static event System.Action<int> OnEnemyKilled;

    public static void NotifyKill(int scoreValue)
    {
        OnEnemyKilled?.Invoke(scoreValue);
    }
}
