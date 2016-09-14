using System;
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

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_username"));
            Console.WriteLine();

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
                    Console.WriteLine("enter_username");
                }

                else if(takenUsernames.Contains(username))
                {
                    usernameTaken = true;
                    Console.WriteLine();
                    Console.WriteLine("username_taken");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_first_name"));
            Console.WriteLine();

            while(!GetStringInput(out firstName))
            {
                Console.WriteLine();
                Console.WriteLine("error");
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_first_name"));
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_last_name"));
            Console.WriteLine();

            while (!GetStringInput(out lastName))
            {
                Console.WriteLine();
                Console.WriteLine("error");
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_last_name"));
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_street_address"));
            Console.WriteLine();

            while (!GetStringInput(out streetAddress))
            {
                Console.WriteLine();
                Console.WriteLine("error");
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_street_address"));
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_city"));
            Console.WriteLine();

            while (!GetStringInput(out city))
            {
                Console.WriteLine();
                Console.WriteLine("error");
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_city"));
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_state"));
            Console.WriteLine();

            
            while (!GetStringInput(out state) || !stateAbbreviations.Contains(state.ToUpper()) )
            {
                Console.WriteLine();
                Console.WriteLine(GetXmlText("error"));
                Console.WriteLine();
                Console.WriteLine(GetXmlText("enter_state"));
                Console.WriteLine();
            }

            //After getting valid state abbreviation, we want all state abbreviations to be uppercase
            state = state.ToUpper();

            return;

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
