namespace WebStore.Api.DataTransferObjects
{
    public class ProductItemReportDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SoldCount { get; set; }
        public decimal Income { get; set; }
        public decimal Cost { get; set; }

        public decimal Profit { get; set; }
        public decimal AverageProfitPerItem { get; set; }
    }
}
