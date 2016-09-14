using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProgram
{
    class WelcomeScene : Scene
    {
        public WelcomeScene()
        {
            isRunning = true;

            //Update based on strings.xml root structure
            xmlRootNode += "welcome/";
        }

        public override void RunScene(float framesPerSecond)
        {
            int selection;
            string originalInput;
            Console.WriteLine(GetXmlText("greeting"));
            Console.WriteLine(GetXmlText("assist"));
            Console.WriteLine();
            Console.WriteLine(GetXmlText("main_menu"));
            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_number"));

            while (!GetIntInput(out selection, out originalInput) || selection > 5 || selection < 0)
            {
                //If the user tried tying the phrase from the menu, allow it using the first letter (since all are unique)
                //Make sure there is something in the originalInput

                try
                {
                    switch (originalInput[0])
                    {
                        //DEPOSIT FUNDS
                        case 'd':
                        case 'D':
                            selection = 1;
                            break;
                        //WITHDRAW FUNDS
                        case 'w':
                        case 'W':
                            selection = 2;
                            break;
                        //GET BALANCE STATEMENT
                        case 'g':
                        case 'G':
                        case 'b':
                        case 'B':
                        case 's':
                        case 'S':
                            selection = 3;
                            break;
                        //CREATE NEW ACCOUNT
                        case 'c':
                        case 'C':
                        case 'n':
                        case 'N':
                        case 'a':
                        case 'A':
                            selection = 4;
                            break;
                        //INVALID INPUT
                        default:
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("error") + GetXmlText("enter_number"));
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("assist"));
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("main_menu"));
                            Console.WriteLine();
                            break;
                    }
                }
                catch(Exception e)
                {
                    //If user didn't provide any input, go through loop again
                    if (originalInput == "")
                    {
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("error") + GetXmlText("enter_number"));
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("assist"));
                        Console.WriteLine();
                        Console.WriteLine(GetXmlText("main_menu"));
                        Console.WriteLine();
                    }

                    else
                        throw (e);
                }

                //Break out of loop if above cases were true
                if (selection == 1 || selection == 2 || selection == 3 || selection == 4)
                    break;
            }

            //Lower selection by one so it matches up to enumerated types
            selection--;

            Director director = Director.GetInstance();

            switch (selection)
            {
                case (int)(SELECTION.DEPOSIT):
                    director.ChangeScene(new DepositScene());
                    break;
                case (int)(SELECTION.WITHDRAW):
                    director.ChangeScene(new WithdrawScene());
                    break;
                case (int)(SELECTION.STATEMENT):
                    director.ChangeScene(new StatementScene());
                    break;
                case (int)(SELECTION.NEW):
                    director.ChangeScene(new CreateAccountScene());
                    break;
            }

            return;

        }

        enum SELECTION { DEPOSIT, WITHDRAW, STATEMENT, NEW };
    }
}
