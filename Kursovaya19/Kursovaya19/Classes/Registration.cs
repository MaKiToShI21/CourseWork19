using System.Collections.ObjectModel;

namespace Kursovaya19.Classes
{
    public class Registration
    {
        public Supplier Supplier { get; set; }
        public DateTime DateTime { get; set; }
        public ObservableCollection<RegistrationMaterial> ListOfRegistrationMaterials { get; set; }

        public Registration(Supplier supplier, DateTime dateTime, ObservableCollection<RegistrationMaterial> listOfRegistrationMaterials)
        {
            Supplier = supplier;
            DateTime = dateTime;
            ListOfRegistrationMaterials = listOfRegistrationMaterials;
        }

        public Registration()
        {
            ListOfRegistrationMaterials = new ObservableCollection<RegistrationMaterial>();
        }
    }
}
