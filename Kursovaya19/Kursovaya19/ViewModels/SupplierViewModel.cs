using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class SupplierViewModel : BaseViewModel
    {
        // Поля для Supplier
        private Building building;
        private string fullName;
        private string phoneNumber;
        private string bankName;
        private string currentAccount;
        private string inn;

        // Доп. поля для изменения
        private Supplier originalSupplier;
        private Supplier selectedSupplier;

        // Основной список ListOfSuppliers
        public ObservableCollection<Supplier> ListOfSuppliers { get; set; } = new();

        // Список ListOfBuildings
        public ObservableCollection<Building> ListOfBuildings { get; set; } = new();

        // Команды для Supplier
        public ICommand AddNewSupplierCommand { get; }
        public ICommand PreparingToChangeSupplierCommand { get; }
        public ICommand SaveSupplierCommand { get; }
        public ICommand RemoveSupplierCommand { get; }
        public ICommand CancelCommand { get; }

        // Конструктор
        public SupplierViewModel()
        {
            AddNewSupplierCommand = new Command(() => { AddNewSupplier(); });
            PreparingToChangeSupplierCommand = new Command(() => { PreparingToChangeSupplier(); });
            SaveSupplierCommand = new Command(() => { SaveSupplier(); });
            RemoveSupplierCommand = new Command(RemoveSupplier);

            CancelCommand = new Command<string>(Cancel);
        }

        // Свойства

        public Supplier SelectedSupplier
        {
            get => selectedSupplier;
            set
            {
                selectedSupplier = value;
                OnPropertyChanged();
            }
        }

        public Building Building
        {
            get => building;
            set
            {
                building = value;
                OnPropertyChanged();
            }
        }

        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string BankName
        {
            get => bankName;
            set
            {
                bankName = value;
                OnPropertyChanged();
            }
        }

        public string CurrentAccount
        {
            get => currentAccount;
            set
            {
                currentAccount = value;
                OnPropertyChanged();
            }
        }

        public string INN
        {
            get => inn;
            set
            {
                inn = value;
                OnPropertyChanged();
            }
        }

        // Убираем выделение с Supplier и очищаем поля
        private void AddNewSupplier()
        {
            if (SelectedSupplier != null)
            {
                SelectedSupplier = null;
                ClearSupplierFields();
            }
        }

        // Подготовка к изменению Supplier
        private async void PreparingToChangeSupplier()
        {
            if (SelectedSupplier != null)
            {
                originalSupplier = new Supplier(
                    SelectedSupplier.Building,
                    SelectedSupplier.FullName,
                    SelectedSupplier.PhoneNumber,
                    SelectedSupplier.BankName,
                    SelectedSupplier.CurrentAccount,
                    SelectedSupplier.INN);

                Building = SelectedSupplier.Building;
                FullName = SelectedSupplier.FullName;
                PhoneNumber = SelectedSupplier.PhoneNumber;
                BankName = SelectedSupplier.BankName;
                CurrentAccount = SelectedSupplier.CurrentAccount;
                INN = SelectedSupplier.INN;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите поставщика", "Ок");
        }

        // Сохранение Supplier
        private async void SaveSupplier()
        {
            var hasErrors = await HasValidationErrors(
                (Building == null, "Выберите строящийся объект!"),
                (string.IsNullOrEmpty(FullName), "Введите ФИО поставщика!"),
                (string.IsNullOrEmpty(PhoneNumber), "Введите номер телефона!"),
                (PhoneNumber?.Length != 12, "Некорректный номер телефона!"),
                (string.IsNullOrEmpty(BankName), "Введите название банка!"),
                (string.IsNullOrEmpty(CurrentAccount), "Введите номер счёта!"),
                (CurrentAccount?.Length != 20, "В номере банковского счёта должно быть 20 цифр!"),
                (string.IsNullOrEmpty(INN), "Введите ИНН поставщика!"),
                (INN?.Length != 12, "В ИНН должно быть 12 цифр!"));

            if (hasErrors) return;

            if (SelectedSupplier == null)
                ListOfSuppliers.Add(new Supplier(Building, FullName, PhoneNumber, BankName, CurrentAccount, INN));
            else
            {
                SelectedSupplier.Building = Building;
                SelectedSupplier.FullName = FullName;
                SelectedSupplier.PhoneNumber = PhoneNumber;
                SelectedSupplier.BankName = BankName;
                SelectedSupplier.CurrentAccount = CurrentAccount;
                SelectedSupplier.INN = INN;
                SelectedSupplier = null;
            }

            ClearSupplierFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление Supplier
        private async void RemoveSupplier()
        {
            if (SelectedSupplier != null)
            {
                ListOfSuppliers.Remove(SelectedSupplier);
                SelectedSupplier = null;

                await App.Current.MainPage.DisplayAlert("Уведомление", "Поставщик успешно удалён!", "Ок");
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите поставщика!", "Ок");
        }

        // Отмена
        private async void Cancel(string parameter)
        {
            if (parameter is string cancelType)
            {
                if (cancelType == "Supplier")
                {
                    SelectedSupplier = null;
                    ClearSupplierFields();
                }

                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Очистка полей у Supplier
        private void ClearSupplierFields()
        {
            Building = null;
            FullName = string.Empty;
            PhoneNumber = string.Empty;
            BankName = string.Empty;
            CurrentAccount = string.Empty;
            INN = string.Empty;
        }
    }
}
