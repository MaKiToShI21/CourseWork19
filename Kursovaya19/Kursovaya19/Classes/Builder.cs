using System.Collections.ObjectModel;
using System.Reflection;

namespace Kursovaya19.Classes
{
    public class Builder
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string ResidenceAddress { get; set; }
        public int WorkExperience { get; set; }
        public ObservableCollection<string> ListOfSpecialities { get; set; }

        public Builder(string fullName, string gender, DateTime birthday, string residenceAddress, int workExperience, ObservableCollection<string> listOfSpecialities)
        {
            FullName = fullName;
            Gender = gender;
            Birthday = birthday;
            ResidenceAddress = residenceAddress;
            WorkExperience = workExperience;
            ListOfSpecialities = listOfSpecialities;
        }

        public Builder()
        {
            ListOfSpecialities = new ObservableCollection<string>();
        }

        public Builder(Builder other)
        {
            FullName = other.FullName;
            Gender = other.Gender;
            Birthday = other.Birthday;
            ResidenceAddress = other.ResidenceAddress;
            WorkExperience = other.WorkExperience;
            ListOfSpecialities = other.ListOfSpecialities;
        }
    }
}
