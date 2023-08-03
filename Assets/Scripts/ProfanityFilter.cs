using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


public class ProfanityFilter
{
    public async Task<string> FilterWord(string unfilteredWord)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = "https://www.purgomalum.com/service/plain?text=" + unfilteredWord;
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return unfilteredWord;
            }
        }
    }
}