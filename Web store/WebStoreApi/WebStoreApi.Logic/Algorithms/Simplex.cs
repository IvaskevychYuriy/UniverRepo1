using System.Collections.Generic;

namespace WebStoreApi.Logic.Algorithms
{
    public class Simplex
    {
        private double[,] _table; // simplex table
        private List<int> _basis; // basis variables

        private readonly int M, N;
        private const int MaxIterations = 100;

        public Simplex(double[,] source)
        {
            M = source.GetLength(0);
            N = source.GetLength(1);
            _table = new double[M, N + M - 1];
            _basis = new List<int>();

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < _table.GetLength(1); j++)
                {
                    _table[i, j] = j < N ? source[i, j] : 0;
                }

                // put coefficient 1 before basis variable in the row
                if ((N + i) < _table.GetLength(1))
                {
                    _table[i, N + i] = 1;
                    _basis.Add(N + i);
                }
            }

            N = _table.GetLength(1);
        }

        public double[,] Calculate(int[] result)
        {
            int mainCol, mainRow;
            int iterations = 0;

            while (!Done(iterations))
            {
                ++iterations;
                mainCol = FindPivotColumn();
                mainRow = FindPivotRow(mainCol);
                _basis[mainRow] = mainCol;

                double[,] newTable = new double[M, N];
                for (int j = 0; j < N; j++)
                {
                    newTable[mainRow, j] = _table[mainRow, j] / _table[mainRow, mainCol];
                }

                for (int i = 0; i < M; i++)
                {
                    if (i == mainRow)
                    {
                        continue;
                    }

                    for (int j = 0; j < N; j++)
                    {
                        newTable[i, j] = _table[i, j] - _table[i, mainCol] * newTable[mainRow, j];
                    }
                }

                _table = newTable;
            }

            // write result
            for (int i = 0; i < result.Length; i++)
            {
                int k = _basis.IndexOf(i + 1);
                result[i] = k != -1 ? (int)_table[k, 0] : 0;
            }
            
            return _table;
        }

        private bool Done(int iterations)
        {
            if (iterations > MaxIterations)
            {
                return true;
            }

            for (int j = 1; j < N; j++)
            {
                if (_table[M - 1, j] < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private int FindPivotColumn()
        {
            int pivotCol = 1;

            for (int j = 2; j < N; j++)
                if (_table[M - 1, j] < _table[M - 1, pivotCol])
                    pivotCol = j;

            return pivotCol;
        }

        private int FindPivotRow(int mainCol)
        {
            int pivotRow = 0;

            for (int i = 0; i < M - 1; i++)
            {
                if (_table[i, mainCol] > 0)
                {
                    pivotRow = i;
                    break;
                }
            }
            for (int i = pivotRow + 1; i < M - 1; i++)
                if ((_table[i, mainCol] > 0) && ((_table[i, 0] / _table[i, mainCol]) < (_table[pivotRow, 0] / _table[pivotRow, mainCol])))
                    pivotRow = i;

            return pivotRow;
        }
    }
}
