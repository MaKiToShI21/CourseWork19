using Kursovaya19.ViewModels;
using System.Collections.ObjectModel;

namespace Kursovaya19.AddAndSetPages;

public partial class AddAndSetWorkAccountingPage : ContentPage
{
    public WorkAccountingViewModel WorkAccountingViewModel { get; set; }
    public AddAndSetWorkAccountingPage(WorkAccountingViewModel workAccountingViewModel)
	{
		InitializeComponent();
        WorkAccountingViewModel = workAccountingViewModel;
        BindingContext = WorkAccountingViewModel;

        buildingsPicker.ItemsSource = new ObservableCollection<string>(
                MainPage.BuildingsViewModel.ListOfBuildings
                .Select(s => s.NameOfBuilding)
                .ToList());

        brigadesPicker.ItemsSource = new ObservableCollection<string>(
            MainPage.BrigadeViewModel.ListOfBrigades
            .Select(s => s.NameOfBrigade)
            .ToList());

        SetInitialBuildingsPickerPickerValue();
        SetInitialBrigadesPickerValue();

        NavigationPage.SetHasBackButton(this, false);
    }

    private void SetInitialBuildingsPickerPickerValue()
    {
        if (MainPage.BuildingsViewModel.ListOfBuildings != null)
        {
            var selectedNameOfBuilding = MainPage.BuildingsViewModel.ListOfBuildings
                .FirstOrDefault(s => s.NameOfBuilding == WorkAccountingViewModel.NameOfBuilding);

            if (selectedNameOfBuilding != null)
            {
                int index = MainPage.BuildingsViewModel.ListOfBuildings.IndexOf(selectedNameOfBuilding);
                buildingsPicker.SelectedIndex = index;
            }
        }
    }

    private void SetInitialBrigadesPickerValue()
    {
        var selectedNameOfBrigade = MainPage.BrigadeViewModel.ListOfBrigades
            .FirstOrDefault(s => s.NameOfBrigade == WorkAccountingViewModel.NameOfBrigade);

        if (selectedNameOfBrigade != null)
        {
            int index = MainPage.BrigadeViewModel.ListOfBrigades.IndexOf(selectedNameOfBrigade);
            brigadesPicker.SelectedIndex = index;
        }
    }

    async void BuildingsPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (buildingsPicker.SelectedIndex != -1)
            WorkAccountingViewModel.NameOfBuilding = buildingsPicker.SelectedItem.ToString();
    }

    async void BrigadesPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (buildingsPicker.SelectedIndex != -1)
            WorkAccountingViewModel.NameOfBrigade = brigadesPicker.SelectedItem.ToString();
    }

    private async void CancelChangeCompletedWorkCommand(object sender, EventArgs e)
    {
        listOfCompletedWorks.SelectedItem = null;
    }
}