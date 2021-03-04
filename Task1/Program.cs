using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "Numbers.txt";
            List<int> list = ReadFromFile(path);
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            //1
            var resT1 = list.AsParallel().GroupBy(l => l).Where(l => l.Count() == 1).Count();
            Console.WriteLine();
            Console.WriteLine("Count unique values:" + resT1);


            List<List<int>> lists = new List<List<int>>();
            for (int i = 0; i < list.Count; i++)
            {
                lists.Add(list.Skip(i).ToList());
       

            }
            //2
            var resT2 = lists.AsParallel().Select(l => AscSeqLength(l));
            Console.WriteLine("Lenght asc sequance:"+ resT2.Max());

            //3
            var resT3 = lists.AsParallel().Select(l => SeqPosLenth(l));
            Console.WriteLine("Lenght positive sequance: "+resT3.Max());
        }
       
        //2
        static int AscSeqLength(List<int>list)
        {
            int count = 1;
            for (int i =0; i < list.Count - 1; i++)
            {
                if (list[i] < list[i + 1])
                    count++;
                else return count;
            }
            return count;
        }
        //3
        static int SeqPosLenth(List<int> list)
        {
            int count = 0;
            for (int i = 0; i <list.Count;i++)
            {
                if (list[i] > 0)
                    count++;
                else  return count;
            }
            return count;
        }
    

    static List<int> ReadFromFile(string path)
    {
        List<int> res = new List<int>();
        using (StreamReader sr = new StreamReader(path))
        {
            string line;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                string[] split = line.Split(new char[] { ' ', ',' });
                foreach (var item in split)
                {
                    if (int.TryParse(item, out int parse))
                        res.Add(parse);
                }
            }
            sr.Close();
        }
        return res;
    }
}
}
