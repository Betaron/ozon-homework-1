namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.Analytics;

public record Report1Model(
	decimal MaxWeight,
	decimal MaxVolume,
	decimal MaxDistanceForHeaviestGood,
	decimal MaxDistanceForLargestGood,
	decimal WavgPrice);