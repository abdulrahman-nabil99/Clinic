using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
         // img , description
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
