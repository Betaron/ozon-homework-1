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
	[HttpPost("reports/{reportId}")]
	public IActionResult GenerateStatisticReport(int reportId)
	{
		var report = _analyticsService.GetAnalyticsReport(reportId);
		return Ok(report);
	}
}