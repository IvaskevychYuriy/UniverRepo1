namespace WebStoreApi.Logic.BinPacker.Models
{
	public class PackedBin
	{
		public int Id { get; set; }

		public float Capacity { get; set; }

		public ItemCollection ItemSet { get; set; }
	}
}
