namespace WebStore.Api.DataTransferObjects
{
    public class TotalReportDTO
    {
        public int SoldCount { get; set; }

        public decimal Income { get; set; }
        public decimal Cost { get; set; }

        public decimal Profit { get; set; }
    }
}
