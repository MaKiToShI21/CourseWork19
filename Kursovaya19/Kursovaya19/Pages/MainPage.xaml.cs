using Kursovaya19.Classes;
using Kursovaya19.Pages;
using Kursovaya19.ViewModels;
using System.Collections.ObjectModel;

namespace Kursovaya19
{
    public partial class MainPage : ContentPage
    {
        public static ExistingMaterialsViewModel ExistingMaterialsViewModel { get; } = new();
        public static BuildingsViewModel BuildingsViewModel { get; } = new();
        public static SupplierViewModel SupplierViewModel { get; } = new();
        public static RegistrationViewModel RegistrationViewModel { get; } = new();
        public static RequestViewModel RequestViewModel { get; } = new();
        public static BuilderViewModel BuilderViewModel { get; } = new();
        public static BrigadeViewModel BrigadeViewModel { get; } = new();
        public static WorkAccountingViewModel WorkAccountingViewModel { get; } = new();

        public MainPage()
        {
            InitializeComponent();
            SaveAndLoadData.LoadMainListsFromViewModels();
        }

        async void ConstructionObjectsCliked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new BuildingsPage(BuildingsViewModel));
        }

        async void MaterialsAtWarehoudeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MaterialsAtWarehousePage(RegistrationViewModel));
        }

        async void SupplierClicked(object sender, System.EventArgs e)
        {
            SupplierViewModel.ListOfBuildings = new ObservableCollection<Building>(BuildingsViewModel.ListOfBuildings);
            await Navigation.PushAsync(new SupplierPage(SupplierViewModel));
        }

        async void RegistrReceiptOfMaterialsClicked(object sender, System.EventArgs e)
        {
            RegistrationViewModel.ListOfSuppliers = new ObservableCollection<Supplier>(SupplierViewModel.ListOfSuppliers);
            await Navigation.PushAsync(new RegistrationPage(RegistrationViewModel));
        }

        async void RequestClicked(object sender, System.EventArgs e)
        {
            RequestViewModel.ListOfBuildings = new ObservableCollection<Building>(BuildingsViewModel.ListOfBuildings);
            await Navigation.PushAsync(new RequestPage(RequestViewModel));
        }

        async void BuilderClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new BuildersPage(BuilderViewModel));
        }

        async void BrigadeClicked(object sender, System.EventArgs e)
        {
            BrigadeViewModel.AllBuilders = new ObservableCollection<Builder>(BuilderViewModel.ListOfBuilders);
            await Navigation.PushAsync(new BrigadesPage(BrigadeViewModel));
        }

        async void WorkClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new WorksAccountingPage(WorkAccountingViewModel));
        }

        async void ExistingMaterialsCliked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ExistingMaterialsPage(ExistingMaterialsViewModel));
        }

        private void SaveDataClicked(object sender, EventArgs e)
        {
            SaveAndLoadData.SaveMainListsFromViewModels();
        }
    }
}
