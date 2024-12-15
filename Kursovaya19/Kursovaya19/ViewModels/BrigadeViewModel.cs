using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Windows.Input;
using Kursovaya19.Classes;

namespace Kursovaya19.ViewModels
{
    public class BrigadeViewModel : BaseViewModel
    {
        // Основной список ListOfBrigades
        public ObservableCollection<Brigade> ListOfBrigades { get; set; } = new();

        // Поля Бригады
        public ObservableCollection<Builder> Builders { get; set; } = new();
        private string nameOfBrigade;
        private Builder brigadier;

        // Доп. Поля для изменения
        private Brigade selectedBrigade;
        private Builder selectedWorker;
        private Builder selectedBuilder;

        // Доп. Свойства
        public ObservableCollection<Builder> AllBuilders { get; set; } = new();
        public ObservableCollection<Builder> ListOfFreeBuilders { get; set; } = new();
        public ObservableCollection<Builder> ListOfBusyBuilders { get; set; } = new();

        // Снимки состояния списков для отката изменений
        private List<Builder> originalFreeBuilders = new();
        private List<Builder> originalBusyBuilders = new();

        // Команды для Brigade
        public ICommand AddNewBrigadeCommand { get; }
        public ICommand PreparingToChangeBrigadeCommand { get; }
        public ICommand SaveBrigadeCommand { get; }
        public ICommand RemoveBrigadeCommand { get; }

        // Комаднды для Brigadier
        public ICommand AddBrigadierCommand { get; }
        public ICommand AddBuilderCommand { get; }
        public ICommand RemoveBuilderCommand { get; }

        // Комаднда Отмены
        public ICommand CancelCommand { get; }

        public BrigadeViewModel()
        {
            // Команды для Brigade
            AddNewBrigadeCommand = new Command(() => { AddNewBrigade(); });
            PreparingToChangeBrigadeCommand = new Command(() => { PreparingToChangeBrigade(); });
            SaveBrigadeCommand = new Command(() => { SaveBrigade(); });
            RemoveBrigadeCommand = new Command(() => { RemoveBrigade(); });

            // Комаднды для Brigadier
            AddBrigadierCommand = new Command(() => { AddBrigadier(); });
            AddBuilderCommand = new Command(() => { AddBuilder(); });
            RemoveBuilderCommand = new Command(() => { RemoveBuilder(); });

            // Комаднда Отмены
            CancelCommand = new Command(Cancel);
        }

        // Свойства
        public Brigade SelectedBrigade
        {
            get => selectedBrigade;
            set
            {
                selectedBrigade = value;
                OnPropertyChanged();
            }
        }

        public Builder SelectedWorker
        {
            get => selectedWorker;
            set
            {
                if (value != selectedWorker)
                {
                    selectedWorker = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public string NameOfBrigade
        {
            get => nameOfBrigade;
            set
            {
                nameOfBrigade = value;
                OnPropertyChanged();
            }
        }

        public Builder Brigadier
        {
            get => brigadier;
            set
            {
                if (brigadier != value)
                {
                    brigadier = value;
                    OnPropertyChanged();
                }
            }
        }

        // Убираем выделение с Brigade и очищаем поля
        private void AddNewBrigade()
        {
            if (SelectedBrigade != null)
            {
                SelectedBrigade = null;
                ClearBrigadeFields();
            }
            // Сохраняем снимок состояния списков
            originalFreeBuilders = new List<Builder>(ListOfFreeBuilders);
            originalBusyBuilders = new List<Builder>(ListOfBusyBuilders);
            UpdateBuilderLists();
        }

        // Подготовка к изменению Brigade
        private async void PreparingToChangeBrigade()
        {
            if (SelectedBrigade != null)
            {
                NameOfBrigade = SelectedBrigade.NameOfBrigade;
                Brigadier = SelectedBrigade.Brigadier;
                Builders = new ObservableCollection<Builder>(SelectedBrigade.Builders);

                // Сохраняем снимок состояния списков
                originalFreeBuilders = new List<Builder>(ListOfFreeBuilders);
                originalBusyBuilders = new List<Builder>(ListOfBusyBuilders);
                UpdateBuilderLists();
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите бригаду", "Ок");
        }

        // Сохранение Brigade
        private async void SaveBrigade()
        {
            var hasErrors = await HasValidationErrors(
                (string.IsNullOrEmpty(NameOfBrigade), "Введите название бригады!"),
                (Brigadier == null, "Выберите бригадира"),
                (Builders.Count == 0, "Добавьте строителей в список!"));

            if (hasErrors) return;

            if (SelectedBrigade == null)
                ListOfBrigades.Add(new Brigade(NameOfBrigade, Brigadier, Builders));
            else
            {
                SelectedBrigade.NameOfBrigade = NameOfBrigade;
                SelectedBrigade.Brigadier = Brigadier;
                SelectedBrigade.Builders = Builders;
                SelectedBrigade = null;
            }

            ClearBrigadeFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Удаление Brigade
        private async void RemoveBrigade()
        {
            if (SelectedBrigade != null)
            {
                foreach (var builder in SelectedBrigade.Builders)
                {
                    ListOfFreeBuilders.Add(builder);
                    ListOfBusyBuilders.Remove(builder);
                }
                ListOfFreeBuilders.Add(SelectedBrigade.Brigadier);
                ListOfBusyBuilders.Remove(SelectedBrigade.Brigadier);

                ListOfBrigades.Remove(SelectedBrigade);
                SelectedBrigade = null;

                await App.Current.MainPage.DisplayAlert("Уведомление", "Бригада удалена", "Ок");
            }
            else await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите бригаду", "Ок");
        }

        // Добавление Brigadier
        private async void AddBrigadier()
        {
            if (SelectedWorker != null)
            {
                if (Brigadier != null)
                {
                    if (SelectedWorker == Brigadier)
                        await App.Current.MainPage.DisplayAlert("Уведомление", "Этот работник уже является бригадиром", "Ок");
                    else
                    {
                        var result = await App.Current.MainPage.DisplayAlert(
                            "Уведомление",
                            $"Вы точно хотите заменить бригадира?\n\n" +
                            $"🛠️ **Текущий бригадир**\n" +
                            $"👤 ФИО: {Brigadier.FullName}\n" +
                            $"🎂 Дата рождения: {Brigadier.Birthday:dd.MM.yyyy}\n\n" +
                            $"➡️ **Новый бригадир**\n" +
                            $"👤 ФИО: {SelectedWorker.FullName}\n" +
                            $"🎂 Дата рождения: {SelectedWorker.Birthday:dd.MM.yyyy}\n\n" +
                            $"❗ Подтвердите замену бригадира.",
                            "Ок",
                            "Отмена"
                        );

                        if (result)
                        {
                            ListOfFreeBuilders.Add(Brigadier);
                            Brigadier = SelectedWorker;
                            ListOfBusyBuilders.Add(SelectedWorker);
                            ListOfFreeBuilders.Remove(SelectedWorker);
                        }
                    }
                }
                else
                {
                    Brigadier = SelectedWorker;
                    ListOfBusyBuilders.Add(SelectedWorker);
                    ListOfFreeBuilders.Remove(SelectedWorker);
                }
                SelectedWorker = null;
            }
            else await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите бригадира!", "Ок");
        }

        // Добавление Builder
        private async void AddBuilder()
        {
            if (SelectedWorker != null)
            {
                if (SelectedWorker == Brigadier) await App.Current.MainPage.DisplayAlert("Уведомление", "Бригадир не может быть в составе строителей!", "Ок");
                else if (!Builders.Contains(SelectedWorker))
                {
                    Builders.Add(SelectedWorker);
                    ListOfBusyBuilders.Add(SelectedWorker);
                    ListOfFreeBuilders.Remove(SelectedWorker);
                }

                else await App.Current.MainPage.DisplayAlert("Уведомление", "Этот строитель уже есть в составе!", "Ок");

                SelectedWorker = null;
            }
            else await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите строителя!", "Ок");
        }

        // Удаление Builder
        private async void RemoveBuilder()
        {
            if (SelectedBuilder != null)
            {
                Builders.Remove(SelectedBuilder);
                ListOfFreeBuilders.Add(SelectedBuilder);
                ListOfBusyBuilders.Remove(SelectedBuilder);
                SelectedBuilder = null;
            }
            else
                await App.Current.MainPage.DisplayAlert("Уведомление", "Выберите строителя!", "Ок");
        }

        // Отмена
        private async void Cancel()
        {
            if (SelectedBrigade != null)
                SelectedBrigade = null;

            // Восстанавливаем списки строителей
            ListOfFreeBuilders.Clear();
            foreach (var builder in originalFreeBuilders)
                ListOfFreeBuilders.Add(builder);

            ListOfBusyBuilders.Clear();
            foreach (var builder in originalBusyBuilders)
                ListOfBusyBuilders.Add(builder);

            ClearBrigadeFields();

            await App.Current.MainPage.Navigation.PopAsync();
        }

        // Очитска полей у Бригады
        private void ClearBrigadeFields()
        {
            NameOfBrigade = string.Empty;
            Brigadier = null;
            Builders = new();
        }

        // Одновление Списков ListOfBusyBuilders и ListOfFreeBuilders
        private void UpdateBuilderLists()
        {
            var busyBuilders = ListOfBrigades.SelectMany(b => b.Builders).ToList();
            foreach(var brigadier in ListOfBrigades.Select(b => b.Brigadier).ToList())
                busyBuilders.Add(brigadier);
            var allBuilders = AllBuilders;

            ListOfFreeBuilders.Clear();
            ListOfBusyBuilders.Clear();

            foreach (var builder in allBuilders)
            {
                if (busyBuilders.Any(b => b.FullName == builder.FullName && b.Birthday == builder.Birthday && b.Gender == builder.Gender && b.ResidenceAddress == builder.ResidenceAddress && b.WorkExperience == builder.WorkExperience))
                    ListOfBusyBuilders.Add(builder);
                else
                    ListOfFreeBuilders.Add(builder);
            }
        }
    }
}
