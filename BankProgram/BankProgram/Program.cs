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

    public enum ACCOUNT_STATUS { ACTIVE, OVER_DRAWN, FROZEN, UNDER_REVIEW, CLOSED };

    public enum STATE
    {
        AL, AK, AZ, AR, CA,
        CO, CT, DE, FL, GA,
        HI, ID, IL, IN, IA,
        KS, KY, LA, ME, MD,
        MA, MI, MN, MS, MO,
        MT, NE, NV, NH, NJ,
        NM, NY, NC, ND, OH,
        OK, OR, PA, RI, SC,
        SD, TN, TX, UT, VT,
        VA, WA, WV, WI, WY
    };
}
