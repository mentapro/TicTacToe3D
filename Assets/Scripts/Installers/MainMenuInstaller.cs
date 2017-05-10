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
            Container.Bind<PlayerRowMenuModel.Registry>().AsSingle();

            Container.Bind<PlayerRowMenuModel>().AsTransient();
            Container.BindFactory<PlayerRowMenuFacade, PlayerRowMenuFacade.Factory>()
                .FromComponentInNewPrefab(_settings.PlayerRowMenuFacadePrefab);

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
            public GameObject PlayerRowMenuFacadePrefab;
        }
    }
}