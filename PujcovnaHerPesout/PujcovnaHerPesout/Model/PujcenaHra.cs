using LiteDB;

namespace PujcovnaHerPesout.Model
{
    public class PujcenaHra
    {
        [BsonId] public int Id { get; set; }
        public Hra Hra { get; set; }
        public Uzivatel Uzivatel { get; set; }
        public PujcenaHra() { }
        public PujcenaHra(Hra hra, Uzivatel uzivatel)
        {
            Hra = hra;
            Uzivatel = uzivatel;
        }

        public override string? ToString()
        {
            return string.Format("Hra: {0,-20}  Uživatel: {1,-20} Telefon: {2,-15} Id: {3}", Hra.Jmeno, Uzivatel.Jmeno, Uzivatel.Telefon, Id);
        }
    }
}
