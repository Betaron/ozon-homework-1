namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.Analytics;

public record OrderModel(
	decimal Volume,
	decimal Weight,
	decimal Price,
	decimal Distance,
	int GoodsQuantity);
