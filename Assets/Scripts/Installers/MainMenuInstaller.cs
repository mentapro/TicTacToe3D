using System;
using Zenject;

namespace TicTacToe3D
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<MenuManager>().To<MenuManager>().AsSingle();
            Container.Bind<IMenuPresenter>().To<MainMenuPresenter>().AsSingle().WhenInjectedInto<MenuManager>();

            InstallPresenters();
        }

        private void InstallPresenters()
        {
            Container.Bind<IInitializable>().To<MainMenuPresenter>().AsSingle();
            Container.Bind<IDisposable>().To<MainMenuPresenter>().AsSingle();
            Container.Bind<MainMenuPresenter>().AsSingle().WhenInjectedInto<MainMenuView>();
        }
    }
}