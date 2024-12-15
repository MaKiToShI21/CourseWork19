using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class BuildersPage : ContentPage
{
    BuilderViewModel BuilderViewModel { get; set; }
    public BuildersPage(BuilderViewModel builderViewModel)
	{
		InitializeComponent();
        BuilderViewModel = builderViewModel;
        BindingContext = BuilderViewModel;
    }

    async void AddBuilderClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAndSetBuilderPage(BuilderViewModel));
    }

    async void ChangeBuilderClicked(object sender, EventArgs e)
    {
        if (listOfBuilders.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetBuilderPage(BuilderViewModel));
    }
}