using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V1;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers.V1;

[ApiController]
[Route("/v1/[controller]")]
public class DeliveryPriceController : ControllerBase
{
	private readonly IPriceCalculatorService _priceCalculatorService;
	private readonly IAnalyticsService _analyticsService;

	public DeliveryPriceController(
		IPriceCalculatorService priceCalculatorService,
		IAnalyticsService analyticsService)
	{
		_priceCalculatorService = priceCalculatorService;
		_analyticsService = analyticsService;
	}

	/// <summary>
	/// Метод расчета стоимости доставки на основе объема товаров
	/// </summary>
	/// <param name="Height">В миллиметрах</param>
	/// <param name="Length">В миллиметрах</param>
	/// <param name="Width">В миллиметрах</param>
	/// <returns></returns>
	[HttpPost("//v1/V1DeliveryPrise/calculate")]
	public CalculateResponse Calculate(
		CalculateRequest request)
	{
		var price = _priceCalculatorService.CalculatePrice(
			request.Goods
				.Select(x => new GoodModel(
					x.Height,
					x.Length,
					x.Width,
					0 /* для v1 рассчет по весу не предусмотрен */))
				.ToArray()
				, 1000 /* для версий ниже v3 расчет на 1 км (1000 м)*/);

		return new CalculateResponse(price);
	}

	/// <summary>
	/// Метод получения истории вычисления
	/// </summary>
	/// <returns></returns>
	[HttpPost("//v1/V1DeliveryPrise/get-history")]
	public GetHistoryResponse[] History(GetHistoryRequest request)
	{
		var log = _priceCalculatorService.QueryLog(request.Take);

		return log
			.Select(x => new GetHistoryResponse(
				new CargoResponse(
					x.Volume,
					x.Weight),
				x.Price))
			.ToArray();
	}

	/// <summary>
	/// Полностью очищает историю заказов
	/// </summary>
	/// <returns></returns>
	[HttpPost("delete-history")]
	public DeleteHistoryResponse Delete()
	{
		_priceCalculatorService.ClearHistory();

		return new();
	}

	/// <summary>
	/// Формирует отчет со статистикой для анализа.
	/// </summary>
	/// <returns>
	/// max_weight - наибольший вес товара среди всех расчетов<br/>
	/// max_volume - наибольший объем товара среди всех расчетов<br/>
	/// max_distance_for_heaviest_good - расстояние, на которое был перевезен товар с наибольшим весом<br/>
	/// max_distance_for_largest_good - расстояние, на которое был перевезен товар с наибольшим объемом<br/>
	/// wavg_price - средневзвешенная стоимость доставки
	/// </returns>
	[HttpPost("reports/01")]
	public StatisticReport1Responce StatisticReport1()
	{
		var maxWeightOrder = _analyticsService.FindMaxWeightOrder();
		var maxVolumeOrder = _analyticsService.FindMaxVolumeOrder();
		var wavgPrice = _analyticsService.CalculateWavgPrice();

		return new(
			maxWeightOrder.Weight,
			maxVolumeOrder.Volume / 1000000,
			maxWeightOrder.Distance,
			maxVolumeOrder.Distance,
			wavgPrice);
	}
}