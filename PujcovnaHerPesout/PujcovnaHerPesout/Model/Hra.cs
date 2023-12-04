using LiteDB;

namespace PujcovnaHerPesout.Model
{
    public class Hra
    {
        [BsonId] public int Id { get; set; }
        public Pobocka Pobocka { get; set; }
        public string Jmeno { get; set; }
        public Zanr Zanr { get; set; }
        public int Pocet { get; set; }
        public Hra() { }

        public override string? ToString()
        {
            return string.Format(" Jméno: {1,-20} Žánr: {2,-15} Pocet: {3,-10} Pobocka: {0,-15} Id: {4}", Pobocka.Nazev, Jmeno, Zanr, Pocet, Id);
        }
    }
}
