// Author: Tomas Barak

namespace GeneratorDanovehoPriznani
{
	public class VATRate
	{
		public static VATRate Standard = new VATRate(1.21m);

		decimal Rate { get; set; }

		VATRate(decimal rate)
		{
			Rate = rate;
		}

		public decimal CalculateVAT(decimal value) => value * (Rate - 1m);
	}
}
