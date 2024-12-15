using System.Collections.ObjectModel;

namespace Kursovaya19.Classes
{
    public class Brigade
    {
        public string NameOfBrigade { get; set; }
        public Builder Brigadier { get; set; }
        public ObservableCollection<Builder> Builders { get; set; }

        public Brigade(string nameOfBrigade, Builder brigadier, ObservableCollection<Builder> builders)
        {
            NameOfBrigade = nameOfBrigade;
            Brigadier = brigadier;
            Builders = builders;
        }

        public Brigade()
        {
            Builders = new ObservableCollection<Builder>();
        }
    }
}
