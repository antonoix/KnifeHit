using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Internal.Scripts.UI
{
    public class MenuUIPresenter
    {
        private readonly MenuUIView _viewPrefab;
        private MenuUIView _view;

        public event Action OnStartBtnClicked;

        public MenuUIPresenter(MenuUIView viewPrefab)
        {
            _viewPrefab = viewPrefab;
        }

        public void Initialize()
        {
            _view = Object.Instantiate(_viewPrefab);
            _view.Initialize();
        
            _view.OnStartBtnClicked += HandleStartBtnClicked;
        }

        public void Dispose()
        {
            _view.Dispose();
        }

        private void HandleStartBtnClicked()
        {
            OnStartBtnClicked?.Invoke();
        }
    }
}
