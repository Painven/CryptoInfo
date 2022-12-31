using CryptoInfo.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CryptoInfo.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    int takePagesCount = 10;
    public int TakePagesCount
    {
        get => takePagesCount;
        set => Set(ref takePagesCount, value);
    }

    bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set => Set(ref inProgress, value);
    }

    public ConditionalFilter ConditionalFilter { get; set; } = new();

    public ICommand DownloadCurrenciesCommand { get; }

    public ObservableCollection<CryptoCurrency> Currencies { get; set; } = new();
    private readonly InvestingComParser parser = new InvestingComParser();

    public MainWindowViewModel()
    {
        DownloadCurrenciesCommand = new LambdaCommand(async e => await DownloadCurrencies(), e => !InProgress);
        ConditionalFilter.PropertyChanged += (o, e) => RefreshFilter();

    }

    private async Task DownloadCurrencies()
    {
        try
        {
            InProgress = true;

            Stopwatch sw = Stopwatch.StartNew();
            var data = await parser.Parse(TakePagesCount);
            sw.Stop();

            Currencies.Clear();

            foreach (var currency in data)
            {
                Currencies.Add(currency);
            }
            MessageBox.Show($"Переобход выполнен за {(int)sw.Elapsed.TotalSeconds} сек.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка:\r\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            InProgress = false;
        }
    }

    private void RefreshFilter()
    {
        throw new NotImplementedException();
    }
}

public class ConditionalFilter : ViewModelBase
{
    decimal minPrice;
    public decimal MinPrice
    {
        get => minPrice;
        set => Set(ref minPrice, value);
    }

    decimal maxPrice;
    public decimal MaxPrice
    {
        get => maxPrice;
        set => Set(ref maxPrice, value);
    }

    double oneDayMinChangePercent;
    public double OneDayMinChangePercent
    {
        get => oneDayMinChangePercent;
        set => Set(ref oneDayMinChangePercent, value);
    }
    double oneDayMaxChangePercent;
    public double OneDayMaxChangePercent
    {
        get => oneDayMaxChangePercent;
        set => Set(ref oneDayMaxChangePercent, value);
    }

    double oneWeekMinChangePercent;
    public double OneWeekMinChangePercent
    {
        get => oneWeekMinChangePercent;
        set => Set(ref oneWeekMinChangePercent, value);
    }
    double oneWeekMaxChangePercent;
    public double OneWeekMaxChangePercent
    {
        get => oneWeekMaxChangePercent;
        set => Set(ref oneWeekMaxChangePercent, value);
    }

    public ConditionalFilter()
    {

    }
}
