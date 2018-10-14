// Author: Tomas Barak

namespace GeneratorDanovehoPriznani
{
	class Program
	{
		static void Main(string[] args)
		{
			var inPath = "../../dph3-in.xml";
			var outPath = "../../dph3.xml";

			var dph3 = DPH3.Pisemnost.FromXml(inPath);
			var ctx = new GeneratorContext();
			ctx.FetchData();

			dph3.Generate(ctx);
			dph3.ToXml(outPath);
		}
	}
}
