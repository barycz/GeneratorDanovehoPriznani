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
			VetaA4 = PisemnostDPHKH1VetaA4.CreateVetaA4Array(ctx);
			VetaA5 = PisemnostDPHKH1VetaA5.CreateIfNeeded(ctx);

			VetaB1 = null;
			VetaB2 = PisemnostDPHKH1VetaB2.CreateVetaB2Array(ctx);
			VetaB3 = PisemnostDPHKH1VetaB3.CreateIfNeeded(ctx);

			VetaC = new PisemnostDPHKH1VetaC();
			VetaC.Generate(ctx);
		}
	}

	public partial class PisemnostDPHKH1VetaA4
	{
		public static PisemnostDPHKH1VetaA4[] CreateVetaA4Array(GeneratorContext ctx)
		{
			var inNotAnnonTrans =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Incoming && t.IsAnnonymousInKH == false
				select t;

			var inNotAnnonTransList = inNotAnnonTrans.ToList();

			var ret = new PisemnostDPHKH1VetaA4[inNotAnnonTransList.Count];
			for (var i = 0; i < inNotAnnonTransList.Count; ++i)
			{
				ret[i] = new PisemnostDPHKH1VetaA4();
				ret[i].Generate(ctx, inNotAnnonTransList[i], i + 1);
			}
			return ret;
		}

		public void Generate(GeneratorContext ctx, Transaction t, int row1)
		{
			c_radku = row1;
			c_radkuSpecified = true;
			dic_odb = t.VATId;
			c_evid_dd = t.Id;
			dppd = t.Date.ToString("d.M.yyyy");
			zakl_dane1 = Math.Round(t.Value);
			zakl_dane1Specified = true;
			dan1 = Math.Round(t.VAT);
			dan1Specified = true;
			kod_rezim_pl = "0";
			zdph_44 = "N";
		}
	}

	public partial class PisemnostDPHKH1VetaA5
	{
		public static PisemnostDPHKH1VetaA5 CreateIfNeeded(GeneratorContext ctx)
		{
			var inAnnonTrans =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Incoming && t.IsAnnonymousInKH
				select t;

			var ret = new PisemnostDPHKH1VetaA5();
			ret.zakl_dane1 = Math.Round((from t in inAnnonTrans select t.Value).Sum());
			ret.zakl_dane1Specified = true;

			if (ret.zakl_dane1 == 0)
			{
				return null;
			}

			ret.dan1 = Math.Round((from t in inAnnonTrans select t.VAT).Sum());
			ret.dan1Specified = true;

			return ret;
		}
	}

	public partial class PisemnostDPHKH1VetaB2
	{
		public static PisemnostDPHKH1VetaB2[] CreateVetaB2Array(GeneratorContext ctx)
		{
			var outNotAnnonTrans =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Outgoing && t.IsAnnonymousInKH == false
				select t;

			var outNotAnnonTransList = outNotAnnonTrans.ToList();

			var ret = new PisemnostDPHKH1VetaB2[outNotAnnonTransList.Count];
			for (var i = 0; i < outNotAnnonTransList.Count; ++i)
			{
				ret[i] = new PisemnostDPHKH1VetaB2();
				ret[i].Generate(ctx, outNotAnnonTransList[i], i + 1);
			}
			return ret;
		}

		public void Generate(GeneratorContext ctx, Transaction t, int row1)
		{
			c_radku = row1;
			c_radkuSpecified = true;
			dic_dod = t.VATId;
			c_evid_dd = t.Id;
			dppd = t.Date.ToString("d.M.yyyy");
			zakl_dane1 = Math.Round(t.Value);
			zakl_dane1Specified = true;
			dan1 = Math.Round(t.VAT);
			dan1Specified = true;
			pomer = "N";
			zdph_44 = "N";
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

	public partial class PisemnostDPHKH1VetaB3
	{
		public static PisemnostDPHKH1VetaB3 CreateIfNeeded(GeneratorContext ctx)
		{
			var outAnnonTrans =
				from t in ctx.Transactions
				where t.Direction == Transaction.EDirection.Outgoing && t.IsAnnonymousInKH
				select t;

			var ret = new PisemnostDPHKH1VetaB3();
			ret.zakl_dane1 = Math.Round((from t in outAnnonTrans select t.Value).Sum());
			ret.zakl_dane1Specified = true;

			if (ret.zakl_dane1 == 0)
			{
				return null;
			}

			ret.dan1 = Math.Round((from t in outAnnonTrans select t.VAT).Sum());
			ret.dan1Specified = true;

			return ret;
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
