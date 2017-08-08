using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models
{
    public class Translations
    {
        public List<Translation> translations { get; set; }
    }

    public class Translation
    {
        public string languagecode { get; set; }
        public string sourcetext { get; set; }
        public string translatedtext { get; set; }
    }
}
