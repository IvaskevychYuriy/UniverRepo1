using System.Collections.Generic;
using System.Linq;
using WebStoreApi.Logic.BinPacker.Models;
using Xunit;

namespace WebStoreApi.Logic.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var data = new BinPackerInput()
			{
				Bins = new List<Bin>()
				{
					new Bin() { Id = 1, Capacity = 120f },
					new Bin() { Id = 2, Capacity = 30f }
				},
				ItemSets = new List<ItemCollection>()
				{
					new ItemCollection()
					{
						Id = 1,
						Items = new List<Item>()
						{
							new Item() { Id = 1, Weight = 10f },
							new Item() { Id = 2, Weight = 20f }
						}
					}
				}
			};

			var result = BinPacker.BinPacker.Pack(data);

			Assert.Single(result.Bins);
			Assert.Equal(2, result.Bins[0].Id);
			Assert.Equal(1, result.Bins[0].ItemSet.Id);
			Assert.Equal(2, result.Bins[0].ItemSet.Items.Count);
			Assert.Contains(result.Bins[0].ItemSet.Items, i => i.Id == 1 && i.Weight == 10f);
			Assert.Contains(result.Bins[0].ItemSet.Items, i => i.Id == 2 && i.Weight == 20f);
		}
	}
}
