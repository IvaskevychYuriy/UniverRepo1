using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class TotalReportDTO : IDataTransferObject
    {
        public int SoldCount { get; set; }

        public decimal Income { get; set; }
        public decimal Cost { get; set; }

        public decimal Profit { get; set; }
    }
}
