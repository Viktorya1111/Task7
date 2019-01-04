using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace Data
{
	public class XLSXReader : IData
	{
		private string DocumentPath;

		public XLSXReader(string path)
		{
			DocumentPath = path;
		}

		public Stock Report()
		{
			Stock report = new Stock();

			using (ExcelPackage Package = new ExcelPackage(new FileInfo(DocumentPath)))
			{
				var Worksheet = Package.Workbook.Worksheets.First();
				var Rows = Worksheet.Dimension.End.Row;
				var Columns = Worksheet.Dimension.End.Column;

				for (int rowNum = 2; rowNum <= Rows; rowNum++) 
				{
					var row = Worksheet.Cells[rowNum, 1, rowNum, Columns]
						.Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();

					string DateTime = row[2] + " " + row[3];

                    DateTime dateTime = System.DateTime.ParseExact(DateTime, "yyyyMMdd HHmmss", null);

					Candle candle = new Candle() {
						High = int.Parse(row[5]),
						Low = int.Parse(row[6]),
						Open = int.Parse(row[4]),
						Close = int.Parse(row[7]),
						Time = dateTime,
						TStamp = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds(),
					};

					report.PushCandle(candle);
				}
			}

			return report;
		}
	}
}
