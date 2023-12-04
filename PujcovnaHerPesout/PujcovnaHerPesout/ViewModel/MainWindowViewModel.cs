using LiteDB;
using PujcovnaHerPesout.Model;
using PujcovnaHerPesout.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PujcovnaHerPesout.ViewModel
{

    public class MainWindowViewModel : ViewModelBase
    {
        private LiteDatabase db;
        private Records<Hra> poleHry;
        private Records<Pobocka> polePobocky;
        private Records<Uzivatel> poleUzivatele;
        private Records<PujcenaHra> polePujcky;

        private LiteCollection<Hra> hraRecords;
        private LiteCollection<Pobocka> pobockaRecords;
        private LiteCollection<Uzivatel> uzivatelRecords;
        private LiteCollection<PujcenaHra> pujckyRecords;

        private Vyber vyber;

        private string labelCoProhlizis;
        public string LabelCoProhlizis
        {
            get { return labelCoProhlizis; }
            set { SetProperty(ref labelCoProhlizis, value); }
        }

        private ObservableCollection<string> filteredListBoxItems;
        public ObservableCollection<string> FilteredListBoxItems
        {
            get { return filteredListBoxItems; }
            set { SetProperty(ref filteredListBoxItems, value); }
        }

        private string textBoxNajdiHruText;
        public string TextBoxNajdiHruText
        {
            get { return textBoxNajdiHruText; }
            set { SetProperty(ref textBoxNajdiHruText, value); }
        }

        private Visibility textBoxNajdiHruVisibility;
        public Visibility TextBoxNajdiHruVisibility
        {
            get { return textBoxNajdiHruVisibility; }
            set { SetProperty(ref textBoxNajdiHruVisibility, value); }
        }

        private Visibility buttonNajdiHruVisibility;
        public Visibility ButtonNajdiHruVisibility
        {
            get { return buttonNajdiHruVisibility; }
            set { SetProperty(ref buttonNajdiHruVisibility, value); }
        }

        private object selectedListBoxItem;
        public object SelectedListBoxItem
        {
            get { return selectedListBoxItem; }
            set { SetProperty(ref selectedListBoxItem, value); }
        }
        public MainWindowViewModel()
        {
            InitializeCommands();
            FilteredListBoxItems = new ObservableCollection<string>();
            db = DatabaseManager.GetDatabaseInstance();
            if (db == null) { return; }
            InitializeDatabase();
            Obnov(Vyber.Pobocky);
        }

        public ICommand ButtonHryCommand { get; private set; }
        public ICommand ButtonPobockyCommand { get; private set; }
        public ICommand ButtonPujceneCommand { get; private set; }
        public ICommand ButtonUzivateleCommand { get; private set; }
        public ICommand ButtonOdeberCommand { get; private set; }
        public ICommand ButtonPridejCommand { get; private set; }
        public ICommand ButtonUpravCommand { get; private set; }
        public ICommand ButtonKonecCommand { get; private set; }
        public ICommand ButtonNajdiHruCommand { get; private set; }

        private void InitializeCommands()
        {
            ButtonHryCommand = new RelayCommand(_ => Obnov(Vyber.Hry));
            ButtonPobockyCommand = new RelayCommand(_ => Obnov(Vyber.Pobocky));
            ButtonPujceneCommand = new RelayCommand(_ => Obnov(Vyber.PujceneHry));
            ButtonUzivateleCommand = new RelayCommand(_ => Obnov(Vyber.Uzivatele));
            ButtonOdeberCommand = new RelayCommand(_ => ButtonOdeberClick(vyber));
            ButtonPridejCommand = new RelayCommand(_ => ButtonPridejClick(vyber));
            ButtonUpravCommand = new RelayCommand(_ => ButtonUpravClick());
            ButtonKonecCommand = new RelayCommand(_ => ButtonKonecClick());
            ButtonNajdiHruCommand = new RelayCommand(_ => buttonNajdiHruClick());
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

                pujckyRecords = (LiteCollection<PujcenaHra>)db.GetCollection<PujcenaHra>("PujckyRecords");
                polePujcky = new Records<PujcenaHra>(pujckyRecords);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při inicializaci databáze: " + ex.Message);
            }

        }
        private void Obnov(Vyber vyber)
        {
            FilteredListBoxItems.Clear();

            switch (vyber)
            {
                case Vyber.Pobocky:
                    LabelCoProhlizis = "Pobočky";
                    this.vyber = Vyber.Pobocky;
                    NajdiNotVisible();
                    foreach (var pobocka in polePobocky.GetAll())
                    {
                        FilteredListBoxItems.Add(pobocka.ToString());
                    }
                    break;
                case Vyber.Hry:
                    LabelCoProhlizis = "Hry";
                    this.vyber = Vyber.Hry;
                    NajdiVisible();
                    foreach (var hra in poleHry.GetAll())
                    {
                        FilteredListBoxItems.Add(hra.ToString());
                    }
                    break;
                case Vyber.Uzivatele:
                    LabelCoProhlizis = "Uživatelé";
                    this.vyber = Vyber.Uzivatele;
                    NajdiNotVisible();
                    foreach (var uzivatel in poleUzivatele.GetAll())
                    {
                        FilteredListBoxItems.Add(uzivatel.ToString());
                    }
                    break;
                case Vyber.PujceneHry:
                    LabelCoProhlizis = "Půjčené hry";
                    this.vyber = Vyber.PujceneHry;
                    NajdiNotVisible();
                    foreach (var pujcka in polePujcky.GetAll())
                    {
                        FilteredListBoxItems.Add(pujcka.ToString());
                    }
                    break;
            }
        }
        private void NajdiVisible()
        {
            TextBoxNajdiHruVisibility = Visibility.Visible;
            ButtonNajdiHruVisibility = Visibility.Visible;
        }
        private void NajdiNotVisible()
        {
            TextBoxNajdiHruVisibility = Visibility.Collapsed;
            ButtonNajdiHruVisibility = Visibility.Collapsed;
        }
        private void ButtonOdeberClick(Vyber vyber)
        {
            if (selectedListBoxItem != null)
            {
                var selectedString = selectedListBoxItem.ToString();
                string[] parts = selectedString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0 && int.TryParse(parts[parts.Length - 1].Trim(), out int selectedId))
                {
                    switch (vyber)
                    {
                        case Vyber.Hry:
                            OdeberHra(selectedId);
                            break;
                        case Vyber.Pobocky:
                            OdeberPobocka(selectedId);
                            break;
                        case Vyber.Uzivatele:
                            OdeberUzivatel(selectedId);
                            break;
                        case Vyber.PujceneHry:
                            OdeberPujcka(selectedId);
                            break;
                    }
                }
                Obnov(vyber);
            }
        }
        private void OdeberHra(int hraId)
        {
            var hraToDelete = poleHry.GetAll().FirstOrDefault(h => h.Id == hraId);

            if (hraToDelete != null)
            {
                if (polePujcky.GetAll().Any(p => p.Hra.Id == hraId))
                {
                    MessageBox.Show("Hra je půjčená!");
                }
                else
                {
                    poleHry.Delete(hraToDelete.Id);
                }
            }
        }
        private void OdeberPobocka(int pobockaId)
        {
            var pobockaToDelete = polePobocky.GetAll().FirstOrDefault(p => p.Id == pobockaId);

            if (pobockaToDelete != null)
            {
                if (poleHry.GetAll().Any((Func<Hra, bool>)(h => h.Pobocka.Id == pobockaId)))
                {
                    MessageBox.Show("Na pobočce jsou stále hry, nelze smazat pobočku.");
                }
                else
                {
                    polePobocky.Delete((int)pobockaToDelete.Id);
                }
            }
        }
        private void OdeberUzivatel(int uzivatelId)
        {
            var uzivatelToDelete = poleUzivatele.GetAll().FirstOrDefault(u => u.Id == uzivatelId);

            if (uzivatelToDelete != null)
            {
                if (polePujcky.GetAll().Any((Func<PujcenaHra, bool>)(p => p.Uzivatel.Id == uzivatelId)))
                {
                    MessageBox.Show("Uživatel má stále půjčky, nelze smazat uživatele.");
                }
                else
                {
                    poleUzivatele.Delete((int)uzivatelToDelete.Id);
                }
            }
        }
        private void OdeberPujcka(int pujckyId)
        {
            var pujckyToDelete = polePujcky.GetAll().FirstOrDefault(p => p.Id == pujckyId);

            if (pujckyToDelete != null)
            {
                var hra = poleHry.GetAll().FirstOrDefault(h => h.Id == pujckyToDelete.Hra.Id);

                if (hra != null)
                {
                    hra.Pocet++;
                    poleHry.Update(hra);
                }

                polePujcky.Delete(pujckyToDelete.Id);
            }
        }
        private void ButtonPridejClick(Vyber vyber)
        {
            switch (vyber)
            {
                case Vyber.Hry:
                    PridejHru();
                    break;
                case Vyber.Pobocky:
                    PridejPobocku();
                    break;
                case Vyber.Uzivatele:
                    PridejUzivatele();
                    break;
                case Vyber.PujceneHry:
                    PridejPujcku();
                    break;
            }
        }
        private void PridejHru()
        {
            PridejHruViewModel dialogHraViewModel = new PridejHruViewModel();
            PridejHru dialogHra = new PridejHru { DataContext = dialogHraViewModel };

            bool? dialogResultHra = dialogHra.ShowDialog();

            if (dialogHraViewModel.PridejHruUspesne)
            {
                poleHry.Add(dialogHraViewModel.Hra);
                Obnov(Vyber.Hry);
            }
        }
        private void PridejPobocku()
        {
            PridejPobockuViewModel dialogPobockaViewModel = new PridejPobockuViewModel();
            PridejPobocku dialogPobocka = new PridejPobocku { DataContext = dialogPobockaViewModel };

            bool? dialogResultPobocka = dialogPobocka.ShowDialog();

            if (dialogPobockaViewModel.PridejPobockuUspesne)
            {
                polePobocky.Add(dialogPobockaViewModel.Pobocka);
                Obnov(Vyber.Pobocky);
            }
        }
        private void PridejUzivatele()
        {
            PridejUzivateleViewModel dialogUzivatelViewModel = new PridejUzivateleViewModel();
            PridejUzivatele dialogUzivatel = new PridejUzivatele { DataContext = dialogUzivatelViewModel };

            bool? dialogResultUzivatel = dialogUzivatel.ShowDialog();

            if (dialogUzivatelViewModel.PridejUzivateleUspesne)
            {
                poleUzivatele.Add(dialogUzivatelViewModel.Uzivatel);
                Obnov(Vyber.Uzivatele);
            }
        }
        private void PridejPujcku()
        {
            PridejPujckuViewModel dialogPujckaViewModel = new PridejPujckuViewModel();
            PridejPujcku dialogPujcka = new PridejPujcku { DataContext = dialogPujckaViewModel };

            bool? dialogResultPujcka = dialogPujcka.ShowDialog();

            if (dialogPujckaViewModel.PridejPujckuUspesne)
            {
                polePujcky.Add(dialogPujckaViewModel.PujcenaHra);
                Obnov(Vyber.PujceneHry);
            }
        }
        private void ButtonUpravClick()
        {
            if (SelectedListBoxItem != null)
            {
                string selectedString = SelectedListBoxItem.ToString();
                string[] parts = selectedString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0 && int.TryParse(parts[parts.Length - 1].Trim(), out int itemId))
                {
                    switch (vyber)
                    {
                        case Vyber.Pobocky:
                            UpravPobocku(itemId);
                            break;
                        case Vyber.Hry:
                            UpravHru(itemId);
                            break;
                        case Vyber.Uzivatele:
                            UpravUzivatele(itemId);
                            break;
                    }
                }
            }
        }
        private void UpravPobocku(int pobockaId)
        {
            var pobockaToUpdate = polePobocky.GetAll().FirstOrDefault(p => p.Id == pobockaId);

            if (pobockaToUpdate != null)
            {
                PridejPobockuViewModel updatePobockyViewModel = new PridejPobockuViewModel(pobockaToUpdate);
                PridejPobocku updatePobocky = new PridejPobocku { DataContext = updatePobockyViewModel };
                bool? dialogResultPobocka = updatePobocky.ShowDialog();

                if (updatePobockyViewModel.PridejPobockuUspesne)
                {
                    Pobocka novaPobocka = updatePobockyViewModel.Pobocka;
                    novaPobocka.Id = pobockaId;

                    polePobocky.Update(novaPobocka);

                    foreach (var hra in poleHry.GetAll().Where(h => h.Pobocka.Id == pobockaId))
                    {
                        hra.Pobocka.AktualizujNazev(novaPobocka.Nazev);
                        poleHry.Update(hra);
                    }

                    Obnov(vyber);
                }
            }
        }
        private void UpravHru(int hraId)
        {
            var hraToUpdate = poleHry.GetAll().FirstOrDefault(h => h.Id == hraId);

            if (hraToUpdate != null)
            {
                PridejHruViewModel updateHryViewModel = new PridejHruViewModel(hraToUpdate, polePobocky);
                PridejHru updateHry = new PridejHru { DataContext = updateHryViewModel };
                bool? dialogResultHra = updateHry.ShowDialog();
                if (updateHryViewModel.PridejHruUspesne)
                {
                    hraToUpdate.Jmeno = updateHryViewModel.Hra.Jmeno;
                    hraToUpdate.Zanr = updateHryViewModel.Hra.Zanr;
                    hraToUpdate.Pocet = updateHryViewModel.Hra.Pocet;
                    hraToUpdate.Pobocka = updateHryViewModel.Hra.Pobocka;

                    foreach (var pujcenaHra in polePujcky.GetAll().Where(p => p.Hra.Id == hraToUpdate.Id))
                    {
                        pujcenaHra.Hra.Jmeno = updateHryViewModel.Hra.Jmeno;
                        polePujcky.Update(pujcenaHra);
                    }

                    poleHry.Update(hraToUpdate);
                    Obnov(vyber);
                }
            }
        }
        private void UpravUzivatele(int uzivatelId)
        {
            var uzivatelToUpdate = poleUzivatele.GetAll().FirstOrDefault(u => u.Id == uzivatelId);

            if (uzivatelToUpdate != null)
            {
                PridejUzivateleViewModel updateUzivateleViewModel = new PridejUzivateleViewModel(uzivatelToUpdate);
                PridejUzivatele updateUzivatele = new PridejUzivatele { DataContext = updateUzivateleViewModel };
                bool? dialogResulUzivatel = updateUzivatele.ShowDialog();
                if (updateUzivateleViewModel.PridejUzivateleUspesne)
                {
                    uzivatelToUpdate.Jmeno = updateUzivateleViewModel.Uzivatel.Jmeno;
                    uzivatelToUpdate.Adresa = updateUzivateleViewModel.Uzivatel.Adresa;
                    uzivatelToUpdate.Telefon = updateUzivateleViewModel.Uzivatel.Telefon;

                    foreach (var pujcenaHra in polePujcky.GetAll().Where(p => p.Uzivatel.Id == uzivatelToUpdate.Id))
                    {
                        pujcenaHra.Uzivatel.Jmeno = updateUzivateleViewModel.Uzivatel.Jmeno;
                        pujcenaHra.Uzivatel.Telefon = updateUzivateleViewModel.Uzivatel.Telefon;
                        polePujcky.Update(pujcenaHra);
                    }

                    poleUzivatele.Update(uzivatelToUpdate);
                    Obnov(vyber);
                }
            }
        }
        public void ButtonKonecClick()
        {
            MessageBoxResult dialogResult = MessageBox.Show("Opravdu chcete konec ?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dialogResult == MessageBoxResult.Yes)
            {
                db.Dispose();
                Application.Current.Shutdown();
            }
        }

        public event EventHandler<int> ItemFoundEvent;
        private void buttonNajdiHruClick()
        {
            string hledanyNazev = TextBoxNajdiHruText;
            bool nalezena = false;
            if (hledanyNazev != null)
            {
                for (int i = 0; i < FilteredListBoxItems.Count; i++)
                {
                    string listItem = FilteredListBoxItems[i];
                    if (listItem.Contains(hledanyNazev))
                    {
                        nalezena = true;

                        ItemFoundEvent?.Invoke(this, i);
                        break;
                    }
                }
                if (!nalezena)
                {
                    MessageBox.Show("Hra nenalezena!");
                }
            }
            else { MessageBox.Show("Prázdný název!"); }
        }
    }
}
