using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Messages
{
    public interface INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code);

        Task SendConfirmationCodeAsync(string formattedPhone, int confirmationCode);

        void StartProcess(Order order);

        Task StartProcessAsync(Order order);
    }
}
