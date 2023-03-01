using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V1;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class V1DeliveryPriseController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V1DeliveryPriseController(
        IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// </summary>
    /// <param name="Height">В миллиметрах</param>
    /// <param name="Length">В миллиметрах</param>
    /// <param name="Width">В миллиметрах</param>
    /// <returns></returns>
    [HttpPost("calculate")]
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
    [HttpPost("get-history")]
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
    [HttpDelete]
    public DeleteHistoryResponse Delete()
    {
        _priceCalculatorService.ClearHistory();

        return new();
    }
}