using System.Collections.Generic;

namespace Nyss.Web.Features.DataCollectors
{
    public class DataCollectorViewModel
    {
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string YearOfBirth { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string Zone { get; set; }
        public IEnumerable<string> PhoneNumbers { get; set; }

        public string Supervisor { get; set; }
    }

    public static class DataCollectorViewModelExtensions
    {
        public static DataCollectorViewModel ToViewModel(this Nyss.Data.Models.DataCollector d)
        {
            return new DataCollectorViewModel
            {
                DisplayName = d.DisplayName,
                District = d.Village.District.Name,
                Village = d.Village.Name,
                FullName = d.Name,
                Language = d.Project.ContentLanguage.DisplayName,
                Latitude = d.Location.Y.ToString("0:0000"),
                Longitude = d.Location.X.ToString("0:0000"),
                PhoneNumbers = new[] { d.PhoneNumber },
                Region = d.Village.District.Region.Name,
                Sex = d.Id % 2 == 0 ? "Male" : "Female",
                Supervisor = d.Supervisor.Name,
                YearOfBirth = (1950 + d.Id % 50).ToString(),
                Zone = null
            };
        }
    }
}
