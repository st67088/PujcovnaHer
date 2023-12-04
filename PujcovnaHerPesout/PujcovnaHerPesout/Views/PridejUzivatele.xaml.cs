using PujcovnaHerPesout.ViewModel;
using System.Windows;
namespace PujcovnaHerPesout.Views
{
    public partial class PridejUzivatele : Window
    {
        private PridejUzivateleViewModel viewModel;

        public PridejUzivatele()
        {
            InitializeComponent();
            viewModel = new PridejUzivateleViewModel();
            DataContext = viewModel;
        }
    }
}