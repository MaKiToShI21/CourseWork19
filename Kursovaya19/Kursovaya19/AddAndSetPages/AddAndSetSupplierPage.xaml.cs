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

    // Обработчик для ФИО поставщика (только буквы)
    private void OnFullNameTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry == null || string.IsNullOrEmpty(e.NewTextValue))
            return;

        // Фильтруем только буквы (и пробелы между ними)
        string filteredText = new string(e.NewTextValue.Where(c => char.IsLetter(c) || c == ' ').ToArray());

        // Если текст изменился, обновляем текст в поле
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

        // Оставляем только цифры и символ '+' (если они находятся в начале строки)
        string filteredText = new string(e.NewTextValue.Where(c => char.IsDigit(c) || c == '+').ToArray());

        // Убираем лишние символы '+' (разрешаем только один '+' в начале)
        if (filteredText.IndexOf('+') > 0)
            filteredText = filteredText.Replace("+", "");

        // Если текст не изменился, возвращаем его в поле
        if (filteredText != e.NewTextValue)
            entry.Text = filteredText;

        // Проверка на запрет удаления "+7"
        if (entry.Text.Length < 3)
            entry.Text = "+7";
    }


    // Обработчик для Названия банка (буквы и некоторые спецсимволы)
    private void OnBankNameTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry == null || string.IsNullOrEmpty(e.NewTextValue))
            return;

        // Разрешаем буквы и указанные спецсимволы
        string allowedSymbols = "-\"'()«»";  // Допустимые спецсимволы
        string filteredText = new string(e.NewTextValue.Where(c => char.IsLetter(c) || allowedSymbols.Contains(c) || char.IsWhiteSpace(c)).ToArray());

        // Если текст изменился, обновляем текст в поле
        if (e.NewTextValue != filteredText)
        {
            entry.Text = filteredText;
        }
    }
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // Оставляем только цифры
        string filteredText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // Если текст изменился после фильтрации, обновляем поле ввода
        if (e.NewTextValue != filteredText)
        {
            entry.Text = filteredText;
        }
    }
}