using QLBVCB.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace QLBVCB.ViewModel
{
    public class BoolToColorConverterEmpty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChosen = (bool)value;
            return isChosen ? new SolidColorBrush(Color.FromRgb(57, 93, 105)) : new SolidColorBrush(Color.FromArgb(255, 36, 255, 147));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   public class VM_EmptySeat : INotifyPropertyChanged
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

        public VM_EmptySeat()
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
