using LiteDB;

namespace PujcovnaHerPesout.Model
{
    public class Uzivatel
    {
        [BsonId] public int Id { get; set; }
        public string Jmeno { get; set; }
        public string Adresa { get; set; }
        public int Telefon { get; set; }
        public Uzivatel() { }
        public override string? ToString()
        {
            return string.Format("Jméno: {0,-25} Adresa: {1,-25} Telefon: {2,-15} Id: {3}", Jmeno, Adresa, Telefon, Id);
        }
    }
}
