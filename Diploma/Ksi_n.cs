using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class Ksi_n
    {
        private int size;
        private const decimal epsilon = 0.001m;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public Ksi_n(int Size)
        {
            this.Size = Size;
        }

        // Матрица Якоби
        public Decimal[,] PartialDerivative(Simplex Sim)
        {
            Decimal ksi = Sim.Ksi();
            Decimal[,] result = new Decimal[Size + 1, Size + 1];

            // Определяю количество потоков равное количеству вершин симплекса
            Task[] tasks = new Task[Size + 1]; 
            for (int i = 0; i <= Size; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Simplex tmpSim = new Simplex(Sim);
                    decimal delta;
                    for(int j = 0; j < Size; j++)
                    {
                        if (tmpSim.array.Array[i,j] < 1)
                        {
                            delta = epsilon;
                        }
                        else
                        {
                            delta = -epsilon;
                        }
                        // Делаем прирощение 
                        tmpSim.array.Array[i, j] += delta;
                        tmpSim.matrix.Array[i, j] += delta;

                        //Считаем частную производную в точке
                        result[i, j] = (ksi - tmpSim.Ksi()) / delta;

                        // Вернули обратно, чтобы посчитать следующее прирощение
                        tmpSim.array.Array[i, j] = Sim.array.Array[i, j];
                        tmpSim.matrix.Array[i, j] = Sim.matrix.Array[i, j];
                    }
                });
                tasks[i].Start();
                tasks[i].Wait();
                result[i, Size] = 0;
            }
            return result;
        }

        // Начальное приближение 
        public Simplex Start() 
        {
            decimal[,] mass = new decimal[Size + 1, Size];
            for(int i = 0; i < Size+1; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    mass[i, j] = 0;
                }
                if(i>0)
                {
                    mass[i, i - 1] = 1m;
                }
            }
            Matrix matr = new Matrix(Size + 1, Size, mass);
            Simplex res = new Simplex(matr);
            return res;
        }

        // Считаем Лямбду
        public decimal Lambda()
        {
            return epsilon * 10m;
        }

        //Возможна ошибка в том, что изначальная матрица [n, n+1], а матрица Якоби [n+1, n+1] (Последний столбец - нули). Проблема в том, что берётся транспонированная  
        private Simplex OneIteration(Matrix X, Matrix W, decimal F)
        {
            Matrix rightMatr = W.Transpose() * Lambda() * F;
            Matrix result = new Matrix(X);

            for(int i = 0; i < X.Row; i++)
            {
                for(int j = 0; j < X.Column; j++)
                {
                    result.Array[i, j] -= rightMatr.Array[i, j];
                    if (result.Array[i, j] > 1)
                        result.Array[i, j] = 1;
                    if (result.Array[i, j] < 0)
                        result.Array[i, j] = 0;
                }
            }
            Simplex res = new Simplex(result);
            return res;
        }

        public void Main()
        {
            // Начальная инициализация
            Simplex S_k = Start();
            Decimal ksi_k = S_k.Ksi();
            Simplex S_k1 = OneIteration(S_k.array, new Matrix(size + 1, size + 1, PartialDerivative(S_k)), ksi_k);
            Decimal ksi_k1 = S_k1.Ksi();
            Console.WriteLine(S_k.array.ToString());
            Console.WriteLine(ksi_k);
            Console.WriteLine(S_k1.array.ToString());
            Console.WriteLine(ksi_k1);
        }

    }
}
