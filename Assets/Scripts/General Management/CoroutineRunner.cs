using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject runner = new("CoroutineRunner");
                DontDestroyOnLoad(runner);
                _instance = runner.AddComponent<CoroutineRunner>();
            }
            return _instance;
        }
    }
}