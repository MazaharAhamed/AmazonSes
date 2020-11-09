using System;
using Amazon;
using System.Collections.Generic;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Diagnostics;

namespace AmazonSes
{
    class Program
    {
        
        static readonly string senderAddress = "testemailinn@gmail.com";

        static public List<string> emList { get; set; }


        // The configuration set to use for this email. If you do not want to use a
        // configuration set, comment out the following property and the
        // ConfigurationSetName = configSet argument below. 
        //static readonly string configSet = "ConfigSet";

        static readonly string subject = "Amazon SES test (AWS SDK for .NET)";

        static readonly string textBody = "Amazon SES Test (.NET)\r\n"
                                        + "This email was sent through Amazon SES "
                                        + "using the AWS SDK for .NET.";

        static readonly string htmlBody = @"<html>
<head></head>
<body>
  <h1>Amazon SES Test (AWS SDK for .NET)</h1>
  <p>This email was sent with
    <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
    <a href='https://aws.amazon.com/sdk-for-net/'>
      AWS SDK for .NET</a>.</p>
</body>
</html>";

        static void Main(string[] args)
        {
            var watch = new Stopwatch();

            watch.Start();
            emList = new List<string>();

            for (int i = 1; i <= 2; i++)
            {
                //emList.Add($"test{i}+mytestemail@gmail.com");
                emList.Add($"testemailinn{i}@gmail.com");
            }

            foreach (var item in emList)
            {
                using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USWest2))
                {
                    var sendRequest = new SendEmailRequest
                    {
                        Source = senderAddress,
                        Destination = new Destination
                        {
                            ToAddresses =
                            new List<string> { item }
                        },
                        Message = new Message
                        {
                            Subject = new Content(subject),
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = htmlBody
                                },
                                Text = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = textBody
                                }
                            }
                        },
                        // If you are not using a configuration set, comment
                        // or remove the following line 
                        //ConfigurationSetName = configSet
                    };
                    try
                    {
                        Console.WriteLine("Sending email using Amazon SES...");
                        var response = client.SendEmail(sendRequest);
                        Console.WriteLine("The email was sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The email was not sent.");
                        Console.WriteLine("Error message: " + ex.Message);

                    }
                }
            }
            watch.Stop();
            Console.WriteLine($"It took {watch.ElapsedMilliseconds} seconds to send 100 mails");

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}