namespace BinanceOptionsApp.Models
{
    using System;
    using System.Xml.Serialization;
    using System.Collections.ObjectModel;

    public class ProjectModel : BaseModel
    {
        public ProjectModel()
        {
            Inputs = new ObservableCollection<InputItemModel>();
            //Outputs = new ObservableCollection<OutputItemModel>();
        }

        private bool _Started;
        [XmlIgnore]
        public bool Started
        {
            get { return _Started; }
            set { if (_Started != value) { _Started = value; OnPropertyChanged(); } }
        }

        private bool _ViewSession;
        public bool ViewSession
        {
            get { return _ViewSession; }
            set { if (_ViewSession != value) { _ViewSession = value; OnPropertyChanged(); } }
        }
        private bool _SaveSession;
        public bool SaveSession
        {
            get { return _SaveSession; }
            set { if (_SaveSession != value) { _SaveSession = value; OnPropertyChanged(); } }
        }

        private ObservableCollection<InputItemModel> _Inputs;
        public ObservableCollection<InputItemModel> Inputs
        {
            get { return _Inputs; }
            set { if (_Inputs != value) { _Inputs = value; OnPropertyChanged(); } }
        }

        private DateTime _InputTime;
        [XmlIgnore]
        public DateTime InputTime
        {
            get { return _InputTime; }
            set { if (_InputTime != value) { _InputTime = value; OnPropertyChanged(); } }
        }


        //private ObservableCollection<OutputItemModel> _Outputs;
        //public ObservableCollection<OutputItemModel> Outputs
        //{
        //    get { return _Outputs; }
        //    set { if (_Outputs != value) { _Outputs = value; OnPropertyChanged(); } }
        //}
        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(ProjectModel), new Type[] { typeof(BaseModel), typeof(InputItemModel)/*, typeof(OutputItemModel) */});
        }
        public static ProjectModel Load(string filename)
        {
            ProjectModel res = null;
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                {
                    var xs = CreateSerializer();
                    res = xs.Deserialize(fs) as ProjectModel;
                }
            }
            catch
            {
                res = null;
            }
            if (res == null) res = new ProjectModel();
            if (res.Inputs == null) res.Inputs = new ObservableCollection<InputItemModel>();
            //if (res.Outputs == null) res.Outputs = new ObservableCollection<OutputItemModel>();
            //foreach (var output in res.Outputs)
            //{
            //    if (Model.ProcessorsNames.IndexOf(output.Name) < 0)
            //    {
            //        output.Name = Model.ProcessorsNames[0];
            //    }
            //}
            return res;
        }
        public void Save(string filename)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
                {
                    var xs = CreateSerializer();
                    xs.Serialize(fs, this);
                }
            }
            catch
            {
            }
        }
    }
}
