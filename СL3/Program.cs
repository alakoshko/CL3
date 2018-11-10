using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace СL3
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. Даны 2 двумерных матрицы. Размерность 100х100 каждая. 
            //Напишите приложение, производящее параллельное умножение матриц. 
            //Матрицы заполняются случайными целыми числами от 0 до10.
            #region 
            //const int size = 10;
            //var m1 = new Matrix(size);
            //var m2 = new Matrix(size);

            //Console.WriteLine($"{m1}\n{m2}");

            //m1.Multiplication(m2);

            //Console.WriteLine($"{m1}");
            #endregion

            #region
            //Создание файлов
            var swatch = Stopwatch.StartNew();
            CreateFiles();
            swatch.Stop();
            Console.WriteLine($"Elepsed time for creation files: {swatch.Elapsed}");



            #endregion

            Console.ReadLine();
        }

        const string sPath = "..\\..\\files\\";
        static public string GetRandomFileName()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return sPath + path.Substring(0, 8)+".txt";  // Return 8 character string
        }

        static void CreateFiles()
        {
            var random = new Random();
            Parallel.For(0, 100, i =>
            {
                string filename = GetRandomFileName();
                byte[] info = new UTF8Encoding(true).GetBytes($"{random.Next(1, 2)};{random.NextDouble() * random.Next(100, 10000)};{random.NextDouble() * random.Next(100, 10000)}");

                using (var fs = new FileStream(filename, FileMode.CreateNew,
                         FileAccess.Write, FileShare.None,
                         4096, FileOptions.None))
                {
                    fs.Write(info, 0, info.Length);
                }
            });
        }
        
        static double ReadCalc(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            int action = 0;
            double v1 = 0;
            double v2 = 0;
            
            string temp = sr.ReadLine();
            sr.Close();

            var strValue = temp.Split(';');
            int.TryParse(strValue[0], out action);
            double.TryParse(strValue[1], out v1);
            double.TryParse(strValue[2], out v2);

            if (action == 1)
                return v1* v2;
            else if (action == 2)
                return v1/v2;

            return 0;
        }
        
    }
}
