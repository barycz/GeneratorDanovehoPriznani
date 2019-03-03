// Author: Tomas Barak

using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Linq;

namespace GeneratorDanovehoPriznani
{
	public class Book
	{
		public List<Transaction> Transactions { get; private set; }

		public Book()
		{
			Transactions = new List<Transaction>();
		}

		public Book GetBookForMonth(uint monthCode)
		{
			var ret = new Book();
			ret.Transactions = Transactions.Where(t => t.MonthCode == monthCode).ToList();
			return ret;
		}

		public void LoadFromSheets()
		{
			UserCredential credential;
			using (var stream =
				new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				var credPath = "token.json";
				string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
			}

			var service = new SheetsService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
			});

			var spreadsheetId = "";
			var range = "Transactions!A2:F";

			var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
			request.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
			var response = request.Execute();

			foreach (var row in response.Values)
			{
				if(string.IsNullOrEmpty((string)row[2]))
				{
					break;
				}

				var trans = new Transaction(row);
				Transactions.Add(trans);
			}
		}
	}
}
