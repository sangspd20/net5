using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Logging
{
    public static class F88LogManage
    {
        public static readonly ILog F88PartnerLog = LogManager.GetLogger("F88PartnerLog");
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }
    }
}
