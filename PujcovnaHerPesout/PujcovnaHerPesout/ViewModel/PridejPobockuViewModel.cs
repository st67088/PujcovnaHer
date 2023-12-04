using System.Windows.Input;
using PujcovnaHerPesout.Model;
using System.Windows;
using System.Linq;

namespace PujcovnaHerPesout.ViewModel
{
    public class PridejPobockuViewModel : ViewModelBase
    {
        private Pobocka pobocka;
        private string nazevPobocky;
        private string adresaPobocky;
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public PridejPobockuViewModel()
        {          
            InitializeCommands();
        }
        public PridejPobockuViewModel(Pobocka existujiciPobocka)
        {
            Pobocka = existujiciPobocka;

            NazevPobocky = existujiciPobocka.Nazev;
            AdresaPobocky = existujiciPobocka.Adresa;
            this.pobocka = existujiciPobocka;
            InitializeCommands();
        }
        public string NazevPobocky
        {
            get { return nazevPobocky; }
            set { SetProperty(ref nazevPobocky, value); }
        }

        public string AdresaPobocky
        {
            get { return adresaPobocky; }
            set { SetProperty(ref adresaPobocky, value); }
        }
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }
        private bool pridejPobockuUspesne;
        public bool PridejPobockuUspesne
        {
            get { return pridejPobockuUspesne; }
            set { SetProperty(ref pridejPobockuUspesne, value); }
        }
        public Pobocka Pobocka
        {
            get { return pobocka; }
            set { SetProperty(ref pobocka, value); }
        }
        private void InitializeCommands()
        {
            OkCommand = new RelayCommand(_ => Ok());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void Ok()
        {
            try
            {
                if (string.IsNullOrEmpty(NazevPobocky) || string.IsNullOrEmpty(AdresaPobocky))
                {
                    MessageBox.Show("Prosím, vyplňte všechny údaje správně.");
                }
                else
                {
                    pobocka = new Pobocka { Nazev = NazevPobocky, Adresa = AdresaPobocky };
                    DialogResult = true;
                    PridejPobockuUspesne = true;
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                    window?.Close();
                }
            }
            catch
            {
                MessageBox.Show("Špatně zadané hodnoty!");
            }
        }

        private void Cancel()
        {
            DialogResult = false;
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window?.Close();
        }
    }
}
