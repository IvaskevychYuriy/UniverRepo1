namespace WebStore.Api.DataTransferObjects
{
	public class DroneUtilizationReportDTO
	{
		public double TotalAverage { get; set; }

		public double MaxPerOrder { get; set; }

		public double MinPerOrder { get; set; }

		public double ItemsPerDronePerOrderAverage { get; set; }
	}
}
