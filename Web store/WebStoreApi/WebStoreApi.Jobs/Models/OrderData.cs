using System.Collections.Generic;
using System.Linq;
using WebStore.Models.Models;

namespace WebStoreApi.Jobs.Models
{
	public class OrderData
	{
		public int OrderId { get; set; }

		public int Priority { get; set; }

		public AddressCoordinates Coordinates { get; set; }

		public List<CartItemData> Products { get; set; }


		public float TotalWeight => Products.Sum(p => p.Weight);
	}
}
