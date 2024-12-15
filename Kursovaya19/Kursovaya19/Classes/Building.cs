using System.Collections.ObjectModel;


namespace Kursovaya19.Classes
{
    public class Building
    {
        public string NameOfBuilding { get; set; }
        public string AddressOfBuilding { get; set; }
        public ObservableCollection<EstimateMaterial> ListOfEstimateMaterials { get; set; }
        public Building(string nameOfBuilding, string addressOfBuilding, ObservableCollection<EstimateMaterial> listOfEstimateMaterials)
        {
            NameOfBuilding = nameOfBuilding;
            AddressOfBuilding = addressOfBuilding;
            ListOfEstimateMaterials = listOfEstimateMaterials;
        }

        public Building()
        {
            ListOfEstimateMaterials = new ObservableCollection<EstimateMaterial>();
        }
    }
}
