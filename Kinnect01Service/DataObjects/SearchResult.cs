using Kinnect01Service.Models;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kinnect01Service.DataObjects
{

    public class SearchScore
    {
        public int Id { get; set; }
        public string OwnUserId { get; set; }
        public string TargetUserId { get; set; }
        public string SearchType { get; set; }

        public double Distance { get; set; }

        public double GenderScore { get; set; }
        public double AgeScore { get; set; }
        public double LocationScore { get; set; }
        public double IndustryScore { get; set; }
        public double OrganisationTypeScore { get; set; }
        public double JobLevelScore { get; set; }
        public double TotalScore { get; set; }
    }
    public class SearchFilterCriteria
    {
        public string ownGender { get; set; }
        public string ownAge { get; set; }
        public double ownIndustry { get; set; }
    }

    public class SearchResult
    {
        public string Name { get; set; }
        public string PositionTitle { get; set; }
        public double TotalScore { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }
        public string OrganisationType { get; set; }
        public string FocusArea { get; set; }
        public string OrganisationName { get; set; }

        public string OverseasExperience { get; set; }
        public string ProfessionalInterests { get; set; }
    }

    public class SearchResultHelper
    {
        static private Kinnect01Context context = new Kinnect01Context();

        static public List<SearchResult> GetSearchResults(string ownUserId, string searchType)
        {

            //save upserProfiles to memory

            List<UserProfile> userProfiles = context.UserProfiles.ToList();

            //Get all records containing own profile from SearchScores Table
            List<SearchScore> searchscores = context.SearchScores.Where(u => u.OwnUserId == ownUserId).ToList();

            //define output list
            List<SearchResult> searchResults = new List<SearchResult>();

            foreach (SearchScore item in searchscores)
            {
                //Retrieve userprofile
                UserProfile targetProfile = userProfiles.Single(u => u.Id == item.TargetUserId);

                searchResults.Add(new SearchResult()
                {
                    Name = targetProfile.Name,
                    PositionTitle = targetProfile.PositionTitle,
                    TotalScore = item.TotalScore,
                    Location = targetProfile.Location,
                    Industry = targetProfile.Industry,
                    OrganisationType = targetProfile.OrganisationType,
                    FocusArea = String.Empty,
                    OrganisationName = targetProfile.OrganisationName,
                    OverseasExperience = String.Empty,
                    ProfessionalInterests = String.Empty
                }
                    );
            }

            return searchResults;

        }

        static public List<SearchScore> PopulateSearchScoresTable(string searchType)
        {

            List<SearchScore> finalSearchScores = new List<SearchScore>();

            //Define Weight from looking it up in total score
            TotalScore genderWeight = context.TotalScores.Single(u => u.Category == "Gender");
            TotalScore ageWeight = context.TotalScores.Single(u => u.Category == "Age");
            TotalScore locationWeight = context.TotalScores.Single(u => u.Category == "Location");
            TotalScore industryWeight = context.TotalScores.Single(u => u.Category == "Industry");
            TotalScore organisationTypeWeight = context.TotalScores.Single(u => u.Category == "OrgType");
            TotalScore jobLevelWeight = context.TotalScores.Single(u => u.Category == "JobLevel");

            //from the record, retrieve the score (note this has to be together)
            long genderWeight_Final = Convert.ToInt64(genderWeight.GetType().GetProperty(searchType + "Value").GetValue(genderWeight, null));
            long ageWeight_Final = Convert.ToInt64(ageWeight.GetType().GetProperty(searchType + "Value").GetValue(ageWeight, null));
            long locationWeight_Final = Convert.ToInt64(locationWeight.GetType().GetProperty(searchType + "Value").GetValue(locationWeight, null));
            long industryWeight_Final = Convert.ToInt64(industryWeight.GetType().GetProperty(searchType + "Value").GetValue(industryWeight, null));
            long organisationTypeWeight_Final = Convert.ToInt64(organisationTypeWeight.GetType().GetProperty(searchType + "Value").GetValue(organisationTypeWeight, null));
            long jobLevelWeight_Final = Convert.ToInt64(jobLevelWeight.GetType().GetProperty(searchType + "Value").GetValue(jobLevelWeight, null));

            //Pick up values from each table and save it in memory
            List<AgeScore> ageScores = context.AgeScores.ToList();
            List<GenderScore> genderScores = context.GenderScores.ToList();
            List<IndustryScore> industryScores = context.IndustryScores.ToList();
            List<JobLevelScore> jobLevelScores = context.JobLevelScores.ToList();
            List<LocationScore> locationScores = context.LocationScores.ToList();
            List<OrganisationTypeScore> organisationTypeScores = context.OrganisationTypeScores.ToList();

            //Pick up values from JobMapping
            List<JobLevelMapping> jobLevelMappings = context.JobLevelMappings.ToList();

            //Save UserProfiles in memory
            List<UserProfile> userProfiles = context.UserProfiles.ToList();

            //Being Loop
            foreach (UserProfile ownProfile in userProfiles)
            {
                //define output SearchResult record for each OwnProfile
                List<SearchScore> searchscores = new List<SearchScore>();

                //loop through table to get score for indivdiual items
                foreach (UserProfile targetProfile in userProfiles)
                {
                    //Ignore the following
                    if (ownProfile.Id == targetProfile.Id) continue;
                    //need to add Mentor / mentee Filter

                    //Get Score for each dimension
                    double genderScore = GetGenderScore(genderScores, ownProfile.Gender, targetProfile.Gender, searchType);
                    double ageScore = GetAgeScore(ageScores, ownProfile.Age, targetProfile.Age, searchType);
                    double locationScore = GetLocationScore(locationScores, ownProfile.Latitude, ownProfile.Longtitude, targetProfile.Latitude, targetProfile.Longtitude, searchType);
                    double industryScore = GetIndustryScore(industryScores, ownProfile.Industry, targetProfile.Industry, searchType);
                    double organisationTypeScore = GetOrganisationTypeScore(organisationTypeScores, ownProfile.OrganisationType, targetProfile.OrganisationType, searchType);
                    double jobLevelScore = GetJobLevelScore(jobLevelMappings, jobLevelScores, ownProfile.JobLevel, targetProfile.JobLevel, searchType);

                    //Calculate TotalScore
                    double totalScore = (genderWeight_Final * genderScore
                                        + ageWeight_Final * ageScore
                                        + locationWeight_Final * locationScore
                                        + industryWeight_Final * industryScore
                                        + organisationTypeWeight_Final * organisationTypeScore
                                        + jobLevelWeight_Final * jobLevelScore) / 100;

                    searchscores.Add(new SearchScore()
                    {
                        OwnUserId = ownProfile.Id,
                        TargetUserId = targetProfile.Id,
                        SearchType = searchType,
                        Distance = DistanceAlgorithm.DistanceBetweenPlaces(ownProfile.Latitude, ownProfile.Longtitude, targetProfile.Latitude, targetProfile.Longtitude),
                        GenderScore = genderScore,
                        AgeScore = ageScore,
                        LocationScore = locationScore,
                        IndustryScore = industryScore,
                        OrganisationTypeScore = organisationTypeScore,
                        JobLevelScore = jobLevelScore,
                        TotalScore = totalScore
                    });

                }

                //Get top 10 search results and save to final list
                searchscores = searchscores.OrderByDescending(x => x.TotalScore).ToList();
                //searchscores = searchscores.OrderByDescending(x => x.TotalScore).Take(10).ToList();
                foreach (SearchScore item in searchscores)
                {
                    finalSearchScores.Add(item);
                }
            }

            return (finalSearchScores);
        }

        static private double GetGenderScore(List<GenderScore> genderScores, string ownGender, string targetGender, string searchType)
        {
            //Get gender Difference
            int genderDifference;
            if (ownGender == targetGender)
                genderDifference = 0;
            else
                genderDifference = 1;

            //Get Gender MappingProfile
            GenderScore genderScore = genderScores.Where(u => u.GenderDistance == genderDifference).FirstOrDefault();

            if (genderScore == null) return 0;
            double output = Convert.ToInt64(genderScore.GetType().GetProperty(searchType + "Value").GetValue(genderScore, null));
            return (output);
        }

        static private double GetAgeScore(List<AgeScore> ageScores, int ownAge, int targetAge, string searchType)
        {
            //Get Age difference
            int diffAge = Math.Min(Math.Max(targetAge - ownAge, -10), 10);

            //Get AgeScore profile
            AgeScore ageScore = ageScores.Where(u => u.AgeDistance == diffAge).FirstOrDefault();

            if (ageScore == null) return 0;

            double output = Convert.ToInt64(ageScore.GetType().GetProperty(searchType + "Value").GetValue(ageScore, null));
            return (output);

        }

        static private double GetLocationScore(List<LocationScore> locationScores, double ownLatitude, double ownLongtitude, double targetLatitude, double targetLongtitude, string searchType)
        {
            int locationDifference;
            double distance = DistanceAlgorithm.DistanceBetweenPlaces(ownLatitude, ownLongtitude, targetLatitude, targetLongtitude);

            if (distance > 1000) { locationDifference = 1000; }
            else if (distance > 500) { locationDifference = 500; }
            else if (distance > 250) { locationDifference = 250; }
            else if (distance > 100) { locationDifference = 100; }
            else if (distance > 50) { locationDifference = 50; }
            else if (distance > 25) { locationDifference = 25; }
            else { locationDifference = 0; }

            LocationScore locationScore = locationScores.Where(u => u.LocationDistance == locationDifference).FirstOrDefault();

            if (locationScore == null) return 0;

            double output = Convert.ToInt64(locationScore.GetType().GetProperty(searchType + "Value").GetValue(locationScore, null));
            return (output);

        }

        static private double GetIndustryScore(List<IndustryScore> industryScores, string ownIndustry, string targetIndustry, string searchType)
        {
            IndustryScore industryScore = industryScores.Where(u => u.OwnIndustry == ownIndustry).Where(u => u.TargetIndustry == targetIndustry).FirstOrDefault();

            if (industryScore == null) return 0;

            double output = Convert.ToInt64(industryScore.GetType().GetProperty(searchType + "Value").GetValue(industryScore, null));
            return (output);

        }

        static private double GetOrganisationTypeScore(List<OrganisationTypeScore> organisationTypeScores, string ownOrganisationType, string targetOrganisationType, string searchType)
        {
            OrganisationTypeScore organisationTypeScore = organisationTypeScores.Where(u => u.OwnOrganisationType == ownOrganisationType).Where(u => u.TargetOrganisationType == targetOrganisationType).FirstOrDefault();

            if (organisationTypeScore == null) return 0;

            double output = Convert.ToInt64(organisationTypeScore.GetType().GetProperty(searchType + "Value").GetValue(organisationTypeScore, null));
            return (output);

        }

        static private double GetJobLevelScore(List<JobLevelMapping> jobLevelMappings, List<JobLevelScore> jobLevelScores, string ownJobLevel, string targetJobLevel, string searchType)
        {
            JobLevelMapping ownJobMapping = jobLevelMappings.Where(u => u.JobLevelDesc == ownJobLevel).FirstOrDefault();
            JobLevelMapping targetJobMapping = jobLevelMappings.Where(u => u.JobLevelDesc == targetJobLevel).FirstOrDefault();

            if (ownJobMapping == null || targetJobMapping == null) return 0;

            int jobLevelDifference = targetJobMapping.JobLevelValue - ownJobMapping.JobLevelValue;

            JobLevelScore jobLevelScore = jobLevelScores.Where(u => u.JobLevelDistance == jobLevelDifference).FirstOrDefault();

            double output = Convert.ToInt64(jobLevelScore.GetType().GetProperty(searchType + "Value").GetValue(jobLevelScore, null));
            return (output);
        }
    }
}