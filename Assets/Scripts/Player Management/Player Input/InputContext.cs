using UnityEngine;

public class InputContext : MonoBehaviour
{
    public PlayerInputActions input;

    private void Awake() => input = new();
    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();
}