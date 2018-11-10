using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СL3
{
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

        /// <summary>
        /// реализация IEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<int> GetEnumerator()
        {

            for (int i = 0; i < _size; i++)
                for (int y = 0; y < _size; y++)
                    yield return m[i, y];
        }


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

        /// <summary>
        /// Умножение матриц, параметр - вторая матрица
        /// </summary>
        /// <param name="m2"></param>
        /// <returns></returns>
        public void Multiplication(Matrix m2)
        {
            if (m.GetLength(1) != m2.m.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");

            int[,] r = new int[m.GetLength(0), m2.m.GetLength(1)];
            Parallel.For(0, m.GetLength(0), (i) =>
            {
                for (int j = 0; j < m2.m.GetLength(1); j++)
                    for (int k = 0; k < m2.m.GetLength(0); k++)
                    {
                        r[i, j] += m[i, k] * m2.m[k, j];
                    }
            });
            m = r;
        }
    }

}
