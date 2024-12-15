using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class SupplierPage : ContentPage
{
    SupplierViewModel SupplierViewModel { get; set; }
	public SupplierPage(SupplierViewModel supplierViewModel)
	{
		InitializeComponent();
        SupplierViewModel = supplierViewModel;
        BindingContext = SupplierViewModel;
	}

    async void AddSupplierClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddAndSetSupplierPage(SupplierViewModel));
	}

    async void ChangeSupplierClicked(object sender, EventArgs e)
    {
        if (listOfSuppliers.SelectedItem != null)
            await Navigation.PushAsync(new AddAndSetSupplierPage(SupplierViewModel));
    }
}