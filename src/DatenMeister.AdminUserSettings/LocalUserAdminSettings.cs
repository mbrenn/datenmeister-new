using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.Xmi;

namespace DatenMeister.AdminUserSettings
{
    public class LocalUserAdminSettings
    {
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt for the password
        /// </summary>
        public string Salt
        {
            get;
            set;
        }

        public string HashedPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value whether the user has a password set
        /// </summary>
        public bool HasPasswordSet => !string.IsNullOrEmpty(HashedPassword) || !string.IsNullOrEmpty(Salt);

        public LocalUserAdminSettings()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            var loadedSettings = ConfigurationLoader.LoadSetting();
            var found = loadedSettings.elements().WhenMetaClassIs(
                new MofObjectShadow(XmiProviderObject.NodeMetaClassPrefix + "admin"))
                .OfType<IElement>();

            foreach (var element in found)
            {
                var username= element.getOrDefault<string>("username");
                if (username != null)
                {
                    UserName = username;
                }

                var hashedPasswort = element.getOrDefault<string>("hashedPassword");
                if (hashedPasswort != null)
                {
                    HashedPassword = hashedPasswort;
                }

                var salt = element.getOrDefault<string>("salt");
                if (salt != null)
                {
                    Salt = salt;
                }
            }
        }

        /// <summary>
        /// Checks whether the password is correct
        /// </summary>d
        /// <param name="password">Password to be checked</param>
        /// <returns>true, if the password is corrected</returns>
        public bool IsPasswordCorrect(string username, string password)
        {
            if (!HasPasswordSet)
            {
                throw new InvalidOperationException("Password cannot be retrieved");
            }

            if (username != UserName)
            {
                return false;
            }

            var fullString = $"{username}{Salt}{password}";
            
            // Get Hash
            var hash = ComputeSha256Hash(fullString);
            return string.Compare(hash, HashedPassword, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
