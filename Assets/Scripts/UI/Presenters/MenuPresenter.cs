namespace TicTacToe3D
{
    public interface IMenuPresenter
    {
        void Open();
        void Close();
    }

    public abstract class MenuPresenter<TView> : IMenuPresenter where TView : MenuView
    {
        protected TView View { get; private set; }

        public void SetView(TView view)
        {
            View = view;
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }
    }
}