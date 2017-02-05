using Zenject;

namespace TicTacToe3D
{
    public class GameBoardSettingsInstaller : ScriptableObjectInstaller<GameBoardSettingsInstaller>
    {
        public GameBoardSpawner.Settings GameBoardSpawnerSettings;
        public CameraHandler.Settings CameraSettings;
        public BadgeSpawnPoint.Settings BadgeSpawnPointSettings;
        public BadgeFacade.Settings BadgeSettings;
        public GameSettings GameSettings;
        public GameBoardInstaller.Settings GameBoardInstallerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(GameBoardSpawnerSettings);
            Container.BindInstance(CameraSettings);
            Container.BindInstance(BadgeSpawnPointSettings);
            Container.BindInstance(BadgeSettings);
            Container.BindInstance(GameSettings);
            Container.BindInstance(GameBoardInstallerSettings);
        }
    }
}