// Author: Tomas Barak

using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Linq;
using System;

namespace GeneratorDanovehoPriznani
{
	public class Book
	{
		public IEnumerable<Transaction> Transactions => AllTransactions.Where(PeriodFilter);

		private List<Transaction> AllTransactions { get; set; }

		private Func<Transaction, bool> PeriodFilter { get; }

		public Book(Func<Transaction, bool> periodFilter)
		{
			AllTransactions = new List<Transaction>();
			PeriodFilter = periodFilter;
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
				AllTransactions.Add(trans);
			}
		}
	}
}
