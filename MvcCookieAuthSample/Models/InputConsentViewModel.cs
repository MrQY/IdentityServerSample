using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.Models
{
    public class InputConsentViewModel
    {
        public bool RememberConsent { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public string ReturnUrl { get; set; }
        public string Button { get; set; }
    }
}
