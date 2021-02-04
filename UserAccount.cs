using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;


namespace Budgeting_Course_Work
{
    [Serializable]
    class UserAccount
    {

        private string firstName;
        private string surName;
        private string dateOfBirth;
        private string email;
        private string userName;
        private string password;
        private decimal balance;


        private UserAccount(string aFirstName, string aSurname, string aEmail, string aDateOfBirth, string aPassword, string aUsername, decimal aBalance)
        {
            firstName = aFirstName;
            surName = aSurname;
            email = aEmail;
            dateOfBirth = aDateOfBirth;
            userName = aUsername;
            password = aPassword;
            balance = aBalance;
        }



        public static void addAccount(Dictionary<string, UserAccount> newAccount)
        {

            string firstName = firstNameChecker("Please insert your first name: ");
            string surName = surNameChecker("Please insert your surname: ");
            string email = emailChecker("Please insert your email: ");
            string dateOfBirth = DateOfBirth("Please insert your date of birth (DD/MM/YYYY): ");
            string password = passwordChecker("Please insert your password: ");
            string userName = firstName + surName;
            decimal balance = decimalChecker("Please insert your Balance: ");


            newAccount[userName] = new UserAccount(firstName, surName, email, dateOfBirth, password, userName, balance);
            Console.WriteLine("Your account has been created, Your Username is you first name and your surname.");

            // Saves the user's information in a binary format
            saveAccount(newAccount, userName);


        }



        public static string stringChecker(string message)
        {
            Console.WriteLine(message);
            string value = Console.ReadLine();

            while (value == " ")
            {
                Console.Clear();
                Console.WriteLine("Invalid input, " + message);
                value = Console.ReadLine();

            }

            Console.Clear();
            return value;
        }

        public static int intChecker(string message, int min, int max)
        {

            string userInput = stringChecker(message);
            int numberFromInput;
            bool isNumber = int.TryParse(userInput, out numberFromInput);
            
            while (isNumber == false || numberFromInput < min || numberFromInput > max)
            {
                userInput = stringChecker("Invalid input, " + message);
                isNumber = int.TryParse(userInput, out numberFromInput);
            }

            return numberFromInput;
        }

        public static decimal decimalChecker(string message)
        {
            string userInput = stringChecker(message);
            decimal numberFromInput;
            bool isNumber = decimal.TryParse(userInput, out numberFromInput);

            while (isNumber == false)
            {
                Program.mainMenu();
                userInput = stringChecker("Invalid input, " + message);
                isNumber = decimal.TryParse(userInput, out numberFromInput);
            }

            return numberFromInput;
        }

        public static char charChecker(string message, char y, char n)
        {
            Console.WriteLine(message);
            char exit = Console.ReadKey().KeyChar;
            Console.ReadLine();

            while (exit != y && exit != n)
            {
                Console.Clear();
                Console.WriteLine("Invalid Input, try again: \n" + message);
                exit = Console.ReadKey().KeyChar;
                Console.ReadLine();

            }

            return exit;
        }

        private static string firstNameChecker(string message)
        {
            Console.Clear();
            bool incorrectInput = false;
            string value = stringChecker(message);

            do
            {
                for (int i = 0; i < value.Length; i++)
                {

                    if (Char.IsLetter(value[i]))
                    {

                        incorrectInput = true;

                    }
                    else
                    {
                        Console.WriteLine("Invalid input, first name may only contain letters.");
                        value = stringChecker("\n" + message);
                    }

                }

            } while (incorrectInput == false);

            return value;
        }

        private static string surNameChecker(string message)
        {
            Console.Clear();
            bool incorrectInput = false;
            string value = stringChecker(message);

            do
            {
                for (int i = 0; i < value.Length; i++)
                {

                    if (Char.IsLetter(value[i]))
                    {
                        incorrectInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, surname may only contain letters.");
                        value = stringChecker("\n" + message);
                    }

                }

            } while (incorrectInput == false);

            return value;
        }

        private static string emailChecker(string message)
        {
            bool incorrectInput;
            string emailCheck = stringChecker(message);

            do
            {
                //Code dapted from StackOverFlow Lines 192 - 202 by Cogwheel
                try
                {
                    new MailAddress(emailCheck);
                    incorrectInput = true;
                }
                catch (Exception error)
                {

                    Console.WriteLine(error.Message);
                    incorrectInput = false;
                }

                if (incorrectInput == false)
                {
                    emailCheck = stringChecker("\n" + message);
                }


            } while (incorrectInput == false);


            return emailCheck;

        }

        private static string DateOfBirth(string message)
        {
            bool incorrectInput = false;
            string value = stringChecker(message);
            DateTime date;

            do
            {

                // Code dapted from StackOverFlow Line 226 - 236 by Stuart Frankish
                if (DateTime.TryParse(value, out date))
                {
                    incorrectInput = true;
                    value = date.ToString("dd/MM/yyyy");
                }

                else
                {
                    Console.WriteLine("Invalid input, date must be in \"DD/MM/YYYY\" format");
                    value = stringChecker("\n" + message);
                }

            } while (incorrectInput == false);



            return value;
        }

        private static string passwordChecker(string message)
        {

            Console.Clear();
            bool incorrectInput = false;
            string value = stringChecker(message);

            do
            {
                for (int i = 0; i < value.Length; i++)
                {

                    if (Char.IsLetterOrDigit(value[i]))
                    {
                        incorrectInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Password may only contain letters or digits.");
                        value = stringChecker("\n" + message);
                    }

                }

            } while (incorrectInput == false);
            return value;
        }




        private static void saveAccount(Dictionary<string, UserAccount> newAccount, string userName)
        {
            BinaryFormatter saveAccount = new BinaryFormatter();
            FileStream accountToBeSaved = File.Create(userName + ".bin");
            saveAccount.Serialize(accountToBeSaved, newAccount);
            accountToBeSaved.Close();

        }

        public static Dictionary<string, UserAccount> accountLogin(Dictionary<string, UserAccount> account)
        {
            string userName = stringChecker("Insert Username: ");
            account = loadAccounts(userName);

            //Checks to see if the user name exist as a binary file and opens it.
            while (account == null || account.ContainsKey(userName) == false && userName != "exit")
            {
                userName = stringChecker("Account does not exist, Try again: \nOr type exit to go back");
                account = loadAccounts(userName);
            }


            if (userName == "exit")
            {
                return null;
            }

            // verify the password set by the user.
            account = accountPassword(account, userName);

            // If the account has been verified
            if (account != null)
            {
                accountMenuDiplay(account, userName);
            }


            return account;

        }

        //Loads the Binary file if user name matches the name of the file
        private static Dictionary<string, UserAccount> loadAccounts(string userName)
        {
            string file = userName + ".bin";
            BinaryFormatter loadAccount;
            FileStream accountToLoad;
            Dictionary<string, UserAccount> userAccount;


            if (File.Exists(file) == true)
            {
                loadAccount = new BinaryFormatter();
                accountToLoad = File.OpenRead(file);
                userAccount = (Dictionary<string, UserAccount>)loadAccount.Deserialize(accountToLoad);

            }

            else
            {
                return userAccount = null;
            }



            accountToLoad.Close();



            return userAccount;
        }

        private static Dictionary<string, UserAccount> accountPassword(Dictionary<string, UserAccount> listOfAccounts, string userName)
        {
            string accessPassword = passwordChecker("Please insert your password");
            int loggingAttempts = 3;



            while (listOfAccounts[userName].password != accessPassword && loggingAttempts > 0)
            {
                accessPassword = passwordChecker("Password incorrect.\nAttempts left: " + loggingAttempts + "\nPlease try again:");

                loggingAttempts--;
            }

            if (accessPassword == listOfAccounts[userName].password)
            {
                Console.WriteLine("Login Successfull");
            }

            else
            {
                Console.WriteLine("Login Fail");
                return null;
            }


            Console.ReadLine();
            Console.Clear();
            return listOfAccounts;
        }



        // Display account menu
        public static void accountMenuDiplay(Dictionary<string, UserAccount> account, string userName)
        {


            char exit;

            do
            {
                account = loadAccounts(userName);
                Console.Clear();


                if (accountMenu(account, userName) == false)
                {
                    return;
                }


                int userOption = intChecker($"Please select one of the options between {0} and {3}: ", 0, 3);


                switch (userOption)
                {
                    case 0:
                        profile(account, userName);
                        break;

                    case 1:
                        checkBalance(account, userName);
                        break;

                    case 2:
                        account = addTransaction(account, userName);
                        break;
                    //Exit Program
                    case 3:
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }


                exit = charChecker("Would you like to sign out? (y / n)", 'y', 'n');

            } while (exit == 'n');
        }

        private static void profile(Dictionary<string, UserAccount> account, string userName)
        {
            Console.WriteLine($"First Name: {account[userName].firstName} \nSurname: {account[userName].surName} \nUser Name: {account[userName].userName} \nemail: {account[userName].email} \nDate of Birth: {account[userName].dateOfBirth}");
        }

        private static void checkBalance(Dictionary<string, UserAccount> account, string userName)
        {
            Console.WriteLine($"Your balance is: £{account[userName].balance}\n");

            char statement = charChecker("Would you like to see your statement? (y / n)", 'y', 'n');

            if (statement == 'y')
            {
                loadStatement(userName);
            }

        }

        
        private static Dictionary<string, UserAccount> addTransaction(Dictionary<string, UserAccount> account, string userName)
        {
            
            char[] typeOfTransaction = new char[] { '+', '-' };
            char exit;
            decimal transaction;
            char userChoice = charChecker($"Press: {typeOfTransaction[0]} for an income. \nPress: {typeOfTransaction[1]} for an outcome.\n", '+', '-');

            //Creates a text file to save as statement.
            StreamWriter statementWritter = new StreamWriter(userName + " statement.txt", append: true);
            statementWritter.WriteLine($"[{DateTime.Now}]" + $"   Your Current balance is of: £{account[userName].balance}");


            do
            {
                // Add an amount
                if (userChoice == typeOfTransaction[0])
                {
                    transaction = decimalChecker("Please insert the amount of the transaction in £: £");
                    statementWritter.WriteLine($"\nTransaction amount: £{transaction}" + "\n+\n" + $"Old balance: £{account[userName].balance}\n");
                    account[userName].balance = account[userName].balance + transaction;
                    statementWritter.WriteLine($"[{DateTime.Now}]" + $"  Your new balance is of: £{account[userName].balance}\n");
                    statementWritter.Close();

                    Console.WriteLine($"[{DateTime.Now}]" + $"  Your new balace is of: £{account[userName].balance}\n");

                }

                // Subtract 
                else if (userChoice == typeOfTransaction[1])
                {
                    transaction = decimalChecker("Please insert the amount of the transaction in £: ");
                    statementWritter.WriteLine($"Transaction amount: £{transaction}" + "\n-\n" + $"Old balance: £{account[userName].balance}\n");
                    account[userName].balance = account[userName].balance - transaction;
                    statementWritter.WriteLine($"[{DateTime.Now}]" + $"  Your new balance is of: £{account[userName].balance}\n");
                    statementWritter.Close();

                    Console.WriteLine($"[{DateTime.Now}]" + $"  Your new balace is of: £{account[userName].balance}\n");
                }


                exit = charChecker("Would you like to add a new transaction? y/n", 'y', 'n');

            } while (exit == 'y');


            // updates the binary file with the new amount
            saveAccount(account, userName);

            Console.Clear();
            return account;

        }


        //Load the configurations for the Account menu to be displayed
        private static bool accountMenu(Dictionary<string, UserAccount> account, string userName)
        {
            const string configurationAccountMenuFile = "Account Menu File.txt";
            Dictionary<string, int> configurationAccountMenuFunctions = new Dictionary<string, int>();




            if (File.Exists(configurationAccountMenuFile) == true)
            {
                string[] configuration = File.ReadAllLines(configurationAccountMenuFile);



                // Pass the whole line from the file to be split into multiple cells
                for (int i = 0; i < configuration.Length; i++)
                {
                    string[] configurationLines = configuration[i].Split(':');
                    configurationAccountMenuFunctions.Add(configurationLines[0], int.Parse(configurationLines[1]));
                }
            }

            else
            {
                Console.WriteLine("There has been a problem loading the configuration file");
                return false;
            }


            //If file exits the section below will be displayed
            List<string> configurationAccountMenuIndex = new List<string>(configurationAccountMenuFunctions.Keys);

            Console.WriteLine($"Welcome {account[userName].firstName}\n");

            for (int i = 0; i < configurationAccountMenuFunctions.Count; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine($"[{configurationAccountMenuFunctions["Profile"]}] {configurationAccountMenuIndex[i]}");
                }

                else if (i == 1)
                {
                    Console.WriteLine($"[{configurationAccountMenuFunctions["Check Balance"]}] {configurationAccountMenuIndex[i]}");
                }

                else if (i == 2)
                {
                    Console.WriteLine($"[{configurationAccountMenuFunctions["Add New Transaction"]}] {configurationAccountMenuIndex[i]}");
                }

                else if (i == 3)
                {
                    Console.WriteLine($"[{configurationAccountMenuFunctions["Exit"]}] {configurationAccountMenuIndex[i]}\n");
                }

            }

            return true;
        }

        //Reads the stament text file
        private static void loadStatement(string userName)
        {
            Console.Clear();
            string file = userName + " statement.txt";


            if (File.Exists(file) == true)
            {
                string[] statement = File.ReadAllLines(userName + " statement.txt");

                for (int i = 0; i < statement.Length; i++)
                {
                    Console.WriteLine(statement[i]);
                }
            }

            else
            {
                Console.WriteLine("There are no transaction to be displayed.");
                return;
            }


        }

    }
}
