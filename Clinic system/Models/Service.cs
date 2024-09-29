using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceImage { get; set; } = "default.jpg";
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
