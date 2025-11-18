using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance;
    public int randomSeed = 12345;

    private void Awake()
    {
        SingletonInitialize();
        Init();
    }

    private void SingletonInitialize()
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

        foreach (CardObject co in Cards.byName.Values) Debug.Log($"{co.cardName} -> {co.ObjectID}");
    }
}
