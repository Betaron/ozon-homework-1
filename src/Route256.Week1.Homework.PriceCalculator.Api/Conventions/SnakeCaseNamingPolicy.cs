using System.Text.Json;

namespace Route256.Week1.Homework.PriceCalculator.Api.Conventions;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
	public override string ConvertName(string name) =>
		string.Concat(name.Select((character, index) =>
				index > 0 && char.IsUpper(character)
					? "_" + character
					: character.ToString()))
			.ToLower();
}
