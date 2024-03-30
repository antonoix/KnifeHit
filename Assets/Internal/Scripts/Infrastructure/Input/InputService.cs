using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Internal.Scripts.Infrastructure.Input
{
    public class InputService : IInitializable, ILateDisposable, IInputService
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

        public void LateDispose()
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
