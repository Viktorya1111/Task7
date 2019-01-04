using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data
{
	public class JSONReader : IData
	{
		private string _documentPath;

		public JSONReader(string path)
		{
			_documentPath = path;
		}

		public Stock Report()
		{
			Stock report = new Stock();

			dynamic jsonobj = JsonConvert.DeserializeObject(File.ReadAllText(_documentPath));
			
			var high = ((JArray)jsonobj.h);
			var low = ((JArray)jsonobj.l);

			var open = ((JArray)jsonobj.o);
			var close = ((JArray)jsonobj.c);
			var timestamps = ((JArray)jsonobj.t);
			
			for (int i = 0; i < timestamps.Count; i++)
			{
				Candle candle = new Candle()
				{
					High = (decimal)high[i],
					Low = (decimal)low[i],
					Open = (decimal)open[i],
					Close = (decimal)close[i],
					Time = DateTimeOffset.FromUnixTimeSeconds((long)timestamps[i]).UtcDateTime,
					TStamp = (long)timestamps[i],
				};

				report.PushCandle(candle);

			}

			return report;
		}
	}
}
