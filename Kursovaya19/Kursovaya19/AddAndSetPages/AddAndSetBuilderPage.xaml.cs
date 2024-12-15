using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetBuilderPage : ContentPage
{
    public BuilderViewModel BuilderViewModel { get; set; }
    public AddAndSetBuilderPage(BuilderViewModel builderViewModel)
	{
		InitializeComponent();
        BuilderViewModel = builderViewModel;
        BindingContext = BuilderViewModel;

        NavigationPage.SetHasBackButton(this, false);
    }

    private void OnFullNameTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry == null || string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filteredText = new string(e.NewTextValue.Where(c => char.IsLetter(c) || c == ' ').ToArray());

        if (e.NewTextValue != filteredText)
        {
            entry.Text = filteredText;
        }
    }
}