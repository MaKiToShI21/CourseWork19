using System.Collections.ObjectModel;
using System.ComponentModel; 
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class BuildingsViewModel : BaseViewModel
    {
        // Основной список Building
        public ObservableCollection<Building> ListOfBuildings { get; set; } = new();

        // Поля Building
        public ObservableCollection<EstimateMaterial> ListOfEstimateMaterials { get; set; } = new();
        private string nameOfBuilding;
        private string addressOfBuilding;

        // Поля EstimateMaterial
        private string nameOfMaterial;
        private int quantity;
        private string unit;

        // Доп. поля для изменения
        private Building originalBuilding;
        private Building selectedBuilding;
        private EstimateMaterial selectedMaterial;

        // Команды для Building
        public ICommand AddNewBuildingCommand { get; }
        public ICommand PreparingToChangeBuildingCommand { get; }
        public ICommand SaveBuildingCommand { get; }
        public ICommand RemoveBuildingCommand { get; }

        // Команды для EstimateMaterial
        public ICommand AddNewEstimateMaterialCommand { get; }
        public ICommand SaveEstimateMaterialCommand { get; }
        public ICommand PreparingToChangeEstimateMaterialCommand { get; }
        public ICommand RemoveEstimateMaterialCommand { get; }

        // Команда Отмены
        public ICommand CancelCommand { get; }

        // Конструктор
        public BuildingsViewModel()
        {
            // Команды для Building
            AddNewBuildingCommand = new Command(() => { AddNewBuilding(); });
            SaveBuildingCommand = new Command(() => { SaveBuilding(); });
            PreparingToChangeBuildingCommand = new Command(() => { PreparingToChangeBuilding(); });
            RemoveBuildingCommand = new Command(() => { RemoveBuilding(); });

            // Команды для EstimateMaterial
            AddNewEstimateMaterialCommand = new Command(() => { AddNewEstimateMaterial(); });
            SaveEstimateMaterialCommand = new Command(() => { SaveEstimateMaterial(); });
            PreparingToChangeEstimateMaterialCommand = new Command(() => { PreparingToChangeEstimateMaterial(); });
            RemoveEstimateMaterialCommand = new Command(() => { RemoveEstimateMaterial(); });

            // Команда Отмены
            CancelCommand = new Command<string>(Cancel);
        }

        // Свойства
        public Building SelectedBuilding
        {
            get => selectedBuilding;
            set
            {
                if (selectedBuilding != value)
                {
                    selectedBuilding = value;
                    OnPropertyChanged();
                }
            }
        }

        public EstimateMaterial SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                if (selectedMaterial != value)
                {
                    selectedMaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NameOfBuilding
        {
            get => nameOfBuilding;
            set
            {
                nameOfBuilding = value;
                OnPropertyChanged();
            }
        }

        public string AddressOfBuilding
        {
            get => addressOfBuilding;
            set
            {
                addressOfBuilding = value;
                OnPropertyChanged();
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

        // Убираем выделение с Building и очищаем поля
        private void AddNewBuilding()
        {
            SelectedBuilding = null;
            ClearBuildingFields();
        }


        // Подготовка к изменению Building
        private async void PreparingToChangeBuilding()
        {
            if (SelectedBuilding != null)
            {
                originalBuilding = new Building(
                    SelectedBuilding.NameOfBuilding,
                    SelectedBuilding.AddressOfBuilding,
                    new ObservableCollection<EstimateMaterial>(SelectedBuilding.ListOfEstimateMaterials.Select(m => new EstimateMaterial(m))
            )
                );

                NameOfBuilding = SelectedBuilding.NameOfBuilding;
                AddressOfBuilding = SelectedBuilding.AddressOfBuilding;
                ListOfEstimateMaterials = new ObservableCollection<EstimateMaterial>(SelectedBuilding.ListOfEstimateMaterials);
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите объект", "Ок");
        }

        // Сохранение Building
        private async void SaveBuilding()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(NameOfBuilding), "Введите название строящегося объекта!"),
                (string.IsNullOrEmpty(AddressOfBuilding), "Введите адрес строящегося объекта!"),
                (ListOfEstimateMaterials.Count == 0, "Добавьте материалы!"));

            if (hasErrors) return;

            if (SelectedBuilding == null)
                ListOfBuildings.Add(new Building(NameOfBuilding, AddressOfBuilding, ListOfEstimateMaterials));
            else
            {
                SelectedBuilding.NameOfBuilding = NameOfBuilding;
                SelectedBuilding.AddressOfBuilding = AddressOfBuilding;
                SelectedBuilding.ListOfEstimateMaterials = ListOfEstimateMaterials;
                SelectedBuilding = null;
            }

            ClearBuildingFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление Building
        private async void RemoveBuilding()
        {
            if (SelectedBuilding != null)
            {
                ListOfBuildings.Remove(SelectedBuilding);
                SelectedBuilding = null;

            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите объект!", "Ок");
        }

        // Убираем выделение с EstimateMaterial и очищаем поля
        private void AddNewEstimateMaterial()
        {
            SelectedMaterial = null;
            ClearEstimateMaterialFields();
        }

        // Подготовка к изменению EstimateMaterial
        private async void PreparingToChangeEstimateMaterial()
        {
            if (SelectedMaterial != null)
            {
                NameOfMaterial = SelectedMaterial.NameOfMaterial;
                Unit = SelectedMaterial.Unit;
                Quantity = SelectedMaterial.Quantity;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите материал!", "Ок");
        }

        // Сохранение EstimateMaterial
        private async void SaveEstimateMaterial()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(NameOfMaterial), "Выберите название материала!"),
                (string.IsNullOrEmpty(Unit), "Выберите единицы измерения!"),
                (Quantity <= 0, "Введите количество!"));

            if (hasErrors) return;

            bool flag = false;
            foreach (var material in ListOfEstimateMaterials)
            {
                if (material.NameOfMaterial == NameOfMaterial && material.Unit == Unit && material != SelectedMaterial)
                {
                    flag = true;
                    material.Quantity += Quantity;
                    if (SelectedMaterial != null)
                        RemoveEstimateMaterial();
                    break;
                }
            }
            if (SelectedMaterial == null)
            {
                if (!flag)
                    ListOfEstimateMaterials.Add(new EstimateMaterial(NameOfMaterial, Quantity, Unit));
            }
            else
            {
                if (!flag)
                {
                    SelectedMaterial.NameOfMaterial = NameOfMaterial;
                    SelectedMaterial.Unit = Unit;
                    SelectedMaterial.Quantity = Quantity;
                    SelectedMaterial = null;
                }
            }

            ClearEstimateMaterialFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление EstimateMaterial
        private async void RemoveEstimateMaterial()
        {
            if (SelectedMaterial != null)
            {
                ListOfEstimateMaterials.Remove(SelectedMaterial);
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
                if (cancelType == "Building")
                {
                    if (SelectedBuilding != null)
                    {
                        SelectedBuilding.ListOfEstimateMaterials = new ObservableCollection<EstimateMaterial>(originalBuilding.ListOfEstimateMaterials);
                        SelectedBuilding = null;
                        ClearBuildingFields();
                    }
                }
                else if (cancelType == "EstimateMaterial")
                {
                    SelectedMaterial = null;
                    ClearEstimateMaterialFields();
                }

                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Очистка полей у EstimateMaterial
        private void ClearEstimateMaterialFields()
        {
            NameOfMaterial = string.Empty;
            Unit = string.Empty;
            Quantity = 0;
        }

        // Очистка полей у Building
        private void ClearBuildingFields()
        {
            NameOfBuilding = string.Empty;
            AddressOfBuilding = string.Empty;
            ListOfEstimateMaterials = new();
        }
    }
}
