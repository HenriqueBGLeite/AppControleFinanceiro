using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;
    private ITransactionRepository _repository;

    public TransactionEdit(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (_transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else 
            RadioExpense.IsChecked = true;

        EntryName.Text = _transaction.Name;
        DatePickerDate.Date = _transaction.Date.Date;
        EntryValue.Text = _transaction.Value.ToString();
    }

    private void OnClosedTransactionEdit(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void OnButtonClicked_Edit(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;

        EditTransactionInDatabase();

        Navigation.PopModalAsync();

        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void EditTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Id = _transaction.Id,
            Name = EntryName.Text,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Date = DatePickerDate.Date,
            Value = double.Parse(EntryValue.Text),
        };

        _repository.Update(transaction);
    }

    private bool IsValidData()
    {
        bool valid = true;
        LabelError.IsVisible = false;
        StringBuilder sb = new StringBuilder();

        var entryName = EntryName.Text;
        var entryValue = EntryValue.Text;


        if (string.IsNullOrEmpty(entryName) || string.IsNullOrWhiteSpace(entryName))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            valid = false;
        }

        if (string.IsNullOrEmpty(entryValue) || string.IsNullOrWhiteSpace(entryValue))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            valid = false;
        }

        if (!string.IsNullOrEmpty(entryValue) && !double.TryParse(entryValue, out double result))
        {
            sb.AppendLine("O campo 'Valor' é inválido!");
            valid = false;
        }

        if (!valid)
        {
            LabelError.Text = sb.ToString();
            LabelError.IsVisible = true;
        }

        return valid;
    }
}