using System;
using System.Collections.Generic;

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

        public static string WeighBars(List<int> leftBowl, List<int> rightBowl)
        {
            testPage.SetBowls("left", leftBowl);
            testPage.SetBowls("right", rightBowl);
            testPage.ClickWeigh();
            return testPage.GetResult();
        }

        //try to split the list into three equal groups
        public static List<List<int>> SplitGroup(List<int> group)
        {
            List<List<int>> newGroups= new List<List<int>>();
            List<int> group1 = new List<int>();
            List<int> group2 = new List<int>();
            List<int> group3 = new List<int>();
            int count = 0;
            if ((group.Count % 3) != 1)
            {
                if (group.Count == 2) //if n=2, then group 3 is empty
                {
                    group1.Add(group[count]);
                    group2.Add(group[count++]);
                }
                else
                {
                    while (count < group.Count) //if 0 remainders, its evenly split, but if n>2 and there is a remainder of 2, then group 3 will have 1 less bar
                    {
                        group1.Add(group[count]);
                        count++;
                        group2.Add(group[count]);
                        count++;
                        group3.Add(group[count]);
                        count++;
                    }
                }
            }
            else
            {
                while (count < group.Count-2) //if there is a remainder of 1, then group 3 has 1 extra bar
                {
                    group1.Add(group[count]);
                    count++;
                    group2.Add(group[count]);
                    count++;
                    group3.Add(group[count]);
                    count++;
                }
                group3.Add(group[group.Count - 1]);
            }
            newGroups.Add(group1);
            newGroups.Add(group2);
            newGroups.Add(group3);
            return newGroups;
        }
        //since there are 9 bars, splitting them into three groups of three and weighing two pairs will determine which group has the fake bar
        //it will then compare two numbers of that one group to find the fake
        //this will always result in 2 iterations
        public static string FindFakeBar(List<int> group1, List<int> group2, List<int> group3)
        {
            if(!fakeBar.Equals(""))
             {
                return fakeBar;
            }
            testPage.ClickReset();
            switch (WeighBars(group1, group2))
             {
                case "=":
                    if (group3.Count > 1)
                    {
                        List<List<int>> newGroup = SplitGroup(group3);
                        FindFakeBar(newGroup[0],newGroup[1],newGroup[2]);
                    }
                    else
                        fakeBar= group3[0].ToString();
                        break;

                case ">":
                    if (group2.Count > 1)
                    {
                        List<List<int>> newGroup = SplitGroup(group2);
                        FindFakeBar(newGroup[0], newGroup[1], newGroup[2]);
                    }
                    else
                        fakeBar = group2[0].ToString();
                    break;
                case "<":
                    if (group1.Count > 1)
                    {
                        List<List<int>> newGroup = SplitGroup(group1);
                        FindFakeBar(newGroup[0], newGroup[1], newGroup[2]);
                    }
                    else
                        fakeBar = group1[0].ToString();
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
            Console.WriteLine(4 % 3);
            Console.WriteLine(5 % 3);
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
                List<int> originalGroup = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                List<List<int>> splitGroups = SplitGroup(originalGroup);

                string fake = FindFakeBar(splitGroups[0], splitGroups[1], splitGroups[2]);
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
