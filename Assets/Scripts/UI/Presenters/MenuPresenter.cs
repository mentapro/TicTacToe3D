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

        public virtual void Open()
        {
            View.transform.SetAsLastSibling();
            View.IsOpen = true;
        }

        public virtual void Close()
        {
            View.transform.SetAsFirstSibling();
            View.IsOpen = false;
        }
    }
}