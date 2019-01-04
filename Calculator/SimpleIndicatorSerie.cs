using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	public class SimpleIndicatorSerie : IndicatorSerie
	{
		public Queue<double?> SerieData { get; set; } = new Queue<double?>();

		public void PushValue(double? value)
		{
			SerieData.Enqueue(value);
		}

		public IEnumerator<double?> GetEnumerator()
		{
			return SerieData.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return SerieData.GetEnumerator();
		}

        public double? PopValue()
        {
            return SerieData.Dequeue();
        }

    }
}
