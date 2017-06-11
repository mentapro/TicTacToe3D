namespace TicTacToe3D
{
    public enum Menus
    {
        // MainMenu Scene
        MainMenu,
        NewGameMenu,
        GameInfoMenu,
        AdvancedSettingsMenu,
        HighscoresMenu,
        LoadGameMenu,

        // GameBoard Scene
        ConfirmStepWindow,
        PlayAgainWindow,
        PauseWindow,
        SaveGameWindow,
        LoadGameWindow
    }

    public class MenuManager
    {
        public IMenuPresenter CurrentMenu { get; private set; }

        // MainMenu Scene
        private MainMenuPresenter MainMenu { get; set; }
        private NewGameMenuPresenter NewGameMenu { get; set; }
        private GameInformationPresenter GameInfoMenu { get; set; }
        private AdvancedSettingsPresenter AdvancedSettingsMenu { get; set; }
        private HighscoresMenuPresenter HighscoresMenu { get; set; }
        private LoadGameMenuPresenter LoadGameMenu { get; set; }

        // GameBoard Scene
        private ConfirmStepWindowPresenter ConfirmStepWindow { get; set; }
        private PlayAgainWindowPresenter PlayAgainWindow { get; set; }
        private PauseWindowPresenter PauseWindow { get; set; }
        private SaveGameWindowPresenter SaveGameWindow { get; set; }
        private LoadGameWindowPresenter LoadGameWindow { get; set; }

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
                case Menus.HighscoresMenu:
                    OpenMenu(HighscoresMenu);
                    break;
                case Menus.LoadGameMenu:
                    OpenMenu(LoadGameMenu);
                    break;
                case Menus.ConfirmStepWindow:
                    ConfirmStepWindow.Open();
                    break;
                case Menus.PlayAgainWindow:
                    PlayAgainWindow.Open();
                    break;
                case Menus.PauseWindow:
                    OpenMenu(PauseWindow);
                    break;
                case Menus.SaveGameWindow:
                    OpenMenu(SaveGameWindow);
                    break;
                case Menus.LoadGameWindow:
                    OpenMenu(LoadGameWindow);
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
                case Menus.HighscoresMenu:
                    HighscoresMenu.Close();
                    CurrentMenu = null;
                    break;
                case Menus.LoadGameMenu:
                    LoadGameMenu.Close();
                    CurrentMenu = null;
                    break;
                case Menus.ConfirmStepWindow:
                    ConfirmStepWindow.Close();
                    break;
                case Menus.PlayAgainWindow:
                    PlayAgainWindow.Close();
                    break;
                case Menus.PauseWindow:
                    PauseWindow.Close();
                    CurrentMenu = null;
                    break;
                case Menus.SaveGameWindow:
                    SaveGameWindow.Close();
                    CurrentMenu = null;
                    break;
                case Menus.LoadGameWindow:
                    LoadGameWindow.Close();
                    CurrentMenu = null;
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

        public void SetMenu(HighscoresMenuPresenter highscoresMenu)
        {
            HighscoresMenu = highscoresMenu;
        }

        public void SetMenu(LoadGameMenuPresenter loadGameMenu)
        {
            LoadGameMenu = loadGameMenu;
        }

        public void SetMenu(ConfirmStepWindowPresenter confirmStepWindow)
        {
            ConfirmStepWindow = confirmStepWindow;
        }

        public void SetMenu(PlayAgainWindowPresenter playAgainWindow)
        {
            PlayAgainWindow = playAgainWindow;
        }

        public void SetMenu(PauseWindowPresenter pauseWindow)
        {
            PauseWindow = pauseWindow;
        }

        public void SetMenu(SaveGameWindowPresenter saveGameWindow)
        {
            SaveGameWindow = saveGameWindow;
        }

        public void SetMenu(LoadGameWindowPresenter loadGameWindow)
        {
            LoadGameWindow = loadGameWindow;
        }
    }
}