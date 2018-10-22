using CredentialManagement;

namespace SalesInfoCall
{
    internal class CredentialsRepository
    {

        private const string PasswordName = "ServerPassword";
        private const string Username = "ServerUsername";

        public void SavePassword(string password)
        {
            using (var cred = new Credential())
            {
                cred.Password = password;
                cred.Target = PasswordName;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }

        public string GetPassword()
        {
            using (var cred = new Credential())
            {
                cred.Target = PasswordName;
                cred.Load();
                return cred.Password;
            }
        }

        public void ClearPassword()
        {
            using (var cred = new Credential())
            {
                cred.Target = PasswordName;
                cred.Delete();
            }
        }


        public void SaveUsername(string username)
        {
            using (var cred = new Credential())
            {
                cred.Username = username;
                cred.Target = Username;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }

        public string GetUsername()
        {
            using (var cred = new Credential())
            {
                cred.Target = Username;
                cred.Load();
                return cred.Username;
            }
        }

        public void ClearUsername()
        {
            using (var cred = new Credential())
            {
                cred.Target = Username;
                cred.Delete();
            }
        }
    }
}