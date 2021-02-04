using System;
using System.Collections.Generic;
using System.IO;



namespace Budgeting_Course_Work
{
    class Program
    {
        static void Main()
        {

            Dictionary<string, UserAccount> userList = new Dictionary<string, UserAccount>();

            try
            {
                loginOrCreateAccount(userList);

            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                Console.WriteLine($"\n\n{error.StackTrace}");
            }


            Console.WriteLine("\nEnd of the program!!!!");
        }

        //Main Menu
        static void loginOrCreateAccount(Dictionary<string, UserAccount> account)
        {
            char exit;

            do
            {
                Console.Clear();

                if (mainMenu() == false)
                {
                    return;
                }


                int userOption = UserAccount.intChecker($"Please select one of the options between {0} and {2}: ", 0, 2);

                //
                switch (userOption)
                {
                    case 0:

                        account = UserAccount.accountLogin(account);
                        break;

                    case 1:
                        UserAccount.addAccount(account);
                        break;
                    case 2:
                        // Exit program
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }


                exit = UserAccount.charChecker("Would you like to exit the program? (y / n)", 'y', 'n');

            } while (exit == 'n' || account == null);



        }


        public static bool mainMenu()
        {

            const string configurationFile = "Configurations.txt";
            Dictionary<string, int> configurationFunctions = new Dictionary<string, int>();


            if (File.Exists(configurationFile) == true)
            {
                string[] configuration = File.ReadAllLines(configurationFile);



                for (int i = 0; i < configuration.Length; i++)
                {
                    string[] configurationLines = configuration[i].Split(':');
                    configurationFunctions.Add(configurationLines[0], int.Parse(configurationLines[1]));
                }
            }

            else
            {
                Console.WriteLine("There has been a problem loading the configuration file");
                return false;
            }


            //Confinguation loaded will be displayed on the section below
            List<string> configurationIndex = new List<string>(configurationFunctions.Keys);

            Console.WriteLine("Welcome to NTU Budgeting System ");

            for (int i = 0; i < configurationFunctions.Count; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine($"[{configurationFunctions["Login"]}] {configurationIndex[i]}");
                }

                else if (i == 1)
                {
                    Console.WriteLine($"[{configurationFunctions["Create Account"]}] {configurationIndex[i]}");
                }

                else if (i == 2)
                {
                    Console.WriteLine($"[{configurationFunctions["Exit"]}] {configurationIndex[i]}\n");
                }

            }

            return true;
        }






    }

}
