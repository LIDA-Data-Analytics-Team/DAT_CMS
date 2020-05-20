﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataControlsLib.DataModels
{
    public class DsaModel
    {
        public int DsaId { get; set; }
        public int DataOwner { get; set; }
        public int? AmendmentOf { get; set; }
        public string DsaName { get; set; }
        public string DsaFileLoc { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool DSPT { get; set; }
        public bool ISO270001 { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}