using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace Kursovaya19.Classes
{
    public class DataStorage
    {
        public List<Brigade> SavedListOfBrigades { get; set; }
        public List<Builder> SavedListOfBuilders { get; set; }
        public List<Building> SavedListOfBuildings { get; set; }
        public List<string> SavedListOfExistingMaterials { get; set; }
        public List<Registration> SavedListOfRegistrations { get; set; }
        public List<Request> SavedListOfRequests { get; set; }
        public List<Supplier> SavedListOfSuppliers{ get; set; }
        public List<WorkAccounting> SavedListOfWorksAccounting { get; set; }
        public List<Builder> SavedListOfFreeBuilders { get; set; }
        public List<Builder> SavedListOfBusyBuilders { get; set; }
    }
        
    public class SaveAndLoadData
    {
        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataStorage.json");

        public static async void SaveMainListsFromViewModels()
        {
            try
            {
                var dataToSave = new DataStorage
                {
                    SavedListOfBrigades = new List<Brigade>(MainPage.BrigadeViewModel.ListOfBrigades),
                    SavedListOfBuilders = new List<Builder>(MainPage.BuilderViewModel.ListOfBuilders),
                    SavedListOfBuildings = new List<Building>(MainPage.BuildingsViewModel.ListOfBuildings),
                    SavedListOfExistingMaterials = new List<string>(MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials),
                    SavedListOfRegistrations = new List<Registration>(MainPage.RegistrationViewModel.ListOfRegistrations),
                    SavedListOfRequests = new List<Request>(MainPage.RequestViewModel.ListOfRequests),
                    SavedListOfSuppliers = new List<Supplier>(MainPage.SupplierViewModel.ListOfSuppliers),
                    SavedListOfWorksAccounting = new List<WorkAccounting>(MainPage.WorkAccountingViewModel.ListOfWorksAccounting),
                    SavedListOfFreeBuilders = new List<Builder>(MainPage.BrigadeViewModel.ListOfFreeBuilders),
                    SavedListOfBusyBuilders = new List<Builder>(MainPage.BrigadeViewModel.ListOfBusyBuilders)
                };


                string jsonData = JsonConvert.SerializeObject(dataToSave, Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
                await App.Current.MainPage.DisplayAlert("Уведомление", "Данные успешно сохранились", "Ок");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", $"Ошибка сохранения: {ex.Message}", "Ок");
            }
        }

        public static async void LoadMainListsFromViewModels()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    var loadedData = JsonConvert.DeserializeObject<DataStorage>(jsonData);

                    if (loadedData != null)
                    {
                        MainPage.BrigadeViewModel.ListOfBrigades = new ObservableCollection<Brigade>(loadedData.SavedListOfBrigades);
                        MainPage.BuilderViewModel.ListOfBuilders = new ObservableCollection<Builder>(loadedData.SavedListOfBuilders);
                        MainPage.BuildingsViewModel.ListOfBuildings = new ObservableCollection<Building>(loadedData.SavedListOfBuildings);
                        MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials = new ObservableCollection<string>(loadedData.SavedListOfExistingMaterials);
                        MainPage.RegistrationViewModel.ListOfRegistrations = new ObservableCollection<Registration>(loadedData.SavedListOfRegistrations);
                        MainPage.RequestViewModel.ListOfRequests = new ObservableCollection<Request>(loadedData.SavedListOfRequests);
                        MainPage.SupplierViewModel.ListOfSuppliers = new ObservableCollection<Supplier>(loadedData.SavedListOfSuppliers);
                        MainPage.WorkAccountingViewModel.ListOfWorksAccounting = new ObservableCollection<WorkAccounting>(loadedData.SavedListOfWorksAccounting);
                        MainPage.BrigadeViewModel.ListOfFreeBuilders = new ObservableCollection<Builder>(loadedData.SavedListOfFreeBuilders);
                        MainPage.BrigadeViewModel.ListOfBusyBuilders = new ObservableCollection<Builder>(loadedData.SavedListOfBusyBuilders);
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", $"Ошибка загрузки: {ex.Message}", "Ок");
            }
        }
    }
}
