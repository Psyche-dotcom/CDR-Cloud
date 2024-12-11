using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class HelpDetailDto
    {
        public string Element { get; set; }
        public string LocalizationKey { get; set; }
        public string JsText { get; set; }
        public byte Type { get; set; }
        public string ReturnJsText
        {
            get
            {
                return "{'click " + this.Element + "' : '_-" + this.LocalizationKey + "-_',_-NextButton-_'shape': 'circle', onBeforeStart: function() {" + this.JsText + "}},";
            }
        }
    }
}
