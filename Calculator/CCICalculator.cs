using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Calculator
{
    public class CCICalculator : IndicatorCalculator
    {
        public int periods = 6;
        public IndicatorSerie Calculate(Stock report)
        {
            IndicatorSerie sarSerie = new SimpleIndicatorSerie();
            var candlesSerie = report.Candles.ToList();
            List<double> typicalPrices = new List<double>();
            foreach(var c in candlesSerie)
            {
                var price = (c.High + c.Low + c.Close) / 3;
                typicalPrices.Add((double)price);
            }

            List<double> ccis = new List<double>();
            for (int i = 0; i < typicalPrices.Count; i++)
            {
                var price = typicalPrices[i];
                var cci = ((price - SMACalc(typicalPrices.GetRange(0, i), periods))/ MADCalc(typicalPrices.GetRange(0, i), periods)) / 0.015 ;
                sarSerie.PushValue(cci * 100);
            }
            return sarSerie;
        }

        private double SMACalc(List<double> prices, int period)
        {
            double value = 0;
            for (int i = 0; i < period; i++)
            {
                if (!(i > prices.Count - 1))
                    value += prices[prices.Count - 1 - i];
                else break;
            }
            return value / period;
        }

        private double MADCalc(List<double> prices, int period)
        {
            double value = 0;
            for (int i = 0; i < period; i++)
            {
                if (!(i > prices.Count - 1))
                    value += Math.Abs(prices[prices.Count - 1 - i] - SMACalc(prices.GetRange(prices.Count - 1 - i, i), period));
                else break;
            }
            return value / period;
        }
    }
}
