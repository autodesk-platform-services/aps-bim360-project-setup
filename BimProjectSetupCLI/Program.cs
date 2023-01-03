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

using NLog;
using System;

using BimProjectSetupCommon;

namespace Autodesk.BimProjectSetup
{
    public class Program
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Application.PrintHelp();
                return;
            }
            if (args[0] != null && args[0].Trim().Equals("/?"))
            {
                Application.PrintHelp();
                return;
            }
            try
            {                
                Application.PrintHeader();
                AppOptions options = AppOptions.Parse(args);
                Application app = new Application(options);
                if(app.Initialize() == true)
                {
                    app.Process();
                }
                else
                {
                    Console.WriteLine("Application could not initialize correctly!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                Console.WriteLine("ERROR: {0}", ex.StackTrace);
                Application.PrintHelp();
            }
        }
    }
}
