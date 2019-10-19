
using System;

namespace Nyss.Web.Features.Reports
{
    public class ReportViewModel
    {
        public DateTime Timestamp { get; set; }
        public string Date => Timestamp.ToString("dd/MM/yyyy");
        public string Time => Timestamp.ToString("HH:mm");
        public string Status { get; set; }
        public string DataCollector { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string HealthRisk { get; set; }
        public string MalesUnder5 { get; set; }
        public string Males5OrOlder { get; set; }
        public string FemalesUnder5 { get; set; }
        public string Females5OrOlder { get; set; }
        public string TotalUnder5 { get; set; }
        public string Total5OrOlder { get; set; }
        public string TotalFemales { get; set; }
        public string TotalMales { get; set; }
        public string Total { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public string IsoYear { get; set; }
        public string IsoWeek { get; set; }
        public string IsoYearIsoWeek { get; set; }
    }

    public static class ReportModelExtensions
    {
        public static ReportViewModel ToViewModel(this Nyss.Data.Models.Report report)
        {
            if (report == null) return null;



            var viewModel = new  ReportViewModel
            {
                DataCollector = report.DataCollector?.DisplayName,
                Village = report.DataCollector?.Village?.Name,
                Region = report.DataCollector?.Village?.District?.Region?.Name,
                District = report.DataCollector?.Village?.District?.Name,
                Status = report.Status.ToString(),
                HealthRisk = report.ProjectHealthRisk?.HealthRisk?.Name,
                //Location = $"{report.Location.Coordinate.X:00.0000}/{report.Location.Coordinate.Y:00.0000}",
                Location = null,
                Errors = null,
                Females5OrOlder = report.ReportedCase?.CountFemalesAtLeastFive.ToString(),
                FemalesUnder5 = report.ReportedCase?.CountFemalesBelowFive.ToString(),
                Males5OrOlder = report.ReportedCase?.CountMalesAtLeastFive.ToString(),
                MalesUnder5 = report.ReportedCase?.CountMalesBelowFive.ToString(),
                Message = report.RawContent,
                Total = (report.ReportedCase?.CountFemalesAtLeastFive 
                         + report.ReportedCase?.CountFemalesBelowFive
                         + report.ReportedCase?.CountMalesAtLeastFive
                         + report.ReportedCase?.CountMalesBelowFive).ToString(),
                Total5OrOlder = (report.ReportedCase?.CountFemalesAtLeastFive 
                                 + report.ReportedCase?.CountMalesAtLeastFive).ToString(),
                TotalUnder5 = (report.ReportedCase?.CountFemalesBelowFive
                               + report.ReportedCase?.CountMalesBelowFive).ToString(),
                TotalFemales = (report.ReportedCase?.CountFemalesAtLeastFive 
                                + report.ReportedCase?.CountFemalesBelowFive).ToString(),
                TotalMales = (report.ReportedCase?.CountMalesAtLeastFive
                              + report.ReportedCase?.CountMalesBelowFive).ToString(),
                Timestamp = report.CreatedAt,
                IsoWeek = report.CreatedAt.GetIsoWeek().ToString(),
                IsoYear = $"{report.CreatedAt:yyyy}",
                IsoYearIsoWeek = $"{report.CreatedAt:yyyy}-{report.CreatedAt.GetIsoWeek()}"
            };

            return viewModel;
        }
    }
}
