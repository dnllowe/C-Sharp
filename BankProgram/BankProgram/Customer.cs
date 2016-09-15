using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BankProgram
{
    class Customer
    {
        Customer(string firstNameInput, string lastNameInput, string streetAddressInput, string cityInput, STATE stateInput, 
                 string zipInput, string primaryPhoneInput, string secondaryPhoneInput, string emailInput)
        {
            //These inputs should be checked for validity in the program prior to creating a new customer object
            firstName = firstNameInput;
            lastName = lastNameInput;
            streetAddress = streetAddressInput;
            city = cityInput;
            state = stateInput;
            zip = zipInput.ToCharArray();
            email = emailInput;
            primaryPhone = primaryPhoneInput.ToCharArray();
            secondaryPhone = secondaryPhoneInput.ToCharArray();
            balance = 0.00f;
            
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

            try
            {
                MySqlHelper.ConnectToMySql();
            }
            catch(MySqlException e)
            {
                Console.WriteLine("MySQL Failed Connection in Customer.cs");
                throw (e);
            }

            MySqlHelper.ExecuteNonQueryCommand(
                string.Format("insert into customer_accounts " +
                "(balance, account_status, pin, first_name, last_name," +
                "street_address, city, state, zip," +
                "primary_phone, secondary_phone, email)" +
                "values ({0},{1},{2},{3},{4},{5},{6},{7}," +
                "{8},{9},{10},{11},{12};", 0.00, "ACTIVE", pin.ToString(), firstName,
                lastName, streetAddress, city, state.ToString(), zip.ToString(),
                primaryPhone, secondaryPhone, email));

            accountStatus = ACCOUNT_STATUS.ACTIVE;
            

            MySqlDataReader reader = MySqlHelper.ExecuteQueryCommand("select id from customer_accounts order by id desc limit 1;");
            id = reader.GetInt32("id");
            reader.Close();
        }

        int id;
        float balance; //Customer balance. If negative, account status should be set to OVER_DRAWN
        public ACCOUNT_STATUS accountStatus;
        char[] pin; //Customer PIN. Number can only be 4 digits
        string username;
        string password;
        string firstName;
        string lastName;
        string streetAddress;
        string city;
        public STATE state;
        char[] zip;
        string email;
        char[] primaryPhone;
        char[] secondaryPhone;
        
        

        
    }

}
