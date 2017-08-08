using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Models
{
    public class DTO<T>
    {
        public T response { get; set; }
        public string objname { get; set; }
        public Status status { get; set; }
    }
}
