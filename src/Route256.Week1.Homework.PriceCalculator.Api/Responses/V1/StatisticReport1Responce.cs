﻿namespace Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

public record StatisticReport1Responce(
	decimal MaxWeight,
	decimal MaxVolume,
	decimal MaxDistanceForHeaviestGood,
	decimal MaxDistanceForLargestGood,
	decimal WavgPrice);
