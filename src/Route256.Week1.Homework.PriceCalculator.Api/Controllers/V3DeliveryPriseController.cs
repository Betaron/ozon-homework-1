using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V3;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V3;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v3/[controller]")]
public class V3DeliveryPriseController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V3DeliveryPriseController(
        IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая.
    /// Стоимость увеличивается пропорционально расстоянию.
    /// </summary>
    /// <param name="Height">В метрах</param>
    /// <param name="Length">В метрах</param>
    /// <param name="Width">В метрах</param>
    /// <param name="Weight">В килограммах</param>
    /// <param name="Distance">В метрах</param>
    /// <returns></returns>
    [HttpPost("calculate")]
    public CalculateResponse Calculate(
        CalculateRequest request)
    {
        var price = _priceCalculatorService.CalculatePrice(
            request.Goods
                .Select(x => new GoodModel(
                    x.Height * 1000,
                    x.Length * 1000,
                    x.Width * 1000,
                    x.Weight * 1000))
                .ToArray(),
            request.Distance);

        return new CalculateResponse(price);
    }

    /// <summary>
    /// Метод получения истории вычисления.
    /// Все значения выводятся в единицах СИ.
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public GetHistoryResponse[] History(GetHistoryRequest request)
    {
        var log = _priceCalculatorService.QueryLog(request.Take);

        return log
            .Select(x => new GetHistoryResponse(
                new CargoResponse(
                    x.Volume / 1000000M,    // м^3
                    x.Weight),              // кг
                x.Price,                    // у.е.
                x.Distance))                // м
            .ToArray();
    }
}