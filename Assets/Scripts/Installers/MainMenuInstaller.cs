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
            Container.Bind<HighscoreItemModel.Registry>().AsSingle();
            Container.Bind<IFetchService<Stats>>().To<StatsFetchService>().AsSingle();

            Container.Bind<PlayerRowMenuModel>().AsTransient();
            Container.Bind<HighscoreItemModel>().AsTransient();

            InstallPresenters();
            InstallFactories();
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<NewGameMenuPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInformationPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdvancedSettingsPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<HighscoresMenuPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ModalDialog>().AsSingle();
        }

        private void InstallFactories()
        {
            Container.BindFactory<PlayerRowMenuFacade, PlayerRowMenuFacade.Factory>()
                .FromComponentInNewPrefab(_settings.PlayerRowMenuFacadePrefab);
            Container.BindFactory<HighscoreItemFacade, HighscoreItemFacade.Factory>()
                .FromComponentInNewPrefab(_settings.HighscoresItemPrefab);
        }

        [Serializable]
        public class Settings
        {
            public GameObject PlayerRowMenuFacadePrefab;
            public GameObject HighscoresItemPrefab;
        }
    }
}