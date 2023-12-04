using PujcovnaHerPesout.ViewModel;
using System.Windows;

namespace PujcovnaHerPesout.Views
{
    public partial class PridejHru : Window
    {
        private PridejHruViewModel viewModel;

        public PridejHru()
        {
            InitializeComponent();
            viewModel = new PridejHruViewModel();
            DataContext = viewModel;
        }
    }
}
