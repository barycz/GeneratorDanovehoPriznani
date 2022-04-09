// Author: Tomas Barak

using System;
using System.Collections.Generic;

namespace GeneratorDanovehoPriznani
{
	public class GeneratorContext
	{
		public string AppName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
		public string AppVersion { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();  } }

		public DateTime Period { get; private set; }
		public uint PeriodMonthCode { get { return (uint)Period.Year * 100 + (uint)Period.Month; } }
		public DateTime SubmitDate { get; private set; }
		private Book Book { get; set; }
		public IEnumerable<Transaction> Transactions { get { return Book.Transactions; } }

		public GeneratorContext()
		{
			SubmitDate = DateTime.Today;
			Period = DateTime.Today.AddMonths(-1);
		}

		public void FetchData()
		{
			Book = new Book(t => t.MonthCode == PeriodMonthCode);
			Book.LoadFromSheets();
		}
	}
}
