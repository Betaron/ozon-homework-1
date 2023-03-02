using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.Analytics;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface IAnalyticsService
{
	OrderModel FindMaxWeightOrder();
	OrderModel FindMaxVolumeOrder();
	decimal CalculateWavgPrice();
}
