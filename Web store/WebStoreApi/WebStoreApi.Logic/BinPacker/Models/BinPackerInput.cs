using System.Collections.Generic;

namespace WebStoreApi.Logic.BinPacker.Models
{
	public class BinPackerInput
	{
		public List<Bin> Bins { get; set; }

		public List<ItemCollection> ItemSets { get; set; }
	}
}
