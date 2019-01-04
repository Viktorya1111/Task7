using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Calculator
{
    public interface IndicatorCalculator
    {
		IndicatorSerie Calculate(Stock report);
    }
}
