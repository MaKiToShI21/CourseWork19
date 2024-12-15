using Kursovaya19.AddAndSetPages;
using Kursovaya19.ViewModels;

namespace Kursovaya19.Pages;

public partial class BrigadesPage : ContentPage
{
	BrigadeViewModel BrigadeViewModel { get; set; }
    public BrigadesPage(BrigadeViewModel brigadeViewModel)
	{
		InitializeComponent();
		BrigadeViewModel = brigadeViewModel;
		BindingContext = BrigadeViewModel;
    }

	async void AddNewBrigadeClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddAndSetBrigadePage(BrigadeViewModel));
	}

    async void ChangeBrigadeClicked(object sender, EventArgs e)
    {
		if (listOfBrigades.SelectedItem != null)
			await Navigation.PushAsync(new AddAndSetBrigadePage(BrigadeViewModel));
    }
}