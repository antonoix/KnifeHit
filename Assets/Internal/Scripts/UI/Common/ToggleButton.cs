using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Internal.Scripts.UI.Common
{
    public class ToggleButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _toggledRoot;
        [SerializeField] private GameObject _untoggledRoot;
        
        public bool IsToggled { get; private set; }

        public event Action<bool> OnToggled; 
        
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            SwitchState(!IsToggled);
            OnToggled?.Invoke(IsToggled);
        }

        public void SwitchState(bool isToggled)
        {
            _toggledRoot.SetActive(isToggled);
            _untoggledRoot.SetActive(!isToggled);

            IsToggled = isToggled;
        }
    }
}