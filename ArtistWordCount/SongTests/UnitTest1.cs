using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtistWordCount;

namespace SongTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEmptySong()
        {
            Song song = new Song("", "");
            Assert.AreEqual(song.GetWordCount(), 0);
        }

        [TestMethod]
        public void TestDummySong()
        {
            Song song= new Song("artist", "one two three four five");
            Assert.AreEqual(song.GetWordCount(), 5);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestTenementFunsterAsync()
        {
            string actualLyrics = "My new purple shoes, bin' amazing the people next door\r\nAnd my rock 'n' roll 45's, bin' enraging the folks on the lower floor\r\nI got a way with the girls on my block\r\nTry my best be a real individual\r\nAnd when we go down to smokies and rock\n\nThey line up like it's some kinda ritual\n\n\n\nOoh give me a good guitar, and you can say that my hair's a disgrace\n\nOr, just find me an open car, I'll make the speed of light outta this place\n\n\n\nMmm, I like the good things in life\n\nBut most of the best things ain't free\n\nSome same situation, just cuts like a knife\n\nWhen you're young, and you're poor, and you're crazy, crazy\n\nYoung and you're crazy\n\nYoung and you're crazy\n\nYoung and you're crazy\n\n\n\nBut ooh give me a good guitar and you can say that my hair's a disgrace\n\nOr, just find me an open car, I'll make the speed of light outta this place";
            string lyrics = await SongComms.FindSongAsync("queen", "tenement funster");
            Song song = new Song("queen", lyrics);

            Assert.AreEqual(song.GetWordCount(), CountWords(actualLyrics));
            Assert.AreNotEqual(song.GetWordCount(), 4);
        }


        private int CountWords(string text)
        {
            int wordCount = 0;
            int index = 0;

            while (index < text.Length && char.IsWhiteSpace(text[index]))
            {
                index++;
            }

            while (index < text.Length)
            {
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                {
                    index++;
                }

                wordCount++;

                while (index < text.Length && char.IsWhiteSpace(text[index]))
                {
                    index++;
                }
            }

            return wordCount;
        }

    }
}
