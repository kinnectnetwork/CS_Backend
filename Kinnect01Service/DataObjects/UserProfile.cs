using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kinnect01Service.DataObjects
{
    public class UserProfile : EntityData
    {

        public bool SeekMentorInd { get; set; }
        public bool SeekMenteeInd { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }

        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public string Industry { get; set; }
        public string OrganisationType { get; set; }
        public string JobLevel { get; set; }
        public int YearsOfExperience { get; set; }

        public string OrganisationName { get; set; }
        public string PositionTitle { get; set; }

    }
}