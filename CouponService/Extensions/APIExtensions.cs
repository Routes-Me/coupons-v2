using RestSharp;
using System.Net;

namespace CouponService.Extensions
{
    public static class APIExtensions
    {
        public static IRestResponse GetAPI(string url, string query = "")
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.IsSuccessful ? response : throw new HttpListenerException((int)response.StatusCode, response.Content);
        }
    }
}
