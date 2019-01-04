using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class Stock
	{
		public Queue<Candle> Candles { get; private set; } = new Queue<Candle>();

		public Stock()
		{

		}

		public void PushCandle(Candle candle)
		{
			Candles.Enqueue(candle);
		}

		public Candle PeekCandle()
		{
			return Candles.Peek();
		}

        public Candle PopCandle()
        {
            return Candles.Dequeue();
        }
    }
}
