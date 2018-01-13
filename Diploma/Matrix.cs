using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class Matrix
    {
        private Decimal[,] array;
        private int row, column;

        public Decimal[,] Array
        {
            get { return array; }
            set { array = value; }
        }

        public int Row { get { return row; } }
        public int Column { get { return column; } }

        public Matrix(int Row, int Column, Decimal[,] Array)
        {
            this.row = Row;
            this.column = Column;
            this.array = new Decimal[row, column];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    this.array[i, j] = Array[i, j];
                }
        }
        public Matrix(int Row, int Column)
        {
            this.row = Row;
            this.column = Column;
            array = new Decimal[row, column];
        }
        public Matrix(Matrix Matrix)
        {
            this.column = Matrix.Column;
            this.row = Matrix.Row;
            this.array = new Decimal[row, column];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    this.array[i, j] = Matrix.array[i, j];
                }
        }

        // Возвращает транспонированную матрицу
        public Matrix Transpose()
        {
            Matrix res = new Matrix(column, row);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    res.array[j, i] = array[i, j];
                }
            }

            return res;
        }

        // Применяет результат транспонирования на себя
        public void TransposeMyself()
        {
            array = Transpose().array;
        }

        // Возвращает обратную матрицу
        public Matrix Inverse()
        {
            Decimal det = Determinant();
            if (det == 0)
            {
                throw new Exception("Матрица вырождена");
            }

            Matrix res = new Matrix(row, column);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    res.array[i, j] = Cofactor(array, i, j) / det;
                }
            }

            return res.Transpose();
        }

        // Возвращает определитель
        public Decimal Determinant()
        {
            if (column != row)
            {
                throw new Exception("Расчет определителя невозможен");
            }
            return Determinant(array);
        }

        // Считает определитель
        private Decimal Determinant(Decimal[,] array)
        {
            int n = (int)Math.Sqrt(array.Length);

            if (n == 1)
            {
                return array[0, 0];
            }

            Decimal det = 0;

            for (int k = 0; k < n; k++)
            {
                det += array[0, k] * Cofactor(array, 0, k);
            }

            return det;
        }

        private Decimal Cofactor(Decimal[,] array, int row, int column)
        {
            return Convert.ToDecimal(Math.Pow(-1, column + row)) * Determinant(Minor(array, row, column));
        }

        private Decimal[,] Minor(Decimal[,] array, int row, int column)
        {
            int n = (int)Math.Sqrt(array.Length);
            Decimal[,] minor = new Decimal[n - 1, n - 1];

            int _i = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == row)
                {
                    continue;
                }
                int _j = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == column)
                    {
                        continue;
                    }
                    minor[_i, _j] = array[i, j];
                    _j++;
                }
                _i++;
            }
            return minor;
        }

        // Возвращает заданную строчку
        public Decimal[] GetRow(int IndexRow)
        {
            Decimal[] resault = new Decimal[column];
            for (int i = 0; i < column; i++)
            {
                resault[i] = array[IndexRow, i];
            }
            return resault;
        }

        // Изменяет заданную строчку
        public bool SetRow(int IndexRow, Decimal[] Row)
        {
            if (column != Row.Length || IndexRow < 0 || IndexRow > row - 1)
                return false;
            for (int i = 0; i < column; i++)
            {
                array[IndexRow, i] = Row[i];
            }
            return true;
        }

        // Скалярное произведение самого на себя
        public Decimal Scalar()
        {
            decimal res = 0;
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    res += array[i, j] * array[i, j];
                }
            }
            return res;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Row != m2.Row || m1.Column != m2.Column)
            {
                throw new Exception("Сложение невозможно");
            }

            Matrix res = new Matrix(m1.Row, m1.Column);

            for (int i = 0; i < m1.Row; i++)
            {
                for (int j = 0; j < m1.Column; j++)
                {
                    res.Array[i, j] = m1.Array[i, j] + m2.Array[i, j];
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Row != m2.Row || m1.Column != m2.Column)
            {
                throw new Exception("Вычитание невозможно");
            }

            Matrix res = new Matrix(m1.Row, m1.Column);

            for (int i = 0; i < m1.Row; i++)
            {
                for (int j = 0; j < m1.Column; j++)
                {
                    res.Array[i, j] = m1.Array[i, j] - m2.Array[i, j];
                }
            }

            return res;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Column != m2.Row)
            {
                throw new Exception("Умножение невозможно");
            }

            Matrix res = new Matrix(m1.Row, m2.Column);

            for (int i = 0; i < m1.Row; i++)
            {
                for (int j = 0; j < m2.Column; j++)
                {
                    decimal sum = 0;

                    for (int k = 0; k < m1.Column; k++)
                    {
                        sum += m1.Array[i, k] * m2.Array[k, j];
                    }

                    res.Array[i, j] = sum;
                }
            }

            return res;
        }

        public static Matrix operator *(Matrix m1, decimal a)
        {
            Matrix res = new Matrix(m1.row, m1.column);
            for (int i = 0; i < m1.Row; i++)
            {
                for (int j = 0; j < m1.Column; j++)
                {
                    res.Array[i, j] = m1.Array[i, j] * a;
                }
            }
            return res;
        }

        public override string ToString()
        {
            string str = "";

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    str += array[i, j] + "\t";
                }
                str += "\n";
            }

            return str;
        }
    }
}
