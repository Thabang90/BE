using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Models
{
    public class EmailModel
    {
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailModel(string receiver, string subject, string content)
        {
            Receiver = receiver;
            Subject = subject;
            Content = content;
        }

    }
}
