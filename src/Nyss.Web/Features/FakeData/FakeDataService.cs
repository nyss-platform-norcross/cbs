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

        public async Task< IEnumerable<DataCollector>> GenerateDataCollectorsAsync(int count)
        {
            var r = new Random();
            List<DataCollector> collectors = new List<DataCollector>(count);
            for (int i = 0; i < count; i++)
            {
                collectors.Add(await GenerateDataCollectorAsync(r));
            }

            return collectors;
        }

        public async Task< IEnumerable<Report>> GenerateReportsAsync(int count, DateTime from, DateTime to)
        {
            var r = new Random();
            List<Report> reports = new List<Report>(count);
            for (int i = 0; i < count; i++)
            {
                reports.Add(await GenerateReportAsync(r, from, to));
            }

            return reports;
        }

        private async Task<DataCollector > GenerateDataCollectorAsync(Random r)
        {
            var name = r.GenerateRandomFirstAndLastName();
            var collector =  new DataCollector
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

        private async Task<Report>  GenerateReportAsync(Random r, DateTime from, DateTime to)
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
            return new Village { Name = r.GenerateRandomFirstAndLastName() };
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

//        private IEnumerable<Village> GenerateLocations()
//        {
//            /*
//             *
//Mbendi:
//- Malden
//- Panau
//- Plama
//- Baldova
//- Farfelu
//Lasondra:
//- Guianbilan
//- Roller
//- Kurio
//- Nouuga
//- Chapito
//- Tayti
//- Pill
//- Gringen
//- Dernyka
//- Gana
//- Sopahonta
//- Guadec
//- Sabakalan
//- Lacroa
//- Bogay
//Coronia:
//- Yasi
//- Hanplida
//- Goub
//- Yopao
//- Imara
//- Nayak
//- Cialia
//- Messa
//- Santalu
//- Tribia
//Layuna:
//- Rohen
//- Sopigu
//- Gunsa
//- Nitaba
//- Realia
//- Yanpula
//- Tubigsopa
//- Caboza
//- Povia
//- Topigo
//- Santa Rita
//Bahahl:
//- Ploucland
//- Taronia
//- Kalewool
//- Radistan
//- Mifan
//- Wadata
//- Haygam
//- Carouge
//- Join
//- Aimesalang
//- Notsimon
//- Titof
//- Buligtan
//- Slaka
//- Kurkama
//- Banglafi
//- Glupi
//- Taotat
//Moroni:
//- Dipalong
//- Alain
//- Toros
//- Provida
//- Marwi
//- Ankapol
//- Pornic
//- Silenciosa
//- Madiba
//- Sapin
//- Simbad
//- Rabito
//- Madiba
//Bamkao:
//- Padang
//- Bahkan
//- Ridapel
//- Pela
//- Jennifo
//- Edna
//- Orchia
//- Keep
//- Prondisa
//- Jose
//- Okko
//- Ladetu
//- Loreal
//- San Miguel
//- Maioza
//- Giboa
//- Mariba
//             *
//             */
//        }
    }
}
