using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.Analytics;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface IAnalyticsService
{
	object GetAnalyticsReport(int reportId);

	/// <returns>
	/// max_weight - наибольший вес товара среди всех расчетов<br/>
	/// max_volume - наибольший объем товара среди всех расчетов<br/>
	/// max_distance_for_heaviest_good - расстояние, на которое был перевезен товар с наибольшим весом<br/>
	/// max_distance_for_largest_good - расстояние, на которое был перевезен товар с наибольшим объемом<br/>
	/// wavg_price - средневзвешенная стоимость доставки
	/// </returns>
	Report1Model GenerateReport1();
}
