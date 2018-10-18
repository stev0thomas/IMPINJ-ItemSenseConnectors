////////////////////////////////////////////////////////////////////////////////
//
//    Zone Transition Message Queue Config
//
////////////////////////////////////////////////////////////////////////////////


// Copyright ©2018 Impinj, Inc. All rights reserved.
// You may use and modify this code under the terms of the Impinj Software Tools License & Disclaimer.
// Visit https://support.impinj.com/hc/en-us/articles/360000468370-Software-Tools-License-Disclaimer
// for full license details, or contact Impinj, Inc. at support@impinj.com for a copy of the license.

using Newtonsoft.Json;

namespace ItemSense
{
    internal class ZoneTransitionMessageQueueConfig
    {
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int Distance { get; set; } = 0;

        [JsonProperty("jobId", NullValueHandling = NullValueHandling.Ignore)]
        public string JobId { get; set; } 

        [JsonProperty("fromZone", NullValueHandling=NullValueHandling.Ignore)]
        public string FromZone { get; set; }

        [JsonProperty("toZone", NullValueHandling = NullValueHandling.Ignore)]
        public string ToZone { get; set; }

         [JsonProperty("epc", NullValueHandling=NullValueHandling.Ignore)]
        public string EPC { get; set; }

       [JsonProperty("fromFacility", NullValueHandling = NullValueHandling.Ignore)]
        public string FromFacility { get; set; }

        [JsonProperty("toFacility", NullValueHandling = NullValueHandling.Ignore)]
        public string ToFacility { get; set; }

        [JsonProperty("zoneTransitionsOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool ZoneTransitionsOnly { get; set; } = false;

    }
}
