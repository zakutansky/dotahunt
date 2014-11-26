using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushiServices.Context.Data
{
    public class ProgamerWithKey : Progamer
    {
        public string ProviderKey { get; set; }

        public string NickName { get; set; }
    }
}
