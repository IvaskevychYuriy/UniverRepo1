using System.Collections.Generic;

namespace WebStoreApi.Jobs.Models
{
	public class ProcessingData
	{
		public StorageData Storage { get; set; }

		public List<OrderData> Orders { get; set; }
	}
}
