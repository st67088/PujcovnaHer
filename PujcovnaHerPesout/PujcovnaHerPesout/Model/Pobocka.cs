using LiteDB;

namespace PujcovnaHerPesout.Model
{
    public class Pobocka
    {
        [BsonId] public int Id { get; set; }
        public string Nazev { get; set; }
        public string Adresa { get; set; }

        public Pobocka() { }
        public void AktualizujNazev(string novyNazev)
        {
            Nazev = novyNazev;
        }
        public override string? ToString()
        {
            return $"Název: {Nazev,-25} Adresa: {Adresa,-25}  Id: {Id}";
        }
    }
}
