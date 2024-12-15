using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetRegistrationPage : ContentPage
{
    public RegistrationViewModel RegistrationViewModel { get; set; }
    public AddAndSetRegistrationPage(RegistrationViewModel registrationViewModel)
	{
        InitializeComponent();
        RegistrationViewModel = registrationViewModel;
        BindingContext = RegistrationViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }

    async void AddRegistrationMaterialClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAndSetRegistrationMaterialPage(RegistrationViewModel));
    }

    async void ChangeRegistrationMaterialClicked(object sender, EventArgs e)
    {
        if (listOfMaterials.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetRegistrationMaterialPage(RegistrationViewModel));
    }
}