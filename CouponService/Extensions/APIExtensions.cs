using CouponService.Functions;
using CouponService.Models.Dto;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static CouponService.Models.ResponseModel.Response;

namespace CouponService.Extensions
{
    public static class ApiExtensions
    {
        public static IRestResponse GetApi(string url, string query = "")
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return response.IsSuccessful ? response : throw new HttpListenerException((int)response.StatusCode, response.Content);
        }
        public static dynamic GetUsersIncludedData(List<CouponReadDto> couponsModelList, string url)
        {
            var lstUsers = new List<UserReadDto>();
            foreach (GetResponseApi<UserReadDto> userData in from item in couponsModelList select new RestClient(url + item.UserId) into client let request = new RestRequest(Method.GET) select client.Execute(request) into response where response.StatusCode == HttpStatusCode.OK select response.Content into result select JsonConvert.DeserializeObject<GetResponseApi<UserReadDto>>(result))
            {
                lstUsers.AddRange(userData.Data);
            }
            var usersList = lstUsers.GroupBy(x => x.UserId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(usersList.Cast<dynamic>().ToList());
        }

        public static dynamic GetByIdUsersIncludedData(CouponReadDto couponsMode, string url)
        {
            var lstUsers = new List<UserReadDto>();

            var client = new RestClient(url + couponsMode.UserId);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content;
                var userData = JsonConvert.DeserializeObject<GetResponseApi<UserReadDto>>(result);
                lstUsers.AddRange(userData.Data);
            }

            var usersList = lstUsers.GroupBy(x => x.UserId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(usersList.Cast<dynamic>().ToList());
        }

        public static dynamic GetInstitutionsIncludedData(List<PromotionReadDto> authoritiesModelList, string url)
        {
            var lstInstitutions = new List<InstitutionReadDto>();
            foreach (var item in authoritiesModelList)
            {
                var client = new RestClient(url + item.Institution_Id);
                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content;
                    var institutionsData = JsonConvert.DeserializeObject<GetResponseApi<InstitutionReadDto>>(result);
                    lstInstitutions.AddRange(institutionsData.Data);
                }
            }
            var institutionsList = lstInstitutions.GroupBy(x => x.InstitutionId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(institutionsList.Cast<dynamic>().ToList());
        }

        public static dynamic GetByIdInstitutionsIncludedData(PromotionReadDto authoritiesModelList, string url)
        {
            var lstInstitutions = new List<InstitutionReadDto>();

            var client = new RestClient(url + authoritiesModelList.Institution_Id);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content;
                var institutionsData = JsonConvert.DeserializeObject<GetResponseApi<InstitutionReadDto>>(result);
                lstInstitutions.AddRange(institutionsData.Data);
            }

            var institutionsList = lstInstitutions.GroupBy(x => x.InstitutionId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(institutionsList.Cast<dynamic>().ToList());
        }

        public static dynamic GetAdvertisementsIncludedData(List<PromotionReadDto> promotionsModelList, string url)
        {
            var lstAdvertisements = new List<AdvertisementReadDto>();
            foreach (var item in promotionsModelList)
            {
                var client = new RestClient(url + item.Advertisement_Id);
                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content;
                    var advertisementsData = JsonConvert.DeserializeObject<GetResponseApi<AdvertisementReadDto>>(result);
                    lstAdvertisements.AddRange(advertisementsData.Data);
                }
            }
            var advertisementsList = lstAdvertisements.GroupBy(x => x.AdvertisementId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(advertisementsList.Cast<dynamic>().ToList());
        }

        public static dynamic GetAdvertisementsByIdIncludedData(PromotionReadDto promotionsModelList, string url)
        {
            var lstAdvertisements = new List<AdvertisementReadDto>();

            var client = new RestClient(url + promotionsModelList.Advertisement_Id);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content;
                var advertisementsData = JsonConvert.DeserializeObject<GetResponseApi<AdvertisementReadDto>>(result);
                lstAdvertisements.AddRange(advertisementsData.Data);
            }

            var advertisementsList = lstAdvertisements.GroupBy(x => x.AdvertisementId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(advertisementsList.Cast<dynamic>().ToList());
        }
    }
}
