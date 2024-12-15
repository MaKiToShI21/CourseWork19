using Kursovaya19.ViewModels;

namespace Kursovaya19.AddAndSetPages;

public partial class AddAndSetBrigadePage : ContentPage
{
    BrigadeViewModel BrigadeViewModel { get; set; }
    public AddAndSetBrigadePage(BrigadeViewModel brigadeViewModel)
	{
		InitializeComponent();
        BrigadeViewModel = brigadeViewModel;
        BindingContext = BrigadeViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }
}