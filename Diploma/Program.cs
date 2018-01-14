using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class Program
    {
        static void Main(string[] args)
        {
            //// Тест работоспособности нахождения КСИ
            //Decimal[,] array = new decimal[4, 3] { { 0.000m, 0.000m, 0.000m }, { 1.000m, 1.000m, 0.000m }, { 1.000m, 0.000m, 1.000m }, { 0.000m, 1.000m, 1.000m } };
            //Matrix matr = new Matrix(4, 3, array);
            //Simplex simplex = new Simplex(matr);
            //Console.WriteLine(simplex.Ksi().ToString());


            //// Тест работоспособности нахождения матрицы Якоби
            //Decimal[,] array = new decimal[4, 3] { { 0.000m, 0.000m, 0.000m }, { 1.000m, 0.500m, 0.000m }, { 0.300m, 0.000m, 0.200m }, { 0.000m, 0.700m, 0.800m } };
            //Matrix matr = new Matrix(4, 3, array);
            //Simplex simplex = new Simplex(matr);
            //Ksi_n ksi_n = new Ksi_n(3);
            //Matrix matr2 = new Matrix(4, 4, ksi_n.PartialDerivative(simplex));
            //Console.WriteLine(matr2.ToString());

            //decimal lambda = ksi_n.Lambda(simplex.Ksi(), matr);
            //Console.WriteLine(matr2 * lambda * simplex.Ksi()); 

            //// Тест начальной инициализации
            //Ksi_n S = new Ksi_n(2);
            //Simplex sim = S.Start();
            //Console.WriteLine(sim.matrix.ToString());
            //Console.WriteLine (sim.Ksi() );


            // Тест итерационного процесса
            Ksi_n S = new Ksi_n(2);
            S.Main();
            Console.WriteLine();
        }
    }
}
