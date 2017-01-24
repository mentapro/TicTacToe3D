using Zenject;

namespace TicTacToe3D
{
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameSettings GameSettings;
        public NewGameMenuPresenter.Settings NewGameMenuSettings;
        public PlayerRowModel.Settings PlayerRowSettings;
        public MainMenuInstaller.Settings MainMenuInstaller;

        public override void InstallBindings()
        {
            Container.BindInstance(GameSettings);
            Container.BindInstance(NewGameMenuSettings);
            Container.BindInstance(PlayerRowSettings);
            Container.BindInstance(MainMenuInstaller);
        }
    }
}