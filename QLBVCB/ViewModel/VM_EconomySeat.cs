using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace QLBVCB.ViewModel
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChosen = (bool)value;
            return isChosen ? new SolidColorBrush(Color.FromRgb(57, 93, 105)) : new SolidColorBrush(Color.FromRgb(6, 9, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VM_EconomySeat : INotifyPropertyChanged
    {
        private bool _isChosen;
        public bool IsChosen
        {
            get => _isChosen;
            set
            {
                _isChosen = value;
                OnPropertyChanged(nameof(IsChosen));
            }
        }

        public ICommand ToggleCommand { get; }

        public VM_EconomySeat()
        {
            ToggleCommand = new RelayCommand(ToggleChosen);
        }

        private void ToggleChosen(object parameter)
        {
            IsChosen = !IsChosen;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
