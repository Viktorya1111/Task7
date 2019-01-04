using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Core;

namespace Calculator
{
	public interface IndicatorSerie : IEnumerable<double?>
	{
		Queue<double?> SerieData { get; }

		void PushValue(double? value);
		double? PopValue();
	}
}
