using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nyss.Web.Features.Report.Data
{
    public class InMemoryReportRepository : IReportRepository
    {
        private static List<Nyss.Data.Models.Report> _reports;

        public InMemoryReportRepository()
        {
            _reports = new List<Nyss.Data.Models.Report>();
        }

        public IEnumerable<Nyss.Data.Models.Report> All()
        {
            return _reports;
        }

        public Task<Nyss.Data.Models.Report> InsertAsync(Nyss.Data.Models.Report report)
        {
            report.Id = _reports.Count + 1;
            _reports.Add(report);

            return Task.FromResult(report);
        }

        public Task<PaginationResult<Nyss.Data.Models.Report>> GetReportsAsync(PaginationOptions options)
        {
            var query =  _reports.AsQueryable();

            var totalCount = query.Count();

            switch (options.Order?.ToLower())
            {
                case "date":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.CreatedAt.Date)
                        : query.OrderByDescending(x => x.CreatedAt.Date);
                    break;

                case "time":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.CreatedAt.TimeOfDay)
                        : query.OrderByDescending(x => x.CreatedAt.TimeOfDay);
                    break;

                case "status":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.Status)
                        : query.OrderByDescending(x => x.Status);
                    break;

                case "datacollector":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.DataCollector)
                        : query.OrderByDescending(x => x.DataCollector);
                    break;

                case "region":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.DataCollector.Village.District.Region.Name)
                        : query.OrderByDescending(x => x.DataCollector.Village.District.Region.Name);
                    break;

                case "district":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.DataCollector.Village.District.Name)
                        : query.OrderByDescending(x => x.DataCollector.Village.District.Name);
                    break;

                case "village":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.DataCollector.Village.Name)
                        : query.OrderByDescending(x => x.DataCollector.Village.Name);
                    break;

                case "healthrisk":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ProjectHealthRisk.HealthRisk.Name)
                        : query.OrderByDescending(x => x.ProjectHealthRisk.HealthRisk.Name);
                    break;

                case "malesunder5":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountMalesBelowFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountMalesBelowFive);
                    break;

                case "males5orolder":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountMalesAtLeastFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountMalesAtLeastFive);
                    break;

                case "femalesunder5":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountFemalesBelowFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountFemalesBelowFive);
                    break;

                case "females5orolder":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountFemalesAtLeastFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountFemalesAtLeastFive);
                    break;

                case "totalunder5":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountMalesBelowFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountMalesBelowFive);
                    break;

                case "total5orolder":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountFemalesAtLeastFive + x.ReportedCase.CountMalesAtLeastFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountFemalesAtLeastFive + x.ReportedCase.CountMalesAtLeastFive);
                    break;

                case "totalfemales":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountFemalesAtLeastFive + x.ReportedCase.CountFemalesBelowFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountFemalesAtLeastFive + x.ReportedCase.CountFemalesBelowFive);
                    break;

                case "totalmales":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountMalesAtLeastFive + x.ReportedCase.CountMalesBelowFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountMalesAtLeastFive + x.ReportedCase.CountMalesBelowFive);
                    break;

                case "total":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.ReportedCase.CountMalesAtLeastFive + x.ReportedCase.CountMalesBelowFive
                            + x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountFemalesAtLeastFive)
                        : query.OrderByDescending(x => x.ReportedCase.CountMalesAtLeastFive + x.ReportedCase.CountMalesBelowFive
                            + x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountFemalesAtLeastFive);
                    break;

                case "location":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.Location)
                        : query.OrderByDescending(x => x.Location);
                    break;

                case "message":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.RawContent)
                        : query.OrderByDescending(x => x.RawContent);
                    break;

                //case "errors":
                //    query = options.OrderAsc
                //        ? query.OrderBy(x => x.);
                //        : query.OrderByDescending(x => x.)
                //    break;

                case "isoyear":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.CreatedAt);
                    break;

                case "isoweek":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.CreatedAt);
                    break;

                case "isoyearisoweek":
                    query = options.OrderAsc
                        ? query.OrderBy(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.CreatedAt);
                    break;
            }

            foreach (var searchKeyValuePair in options.SearchDictionary.Where(x => !string.IsNullOrEmpty(x.Value)))
            {
                var search = searchKeyValuePair.Key.ToLower();
                var value = searchKeyValuePair.Value;
                if (search == "date" && DateTime.TryParse(value, out var searchedDate))
                {
                    query = query.Where(x => x.CreatedAt.Date == searchedDate);
                }
                else if (search == "time"  && TimeSpan.TryParse(value, out var searchedTime))
                {
                    query = query.Where(x => x.CreatedAt.TimeOfDay ==  searchedTime);
                }
                else if (search == "status")
                {
                    query = query.Where(x => x.Status.ToString().Contains(value));
                }
                else if (search == "datacollector")
                {
                    query = query.Where(x => x.DataCollector.DisplayName.Contains(value));
                }
                else if (search == "region")
                {
                    query = query.Where(x => x.DataCollector.Village.District.Region.Name.Contains(value));
                }
                else if (search == "district")
                {
                    query = query.Where(x => x.DataCollector.Village.District.Name.Contains(value));
                }
                else if (search == "village")
                {
                    query = query.Where(x => x.DataCollector.Village.District.Region.Name.Contains(value));
                }
                else if (search == "healthrisk")
                {
                    query = query.Where(x => x.ProjectHealthRisk.HealthRisk.Name.Contains(value));
                }
                else if (search == "malesunder5" && int.TryParse(value, out var searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountMalesBelowFive == searchedCount);
                }
                else if (search == "males5orolder" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesAtLeastFive ==  searchedCount);
                }
                else if (search == "femalesunder5" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesBelowFive == searchedCount);
                }
                else if (search == "females5orolder" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesAtLeastFive == searchedCount);
                }
                else if (search == "totalunder5" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountMalesBelowFive == searchedCount);
                }
                else if (search == "total5orolder" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesAtLeastFive + x.ReportedCase.CountMalesAtLeastFive == searchedCount);
                }
                else if (search == "totalfemales" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountFemalesAtLeastFive == searchedCount);
                }
                else if (search == "totalmales" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountMalesAtLeastFive + x.ReportedCase.CountMalesBelowFive == searchedCount);
                }
                else if (search == "total" && int.TryParse(value, out searchedCount))
                {
                    query = query.Where(x => x.ReportedCase.CountFemalesBelowFive + x.ReportedCase.CountMalesBelowFive
                                             + x.ReportedCase.CountFemalesAtLeastFive + x.ReportedCase.CountMalesAtLeastFive== searchedCount);
                }
                //else if (search == "location")
                //{
                //    query = query.Where(x => (x.Location).Contains(value));
                //}
                else if (search == "message")
                {
                    query = query.Where(x => x.RawContent.Contains(value));
                }
                //else if (search == "errors")
                //{
                //    query = query.Where(x => (x.Errors).Contains(value));
                //}
                //else if (search == "isoyear")
                //{
                //    query = query.Where(x => (x.IsoYear).Contains(value));
                //}
                //else if (search == "isoweek")
                //{
                //    query = query.Where(x => (x.IsoWeek).Contains(value));
                //}
                //else if (search == "isoyearisoweek")
                //{
                //    query = query.Where(x => (x.IsoYearIsoWeek).Contains(value));
                //}
            }

            var filteredCount = query.Count();

            query = query.Skip(options.Start);

            if (options.Count != -1)
            {
                query = query.Take(options.Count);
            }

            var result = new PaginationResult<Nyss.Data.Models.Report>
            {
                TotalCount = totalCount,
                FilteredCount = filteredCount,
                Data = query.ToList()
            };

            return Task.FromResult(result);
        }

        public async Task SaveChangesAsync()
        {
            await Task.Delay(0);
        }
    }
}
