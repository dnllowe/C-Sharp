﻿using System;
using MySql.Data.MySqlClient;

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
            int id;
            decimal currentBalance = 0.00M;
            int dollarsToDeposit = 0;
            int centsToDeposit = 0;
            decimal amountToDeposit = 0.00M;
            decimal newBalance = 0.00M;
            string dollarsString;
            string centsString;

            if (!isLoggedIn)
            {
                do
                {
                    Console.WriteLine(GetXmlText(@"general/enter_username"));
                    username = Console.ReadLine();
                    Console.WriteLine();

                    reader = MySqlHelper.ExecuteQueryCommand("select * from customer_accounts where username = @0;", new string[] { username });

                    //Make sure there is data for this username. No rows = no data.
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        Console.WriteLine(GetXmlText(@"general/invalid_username") + "'{0}'", username);
                        Console.WriteLine();
                    }
                }
                while (!reader.HasRows);
            }

            else
                reader = MySqlHelper.ExecuteQueryCommand("select * from customer_accounts where username = @0;", new string[] { username });

            //Get first (and only) record to get data from
            reader.Read();
            id = reader.GetInt32("id");
            string pin = reader.GetString("pin");
            currentBalance = reader.GetDecimal("balance");
            reader.Close();

            //CHECK PIN IF NOT LOGGED IN
            if (!isLoggedIn)
            {
                bool isPINValid = CheckPIN(validPin: pin, attemptsAllowed: 5);

                //EXIT TO WELCOME SCENE IF ATTEMPTS EXCEED LIMIT
                if (!isPINValid)
                {
                    Console.WriteLine(GetXmlText("general/attempts_exceeded"));
                    Console.WriteLine();
                    Director.GetInstance().ChangeScene(new WelcomeScene());
                    return;
                }
            }

            bool isInputValid = false;

            //Get dollars to deposit
            do
            {
                Console.WriteLine(GetXmlText(@"deposit/deposit_dollars"));
                dollarsString = Console.ReadLine();
                Console.WriteLine();

                if (dollarsString == "" || int.TryParse(dollarsString, out dollarsToDeposit))
                {
                    //Can't deposit negative dollars
                    if (dollarsToDeposit >= 0)
                        isInputValid = true;
                }
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
                Console.WriteLine();

                if (centsString == "" || (int.TryParse(centsString, out centsToDeposit) && centsToDeposit >= 0 && centsToDeposit <= 99))
                    isInputValid = true;
                else
                {
                    Console.WriteLine(GetXmlText(@"deposit/invalid_cents"));
                    Console.WriteLine();
                }
            }
            while (!isInputValid);

            amountToDeposit = dollarsToDeposit + centsToDeposit / 100.00M;
            newBalance = currentBalance + amountToDeposit;
            MySqlHelper.ExecuteNonQueryCommand("update customer_accounts set balance=@0 where id=@1;", new string[] { newBalance.ToString(), id.ToString() });

            //CONFIRMATION
            Console.WriteLine(GetXmlText("deposit/success") + GetXmlText("general/new_balance") + string.Format("${0:0.00}", newBalance));
            Console.WriteLine();

            Director.GetInstance().ChangeScene(new WelcomeScene());
            return;
        }
    }
}
