using Zenject;

namespace TicTacToe3D
{
    public class MenuController : IInitializable
    {
        private MenuManager _menuManager;
        private MainMenuPresenter _mainMenu;

        public MenuController(MenuManager menuManager, MainMenuPresenter mainMenu)
        {
            _menuManager = menuManager;
            _mainMenu = mainMenu;
        }

        public void Initialize()
        {
            _menuManager.OpenMenu(_mainMenu);
        }
    }
}