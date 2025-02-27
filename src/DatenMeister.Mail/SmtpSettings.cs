using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.Xmi;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DatenMeister.Mail
{
    public class SmtpLogic
    {
        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public MailboxAddress Sender
        {
            get;
            set;
        }

        public SmtpLogic()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            var loadedSettings = ConfigurationLoader.LoadSetting();
            var found = loadedSettings.elements().WhenMetaClassIs(
                    new MofObjectShadow(XmiProviderObject.NodeMetaClassPrefix + "smtp"))
                .OfType<IElement>();

            foreach (var element in found)
            {
                var host = element.getOrDefault<string>("host");
                if (host != null)
                {
                    Host = host;
                }

                var port = element.getOrDefault<int>("port");
                Port = port;

                var username = element.getOrDefault<string>("username");
                if (username != null)
                {
                    Username = username;
                }

                var password = element.getOrDefault<string>("password");
                if (password != null)
                {
                    Password = password;
                }

                var sender = element.getOrDefault<string>("sender");
                if (sender != null)
                {
                    Sender = MailboxAddress.Parse(sender);
                }
            }
        }

        public SmtpClient GetConnectedClient()
        {
            var smtp = new SmtpClient();
            smtp.Connect(Host, Port, SecureSocketOptions.StartTls);
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                smtp.Authenticate(Username, Password);   
            }
            
            return smtp;
        }
    }
}
