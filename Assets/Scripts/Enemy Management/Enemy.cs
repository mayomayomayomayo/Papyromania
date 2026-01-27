using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthManager hm;

    private void Awake()
    {
        hm.mono = this;
    }
}