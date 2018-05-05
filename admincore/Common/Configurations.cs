using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Common
{
    public class AmazonSettings
    {
        public string SliderBucketKeyId { get; set; }

        public string SliderBucketKey { get; set; }

        public string SliderBucketName { get; set; }

        public string AWSURL { get; set; }
    }

    public class SendgridConfigs
    {
        public string APIKey { get; set; }

        public string From { get; set; }
    }
}
