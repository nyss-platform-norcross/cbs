using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nyss.Web.Features.SmsGateway.Logic.Models;

namespace Nyss.Web.Features.SmsGateway.Logic
{
    public class InFileSmsGatewayService : ISmsGatewayService
    {
        private const string PathToOurSuperFileDatabase = @"C:\temp\Nyss-sms.txt";
        private const string OurSuperSecretApiKey = "oursupersecretapikey";

        private readonly ISmsParser _smsParser;

        public InFileSmsGatewayService(ISmsParser smsParser)
        {
            _smsParser = smsParser;
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
                using (var file = new StreamWriter(PathToOurSuperFileDatabase, true))
                {
                    var json = JsonConvert.SerializeObject(sms);
                    await file.WriteLineAsync(json);
                }
                
                var parsedCase = _smsParser.ParseSmsToCase(sms.Text);

                if (parsedCase.IsValid)
                {
                    result.FeedbackMessage = "Yay ! Thank you bro";
                }
                else
                {
                    result.FeedbackMessage = "Sorry bro, I do not understand.";
                }
            }
            
            return await Task.FromResult(result);
        }
    }
}
