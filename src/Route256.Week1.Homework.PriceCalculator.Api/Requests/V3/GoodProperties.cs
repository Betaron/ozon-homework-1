namespace Route256.Week1.Homework.PriceCalculator.Api.Requests.V3;

/// <summary>
/// Характеристики товара
/// </summary>
public record GoodProperties(
	decimal Height,
	decimal Length,
	decimal Width,
	decimal Weight);