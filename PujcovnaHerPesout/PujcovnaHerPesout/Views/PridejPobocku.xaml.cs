using PujcovnaHerPesout.ViewModel;
using System.Windows;
namespace PujcovnaHerPesout.Views
{
    public partial class PridejPobocku : Window
    {
        private PridejPobockuViewModel viewModel;

        public PridejPobocku()
        {
            InitializeComponent();
            viewModel = new PridejPobockuViewModel();
            DataContext = viewModel;
        }
    }
}