using System.Collections.Generic;
using System.ComponentModel;
using Zenject;

namespace TicTacToe3D
{
    public class GameInfo : IInitializable, INotifyPropertyChanged
    {
        private int _dimension;
        private int _badgesToWin;
        private int _stepSize;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Dimension
        {
            get
            {
                return _dimension;
            }
            set
            {
                if (value == _dimension) return;
                _dimension = value;
                NotifyPropertyChanged("Dimension");
            }
        }

        public int BadgesToWin
        {
            get
            {
                return _badgesToWin;
            }
            set
            {
                if (value == _badgesToWin) return;
                _badgesToWin = value;
                NotifyPropertyChanged("BadgesToWin");
            }
        }

        public int StepSize
        {
            get
            {
                return _stepSize;
            }
            set
            {
                if (value == _stepSize) return;
                _stepSize = value;
                NotifyPropertyChanged("StepSize");
            }
        }
        
        public IList<Player> Players { get; private set; }

        public void Initialize()
        {
            Players = new List<Player>();
        }
        
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}