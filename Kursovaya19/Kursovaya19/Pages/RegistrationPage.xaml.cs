using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class RegistrationPage : ContentPage
{
	public RegistrationViewModel RegistrationViewModel { get; set; }
    public RegistrationPage(RegistrationViewModel registrationViewModel)
	{
		InitializeComponent();
        RegistrationViewModel = registrationViewModel;
		BindingContext = RegistrationViewModel;
    }

    async void RegistrationClicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AddAndSetRegistrationPage(RegistrationViewModel));
    }

    async void ChangeRegistrationClicked(object sender, EventArgs e)
    {
        if (listOfRegistrations.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetRegistrationPage(RegistrationViewModel));
    }
}