/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Autodesk
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

using Autodesk.APS.BIM360.Serialization;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Autodesk.APS.BIM360
{
    public class AccountApi : APSApi
    {
        public AccountApi(Token token, ApplicationOptions options) : base(token, options) { }

        public RestResponse GetAccountUsers(out List<HqUser> result, string accountId = null)
        {
            int limit = 100;
            Log.Info($"Querying Users for AccountID '{options.APSBimAccountId}'");
            result = new List<HqUser>();
            List<HqUser> users;
            RestResponse response = null;
            int offset = 0;
            do
            {
                users = null;
                try
                {
                    //request.Resource = "hq/v1/accounts/{AccountId}/users";
                    var request = new RestRequest(Urls["users"]);

                    if (accountId == null)
                    {
                        request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
                    }
                    else
                    {
                        request.AddParameter("AccountId", accountId, ParameterType.UrlSegment);
                    }
                    request.AddHeader("authorization", $"Bearer {Token.Result}");
                    request.AddParameter("limit", limit, ParameterType.QueryString);
                    request.AddParameter("offset", offset, ParameterType.QueryString);

                    response = ExecuteRequest(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        users = JsonConvert.DeserializeObject<List<HqUser>>(response.Content, settings);
                        result.AddRange(users);
                        offset += limit;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    throw ex;
                }
            }
            while (users != null && users.Count == limit);

            return response;
        }

        public RestResponse PostUsers(List<HqUser> users)
        {
            //request.Resource = "hq/v1/accounts/{AccountId}/users/import";
            var request = new RestRequest(Urls["users_import"], Method.Post);


            request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd";
            settings.NullValueHandling = NullValueHandling.Ignore;
            string projectString = JsonConvert.SerializeObject(users, settings);
            request.AddParameter("application/json", projectString, ParameterType.RequestBody);

            request.AddHeader("content-type", ContentType);
            request.AddHeader("authorization", $"Bearer {Token.Result}");

            RestResponse response = ExecuteRequest(request);
            return response;
        }

        public RestResponse GetCompanies(out List<BimCompany> result)
        {
            Log.Info($"Querying Companies for AccountID '{options.APSBimAccountId}'");
            result = new List<BimCompany>();
            List<BimCompany> companies;
            RestResponse response = null;
            int offset = 0;
            do
            {
                companies = null;
                try
                {
                    //request.Resource = "hq/v1/accounts/{AccountId}/companies";
                    var request = new RestRequest(Urls["companies"], Method.Get);

                    request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
                    request.AddHeader("authorization", $"Bearer {Token.Result}");
                    request.AddParameter("limit", 100);
                    request.AddParameter("offset", offset);

                    response = ExecuteRequest(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        companies = JsonConvert.DeserializeObject<List<BimCompany>>(response.Content, settings);
                        result.AddRange(companies);
                        offset += 100;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    throw ex;
                }
            }
            while (companies != null && companies.Count == 100);

            return response;
        }

        public RestResponse PostCompanies(List<BimCompany> companies)
        {
            //request.Resource = "hq/v1/accounts/{AccountId}/companies/import";
            var request = new RestRequest(Urls["companies_import"], Method.Post);

            request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd";
            settings.NullValueHandling = NullValueHandling.Ignore;
            string projectString = JsonConvert.SerializeObject(companies, settings);
            request.AddParameter("application/json", projectString, ParameterType.RequestBody);

            request.AddHeader("content-type", ContentType);
            request.AddHeader("authorization", $"Bearer {Token.Result}");

            RestResponse response = ExecuteRequest(request);
            return response;
        }

        public RestResponse GetBusinessUnits(out List<BusinessUnit> businessUnits)
        {
            Log.Info($"Querying Business Units for AccountID '{options.APSBimAccountId}'");
            businessUnits = new List<BusinessUnit>();
            try
            {
                //request.Resource = "hq/v1/accounts/{AccountId}/business_units_structure";
                var request = new RestRequest(Urls["businessUnitsStructure"] );
                request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
                request.AddHeader("authorization", $"Bearer {Token.Result}");

                RestResponse response = ExecuteRequest(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    BusinessUnits units = JsonConvert.DeserializeObject<BusinessUnits>(response.Content, settings);
                    if (units != null && units.business_units != null)
                    {
                        businessUnits.AddRange(units.business_units);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
    }


}
