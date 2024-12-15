using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class MaterialsAtWarehousePage : ContentPage
{
	public RegistrationViewModel RegistrationViewModel { get; set; }

    public MaterialsAtWarehousePage(RegistrationViewModel registrationViewModel)
	{
		InitializeComponent();
        RegistrationViewModel = registrationViewModel;
        BindingContext = RegistrationViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }
}