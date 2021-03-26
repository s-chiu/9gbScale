using System;


namespace _9gbScale
{
    class gbScale
    {
        static WebDriver webDriver;
        static TestPage testPage;
        static string fakeBar="";
        private static void StartDriver()
        {
            webDriver = new();
        }

        public static string WeighBars(int[] leftBowl, int[] rightBowl)
        {
            testPage.SetBowls("left", leftBowl);
            testPage.SetBowls("right", rightBowl);
            testPage.ClickWeigh();
            return testPage.GetResult();
        }

        //since there are 9 bars, splitting them into three groups of three and weighing two pairs will determine which group has the fake bar
        //it will then compare two numbers of that one group to find the fake
        //this will always result in 2 iterations
        public static string FindFakeBar(int[] group1, int[] group2, int[] group3)
        {
            if(!fakeBar.Equals(""))
             {
                return fakeBar;
            }
            testPage.ClickReset();
            switch (WeighBars(group1, group2))
             {
                case "=":
                    if (group3.Length > 1)
                    {
                        int[] newGroup1 = { group3[0] };
                        int[] newGroup2 = { group3[1] };
                        int[] newGroup3 = { group3[2] };
                        FindFakeBar(newGroup1,newGroup2,newGroup3);
                    }
                    else
                        fakeBar= group3[0].ToString();
                        break;

                case ">":
                    if (group2.Length > 1)
                    {
                        int[] newGroup1 = { group2[0] };
                        int[] newGroup2 = { group2[1] };
                        int[] newGroup3 = { group2[2] };
                        FindFakeBar(newGroup1, newGroup2, newGroup3);
                    }
                    else
                        fakeBar = group2.GetValue(0).ToString();
                        break;
                case "<":
                    if (group1.Length > 1)
                    {
                        int[] newGroup1 = { group1[0] };
                        int[] newGroup2 = { group1[1] };
                        int[] newGroup3 = { group1[2] };
                        FindFakeBar(newGroup1, newGroup2, newGroup3);
                    }
                    else
                        fakeBar = group1.GetValue(0).ToString();
                        break;
                default:
                    throw new Exception("Something bad happened");
            }
            return fakeBar;
        }

        public static void SelectAnswer(string fake)
        {
            testPage.ClickCoin(fake);
            testPage.GetAlertMessage();
            Console.WriteLine("Enter any key to print the Weighings");
            Console.ReadLine();
            testPage.CloseAlertMessage();
        }
        public static void PrintWeighings()
        {
            Console.WriteLine("List of Weighings");
            testPage.GetWeighings();
        }
        public static void ExitProgram()
        {
            webDriver.Driver.Quit();
            Environment.Exit(0);
        }
        public static void Main()
        {
            StartDriver();
            Console.WriteLine("Enter any key to start");
            Console.ReadLine();
            testPage = new TestPage(webDriver);
            testPage.GoToURL();
            bool restart = true;
            while (restart is true)
            {
                 if(!fakeBar.Equals(""))
                {
                    fakeBar = "";
                }
                int[] group1 = { 0, 1, 2 };
                int[] group2 = { 3, 4, 5 };
                int[] group3 = { 6, 7, 8 };
                string fake = FindFakeBar(group1, group2, group3);
                Console.WriteLine("The fake is " + fake);
                SelectAnswer(fake);
                PrintWeighings();
                Console.WriteLine("Enter 1 to restart, any other key to end");
                if (!Console.ReadLine().Equals("1"))
                {
                    restart = false;
                }
                webDriver.Driver.Navigate().Refresh();
            }
            ExitProgram();


        }
    }
}
