using System;
using System.Collections.Generic;
using System.Text;

namespace ArtistWordCount
{
    public class Song
    {
        string title_;
        string lyrics_;
        int wordCount_;

        public Song(string title, string lyrics)
        {
            title_ = title;
            lyrics_ = lyrics;

            if (string.IsNullOrEmpty(lyrics_) == false)
            {
                CountWords();
            }
            else
            {
                wordCount_ = 0;
            }
        }

        public int GetWordCount()
        {
            return wordCount_;
        }

        private void CountWords()
        {
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            wordCount_= lyrics_.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
