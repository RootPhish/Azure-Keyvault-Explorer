﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyvaultExplorer.Classes
{
    internal class AppSettings
    {
        public string TenantID { get; set; } = "00000000-0000-0000-0000-000000000000";
        public string ClientID { get; set; } = "00000000-0000-0000-0000-000000000000";
        public int ClearClipboardAfterSeconds { get; set; } = 10;
    }
}