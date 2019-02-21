// Author: Tomas Barak

using System;
using System.Collections.Generic;
using System.Globalization;

namespace GeneratorDanovehoPriznani
{
	public class Transaction
	{
		public enum EDirection
		{
			Incoming,
			Outgoing
		}

		public uint MonthCode { get; }
		public DateTime Date { get; } // datum uskutecneni zdanitelneho plneni
		public EDirection Direction { get; }
		public string Id { get; }
		public decimal Value { get; }
		public VATRate VATRate { get; }
		public decimal VAT { get { return VATRate.CalculateVAT(Value); } }

		public Transaction(IList<object> row)
		{
			MonthCode = uint.Parse((string)row[0]);
			Date = DateTime.Parse((string)row[1]);
			Direction = (string)row[2] == "in" ? EDirection.Incoming : EDirection.Outgoing;
			Id = (string)row[3];
			Value = Convert.ToDecimal((string)row[4], new CultureInfo("en-US"));

			VATRate = VATRate.Standard;
		}
	}
}
