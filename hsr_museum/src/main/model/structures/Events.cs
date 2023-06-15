using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hsr_museum.src.main.model.structures
{
    public class Events
    {
        [JsonProperty("ID")]
        public uint id { get; private set; }
        [JsonProperty("Name")]
        public string name { get; private set; }
        
        public Events(uint ID, string Name) {
            this.id = ID;
            this.name = Name;
        }
    }

    
}