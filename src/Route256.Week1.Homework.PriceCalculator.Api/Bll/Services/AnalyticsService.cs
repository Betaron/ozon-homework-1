using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.Analytics;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class AnalyticsService : IAnalyticsService
{
	private readonly IStorageRepository _storageRepository;
	private readonly Dictionary<int, Func<object>> _reports;

	public AnalyticsService(
		IStorageRepository storageRepository)
	{
		_storageRepository = storageRepository;

		_reports = new()
		{
			{ 1,  new(GenerateReport1)}
		};
	}

	public object GetAnalyticsReport(int reportId)
	{
		if (!_reports.ContainsKey(reportId))
			throw new ArgumentOutOfRangeException(nameof(reportId));
		return _reports[reportId].Invoke();
	}

	public Report1Model GenerateReport1()
	{
		var orders = _storageRepository.Query();

		if (!orders.Any())
			throw new ArgumentOutOfRangeException(nameof(orders), message: "History is empty");

		var maxWeightOrder = FindMaxWeightOrder(orders);
		var maxVolumeOrder = FindMaxVolumeOrder(orders);
		var wavgPrice = CalculateWavgPrice(orders);

		return new Report1Model(
			maxWeightOrder.Weight,
			maxVolumeOrder.Volume / 1000000,
			maxWeightOrder.Distance,
			maxVolumeOrder.Distance,
			wavgPrice);
	}

	private static decimal CalculateWavgPrice(in IEnumerable<StorageEntity> orders)
	{
		var wavg =
			orders.Sum(x => x.Price)
			/
			orders.Sum(x => x.GoodsQuantity);

		return wavg;
	}

	/// <summary>
	/// Метод ищет самый большой по объему заказ.
	/// Отбор производится также по расстоянию доставки.
	/// </summary>
	private static OrderModel FindMaxVolumeOrder(in IEnumerable<StorageEntity> orders)
	{
		var entity = orders.OrderByDescending(x => x.Volume).ThenByDescending(x => x.Distance).First();

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
	private static OrderModel FindMaxWeightOrder(in IEnumerable<StorageEntity> orders)
	{
		var entity = orders.OrderByDescending(x => x.Weight).ThenByDescending(x => x.Distance).First();

		return new(
			entity!.Volume,
			entity.Weight,
			entity.Price,
			entity.Distance,
			entity.GoodsQuantity
		);
	}
}
