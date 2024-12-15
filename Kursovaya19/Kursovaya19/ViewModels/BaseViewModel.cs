using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya19.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Обновление данных в UI
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        // Проверка на корректность ввода данных
        protected async Task<bool> HasValidationErrors(params (bool condition, string message)[] errors)
        {
            foreach (var (condition, message) in errors)
            {
                if (condition)
                {
                    App.Current.MainPage.DisplayAlert("Уведомление", message, "Ок");
                    return true;
                }
            }
            return false;
        }
    }
}
