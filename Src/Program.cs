// Author: Tomas Barak

namespace GeneratorDanovehoPriznani
{
	class Program
	{
		static void ProcessDPH3(GeneratorContext ctx)
		{
			var inPath = "../../dph3-in.xml";
			var dph3 = DPH3.Pisemnost.FromXml(inPath);

			dph3.Generate(ctx);

			var outPath = string.Format("../../{0}-dph3.xml", ctx.PeriodMonthCode);
			dph3.ToXml(outPath);
		}

		static void ProcessKH1(GeneratorContext ctx)
		{
			var inPath = "../../kh1-in.xml";
			var kh1 = KH1.Pisemnost.FromXml(inPath);

			kh1.Generate(ctx);

			var outPath = string.Format("../../{0}-kh1.xml", ctx.PeriodMonthCode);
			kh1.ToXml(outPath);
		}

		static void Main(string[] args)
		{
			var ctx = new GeneratorContext();
			ctx.FetchData();

			ProcessDPH3(ctx);
			ProcessKH1(ctx);
		}
	}
}
