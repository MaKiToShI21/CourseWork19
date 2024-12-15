using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class RequestPage : ContentPage
{
    public RequestViewModel RequestViewModel { get; set; }
    public RequestPage(RequestViewModel requestViewModel)
	{
		InitializeComponent();
        RequestViewModel = requestViewModel;
        BindingContext = RequestViewModel;
    }

    async void AddRequestClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAndSetRequestPage(RequestViewModel));
    }

    async void ChangeRequestClicked(object sender, EventArgs e)
    {
        if (listOfRequests.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetRequestPage(RequestViewModel));
    }
}