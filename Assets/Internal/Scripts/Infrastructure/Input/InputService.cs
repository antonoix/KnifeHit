using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Internal.Scripts.Infrastructure.Input
{
    public class InputService : IDisposable
    {
        private readonly PlayerInputMap _playerMap;
        
        public event Action<Vector2> OnClicked;

        public InputService()
        {
            _playerMap = new PlayerInputMap();
        }

        public void Initialize()
        {
            _playerMap.Enable();
            _playerMap.Player.Click.started += HandleClick;
        }
        
        public void Dispose()
        {
            _playerMap.Disable();
            _playerMap.Player.Click.started -= HandleClick;
        }
        
        public Vector2 GetPointerPosition()
        {
            return _playerMap.Player.PointerPosition.ReadValue<Vector2>();
        }

        private void HandleClick(InputAction.CallbackContext context)
        {
            OnClicked?.Invoke(GetPointerPosition());
        }
    }
}
