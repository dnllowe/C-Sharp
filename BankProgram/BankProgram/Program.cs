using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BankProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Director director = Director.GetInstance();
            director.ChangeScene(new WelcomeScene());

            while (director.GetCurrentScene().IsRunning())
                director.RunScene();
            
            return;
        }
    }
}
