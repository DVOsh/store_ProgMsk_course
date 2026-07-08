using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Messages
{
    public interface INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code);

        void StartProcess(Order order);
    }
}
