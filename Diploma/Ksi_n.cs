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
        private Simplex Start() 
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
        public decimal Lambda(decimal F, Matrix W)
        {
            /*Matrix WTrans = new Matrix(W.Transpose());
            Matrix WWF = W * WTrans * F;

            decimal res = 0;
            for (int i = 0; i < WWF.Row; i++)
            {
                for (int j = 0; j < WWF.Column; j++)
                {
                    res += F * WWF.Array[i, j];
                }
            }
            res /= WWF.Scalar();
            return res;*/
            return epsilon / 1000m;
        }


    }
}
