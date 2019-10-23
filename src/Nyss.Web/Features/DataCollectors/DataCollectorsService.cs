using System.Collections.Generic;
using System.Linq;
using Nyss.Web.Features.DataCollectors.Data;

namespace Nyss.Web.Features.DataCollectors
{
    public class DataCollectorsService : IDataCollectorsService
    {
        private readonly IDataCollectorRepository _dataCollectorRepository;

        public DataCollectorsService(
            IDataCollectorRepository dataCollectorRepository)
        {
            _dataCollectorRepository = dataCollectorRepository;
        }

        public IEnumerable<DataCollectorViewModel> All()
        {
            return _dataCollectorRepository.GetAllAsync().Result.Select(_ => _.ToViewModel());

            //var dataCollectors = new List<DataCollectorViewModel>();
            //for (var i = 0; i < 100; ++i)
            //{
            //   dataCollectors.Add(GenerateRandomDataCollector());
            //}
            //return dataCollectors;
        }

        public DataCollectorViewModel ByPhoneNumber(string phoneNumber)
        {
            return _dataCollectorRepository.GetDataCollectorByPhoneNumberAsync(phoneNumber).Result.ToViewModel();

            //return GenerateRandomDataCollector();
        }
    }
}