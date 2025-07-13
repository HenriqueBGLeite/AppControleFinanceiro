using AppControleFinanceiro.Libraries.Utils.FixBugs;
using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Platform;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _repository;
    public TransactionAdd(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    private void OnClosedTransactionAdd(object sender, TappedEventArgs e)
    {
        KeyboardFixBugs.HideKeyboard();
        Navigation.PopModalAsync();
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;

        SaveTransactionInDatabase();

        KeyboardFixBugs.HideKeyboard();

        Navigation.PopModalAsync();

        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Name = EntryName.Text,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Date = DatePickerDate.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text)),
        };

        _repository.Add(transaction);
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