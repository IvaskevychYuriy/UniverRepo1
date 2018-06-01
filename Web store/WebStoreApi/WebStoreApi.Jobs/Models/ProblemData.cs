namespace WebStoreApi.Jobs.Models
{
    public class ProblemData
    {
        public int StoragesCount { get; set; }

        public int ProductsCount { get; set; }

        public int OrdersCount { get; set; }

        public double[,] Data { get; set; }
    }
}
