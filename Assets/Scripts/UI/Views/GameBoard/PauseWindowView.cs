using Zenject;

namespace TicTacToe3D
{
    public class PauseWindowView : MenuView
    {
        [Inject]
        public void Construct(PauseWindowPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }
    }
}