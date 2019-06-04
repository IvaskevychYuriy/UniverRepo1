using System.Collections.Generic;
using System.Linq;

namespace WebStoreApi.Logic.BinPacker.Models
{
	public class ItemCollection
	{
		public int Id { get; set; }

		public List<Item> Items { get; set; }

		public float TotalWeight => Items.Sum(x => x.Weight);
	}
}
