// Author: Tomas Barak

namespace GeneratorDanovehoPriznani
{
	public class Transaction
	{
		public enum EDirection
		{
			Incoming,
			Outgoing
		}

		public uint MonthCode { get; set; }
		public EDirection Direction { get; set; }
		public string Id { get; set; }
		public decimal Value { get; set; }
		public VATRate VATRate { get; set; }
		public decimal VAT { get { return VATRate.CalculateVAT(Value); } }
	}
}
