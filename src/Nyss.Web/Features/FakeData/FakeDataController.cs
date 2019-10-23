using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nyss.Web.Features.DataCollectors.Data;
using Nyss.Web.Features.Reports;
using Nyss.Web.Features.Reports.Data;
using Nyss.Web.Utils;
using System.Linq;
using Nyss.Web.Features.DataCollectors;

namespace Nyss.Web.Features.FakeData
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataController : BaseController
    {
        private readonly IFakeDataService _fakeDataService;
        private readonly IReportRepository _reportRepository;
        private readonly IDataCollectorRepository _dataCollectorRepository;

        public FakeDataController(
            IFakeDataService fakeDataService,
            IReportRepository reportRepository,
            IDataCollectorRepository dataCollectorRepository)
        {
            _fakeDataService = fakeDataService;
            _reportRepository = reportRepository;
            _dataCollectorRepository = dataCollectorRepository;
        }

        [HttpPost("reports/generate")]
        [ProducesResponseType(typeof(IEnumerable<ReportViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateReportsAsync([FromBody] FakeDataGenerationOptions options)
        {
            if (options == null) options = new FakeDataGenerationOptions();

            var count = options.Count ?? 10;
            var from = options.FromDate ?? _reportRepository.All().Max(_ => _.CreatedAt);
            var to = options.ToDate ?? DateTime.Now;

            var r = options.RandomizationSeed.HasValue
                ? new Random(options.RandomizationSeed.Value)
                : new Random();

            var generated = await _fakeDataService.GenerateReportsAsync(count, from, to, r);
            var results = generated.Select(_ => _.ToViewModel());

            return Ok(results);
        }

        [HttpPost("datacollectors/generate")]
        [ProducesResponseType(typeof(IEnumerable<DataCollectorViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateDataCollectorsAsync([FromBody] FakeDataGenerationOptions options)
        {
            if (options == null) options = new FakeDataGenerationOptions();

            var count = options.Count ?? 10;

            var r = options.RandomizationSeed.HasValue
                ? new Random(options.RandomizationSeed.Value)
                : new Random();

            var generated = await _fakeDataService.GenerateDataCollectorsAsync(count, r);
            var results = generated.Select(_ => _.ToViewModel());

            return Ok(results);
        }
    }
}
