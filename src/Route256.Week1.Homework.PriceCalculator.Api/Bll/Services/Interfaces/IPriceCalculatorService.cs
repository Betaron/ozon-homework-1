using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, int Distance);

    CalculationLogModel[] QueryLog(int take);

    void ClearHistory();
}