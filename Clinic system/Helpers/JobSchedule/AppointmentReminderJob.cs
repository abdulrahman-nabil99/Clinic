using Clinic_system.Services;
using Quartz;
using System;
using System.Diagnostics;
using System.Linq;

namespace Clinic_system.Helpers.JobSchedule
{
    public class AppointmentReminderJob : IJob
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentReminderJob(IAppointmentService _appointmentService)
        {
            appointmentService = _appointmentService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var tomorrow = DateTime.Now.AddDays(1).Date;

                var bookings = await appointmentService.GetAppointmentToNotifyAsync(tomorrow);
                if (bookings is not null)
                {
                    foreach (var booking in bookings)
                    {
                        // Send notification (via email or SMS)
                        await EmailHelper.SendEmailAsync(booking.Patient.Email, "تذكير بالحجز", "لديك حجز موعد غدا"+booking.AppointmentDate.Date);

                        // Update NotificationSent flag
                        await appointmentService.Notify(booking);
                    }
                }
            }

            catch (Exception ex)
            { 
                // Log the exception to the console or your logging system
                Console.WriteLine($"Error in AppointmentReminderJob: {ex.Message}");
                throw; // Ensure Quartz knows about the failure
            }
        }
    }
}
