using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService.Base
{
    public class BaseUIPresenter<T> : IBaseUiView<T> where T : BaseUIView
    {
        protected readonly T _viewPrefab;
        protected T _view;

        public BaseUIPresenter(T baseUIViewPrefab)
        {
            _viewPrefab = baseUIViewPrefab;
        }

        public virtual void Show() 
            => _view.Show();

        public virtual void Hide() 
            => _view.Hide();

        public virtual void Initialize()
        {
            _view = Object.Instantiate(_viewPrefab);
            Object.DontDestroyOnLoad(_view);
            Hide();

            _view.Initialize();
        }
    }
}