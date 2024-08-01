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
using System.Threading.Tasks;
using System.Threading;

namespace Autodesk.APS.BIM360
{
    public class BimProjectsApi: APSApi
    {          

        public BimProjectsApi(Token token, ApplicationOptions options) : base(token, options)
        {            
            ContentType = "application/json";
        }

        public RestResponse PostProject(BimProject project, string accountId = null)
        {           
            //request.Resource = "hq/v1/accounts/{AccountId}/projects";
            var request = new RestRequest(Urls["projects"], Method.Post);

            if (accountId == null)
            {
                request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            }
            else
            {
                request.AddParameter("AccountId", accountId, ParameterType.UrlSegment);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd";
            settings.NullValueHandling = NullValueHandling.Ignore;
            string projectString = JsonConvert.SerializeObject(project, settings);
            request.AddParameter("application/json", projectString, ParameterType.RequestBody);

            request.AddHeader("content-type", ContentType);
            request.AddHeader("authorization", $"Bearer {Token.Result}");
            
            RestResponse response = ExecuteRequest(request);
            return response;
        }

        /// <summary>
        /// Assigns an admin user and services to a project. Returns an error if that user is already assigned
        /// To update projects and add new services use PatchProjects instead
        /// </summary>
        /// <param name="projectId">Id of the project</param>
        /// <param name="service">A ServiceActivation object</param>
        /// <returns>RestResponse object</returns>
        public RestResponse PostUserAndService(string projectId, ServiceActivation service)
        {
            //request.Resource = "hq/v1/accounts/{AccountId}/projects/{ProjectId}/users";
            var request = new RestRequest(Urls["projects_projectId_users"], Method.Post);

            request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            request.AddParameter("ProjectId", projectId, ParameterType.UrlSegment);

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", $"Bearer {Token.Result}");
            request.AddHeader("content-type", ContentType);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string serviceString = JsonConvert.SerializeObject(service, settings);
            request.AddParameter("application/json", serviceString, ParameterType.RequestBody);

            RestResponse response = ExecuteRequest(request); // Client.Execute(request);
            return response;
        }

        public RestResponse PostUsersImport(string projectId, string userId, List<ProjectUser> users, string accountId = null)
        {
            //request.Resource = "hq/v2/accounts/{AccountId}/projects/{ProjectId}/users/import";
            var request = new RestRequest(Urls["projects_projectId_users_import"], Method.Post);

            if (accountId == null)
            {
                request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            }
            else
            {
                request.AddParameter("AccountId", accountId, ParameterType.UrlSegment);
            }

            request.AddParameter("ProjectId", projectId, ParameterType.UrlSegment);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string serviceString = JsonConvert.SerializeObject(users, settings);
            request.AddParameter("application/json", serviceString, ParameterType.RequestBody);

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", $"Bearer {Token.Result}");
            request.AddHeader("content-type", ContentType);
            request.AddHeader("x-user-id", userId);

            RestResponse response = ExecuteRequest(request);
            return response;
        }

        public RestResponse PatchUser(string projectId, string adminUserId, string userId, ProjectUser user, string accountId = null)
        {
            var request = new RestRequest(Urls["projects_projectId_user_patch"], Method.Patch);
            if (accountId == null)
            {
                request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            }
            else
            {
                request.AddParameter("AccountId", accountId, ParameterType.UrlSegment);
            }

            request.AddParameter("ProjectId", projectId, ParameterType.UrlSegment);
            request.AddParameter("UserId", userId, ParameterType.UrlSegment);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            user.email = null;
            string serviceString = JsonConvert.SerializeObject(user, settings);
            request.AddParameter("application/json", serviceString, ParameterType.RequestBody);

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", $"Bearer {Token.Result}");
            request.AddHeader("content-type", ContentType);
            request.AddHeader("x-user-id", adminUserId);

            RestResponse response = ExecuteRequest(request);
            return response;
        }


        /// <summary>
        /// Update projects properties and services assigned to the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public RestResponse PatchProjects(string projectId, BimProject project)
        {
            //request.Resource = "hq/v1/accounts/{AccountId}/projects/{ProjectId}";
            var request = new RestRequest(Urls["projects_projectId"], Method.Patch);

            request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
            request.AddParameter("ProjectId", projectId, ParameterType.UrlSegment);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd";
            settings.NullValueHandling = NullValueHandling.Ignore;
            string projectString = JsonConvert.SerializeObject(project, settings);
            request.AddParameter("application/json", projectString, ParameterType.RequestBody);

            request.AddHeader("content-type", ContentType);
            request.AddHeader("authorization", $"Bearer {Token.Result}");

            RestResponse response = ExecuteRequest(request);
            return response;
        }


        /// <summary>
        /// Get all projects of an account - this uses the paged GET request and collects all
        /// results in a list which is the out parameter of the method
        /// </summary>
        /// <param name="result">List of all BimProject objects</param>
        /// <returns>RestResponse that indicates the status of the call</returns>
        public RestResponse GetProjects(out List<BimProject> result, string sortProp = "updated_at", int limit = 100, int offset = 0 )
        {
            Log.Info($"Querying Projects from AccountID '{options.APSBimAccountId}'");
            result = new List<BimProject>();
            List<BimProject> projects;
            RestResponse response = null;
            do
            {
                projects = null;
                try
                {
                    //request.Resource = "hq/v1/accounts/{AccountId}/projects";
                    var request = new RestRequest(Urls["projects"]);

                    request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
                    request.AddHeader("authorization", $"Bearer {Token.Result}");
                    request.AddParameter("sort", sortProp, ParameterType.QueryString);
                    request.AddParameter("limit", limit, ParameterType.QueryString);
                    request.AddParameter("offset", offset, ParameterType.QueryString);

                    response = ExecuteRequest(request);
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        projects = JsonConvert.DeserializeObject<List<BimProject>>(response.Content, settings);
                        result.AddRange(projects);
                        offset += limit;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    throw ex;
                }
            }
            while (projects != null && projects.Count == limit);

            return response;
        }




        /// <summary>
        /// Get all projects of an account - this uses the paged GET request and collects all
        /// results in a list which is the out parameter of the method
        /// </summary>
        /// <param name="result">List of all BimProject objects</param>
        /// <returns>RestResponse that indicates the status of the call</returns>
        public RestResponse GetProject(string projectId, string accountId = null)
        {
            Log.Info($"Querying Projects from AccountID '{options.APSBimAccountId}'");
            RestResponse response = null;
            try
            {
                //request.Resource = "hq/v1/accounts/{AccountId}/projects/{projectId}";
                var request = new RestRequest(Urls["projects_projectId"]);

                if (accountId == null)
                {
                    request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
                }
                else
                {
                    request.AddParameter("AccountId", accountId, ParameterType.UrlSegment);
                }

                request.AddParameter("ProjectId", projectId, ParameterType.UrlSegment);
                request.AddHeader("authorization", $"Bearer {Token.Result}");

                response = ExecuteRequest(request);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return response;
        }

        
        public RestResponse GetIndustryRoles(string projectId, out List<IndustryRole> result, string accountId = null)
        {
            Log.Info($"Querying industry roles from project '{projectId}'");
            result = new List<IndustryRole>();
            try
            {
                //request.Resource = "hq/v2/accounts/{AccountId}/projects/{ProjectId}/industry_roles";
                var request = new RestRequest(Urls["projects_projectId_industryRoles"]);

                if (accountId == null)
                {
                    request.AddParameter("AccountId", options.APSBimAccountId, ParameterType.UrlSegment);
                }
                else
                {
                    request.AddParameter("AccountId", accountId, ParameterType.UrlSegment);
                }

                request.AddParameter("ProjectId", projectId, ParameterType.UrlSegment);
                request.AddHeader("authorization", $"Bearer {Token.Result}");

                RestResponse response = ExecuteRequest(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    List<IndustryRole> roles = JsonConvert.DeserializeObject<List<IndustryRole>>(response.Content, settings);
                    result.AddRange(roles);
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
