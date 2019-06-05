using System.Collections.Generic;
using WebStoreApi.Logic.BinPacker.Models;
using Xunit;

namespace WebStoreApi.Logic.Tests
{
	public class BinPackerTests
	{
		[Fact]
		public void Pack_Should_Correctly_Shift_Items()
		{
			// Arrange
			var data = new BinPackerInput()
			{
				Bins = new List<Bin>()
				{
					new Bin() { Id = 1, Capacity = 120f },
					new Bin() { Id = 2, Capacity = 30 }
				},
				ItemSets = new List<ItemCollection>()
				{
					new ItemCollection()
					{
						Id = 1,
						Items = new List<Item>()
						{
							new Item() { Id = 1, Weight = 10 },
							new Item() { Id = 2, Weight = 20 }
						}
					}
				}
			};

			// Act
			var result = BinPacker.BinPacker.Pack(data);

			// Assert
			Assert.Single(result.Bins);
			Assert.Equal(2, result.Bins[0].Id);
			Assert.Equal(1, result.Bins[0].ItemSet.Id);
			Assert.Equal(2, result.Bins[0].ItemSet.Items.Count);
			Assert.Contains(result.Bins[0].ItemSet.Items, i => i.Id == 1 && i.Weight == 10);
			Assert.Contains(result.Bins[0].ItemSet.Items, i => i.Id == 2 && i.Weight == 20);
		}

		[Fact]
		public void Pack_Should_Place_Items_Into_Different_Bins_On_Different_Collections()
		{
			// Arrange
			var data = new BinPackerInput()
			{
				Bins = new List<Bin>()
				{
					new Bin() { Capacity = 2.5f, Id = 14 },
					new Bin() { Capacity = 3.2f, Id = 15 },
					new Bin() { Capacity = 5f,   Id = 16 },
					new Bin() { Capacity = 6f,   Id = 17 },
					new Bin() { Capacity = 4.4f, Id = 18 },
					new Bin() { Capacity = 4f,	 Id = 19 },
					new Bin() { Capacity = 3.5f, Id = 20 },
					new Bin() { Capacity = 3f,   Id = 21 },
					new Bin() { Capacity = 2.8f, Id = 34 }
				},
				ItemSets = new List<ItemCollection>()
				{
					new ItemCollection()
					{
						Id = 1029,
						Items = new List<Item>()
						{
							new Item() { Id = 1096, Weight = 0.3f },
							new Item() { Id = 1097, Weight = 1.1f },
							new Item() { Id = 1098, Weight = 1.1f }
						}
					},
					new ItemCollection()
					{
						Id = 1030,
						Items = new List<Item>()
						{
							new Item() { Id = 1099, Weight = 0.2f },
							new Item() { Id = 1100, Weight = 0.2f },
							new Item() { Id = 1101, Weight = 0.2f },
							new Item() { Id = 1102, Weight = 1.2f },
							new Item() { Id = 1103, Weight = 1f   }
						}
					},
					new ItemCollection()
					{
						Id = 1031,
						Items = new List<Item>()
						{
							new Item() { Id = 1104, Weight = 0.3f }
						}
					}
				}
			};

			// Act
			var result = BinPacker.BinPacker.Pack(data);

			// Assert
			Assert.Equal(3, result.Bins.Count);
		}

		[Fact]
		public void Pack_Should_Split_Items_Into_Different_Bins_For_Same_Collection_If_They_Dont_Fit()
		{
			// Arrange
			var data = new BinPackerInput()
			{
				Bins = new List<Bin>()
				{
					new Bin() { Capacity = 2.5f, Id = 14 },
					new Bin() { Capacity = 3.2f, Id = 15 },
					new Bin() { Capacity = 3f,   Id = 21 },
					new Bin() { Capacity = 2.8f, Id = 34 }
				},
				ItemSets = new List<ItemCollection>()
				{
					new ItemCollection()
					{
						Id = 1029,
						Items = new List<Item>()
						{
							new Item() { Id = 1096, Weight = 0.5f },
							new Item() { Id = 1097, Weight = 1.1f },
							new Item() { Id = 1098, Weight = 1.1f }
						}
					},
					new ItemCollection()
					{
						Id = 1030,
						Items = new List<Item>()
						{
							new Item() { Id = 1099, Weight = 1.2f },
							new Item() { Id = 1100, Weight = 0.5f },
							new Item() { Id = 1101, Weight = 0.2f },
							new Item() { Id = 1102, Weight = 1.2f },
							new Item() { Id = 1103, Weight = 1f   }
						}
					}
				}
			};

			// Act
			var result = BinPacker.BinPacker.Pack(data);

			// Assert
			Assert.Equal(3, result.Bins.Count);
		}
	}
}
