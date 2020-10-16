using Microsoft.VisualStudio.TestTools.UnitTesting;

using ArtistWordCount;

namespace ArtistTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestArtist()
        {
            Song one = new Song("artist", "one two three four");
            Song two = new Song("artist", "one two three four five six");

            ArtistData artist = new ArtistData("artist");
            artist.AddSong(one);
            artist.AddSong(two);
            artist.CalculateAverage();

            Assert.AreEqual(artist.Report(), "artist has 2 songs with an average word count of 5");

        }

        [TestMethod]
        public void TestArtistSingleSong()
        {
            Song one = new Song("artist", "one two three four");

            ArtistData artist = new ArtistData("artist");
            artist.AddSong(one);
            artist.CalculateAverage();

            Assert.AreEqual(artist.Report(), "artist has only one song found with word count of 4");

        }

        [TestMethod]
        public void TestArtistNoSongs()
        {
            Song one = new Song("artist", "one two three four");

            ArtistData artist = new ArtistData("artist");
            artist.CalculateAverage();

            Assert.AreEqual(artist.Report(), "artist had no known songs");

        }
    }
}
