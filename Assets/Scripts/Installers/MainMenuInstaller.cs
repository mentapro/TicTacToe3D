using System;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        [Inject]
        private Settings _settings = null;

        public override void InstallBindings()
        {
            Container.Bind<MenuManager>().AsSingle();

            Container.Bind<GameInfo>().AsSingle();
            Container.Bind<PlayerRowModel.Registry>().AsSingle();

            Container.Bind<PlayerRowModel>().AsTransient();
            Container.BindFactory<PlayerRowFacade, PlayerRowFacade.Factory>()
                .FromPrefab(_settings.PlayerRowFacadePrefab);

            Container.BindAllInterfacesAndSelf<ModalDialog>().To<ModalDialog>().AsSingle();

            InstallPresenters();
        }

        private void InstallPresenters()
        {
            Container.BindAllInterfacesAndSelf<MainMenuPresenter>().To<MainMenuPresenter>().AsSingle();
            Container.BindAllInterfacesAndSelf<NewGameMenuPresenter>().To<NewGameMenuPresenter>().AsSingle();
            Container.BindAllInterfacesAndSelf<GameInformationPresenter>().To<GameInformationPresenter>().AsSingle();
            Container.BindAllInterfacesAndSelf<AdvancedSettingsPresenter>().To<AdvancedSettingsPresenter>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject PlayerRowFacadePrefab;
        }
    }
}