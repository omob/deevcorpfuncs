using System.Text.RegularExpressions;
using System;
using System.IO;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace deevcorpfuncs
{
    public static class SMSLicenseCode
    {
        [FunctionName("SMSLicenseCode")]
        public static void Run([
            BlobTrigger("licenses/{name}", Connection = "AzureWebJobsStorage")]string licenseFileContents,
            string name,
            ILogger log)
        {

            log.LogInformation("Called SmsLicense function");

            var index = licenseFileContents.IndexOf("SecretCode");
            var licenseCode = licenseFileContents.Substring(index);

            var message = $"The Code for your Order is\n {licenseCode}";
            var recipient = Environment.GetEnvironmentVariable("Recipient");

            // Send SMS to recipient
            SendSMS(message, recipient, true);
        }

        public static void SendSMS(string text, string recipient, bool isSending = false)
        {
            if (isSending)
            {
                InitializeTwilio();
                MessageResource.Create(
                    body: text,
                    from: new Twilio.Types.PhoneNumber(Environment.GetEnvironmentVariable("TwilioNumber")),
                    to: new Twilio.Types.PhoneNumber(recipient)
                );
            }
        }

        public static void InitializeTwilio()
        {
            var twilioAccountSid = Environment.GetEnvironmentVariable("TwilioAccountSid");
            var twilioAuthToken = Environment.GetEnvironmentVariable("TwilioAuthToken");

            TwilioClient.Init(twilioAccountSid, twilioAuthToken);
        }
    }
}
