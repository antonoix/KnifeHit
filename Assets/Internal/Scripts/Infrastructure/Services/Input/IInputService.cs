using System;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Input
{
    public interface IInputService
    {
        event Action<Vector2> OnClicked;
        Vector2 GetPointerPosition();
    }
}