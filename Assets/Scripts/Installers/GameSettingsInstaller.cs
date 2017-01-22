using Zenject;

namespace TicTacToe3D
{
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameSettings GameSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(GameSettings);
        }
    }
}