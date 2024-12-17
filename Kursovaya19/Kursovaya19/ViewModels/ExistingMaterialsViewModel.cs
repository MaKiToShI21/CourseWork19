using Kursovaya19.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Kursovaya19.ViewModels
{
    public class ExistingMaterialsViewModel : BaseViewModel
    {
        // Основной список ListOfExistingMaterials
        public ObservableCollection<string> ListOfExistingMaterials { get; set; } = new();

        // Поля для ExistingMaterial
        string namesOfExistingMaterials;

        // Доп. Поля для изменения
        string selectedExistingMaterial;
        string[] enteredListOfExistingMaterials;

        // Доп. Свойство
        public ObservableCollection<string> ListOfUnits { get; private set; } = new ObservableCollection<string> { "кг", "т", "м³", "м²", "м", "л", "шт." };

        // Команды для ExistingMaterial
        public ICommand AddEnteredListOfExistingMaterialsCommand { get; }
        public ICommand ChangeExistingMaterialCommand { get; }
        public ICommand RemoveExistingMaterialCommand { get; }

        // Команда Отмены
        public ICommand CancelCommand { get; }

        public ExistingMaterialsViewModel()
        {
            // Команды для ExistingMaterial
            AddEnteredListOfExistingMaterialsCommand = new Command(() => { AddEnteredListOfExistingMaterials(); });
            ChangeExistingMaterialCommand = new Command(() => { ChangeExistingMaterial(); });
            RemoveExistingMaterialCommand = new Command(() => { RemoveExistingMaterial(); });

            // Команда Отмены
            CancelCommand = new Command<string>(Cancel);
        }

        // Свойства
        public string SelectedExistingMaterial
        {
            get => selectedExistingMaterial;
            set
            {
                if (selectedExistingMaterial != value)
                {
                    selectedExistingMaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] EnteredListOfExistingMaterials
        {
            get => enteredListOfExistingMaterials;
            set
            {
                enteredListOfExistingMaterials = value;
                OnPropertyChanged();
            }
        }

        public string NamesOfExistingMaterials
        {
            get => namesOfExistingMaterials;
            set
            {
                namesOfExistingMaterials = value;
                OnPropertyChanged();
            }
        }

        // Добавление введённых материалов в ListOfExistingMaterials
        private async void AddEnteredListOfExistingMaterials()
        {
            if (NamesOfExistingMaterials != null)
            {
                char[] delimiters = new char[] { '-', '.', ',', '(', ')', '{', '}', '@', '#', '$', '%', '^', '<', '>', '?', '&', '*', '!', '+', '=', ':', ';', '\'', '\"', '/', '\\', '[', ']', '+' };
                var result = NamesOfExistingMaterials.Replace("\r\n", ", ").Replace("\n", ", ").Replace("\r", ", ");

                EnteredListOfExistingMaterials = result
                    .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.Trim())
                    .Where(word => !string.IsNullOrEmpty(word) && word.Length <= 50)  // Игнорируем пустые и слишком длинные строки
                    .Select(word => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower())) // Заглавные буквы
                    .OrderBy(word => word, StringComparer.OrdinalIgnoreCase)  // Сортировка без учета регистра
                    .ToArray();

                foreach (var existingMaterial in EnteredListOfExistingMaterials)
                    if (!string.IsNullOrEmpty(existingMaterial) && existingMaterial.Length <= 50 && !ListOfExistingMaterials.Contains(existingMaterial))
                        ListOfExistingMaterials.Add(existingMaterial);

                var sortedList = ListOfExistingMaterials.OrderBy(word => word, StringComparer.OrdinalIgnoreCase).ToList();

                ListOfExistingMaterials.Clear();
                foreach (var material in sortedList)
                    ListOfExistingMaterials.Add(material);

                EnteredListOfExistingMaterials = Array.Empty<string>();
                NamesOfExistingMaterials = string.Empty;

                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Изменение ExistingMaterial
        private async void ChangeExistingMaterial()
        {
            if (SelectedExistingMaterial != null)
            {
                var result = await Application.Current.MainPage.DisplayPromptAsync("Название материала", "Введите новое название материала:", "Ок", "Отмена", initialValue: SelectedExistingMaterial);

                if (!string.IsNullOrEmpty(result))
                {
                    var check = IsValidMaterialName(result);

                    if (check)
                    {
                        var changedMaterialName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());

                        if (!ListOfExistingMaterials.Contains(changedMaterialName))
                        {
                            var index = ListOfExistingMaterials.IndexOf(SelectedExistingMaterial);

                            ListOfExistingMaterials[index] = changedMaterialName;

                            var sortedList = ListOfExistingMaterials.OrderBy(word => word).ToList();

                            ListOfExistingMaterials.Clear();
                            foreach (var material in sortedList)
                                ListOfExistingMaterials.Add(material);

                            await Application.Current.MainPage.DisplayAlert("Уведомление", "Материал успешно изменён!", "Ок");
                        }
                        else
                            await Application.Current.MainPage.DisplayAlert("Уведомление", "Такой материал уже есть в списке!", "Ок");
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Уведомление", "Некорректные данные!", "Ок");
                }
                SelectedExistingMaterial = null;
            }
            else
                await Application.Current.MainPage.DisplayAlert("Уведомление", "Выберите материал!", "Ок");
        }

        // Удаление ExistingMaterial
        private async void RemoveExistingMaterial()
        {
            if (SelectedExistingMaterial != null)
            {
                ListOfExistingMaterials.Remove(SelectedExistingMaterial);
                SelectedExistingMaterial = null;

                await App.Current.MainPage.DisplayAlert("Уведомление", "Материал успешно удалён!", "Ок");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите материал!", "Ок");
            }
        }

        // Отмена
        private async void Cancel(string parameter)
        {
            if (parameter is string cancelType)
            {
                if (cancelType == "ExistingMaterial")
                {
                    NamesOfExistingMaterials = string.Empty;
                }

                await App.Current.MainPage.Navigation.PopAsync();
            }
        }

        // Проверка на корректность введённых данных
        private bool IsValidMaterialName(string materialName)
        {
            return Regex.IsMatch(materialName, @"^[a-zA-Zа-яА-Я0-9 ]*$");
        }
    }
}
