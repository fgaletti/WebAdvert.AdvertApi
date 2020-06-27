using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertApi.Models.Messages
{

    // 30 message to AWS 
    public class AdvertConfirmedMessage
    {
        public string  Id { get; set; }
        public string Title { get; set; }
    }
}
