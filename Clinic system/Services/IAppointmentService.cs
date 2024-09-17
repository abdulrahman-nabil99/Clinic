using Clinic_system.Models;
using Clinic_system.ViewModels;

namespace Clinic_system.Services
{
    public interface IAppointmentService : IGenericService<Appointment>
    {
        Task<Appointment> CreateAppointmentAsync(BookViewModel model, Patient patient);
        Task<int> GetAppointmentOrderForDayAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentToNotifyAsync(DateTime date);
        Task Notify(Appointment appointment);

    }
}
