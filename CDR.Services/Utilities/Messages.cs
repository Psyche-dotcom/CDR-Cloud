using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Utilities
{
    public static class Messages
    {
        public static class Queries
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Query bulunamadı.";
                return "Böyle bir query bulunamadı.";
            }
        }

        public static class Localizations
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Çeviri bulunamadı.";
                return "Böyle bir çeviri bulunamadı.";
            }
        }

        public static class Content
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "İçerik bulunamadı.";
                return "Böyle bir içerik bulunamadı.";
            }
        }
    }
}
