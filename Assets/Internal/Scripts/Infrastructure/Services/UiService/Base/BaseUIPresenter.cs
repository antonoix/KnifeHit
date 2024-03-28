using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService.Base
{
    public class BaseUIPresenter<T> : IBaseUiView<T> where T : BaseUIView
    {
        protected readonly T _viewPrefab;
        protected T _view;

        public virtual void Show() 
            => _view.Show();

        public virtual void Hide() 
            => _view.Hide();

        public virtual void Initialize(GameObject view)
        {
            _view = view.GetComponent<T>();
            Hide();

            _view.Initialize();
        }
    }
}