using AppControleFinanceiro.Views;

namespace AppControleFinanceiro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var listPage = this.Handler.MauiContext.Services.GetService<TransactionList>();
            return new Window(new NavigationPage(listPage));
        }
    }
}