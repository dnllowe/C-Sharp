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

            //Update based on desired xml root structure
            defaultXmlPath = "../../strings.xml";
            defaultXmlRootNode = "prompts";
            defaultXmlElement = "welcome";
        }

        public override void RunScene(float framesPerSecond)
        {
            string selectionString;
            int? selection = null;

            Console.WriteLine(GetXmlText("greeting"));
            Console.WriteLine(GetXmlText("assist"));
            Console.WriteLine();
            Console.WriteLine(GetXmlText("main_menu"));
            Console.WriteLine();
            Console.WriteLine(GetXmlText("enter_number"));

            bool isSelectionValid = false;
            do
            {
                //User didn't enter blank
                if (GetStringInput(out selectionString))
                {
                    //Assume true, will set to false if all cases fail
                    isSelectionValid = true;

                    //Check if user typed in phrase, or number along with phrase, or number + '.'
                    switch (selectionString[0])
                    {
                        //DEPOSIT FUNDS
                        case '1':
                        case 'd':
                        case 'D':
                            selection = 1;
                            break;
                        //WITHDRAW FUNDS
                        case '2':
                        case 'w':
                        case 'W':
                            selection = 2;
                            break;
                        //GET BALANCE STATEMENT
                        case '3':
                        case 'g':
                        case 'G':
                        case 'b':
                        case 'B':
                        case 's':
                        case 'S':
                            selection = 3;
                            break;
                        //CREATE NEW ACCOUNT
                        case '4':
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
                            isSelectionValid = false;
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("error"));
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("assist"));
                            Console.WriteLine();
                            Console.WriteLine(GetXmlText("main_menu"));
                            Console.WriteLine();
                            break;
                    }
                }
                //User entered blank
                else
                {
                    Console.WriteLine(GetXmlText("error"));
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("assist"));
                    Console.WriteLine();
                    Console.WriteLine(GetXmlText("main_menu"));
                    Console.WriteLine();
                }
            }
            while (!isSelectionValid);

            //Lower selection by one so it matches up to enumerated types
            if(selection != null)
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
