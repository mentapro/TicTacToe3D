using System;
using Zenject;

namespace TicTacToe3D
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<MenuManager>().To<MenuManager>().AsSingle();
            Container.BindAllInterfacesAndSelf<MenuController>().To<MenuController>().AsSingle();

            InstallPresenters();
        }

        private void InstallPresenters()
        {
            Container.Bind<IInitializable>().To<MainMenuPresenter>().AsSingle();
            Container.Bind<IDisposable>().To<MainMenuPresenter>().AsSingle();
            Container.Bind<MainMenuPresenter>().AsSingle();

            Container.Bind<IInitializable>().To<NewGameMenuPresenter>().AsSingle();
            Container.Bind<IDisposable>().To<NewGameMenuPresenter>().AsSingle();
            Container.Bind<NewGameMenuPresenter>().AsSingle();
        }
    }
}