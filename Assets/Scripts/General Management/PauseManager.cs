using UnityEngine;

public static class PauseManager
{
    public static bool isPaused;

    public static void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}
