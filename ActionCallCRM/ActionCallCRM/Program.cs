using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SalesInfoCall 
{
    partial class SalesInfoCall
    {
        static void Main(string[] args)
        {
            while (true)
            {
               
                CredentialsRepository userCredentials = new CredentialsRepository();
                if (userCredentials.GetUsername() == string.Empty || userCredentials.GetPassword() == string.Empty)
                {
                    Console.WriteLine("Enter your Username: ");
                    userCredentials.SaveUsername(Console.ReadLine());
                    string password;
                    do
                    {
                        password = Console.ReadKey(true);

                        // Backspace Should Not Work
                        if (key.Key != ConsoleKey.Backspace)
                        {
                            pass += key.KeyChar;
                            Console.Write("*");
                        }
                        else
                        {
                            Console.Write("\b");
                        }
                    }
                    Console.WriteLine("Enter your Password: ");
                    userCredentials.SavePassword(password);
                }

           //     Console.WriteLine(userData.GetUsername());
            //    Console.WriteLine(userData.GetPassword());

                string URL = string.Empty;
                string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
                string fileName = Path.Combine(path, "URL.txt");

                if (File.Exists(fileName) && File.ReadAllText(fileName).Length != 0)
                {
                    URL = File.ReadAllText(fileName);
                }
                else
                {
                //  File.WriteAllText(fileName, string.Empty);
                    Console.WriteLine("Enter your organization URL : ");
                    URL = Console.ReadLine();
                    File.WriteAllText(fileName, URL);
                }

            //  Console.WriteLine(URL);


                var credentials = new ClientCredentials();
                credentials.UserName.UserName = userCredentials.GetUsername();
                credentials.UserName.Password = userCredentials.GetPassword();

               // Console.WriteLine(URL);
            //    Console.WriteLine(credentials.UserName.UserName);
              //  Console.WriteLine(credentials.UserName.Password);
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Uri OrganizationUri = new Uri(URL);

                    using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, null, credentials, null))
                    {
                        IOrganizationService service = (IOrganizationService)serviceProxy;
                        //Console.WriteLine("ancav!");
                        OrganizationRequest request = new OrganizationRequest("new_sales_info_retrieval");

                        OrganizationResponse response = service.Execute(request);
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("Please Check connection, or try to enter your credentials again!");
                    userCredentials.ClearPassword();
                    userCredentials.ClearUsername();
                    File.WriteAllText(fileName, string.Empty);
                    continue;
                }

                break;
            }
        }
    }
}

            