using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProgram
{
    class Director
    {
        protected Director()
        {
        }

        public static Director GetInstance()
        {
            if (instance == null)
                instance = new Director();
            return instance;
        }

        public void RunScene(float framesPerSecond = 60.0f)
        {
            try
            {
                currentScene.RunScene(framesPerSecond);
            }
            catch(Exception e)
            {
                if (currentScene == null)
                {
                    Console.WriteLine("New Scene was unable to load. " + e.Message);
                    Console.WriteLine("Returning to Main Menu");
                    Console.ReadLine();
                    ChangeScene(new WelcomeScene());
                }
                else
                    throw;
            }
            return;
        }

        public Scene GetCurrentScene()
        {
            return currentScene;
        }

        public void ChangeScene(Scene newScene)
        {
            if(currentScene != null)
                currentScene.Clean();
            currentScene = newScene;
            return;
        }
        static Director instance = null;
        Scene currentScene = null;
    }

    
}
