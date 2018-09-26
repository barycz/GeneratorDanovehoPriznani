// Author: Tomas Barak

using System;

namespace GeneratorDanovehoPriznani
{
	public class GeneratorContext
	{
		public string AppName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
		public string AppVersion { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();  } }
		public DateTime Period { get; private set; }
		public DateTime SubmitDate { get; private set; }

		public GeneratorContext()
		{
			SubmitDate = DateTime.Today;
			Period = DateTime.Today.AddMonths(-1);
		}
	}
}
