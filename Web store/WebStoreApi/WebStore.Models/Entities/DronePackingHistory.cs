namespace WebStore.Models.Entities
{
	public class DronePackingHistory : EntityBase<int>
	{
		public int OrderId { get; set; }
		public Order Order { get; set; }

		public float MaxWeight { get; set; }
		public float LoadedWeight { get; set; }
	}
}
