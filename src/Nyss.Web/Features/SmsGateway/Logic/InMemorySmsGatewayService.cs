using System;
using System.Threading.Tasks;
using Nyss.Data.Concepts;
using Nyss.Data.Models;
using Nyss.Web.Features.Reports.Data;
using Nyss.Web.Features.SmsGateway.Logic.Models;

namespace Nyss.Web.Features.SmsGateway.Logic
{
    public class InMemorySmsGatewayService : ISmsGatewayService
    {
        private const string OurSuperSecretApiKey = "oursupersecretapikey";

        private readonly ISmsParser _smsParser;
        private readonly IReportRepository _reportRepository;

        public InMemorySmsGatewayService(ISmsParser smsParser,
            IReportRepository reportRepository)
        {
            _smsParser = smsParser;
            _reportRepository = reportRepository;
        }
        
        public async Task<SmsProcessResult> SaveReportAsync(Sms sms)
        {
            if (sms == null) throw new ArgumentNullException(nameof(sms), "Sms informations are required.");

            var result = sms.Validate();
            result.PhoneNumber = sms.Sender;

            if (sms.ApiKey != OurSuperSecretApiKey)
                result.IsAuthorized = false;

            if (result.IsRequestValid)
            {   
                var parsedCase = _smsParser.ParseSmsToCase(sms.Text);

                if (parsedCase.IsValid)
                {
                    result.FeedbackMessage = "Yay ! Thank you bro";
                }
                else
                {
                    result.FeedbackMessage = "Sorry bro, I do not understand.";
                }

                var report = new Data.Models.Report
                {
                    CreatedAt = sms.ReceivedAt,
                    ReportedCase = new ReportCase
                    {
                        CountFemalesAtLeastFive = parsedCase.CountFemalesAtLeastFive,
                        CountFemalesBelowFive = parsedCase.CountFemalesBelowFive,
                        CountMalesAtLeastFive = parsedCase.CountMalesAtLeastFive,
                        CountMalesBelowFive = parsedCase.CountFemalesBelowFive
                    },
                    IsTraining = false,
                    IsValid = parsedCase.IsValid,
                    RawContent = sms.Text,
                    Location = sms.Location,
                    Status = ReportStatus.Pending,
                    DataCollector = null,
                    ProjectHealthRisk = null,
                    ReportType = parsedCase.IsSingle ? ReportType.Single : ReportType.Aggregate
                };

                await _reportRepository.InsertAsync(report);
                await _reportRepository.SaveChangesAsync();
            }
            
            return result;
        }
    }
}
