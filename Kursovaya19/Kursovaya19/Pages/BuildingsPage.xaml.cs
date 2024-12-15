using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class BuildingsPage : ContentPage
{
    public BuildingsViewModel BuildingsViewModel { get; set; }
    public BuildingsPage(BuildingsViewModel buildingsViewModel)
	{
		InitializeComponent();
        BuildingsViewModel = buildingsViewModel;
        BindingContext = BuildingsViewModel;
    }

    async void AddConstrObjectClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAndSetBuildingPage(BuildingsViewModel));
    }

    async void ChangeConstrObjectClicked(object sender, EventArgs e)
    {
        if (listOfConstrObjects.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetBuildingPage(BuildingsViewModel));
    }
}