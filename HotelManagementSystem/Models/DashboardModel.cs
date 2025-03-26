using System.ComponentModel;

namespace HotelManagementSystem.Models
{
    public class DashboardModel : INotifyPropertyChanged
    {
        private int _brojGostiju;
        private int _brojSoba;
        private int _brojZaposlenih;

        public int BrojGostiju
        {
            get => _brojGostiju;
            set
            {
                if (_brojGostiju != value)
                {
                    _brojGostiju = value;
                    OnPropertyChanged(nameof(BrojGostiju));
                }
            }
        }

        public int BrojSoba
        {
            get => _brojSoba;
            set
            {
                if (_brojSoba != value)
                {
                    _brojSoba = value;
                    OnPropertyChanged(nameof(BrojSoba));
                }
            }
        }

        public int BrojZaposlenih
        {
            get => _brojZaposlenih;
            set
            {
                if (_brojZaposlenih != value)
                {
                    _brojZaposlenih = value;
                    OnPropertyChanged(nameof(BrojZaposlenih));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
