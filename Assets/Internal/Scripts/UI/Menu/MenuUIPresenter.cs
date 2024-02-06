using System;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using Object = UnityEngine.Object;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIPresenter : BaseUIPresenter<MenuUIView>
    {
        public event Action OnStartBtnClicked;

        public MenuUIPresenter(MenuUIView baseUIViewPrefab) : base(baseUIViewPrefab)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            
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
