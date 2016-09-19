using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Xml;

namespace BankProgram
{
    class MySqlHelper
    {
        //Establishes connection to MySQL server based on system_info.xml
        static public bool ConnectToMySql()
        {
            if (isConnected)
                return true;

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("../../system_info.xml");
            string server = xDoc.SelectSingleNode("system_info/server").InnerText;
            string userID = xDoc.SelectSingleNode("system_info/userID").InnerText;
            string database = xDoc.SelectSingleNode("system_info/database").InnerText;
            string port = xDoc.SelectSingleNode("system_info/port").InnerText;
            string sqlPassword = xDoc.SelectSingleNode("system_info/password").InnerText;

            string connectionInput = string.Format(
                "server={0}; " +
                "user={1}; " +
                "database={2}; " +
                "port={3}; " +
                "password={4}; ", server, userID, database, port, sqlPassword
                );

            mySql = new MySqlConnection(connectionInput);
            try
            {
                mySql.Open();
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 1045:
                        Console.WriteLine("Invalid username/password.");
                        break;
                    default:
                        Console.WriteLine("Error code: {0}. {1}", e.HResult, e.Message);
                        break;
                }
                throw;
                return false;
            }
            isConnected = true;
            return true;
        }

        static public void ExecuteNonQueryCommand(string command, string[] parameters)
        {
            if (!isConnected)
                ConnectToMySql();

            MySqlCommand cmd = new MySqlCommand(command, mySql);

            //Use parameters to protect aganist injection
            for (int iii = 0; iii < parameters.Length; iii++)
            {
                string parameterName = @"@" + iii.ToString();
                string parameterValue = parameters[iii];
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }

            cmd.ExecuteNonQuery();
        }

        static public void ExecuteNonQueryCommand(List<string>command, string[] parameters)
        {
            string commandList = null;
            
            for (int iii = 0; iii < command.Count(); iii++)
                commandList += command[iii];
            MySqlCommand cmd = new MySqlCommand(commandList, mySql);

            //Use parameters to protect aganist injection
            for (int iii = 0; iii < parameters.Length; iii++)
            {
                string parameterName = @"@" + iii.ToString();
                string parameterValue = parameters[iii];
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }

            cmd.ExecuteNonQuery();
        }
        
        static public MySqlDataReader ExecuteQueryCommand(string command, string[] parameters)
        {
            if (!isConnected)
                ConnectToMySql();

            MySqlCommand cmd = new MySqlCommand(command, mySql);

            //Use parameters to protect aganist injection
            for(int iii = 0; iii < parameters.Length; iii++)
            {
                string parameterName = @"@" + iii.ToString();
                string parameterValue = parameters[iii];
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }

            return cmd.ExecuteReader();
        }

        static public MySqlDataReader ExecuteQueryCommand(List<string> command, string[] parameters)
        {
            string commandList = null;

            for (int iii = 0; iii < command.Count(); iii++)
                commandList += command[iii];

            MySqlCommand cmd = new MySqlCommand(commandList, mySql);

            //Use parameters to protect aganist injection
            for (int iii = 0; iii < parameters.Length; iii++)
            {
                string parameterName = @"@" + iii.ToString();
                string parameterValue = parameters[iii];
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }

            
            return cmd.ExecuteReader();
        }

        static public MySqlConnection GetConnection()
        {
            return mySql;
        }

        static MySqlConnection mySql = null;
        static bool isConnected = false;
    }
}
