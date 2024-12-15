namespace Kursovaya19.Classes
{
    public class Supplier
    {
        public Building Building { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string BankName { get; set; }
        public string CurrentAccount { get; set; }
        public string INN { get; set; }

        public Supplier(Building building, string fullName, string phoneNumber, string bankName, string currentAccount, string inn)
        {
            Building = building;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            BankName = bankName;
            CurrentAccount = currentAccount;
            INN = inn;
        }

        public Supplier() { }
    }
}
