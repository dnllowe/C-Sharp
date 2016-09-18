using System;
using MySql.Data.MySqlClient;

namespace BankProgram
{
    class WithdrawScene : Scene
    {
        public WithdrawScene()
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
            int dollarsToWithdraw = 0;
            int centsToWithdraw = 0;
            decimal amountToWithdraw = 0.00M;
            decimal newBalance = 0.00M;
            string dollarsString;
            string centsString;

            do
            {
                Console.WriteLine(GetXmlText(@"general/enter_username"));
                username = Console.ReadLine();
                Console.WriteLine();

                reader = MySqlHelper.ExecuteQueryCommand("select * from customer_accounts where username = '" + username + "';");

                //Make sure there is data for this username. No rows = no data.
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
            bool isPINValid = CheckPIN(validPin: pin, attemptsAllowed: 5);

            //EXIT TO WELCOME SCENE IF ATTEMPTS EXCEED LIMIT
            if (!isPINValid)
            {
                Console.WriteLine(GetXmlText("general/attempts_exceeded"));
                Console.WriteLine();
                Director.GetInstance().ChangeScene(new WelcomeScene());
                return;
            }

            bool isInputValid = false;

            //Get dollars to deposit
            do
            {
                Console.WriteLine(GetXmlText(@"withdraw/withdraw_dollars"));
                dollarsString = Console.ReadLine();
                Console.WriteLine();

                if (dollarsString == "" || int.TryParse(dollarsString, out dollarsToWithdraw))
                {
                    //Can't withdraw negative dollars, and can't overdraw account
                    if (dollarsToWithdraw > 0 && dollarsToWithdraw <= currentBalance)
                        isInputValid = true;
                    else if (dollarsToWithdraw > currentBalance)
                        Console.WriteLine(GetXmlText(@"withdraw/not_enough_funds"));

                }
                else
                {
                    Console.WriteLine(GetXmlText(@"withdraw/invalid_dollars"));
                    Console.WriteLine();
                }
            }
            while (!isInputValid);

            //Reset switch for next check
            isInputValid = false;

            //Get cents to deposit
            do
            {
                Console.WriteLine(GetXmlText(@"withdraw/withdraw_cents"));
                centsString = Console.ReadLine();
                Console.WriteLine();


                if (centsString == "" || (int.TryParse(centsString, out centsToWithdraw) && centsToWithdraw >= 0 && centsToWithdraw <= 99))
                {
                    //Can't overdraw account
                    if (dollarsToWithdraw + centsToWithdraw / 100.00M > currentBalance)
                        Console.WriteLine(GetXmlText(@"withdraw/not_enough_funds"));
                    else
                        isInputValid = true;
                }
                else
                {
                    Console.WriteLine(GetXmlText(@"withdraw/invalid_cents"));
                    Console.WriteLine();
                }
            }
            while (!isInputValid);

            amountToWithdraw = dollarsToWithdraw + centsToWithdraw / 100.00M;
            newBalance = currentBalance - amountToWithdraw;
            MySqlHelper.ExecuteNonQueryCommand(string.Format("update customer_accounts set balance={0} where id={1};", newBalance, id));

            //CONFIRMATION
            Console.WriteLine(GetXmlText("withdraw/success") + GetXmlText("general/new_balance") + string.Format("${0:0.00}", newBalance));
            Console.WriteLine();

            Director.GetInstance().ChangeScene(new WelcomeScene());
            return;
        }
    }
}
