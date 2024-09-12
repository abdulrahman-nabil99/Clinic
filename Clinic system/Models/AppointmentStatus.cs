using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public enum AppointmentStatus
    {
        [Display(Name = "تم الحجز")]
        Booked,

        [Display(Name = "تم الكشف")]
        Checked,

        [Display(Name = "تم الإلغاء")]
        Cancelled
    }
}
