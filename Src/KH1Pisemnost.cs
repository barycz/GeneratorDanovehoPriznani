// Author: Tomas Barak

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace GeneratorDanovehoPriznani.KH1
{
	public partial class Pisemnost
	{
		public static Pisemnost FromXml(string fileName)
		{
			var serializer = new XmlSerializer(typeof(Pisemnost));
			var reader = new StreamReader(fileName);
			var kh1 = (Pisemnost)serializer.Deserialize(reader);
			return kh1;
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
			DPHKH1?.Generate(ctx);
		}
	}

	public partial class PisemnostDPHKH1
	{
		public void Generate(GeneratorContext ctx)
		{
			VetaD?.Generate(ctx);
			VetaA1 = null;
			VetaA2 = null;
			VetaA3 = null;
			VetaA4 = null;
			VetaB1 = null;
			VetaB2 = null;
			VetaC = new PisemnostDPHKH1VetaC();
			VetaC.Generate(ctx);
		}
	}

	public partial class PisemnostDPHKH1VetaD
	{
		public void Generate(GeneratorContext ctx)
		{
			mesic = ctx.Period.Month;
			rok = ctx.Period.Year;
			d_poddp = ctx.SubmitDate.ToString("d.M.yyyy");
		}
	}

	public partial class PisemnostDPHKH1VetaC
	{
		public void Generate(GeneratorContext ctx)
		{
			var incommingValue =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Incoming
				select t.Value;
			obrat23 = Math.Round(incommingValue.Sum());
			obrat23Specified = true;

			var outgoingValue =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Outgoing
				select t.Value;
			pln23 = Math.Round(outgoingValue.Sum());
			pln23Specified = true;
		}
	}
}
