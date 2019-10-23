using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nyss.Web.Features.FakeData
{
    public class FakeDataGenerationOptions
    {
        public int? Count { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? RandomizationSeed { get; set; }
    }
}
