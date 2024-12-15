using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class ExistingMaterialsPage : ContentPage
{
    public ExistingMaterialsViewModel ExistingMaterialsViewModel { get; set; }
	public ExistingMaterialsPage(ExistingMaterialsViewModel existingMaterialsViewModel)
	{
		InitializeComponent();
        ExistingMaterialsViewModel = existingMaterialsViewModel;
        BindingContext = ExistingMaterialsViewModel;
    }

    async void AddListOfExistingMaterialsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddListOfExistingMaterialsPage(ExistingMaterialsViewModel));
    }
}