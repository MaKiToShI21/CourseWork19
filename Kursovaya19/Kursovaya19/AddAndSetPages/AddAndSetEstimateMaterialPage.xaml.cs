using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetEstimateMaterialPage : ContentPage
{
    public BuildingsViewModel ConstructionObjectsViewModel { get; set; }
    public AddAndSetEstimateMaterialPage(BuildingsViewModel constructionObjectsViewModel)
	{
		InitializeComponent();
        ConstructionObjectsViewModel = constructionObjectsViewModel;
        BindingContext = ConstructionObjectsViewModel;

        existingMaterialPicker.ItemsSource = MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials;
        unitPicker.ItemsSource = MainPage.ExistingMaterialsViewModel.ListOfUnits;

        SetInitialNameOfMaterialPickerValue();
        SetInitialUnitPickerValue();

        NavigationPage.SetHasBackButton(this, false);
    }

    private void SetInitialNameOfMaterialPickerValue()
    {
        if (MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials != null)
        {
            var selectedNameOfMaterial = MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials
                .FirstOrDefault(s => s == ConstructionObjectsViewModel.NameOfMaterial);

            if (selectedNameOfMaterial != null)
            {
                int index = MainPage.ExistingMaterialsViewModel.ListOfExistingMaterials.IndexOf(selectedNameOfMaterial);
                existingMaterialPicker.SelectedIndex = index;
            }
        }
    }

    private void SetInitialUnitPickerValue()
    {
        var selectedUnit = MainPage.ExistingMaterialsViewModel.ListOfUnits
            .FirstOrDefault(s => s == ConstructionObjectsViewModel.Unit);

        if (selectedUnit != null)
        {
            int index = MainPage.ExistingMaterialsViewModel.ListOfUnits.IndexOf(selectedUnit);
            unitPicker.SelectedIndex = index;
        }
    }

    async void NameOfMaterialPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (existingMaterialPicker.SelectedIndex != -1)
            ConstructionObjectsViewModel.NameOfMaterial = existingMaterialPicker.SelectedItem.ToString();
    }

    async void UnitPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (unitPicker.SelectedIndex != -1)
            ConstructionObjectsViewModel.Unit = unitPicker.SelectedItem.ToString();
    }
}