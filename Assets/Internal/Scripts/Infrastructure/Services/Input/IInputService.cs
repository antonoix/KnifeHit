using System;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Input
{
    public interface IInputService
    {
        event Action<Vector2> OnClicked;
        Vector2 GetPointerPosition();
    }
}