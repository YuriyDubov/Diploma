using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class Simplex
    {
        private int size;//n характеристика
        public Matrix matrix;
        public Matrix array;//та же самая матрица, но без столбца единиц
        public int Size { get { return size; } }

        public Simplex(Simplex simplex)
        {
            size = simplex.Size;
            this.matrix = new Matrix(size + 1, size + 1);
            //matrix = simplex.matrix;
            this.array = new Matrix(size + 1, size);
            //array = simplex.array;
            for (int i = 0; i < size + 1; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.matrix.Array[i, j] = simplex.matrix.Array[i, j];
                    this.array.Array[i, j] = simplex.matrix.Array[i, j];
                }
                this.matrix.Array[i, size] = simplex.matrix.Array[i, size];
            }
        }
        public Simplex(Matrix matrix)//конструктор
        {
            if (matrix.Row != matrix.Column + 1)
            {
                throw new Exception("Матрица не подходит");
            }
            this.matrix = new Matrix(matrix.Row, matrix.Column + 1);
            this.array = new Matrix(matrix.Row, matrix.Column);
            size = matrix.Column;
            for (int i = 0; i < size + 1; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.matrix.Array[i, j] = matrix.Array[i, j];
                    this.array.Array[i, j] = matrix.Array[i, j];
                }
                this.matrix.Array[i, size] = 1;
            }
        }

        private Decimal LambdaMax()
        {
            Matrix inverseMatrix = new Matrix(this.matrix.Inverse());
            Decimal[] lambda_i = new Decimal[size + 1];
            for (int i = 0; i <= size; i++)
                lambda_i[i] = 0;
            //находим максимальное значение для -Лямбда_i 
            for (int j = 0; j <= size; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    if (inverseMatrix.Array[i, j] < 0)
                    {
                        lambda_i[j] -= inverseMatrix.Array[i, j];
                    }
                }
                lambda_i[j] -= inverseMatrix.Array[size, j];
            }
            return lambda_i.Max();
        }

        public Decimal Ksi()
        {
            decimal lambda = LambdaMax();
            return (size + 1) * lambda + 1;
        }

        public void RefreshMatrix(Matrix matrix)//обновляет матрицу симплекса на новую 
        {
            if (matrix.Row != matrix.Column + 1 && matrix.Row != this.matrix.Row)
            {
                throw new Exception("Матрица не подходит");
            }
            for (int i = 0; i < size + 1; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.matrix.Array[i, j] = matrix.Array[i, j];
                    this.array.Array[i, j] = matrix.Array[i, j];
                }
                this.matrix.Array[i, size] = 1;
            }
        }

        public void RefreshMatrix(Decimal[,] Array)//обновляет матрицу симплекса на новую 
        {
            Matrix newMatrix = new Matrix(this.matrix.Row, this.matrix.Column - 1, Array);
            RefreshMatrix(newMatrix);
        }

    }
}
