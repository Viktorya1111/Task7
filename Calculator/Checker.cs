using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Data;

namespace Calculator
{
	public class Checker : IDisposable
	{
		public delegate void ReceiveCandleEvent(Candle candle, double? indicatorValue);
		public event ReceiveCandleEvent OnReceiveCandle;

		private IData dataSource;
		private Thread checker;
		private IndicatorCalculator indicatorCalculator;

		private Stock stock;
		private IndicatorSerie indicatorSerie;

		public Checker(IData source, IndicatorCalculator calculator)
		{
			dataSource = source;
			indicatorCalculator = calculator;
		}

		public void Run()
		{
			stock = dataSource.Report();
			indicatorSerie = indicatorCalculator.Calculate(stock);

			checker = new Thread(new ThreadStart(Update));
			checker.Start();
		}

		protected void Update()
		{
			while (stock.Candles.Count != 0)
			{
				OnReceiveCandle?.Invoke(stock.PopCandle(), indicatorSerie.PopValue());

				Thread.Sleep(1000);
			}
		}

        public void Dispose()
        {
            checker.Abort();
        }
    }
}
