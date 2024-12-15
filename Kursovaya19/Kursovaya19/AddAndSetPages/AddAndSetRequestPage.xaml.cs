using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetRequestPage : ContentPage
{
	public RequestViewModel RequestViewModel { get; set; }
    public AddAndSetRequestPage(RequestViewModel requestViewModel)
	{
		InitializeComponent();
		RequestViewModel = requestViewModel;
		BindingContext = RequestViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }

    async void AddRequestMaterialClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddAndSetRequestMaterialPage(RequestViewModel));
	}

    async void ChangeRequestMaterialClicked(object sender, EventArgs e)
    {
        if (listOfRequestMaterials.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetRequestMaterialPage(RequestViewModel));
    }
}