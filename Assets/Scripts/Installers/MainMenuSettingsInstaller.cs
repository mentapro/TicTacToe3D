using Zenject;

namespace TicTacToe3D
{
    public class MainMenuSettingsInstaller : ScriptableObjectInstaller<MainMenuSettingsInstaller>
    {
        public AdvancedSettingsPresenter.Settings AdvancedSettingsPresenterSettings;
        public NewGameMenuPresenter.Settings NewGameMenuSettings;
        public PlayerRowMenuModel.Settings PlayerRowSettings;
        public MainMenuInstaller.Settings MainMenuInstallerSettings;
        public AudioController.Settings AudioSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(AdvancedSettingsPresenterSettings);
            Container.BindInstance(NewGameMenuSettings);
            Container.BindInstance(PlayerRowSettings);
            Container.BindInstance(MainMenuInstallerSettings);
            Container.BindInstance(AudioSettings);
        }
    }
}