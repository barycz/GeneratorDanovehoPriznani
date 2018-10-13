﻿// Author: Tomas Barak

using System;
using System.IO;
using System.Linq;
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
			DPHDP3?.Generate(ctx);
		}
	}

	public partial class PisemnostDPHDP3
	{
		public void Generate(GeneratorContext ctx)
		{
			VetaD?.Generate(ctx);
			Veta1 = new PisemnostDPHDP3Veta1();
			Veta1.Generate(ctx);
			Veta4 = new PisemnostDPHDP3Veta4();
			Veta4.Generate(ctx);
			Veta6 = new PisemnostDPHDP3Veta6();
			Veta6.Generate(ctx);
		}
	}

	public partial class PisemnostDPHDP3Veta1
	{
		public void Generate(GeneratorContext ctx)
		{
			var standardVATTransactions =
				from t in ctx.Transactions
				where t.VATRate == VATRate.Standard && t.Direction == Transaction.EDirection.Incoming
				select t;
			obrat23 = Math.Round((from t in standardVATTransactions select t.Value).Sum());
			obrat23FieldSpecified = true;
			dan23 = Math.Round((from t in standardVATTransactions select t.VAT).Sum());
			dan23Specified = true;
		}
	}

	public partial class PisemnostDPHDP3Veta4
	{
		public void Generate(GeneratorContext ctx)
		{
			var standardVATTransactions =
				from t in ctx.Transactions
				where t.VATRate == VATRate.Standard && t.Direction == Transaction.EDirection.Outgoing
				select t;
			pln23 = Math.Round((from t in standardVATTransactions select t.Value).Sum());
			pln23FieldSpecified = true;
			odp_tuz23_nar = Math.Round((from t in standardVATTransactions select t.VAT).Sum());
			odp_tuz23_narSpecified = true;
			odp_sum_nar = odp_tuz23_nar;
			odp_sum_narSpecified = true;
		}
	}

	public partial class PisemnostDPHDP3Veta6
	{
		public void Generate(GeneratorContext ctx)
		{
			var incommingTransVAT =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Incoming
				select t.VAT;
			dan_zocelk = Math.Round(incommingTransVAT.Sum());
			dan_zocelkSpecified = true;

			var outgoingTransVAT =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Outgoing
				select t.VAT;
			odp_zocelk = Math.Round(outgoingTransVAT.Sum());
			odp_zocelkSpecified = true;

			dano_da = dan_zocelk - odp_zocelk;
			dano_daSpecified = true;
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
