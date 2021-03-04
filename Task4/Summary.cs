using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    public class Summary : INotifyPropertyChanged
    {
        private string filePath;
        private string fullName;
        private string city;
        private int excperience;
        private float salaryRequirements;
        private string skills;
        public string FilePath { get { return filePath; } set { if (value != filePath) { filePath = value; OnPropertyChanged(); } } }

        public string FullName { get { return fullName; } set { if (value != fullName) { fullName = value;OnPropertyChanged(); } } }
        public string City { get { return city; } set { if (value != city) { city = value; OnPropertyChanged(); } } }
        public int Excperience { get { return excperience; } set { if (value != excperience) { excperience = value; OnPropertyChanged(); } } }
        public float SalaryRequirements { get { return salaryRequirements; } set { if (value != salaryRequirements) { salaryRequirements = value; OnPropertyChanged(); } } }
        public string Skills { get { return skills; } set { if (value != skills) { skills = value; OnPropertyChanged(); } } }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
