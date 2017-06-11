namespace TicTacToe3D
{
    public interface IMenuPresenter
    {
        void Open();
        void Close();
    }

    public abstract class MenuPresenter<TView> : IMenuPresenter where TView : MenuView
    {
        private readonly AudioController _audioController;
        protected TView View { get; private set; }

        public MenuPresenter(AudioController audioController)
        {
            _audioController = audioController;
        }

        public void SetView(TView view)
        {
            View = view;
        }

        public virtual void Open()
        {
            _audioController.Source.PlayOneShot(_audioController.AudioSettings.MenuOpenedClip);
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