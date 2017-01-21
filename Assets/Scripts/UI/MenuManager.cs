using Zenject;

namespace TicTacToe3D
{
    public class MenuManager : IInitializable
    {
        private IMenuPresenter _currentMenu;

        public MenuManager(IMenuPresenter menu)
        {
            _currentMenu = menu;
        }

        public void Initialize()
        {
            OpenMenu(_currentMenu);
        }
        
        public void OpenMenu(IMenuPresenter menu)
        {
            _currentMenu.Close();
            menu.Open();
            _currentMenu = menu;
        }
    }
}