using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BankProgram
{
    class Account
    {
        public bool LoadAccount(string usernameInput)
        {
            MySqlHelper.ConnectToMySql();
            MySqlDataReader reader = MySqlHelper.ExecuteQueryCommand(string.Format("select * from customer_accounts where username = {0};", username));
            bool accountExists = false;

            //If there are no rows, the record for the username does not exist.
            if (reader.HasRows)
                accountExists = true;
            else
                return false;

            reader.Read();
            id = reader.GetInt32("id");
            balance = reader.GetDecimal("balance");
            pin = reader.GetString("pin");
            username = usernameInput;
            password = reader.GetString("password");
            firstName = reader.GetString("first_name");
            lastName = reader.GetString("last_name");
            streetAddress = reader.GetString("street_address");
            city = reader.GetString("city");
            state = reader.GetString("state");
            zip = reader.GetString("zip");
            primaryPhone = reader.GetString("primary_phone");
            secondaryPhone = reader.GetString("secondary_phone");
            email = reader.GetString("email");
            reader.Close();

            return accountExists;
        }

        public decimal GetBalance()
        {
            return balance;
        }

        int id;
        decimal balance; //Customer balance. If negative, account status should be set to OVER_DRAWN
        public ACCOUNT_STATUS accountStatus;
        string pin; //Customer PIN. Number can only be 4 digits
        string username;
        string password;
        string firstName;
        string lastName;
        string streetAddress;
        string city;
        string state;
        string zip;
        string email;
        string primaryPhone;
        string secondaryPhone;
    }
}
