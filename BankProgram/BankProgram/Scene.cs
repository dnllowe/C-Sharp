using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BankProgram
{
    class Scene
    {
        //Check if any scene is running
        public bool IsRunning()
        {
            return isRunning;
        }

        //Run scene's loop
        virtual public void RunScene(float framesPerSecond)
        {
            return;
        }
    
        //Clean up any necessary resources
        virtual public void Clean()
        {
            return;
        }

        //Returns true if user input a valid string
        public bool GetStringInput(out string newString)
        {
            newString = Console.ReadLine();
            if (newString == "")
                return false;
            else
                return true;
        }

        //Returns true if user input a valid integer
        public bool GetIntInput(out int newInt)
        {
            string input = Console.ReadLine();
            bool isInputValid = false;

            //Make sure all characters are numeric (remove letters, commas, and anything after a decimal)
            for (int iii = 0; iii < input.Length; iii++)
            {
                //Just using to get result from int.TryParse()
                int result;

                //If the element is not numeric
                if(!int.TryParse(input[iii].ToString(), out result))
                {
                    //Remove commas from input
                    //NOTE: THIS WOULD NEED TO LOOK FOR '.' FOR INTERNATIONAL USE
                    if (input[iii] == ',')
                        input = input.Remove(iii, 1);
                }
            }

            //If any characters are left, and they are all numeric, the input is valid
            if (input.Length > 0 && int.TryParse(input, out newInt))
                isInputValid = true;

            //Return an obviously wrong result if user provided wrong input.
            else
                newInt = -1029384756;

            return isInputValid;
        }

        //Returns true if user input a valid integer, and keeps the original input if needed
        public bool GetIntInput(out int newInt, out string originalInput)
        {
            string input = Console.ReadLine();
            originalInput = input;
            bool isInputValid = false;

            //Make sure all characters are numeric (remove letters, commas, and anything after a decimal)
            for (int iii = 0; iii < input.Length; iii++)
            {
                //Just using to get result from int.TryParse()
                int result;

                //If the element is not numeric
                if (!int.TryParse(input[iii].ToString(), out result))
                {
                    //Remove commas from input
                    //NOTE: THIS WOULD NEED TO LOOK FOR '.' FOR INTERNATIONAL USE
                    if (input[iii] == ',')
                        input = input.Remove(iii, 1);
                }
            }

            //If any characters are left, and they are all numeric, the input is valid
            if (input.Length > 0 && int.TryParse(input, out newInt))
                isInputValid = true;

            //Return an obviously wrong result if user provided wrong input.
            else
                newInt = -1029384756;

            return isInputValid;
        }

        public bool GetFloatInput(out float newFloat, int precision = 2)
        {
            bool isInputValid = false;
            string input = Console.ReadLine();

            //Make sure input only contains one decimal points
            //NOTE: THIS WOULD NEED TO LOOK FOR ',' FOR INTERNATIONAL USE
            int decimalCount = 0;
            for (int iii = 0; iii < input.Length; iii++)
            {
                if (input[iii] == '.')
                    decimalCount++;
            }

            //Make sure all characters are numeric (remove letters, commas, and anything two places after a decimal)
            for (int iii = 0; iii < input.Length; iii++)
            {
                float result;

                //If the element is not numeric
                if(!float.TryParse(input[iii].ToString(), out result))
                {
                    //Remove commas
                    //NOTE: THIS WOULD NEED TO LOOK FOR '.' FOR INTERNATIONAL USE
                    if (input[iii] == ',')
                        input = input.Remove(iii, 1);

                    //Get rid of anything further than the specified decimal precision
                    //NOTE: THIS WOULD NEED TO LOOK FOR ',' FOR INTERNATIONAL USE
                    else
                    {
                        if(iii + precision < input.Length)
                            input = input.Remove(iii + 2);
                    }
                }
            }

            //If any characters are left, and they are all able to parse, and only one decimal provided, the input is valid
            if (input.Length > 0 && float.TryParse(input, out newFloat) && decimalCount < 2)
                isInputValid = true;

            //Return an obviously wrong result if user provided wrong input.
            else
                newFloat = -1029384756;

            return isInputValid;
        }

        public bool GetFloatInput(out float newFloat, out string originalInput, int precision = 2)
        {
            bool isInputValid = false;
            string input = Console.ReadLine();
            originalInput = input;

            //Make sure input only contains one decimal points
            //NOTE: THIS WOULD NEED TO LOOK FOR ',' FOR INTERNATIONAL USE
            int decimalCount = 0;
            for (int iii = 0; iii < input.Length; iii++)
            {
                if (input[iii] == '.')
                    decimalCount++;
            }

            //Make sure all characters are numeric (remove letters, commas, and anything two places after a decimal)
            for (int iii = 0; iii < input.Length; iii++)
            {
                float result;

                //If the element is not numeric
                if (!float.TryParse(input[iii].ToString(), out result))
                {
                    //Remove commas
                    //NOTE: THIS WOULD NEED TO LOOK FOR '.' FOR INTERNATIONAL USE
                    if (input[iii] == ',')
                        input = input.Remove(iii, 1);

                    //Get rid of anything further than the specified decimal precision
                    //NOTE: THIS WOULD NEED TO LOOK FOR ',' FOR INTERNATIONAL USE
                    else
                    {
                        if (iii + precision < input.Length)
                            input = input.Remove(iii + 2);
                    }
                }
            }

            //If any characters are left, and they are all able to parse, and only one decimal provided, the input is valid
            if (input.Length > 0 && float.TryParse(input, out newFloat) && decimalCount < 2)
                isInputValid = true;

            //Return an obviously wrong result if user provided wrong input.
            else
                newFloat = -1029384756;

            return isInputValid;
        }

        public string GetXmlText(string node)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(defaultXmlPath);

            //Sub-nodes not used, root node > node > data
            if(defaultXmlElement == "")
                return xDoc.SelectSingleNode(defaultXmlRootNode + "/" + node).InnerText;

            //Sub-nodes used, root node > node > ... > node > data
            else
                return xDoc.SelectSingleNode(defaultXmlRootNode + "/" + defaultXmlElement + "/" + node).InnerText;
        }

        public string GetXmlText(string xmlPath, string node)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlPath);
            return xDoc.SelectSingleNode(node).InnerText;
        }

        //Runs loop that requests users PIN and checks against database. Only "attemptsAllowed" failures permitted before returning false.
        protected bool CheckPIN(string validPin, int attemptsAllowed = 5)
        {
            int numberOfAttempts = 0;
            string pinInput;

            do
            {
                Console.WriteLine(GetXmlText("general/enter_pin"));
                pinInput = Console.ReadLine();
                Console.WriteLine();

                if (pinInput != validPin)
                {
                    numberOfAttempts++;
                    Console.WriteLine(GetXmlText("general/invalid_pin"));
                    Console.WriteLine(GetXmlText("general/attempts_remaining") + (attemptsAllowed - numberOfAttempts));
                    Console.WriteLine();

                }
            }
            while (pinInput != validPin && (attemptsAllowed - numberOfAttempts) != 0);

            if (attemptsAllowed - numberOfAttempts == 0)
                return false;
            else
                return true;
        }

        protected string defaultXmlPath = "../../strings.xml";
        protected string defaultXmlRootNode = "prompts";
        protected string defaultXmlElement = "";
        protected bool isRunning = false; //Switch for if any scene is running
    }
}
