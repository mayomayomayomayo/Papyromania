using UnityEngine;

// This is... uh, subpar (figure out a cleaner way)

public class GameManager : MonoBehaviour
{
    public GameManager Instance;
    public Player mainPlayer;
    public int randomSeed = 12345;

    private void Awake()
    {
        AssertSingleton();
        Init();
    }

    private void AssertSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Init()
    {
        Random.InitState(randomSeed);
        CardManager.StartUp();

        Players.host = mainPlayer;
    }
}
