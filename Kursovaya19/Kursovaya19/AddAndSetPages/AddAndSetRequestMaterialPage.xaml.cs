using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetRequestMaterialPage : ContentPage
{
    RequestViewModel RequestViewModel { get; set; }
    public AddAndSetRequestMaterialPage(RequestViewModel requestViewModel)
	{
		InitializeComponent();
        RequestViewModel = requestViewModel;
        BindingContext = RequestViewModel;

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
                .FirstOrDefault(s => s == RequestViewModel.NameOfMaterial);

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
            .FirstOrDefault(s => s == RequestViewModel.Unit);

        if (selectedUnit != null)
        {
            int index = MainPage.ExistingMaterialsViewModel.ListOfUnits.IndexOf(selectedUnit);
            unitPicker.SelectedIndex = index;
        }
    }

    async void NameOfMaterialPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (existingMaterialPicker.SelectedIndex != -1)
            RequestViewModel.NameOfMaterial = existingMaterialPicker.SelectedItem.ToString();
    }

    async void UnitPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (unitPicker.SelectedIndex != -1)
            RequestViewModel.Unit = unitPicker.SelectedItem.ToString();
    }
}