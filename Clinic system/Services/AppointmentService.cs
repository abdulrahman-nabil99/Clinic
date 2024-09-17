using Clinic_system.Data;
using Clinic_system.Models;
using Clinic_system.ViewModels;

namespace Clinic_system.Services
{
    public class AppointmentService : GenericService<Appointment>, IAppointmentService
    {
        public AppointmentService(ClinicdbContext _context) : base(_context)
        {
        }

        public async Task<Appointment> CreateAppointmentAsync(BookViewModel model, Patient patient)
        {
            var appointment = new Appointment
            {
                PatientId = patient.PatientId,
                AppointmentDate = model.Date,
                ServiceId = model.ServiceId
            };
            await AddAsync(appointment);
            return appointment;
        }
        public async Task<int> GetAppointmentOrderForDayAsync(DateTime date)
        {
            var dayAppointments = await GetAllAsync();
            return dayAppointments
                .Where(a => a.AppointmentDate.Date == date && a.Status == AppointmentStatus.Booked)
                .Count();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentToNotifyAsync(DateTime date)
        {
            var dayAppointments = await GetAllAsync();
            return dayAppointments
                .Where(a => a.AppointmentDate.Date == date && a.Status == AppointmentStatus.Booked && !a.NotificationSent);
        }

        public async Task Notify(Appointment appointment)
        {
            appointment.NotificationSent = true;
            await SaveChangesAsync();
        }
    }
}
