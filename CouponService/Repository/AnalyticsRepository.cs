﻿using AdvertisementService.Models;
using CouponService.Abstraction;
using CouponService.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CouponService.Repository
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AppSettings _appSettings;
        private readonly CouponContext _context;
        private readonly Dependencies _dependencies;

        public AnalyticsRepository(IOptions<AppSettings> appSettings, CouponContext context, IOptions<Dependencies> dependencies)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _dependencies = dependencies.Value;
        }
        public void InsertAnalytics()
        {
            try
            {
                DateTime? lastCouponDate = null;
                UriBuilder uriBuilder = new UriBuilder(_appSettings.Host + _dependencies.AnalyticsUrl + "lastdate");
                uriBuilder = AppendQueryToUrl(uriBuilder, "type=coupons");
                var client = new RestClient(uriBuilder.Uri);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content;
                    var analyticsData = JsonConvert.DeserializeObject<GetAnalyticsResponse>(result);
                    if (analyticsData != null)
                        lastCouponDate = analyticsData.CreatedAt;
                }

                if (lastCouponDate != null)
                {
                    List<PromotionAnalytics> promotionAnalyticsList = new List<PromotionAnalytics>();
                    var redemptions = _context.Redemptions.Include(x => x.Coupon).Include(x => x.Coupon.Promotion).Where(x => x.CreatedAt > lastCouponDate).ToList();
                    if (redemptions != null && redemptions.Count > 0)
                    {
                        foreach (var group in redemptions.GroupBy(x => x.CouponId))
                        {
                            var items = group.FirstOrDefault();
                            PromotionAnalytics promotionAnalytics = new PromotionAnalytics();
                            promotionAnalytics.PromotionId = Obfuscation.Encode(items.Coupon.Promotion.PromotionId);
                            promotionAnalytics.AdvertismentId = Obfuscation.Encode(Convert.ToInt32(items.Coupon.Promotion.AdvertisementId));
                            promotionAnalytics.InstitutionId = Obfuscation.Encode(Convert.ToInt32(items.Coupon.Promotion.InstitutionId));
                            promotionAnalytics.CreatedAt = DateTime.Now;
                            promotionAnalytics.Count = group.Count();
                            promotionAnalytics.Type = "coupons";
                            promotionAnalyticsList.Add(promotionAnalytics);
                        }
                    }

                    if (promotionAnalyticsList != null && promotionAnalyticsList.Count > 0)
                    {
                        AnalyticsModel analyticsModel = new AnalyticsModel()
                        {
                            analytics = promotionAnalyticsList
                        };

                        var postClient = new RestClient(_appSettings.Host + _dependencies.AnalyticsUrl);
                        var postRequest = new RestRequest(Method.POST);
                        string jsonToSend = JsonConvert.SerializeObject(analyticsModel);
                        postRequest.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                        postRequest.RequestFormat = DataFormat.Json;
                        IRestResponse institutionResponse = postClient.Execute(postRequest);
                        if (institutionResponse.StatusCode != HttpStatusCode.Created)
                        {

                        }
                    }
                }
                else
                {
                    List<PromotionAnalytics> promotionAnalyticsList = new List<PromotionAnalytics>();
                    var redemptions = _context.Redemptions.Include(x => x.Coupon).Include(x => x.Coupon.Promotion).ToList();
                    if (redemptions != null)
                    {
                        foreach (var group in redemptions.GroupBy(x => x.CouponId))
                        {
                            var items = group.FirstOrDefault();
                            PromotionAnalytics promotionAnalytics = new PromotionAnalytics();
                            promotionAnalytics.PromotionId = Obfuscation.Encode(items.Coupon.Promotion.PromotionId).ToString();
                            promotionAnalytics.AdvertismentId = Obfuscation.Encode(Convert.ToInt32(items.Coupon.Promotion.AdvertisementId));
                            promotionAnalytics.InstitutionId = Obfuscation.Encode(Convert.ToInt32(items.Coupon.Promotion.InstitutionId));
                            promotionAnalytics.CreatedAt = DateTime.Now;
                            promotionAnalytics.Count = group.Count();
                            promotionAnalytics.Type = "coupons";
                            promotionAnalyticsList.Add(promotionAnalytics);
                        }
                    }
                    if (promotionAnalyticsList != null && promotionAnalyticsList.Count > 0)
                    {
                        AnalyticsModel analyticsModel = new AnalyticsModel()
                        {
                            analytics = promotionAnalyticsList
                        };

                        var postClient = new RestClient(_appSettings.Host + _dependencies.AnalyticsUrl);
                        var postRequest = new RestRequest(Method.POST);
                        string jsonToSend = JsonConvert.SerializeObject(analyticsModel);
                        postRequest.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                        postRequest.RequestFormat = DataFormat.Json;
                        IRestResponse institutionResponse = postClient.Execute(postRequest);
                        if (institutionResponse.StatusCode != HttpStatusCode.Created)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private UriBuilder AppendQueryToUrl(UriBuilder baseUri, string queryToAppend)
        {
            if (baseUri.Query != null && baseUri.Query.Length > 1)
                baseUri.Query = baseUri.Query.Substring(1) + "&" + queryToAppend;
            else
                baseUri.Query = queryToAppend;
            return baseUri;
        }
    }
}
