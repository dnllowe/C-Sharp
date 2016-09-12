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
            pin = new int[4];

            //Assign random PIN number when creating new account. Customer can change later.
            System.Random rand = new System.Random();

            //Ensure the PIN number digits are between 0 and 9
            for(int iii = 0; iii < 4; iii++)
                pin[iii] = rand.Next() % 9;

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

            id = nextUniqueID;
            nextUniqueID++;
        }

        static int nextUniqueID; //The static unique ID ensures all customer accounts have a different ID. This will increment after each customer account, and be stored on the SQL server.

        string firstName;
        string lastName;
        int streetNumber;
        string streetAddress;
        string city;
        int zip;
       
        int id;
        int[] pin; //Customer PIN. Number can only be 4 digits
        int balance; //Customer balance. If negative, account status should be set to OVER_DRAWN

        string primaryPhone;
        string secondaryPhone;
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

        public ACCOUNT_STATUS accountStatus;
        public STATE state;
        XmlDocument xDoc = new XmlDocument();
        XmlNodeList xNL;
        XmlNode xNode;
    }

}
