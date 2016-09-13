using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Xml;

namespace BankProgram
{
    class Customer
    {
        public Customer()
        { 
            //Make sure Customer PIN number intializes to 4 digits when creating new account.
            pin = new char[4];

            //Assign random PIN number when creating new account. Customer can change later.
            System.Random rand = new System.Random();

            //Ensure the PIN number digits are between 0 and 9
            for(int iii = 0; iii < 4; iii++)
                pin[iii] = (char)(rand.Next() % 9);

            //Restrict zip code to five digits
            zip = new char[5];

            for (int iii = 0; iii < 5; iii++)
                zip[iii] = (char)(0);

            //Restrict phone numbers to 10 digits
            primaryPhone = new char[10];
            secondaryPhone = new char[10];

            for(int iii = 0; iii < 10; iii++)
            {
                primaryPhone[iii] = (char)(0);
                secondaryPhone[iii] = (char)(0);
            }

            //Necessary SQL connection info is in system_info.xml. Manually change source code for your own MySQL Server, or create your own "system_info.xml" file
            xDoc.Load("../../system_info.xml");
            string server = xDoc.SelectSingleNode("system_info/server").InnerText;
            string userID = xDoc.SelectSingleNode("system_info/userID").InnerText;
            string database = xDoc.SelectSingleNode("system_info/database").InnerText;
            string port = xDoc.SelectSingleNode("system_info/port").InnerText;
            string password = xDoc.SelectSingleNode("system_info/password").InnerText;
            
            string connectionInput = string.Format(
                "server={0}; " +
                "user={1}; " +
                "database={2}; " +
                "port={3}; " +
                "password={4}; ", server, userID, database, port, password
                );

            MySqlConnection mySql = new MySqlConnection(connectionInput);
            try
            {
                mySql.Open();
            }
            catch(Exception e)
            {
                switch (e.HResult)
                {
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again.");
                        break;
                    default:
                        Console.WriteLine("Error code: {0}. {1}", e.HResult, e.Message);
                        break;
                }
                Console.ReadLine();
            }

            //Can only create table once, or exception is thrown. Catch to avoid error / crash
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "create table customer_accounts " +
                    "(id int unsigned auto_increment," +
                    "balance decimal(17,2)," +
                    "account_status varchar(255)," +
                    "pin varchar(4)," +
                    "first_name varchar(255)," +
                    "last_name varchar(255)," +
                    "street_number int unsigned," +
                    "street_address varchar(255)," +
                    "city varchar(255)," +
                    "state varchar(2)," +
                    "zip varchar(5)," +
                    "primary_phone varchar(12)," +
                    "secondary_phone varchar(12)," +
                    "email varchar(255)," +
                    "primary key (id));", mySql
                    );

                cmd.ExecuteNonQuery();
            } 
            catch(MySqlException e)
            {
                //Error code 1050 means the table already exists. Ignore if this is the error
                if (e.Number != 1050)
                {
                    Console.WriteLine(e.Message);
                    throw (e);
                }                   
            }

            id = nextUniqueID;
            nextUniqueID++;
        }

        static int nextUniqueID; //The static unique ID ensures all customer accounts have a different ID. This will increment after each customer account, and be stored on the SQL server.

        int id;
        int balance; //Customer balance. If negative, account status should be set to OVER_DRAWN
        public ACCOUNT_STATUS accountStatus;
        char[] pin; //Customer PIN. Number can only be 4 digits
        string firstName;
        string lastName;
        int streetNumber;
        string streetAddress;
        string city;
        public STATE state;
        char[] zip;
        char[] primaryPhone;
        char[] secondaryPhone;
        string email;
        public enum ACCOUNT_STATUS {ACTIVE, OVER_DRAWN, FROZEN, UNDER_REVIEW, CLOSED };

        public enum STATE
        {
            AL, AK, AZ, AR, CA,
            CO, CT, DE, FL, GA,
            HI, ID, IL, IN, IA,
            KS, KY, LA, ME, MD,
            MA, MI, MN, MS, MO,
            MT, NE, NV, NH, NJ,
            NM, NY, NC, ND, OH,
            OK, OR, PA, RI, SC,
            SD, TN, TX, UT, VT,
            VA, WA, WV, WI, WY
        };

        
        
        XmlDocument xDoc = new XmlDocument();
        XmlNodeList xNL;
        XmlNode xNode;
    }

}
