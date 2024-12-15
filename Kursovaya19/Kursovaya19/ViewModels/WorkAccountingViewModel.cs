using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class WorkAccountingViewModel : BaseViewModel
    {
        // Основной список
        public ObservableCollection<WorkAccounting> ListOfWorksAccounting { get; set; } = new();

        // Поля для WorkAccounting
        public ObservableCollection<CompletedWork> ListOfCompletedWorks { get; set; } = new();
        private DateTime dateTime = DateTime.Today;
        private string nameOfBuilding;
        private string nameOfBrigade;

        // Поля для CompletedWork
        private string typeOfWork;
        private double percentOfCompletedWork;
        
        // Доп. Поля для изменения
        private WorkAccounting selectedWorkAccounting;
        private WorkAccounting originalWorkAccounting;
        private CompletedWork selectedCompletedWork;

        // Доп. Свойства
        public DateTime MinDate { get; set; } = DateTime.Today.AddYears(-1);
        public DateTime MaxDate { get; set; } = DateTime.Today.AddYears(10);

        // Команды для WorkAccounting
        public ICommand AddNewWorkAccountingCommand { get; }
        public ICommand PreparingToChangeWorkAccountingCommand { get; }
        public ICommand SaveWorkAccountingCommand { get; }
        public ICommand RemoveWorkAccountingCommand { get; }

        // Команды для CompletedWork
        public ICommand AddNewCompletedWorkCommand { get; }
        public ICommand ChangeCompletedWorkCommand { get; }
        public ICommand RemoveCompletedWorkCommand { get; }
        
        // Команда для кнопки отмена/назад
        public ICommand CancelCommand { get; }

        public WorkAccountingViewModel()
        {
            // Инициализация команд для WorkAccounting
            AddNewWorkAccountingCommand = new Command(() => { AddNewWorkAccounting(); });
            PreparingToChangeWorkAccountingCommand = new Command(() => { PreparingToChangeWorkAccounting(); });
            SaveWorkAccountingCommand = new Command(() => { SaveWorkAccounting(); });
            RemoveWorkAccountingCommand = new Command(() => { RemoveWorkAccounting(); });

            // Инициализация команд для CompletedWork
            AddNewCompletedWorkCommand = new Command(() => { AddNewCompletedWork(); });
            ChangeCompletedWorkCommand = new Command(() => { ChangeCompletedWork(); });
            RemoveCompletedWorkCommand = new Command(() => { RemoveCompletedWork(); });

            // Инициализация команд для кнопки отмена/назад
            CancelCommand = new Command(Cancel);
        }

        // Свойства
        public WorkAccounting SelectedWorkAccounting
        {
            get => selectedWorkAccounting;
            set
            {
                if (selectedWorkAccounting != value)
                {
                    selectedWorkAccounting = value;
                    OnPropertyChanged();
                }
            }
        }

        public CompletedWork SelectedCompletedWork
        {
            get => selectedCompletedWork;
            set
            {
                if (selectedCompletedWork != value)
                {
                    selectedCompletedWork = value;
                    OnPropertyChanged();

                    if (selectedCompletedWork != null)
                    {
                        TypeOfWork = selectedCompletedWork.TypeOfWork;
                        PercentOfCompletedWork = selectedCompletedWork.PercentOfCompletedWork;
                    }
                    else
                        ClearCompletedWorkFields();
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

        public string NameOfBuilding
        {
            get => nameOfBuilding;
            set
            {
                nameOfBuilding = value;
                OnPropertyChanged();
            }
        }

        public string NameOfBrigade
        {
            get => nameOfBrigade;
            set
            {
                nameOfBrigade = value;
                OnPropertyChanged();
            }
        }

        public string TypeOfWork
        {
            get => typeOfWork;
            set
            {
                typeOfWork = value;

                if (SelectedCompletedWork != null)
                    SelectedCompletedWork.TypeOfWork = typeOfWork;

                OnPropertyChanged();
            }
        }

        public double PercentOfCompletedWork
        {
            get => percentOfCompletedWork;
            set
            {
                percentOfCompletedWork = value;

                if (SelectedCompletedWork != null)
                    SelectedCompletedWork.PercentOfCompletedWork = percentOfCompletedWork;
                OnPropertyChanged();
            }
        }

        // Убираем выделение с WorkAccounting и очищаем поля
        private void AddNewWorkAccounting()
        {
            if (SelectedWorkAccounting != null)
            {
                SelectedWorkAccounting = null;
                ClearWorkAccountingFields();
            }
        }

        // Подготовка к изменению WorkAccounting
        private async void PreparingToChangeWorkAccounting()
        {
            if (SelectedWorkAccounting != null)
            {
                originalWorkAccounting = new WorkAccounting(
                    SelectedWorkAccounting.DateTime,
                    SelectedWorkAccounting.NameOfBuilding,
                    SelectedWorkAccounting.NameOfBrigade,
                    new ObservableCollection<CompletedWork>(SelectedWorkAccounting.ListOfCompletedWorks.Select(m => new CompletedWork(m)))
                );

                DateTime = SelectedWorkAccounting.DateTime;
                NameOfBuilding = SelectedWorkAccounting.NameOfBuilding;
                NameOfBrigade = SelectedWorkAccounting.NameOfBrigade;
                ListOfCompletedWorks = new ObservableCollection<CompletedWork>(SelectedWorkAccounting.ListOfCompletedWorks);
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите учёт выполняемых работ!", "Ок");
        }

        // Сохранение WorkAccounting
        private async void SaveWorkAccounting()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(NameOfBuilding), "Выберите название строящегося объекта!"),
                (string.IsNullOrEmpty(NameOfBrigade), "Выберите название бригады!"),
                (ListOfCompletedWorks.Count < 1, "Добавьте перечень выполненных работ!"));

            if (hasErrors) return;

            if (SelectedWorkAccounting == null)
                ListOfWorksAccounting.Add(new WorkAccounting(DateTime, NameOfBuilding, NameOfBrigade, new ObservableCollection<CompletedWork>(ListOfCompletedWorks)));
            else
            {
                SelectedWorkAccounting.DateTime = DateTime;
                SelectedWorkAccounting.NameOfBuilding = NameOfBuilding;
                SelectedWorkAccounting.NameOfBrigade = NameOfBrigade;
                SelectedWorkAccounting.ListOfCompletedWorks = ListOfCompletedWorks;
                SelectedWorkAccounting = null;
            }
            
            ClearWorkAccountingFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление WorkAccounting
        private async void RemoveWorkAccounting()
        {
            if (SelectedWorkAccounting != null)
            {
                ListOfWorksAccounting.Remove(SelectedWorkAccounting);
                SelectedWorkAccounting = null;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите учёт выполняемых работ!", "Ок");
        }

        // Добавление CompletedWork
        private async void AddNewCompletedWork()
        {
            if (SelectedCompletedWork != null)
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Чтобы изменить перечень выполненных работ нажмите соответствующую кнопку!", "Ок");
                return;
            }

            if (string.IsNullOrEmpty(TypeOfWork))
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Укажите вид работы!", "Ок");
                return;
            }

            var check = IsValidTypeOfWorkName(TypeOfWork);

            if (check)
            {
                var newTypeOfWork = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TypeOfWork.ToLower());

                if (!ListOfCompletedWorks.Any(work => work.TypeOfWork == newTypeOfWork))
                {
                    ListOfCompletedWorks.Add(new CompletedWork(newTypeOfWork, Math.Round(PercentOfCompletedWork, 2)));
                    ClearCompletedWorkFields();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Уведомление", "Такой вид работы уже есть в списке!", "Ок");
            }
            else
                await Application.Current.MainPage.DisplayAlert("Уведомление", "Некорректные данные вида работы!", "Ок");
        }

        // Изменение CompletedWork
        private async void ChangeCompletedWork()
        {
            if (SelectedCompletedWork != null)
            {
                if (string.IsNullOrEmpty(TypeOfWork))
                {
                    await App.Current.MainPage.DisplayAlert("Уведомление", "Укажите вид работы!", "Ок");
                    return;
                }

                var check = IsValidTypeOfWorkName(TypeOfWork);

                if (check)
                {
                    var newTypeOfWork = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TypeOfWork.ToLower());

                    int index = ListOfCompletedWorks.IndexOf(SelectedCompletedWork);

                    SelectedCompletedWork.TypeOfWork = TypeOfWork;
                    SelectedCompletedWork.PercentOfCompletedWork = PercentOfCompletedWork;

                    ListOfCompletedWorks[index] = new CompletedWork(SelectedCompletedWork.TypeOfWork, Math.Round(SelectedCompletedWork.PercentOfCompletedWork, 2));

                    SelectedCompletedWork = null;
                    ClearCompletedWorkFields();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Уведомление", "Некорректные данные вида работы!", "Ок");
            }
        }

        // Удаление CompletedWork
        private async void RemoveCompletedWork()
        {
            if (SelectedCompletedWork != null)
            {
                ListOfCompletedWorks.Remove(SelectedCompletedWork);
                SelectedCompletedWork = null;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите перечень выполненных работ!", "Ок");
        }

        // Отмена
        private async void Cancel()
        {
            if (SelectedWorkAccounting != null)
            {
                SelectedWorkAccounting.ListOfCompletedWorks = new ObservableCollection<CompletedWork>(originalWorkAccounting.ListOfCompletedWorks);
                SelectedWorkAccounting = null;
            }
            ClearWorkAccountingFields();
            ClearCompletedWorkFields();
            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Очистка полей у WorkAccounting
        private void ClearWorkAccountingFields()
        {
            DateTime = DateTime.Today;
            NameOfBuilding = string.Empty;
            NameOfBrigade = string.Empty;
            ListOfCompletedWorks = new();
        }

        // Добавление полей у CompletedWork
        private void ClearCompletedWorkFields()
        {
            TypeOfWork = string.Empty;
            PercentOfCompletedWork = 0;
        }

        // Проверка на корректность введённых данных
        private bool IsValidTypeOfWorkName(string TypeOfWork)
        {
            return Regex.IsMatch(TypeOfWork, @"^[a-zA-Zа-яА-Я0-9 \-\""'()«»]*$");
        }
    }
}
