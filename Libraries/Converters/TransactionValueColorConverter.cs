using AppControleFinanceiro.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Libraries.Converters
{
    public class TransactionValueColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Transaction transaction = (Transaction)value;

            if (transaction == null)
                return string.Empty;

            if (transaction.Type == TransactionType.Income)
                return Color.FromRgb(147,158,90);
            else
                return Colors.Red;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
