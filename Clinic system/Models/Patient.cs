using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        // User Full name
        [Required(ErrorMessage = "* Required")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Name Must be between 6 and 40 Characters.")]
        public string FullName { get; set; }

        // User Email
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        // User Phone number
        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        // User Records
        public virtual ICollection<MedicalRecord> MedicalHistory { get; set; } = new List<MedicalRecord>();

        // User Appointments 
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
