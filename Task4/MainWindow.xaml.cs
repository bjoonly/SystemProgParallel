using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Task4
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string folderPath;
        public string FolderPath { get { return folderPath; } set { if (value != folderPath) { folderPath = value; OnPropertyChanged(); } } }
        private string path;
        public string Path { get { return path; } set { if (value != path) { path = value; OnPropertyChanged(); } } }
        private string enteredCity;
        public string EnteredCity { get { return enteredCity; } set { if (value != enteredCity) { enteredCity = value; OnPropertyChanged(); } } }

        private Summary mostExc;
        public Summary MostExc { get { return mostExc; } set { if (value != mostExc) { mostExc = value; OnPropertyChanged(); } } }

        private Summary leastExc;
        public Summary LeastExc { get { return leastExc; } set { if (value != leastExc) { leastExc = value; OnPropertyChanged(); } } }

        private Summary highestSalaryReq;
        public Summary HighestSalaryReq { get { return highestSalaryReq; } set { if (value != highestSalaryReq) { highestSalaryReq = value; OnPropertyChanged(); } } }

        private Summary lowestSalaryReq;
        public Summary LowestSalaryReq { get { return lowestSalaryReq; } set { if (value != lowestSalaryReq) { lowestSalaryReq = value; OnPropertyChanged(); } } }


        private bool isSelectedMostExp;
        private bool isSelectedMostInexp;
        private bool isSelectedHighSalaryReq;
        private bool isSelectedLowestSalaryReq;
        private bool isSelectedSameCity;

        public  bool IsSelectedMostExp { get { return isSelectedMostExp; } set { if (value != isSelectedMostExp) { isSelectedMostExp = value; OnPropertyChanged(); } } }
        public  bool IsSelectedMostInexp { get { return isSelectedMostInexp; } set { if (value != isSelectedMostInexp) { isSelectedMostInexp = value; OnPropertyChanged(); } } }
        public  bool IsSelectedHighSalaryReq { get { return isSelectedHighSalaryReq; } set { if (value != isSelectedHighSalaryReq) { isSelectedHighSalaryReq = value; OnPropertyChanged(); } } }
        public  bool IsSelectedLowestSalaryReq { get { return isSelectedLowestSalaryReq; } set { if (value != isSelectedLowestSalaryReq) { isSelectedLowestSalaryReq = value; OnPropertyChanged(); } } }
        public  bool IsSelectedSameCity { get { return isSelectedSameCity; } set { if (value != isSelectedSameCity ){ isSelectedSameCity = value; OnPropertyChanged(); } } }


        private string filePath;
        public string FilePath { get { return filePath; } set { if (value != filePath) { filePath = value; OnPropertyChanged(); } } }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
           

        }
        private readonly ICollection<Summary> summaries = new ObservableCollection<Summary>();
        public IEnumerable<Summary> Summaries => summaries;
        private readonly ICollection<Summary> summariesOneCity = new ObservableCollection<Summary>();
        public IEnumerable<Summary> SummariesOneCity => summariesOneCity;

        private void fileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = @"All Files|*.txt;*.docx;*.doc";
            if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                FilePath = opd.FileName;
                Path = FilePath;

                Parallel.Invoke(() => AddSummary(new FileInfo(FilePath)));
                
            }
        }
        private void SearchApplOneCity()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
            summariesOneCity.Clear();

              
            }));
            foreach (var item in summaries.AsParallel().Where(s => s.City == enteredCity))
            {
             
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {

                        summariesOneCity.Add(item);
                    }));
                
            }
        }
        private void SearchHighestSalaryReq()
        {

               var result   = summaries.AsParallel().OrderByDescending(s => s.SalaryRequirements).FirstOrDefault();

            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                HighestSalaryReq = result;
            }));

        }
        private void SearchLowestSalaryReq()
        {
              var result=  summaries.AsParallel().OrderBy(s => s.SalaryRequirements).FirstOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LowestSalaryReq = result;
            }));
            

        }
        private void SearchMostExc()
        {
            var result=   summaries.AsParallel().OrderByDescending(s => s.Excperience).FirstOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MostExc = result;
            }));

        }
        private void SearchLeastExc()
        {
              var result= summaries.AsParallel().OrderBy(s => s.Excperience).FirstOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LeastExc = result;


            }));

        }
        private void folderButton_Click(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = fbd.SelectedPath;
                Path = FolderPath;
                DirectoryInfo di = new DirectoryInfo(FolderPath);
                Task.Factory.StartNew(() => Parallel.ForEach(di.GetFiles("*.txt"), AddSummary));
            }

        }
        private void AddSummary(FileInfo fi)
        {
            string path = fi.FullName;

            Summary s = new Summary() { FilePath = path };
            using (StreamReader sr = new StreamReader(path))
            {

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains("Full name: "))
                        s.FullName = line.Remove(0, "Full name: ".Length);
                    else if (line.Contains("City: "))
                        s.City = line.Remove(0, "City: ".Length);
                    else if (line.Contains("Skills: "))
                        s.Skills = line.Remove(0, "Skills: ".Length);
                    else if (line.Contains("Excperience:"))
                    {
                        line = line.Remove(0, "Excperience: ".Length);
                        if (int.TryParse(line, out int parse))
                            s.Excperience = parse;
                    }
                    else if (line.Contains("Salary requirements: "))
                    {
                        line = line.Remove(0, "Salary requirements: ".Length);
                        if (int.TryParse(line, out int parse))
                            s.SalaryRequirements = parse;
                    }
                }

                lock (this)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        summaries.Add(s);

                    }));
                }
                sr.Close();

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedMostExp)
                Task.Factory.StartNew(() => Parallel.Invoke(SearchMostExc));
           
            if (IsSelectedMostInexp)
                Task.Factory.StartNew(() => Parallel.Invoke(SearchLeastExc));
         

            if (IsSelectedHighSalaryReq)
                Task.Factory.StartNew(() => Parallel.Invoke(SearchHighestSalaryReq));
         

            if (IsSelectedLowestSalaryReq)
                Task.Factory.StartNew(() => Parallel.Invoke(SearchLowestSalaryReq));
         

            if (IsSelectedSameCity)
                Task.Factory.StartNew(() => Parallel.Invoke(SearchApplOneCity));
          

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
