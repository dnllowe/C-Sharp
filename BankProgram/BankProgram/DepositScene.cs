using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace BankProgram
{
    class DepositScene : Scene
    {
        public DepositScene()
        {
            isRunning = true;

            //Update based on desired xml root structure
            defaultXmlPath = "../../strings.xml";
            defaultXmlRootNode = "prompts";
        }

        public override void RunScene(float framesPerSecond)
        {
            MySqlHelper.ConnectToMySql();
            MySqlDataReader reader = null;
            string username;
            int id;
            decimal currentBalance = 0.00M;
            int dollarsToDeposit = 0;
            int centsToDeposit = 0;
            decimal amountToDeposit = 0.00M;
            decimal newBalance = 0.00M;
            string dollarsString;
            string centsString;

            do
            {
                Console.WriteLine(GetXmlText(@"general/enter_username"));
                username = Console.ReadLine();
                Console.WriteLine();

                reader = MySqlHelper.ExecuteQueryCommand("select * from customer_accounts where username = '" + username + "';");

                //Make sure there is data for this username
                if (!reader.HasRows)
                {
                    reader.Close();
                    Console.WriteLine(GetXmlText(@"general/invalid_username") + "'{0}'", username);
                    Console.WriteLine();
                }
            }
            while (!reader.HasRows);

            //Get first (and only) record to get data from
            reader.Read();
            id = reader.GetInt32("id");
            string pin = reader.GetString("pin");
            currentBalance = reader.GetDecimal("balance");
            reader.Close();

            //CHECK PIN

            bool isInputValid = false;

            //Get dollars to deposit
            do
            {
                Console.WriteLine(GetXmlText(@"deposit/deposit_dollars"));
                dollarsString = Console.ReadLine();

                if (dollarsString == "" || int.TryParse(dollarsString, out dollarsToDeposit))
                    isInputValid = true;
                else
                {
                    Console.WriteLine(GetXmlText(@"deposit/invalid_dollars"));
                    Console.WriteLine();
                }
            }
            while (!isInputValid);

            //Reset switch for next check
            isInputValid = false;

            //Get cents to deposit
            do
            {
                Console.WriteLine(GetXmlText(@"deposit/deposit_cents"));
                centsString = Console.ReadLine();

                if (centsString == "" || (int.TryParse(centsString, out centsToDeposit) && centsToDeposit >= 0 && centsToDeposit <= 99))
                    isInputValid = true;
                else
                {
                    Console.WriteLine(GetXmlText(@"deposit/invalid_cents"));
                    Console.WriteLine();
                }
            }
            while (!isInputValid);

            MySqlHelper.ExecuteNonQueryCommand(string.Format("update customer_accounts set balance={0} where id={1};", newBalance, id));
           
            Director.GetInstance().ChangeScene(new WelcomeScene());
            return;
        }
    }
}
