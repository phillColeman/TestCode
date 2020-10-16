using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArtistWordCount
{
    public class ArtistData
    {
        string artist_;
        List<Song> songs_;
        double average_;

        public ArtistData(string artist)
        {
            artist_ = artist;
            songs_ = new List<Song>();
        }

        public async Task<bool> InitAsync()
        {
            List<string> songs = await ArtistComms.FindSongsAsync(artist_);

            foreach (var song in songs)
            {
                string lyrics = await SongComms.FindSongAsync(artist_, song);

                if (string.IsNullOrEmpty(lyrics) == false)
                {
                    AddSong(song, lyrics);
                    Console.Write("+");
                }
                else
                {
                    Console.Write("-");
                }
            }

            CalculateAverage();

            return true;
        }

        public void CalculateAverage()
        {
            if (songs_.Count > 0)
            {
                int total = 0;
                foreach (var song in songs_)
                {
                    total += song.GetWordCount();
                }
                average_ = total / songs_.Count;
            }
        }

        private void AddSong(string title, string lyrics)
        {
            Song song = new Song(title, lyrics);
            songs_.Add(song);
        }

        public void AddSong(Song song)
        {
            songs_.Add(song);
        }


        public string Report()
        {
            if (songs_.Count > 0)
            {
                if (songs_.Count == 1)
                {
                    return string.Format("{0} has only one song found with word count of {1}", artist_, average_);
                }
                
                return string.Format("{0} has {1} songs with an average word count of {2}", artist_, songs_.Count, average_);
            }
            else
            {
                return string.Format("{0} had no known songs", artist_);
            }
        }

        internal string ReportError()
        {
            return "Unexpected Error";
        }
    }
}
