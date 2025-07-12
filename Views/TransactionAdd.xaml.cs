namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
	public TransactionAdd()
	{
		InitializeComponent();
	}

    private void OnClosedTransactionAdd(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }
}