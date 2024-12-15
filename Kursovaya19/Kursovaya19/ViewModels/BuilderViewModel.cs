using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class BuilderViewModel : BaseViewModel
    {
        // Основной список ListOfBuilders
        public ObservableCollection<Builder> ListOfBuilders { get; set; } = new();

        // Поля для Builder
        public ObservableCollection<string> ListOfSpecialities { get; set; } = new();
        private string fullName;
        private string gender;
        private DateTime birthday = DateTime.Today.AddYears(-18);
        private string residenceAddress;
        private int workExperience;

        // Доп. Поля для изменения
        private Builder originalBuilder;
        private Builder selectedBuilder;
        private string selectedSpeciality;
        private string speciality;

        // Доп. Свойства
        public ObservableCollection<string> ListOfGenders { get; private set; } = new ObservableCollection<string> { "м", "ж" };
        public ObservableCollection<int> ListOfWorkExperiences { get; set; } = new ObservableCollection<int> { 0 };
        public DateTime MinDate { get; set; } = DateTime.Today.AddYears(-80);
        public DateTime MaxDate { get; set; } = DateTime.Today.AddYears(-18);

        // Комаднды для Builder
        public ICommand AddNewBuilderCommand { get; }
        public ICommand PreparingToChangeBuilderCommand { get; }
        public ICommand SaveBuilderCommand { get; }
        public ICommand RemoveBuilderCommand { get; }

        // Комаднды для Speciality
        public ICommand AddSpecialityCommand { get; }
        public ICommand ChangeSpecialityCommand { get; }
        public ICommand RemoveSpecialityCommand { get; }

        // Команда Отмены
        public ICommand CancelCommand { get; }

        // Конструктор
        public BuilderViewModel()
        {
            // Комаднды для Builder
            AddNewBuilderCommand = new Command(() => { AddNewBuilder(); });
            PreparingToChangeBuilderCommand = new Command(PreparingToChangeBuilder);
            SaveBuilderCommand = new Command(SaveBuilder);
            RemoveBuilderCommand = new Command(RemoveBuilder);

            // Комаднды для Speciality
            AddSpecialityCommand = new Command(AddSpeciality);
            ChangeSpecialityCommand = new Command(ChangeSpeciality);
            RemoveSpecialityCommand = new Command(RemoveSpeciality);

            // Команда отмены
            CancelCommand = new Command<string>(Cancel);
        }

        // Свойства
        public Builder SelectedBuilder
        {
            get => selectedBuilder;
            set
            {
                if (selectedBuilder != value)
                {
                    selectedBuilder = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedSpeciality
        {
            get => selectedSpeciality;
            set
            {
                if (selectedSpeciality != value)
                {
                    selectedSpeciality = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Speciality
        {
            get => speciality;
            set
            {
                speciality = value;
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

        public string Gender
        {
            get => gender;
            set
            {
                gender = value;
                OnPropertyChanged();
            }
        }

        public DateTime Birthday
        {
            get => birthday;
            set
            {
                birthday = value;
                OnPropertyChanged();
                UpdateWorkExperienceList();
            }
        }

        public string ResidenceAddress
        {
            get => residenceAddress;
            set
            {
                residenceAddress = value;
                OnPropertyChanged();
            }
        }

        public int WorkExperience
        {
            get => workExperience;
            set
            {
                workExperience = value;
                OnPropertyChanged();
            }
        }

        public int MaxWorkExperience
        {
            get
            {
                var age = DateTime.Today.Year - Birthday.Year;
                if (DateTime.Today.Month < Birthday.Month || (DateTime.Today.Month == Birthday.Month && DateTime.Today.Day < Birthday.Day))
                {
                    age--;
                }
                return age - 18;
            }
        }

        // Убираем выделение с Builder и очищаем поля
        private void AddNewBuilder()
        {
            if (SelectedBuilder != null)
            {
                SelectedBuilder = null;
                ClearBuilderFields();
            }
        }

        // Подготовка к изменению Builder
        private async void PreparingToChangeBuilder()
        {
            if (SelectedBuilder != null)
            {
                originalBuilder = new Builder(
                    SelectedBuilder.FullName,
                    SelectedBuilder.Gender,
                    SelectedBuilder.Birthday,
                    SelectedBuilder.ResidenceAddress,
                    SelectedBuilder.WorkExperience,
                    new ObservableCollection<string>(SelectedBuilder.ListOfSpecialities.Select(m => new string(m)))
                );

                FullName = SelectedBuilder.FullName;
                Gender = SelectedBuilder.Gender;
                Birthday = SelectedBuilder.Birthday;
                ResidenceAddress = SelectedBuilder.ResidenceAddress;
                WorkExperience = SelectedBuilder.WorkExperience;
                ListOfSpecialities = new ObservableCollection<string>(SelectedBuilder.ListOfSpecialities);
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите строителя", "Ок");
        }

        // Сохранение Builder
        private async void SaveBuilder()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(FullName), "Введите ФИО строителя!"),
                (string.IsNullOrEmpty(Gender), "Выберите пол строителя!"),
                (string.IsNullOrEmpty(ResidenceAddress), "Введите адрес проживания!"),
                (ListOfSpecialities.Count == 0, "Введите хотя бы одну специальность!"));

            if (hasErrors) return;

            if (SelectedBuilder == null)
                ListOfBuilders.Add(new Builder(FullName, Gender, Birthday, ResidenceAddress, WorkExperience, ListOfSpecialities));
            else
            {
                SelectedBuilder.FullName = FullName;
                SelectedBuilder.Gender = Gender;
                SelectedBuilder.Birthday = Birthday;
                SelectedBuilder.ResidenceAddress = ResidenceAddress;
                SelectedBuilder.WorkExperience = WorkExperience;
                SelectedBuilder.ListOfSpecialities = ListOfSpecialities;
                SelectedBuilder = null;
            }
            
            ClearBuilderFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление Builder
        private async void RemoveBuilder()
        {
            if (SelectedBuilder != null)
            {
                ListOfBuilders.Remove(SelectedBuilder);
                SelectedBuilder = null;

                await App.Current.MainPage.DisplayAlert("Уведомление", "Строитель удалён", "Ок");
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите строителя", "Ок");
        }

        // Добавление Speciality
        private async void AddSpeciality()
        {
            if (!string.IsNullOrEmpty(Speciality))
            {
                var check = IsValidSpecialityName(Speciality);

                if (check)
                {
                    var newSpeciality = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Speciality.ToLower());

                    if (!ListOfSpecialities.Contains(newSpeciality))
                    {
                        ListOfSpecialities.Add(newSpeciality);
                        SortSpecialities();
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Уведомление", "Такая специальность уже есть в списке!", "Ок");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Уведомление", "Некорректные данные!", "Ок");
            }

            Speciality = string.Empty;
        }

        // Изменение Speciality
        private async void ChangeSpeciality()
        {
            if (SelectedSpeciality != null)
            {
                var result = await Application.Current.MainPage.DisplayPromptAsync("Название специальности", "Введите новое название специальности:", "Ок", "Отмена");

                if (!string.IsNullOrEmpty(result))
                {
                    var check = IsValidSpecialityName(result);

                    if (check)
                    {
                        var changedSpeciality = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());

                        if (!ListOfSpecialities.Contains(changedSpeciality))
                        {
                            var index = ListOfSpecialities.IndexOf(SelectedSpeciality);

                            ListOfSpecialities[index] = changedSpeciality;
                            SortSpecialities();
                        }
                        else
                            await Application.Current.MainPage.DisplayAlert("Уведомление", "Такая специальность уже есть в списке!", "Ок");
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Уведомление", "Некорректные данные!", "Ок");
                }
                SelectedSpeciality = null;
            }
            else
                await Application.Current.MainPage.DisplayAlert("Уведомление", "Выберите специальность!", "Ок");
        }

        // Удаление Speciality
        private void RemoveSpeciality()
        {
            if (SelectedSpeciality != null)
            {
                ListOfSpecialities.Remove(SelectedSpeciality);
                SelectedSpeciality = null;
            }
        }

        // Отмена
        private async void Cancel(string parameter)
        {
            if (parameter is string cancelType)
            {
                if (cancelType == "Builder")
                {
                    if (SelectedBuilder != null)
                    {
                        SelectedBuilder.ListOfSpecialities = originalBuilder.ListOfSpecialities;
                        SelectedBuilder = null;
                        SelectedSpeciality = null;
                        Speciality = string.Empty;
                        ClearBuilderFields();
                    }
                }                   

                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Очистка полей у Builder
        private void ClearBuilderFields()
        {
            FullName = string.Empty;
            Gender = string.Empty;
            Birthday = MaxDate;
            ResidenceAddress = string.Empty;
            WorkExperience = 0;
            ListOfSpecialities = new();
        }

        // Метод для подсчёта возможного стажа работы
        private void UpdateWorkExperienceList()
        {
            ListOfWorkExperiences.Clear();
            for (int i = 0; i <= MaxWorkExperience; i++)
                ListOfWorkExperiences.Add(i);
        }

        // Метод для отсортировки специальностей
        private void SortSpecialities()
        {
            var sortedList = ListOfSpecialities.OrderBy(word => word).ToList();
            ListOfSpecialities.Clear();
            foreach (var speciality in sortedList)
                ListOfSpecialities.Add(speciality);
        }

        // Проверка на корректность введённых данных
        private bool IsValidSpecialityName(string SpecialityName)
        {
            return Regex.IsMatch(SpecialityName, @"^[a-zA-Zа-яА-Я \-\""'()«»]*$");
        }
    }
}
