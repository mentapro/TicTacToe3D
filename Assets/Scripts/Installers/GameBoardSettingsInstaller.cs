using Zenject;

namespace TicTacToe3D
{
    public class GameBoardSettingsInstaller : ScriptableObjectInstaller<GameBoardSettingsInstaller>
    {
        public GameBoardSpawner.Settings NewGameMenuSettings;
        public GameBoardInstaller.Settings GameBoardInstallerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(NewGameMenuSettings);
            Container.BindInstance(GameBoardInstallerSettings);
        }
    }
}