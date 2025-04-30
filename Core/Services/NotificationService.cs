
using FirebaseAdmin.Messaging;

namespace Services
{
    public class NotificationService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager
        ) : INotificationService
    {
        public async Task SendToEngineerAsync(string title, string body)
        {
            var engineerRepo = unitOfWork.GetRepository<Engineer, string>();
            var engineers = await engineerRepo.GetAllWithIncludesAsync(e => true, e => e.ApplicationUser);

            foreach (var engineer in engineers)
            {
                var fcmToken = engineer.ApplicationUser?.FcmToken;
                if (!string.IsNullOrEmpty(fcmToken))
                {
                    var message = new Message()
                    {
                        Token = fcmToken,
                        Notification = new Notification()
                        {
                            Title = title,
                            Body = body
                        }
                    };
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                }
            }
        }

        public async Task SendToTechnicianAsync(string technicianAppUserId, string title, string body)
        {
            var technician = await userManager.FindByIdAsync(technicianAppUserId);

            if (technician != null)
            {
                var fcmToken = technician.FcmToken;
                if (!string.IsNullOrEmpty(fcmToken))
                {
                    var message = new Message()
                    {
                        Token = fcmToken,
                        Notification = new Notification()
                        {
                            Title = title,
                            Body = body
                        }
                    };
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                }
            }
        }
    }

}
