using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
  public class InputHandler : MonoBehaviour
  {
    private PlayerInputs _inputs;

    public event Action<Vector3> ShootPressed;
    
    private void Awake() => 
      _inputs = new PlayerInputs();

    private void OnEnable()
    {
      _inputs.Enable();

      _inputs.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable() => 
      _inputs.Disable();

    private void OnShoot(InputAction.CallbackContext context)
    {
      var mousePosition = Mouse.current.position.ReadValue();
      
      ShootPressed?.Invoke(mousePosition);
    }
  }
}
