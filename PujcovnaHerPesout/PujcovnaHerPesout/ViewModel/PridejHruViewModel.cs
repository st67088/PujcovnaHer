using LiteDB;
using PujcovnaHerPesout.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PujcovnaHerPesout.ViewModel
{

    public class PridejHruViewModel : ViewModelBase
    {
        private LiteDatabase db;
        private Records<Hra> poleHry;

        private LiteCollection<Hra> hraRecords;
        private LiteCollection<Pobocka> pobockaRecords;

        public ICommand PridejCommand { get; private set; }
        public ICommand ZrusitCommand { get; private set; }

        private ObservableCollection<Zanr> zanry;
        public ObservableCollection<Zanr> Zanry
        {
            get { return zanry; }
            set { SetProperty(ref zanry, value); }
        }

        private string jmenoHry;
        public string JmenoHry
        {
            get { return jmenoHry; }
            set { SetProperty(ref jmenoHry, value); }
        }

        private int pocetHer;
        public int PocetHer
        {
            get { return pocetHer; }
            set { SetProperty(ref pocetHer, value); }
        }

        private Pobocka vybranaPobocka;
        public Pobocka VybranaPobocka
        {
            get { return vybranaPobocka; }
            set { SetProperty(ref vybranaPobocka, value); }
        }

        private Zanr vybranyZanr;
        public Zanr VybranyZanr
        {
            get { return vybranyZanr; }
            set { SetProperty(ref vybranyZanr, value); }
        }

        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }
        private bool pridejHruUspesne;
        public bool PridejHruUspesne
        {
            get { return pridejHruUspesne; }
            set { SetProperty(ref pridejHruUspesne, value); }
        }
        private Hra hra;
        public Hra Hra
        {
            get { return hra; }
            set { SetProperty(ref hra, value); }
        }
        private Records<Pobocka> polePobocky;
        public Records<Pobocka> PolePobocky
        {
            get { return polePobocky; }
            set
            {
                SetProperty(ref polePobocky, value);
                if (polePobocky != null && polePobocky.RecordType == typeof(Pobocka))
                {
                    OnPropertyChanged(nameof(PolePobocky));
                }
            }
        }
        private ObservableCollection<Pobocka> nazvyPobocek;
        public ObservableCollection<Pobocka> NazvyPobocek
        {
            get { return nazvyPobocek; }
            set { SetProperty(ref nazvyPobocek, value); }
        }

        public PridejHruViewModel()
        {
            db = DatabaseManager.GetDatabaseInstance();
            if (db == null) { return; }
            InitializeCommands();
            InitializeDatabase();
            InitializeComboBoxItems();
        }
        public PridejHruViewModel(Hra hraToUpdate, Records<Pobocka> polePobocky)
        {
            db = DatabaseManager.GetDatabaseInstance();
            if (db == null) { return; }

            InitializeCommands();
            InitializeDatabase();

            InitializeComboBoxItems();

            JmenoHry = hraToUpdate.Jmeno;
            PocetHer = hraToUpdate.Pocet;
            VybranaPobocka = hraToUpdate.Pobocka;
            VybranyZanr = hraToUpdate.Zanr;
            this.hra = hraToUpdate;
        }

        private void InitializeComboBoxItems()
        {
            NazvyPobocek = new ObservableCollection<Pobocka>(polePobocky.GetAll());
            Zanry = new ObservableCollection<Zanr>(Enum.GetValues(typeof(Zanr)).Cast<Zanr>());
        }
        private void InitializeCommands()
        {
            PridejCommand = new RelayCommand(_ => PridejHru());
            ZrusitCommand = new RelayCommand(_ => Zrusit());
        }
        private void InitializeDatabase()
        {
            try
            {
                hraRecords = (LiteCollection<Hra>)db.GetCollection<Hra>("HraRecords");
                poleHry = new Records<Hra>(hraRecords);

                pobockaRecords = (LiteCollection<Pobocka>)db.GetCollection<Pobocka>("PobockaRecords");
                polePobocky = new Records<Pobocka>(pobockaRecords);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při inicializaci databáze: " + ex.Message);
            }
        }

        private void PridejHru()
        {
            try
            {
                if (string.IsNullOrEmpty(JmenoHry) || PocetHer <= 0 || VybranaPobocka == null || VybranyZanr == null)
                {
                    MessageBox.Show("Prosím, vyplňte všechny údaje správně.");
                }
                else
                {
                    hra = new Hra
                    {
                        Jmeno = JmenoHry,
                        Pocet = PocetHer,
                        Pobocka = VybranaPobocka,
                        Zanr = VybranyZanr
                    };

                    DialogResult = true;
                    PridejHruUspesne = true;
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                    window?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při přidávání hry: " + ex.Message);
            }
        }

        private void Zrusit()
        {
            DialogResult = false;
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window?.Close();
        }
    }
}
