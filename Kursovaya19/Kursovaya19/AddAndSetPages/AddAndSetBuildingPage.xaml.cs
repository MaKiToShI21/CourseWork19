using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetBuildingPage : ContentPage
{
    public BuildingsViewModel BuildingsViewModel { get; set; }

    public AddAndSetBuildingPage(BuildingsViewModel buildingsViewModel)
    {
        InitializeComponent();
        BuildingsViewModel = buildingsViewModel;
        BindingContext = BuildingsViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }

    async void AddEstimateMaterialClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAndSetEstimateMaterialPage(BuildingsViewModel));
    }

    async void ChangeEstimateMaterialClicked(object sender, EventArgs e)
    {
        if (listOfMaterials.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetEstimateMaterialPage(BuildingsViewModel));
    }
}