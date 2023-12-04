using LiteDB;
using PujcovnaHerPesout.Model;
using PujcovnaHerPesout.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PujcovnaHerPesout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            viewModel.ItemFoundEvent += ViewModel_ItemFoundEvent;
        }

        private void ViewModel_ItemFoundEvent(object sender, int index)
        {
            listBox.SelectedIndex = index;
        }
    }
}

