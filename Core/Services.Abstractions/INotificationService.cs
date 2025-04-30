
namespace Services.Abstractions
{
    public interface INotificationService
    {
        Task SendToEngineerAsync(string title, string body);
        Task SendToTechnicianAsync(string technicianId, string title, string body);
    }
}
