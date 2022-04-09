// Author: Tomas Barak

using System;
using System.Collections.Generic;

namespace GeneratorDanovehoPriznani
{
	public class GeneratorContext
	{
		public string AppName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
		public string AppVersion { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();  } }

		public enum PeriodType { Month = 1, Quarter = 3 }

		public PeriodType Period { get; }

		public int ForMonth { get; }
		public int ForQuarter => Transaction.MonthToQuartal(ForMonth);
		public int ForYear { get; }

		public uint PeriodMonthCode { get { return (uint)ForYear * 100 + (uint)ForMonth; } }
		public string PeriodQuarterCode => Transaction.CreateQuarterCode(PeriodMonthCode);
		public DateTime SubmitDate { get; private set; }
		private Book Book { get; set; }
		public IEnumerable<Transaction> Transactions { get { return Book.Transactions; } }

		public string OutputNameBase => Period == PeriodType.Month ? PeriodMonthCode.ToString() : PeriodQuarterCode;

		public GeneratorContext()
		{
			SubmitDate = DateTime.Today;
			Period = PeriodType.Quarter;
			ForMonth = DateTime.Today.AddMonths(-1).Month;
			ForYear = DateTime.Today.AddMonths(-1).Year;
		}

		public void FetchData()
		{
			if (Period == PeriodType.Month)
			{
				Book = new Book(t => t.MonthCode == PeriodMonthCode);
			}
			else
            {
				Book = new Book(t => t.QuarterCode == PeriodQuarterCode);
			}
			Book.LoadFromSheets();
		}
	}
}
