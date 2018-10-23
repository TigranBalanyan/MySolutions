using System;
using System.IO;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace SalesInfoCall 
{
    class SalesInfoCall
    {
        static void Main(string[] args)
        {
            do
            {
                CredentialsRepository userCredentials = new CredentialsRepository();
                if (userCredentials.GetUsername() == string.Empty || userCredentials.GetPassword() == string.Empty)
                {
                    Console.WriteLine("Enter your Username: ");
                    userCredentials.SaveUsername(Console.ReadLine());

                    Console.WriteLine("Enter your Password: ");
                    string password = string.Empty;

                    do
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        // // Backspace Should Not Work
                        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                        {
                            password += key.KeyChar;
                            Console.Write("*");
                        }
                        else
                        {
                            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                            {
                                password.Substring(0, (password.Length - 1));
                                Console.Write("\b \b");
                            }
                            else if (key.Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                        }
                    } while (true);

                    userCredentials.SavePassword(password);
                    Console.WriteLine();
                }

                string URL = string.Empty;
                string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
                string fileName = Path.Combine(path, "URL.txt");

                if (File.Exists(fileName) && File.ReadAllText(fileName).Length != 0)
                {
                    URL = File.ReadAllText(fileName);
                }
                else
                {
                    Console.WriteLine("Enter your organization URL : ");
                    URL = Console.ReadLine();
                    File.WriteAllText(fileName, URL);
                }

                var credentials = new ClientCredentials();
                credentials.UserName.UserName = userCredentials.GetUsername();
                credentials.UserName.Password = userCredentials.GetPassword();

                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Uri OrganizationUri = new Uri(URL);

                    using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, null, credentials, null))
                    {
                        IOrganizationService service = (IOrganizationService)serviceProxy;
                        OrganizationRequest request = new OrganizationRequest("new_sales_info_retrieval");

                        OrganizationResponse response = service.Execute(request);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please Check connection, or try to enter your credentials again!");
                    userCredentials.ClearPassword();
                    userCredentials.ClearUsername();
                    File.WriteAllText(fileName, string.Empty);
                    continue;
                }
                break;

            } while (true);
        }
    }
}

            