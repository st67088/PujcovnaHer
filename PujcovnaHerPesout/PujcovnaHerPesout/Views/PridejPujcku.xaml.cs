using PujcovnaHerPesout.ViewModel;
using System.Windows;
namespace PujcovnaHerPesout.Views
{
    public partial class PridejPujcku : Window
    {
        private PridejPujckuViewModel viewModel;

        public PridejPujcku()
        {
            InitializeComponent();
            viewModel = new PridejPujckuViewModel();
            DataContext = viewModel;
        }
    }
}