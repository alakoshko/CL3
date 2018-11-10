using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace СL3
{
    class Program
    {
        const string sPath = "..\\..\\files\\";
        const string sResultFile = "..\\..\\..\\..\\files\\result";
        static int FilesCount = 10000;

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
            Console.Write($"Введите кол-во создаваемых файлов (по умолчанию: {FilesCount}): ");
            var s = Console.ReadLine();
            if(s.Length > 0)
                int.TryParse(s, out FilesCount);

            Console.WriteLine($"Start creation..");
            var swatch = Stopwatch.StartNew();
            CreateFiles();
            swatch.Stop();
            Console.WriteLine($"Elepsed time for creation files: {swatch.Elapsed}");

            Console.WriteLine($"Start parallel creation..");
            swatch = null;
            swatch = Stopwatch.StartNew();
            CreateFilesParallel();
            swatch.Stop();
            Console.WriteLine($"Elepsed time for creation files: {swatch.Elapsed}");

           

            Console.WriteLine($"Started reading & calculation Async..");
            ReadFilesAsync(sPath);
            

            //Console.WriteLine($"Start reading & calculation Parallel.Foreach()..");
            //swatch = null;
            //swatch = Stopwatch.StartNew();
            //ReadFilesParallel(sPath);
            //swatch.Stop();
            //Console.WriteLine($"Elepsed time for read&calc files: {swatch.Elapsed}");

            #endregion

            Console.ReadLine();
        }

        
        static public string GetRandomFileName()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return sPath + path.Substring(0, 8)+".txt";  // Return 8 character string
        }

        static void CreateFilesParallel()
        {
            var random = new Random();
            Parallel.For(0, FilesCount, i =>
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

        static void CreateFiles()
        {
            var random = new Random();
            for(int i = 0; i < FilesCount; i++)
            {
                string filename = GetRandomFileName();
                byte[] info = new UTF8Encoding(true).GetBytes($"{random.Next(1, 2)};{random.NextDouble() * random.Next(100, 10000)};{random.NextDouble() * random.Next(100, 10000)}");

                using (var fs = new FileStream(filename, FileMode.CreateNew,
                         FileAccess.Write, FileShare.None,
                         4096, FileOptions.None))
                {
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        static async void ReadFilesAsync(string path)
        {
            string[] files = Directory.GetFiles(path);
            long result = 0;
            var watch = Stopwatch.StartNew();
            var filename = $"_{sResultFile}_async.txt";
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.None);
                using (StreamWriter sr = new StreamWriter(fs, Encoding.UTF8, 512))
                {

                    foreach (var f in files)
                        await Task.Run(() =>
                        {
                            double t = ReadCalc(f);
                            result += (long)t;
                            sr.WriteLine($"ThreadID: {Thread.CurrentThread.ManagedThreadId}, result: {t.ToString()}");
                        });
                    watch.Stop();
                    Console.WriteLine($"Сумма результатов = {result}\nЗатраченное время: {watch.Elapsed}");
                    sr.WriteLine($"Сумма результатов = {result}\nЗатраченное время: {watch.Elapsed}");
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        static void ReadFilesParallel(string path)
        {
            string[] files = Directory.GetFiles(path);
            long result = 0;
            var watch = Stopwatch.StartNew();
            var ts = new TimeSpan();
            var filename = $"_{sResultFile}_parallelForeach.txt";
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.None);
                using (StreamWriter sr = new StreamWriter(fs, Encoding.UTF8, 512))
                {
                    Parallel.ForEach(files, f =>
                    {
                        double t = ReadCalc(f);
                        result += (long)t;
                        sr.WriteLine($"ThreadID: {Thread.CurrentThread.ManagedThreadId}, result: {t.ToString()}");
                        ts += watch.Elapsed;
                    });
                    watch.Stop();
                    Console.WriteLine($"Сумма результатов = {result}\nЗатраченное время: {ts}");
                    sr.WriteLine($"Сумма результатов = {result}\nЗатраченное время: {ts}");
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }


        static double ReadCalc(string filename)
        {
            if (!filename.Contains("result_"))
            {
                using (StreamReader sr = new StreamReader(filename))
                {
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
                        return v1 * v2;
                    else if (action == 2)
                        return v1 / v2;
                }
            }
            return 0;
        }
        
    }
}
