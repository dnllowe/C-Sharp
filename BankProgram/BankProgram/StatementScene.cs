using System;
using MySql.Data.MySqlClient;

namespace BankProgram
{
    class StatementScene : Scene
    {
        public StatementScene()
        {
            isRunning = true;

            //Update based on desired xml root structure
            defaultXmlPath = "../../strings.xml";
            defaultXmlRootNode = "prompts";
            defaultXmlElement = "statement";
        }

        override public void RunScene(float framesPerSecond)
        {
            MySqlHelper.ConnectToMySql();
            MySqlDataReader reader = null;
            int id;
            decimal currentBalance = 0.00M;

            if (!isLoggedIn)
            {
                do
                {
                    Console.WriteLine(GetXmlText(@"../../strings.xml", @"prompts/general/enter_username"));
                    username = Console.ReadLine();
                    Console.WriteLine();

                    reader = MySqlHelper.ExecuteQueryCommand("select * from customer_accounts where username = '" + username + "';");

                    //Make sure there is data for this username. No rows = no data.
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        Console.WriteLine(GetXmlText(@"../../strings.xml", @"prompts/general/invalid_username") + "'{0}'", username);
                        Console.WriteLine();
                    }
                }
                while (!reader.HasRows);
            }

            else
                reader = MySqlHelper.ExecuteQueryCommand("select * from customer_accounts where username = '" + username + "';");

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
                    Console.WriteLine(GetXmlText(@"../../strings.xml", @"prompts/general/attempts_exceeded"));
                    Console.WriteLine();
                    Director.GetInstance().ChangeScene(new WelcomeScene());
                    return;
                }
            }

            Console.WriteLine(GetXmlText("balance") + currentBalance.ToString());
            Console.WriteLine();

            Director.GetInstance().ChangeScene(new WelcomeScene());

            return;
        }
    }
}
