using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using Newtonsoft.Json;

namespace ArtistWordCount
{
    //https://musicbrainz.org/doc/Release

    //Classes generated from  http://json2csharp.com/
    public class Artist
    {
        public string id { get; set; }
        public string name { get; set; }
        [JsonProperty("sort-name")]
        public string SortName { get; set; }
        public string disambiguation { get; set; }
    }

    public class ArtistCredit
    {
        public Artist artist { get; set; }
        public string name { get; set; }
    }

    public class Release
    {
        public string id { get; set; }
        public string title { get; set; }
        public string status { get; set; }
    }

    public class Tag
    {
        public int count { get; set; }
        public string name { get; set; }
    }

    public class ReleaseGroup
    {
        public string id { get; set; }
        [JsonProperty("type-id")]
        public string TypeId { get; set; }
        public int score { get; set; }
        public int count { get; set; }
        public string title { get; set; }
        [JsonProperty("primary-type")]
        public string PrimaryType { get; set; }
        [JsonProperty("artist-credit")]
        public List<ArtistCredit> ArtistCredit { get; set; }
        public List<Release> releases { get; set; }
        public List<Tag> tags { get; set; }
        [JsonProperty("secondary-types")]
        public List<string> SecondaryTypes { get; set; }
    }

    public class ArtistRoot
    {
        public DateTime created { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        [JsonProperty("release-groups")]
        public List<ReleaseGroup> ReleaseGroups { get; set; }
    }

    public class Area
    {
        public string name { get; set; }
        public string id { get; set; }
        public object type { get; set; }
        [JsonProperty("sort-name")]
        public string SortName { get; set; }
        public string disambiguation { get; set; }
        [JsonProperty("iso-3166-1-codes")]
        public List<string> Iso31661Codes { get; set; }
        [JsonProperty("type-id")]
        public object TypeId { get; set; }
    }

    public class ReleaseEvent
    {
        public string date { get; set; }
        public Area area { get; set; }
    }

    public class CoverArtArchive
    {
        public int count { get; set; }
        public bool front { get; set; }
        public bool darkened { get; set; }
        public bool back { get; set; }
        public bool artwork { get; set; }
    }

    public class TextRepresentation
    {
        public string language { get; set; }
        public string script { get; set; }
    }

    public class Recording
    {
        public string length { get; set; }
        public string id { get; set; }
        public bool video { get; set; }
        public string disambiguation { get; set; }
        public string title { get; set; }
    }

    //http://musicbrainz.org/ws/2/release/46c31651-31b6-408d-9b1e-48bd16efd50d?inc=recordings has a length field of null which caused an exception and as we don't need it for now
    public class Track
    {
        public string number { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string length { get; set; }
        public Recording recording { get; set; }
        public int position { get; set; }
    }

    public class Medium
    {
        public string format { get; set; }
        public string title { get; set; }
        [JsonProperty("track-offset")]
        public int TrackOffset { get; set; }
        [JsonProperty("track-count")]
        public int TrackCount { get; set; }
        [JsonProperty("format-id")]
        public string FormatId { get; set; }
        public List<Track> tracks { get; set; }
        public int position { get; set; }
    }

    public class RecordingRoot
    {
        public string quality { get; set; }
        [JsonProperty("release-events")]
        public List<ReleaseEvent> ReleaseEvents { get; set; }
        [JsonProperty("cover-art-archive")]
        public CoverArtArchive CoverArtArchive { get; set; }
        public string status { get; set; }
        [JsonProperty("text-representation")]
        public TextRepresentation TextRepresentation { get; set; }
        public string packaging { get; set; }
        public List<Medium> media { get; set; }
        [JsonProperty("status-id")]
        public string StatusId { get; set; }
        public string country { get; set; }
        public string asin { get; set; }
        [JsonProperty("packaging-id")]
        public string PackagingId { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string barcode { get; set; }
        public string disambiguation { get; set; }
        public string date { get; set; }
    }

    class ArtistComms
    {
        //static Uri address_ = new Uri("https://api.lyrics.ovh/v1/");
        static Uri address_ = new Uri("http://musicbrainz.org/ws/2/");

        public ArtistComms()
        {

        }

        public static async System.Threading.Tasks.Task<List<string>> FindSongsAsync(string artist)
        {
            List<string> releaseIds = await CollectReleaseIdsAsync(artist);
            List<string> songTitles = await CollectSongTitlesAsync(releaseIds);

            return songTitles;
        }

        private static async System.Threading.Tasks.Task<List<string>> CollectSongTitlesAsync(List<string> releaseIds)
        {
            List<string> songTitles = new List<string>();

            using (HttpClient httpClient = new HttpClient { BaseAddress = address_ })
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "C# App");

                foreach (var releaseId in releaseIds)
                {
                    using (var response = await httpClient.GetAsync("release/" + releaseId + "?inc=recordings&fmt=json"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();

                            try
                            {
                                RecordingRoot root = JsonConvert.DeserializeObject<RecordingRoot>(responseData);

                                foreach (var item in root.media)
                                {
                                    if (item != null)
                                    {
                                        foreach (var track in item.tracks)
                                        {
                                            //Console.WriteLine(track.title);

                                            if (track != null && string.IsNullOrEmpty(track.title) == false)
                                            {
                                                if (songTitles.Contains(track.title) == false)
                                                {
                                                    songTitles.Add(track.title);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                //Assume something slightly odd with format of data
                                Console.WriteLine("Error parsing {0}", releaseId);
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
            }

            return songTitles;
        }

        private static async System.Threading.Tasks.Task<List<string>> CollectReleaseIdsAsync(string artist)
        {
            List<string> releaseIds = new List<string>();

            using (HttpClient httpClient = new HttpClient { BaseAddress = address_ })
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "C# App");

                bool firstTime = true;
                int count = 0;
                int offset = 0;

                do
                {
                    string encodedArtist = System.Web.HttpUtility.UrlEncode(artist);

                    try
                    {
                        using (var response = await httpClient.GetAsync("release-group/?query=artist:" + encodedArtist + "&fmt=json&offset=" + offset))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseData = await response.Content.ReadAsStringAsync();

                                ArtistRoot root = JsonConvert.DeserializeObject<ArtistRoot>(responseData);

                                if (firstTime)
                                {
                                    firstTime = false;
                                    count = root.count;

                                    Console.WriteLine("Looking at {0} items", count);
                                }

                                foreach (var item in root.ReleaseGroups)
                                {
                                    if (item != null)
                                    {
                                        if (item.releases != null)
                                        {
                                            foreach (var release in item.releases)
                                            {
                                                //Console.WriteLine(release.id);

                                                if (release != null && string.IsNullOrEmpty(release.id) == false)
                                                {
                                                    if (releaseIds.Contains(release.id) == false)
                                                    {
                                                        releaseIds.Add(release.id);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    Console.Write(".");

                                    offset++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error - {0}", e.Message);
                    }

                } while (count > offset);
            }

            Console.WriteLine("");

            return releaseIds;
        }

    }
}
