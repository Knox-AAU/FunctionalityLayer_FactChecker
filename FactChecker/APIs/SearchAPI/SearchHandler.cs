using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FactChecker.APIs
{
    public class SearchHandler
    {

        public string wordRatioURL = "http://knox-node02.srv.aau.dk/WordRatio";

        static HttpClient client = new HttpClient();

        public async Task<SearchItem[]> GetSearchItem (string term)
        {
            SearchItem[] articles = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(wordRatioURL + "?terms=" + term);
                if (response.IsSuccessStatusCode)
                {
                    articles = await response.Content.ReadAsAsync<SearchItem[]>();
                }
            }
            catch
            {
                throw;
            }
            return articles;
        }
    }
}
