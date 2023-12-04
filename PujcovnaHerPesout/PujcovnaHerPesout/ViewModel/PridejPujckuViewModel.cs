using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LiteDB;
using PujcovnaHerPesout.Model;

namespace PujcovnaHerPesout.ViewModel
{
    public class PridejPujckuViewModel : ViewModelBase
    {
        private ObservableCollection<string> uzivateleItems;
        private ObservableCollection<string> pobockyItems;
        private ObservableCollection<string> hryItems;
        private string selectedUzivatel;
        private string selectedPobocka;
        private string selectedHra;
        private LiteCollection<Hra> hraRecords;
        private LiteCollection<Pobocka> pobockaRecords;
        private Records<Hra> poleHry;
        private LiteDatabase db;
        private Records<Pobocka> polePobocky;
        private Records<Uzivatel> poleUzivatele;
        private LiteCollection<Uzivatel> uzivatelRecords;

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ObservableCollection<string> UzivateleItems
        {
            get { return uzivateleItems; }
            set { SetProperty(ref uzivateleItems, value); }
        }

        public ObservableCollection<string> PobockyItems
        {
            get { return pobockyItems; }
            set { SetProperty(ref pobockyItems, value); }
        }

        public ObservableCollection<string> HryItems
        {
            get { return hryItems; }
            set { SetProperty(ref hryItems, value); }
        }

        public string SelectedUzivatel
        {
            get { return selectedUzivatel; }
            set
            {
                SetProperty(ref selectedUzivatel, value);
                UpdateHryItems();
            }
        }

        public string SelectedPobocka
        {
            get { return selectedPobocka; }
            set
            {
                SetProperty(ref selectedPobocka, value);
                UpdateHryItems();
            }
        }

        public string SelectedHra
        {
            get { return selectedHra; }
            set { SetProperty(ref selectedHra, value); }
        }
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }
        private bool pridejPujckuUspesne;
        public bool PridejPujckuUspesne
        {
            get { return pridejPujckuUspesne; }
            set { SetProperty(ref pridejPujckuUspesne, value); }
        }
        private PujcenaHra pujcenaHra;
        public PujcenaHra PujcenaHra
        {
            get { return pujcenaHra; }
            set { SetProperty(ref pujcenaHra, value); }
        }
        public PridejPujckuViewModel()
        {
            db = DatabaseManager.GetDatabaseInstance();
            if (db == null) { return; }
            InitializeCommands();
            InitializeDatabase();
            InitializeData();         
        }
        private void InitializeData() { 
         UzivateleItems = new ObservableCollection<string>(poleUzivatele.GetAll().Select(u => u.Jmeno));
         PobockyItems = new ObservableCollection<string>(polePobocky.GetAll().Select(p => p.Nazev));
    
        }

        private void InitializeDatabase()
        {
            try
            {
                hraRecords = (LiteCollection<Hra>)db.GetCollection<Hra>("HraRecords");
                poleHry = new Records<Hra>(hraRecords);

                pobockaRecords = (LiteCollection<Pobocka>)db.GetCollection<Pobocka>("PobockaRecords");
                polePobocky = new Records<Pobocka>(pobockaRecords);

                uzivatelRecords = (LiteCollection<Uzivatel>)db.GetCollection<Uzivatel>("UzivatelRecords");
                poleUzivatele = new Records<Uzivatel>(uzivatelRecords);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při inicializaci databáze: " + ex.Message);
            }
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
                Uzivatel selectedUzivatelObj = poleUzivatele.GetAll().FirstOrDefault(u => u.Jmeno == SelectedUzivatel);
                Hra selectedHraObj = poleHry.GetAll().FirstOrDefault(h => h.ToString() == SelectedHra);

                if (SelectedUzivatel != null && SelectedPobocka != null && SelectedHra != null)
                {
                    selectedHraObj.Pocet--;
                    poleHry.Update(selectedHraObj);
                    PujcenaHra novaPujcka = new PujcenaHra(selectedHraObj, selectedUzivatelObj);
                    PujcenaHra = novaPujcka;
                    DialogResult = true;
                    PridejPujckuUspesne = true;
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                    window?.Close();
                }
                else
                {
                    MessageBox.Show("Špatně zadané hodnoty!");
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

        private void UpdateHryItems()
        {
            HryItems = new ObservableCollection<string>(
                poleHry.GetAll()
                    .Where(h => h.Pobocka.Nazev == SelectedPobocka && h.Pocet > 0)
                    .Select(h => h.ToString())
            );
        }
    }
}
