using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aquary.ViewModels
{
    public class RegisterViewModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
