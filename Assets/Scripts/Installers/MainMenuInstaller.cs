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
                .FromComponentInNewPrefab(_settings.PlayerRowFacadePrefab);

            Container.BindInterfacesAndSelfTo<ModalDialog>().AsSingle();

            InstallPresenters();
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<NewGameMenuPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInformationPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdvancedSettingsPresenter>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject PlayerRowFacadePrefab;
        }
    }
}