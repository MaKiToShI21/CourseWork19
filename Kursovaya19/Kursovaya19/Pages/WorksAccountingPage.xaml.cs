using Kursovaya19.AddAndSetPages;
using Kursovaya19.ViewModels;

namespace Kursovaya19.Pages;

public partial class WorksAccountingPage : ContentPage
{
    public WorkAccountingViewModel WorkAccountingViewModel { get; set; }
    public WorksAccountingPage(WorkAccountingViewModel workAccountingViewModel)
    {
        InitializeComponent();
        WorkAccountingViewModel = workAccountingViewModel;
        BindingContext = WorkAccountingViewModel;
    }

    public async void AddWorkAccountingClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAndSetWorkAccountingPage(WorkAccountingViewModel));
    }

    public async void ChangeWorkAccountingClicked(object sender, EventArgs e)
    {
        if (listOfWorksAccounting.SelectedItem != null)
        {
            await Navigation.PushAsync(new AddAndSetWorkAccountingPage(WorkAccountingViewModel));
        }
    }
}