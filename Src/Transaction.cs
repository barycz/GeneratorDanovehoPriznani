﻿// Author: Tomas Barak

using System;
using System.Collections.Generic;
using System.Linq;
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

		public enum ELocation
		{
			Domestic,
			EU,
			ThirdCountry
		}

		public static int MonthToQuartal(int month) => (month - 1) / 3 + 1;

        public static string CreateQuarterCode(uint monthCode)
        {
			return string.Format("{0}q{1}", monthCode / 100, MonthToQuartal((int)(monthCode % 100)));
        }

        public static decimal AnnonymousInKHThreshold = 10000;
		public uint MonthCode { get; }
		public string QuarterCode => CreateQuarterCode(MonthCode);
		public DateTime Date { get; } // datum uskutecneni zdanitelneho plneni
		public EDirection Direction { get; }
		public ELocation Location { get; }
		public string VATId { get; } // danove identifikacni cislo(DIC) of the other party
		public string Id { get; }
		public decimal Value { get; } // without the tax
		public VATRate VATRate { get; }
		public decimal VAT { get { return VATRate.CalculateVAT(Value); } }
		public decimal ValueWithVAT { get { return Value + VAT; } }
		public bool IsAnnonymousInKH { get { return ValueWithVAT < AnnonymousInKHThreshold; } }

		public Transaction(IList<object> row)
		{
			MonthCode = Convert.ToUInt32(row[0]);
			Date = DateTime.Parse((string)row[1]);
			Direction = (string)row[2] == "in" ? EDirection.Incoming : EDirection.Outgoing;
			Location = (string)row[2] == "ou3rd" ? ELocation.ThirdCountry : ((string)row[2] == "outeu" ? ELocation.EU : ELocation.Domestic);
			VATId = Convert.ToString(row[3]);
			Id = Convert.ToString(row[4]);
			Value = Convert.ToDecimal(row[5]);

			VATRate = VATRate.Standard;
		}
	}

	public static class IEnumerableTransactionExtensions
	{
		public static decimal TotalRoundedValue(this IEnumerable<Transaction> transactions)
		{
			return Math.Round((from t in transactions select t.Value).Sum());
		}

		public static decimal TotalRoundedVAT(this IEnumerable<Transaction> transactions)
		{
			return Math.Round((from t in transactions select t.VAT).Sum());
		}
	}
}
