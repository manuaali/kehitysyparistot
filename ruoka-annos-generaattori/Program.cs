namespace ruoka_annos_generaattori
{
    enum PaaraakaAine { Nauta, Kana, Kasvikset }
    enum Lisuke { Peruna, Riisi, Pasta }
    enum Kastike { Curry, Hapanimelä, Pippuri, Chili }

    class Ateria
    {
        public PaaraakaAine Paaraaka { get; set; }
        public Lisuke Lisuke { get; set; }
        public Kastike Kastike { get; set; }

        public override string ToString()
        {
            return $"{Paaraaka.ToString().ToLower()} ja {Lisuke.ToString().ToLower()} {Kastike.ToString().ToLower()}-kastikkeella";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Ateria> annokset = new List<Ateria>();

            Console.WriteLine("Valitse kolme annosta.");

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"\nAnnos {i + 1}:");

                Console.WriteLine("Valitse pääraaka-aine (Nauta, Kana, Kasvikset):");
                PaaraakaAine paaraaka = (PaaraakaAine)Enum.Parse(typeof(PaaraakaAine), Console.ReadLine(), true);

                Console.WriteLine("Valitse lisuke (Peruna, Riisi, Pasta):");
                Lisuke lisuke = (Lisuke)Enum.Parse(typeof(Lisuke), Console.ReadLine(), true);

                Console.WriteLine("Valitse kastike (Curry, Hapanimelä, Pippuri, Chili):");
                Kastike kastike = (Kastike)Enum.Parse(typeof(Kastike), Console.ReadLine(), true);

                Ateria ateria = new Ateria
                {
                    Paaraaka = paaraaka,
                    Lisuke = lisuke,
                    Kastike = kastike
                };

                annokset.Add(ateria);
            }

            Console.WriteLine("\nValitsemasi annokset:");
            foreach (var ateria in annokset)
            {
                Console.WriteLine(ateria);
            }
        }
    }
}
