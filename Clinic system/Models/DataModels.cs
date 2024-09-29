namespace Clinic_system.Models
{
    public class DataModel
    {

        public Social Social { get; set; }
        public List<string> Phones { get; set; }
        public string WorkingHours { get; set; }
        public string WorkingDays { get; set; }
        public List<string> Address { get; set; }
    }

    public class Social
    {
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
    }
}
