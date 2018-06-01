using System;
using WebStoreApi.Jobs.Models;

namespace WebStoreApi.Jobs.Helpers
{
    public static class ProblemDataHelper
    {
        private const int MaxDistance = 21000; // a bit more that half of earth on equator

        public static ProblemData Generate(OrdersProcessingInfo info)
        {
            var I = info.Storages.Count;
            var J = info.Storages[0].ProductCounts.Count;
            var K = info.Orders.Count;

            var table = new double[I + I * J + J * K + 1, I * J * K + 1];
            Func<int, int, int, int> getIndex = (i, j, k) => i * J * K + j * K + k + 1;

            // fill I first rows
            int row = 0;
            for (int i = 0; i < I; ++i)
            {
                table[row, 0] = info.Storages[i].DronesCount;
                for (int j = 0; j < J; ++j)
                {
                    for (int k = 0; k < K; ++k)
                    {
                        table[row, getIndex(i, j, k)] = 1;
                    }
                }

                ++row;
            }

            // fill I * J next rows
            for (int i = 0; i < I; ++i)
            {
                for (int j = 0; j < J; ++j)
                {
                    table[row, 0] = info.Storages[i].ProductCounts[j];
                    for (int k = 0; k < K; ++k)
                    {
                        table[row, getIndex(i, j, k)] = 1;
                    }

                    ++row;
                }
            }

            // fill all the rest but last rows
            for (int j = 0; j < J; ++j)
            {
                for (int k = 0; k < K; ++k)
                {
                    table[row, 0] = info.Orders[k].ProductCounts[j];
                    for (int i = 0; i < I; ++i)
                    {
                        table[row, getIndex(i, j, k)] = 1;
                    }

                    ++row;
                }
            }

            // fill last row
            table[row, 0] = 0;
            for (int i = 0, col = 1; i < I; ++i)
            {
                for (int j = 0; j < J; ++j)
                {
                    for (int k = 0; k < K; ++k)
                    {
                        table[row, col] =  info.Orders[k].Coordinates.Distance(info.Storages[i].Coordinates) - MaxDistance;
                        ++col;
                    }
                }
            }

            return new ProblemData()
            {
                Data = table,
                StoragesCount = I,
                ProductsCount = J,
                OrdersCount = K
            };
        }
    }
}
