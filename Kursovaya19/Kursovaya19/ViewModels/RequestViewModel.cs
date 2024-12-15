using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class RequestViewModel : BaseViewModel
    {
        // Основной список ListOfRequests
        public ObservableCollection<Request> ListOfRequests { get; set; } = new();

        // Поля для Request
        public ObservableCollection<RequestMaterial> ListOfRequestMaterials { get; set; } = new();
        private DateTime dateTime = DateTime.Today;
        private Building building;

        // Поля для RequestMaterial
        private string nameOfMaterial;
        private int quantity;
        private string unit;

        // Доп. Поля для изменения
        private Request originalRegistration;
        private RequestMaterial selectedMaterial;
        private Request selectedRequest;

        // Доп. Свойства
        public ObservableCollection<Building> ListOfBuildings { get; set; } = new();
        public DateTime MaxDate { get; set; } = DateTime.Today.AddMonths(6);

        // Комаднды для Registration
        public ICommand AddNewRequestCommand { get; }
        public ICommand PreparingToChangeRequestCommand { get; }
        public ICommand SaveRequestCommand { get; }
        public ICommand RemoveRequestCommand { get; }

        // Комаднды для RegistrationMaterial
        public ICommand AddNewRequestMaterialCommand { get; }
        public ICommand PreparingToChangeRequestMaterialCommand { get; }
        public ICommand SaveRequestMaterialCommand { get; }
        public ICommand RemoveRequestMaterialCommand { get; }

        // Команда Отмены
        public ICommand CancelCommand { get; }

        // Конструктор
        public RequestViewModel()
        {
            // Комаднды для Registration
            AddNewRequestCommand = new Command(() => { AddNewRequest(); });
            PreparingToChangeRequestCommand = new Command(() => { PreparingToChangeRequest(); });
            SaveRequestCommand = new Command(() => { SaveRequest(); });
            RemoveRequestCommand = new Command(() => { RemoveRequest(); });

            // Комаднды для RegistrationMaterial
            AddNewRequestMaterialCommand = new Command(() => { AddNewRequestMaterial(); });
            PreparingToChangeRequestMaterialCommand = new Command(() => { PreparingToChangeRequestMaterial(); });
            SaveRequestMaterialCommand = new Command(() => { SaveRequestMaterial(); });
            RemoveRequestMaterialCommand = new Command(() => { RemoveRequestMaterial(); });

            // Команда для отмены
            CancelCommand = new Command<string>(Cancel);
        }

        // Свойства

        public Request SelectedRequest
        {
            get => selectedRequest;
            set
            {
                if (selectedRequest != value)
                {
                    selectedRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public RequestMaterial SelectedMaterial
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

        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                dateTime = value;
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

        // Убираем выделение с Request и очищаем поля
        private void AddNewRequest()
        {
            if (SelectedRequest != null)
            {
                SelectedRequest = null;
                ClearRequestFields();
            }
        }

        // Подготовка к изменению Request
        private async void PreparingToChangeRequest()
        {
            if (SelectedRequest != null)
            {
                originalRegistration = new Request(
                    SelectedRequest.Building,
                    SelectedRequest.DateTime,
                    new ObservableCollection<RequestMaterial>(SelectedRequest.ListOfRequestMaterials.Select(m => new RequestMaterial(m)))
                );

                Building = SelectedRequest.Building;
                DateTime = SelectedRequest.DateTime;
                ListOfRequestMaterials = new ObservableCollection<RequestMaterial>(SelectedRequest.ListOfRequestMaterials);
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите заявку", "Ок");
        }

        // Сохранение Request
        private async void SaveRequest()
        {
            var hasErrors = await HasValidationErrors(
                (Building == null, "Выберите строящийся объект!"),
                (ListOfRequestMaterials.Count < 1, "Добавьте материалы!"));

            if (hasErrors) return;

            if (SelectedRequest == null)
                ListOfRequests.Add(new Request(Building, DateTime, ListOfRequestMaterials));
            else
            {
                SelectedRequest.Building = Building;
                SelectedRequest.DateTime = DateTime;
                SelectedRequest.ListOfRequestMaterials = ListOfRequestMaterials;
                SelectedRequest = null;
            }
            
            ClearRequestFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление Request
        private async void RemoveRequest()
        {
            if (SelectedRequest != null)
            {
                ListOfRequests.Remove(SelectedRequest);
                SelectedRequest = null;

                await App.Current.MainPage.DisplayAlert("Уведомление", "Заявка удалена", "Ок");
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите заявку", "Ок");
        }

        // Убираем выделение с RequestMaterial и очищаем поля
        private void AddNewRequestMaterial()
        {
            if (SelectedMaterial != null)
            {
                SelectedMaterial = null;
                ClearMaterialFields();
            }
        }

        // Подготовка к изменению RequestMaterial
        private async void PreparingToChangeRequestMaterial()
        {
            if (SelectedMaterial != null)
            {
                NameOfMaterial = SelectedMaterial.NameOfMaterial;
                Unit = SelectedMaterial.Unit;
                Quantity = SelectedMaterial.Quantity;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите объект!", "Ок");
        }

        // Сохранение RequestMaterial
        private async void SaveRequestMaterial()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(NameOfMaterial), "Выберите материал!"),
                (string.IsNullOrEmpty(Unit), "Выберите единицы измерения!"),
                (Quantity < 1, "Введите количество!"));

            if (hasErrors) return;

            bool flag = false;
            foreach (var material in ListOfRequestMaterials)
            {
                if (material.NameOfMaterial == NameOfMaterial && material.Unit == Unit && material != SelectedMaterial)
                {
                    flag = true;
                    material.Quantity += Quantity;
                    if (SelectedMaterial != null)
                        RemoveRequestMaterial();
                    break;
                }
            }
            if (SelectedMaterial == null)
            {
                if (!flag)
                    ListOfRequestMaterials.Add(new RequestMaterial(NameOfMaterial, Quantity, Unit));
            }
            else
            {
                if (!flag)
                {
                    SelectedMaterial.NameOfMaterial = NameOfMaterial;
                    SelectedMaterial.Unit = Unit;
                    SelectedMaterial.Quantity = Quantity;
                }
                SelectedMaterial = null;
            }

            ClearMaterialFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление RequestMaterial
        private async void RemoveRequestMaterial()
        {
            if (SelectedMaterial != null)
            {
                ListOfRequestMaterials.Remove(SelectedMaterial);
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
                if (cancelType == "Request")
                {
                    if (SelectedRequest != null)
                    {
                        SelectedRequest.ListOfRequestMaterials = new ObservableCollection<RequestMaterial>(originalRegistration.ListOfRequestMaterials);
                        SelectedRequest = null;
                        ClearRequestFields();
                    }                        
                }
                else if (cancelType == "RequestMaterial")
                {
                    SelectedMaterial = null;
                    ClearMaterialFields();
                }

                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Очистка полей у Request
        private void ClearRequestFields()
        {
            Building = null;
            DateTime = DateTime.Today;
            ListOfRequestMaterials = new();
        }

        // Очистка полей у RequestMaterial
        private void ClearMaterialFields()
        {
            NameOfMaterial = string.Empty;
            Unit = string.Empty;
            Quantity = 0;
        }
    }
}
