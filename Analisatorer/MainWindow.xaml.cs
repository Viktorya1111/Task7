using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart;
using SciChart.Charting.Model.DataSeries;
using Data;
using Calculator;
using SciChart.Data.Model;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Analisator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OhlcDataSeries<DateTime, double> cand;
        private XyDataSeries<DateTime, double> sar;

        private Checker tradingMonitor;

        private int candlesCount = 0;
        public MainWindow()
        {
            InitializeComponent();

            ReloadChartSeries();
            StockChart.XAxis.AutoRange = SciChart.Charting.Visuals.Axes.AutoRange.Once;
            StockChart2.XAxis.AutoRange = SciChart.Charting.Visuals.Axes.AutoRange.Once;
            var filepath = System.IO.Directory.GetCurrentDirectory() + "\\" + "prices.xlsx";
            LoadDataSource(new XLSXReader(filepath));
            this.Loaded += OnLoaded;
        }

        private void ReloadChartSeries()
        {
            cand = new OhlcDataSeries<DateTime, double>() { SeriesName = "Candles", FifoCapacity = 10000 };
            sar = new XyDataSeries<DateTime, double>() { SeriesName = "Sar", FifoCapacity = 10000 };

            CandleChart.DataSeries = cand;
            SarChart.DataSeries = sar;
        }

        private void AddNewCandle(Candle candle, double? indicatorValue)
        {
            using (cand.SuspendUpdates())
            using (sar.SuspendUpdates())
            {
                cand.Append(candle.Time, (double)candle.Open, (double)candle.High, (double)candle.Low, (double)candle.Close);

                if (indicatorValue != null)
                    sar.Append(candle.Time, indicatorValue.Value);


                candlesCount++;

                StockChart.XAxis.VisibleRange = new IndexRange(candlesCount - 50, candlesCount);
                StockChart2.XAxis.VisibleRange = new IndexRange(candlesCount - 50, candlesCount);
            }
        }

        private void OnCandleReceived(Candle candle, double? indicatorValue)
        {
            StockChart.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { AddNewCandle(candle, indicatorValue); }));
        }

        private void MainMenu_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx|JSON files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string extension = System.IO.Path.GetExtension(openFileDialog.FileName);

                    if (extension == ".xlsx")
                        LoadDataSource(new XLSXReader(openFileDialog.FileName));
                    else if (extension == ".json")
                        LoadDataSource(new JSONReader(openFileDialog.FileName));
                    else
                        throw new Exception();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Произошла ошибка загрузки данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadDataSource(IData source)
        {
            if (this.tradingMonitor != null)
            {
                tradingMonitor.Dispose();
                tradingMonitor = null;
            }

            tradingMonitor = new Checker(source, new CCICalculator());
            tradingMonitor.OnReceiveCandle += OnCandleReceived;

            ReloadChartSeries();

            tradingMonitor.Run();
        }

        private void MainMenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
        }

    }
}