using System;

namespace ArtistWordCount
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Aire Logic Code Test v0.01");
            Console.WriteLine("==========================");

            Console.WriteLine();
            Console.WriteLine("Please enter an artist or band: ");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("That isn't a valid artist or band.");
                return;
            }

            if (input.Length > 256)
            {
                Console.WriteLine("Your input was considered too long at {0} characters.  Maximum input is 256 characters.", input.Length);
                return;
            }

            ArtistData artist = new ArtistData(input);
            if (await artist.InitAsync())
            {
                Console.WriteLine();
                Console.WriteLine(artist.Report());
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(artist.ReportError());
            }

        }
    }
}
