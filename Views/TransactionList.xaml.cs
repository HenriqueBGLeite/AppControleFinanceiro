using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{
    private ITransactionRepository _repository;

    public TransactionList(ITransactionRepository repository)
    {
        InitializeComponent();

        _repository = repository;

        Reload();

        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });
    }

    private void Reload()
    {
        var items = _repository.GetAll();
        CollectionViewTransactions.ItemsSource = items;

        double income = items.Where(a => a.Type == TransactionType.Income).Sum(a => a.Value);
        double expense = items.Where(a => a.Type == TransactionType.Expenses).Sum(a => a.Value);
        double balance = income - expense;

        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");
    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs e)
    {

        var transactionAddPage = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAddPage);
    }

    private void TapGestureRecognizerTapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];

        Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEditPage = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEditPage.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEditPage);
    }

    private async void TapGestureRecognizerTapped_ToDelete(object sender, TappedEventArgs e)
    {
        await AnimationBorder((Border)sender, true);

        bool result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem certeza que deseja excluir?", "Sim", "Não");

        if (!result)
        {
            await AnimationBorder((Border)sender, false);
            return;
        }

        Transaction transaction = (Transaction)e.Parameter;

        _repository.Delete(transaction);

        Reload();
    }

    private Color _borderOriginalBackgroundColor;
    private string _labelOriginalText;
    private async Task AnimationBorder(Border border, bool isDeleteAnimation)
    {
        var label = (Label)border.Content;

        if (isDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalText = label.Text;

            await border.RotateYTo(90, 500);

            border.BackgroundColor = Colors.Red;
            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 500);
        }
        else
        {
            await border.RotateYTo(90, 500);

            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;
            border.BackgroundColor = _borderOriginalBackgroundColor;

            await border.RotateYTo(0, 500);
        }
    }
}