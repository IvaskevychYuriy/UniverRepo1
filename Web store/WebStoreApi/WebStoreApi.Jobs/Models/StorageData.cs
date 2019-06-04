using System.Collections.Generic;
using WebStore.Models.Models;

namespace WebStoreApi.Jobs.Models
{
	public class StorageData
	{
		public int StorageId { get; set; }

		public AddressCoordinates Coordinates { get; set; }

		public List<DroneData> Drones { get; set; }
	}
}
