using Kursovaya19.ViewModels;

namespace Kursovaya19;

public partial class AddAndSetSupplierPage : ContentPage
{
	public AddAndSetSupplierPage(SupplierViewModel supplierViewModel)
	{
		InitializeComponent();
		BindingContext = supplierViewModel;

        if (string.IsNullOrEmpty(supplierViewModel.PhoneNumber))
            supplierViewModel.PhoneNumber = "+7";

        NavigationPage.SetHasBackButton(this, false);
    }

    // ���������� ��� ��� ���������� (������ �����)
    private void OnFullNameTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry == null || string.IsNullOrEmpty(e.NewTextValue))
            return;

        // ��������� ������ ����� (� ������� ����� ����)
        string filteredText = new string(e.NewTextValue.Where(c => char.IsLetter(c) || c == ' ').ToArray());

        // ���� ����� ���������, ��������� ����� � ����
        if (e.NewTextValue != filteredText)
        {
            entry.Text = filteredText;
        }
    }

    private void OnPhoneNumberTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry == null || string.IsNullOrEmpty(e.NewTextValue))
            return;

        // ��������� ������ ����� � ������ '+' (���� ��� ��������� � ������ ������)
        string filteredText = new string(e.NewTextValue.Where(c => char.IsDigit(c) || c == '+').ToArray());

        // ������� ������ ������� '+' (��������� ������ ���� '+' � ������)
        if (filteredText.IndexOf('+') > 0)
            filteredText = filteredText.Replace("+", "");

        // ���� ����� �� ���������, ���������� ��� � ����
        if (filteredText != e.NewTextValue)
            entry.Text = filteredText;

        // �������� �� ������ �������� "+7"
        if (entry.Text.Length < 3)
            entry.Text = "+7";
    }


    // ���������� ��� �������� ����� (����� � ��������� �����������)
    private void OnBankNameTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry == null || string.IsNullOrEmpty(e.NewTextValue))
            return;

        // ��������� ����� � ��������� �����������
        string allowedSymbols = "-\"'()��";  // ���������� �����������
        string filteredText = new string(e.NewTextValue.Where(c => char.IsLetter(c) || allowedSymbols.Contains(c) || char.IsWhiteSpace(c)).ToArray());

        // ���� ����� ���������, ��������� ����� � ����
        if (e.NewTextValue != filteredText)
        {
            entry.Text = filteredText;
        }
    }
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // ��������� ������ �����
        string filteredText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // ���� ����� ��������� ����� ����������, ��������� ���� �����
        if (e.NewTextValue != filteredText)
        {
            entry.Text = filteredText;
        }
    }
}