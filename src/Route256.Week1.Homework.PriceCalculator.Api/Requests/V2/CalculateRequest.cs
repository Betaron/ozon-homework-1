namespace Route256.Week1.Homework.PriceCalculator.Api.Requests.V2;

/// <summary>
/// Товары, чью цену транспортировки нужно рассчитать
/// </summary>
public record CalculateRequest(
    GoodProperties[] Goods
    );