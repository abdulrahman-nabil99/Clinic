using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_system.Models
{
    public class MedicalRecord
    {
        [Key]
        public int MedicalRecordId { get; set; }

        // Patient
        public int PatientId {  get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient? Patient { get; set; }

        // Date
        public DateTime RecordDate { get; set; } = DateTime.Now;

        // Diagnosis
        public string? Diagnosis { get; set; }

        // Treatment
        public string? Treatment { get; set; }

        // Appointment
        public int? AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }

    }
}
