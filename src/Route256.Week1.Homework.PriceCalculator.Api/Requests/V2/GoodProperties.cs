namespace Route256.Week1.Homework.PriceCalculator.Api.Requests.V2;

/// <summary>
/// Характеристики товара
/// </summary>
public record GoodProperties(
    int Height,
    int Length,
    int Width,
    int Weight);