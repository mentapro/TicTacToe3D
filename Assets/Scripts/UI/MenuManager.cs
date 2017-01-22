using Zenject;

namespace TicTacToe3D
{
    public class MenuManager
    {
        private IMenuPresenter _currentMenu;
        
        public void OpenMenu(IMenuPresenter menu)
        {
            _currentMenu.Close();
            menu.Open();
            _currentMenu = menu;
        }
    }
}