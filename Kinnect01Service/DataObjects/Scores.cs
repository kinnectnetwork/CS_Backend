using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kinnect01Service.DataObjects
{
    public class BaseScore : EntityData
    {
        public int MentorValue { get; set; }
        public int MenteeValue { get; set; }
        public int PeerValue { get; set; }
    }

    public class AgeScore : BaseScore
    {
        public int AgeDistance { get; set; }
    }

    public class GenderScore : BaseScore
    {
        public int GenderDistance { get; set; }
    }

    public class IndustryScore : BaseScore
    {
        public string OwnIndustry { get; set; }
        public string TargetIndustry { get; set; }
    }

    public class JobLevelScore : BaseScore
    {
        public int JobLevelDistance { get; set; }
    }

    public class LocationScore : BaseScore
    {
        public int LocationDistance { get; set; }
    }

    public class OrganisationTypeScore : BaseScore
    {
        public string OwnOrganisationType { get; set; }
        public string TargetOrganisationType { get; set; }
    }

    public class TotalScore : BaseScore
    {
        public string Category { get; set; }
    }
}