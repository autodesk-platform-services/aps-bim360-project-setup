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
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Autodesk.APS.BIM360.Serialization
{
  public class Extension
    {
        #region Properties

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("version")]
        public string version { get; set; }

        //[JsonProperty("schema")]
        //public Schema schema { get; set; }

        //[JsonProperty("data")]
        //public Data data { get; set; }

        #endregion Properties

        #region Constructors

        [DebuggerStepThrough]
        public Extension()
        {
        } // constructor
        #endregion Constructors
    } // class
} // namespace
