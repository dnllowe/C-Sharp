using System;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace BankProgram
{
    class CreateAccountScene : Scene
    {
        public CreateAccountScene()
        {
            isRunning = true;

            //Update based on strings.xml root structure
            xmlRootNode += "create_account/";
        }

        public override void RunScene(float framesPerSecond)
        {
            try
            {
                MySqlHelper.ConnectToMySql();
            }
            catch(MySqlException e)
            {
                Console.WriteLine("MySQL Failed Connection in CreatAccountScene.cs");
                throw;
            }

            //Can only create table once, or exception is thrown. Catch to avoid error / crash
            try
            {
                MySqlHelper.ExecuteNonQueryCommand(
                    "create table customer_accounts " +
                    "(id int unsigned auto_increment," +
                    "balance decimal(17,2)," +
                    "account_status varchar(255)," +
                    "pin varchar(4)," +
                    "username varchar(255)," +
                    "password varchar(255)," +
                    "first_name varchar(255)," +
                    "last_name varchar(255)," +
                    "street_address varchar(255)," +
                    "city varchar(255)," +
                    "state varchar(2)," +
                    "zip varchar(5)," +
                    "primary_phone varchar(12)," +
                    "secondary_phone varchar(12)," +
                    "email varchar(255)," +
                    "primary key (id));"
                    );
            }

            catch (MySqlException e)
            {
                //Error code 1050 means the table already exists. Ignore if this is the error
                if (e.Number != 1050)
                {
                    Console.WriteLine(e.Message);
                    throw (e);
                }
            }

            MySqlDataReader reader = null;
            try
            {
                reader = MySqlHelper.ExecuteQueryCommand("select distinct 'username' from customer_accounts;");
            }
            catch(MySqlException e)
            {
                Console.WriteLine("MySQL failed to exectute command in CreateAccountScene.cs. Error: " + e.Message);
                throw;
            }

            List<string> takenUsernames = new List<string> { };

            //Get all usernames in the customer_account table to make sure chosen username isn't already taken
            while(reader.Read())
                takenUsernames.Add(reader.GetString("username"));
            reader.Close();

            string username;
            string firstName;
            string lastName;
            string streetAddress;
            string city;
            string state;
            string zip;
            string email;
            string primaryPhone;
            string secondaryPhone;

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_username"));

            bool usernameTaken = false;

            while (!GetStringInput(out username) || usernameTaken)
            {
                //Reset before checking
                usernameTaken = false;

                //User left field blank
                if (username == "")
                {
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("error"));
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("enter_username"));
                }

                else if(takenUsernames.Contains(username))
                {
                    usernameTaken = true;
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("username_taken"));
                }
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_first_name"));

            while(!GetStringInput(out firstName))
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("error"));
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_first_name"));
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_last_name"));

            while (!GetStringInput(out lastName))
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("error"));
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_last_name"));
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_street_address"));

            while (!GetStringInput(out streetAddress))
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("error"));
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_street_address"));
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_city"));

            while (!GetStringInput(out city))
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("error"));
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_city"));
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_state"));

            
            while (!GetStringInput(out state) || !stateAbbreviations.Contains(state.ToUpper()) )
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("invalid_state"));
            }

            //After getting valid state abbreviation, we want all state abbreviations to be uppercase
            state = state.ToUpper();

            bool isZipValid = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_zip"));
                GetStringInput(out zip);

                int intCheck;

                //Make sure phone number is 5 numerical digits
                if (zip.Length == 5)
                {
                    if (int.TryParse(zip, out intCheck))
                        isZipValid = true;
                    else
                    {
                        isZipValid = false;
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("invalid_zip"));
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("invalid_zip"));
                }
            }
            while (!isZipValid);

            bool isEmailValid = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_email"));
                GetStringInput(out email);

                //Logically, for email to be valid
                //1. It must contain both "@" and ".com"
                //2. There must be characters before "@". 
                //3. There must be characters between "@" and ".com"
                //4. It must end with ".com"
                if (email.Contains("@") && email.Contains(".com"))
                {
                    bool charactersBeforeAt = false;
                    bool charactersBetweenAtandDotCom = false;
                    bool endsWithDotCom = false;

                    int positionOfAt = email.IndexOf('@');
                    int positionOfDotCom = email.IndexOf(".com");

                    if (positionOfAt > 0)
                        charactersBeforeAt = true;
                    if (positionOfDotCom - positionOfAt > 1)
                        charactersBetweenAtandDotCom = true;
                    endsWithDotCom = email.EndsWith(".com");

                    if (charactersBeforeAt && charactersBetweenAtandDotCom && endsWithDotCom)
                        isEmailValid = true;
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("invalid_email"));
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("invalid_email"));
                }

            }
            while (!isEmailValid);


            bool isPhoneNumberValid = false;

            //Use to find only numeric values in phone number
            Regex regexNumerals = new Regex(@"[0-9]");

            do
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_primary_phone"));
                GetStringInput(out primaryPhone);    
                
                //Gets only characters included in the match pattern   
                MatchCollection validChars = regexNumerals.Matches(primaryPhone);
 
                //First add all valid characters to beginning of string...
                for (int iii = 0; iii < validChars.Count; iii++)
                    primaryPhone = primaryPhone.Insert(iii, validChars[iii].ToString());

                //Now get rid of whatever the original string was to get string of validChars
                if (primaryPhone.Length > validChars.Count)
                    primaryPhone = primaryPhone.Remove(validChars.Count);

                //Make sure phone number is 10 numerical digits
                if (primaryPhone.Length == 10)
                {
                    //Make sure "555" isn't in area code or first 3 digits of phone number
                    char[] areaCodeCheck = new char[3];
                    char[] digitCheck = new char[3];
                    primaryPhone.CopyTo(0, areaCodeCheck, 0, 3);
                    primaryPhone.CopyTo(3, digitCheck, 0, 3);

                    if (!areaCodeCheck.ToString().Contains("555") && !digitCheck.ToString().Contains("555"))
                        isPhoneNumberValid = true;
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("invalid_phone"));
                    }
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("invalid_phone"));
                }
            }
            while (!isPhoneNumberValid);
           

            //Reset for next phonenumber validation
            isPhoneNumberValid = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_secondary_phone"));
                GetStringInput(out secondaryPhone);

                //No input is a valid input because a secondary phone number is optional
                if (secondaryPhone.Length == 0)
                    isPhoneNumberValid = true;

                else if (secondaryPhone.Length > 0)
                {
                    //Gets only characters included in the match pattern   
                    MatchCollection validChars = regexNumerals.Matches(secondaryPhone);

                    //First add all valid characters to beginning of string...
                    for (int iii = 0; iii < validChars.Count; iii++)
                        secondaryPhone = secondaryPhone.Insert(iii, validChars[iii].ToString());

                    //Now get rid of whatever the original string was to get string of validChars
                    if (secondaryPhone.Length > validChars.Count)
                        secondaryPhone = secondaryPhone.Remove(validChars.Count);

                    //Make sure phone number is 10 numerical digits or was left empty
                    if (secondaryPhone.Length == 10)
                    {
                        //Make sure "555" isn't in area code or first 3 digits of phone number
                        char[] areaCodeCheck = new char[3];
                        char[] digitCheck = new char[3];
                        secondaryPhone.CopyTo(0, areaCodeCheck, 0, 3);
                        secondaryPhone.CopyTo(3, digitCheck, 0, 3);

                        if (!areaCodeCheck.ToString().Contains("555") && !digitCheck.ToString().Contains("555"))
                            isPhoneNumberValid = true;
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("invalid_phone"));
                        }

                    }

                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("invalid_phone"));
                    }
                }
               
                
            }
            while (!isPhoneNumberValid);

            //Create list of instructions for MySQL
            List<string> instructions = new List<string>
            {
                "insert into customer_accounts ",
                "set(username, first_name, last_name, ",
                "street_address, city, state, zip, email, primary_phone, secondary_phone) ",
                string.Format("values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9});", username, firstName, lastName, 
                    streetAddress, city, state, zip, email, primaryPhone, secondaryPhone)
            };

            return;

        }

        void CreateCustomerAccount(string username, string firstName, string lastName, string streetAddress, string city, string state,
                 string zip, string primaryPhone, string secondaryPhone, string email)
        {
            /* Inputs should be checked for validity in the program prior to creating a new customer account */

            System.Random rand = new System.Random();

            //Create a random, 10 digit password
            char[] password = new char[10];

            for(int iii = 0; iii < password.Length; iii++)
            {
                int toggle = rand.Next(0, 1);

                //Assign character
                if (toggle == 0)
                {
                    //Convert int to unicode string. Convert unicode string to char array. Get first element of array (the character).
                    char nextCharacter = char.ConvertFromUtf32(rand.Next(65, 90)).ToCharArray(0, 1)[0]; //Characters for A to Z
                    toggle = rand.Next(0, 1);

                    //Decide if upper or lower case
                    if (toggle == 0)
                        password[iii] = nextCharacter;
                    else if (toggle == 1)
                    {
                        //Convert character into its unicode integer. Add 32 to uppercase it.
                        int characterAsInt = char.ConvertToUtf32(nextCharacter.ToString(), 0) + 32;

                        //Convert uppercased integer unicode value back into character.
                        nextCharacter = char.ConvertFromUtf32(characterAsInt).ToCharArray(0, 1)[0];

                        password[iii] = nextCharacter;
                    }
                }

                //Assign number
                else if (toggle == 1)
                    password[iii] = (char)(rand.Next(0, 9));

            }

            //Assign random PIN number when creating new account. Customer can change later.

            //Make sure Customer PIN number intializes to 4 digits when creating new account.
            char[] pin = new char[4];

            //Ensure the PIN number digits are between 0 and 9
            for (int iii = 0; iii < 4; iii++)
                pin[iii] = (char)(rand.Next(0, 9));

            try
            {
                MySqlHelper.ConnectToMySql();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("MySQL Failed Connection in CreateAccountScene.CreateCustomerAccount");
                throw (e);
            }

            MySqlHelper.ExecuteNonQueryCommand(
                string.Format("insert into customer_accounts " +
                "(username, password, balance, account_status, pin, first_name, last_name," +
                "street_address, city, state, zip," +
                "primary_phone, secondary_phone, email)" +
                "values ({0},{1},{2},{3},{4},{5},{6},{7}," +
                "{8},{9},{10},{11},{12};", username, password, 0.00, "ACTIVE", pin.ToString(), firstName,
                lastName, streetAddress, city, state.ToString(), zip.ToString(),
                primaryPhone, secondaryPhone, email)
                );
        }

        List<string> stateAbbreviations = new List<string>
        {
            "AL", "AK", "AZ", "AR", "CA",
            "CO", "CT", "DE", "FL", "GA",
            "HI", "ID", "IL", "IN", "IA",
            "KS", "KY", "LA', 'ME", "MD",
            "MA", "MI", "MN", "MS", "MO",
            "MT", "NE", "NV", "NH", "NJ",
            "NM", "NY", "NC", "ND", "OH",
            "OK", "OR", "PA", "RI", "SC",
            "SD", "TN", "TX", "UT", "VT",
            "VA", "WA", "WV", "WI", "WY"
        };
    }
}
