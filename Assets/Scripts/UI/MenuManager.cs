using Zenject;

namespace TicTacToe3D
{
    public class MenuManager
    {
        private IMenuPresenter CurrentMenu { get; set; }
        
        public void OpenMenu(IMenuPresenter menu)
        {
            if (CurrentMenu != null)
            {
                CurrentMenu.Close();
            }
            menu.Open();
            CurrentMenu = menu;
        }
    }
}