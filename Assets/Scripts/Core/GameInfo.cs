using System.Collections.Generic;
using System.ComponentModel;

namespace TicTacToe3D
{
    public class GameInfo : INotifyPropertyChanged
    {
        private int _dimension;
        private int _badgesToWin;
        private int _stepSize;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Player> Players { get; set; }

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
        
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}