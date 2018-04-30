using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kinnect01Service.DataObjects
{
    public class JobLevelMapping : EntityData
    {
        public string JobLevelDesc { get; set; }
        public int JobLevelValue { get; set; }
    }
}