using System;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<MenuManager>().To<MenuManager>().AsSingle();

            Container.Bind<GameInfo>().AsSingle();
            Container.Bind<PlayerRowRegistry>().AsSingle();

            Container.Bind<PlayerRowModel>().AsTransient();
            Container.BindFactory<PlayerRowFacade, PlayerRowFacade.Factory>()
                .FromPrefab(_settings.PlayerRowFacadePrefab);

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

            Container.Bind<IInitializable>().To<GameInformationPresenter>().AsSingle();
            Container.Bind<IDisposable>().To<GameInformationPresenter>().AsSingle();
            Container.Bind<GameInformationPresenter>().AsSingle();

            Container.Bind<IInitializable>().To<AdvancedSettingsPresenter>().AsSingle();
            Container.Bind<IDisposable>().To<AdvancedSettingsPresenter>().AsSingle();
            Container.Bind<AdvancedSettingsPresenter>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject PlayerRowFacadePrefab;
        }
    }
}