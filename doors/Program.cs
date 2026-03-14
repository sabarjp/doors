using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace doors
{
    class Program
    {
        // The puzzle: locks can take 3-5 keys and their digital root must
        //             equal the lock.
        public static void Main()
        {
            string line;
            string status = "";

            while (!status.Equals("q"))
            {
                // Creates and initializes a new ArrayList.
                ArrayList PersonList = new ArrayList();

                Console.Write("Enter keys (no seperator chars!): ");
                line = Console.ReadLine();

                foreach (var ch in line)
                {
                    PersonList.Add(int.Parse(ch.ToString()));
                }

                Console.Write("Enter locks (no seperator chars!): ");
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
            int limit = 1 << myList.Count;
            ArrayList Mask = new ArrayList();

            //For each mask...
            for (int i = 0; i < limit; i++){
                //Check if each digit is compatible with the mask
                //Console.WriteLine("Mask: " + i);
                //Console.Write("  ");
                count = 1;
                foreach (Object obj in myList){
                    //Console.Write((int)obj + "[" + ((int)Math.Pow(2,(myList.Count-count)) & i) + "]" + " ");
                    if (((1 << (myList.Count - count)) & i) == (1 << (myList.Count - count)))
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
            ArrayList Orig = (ArrayList)myList.Clone();
            ArrayList Path = new ArrayList();
            int pathNumber = 0;
            int remainingMask = (1 << Orig.Count) - 1;

            ValidCombinationsMultiRecursive(Orig, doorns, min, max, 0, remainingMask, Path, ref pathNumber);

            return null;
        }

        public static void ValidCombinationsMultiRecursive(ArrayList origList, string doorns, int min, int max, int ndx, int remainingMask, ArrayList path, ref int pathNumber)
        {
            int limit;
            int targetDoor;

            if (ndx > doorns.Length - 1)
            {
                pathNumber++;
                Console.WriteLine("\nPath #" + pathNumber);

                foreach (ArrayList mask in path)
                {
                    PrintDigitalRoot(mask);
                }

                return;
            }

            if (CountMaskBits(remainingMask) < min)
            {
                return;
            }

            limit = 1 << origList.Count;
            targetDoor = int.Parse(doorns[ndx].ToString());

            for (int i = 0; i < limit; i++)
            {
                ArrayList Mask;
                int bitCount;

                if ((i & remainingMask) != i)
                {
                    continue;
                }

                bitCount = CountMaskBits(i);
                if (bitCount < min || bitCount > max)
                {
                    continue;
                }

                Mask = BuildMaskList(origList, i);

                if (CalcDigitalRoot(Mask) == targetDoor)
                {
                    path.Add((ArrayList)Mask.Clone());
                    ValidCombinationsMultiRecursive(origList, doorns, min, max, ndx + 1, remainingMask ^ i, path, ref pathNumber);
                    path.RemoveAt(path.Count - 1);
                }
            }
        }

        public static int CountMaskBits(int mask)
        {
            int count = 0;

            while (mask > 0)
            {
                count += (mask & 1);
                mask >>= 1;
            }

            return count;
        }

        public static ArrayList BuildMaskList(ArrayList myList, int mask)
        {
            ArrayList Mask = new ArrayList();
            int count = 1;

            foreach (Object obj in myList)
            {
                if (((1 << (myList.Count - count)) & mask) == (1 << (myList.Count - count)))
                {
                    Mask.Add((int)obj);
                }
                count++;
            }

            return Mask;
        }
    }
}
