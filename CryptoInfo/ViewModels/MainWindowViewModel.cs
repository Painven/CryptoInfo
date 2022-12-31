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

    public ConditionalFilter ConditionalFilter { get; set; } = new();

    public ICommand CustomCommand { get; }

    public MainWindowViewModel()
    {
        CustomCommand = new LambdaCommand(e => MessageBox.Show("EXECUTED"), e => true);
        ConditionalFilter.PropertyChanged += ConditionalFilter_PropertyChanged;
    }

    private void ConditionalFilter_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        throw new System.NotImplementedException();
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

    public ConditionalFilter()
    {

    }
}
