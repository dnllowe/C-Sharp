using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
    }

}
