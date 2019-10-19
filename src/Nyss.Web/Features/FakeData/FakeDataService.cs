using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using Nyss.Data.Concepts;
using Nyss.Data.Models;
using Nyss.Web.Features.DataCollectors.Data;
using Nyss.Web.Features.HealthRisks;
using Nyss.Web.Features.Locations.Data;
using Nyss.Web.Features.Reports.Data;
using RandomNameGeneratorLibrary;

namespace Nyss.Web.Features.FakeData
{
    public class FakeDataService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IDataCollectorRepository _dataCollectorRepository;
        private readonly IReportRepository _reportRepository;


        public static Point BottomLeftBoundary { get; set; }
        public static Point TopRightBoundary { get; set; }
        public static int InvalidReportPercentage { get; set; } = 5;
        public static int MaxAggregateCases { get; set; } = 100;

        private static readonly ContentLanguage English = new ContentLanguage { Id = 1, DisplayName = "English", LanguageCode = "EN" };

        private static readonly NationalSociety NationalSociety = new NationalSociety
        {
            Id = 1,
            ContentLanguage = English,
            Name = "CR Mandawi",
        };
        private static readonly Project Project = new Project
        {
            Id = 1,
            Name = "Mandawi project",
            ContentLanguage = English,
            NationalSociety = NationalSociety,
        };
        private static readonly SupervisorUser[] SupervisorUsers =
        {
            new SupervisorUser{Id = 1, Name = "John Doe"},
            new SupervisorUser{ Id = 2, Name = "Jane Dae"}
        };
        private readonly ProjectHealthRisk[] _projectHealthRisks;

        public async Task EnsureFakeDataAsync()
        {
            if ((await _locationRepository.GetRegionsAsync()).Any()) return;

            Random r = new Random();

            GenerateLocations();
            await GenerateDataCollectorsAsync(20, r);
            await GenerateReportsAsync(200, DateTime.Now.AddMonths(-1), DateTime.Now, r);
        }

        public FakeDataService(
            ILocationRepository locationRepository,
            IDataCollectorRepository dataCollectorRepository,
            IReportRepository reportRepository,
            IHealthRisksService healthRisksService)
        {
            _locationRepository = locationRepository;
            _dataCollectorRepository = dataCollectorRepository;
            _reportRepository = reportRepository;

            var healthRisks = healthRisksService.All().Take(3).Select(hr =>
            {
                if (!int.TryParse(hr.Code, out int id)) id = 0;
                return new HealthRisk
                {
                    Id = id,
                    HealthRiskCode = id,
                    Name = hr.DisplayName,
                    HealthRiskType = HealthRiskType.Human
                };
            }).ToArray();

            _projectHealthRisks = healthRisks.Select(hr => new ProjectHealthRisk
            {
                Id = hr.Id,
                HealthRisk = hr,
                Project = Project
            }).ToArray();
        }

        public async Task<IEnumerable<DataCollector>> GenerateDataCollectorsAsync(int count, Random r)
        {
            r = r ?? new Random();
            List<DataCollector> collectors = new List<DataCollector>(count);
            for (int i = 0; i < count; i++)
            {
                collectors.Add(await GenerateDataCollectorAsync(r));
            }

            return collectors;
        }

        public async Task<IEnumerable<Report>> GenerateReportsAsync(int count, DateTime from, DateTime to, Random r)
        {
            r = r ?? new Random();
            List<Report> reports = new List<Report>(count);
            for (int i = 0; i < count; i++)
            {
                reports.Add(await GenerateReportAsync(r, from, to));
            }

            return reports;
        }

        private async Task<DataCollector> GenerateDataCollectorAsync(Random r)
        {
            var name = r.GenerateRandomFirstAndLastName();
            var collector = new DataCollector
            {
                Name = name,
                DisplayName = name,
                DataCollectorType = DataCollectorType.Human,
                PhoneNumber = GeneratePhoneNumber(r),
                Location = GeneratePoint(r),
                Supervisor = GetSupervisor(r),
                Project = Project,
                Village = GetVillage(r),
                AdditionalPhoneNumber = null,
                Zone = null
            };

            await _dataCollectorRepository.InsertAsync(collector);
            await _dataCollectorRepository.SaveChangesAsync();

            return collector;
        }

        private async Task<Report> GenerateReportAsync(Random r, DateTime from, DateTime to)
        {
            var isValid = r.Next(100) < InvalidReportPercentage;
            var dataCollector = GetDataCollector(r);
            var projectHealthRisk = GetProjectHealthRisk(r);
            var reportType = GetReportType(r);
            var reportCase = isValid ? GenerateValidReportCase(r, reportType) : null;
            var dateTime = GetDateTime(r, from, to);

            var report = new Report
            {
                CreatedAt = dateTime,
                DataCollector = dataCollector,
                IsTraining = true,
                IsValid = isValid,
                ReportedCase = reportCase,
                KeptCase = reportCase,
                Location = dataCollector.Location,
                RawContent = isValid
                    ? GetMessageFromReport(reportCase, projectHealthRisk.HealthRisk.HealthRiskCode.ToString())
                    : GenerateInvalidMessage(r),
                ProjectHealthRisk = projectHealthRisk,
                ReceivedAt = dateTime,
                ReportType = reportType,
                Status = isValid ? ReportStatus.Accepted : ReportStatus.Rejected,
            };

            await _reportRepository.InsertAsync(report);
            await _reportRepository.SaveChangesAsync();
            return report;
        }

        private DataCollector GetDataCollector(Random r)
        {
            var all = _dataCollectorRepository.GetAllAsync().Result.ToList();

            return all[r.Next(all.Count)];
        }

        private DateTime GetDateTime(Random r, DateTime from, DateTime to)
        {
            if (from > to)
            {
                var temp = to;
                to = from;
                from = temp;
            }

            var maxTicks = (int)to.Ticks - (int)from.Ticks;

            return from.AddTicks(RandomLong(r, maxTicks));
        }

        private static long RandomLong(Random r, long maxValue)
        {
            var d = r.NextDouble() * maxValue;

            return (long)d;
        }

        private ReportType GetReportType(Random r)
        {
            return r.Next(2) == 0
                   ? ReportType.Single
                   : ReportType.Aggregate;
        }

        private string GenerateInvalidMessage(Random r)
        {
            var digits = new int[4];
            for (int i = 0; i < digits.Length; i++)
            {
                digits[i] = r.Next(10);
            }
            return string.Join("#", digits);
        }

        private ProjectHealthRisk GetProjectHealthRisk(Random r)
        {
            return _projectHealthRisks[r.Next(_projectHealthRisks.Length)];
        }

        private string GetMessageFromReport(ReportCase reportCase, string healthRiskCode)
        {
            if (reportCase.CountFemalesAtLeastFive +
                reportCase.CountFemalesBelowFive +
                reportCase.CountMalesAtLeastFive +
                reportCase.CountMalesBelowFive
                > 1)
            {
                // Aggregate message
                return string.Format("{0}#{1}#{2}#{3}#{4}",
                    healthRiskCode,
                    reportCase.CountMalesBelowFive,
                    reportCase.CountMalesAtLeastFive,
                    reportCase.CountFemalesBelowFive,
                    reportCase.CountFemalesAtLeastFive);
            }
            else
            {
                var sex = reportCase.CountMalesAtLeastFive + reportCase.CountMalesBelowFive >= 1 ? 1 : 2;
                var age = reportCase.CountMalesBelowFive + reportCase.CountFemalesBelowFive >= 1 ? 1 : 2;
                return $"{healthRiskCode}#{sex}#{age}";
            }
        }

        private ReportCase GenerateValidReportCase(Random r, ReportType type)
        {
            return type == ReportType.Aggregate
                ? GenerateAggregateReportCase(r)
                : GenerateSingleReportCase(r);
        }

        private ReportCase GenerateSingleReportCase(Random r)
        {
            var type = r.Next(4);

            return new ReportCase
            {
                CountFemalesAtLeastFive = type == 0 ? 1 : 0,
                CountFemalesBelowFive = type == 1 ? 1 : 0,
                CountMalesAtLeastFive = type == 2 ? 1 : 0,
                CountMalesBelowFive = type == 3 ? 1 : 0,
            };
        }

        private ReportCase GenerateAggregateReportCase(Random r)
        {
            return new ReportCase
            {
                CountFemalesAtLeastFive = r.Next(MaxAggregateCases),
                CountFemalesBelowFive = r.Next(MaxAggregateCases),
                CountMalesAtLeastFive = r.Next(MaxAggregateCases),
                CountMalesBelowFive = r.Next(MaxAggregateCases),
            };
        }

        private Village GetVillage(Random r)
        {
            var all = _locationRepository.GetVillagesAsync().Result.ToList();

            return all[r.Next(all.Count)];
        }

        private SupervisorUser GetSupervisor(Random r)
        {
            return SupervisorUsers[r.Next(SupervisorUsers.Length)];
        }

        private string GeneratePhoneNumber(Random r, string prefix = null)
        {
            string number = prefix;
            if (prefix == null)
            {
                number = $"{r.Next(10)}{r.Next(10)}";
            }

            for (int i = 0; i < 10; i++)
            {
                number += r.Next(10).ToString();
            }

            return number;
        }

        private Point GeneratePoint(Random r)
        {
            double minX, minY, maxX, maxY;
            if (BottomLeftBoundary == null)
            {
                minX = -180;
                minY = -90;
            }
            else
            {
                minX = BottomLeftBoundary.X;
                minY = BottomLeftBoundary.Y;
            }
            if (TopRightBoundary == null)
            {
                maxX = 180;
                maxY = 90;
            }
            else
            {
                maxX = TopRightBoundary.X;
                maxY = TopRightBoundary.Y;
            }

            var longitude = r.NextDouble() * (maxX - minX) + minX;
            var latitude = r.NextDouble() * (maxY - minY) + minY;

            return new Point(longitude, latitude);
        }

        private IEnumerable<Village> GenerateLocations()
        {
            var region = new Region { Name = "Mbendi" };
            GenerateVillageAndSave(new District { Name = "Malden", Region = region });
            GenerateVillageAndSave(new District { Name = "Panau", Region = region });
            GenerateVillageAndSave(new District { Name = "Plama", Region = region });
            GenerateVillageAndSave(new District { Name = "Baldova", Region = region });
            GenerateVillageAndSave(new District { Name = "Farfelu", Region = region });

            _locationRepository.InsertRegionById(region);

            region = new Region { Name = "Lasondra" };
            GenerateVillageAndSave(new District { Name = "Guianbilan", Region = region });
            GenerateVillageAndSave(new District { Name = "Roller", Region = region });
            GenerateVillageAndSave(new District { Name = "Kurio", Region = region });
            GenerateVillageAndSave(new District { Name = "Nouuga", Region = region });
            GenerateVillageAndSave(new District { Name = "Chapito", Region = region });
            GenerateVillageAndSave(new District { Name = "Tayti", Region = region });
            GenerateVillageAndSave(new District { Name = "Pill", Region = region });
            GenerateVillageAndSave(new District { Name = "Gringen", Region = region });
            GenerateVillageAndSave(new District { Name = "Dernyka", Region = region });
            GenerateVillageAndSave(new District { Name = "Gana", Region = region });
            GenerateVillageAndSave(new District { Name = "Sopahonta", Region = region });
            GenerateVillageAndSave(new District { Name = "Guadec", Region = region });
            GenerateVillageAndSave(new District { Name = "Sabakalan", Region = region });
            GenerateVillageAndSave(new District { Name = "Lacroa", Region = region });
            GenerateVillageAndSave(new District { Name = "Bogay", Region = region });


            _locationRepository.InsertRegionById(region);
            region = new Region { Name = "Coronia" };

            GenerateVillageAndSave(new District { Name = "Yasi", Region = region });
            GenerateVillageAndSave(new District { Name = "Hanplida", Region = region });
            GenerateVillageAndSave(new District { Name = "Goub", Region = region });
            GenerateVillageAndSave(new District { Name = "Yopao", Region = region });
            GenerateVillageAndSave(new District { Name = "Imara", Region = region });
            GenerateVillageAndSave(new District { Name = "Nayak", Region = region });
            GenerateVillageAndSave(new District { Name = "Cialia", Region = region });
            GenerateVillageAndSave(new District { Name = "Messa", Region = region });
            GenerateVillageAndSave(new District { Name = "Santalu", Region = region });
            GenerateVillageAndSave(new District { Name = "Tribia", Region = region });



            _locationRepository.InsertRegionById(region);
            region = new Region { Name = "Layuna" };
            GenerateVillageAndSave(new District { Name = "Rohen", Region = region });
            GenerateVillageAndSave(new District { Name = "Sopigu", Region = region });
            GenerateVillageAndSave(new District { Name = "Gunsa", Region = region });
            GenerateVillageAndSave(new District { Name = "Nitaba", Region = region });
            GenerateVillageAndSave(new District { Name = "Realia", Region = region });
            GenerateVillageAndSave(new District { Name = "Yanpula", Region = region });
            GenerateVillageAndSave(new District { Name = "Tubigsopa", Region = region });
            GenerateVillageAndSave(new District { Name = "Caboza", Region = region });
            GenerateVillageAndSave(new District { Name = "Povia", Region = region });
            GenerateVillageAndSave(new District { Name = "Topigo", Region = region });
            GenerateVillageAndSave(new District { Name = "Santa Rita", Region = region });



            _locationRepository.InsertRegionById(region);
            region = new Region { Name = "Bahahl" };
            GenerateVillageAndSave(new District { Name = "Ploucland", Region = region });
            GenerateVillageAndSave(new District { Name = "Taronia", Region = region });
            GenerateVillageAndSave(new District { Name = "Kalewool", Region = region });
            GenerateVillageAndSave(new District { Name = "Radistan", Region = region });
            GenerateVillageAndSave(new District { Name = "Mifan", Region = region });
            GenerateVillageAndSave(new District { Name = "Wadata", Region = region });
            GenerateVillageAndSave(new District { Name = "Haygam", Region = region });
            GenerateVillageAndSave(new District { Name = "Carouge", Region = region });
            GenerateVillageAndSave(new District { Name = "Join", Region = region });
            GenerateVillageAndSave(new District { Name = "Aimesalang", Region = region });
            GenerateVillageAndSave(new District { Name = "Notsimon", Region = region });
            GenerateVillageAndSave(new District { Name = "Titof", Region = region });
            GenerateVillageAndSave(new District { Name = "Buligtan", Region = region });
            GenerateVillageAndSave(new District { Name = "Slaka", Region = region });
            GenerateVillageAndSave(new District { Name = "Kurkama", Region = region });
            GenerateVillageAndSave(new District { Name = "Banglafi", Region = region });
            GenerateVillageAndSave(new District { Name = "Glupi", Region = region });
            GenerateVillageAndSave(new District { Name = "Taotat", Region = region });



            _locationRepository.InsertRegionById(region);
            region = new Region { Name = "Moroni" };
            GenerateVillageAndSave(new District { Name = "Dipalong", Region = region });
            GenerateVillageAndSave(new District { Name = "Alain", Region = region });
            GenerateVillageAndSave(new District { Name = "Toros", Region = region });
            GenerateVillageAndSave(new District { Name = "Provida", Region = region });
            GenerateVillageAndSave(new District { Name = "Marwi", Region = region });
            GenerateVillageAndSave(new District { Name = "Ankapol", Region = region });
            GenerateVillageAndSave(new District { Name = "Pornic", Region = region });
            GenerateVillageAndSave(new District { Name = "Silenciosa", Region = region });
            GenerateVillageAndSave(new District { Name = "Madiba", Region = region });
            GenerateVillageAndSave(new District { Name = "Sapin", Region = region });
            GenerateVillageAndSave(new District { Name = "Simbad", Region = region });
            GenerateVillageAndSave(new District { Name = "Rabito", Region = region });
            GenerateVillageAndSave(new District { Name = "Madiba", Region = region });



            _locationRepository.InsertRegionById(region);
            region = new Region { Name = "Bamkao" };
            GenerateVillageAndSave(new District { Name = "Padang", Region = region });
            GenerateVillageAndSave(new District { Name = "Bahkan", Region = region });
            GenerateVillageAndSave(new District { Name = "Ridapel", Region = region });
            GenerateVillageAndSave(new District { Name = "Pela", Region = region });
            GenerateVillageAndSave(new District { Name = "Jennifo", Region = region });
            GenerateVillageAndSave(new District { Name = "Edna", Region = region });
            GenerateVillageAndSave(new District { Name = "Orchia", Region = region });
            GenerateVillageAndSave(new District { Name = "Keep", Region = region });
            GenerateVillageAndSave(new District { Name = "Prondisa", Region = region });
            GenerateVillageAndSave(new District { Name = "Jose", Region = region });
            GenerateVillageAndSave(new District { Name = "Okko", Region = region });
            GenerateVillageAndSave(new District { Name = "Ladetu", Region = region });
            GenerateVillageAndSave(new District { Name = "Loreal", Region = region });
            GenerateVillageAndSave(new District { Name = "San Miguel", Region = region });
            GenerateVillageAndSave(new District { Name = "Maioza", Region = region });
            GenerateVillageAndSave(new District { Name = "Giboa", Region = region });
            GenerateVillageAndSave(new District { Name = "Mariba", Region = region });

            return new List<Village>();
        }

        private void GenerateVillageAndSave(District district)
        {
            var r = new Random();
            for (var i = 0; i < 5; i++)
            {
                _locationRepository.InsertVillageById(new Village { Name = r.GenerateRandomFirstAndLastName(), District = district });
            }
            _locationRepository.InsertDistrictById(district);
        }

    }
}
