using System.Collections.ObjectModel;

namespace Kursovaya19.Classes
{
    public class Request
    {
        public Building Building { get; set; }
        public DateTime DateTime { get; set; }
        public ObservableCollection<RequestMaterial> ListOfRequestMaterials { get; set; }

        public Request(Building building, DateTime dateTime, ObservableCollection<RequestMaterial> listOfRequestMaterials)
        {
            Building = building;
            DateTime = dateTime;
            ListOfRequestMaterials = listOfRequestMaterials;
        }

        public Request()
        {
            ListOfRequestMaterials = new ObservableCollection<RequestMaterial>();
        }
    }
}
