using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using Newtonsoft.Json;

namespace ArtistWordCount
{
    internal class Lyrics
    {
        public string lyrics { get; set; }
    }
    public class SongComms
    {
        //https://lyricsovh.docs.apiary.io/#reference/0/lyrics-of-a-song/search?console=1

        //static Uri address_ = new Uri("https://api.lyrics.ovh/v1/");
        static Uri address_ = new Uri("https://private-anon-4e537e977a-lyricsovh.apiary-proxy.com/v1/");

        public SongComms()
        {

        }

        public static async System.Threading.Tasks.Task<string> FindSongAsync(string artist, string songTitle)
        {
            string encodedArtist = System.Web.HttpUtility.UrlEncode(artist);
            string encodedTitle = System.Web.HttpUtility.UrlEncode(songTitle);

            using (HttpClient httpClient = new HttpClient { BaseAddress = address_ })
            {
                using (var response = await httpClient.GetAsync(encodedArtist + "/" + encodedTitle)) 
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        Lyrics lyrics = JsonConvert.DeserializeObject<Lyrics>(responseData);

                        return lyrics.lyrics;
                    }
                }
            }

            return "";
        }
    }
}
