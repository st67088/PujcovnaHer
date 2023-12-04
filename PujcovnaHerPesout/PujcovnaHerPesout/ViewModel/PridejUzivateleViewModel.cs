using System.Windows.Input;
using PujcovnaHerPesout.Model;
using System.Windows;
using System.Linq;

namespace PujcovnaHerPesout.ViewModel
{
    public class PridejUzivateleViewModel : ViewModelBase
    {
        private Uzivatel uzivatel;
        private string jmenoUzivatele;
        private string adresaUzivatele;
        private int telefonUzivatele;

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public PridejUzivateleViewModel()
        {
            InitializeCommands();
        }
        public PridejUzivateleViewModel(Uzivatel uzivatelToUpdate)
        {
            InitializeCommands();

            JmenoUzivatele = uzivatelToUpdate.Jmeno;
            AdresaUzivatele = uzivatelToUpdate.Adresa;
            TelefonUzivatele = uzivatelToUpdate.Telefon;
            this.uzivatel = uzivatelToUpdate;
        }
        public string JmenoUzivatele
        {
            get { return jmenoUzivatele; }
            set { SetProperty(ref jmenoUzivatele, value); }
        }

        public string AdresaUzivatele
        {
            get { return adresaUzivatele; }
            set { SetProperty(ref adresaUzivatele, value); }
        }
        public int TelefonUzivatele
        {
            get { return telefonUzivatele; }
            set { SetProperty(ref telefonUzivatele, value); }
        }
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }
        private bool pridejUzivateleUspesne;
        public bool PridejUzivateleUspesne
        {
            get { return pridejUzivateleUspesne; }
            set { SetProperty(ref pridejUzivateleUspesne, value); }
        }
        public Uzivatel Uzivatel
        {
            get { return uzivatel; }
            set { SetProperty(ref uzivatel, value); }
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
                if (string.IsNullOrEmpty(JmenoUzivatele) || TelefonUzivatele <= 0 || string.IsNullOrEmpty(AdresaUzivatele))
                {
                    MessageBox.Show("Prosím, vyplňte všechny údaje správně.");
                }
                else
                {
                    uzivatel = new Uzivatel { Jmeno = JmenoUzivatele, Adresa = AdresaUzivatele, Telefon = TelefonUzivatele };
                    DialogResult = true;
                    PridejUzivateleUspesne = true;
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
