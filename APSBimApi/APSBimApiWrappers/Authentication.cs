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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Autodesk.SDKManager;
using Autodesk.Authentication;
using Autodesk.Authentication.Model;

using NLog;

namespace Autodesk.APS.BIM360
{
    public class Authentication
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static Autodesk.SDKManager.SDKManager _SDKManager;
        private static List<Scopes> _scope = new List<Scopes> { Scopes.DataSearch, Scopes.DataCreate, Scopes.DataRead, Scopes.DataWrite, Scopes.AccountRead, Scopes.AccountWrite };

        public static async Task<string> Authenticate(ApplicationOptions options)
        {
            string token = null;
            _SDKManager = SdkManagerBuilder.Create().Build();
            try
            {
                AuthenticationClient authenticationClient = new AuthenticationClient(_SDKManager);
                var tokenRes = await authenticationClient.GetTwoLeggedTokenAsync(options.APSClientId, options.APSClientSecret, _scope);
                if (tokenRes == null || tokenRes.AccessToken == null )
                {
                    Log.Info("You were not granted a new access_token!");
                    return null;
                }
                token = tokenRes.AccessToken;
            }
            catch (Exception ex )
            {
                Log.Info("Exception while get token: " + ex.Message);
                throw ex;
            }
            return token;
        }
    }
}
