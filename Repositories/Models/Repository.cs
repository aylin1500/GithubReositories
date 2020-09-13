using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Repositories.Models
{
    public class Repository
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Image")]
        public string ImageSrc { get; set; }

        public string HtmlUrl { get; set; }
    }
}