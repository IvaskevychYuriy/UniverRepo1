using System.Collections.Generic;
using System.Linq;
using WebStoreApi.Logic.BinPacker.Models;

namespace WebStoreApi.Logic.BinPacker
{
	public static class BinPacker
	{
		public static BinPackerResult Pack(BinPackerInput data)
		{
			data = PrepareData(data);
			var packedData = PackData(data);

			return new BinPackerResult()
			{
				Bins = packedData
			};
		}

		private static List<PackedBin> GetBinCandidates(List<Bin> bins)
		{
			return bins
				.Select(b => new PackedBin()
				{
					Id = b.Id,
					Capacity = b.Capacity,
					ItemSet = new ItemCollection() { Items = new List<Item>() }
				})
				.ToList();
		}

		private static BinPackerInput PrepareData(BinPackerInput data)
		{
			data.ItemSets = data.ItemSets
				.OrderByDescending(s => s.Items.Count)
				.ThenByDescending(s => s.TotalWeight)
				.Select(s => new ItemCollection()
				{
					Id = s.Id,
					Items = s.Items
						.OrderByDescending(i => i.Weight)
						.ToList()
				})
				.ToList();

			data.Bins = data.Bins
				.OrderByDescending(b => b.Capacity)
				.ToList();

			return data;
		}
		
		private static List<PackedBin> PackData(BinPackerInput data)
		{
			var result = new List<PackedBin>();
			var availableBins = GetBinCandidates(data.Bins);

			foreach (var collection in data.ItemSets)
			{
				if (!availableBins.Any())
				{
					break;
				}

				var collectionResult = FirstFitDescending(availableBins, collection);
				if (collectionResult.Any())
				{
					collectionResult = RepackTighter(collectionResult, availableBins);
					result.AddRange(collectionResult);
					availableBins = availableBins
						.Where(b => !collectionResult.Any(r => r.Id == b.Id))
						.ToList();
				}
			}

			return result;
		}

		private static List<PackedBin> FirstFitDescending(List<PackedBin> bins, ItemCollection itemsCollection)
		{
			bins = bins
				.Select(b => new PackedBin()
				{
					Id = b.Id,
					Capacity = b.Capacity,
					ItemSet = new ItemCollection() { Items = new List<Item>() }
				})
				.ToList();

			foreach(var item in itemsCollection.Items)
			{
				var bin = bins.FirstOrDefault(b => item.Weight <= b.Capacity - b.ItemSet.TotalWeight);
				if (bin != null)
				{
					bin.ItemSet.Items.Add(item);
				}
			}

			var result = bins
				.Where(b => b.ItemSet.Items.Any())
				.ToList();

			foreach (var b in result)
			{
				b.ItemSet.Id = itemsCollection.Id;
			}

			return result;
		}
		
		private static List<PackedBin> RepackTighter(List<PackedBin> packedBins, List<PackedBin> availableBins)
		{
			if (!packedBins.Any() || packedBins.Count == availableBins.Count)
			{
				return packedBins;
			}

			packedBins = packedBins.OrderBy(b => b.ItemSet.TotalWeight).ToList();
			availableBins = availableBins
				.OrderBy(b => b.Capacity)
				.Select(b => new PackedBin()
				{
					Id = b.Id,
					Capacity = b.Capacity,
					ItemSet = new ItemCollection() { Id = b.ItemSet.Id, Items = new List<Item>() }
				})
				.ToList();

			foreach (var bin in packedBins)
			{
				var smallerBinIndex = availableBins.FindIndex(b => b.ItemSet.Id == 0 && b.Capacity >= bin.ItemSet.TotalWeight);
				if (smallerBinIndex != -1)
				{
					availableBins[smallerBinIndex].ItemSet = bin.ItemSet;
				}
			}

			return availableBins
				.Where(b => b.ItemSet.Items.Any())
				.ToList();
		}
	}
}
