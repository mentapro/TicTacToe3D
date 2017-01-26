namespace TicTacToe3D
{
    public enum Menus
    {
        MainMenu,
        NewGameMenu,
        GameInfoMenu,
        AdvancedSettingsMenu
    }

    public class MenuManager
    {
        private IMenuPresenter CurrentMenu { get; set; }

        private MainMenuPresenter MainMenu { get; set; }
        private NewGameMenuPresenter NewGameMenu { get; set; }
        private GameInformationPresenter GameInfoMenu { get; set; }
        private AdvancedSettingsPresenter AdvancedSettingsMenu { get; set; }

        private void OpenMenu(IMenuPresenter menu)
        {
            if (CurrentMenu != null)
            {
                CurrentMenu.Close();
            }
            menu.Open();
            CurrentMenu = menu;
        }

        public void OpenMenu(Menus menu)
        {
            switch (menu)
            {
                case Menus.MainMenu:
                    OpenMenu(MainMenu);
                    break;
                case Menus.NewGameMenu:
                    OpenMenu(NewGameMenu);
                    break;
                case Menus.GameInfoMenu:
                    GameInfoMenu.Open();
                    break;
                case Menus.AdvancedSettingsMenu:
                    AdvancedSettingsMenu.Open();
                    break;
            }
        }

        public void CloseMenu(Menus menu)
        {
            switch (menu)
            {
                case Menus.MainMenu:
                    MainMenu.Close();
                    CurrentMenu = null;
                    break;
                case Menus.NewGameMenu:
                    NewGameMenu.Close();
                    CurrentMenu = null;
                    break;
                case Menus.GameInfoMenu:
                    GameInfoMenu.Close();
                    break;
                case Menus.AdvancedSettingsMenu:
                    AdvancedSettingsMenu.Close();
                    break;
            }
        }

        public void SetMenu(MainMenuPresenter mainMenu)
        {
            MainMenu = mainMenu;
        }

        public void SetMenu(NewGameMenuPresenter newGameMenu)
        {
            NewGameMenu = newGameMenu;
        }

        public void SetMenu(GameInformationPresenter gameInfoMenu)
        {
            GameInfoMenu = gameInfoMenu;
        }

        public void SetMenu(AdvancedSettingsPresenter advancedSettingsMenu)
        {
            AdvancedSettingsMenu = advancedSettingsMenu;
        }
    }
}