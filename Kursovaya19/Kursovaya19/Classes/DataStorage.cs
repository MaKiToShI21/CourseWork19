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
        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SavedDataStorage.json");

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
                else
                {
                    MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials = new ObservableCollection<string>
                    {
                        "Бетон",
                        "Кирпич",
                        "Плитка"
                    };

                    MainPage.BuildingsViewModel.ListOfBuildings = new ObservableCollection<Building>
                    {
                        new Building(
                            "Торговый центр 'МегаМаркет'",
                            "г. Новосибирск, пр. Строителей, 10",
                            new ObservableCollection<EstimateMaterial>
                            {
                                new EstimateMaterial("Бетон", 500, "м³"),
                                new EstimateMaterial("Плитка", 3000, "м²")
                            }
                        ),
                        new Building(
                            "Жилой комплекс 'Солнечный'",
                            "г. Москва, ул. Лесная, 45",
                            new ObservableCollection<EstimateMaterial>
                            {
                                new EstimateMaterial("Бетон", 1000, "м³"),
                                new EstimateMaterial("Кирпич", 10000, "шт.")
                            }
                        )
                    };

                    MainPage.SupplierViewModel.ListOfSuppliers = new ObservableCollection<Supplier>
                    {
                        new Supplier(
                            MainPage.BuildingsViewModel.ListOfBuildings[0],
                            "Иванов Иван Иванович",
                            "+74951234567",
                            "Сбербанк",
                            "40702810400000123456",
                            "772345678901"
                        ),
                        new Supplier(
                            MainPage.BuildingsViewModel.ListOfBuildings[1],
                            "Петров Дмитрий Юрьевич",
                            "+78126543210",
                            "Газпромбанк",
                            "40702810345678901234",
                            "165912345678"
                        )
                    };

                    MainPage.BrigadeViewModel.ListOfBrigades = new ObservableCollection<Brigade>
                    {
                        new Brigade("Бригада 'Монолит'", new Builder("Иванов Сергей Петрович", "м", new DateTime(1990, 3, 15), "г. Москва, ул. Академика, 15", 10, new ObservableCollection<string> { "Каменщик", "Бетонщик" }),
                            new ObservableCollection<Builder>
                            {
                                new Builder("Сидоров Алексей Николаевич", "м", new DateTime(1995, 5, 30), "г. Казань, ул. Советская, 2", 7, new ObservableCollection<string> { "Электрик", "Сантехник" })
                            }
                        ),
                        new Brigade("Бригада 'Отделка'", new Builder("Петрова Мария Ивановна", "ж", new DateTime(1988, 7, 21), "г. Санкт-Петербург, пр. Мира, 10", 12, new ObservableCollection<string> { "Отделочник", "Плиточник" }),
                            new ObservableCollection<Builder>
                            {
                                new Builder("Волков Дмитрий Игоревич", "м", new DateTime(1992, 11, 12), "г. Екатеринбург, ул. Крупской, 8", 8, new ObservableCollection<string> { "Сварщик", "Монтажник" })
                            }
                        )
                    };

                    MainPage.BuilderViewModel.ListOfBuilders = new ObservableCollection<Builder>
                    {
                        new Builder("Иванов Сергей Петрович", "м", new DateTime(1990, 3, 15), "г. Москва, ул. Академика, 15", 10, new ObservableCollection<string> { "Каменщик", "Бетонщик" }),
                        new Builder("Сидоров Алексей Николаевич", "м", new DateTime(1995, 5, 30), "г. Казань, ул. Советская, 2", 7, new ObservableCollection<string> { "Электрик", "Сантехник" }),
                        new Builder("Петрова Мария Ивановна", "ж", new DateTime(1988, 7, 21), "г. Санкт-Петербург, пр. Мира, 10", 12, new ObservableCollection<string> { "Отделочник", "Плиточник" }),
                        new Builder("Волков Дмитрий Игоревич", "м", new DateTime(1992, 11, 12), "г. Екатеринбург, ул. Крупской, 8", 8, new ObservableCollection<string> { "Сварщик", "Монтажник" })
                    };

                    MainPage.BrigadeViewModel.ListOfFreeBuilders = new ObservableCollection<Builder>
                    {
                    };

                    MainPage.BrigadeViewModel.ListOfBusyBuilders = new ObservableCollection<Builder>
                    {
                        new Builder("Иванов Сергей Петрович", "м", new DateTime(1990, 3, 15), "г. Москва, ул. Академика, 15", 10, new ObservableCollection<string> { "Каменщик", "Бетонщик" }),
                        new Builder("Сидоров Алексей Николаевич", "м", new DateTime(1995, 5, 30), "г. Казань, ул. Советская, 2", 7, new ObservableCollection<string> { "Электрик", "Сантехник" }),
                        new Builder("Петрова Мария Ивановна", "ж", new DateTime(1988, 7, 21), "г. Санкт-Петербург, пр. Мира, 10", 12, new ObservableCollection<string> { "Отделочник", "Плиточник" }),
                        new Builder("Волков Дмитрий Игоревич", "м", new DateTime(1992, 11, 12), "г. Екатеринбург, ул. Крупской, 8", 8, new ObservableCollection<string> { "Сварщик", "Монтажник" })
                    };

                    MainPage.RequestViewModel.ListOfRequests = new ObservableCollection<Request>
                    {
                        new Request(MainPage.BuildingsViewModel.ListOfBuildings[0], new DateTime(2024, 12, 4),
                            new ObservableCollection<RequestMaterial>
                            {
                                new RequestMaterial("Бетон", 200, "м³"),
                                new RequestMaterial("Плитка", 1000, "м²")
                            }
                        ),
                        new Request(MainPage.BuildingsViewModel.ListOfBuildings[1], new DateTime(2024, 12, 2),
                            new ObservableCollection<RequestMaterial>
                            {
                                new RequestMaterial("Бетон", 150, "м³"),
                                new RequestMaterial("Кирпич", 3000, "шт.")
                            }
                        )
                    };
                        
                    MainPage.RegistrationViewModel.ListOfRegistrations = new ObservableCollection<Registration>
                    {
                        new Registration(MainPage.SupplierViewModel.ListOfSuppliers[0], new DateTime(2024, 12, 2),
                            new ObservableCollection<RegistrationMaterial>
                            {
                                new RegistrationMaterial("Бетон", 25000, 100, "м³"),
                                new RegistrationMaterial("Кирпич", 50000, 3000, "шт.")
                            }
                        ),
                        new Registration(MainPage.SupplierViewModel.ListOfSuppliers[1], new DateTime(2024, 12, 30),
                            new ObservableCollection<RegistrationMaterial>
                            {
                                new RegistrationMaterial("Плитка", 75000, 500, "м²")
                            }
                        )
                    };

                    MainPage.WorkAccountingViewModel.ListOfWorksAccounting = new ObservableCollection<WorkAccounting>
                    {
                        new WorkAccounting(new DateTime(2024, 12, 7), "Жилой комплекс 'Солнечный'", "Бригада 'Монолит'",
                            new ObservableCollection<CompletedWork>
                            {
                                new CompletedWork("Заливка фундамента", 50.1), new CompletedWork("Укладка плитки", 78.2)
                            }
                        ),
                        new WorkAccounting(new DateTime(2024, 12, 9), "Торговый центр 'МегаМаркет'", "Бригада 'Монолит'",
                            new ObservableCollection<CompletedWork>
                            {
                                new CompletedWork("Армирование стен", 24.3)
                            }
                        )
                    };
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", $"Ошибка загрузки: {ex.Message}", "Ок");
            }
        }
    }
}
