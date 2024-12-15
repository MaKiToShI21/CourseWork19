using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {        
        // Основной список ListOfRegistrations
        public ObservableCollection<Registration> ListOfRegistrations { get; set; } = new();

        // Поля для Registration
        public ObservableCollection<RegistrationMaterial> ListOfRegistrationMaterials { get; set; } = new();
        private Supplier supplier;
        private DateTime dateTime = DateTime.Today;

        // Поля для RegistrationMaterial
        private string nameOfMaterial;
        private double price;
        private int quantity;
        private string unit;

        // Список для MaterialsAtWarehouse
        private ObservableCollection<RegistrationMaterial> allRegistrationMaterials = new();

        // Доп. поля для изменения
        private Registration originalRegistration;
        private RegistrationMaterial selectedMaterial;
        private Registration selectedRegistration;

        // Доп. Свойства
        public ObservableCollection<Supplier> ListOfSuppliers { get; set; } = new();
        public DateTime MaxDate { get; set; } = DateTime.Today.AddMonths(6);

        // Комаднды для Регистрации
        public ICommand AddNewRegistrationCommand { get; }
        public ICommand PreparingToChangeRegistrationCommand { get; }
        public ICommand SaveRegistrationCommand { get; }        
        public ICommand RemoveRegistrationCommand { get; }

        // Комаднды для Материалов
        public ICommand AddNewRegistrationMaterialCommand { get; }
        public ICommand PreparingToChangeRegistrationMaterialCommand { get; }
        public ICommand SaveReceiptOfMaterialCommand { get; }
        public ICommand RemoveReceiptOfMaterialCommand { get; }

        // Команда Отмены
        public ICommand CancelCommand { get; }

        // Конструктор
        public RegistrationViewModel()
        {
            AddNewRegistrationCommand = new Command(() => { AddNewRegistration(); });
            PreparingToChangeRegistrationCommand = new Command(() => { PreparingToChangeRegistration(); });
            SaveRegistrationCommand = new Command(() => { SaveRegistration(); });
            RemoveRegistrationCommand = new Command(() => { RemoveRegistration(); });


            AddNewRegistrationMaterialCommand = new Command(() => { AddNewRegistrationMaterial(); });
            PreparingToChangeRegistrationMaterialCommand = new Command(() => { PreparingToChangeRegistrationMaterial(); });
            SaveReceiptOfMaterialCommand = new Command(() => { SaveReceiptOfMaterial(); });
            RemoveReceiptOfMaterialCommand = new Command(() => { RemoveReceiptOfMaterial(); });

            CancelCommand = new Command<string>(Cancel);
        }

        // Свойства
        public ObservableCollection<RegistrationMaterial> AllRegistrationMaterials
        {
            get
            {
                var groupedMaterials = ListOfRegistrations
                    .Where(registration => registration.DateTime <= DateTime.Today) // Учитываем только те регистрации, где дата не превышает сегодня
                    .SelectMany(registration => registration.ListOfRegistrationMaterials)
                    .GroupBy(material => new { material.NameOfMaterial, material.Unit }) // Группируем по Названию и Единицам измерения
                    .Select(group => new RegistrationMaterial
                    {
                        NameOfMaterial = group.Key.NameOfMaterial,
                        Unit = group.Key.Unit,
                        Quantity = group.Sum(material => material.Quantity) // Суммируем количество
                    });

                return new ObservableCollection<RegistrationMaterial>(groupedMaterials);
            }
            set => allRegistrationMaterials = value;
        }

        public Supplier Supplier
        {
            get => supplier;
            set
            {
                if (Supplier != value)
                {
                    supplier = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                dateTime = value;
                OnPropertyChanged();
            }
        }

        public RegistrationMaterial SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                if (SelectedMaterial != value)
                {
                    selectedMaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        public Registration SelectedRegistration
        {
            get => selectedRegistration;
            set
            {
                if (value != selectedRegistration)
                {
                    selectedRegistration = value;
                    OnPropertyChanged();
                } 
            }
        }
        
        public string NameOfMaterial
        {
            get => nameOfMaterial;
            set
            {
                nameOfMaterial = value;
                OnPropertyChanged();
            }
        }

        public double Price
        {
            get => price;
            set
            {
                if (value >= 0)
                {
                    price = value;
                    OnPropertyChanged();
                }                
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                if (value >= 0)
                {
                    quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Unit
        {
            get => unit;
            set
            {
                unit = value;
                OnPropertyChanged();
            }
        }

        // Убираем выделение с Registration и очищаем поля
        private void AddNewRegistration()
        {
            if (SelectedRegistration != null)
            {
                SelectedRegistration = null;
                ClearRegistrationFields();
            }
        }

        // Подготовка к изменению регистрации
        private async void PreparingToChangeRegistration()
        {
            if (SelectedRegistration != null)
            {
                originalRegistration = new Registration(
                    SelectedRegistration.Supplier,
                    SelectedRegistration.DateTime,
                    new ObservableCollection<RegistrationMaterial>(SelectedRegistration.ListOfRegistrationMaterials.Select(m => new RegistrationMaterial(m)))
                );

                Supplier = SelectedRegistration.Supplier;
                DateTime = SelectedRegistration.DateTime;
                ListOfRegistrationMaterials = new ObservableCollection<RegistrationMaterial>(SelectedRegistration.ListOfRegistrationMaterials);
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите поставку", "Ок");
        }

        // Сохранение регистрации
        private async void SaveRegistration()
        {
            var hasErrors = await HasValidationErrors(
                (Supplier == null, "Выберите поставщика!"),
                (ListOfRegistrationMaterials.Count < 1, "Добавьте материалы!"));

            if (hasErrors) return;

            if (SelectedRegistration == null)
                ListOfRegistrations.Add(new Registration(Supplier, DateTime, ListOfRegistrationMaterials));
            else
            {
                SelectedRegistration.Supplier = Supplier;
                SelectedRegistration.DateTime = DateTime;
                SelectedRegistration.ListOfRegistrationMaterials = ListOfRegistrationMaterials;
                SelectedRegistration = null;
            }

            ClearRegistrationFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление регистрации
        private async void RemoveRegistration()
        {
            if (SelectedRegistration != null)
            {
                ListOfRegistrations.Remove(selectedRegistration);
                SelectedRegistration = null;

                await App.Current.MainPage.DisplayAlert("Уведомление", "Поставка удалена", "Ок");
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите поставку", "Ок");
        }

        // Убираем выделение с RegistrationMaterial и очищаем поля
        private void AddNewRegistrationMaterial()
        {
            if (SelectedMaterial != null)
            {
                SelectedMaterial = null;
                ClearMaterialFields();
            }
        }

        // Подготовка к изменению материала
        private async void PreparingToChangeRegistrationMaterial()
        {
            if (SelectedMaterial != null)
            {
                NameOfMaterial = SelectedMaterial.NameOfMaterial;
                Unit = SelectedMaterial.Unit;
                Quantity = SelectedMaterial.Quantity;
                Price = SelectedMaterial.Price;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите объект!", "Ок");
        }

        // Сохранение материала
        private async void SaveReceiptOfMaterial()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(NameOfMaterial), "Выберите материал!"),
                (string.IsNullOrEmpty(Unit), "Выберите единицы измерения!"),
                (Quantity < 1, "Введите количество!"),
                (Price <= 0, "Некорректное значение цены!"));

            if (hasErrors) return;

            bool flag = false;
            foreach (var material in ListOfRegistrationMaterials)
            {
                if (material.NameOfMaterial == NameOfMaterial && material.Unit == Unit && material != SelectedMaterial)
                {
                    flag = true;
                    material.Quantity += Quantity;
                    material.Price += Price;
                    if (SelectedMaterial != null)
                        RemoveReceiptOfMaterial();
                    break;
                }
            }
            if (SelectedMaterial == null)
            {
                if (!flag)
                    ListOfRegistrationMaterials.Add(new RegistrationMaterial(NameOfMaterial, Price, Quantity, Unit));
            }
            else
            {
                if (!flag)
                {
                    SelectedMaterial.NameOfMaterial = NameOfMaterial;
                    SelectedMaterial.Unit = Unit;
                    SelectedMaterial.Quantity = Quantity;
                    SelectedMaterial.Price = Price;
                }
                SelectedMaterial = null;
            }

            ClearMaterialFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление материала
        private async void RemoveReceiptOfMaterial()
        {
            if (SelectedMaterial != null)
            {
                ListOfRegistrationMaterials.Remove(SelectedMaterial);
                SelectedMaterial = null;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите материал!", "Ок");
        }

        // Отмена
        private async void Cancel(string parameter)
        {
            if (parameter is string cancelType)
            {
                if (cancelType == "Registration")
                {
                    if (SelectedRegistration != null)
                    {
                        SelectedRegistration.ListOfRegistrationMaterials = new ObservableCollection<RegistrationMaterial>(originalRegistration.ListOfRegistrationMaterials);
                        SelectedRegistration = null;
                        ClearRegistrationFields();
                    }
                }
                else if (cancelType == "RegistrationMaterial")
                {
                    SelectedMaterial = null;
                    ClearMaterialFields();
                }
                    
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Очитска полей у Registration
        private void ClearRegistrationFields()
        {
            Supplier = null;
            DateTime = DateTime.Today;
            ListOfRegistrationMaterials = new();
        }

        // Очитска поелй у RegistrationMaterial
        private void ClearMaterialFields()
        {
            NameOfMaterial = string.Empty;
            Unit = string.Empty;
            Quantity = 0;
            Price = 0;
        }
    }
}
