using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.CacheKeys.ShareServiceKey
{
   public static class ShareServiceCacheKeys
    {
        public static string GetShareServiceLocationKey(string serviceKey) => $"ShareService-{serviceKey}";
    }
}
