using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V2;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V2;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers.V2;

[ApiController]
[Route("/v2/[controller]")]
public class DeliveryPriceController : ControllerBase
{
	private readonly IPriceCalculatorService _priceCalculatorService;

	public DeliveryPriceController(
		IPriceCalculatorService priceCalculatorService)
	{
		_priceCalculatorService = priceCalculatorService;
	}

	/// <summary>
	/// Метод расчета стоимости доставки на основе объема товаров
	/// или веса товара. Окончательная стоимость принимается как наибольшая
	/// </summary>
	/// <param name="Height">В миллиметрах</param>
	/// <param name="Length">В миллиметрах</param>
	/// <param name="Width">В миллиметрах</param>
	/// <param name="Weight">В граммах</param>
	/// <returns></returns>
	[HttpPost("//v2/V2DeliveryPrise/calculate")]
	public CalculateResponse Calculate(
		CalculateRequest request)
	{
		var price = _priceCalculatorService.CalculatePrice(
			request.Goods
				.Select(x => new GoodModel(
					x.Height,
					x.Length,
					x.Width,
					x.Weight))
				.ToArray()
				, 1000 /* для версий ниже v3 расчет на 1 км (1000 м)*/);

		return new CalculateResponse(price);
	}

}