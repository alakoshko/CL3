using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СL3
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 100;
            var m1 = new Matrix(size);
            var m2 = new Matrix(size);

            Console.WriteLine($"{m1.ToString()}");

            Console.ReadLine();
        }
    }

    class Matrix
    {
        public int[,] m;
        private int _size;
        public Matrix(int size)
        {
            _size = size;
            m = new int[size, size];

            var rand = new Random();

            for (int i = 0; i < size; i++)
                for (int y = 0; y < size; y++)
                    m[i, y] = rand.Next(1, 10);
        }

        #region реализация IEnumerator
        public IEnumerator<int> GetEnumerator()
        {

            for (int i = 0; i < _size; i++)
                for (int y = 0; y < _size; y++)
                    yield return m[i, y];
        }

        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        public override string ToString()
        {
            string str = "";

            for (int i = 0; i < _size; i++)
            {
                str = str + $"\n";
                for (int y = 0; y < _size; y++)
                    str = str + $"{m[i, y]} ";
            }
            
            return str;
        }
    }
}
