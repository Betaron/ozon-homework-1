using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.Analytics;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class AnalyticsService : IAnalyticsService
{
	private readonly IStorageRepository _storageRepository;

	public AnalyticsService(
		IStorageRepository storageRepository)
	{
		_storageRepository = storageRepository;
	}

	public decimal CalculateWavgPrice()
	{
		var wavg =
			_storageRepository.Query().Sum(x => x.Price)
			/
			_storageRepository.Query().Sum(x => x.GoodsQuantity);

		return wavg;
	}

	/// <summary>
	/// Метод ищет самый большой по объему заказ.
	/// Отбор производится также по расстоянию доставки.
	/// </summary>
	public OrderModel FindMaxVolumeOrder()
	{
		var entity = _storageRepository.Query()
			.OrderByDescending(x => x.Volume).ThenByDescending(x => x.Distance).First();

		return new(
			entity!.Volume,
			entity.Weight,
			entity.Price,
			entity.Distance,
			entity.GoodsQuantity
		);
	}

	/// <summary>
	/// Метод ищет самый большой по весу заказ.
	/// Отбор производится также по расстоянию доставки.
	/// </summary>
	public OrderModel FindMaxWeightOrder()
	{
		var entity = _storageRepository.Query()
			.OrderByDescending(x => x.Weight).ThenByDescending(x => x.Distance).First();

		return new(
			entity!.Volume,
			entity.Weight,
			entity.Price,
			entity.Distance,
			entity.GoodsQuantity
		);
	}
}
