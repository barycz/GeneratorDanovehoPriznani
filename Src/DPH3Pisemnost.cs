// Author: Tomas Barak

using System.IO;
using System.Xml.Serialization;

namespace GeneratorDanovehoPriznani.DPH3
{
	public partial class Pisemnost
	{
		public static Pisemnost FromXml(string fileName)
		{
			var serializer = new XmlSerializer(typeof(Pisemnost));
			var reader = new StreamReader(fileName);
			var dph3 = (Pisemnost)serializer.Deserialize(reader);
			return dph3;
		}

		public void ToXml(string fileName)
		{
			var serializer = new XmlSerializer(typeof(Pisemnost));
			var writer = new StreamWriter(fileName);
			serializer.Serialize(writer, this);
		}

		public void Generate(GeneratorContext ctx)
		{
			nazevSW = ctx.AppName;
			verzeSW = ctx.AppVersion;
			DPHDP3?.VetaD?.Generate(ctx);
		}
	}

	public partial class PisemnostDPHDP3VetaD
	{
		public void Generate(GeneratorContext ctx)
		{
			mesic = ctx.Period.Month;
			rok = ctx.Period.Year;
			d_poddp = ctx.SubmitDate.ToString("d.M.yyyy");
		}
	}
}
