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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autodesk.APS.BIM360.Serialization
{
    public class NestedFolder
    {
        #region Propserties
        public string name { get; set; }
        public string id { get; set; }
        public NestedFolder parentFolder { get; set; }
        public List<NestedFolder> childrenFolders { get; set; }
        public IList<Data> items { get; set; }
        public List<FolderPermission> permissions { get; set; }
        #endregion

        #region Constructor
        public NestedFolder(string name, string id, NestedFolder parentFolder = null)
        {
            this.name = name;
            this.id = id;
            this.parentFolder = parentFolder;

            childrenFolders = new List<NestedFolder>();
            items = new List<Data>();
            permissions = new List<FolderPermission>();
        }
        #endregion
    }
}
