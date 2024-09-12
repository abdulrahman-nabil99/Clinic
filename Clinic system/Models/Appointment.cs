﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_system.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        // Patient
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient? Patient { get; set; }

        // Date
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "* Required")]
        public DateTime AppointmentDate { get; set; }

        // Status
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;// ===> تم الكشف | تم الإلغاء

        // Medical Record
        public int? MedicalRecordId { get; set; }
        [ForeignKey("MedicalRecordId")]
        public virtual MedicalRecord? MedicalRecord { get; set; }

        // Service
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }
}
