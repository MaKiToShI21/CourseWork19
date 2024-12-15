using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddListOfExistingMaterialsPage : ContentPage
{
	public AddListOfExistingMaterialsPage(ExistingMaterialsViewModel existingMaterialsViewModel)
	{
        InitializeComponent();
        BindingContext = existingMaterialsViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }
}