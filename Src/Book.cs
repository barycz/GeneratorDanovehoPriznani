// Author: Tomas Barak

using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace GeneratorDanovehoPriznani
{
	public class Book
	{
		public List<Transaction> Transactions { get; private set; }

		public Book()
		{
			Transactions = new List<Transaction>();
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
			var range = "Transactions!A2:D";

			var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
			var response = request.Execute();

			foreach (var row in response.Values)
			{
				if(string.IsNullOrEmpty((string)row[0]))
				{
					break;
				}

				var trans = new Transaction();
				trans.Value = decimal.Parse((string)row[2]);
				trans.Direction =
					(string)row[1] == "in" ? Transaction.EDirection.Incoming : Transaction.EDirection.Outgoing;
				trans.VATRate = VATRate.Standard;

				Transactions.Add(trans);
			}
		}
	}
}
