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
        }

        public override void RunScene(float framesPerSecond)
        {
            Console.WriteLine(GetXmlText("greeting"));
            Console.WriteLine();
            Console.WriteLine(GetXmlText("main_menu"));
            Console.ReadLine();
            Director.GetInstance().ChangeScene(null); 
            return;
        }
    }
}
