using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace doors
{
    class Program
    {
        public static void Main()
        {
            string line;
            string status = "";

            while (!status.Equals("q"))
            {
                // Creates and initializes a new ArrayList.
                ArrayList PersonList = new ArrayList();

                Console.Write("Enter bracelets (no seperator chars!): ");
                line = Console.ReadLine();

                foreach (var ch in line)
                {
                    PersonList.Add(int.Parse(ch.ToString()));
                }

                Console.Write("Enter doors (no seperator chars!): ");
                line = Console.ReadLine();

                ValidCombinationsMulti(PersonList, line, 3, 5);

                //foreach (var ch in line)
                //{
                //    ValidCombinations(PersonList, int.Parse(ch.ToString()), 3, 5);
                //}

                Console.Write("\n[Enter] to try again, type 'q' to quit...");
                status = Console.ReadLine();
            }
        }

        public static void PrintValues(IEnumerable myList)
        {
            if (((ArrayList)myList).Count > 0)
            {
                foreach (Object obj in myList)
                    Console.Write("   {0}", obj);
                Console.WriteLine();
            }
        }

        public static int CalcDigitalRoot(IEnumerable myList)
        {
            int i = 0;
            int tens, ones;
            int digitalr;

            if (((ArrayList)myList).Count > 0)
            {
                foreach (Object obj in myList)
                    i = i + (int)obj;

                tens = i / 10;
                ones = i % 10;

                digitalr = tens + ones;
            }
            else
            {
                digitalr = 0;
            }

            return digitalr;
        }

        public static void PrintDigitalRoot(IEnumerable myList)
        {
            int i = 0;
            int tens, ones;
            int digitalr;

            if (((ArrayList)myList).Count > 0)
            {
                foreach (Object obj in myList)
                {
                    Console.Write((int)obj + " ");
                    i = i + (int)obj;
                }

                Console.Write("--> ");

                tens = i / 10;
                ones = i % 10;

                Console.Write(tens + " " + ones);

                digitalr = tens + ones;

                Console.WriteLine(" --> " + digitalr);
            }
        }

        public static ArrayList ValidCombinations(ArrayList myList, int doorn, int min, int max)
        {
            int count;
            int limit = (int)(Math.Pow(2.0, myList.Count));
            ArrayList Mask = new ArrayList();

            //For each mask...
            for (int i = 0; i < limit; i++){
                //Check if each digit is compatible with the mask
                //Console.WriteLine("Mask: " + i);
                //Console.Write("  ");
                count = 1;
                foreach (Object obj in myList){
                    //Console.Write((int)obj + "[" + ((int)Math.Pow(2,(myList.Count-count)) & i) + "]" + " ");
                    if (((int)Math.Pow(2, (myList.Count - count)) & i) == (int)Math.Pow(2, (myList.Count - count)))
                    {
                        Mask.Add((int)obj);
                    }
                    count++;
                }
                //Console.WriteLine(Mask.Count + " matches");
                //The list of compatible digits is now calculated
                //Get valid digital roots against the digits of the mask and print them
                //Also make sure the mask is in the limits
                if (Mask.Count >= min && Mask.Count <= max)
                {
                    if (CalcDigitalRoot(Mask) == doorn)
                    {
                        PrintDigitalRoot(Mask);
                    }
                }

                Mask.Clear();
            }

            return null;
        }

        public static ArrayList ValidCombinationsMulti(ArrayList myList, string doorns, int min, int max)
        {
            int count;
            int ndx;
            char ch;
            int limit = (int)(Math.Pow(2.0, myList.Count));

            ArrayList Orig = (ArrayList)myList.Clone();
            ArrayList Mask = new ArrayList();
            ArrayList Leftover = new ArrayList();

            //For each mask...
            for (int i = 0; i < limit; i++)
            {
                //Check if each digit is compatible with the mask
                //Console.WriteLine("Mask: " + i);
                //Console.Write("  ");
                ndx = 0;
                myList = (ArrayList)Orig.Clone();
                
                while (myList.Count > 0 && myList.Count >= min && ndx <= (doorns.Length-1))
                {
                    count = 1;
                    ch = doorns[ndx];
                    foreach (Object obj in myList)
                    {
                        //Console.Write((int)obj + "[" + ((int)Math.Pow(2,(myList.Count-count)) & i) + "]" + " ");
                        if (((int)Math.Pow(2, (myList.Count - count)) & i) == (int)Math.Pow(2, (myList.Count - count)))
                        {
                            Mask.Add((int)obj);
                        }
                        else
                        {
                            //These go onto the next door
                            Leftover.Add((int)obj);
                        }
                        count++;
                    }
                    //Console.WriteLine(Mask.Count + " matches");
                    //The list of compatible digits is now calculated
                    //Get valid digital roots against the digits of the mask and print them
                    //Also make sure the mask is in the limits
                    if (Mask.Count >= min && Mask.Count <= max)
                    {
                        if (CalcDigitalRoot(Mask) == int.Parse(ch.ToString()))
                        {
                            if (myList.Count == Orig.Count)
                            {
                                Console.WriteLine("\nPath #" + i);
                            }
                            PrintDigitalRoot(Mask);
                        }
                    }

                    Mask.Clear();

                    //Set the leftovers to the list
                    myList = (ArrayList)Leftover.Clone();

                    Leftover.Clear();

                    ndx++;
                }
            }

            return null;
        }
    }
}
