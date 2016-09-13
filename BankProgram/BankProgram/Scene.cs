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
            if (newString == null)
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
            for (int iii = 0; iii < input.Count(); iii++)
            {
                int result;

                //If the element is not numeric
                if(!int.TryParse(input[iii].ToString(), out result))
                { 
                    //Remove non-numeric characters
                    if (input[iii] != '.')
                        input = input.Remove(iii, 1);
                    else
                        input = input.Remove(iii);
                }
            }

            //If any characters are left, the input is valid
            if (input.Count() > 0)
            {
                isInputValid = true;

                //If the prase fails, then the input is still invalid
                if (int.TryParse(input, out newInt) == false)
                    isInputValid = false;
            }

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
            for (int iii = 0; iii < input.Count(); iii++)
            {
                int result;

                //If the element is not numeric
                if (!int.TryParse(input[iii].ToString(), out result))
                {
                    //Remove non-numeric characters
                    if (input[iii] != '.')
                        input = input.Remove(iii, 1);
                    else
                        input = input.Remove(iii);
                }
            }

            //If any characters are left, the input is valid
            if (input.Count() > 0)
            {
                isInputValid = true;

                //If the prase fails, then the input is still invalid
                if (int.TryParse(input, out newInt) == false)
                    isInputValid = false;
            }

            //Return an obviously wrong result if user provided wrong input.
            else
                newInt = -1029384756;

            return isInputValid;
        }

        public bool GetFloatInput(out float newFloat)
        {
            bool isInputValid = false;
            string input = Console.ReadLine();

            //Make sure all characters are numeric (remove letters, commas, and anything two places after a decimal)
            for (int iii = 0; iii < input.Count(); iii++)
            {
                float result;

                //If the element is not numeric
                if(!float.TryParse(input[iii].ToString(), out result))
                {
                    //Remove non-numeric characters
                    if (input[iii] != '.')
                        input = input.Remove(iii, 1);

                    //Get rid of anything further than two decimal places out
                    else
                    {
                        if(iii + 2 < input.Count())
                            input = input.Remove(iii + 2);
                    }
                }
            }

            //If any characters are left, the input is valid
            if (input.Count() > 0)
            {
                isInputValid = true;

                //If the prase fails, then the input is still invalid
                if (float.TryParse(input, out newFloat) == false)
                    isInputValid = false;
            }

            //Return an obviously wrong result if user provided wrong input.
            else
                newFloat = -1029384756;

            return isInputValid;
        }

        public bool GetFloatInput(out float newFloat, out string originalInput)
        {
            bool isInputValid = false;
            string input = Console.ReadLine();
            originalInput = input;

            //Make sure all characters are numeric (remove letters, commas, and anything two places after a decimal)
            for (int iii = 0; iii < input.Count(); iii++)
            {
                float result;

                //If the element is not numeric
                if (!float.TryParse(input[iii].ToString(), out result))
                {
                    //Remove non-numeric characters
                    if (input[iii] != '.')
                        input = input.Remove(iii, 1);

                    //Get rid of anything further than two decimal places out
                    else
                    {
                        if (iii + 2 < input.Count())
                            input = input.Remove(iii + 2);
                    }
                }
            }

            //If any characters are left, the input is valid
            if (input.Count() > 0)
            {
                isInputValid = true;

                //If the prase fails, then the input is still invalid
                if (float.TryParse(input, out newFloat) == false)
                    isInputValid = false;
            }

            //Return an obviously wrong result if user provided wrong input.
            else
                newFloat = -1029384756;

            return isInputValid;
        }

        public string GetXmlText(string node)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlPath);
            return xDoc.SelectSingleNode("prompts/" + node).InnerText;
        }

        string xmlPath = "../../strings.xml";
        protected bool isRunning = false; //Switch for if any scene is running
    }
}
