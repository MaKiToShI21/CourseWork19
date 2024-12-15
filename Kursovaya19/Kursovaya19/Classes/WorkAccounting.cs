using System.Collections.ObjectModel;

namespace Kursovaya19.Classes
{
    public class WorkAccounting
    {
        public DateTime DateTime { get; set; }
        public string NameOfBuilding { get; set; }
        public string NameOfBrigade { get; set; }
        public ObservableCollection<CompletedWork> ListOfCompletedWorks{ get; set; }

        public WorkAccounting(DateTime dateTime, string nameOfBuilding, string nameOfBrigade, ObservableCollection<CompletedWork> listOfCompletedWorks)
        {
            DateTime = dateTime;
            NameOfBuilding = nameOfBuilding;
            NameOfBrigade = nameOfBrigade;
            ListOfCompletedWorks = listOfCompletedWorks;
        }

        public WorkAccounting()
        {
            ListOfCompletedWorks = new ObservableCollection<CompletedWork>();
        }
    }
}
