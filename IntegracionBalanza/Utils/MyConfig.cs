using System;
using System.Collections.Generic;
using System.Text;

namespace IntegracionBalanza
{
    public class MyConfig
    {
        public string urlbase { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string wsorbiteciahhrr { get; set; }
        public Int32 interval { get; set; }
        public Int32 interval2 { get; set; }
        public Int32 interval3 { get; set; }

        public Int32 NoTareas { get; set; }

    }


    public class connectionStrings
    {
        public string DefaultConnection { get; set; }

        public string AccessConnectionString { get; set; }
    }


}
