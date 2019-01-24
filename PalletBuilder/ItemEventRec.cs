// Copyright ©2018 Impinj, Inc. All rights reserved.
// You may use and modify this code under the terms of the Impinj Software Tools License & Disclaimer.
// Visit https://support.impinj.com/hc/en-us/articles/360000468370-Software-Tools-License-Disclaimer
// for full license details, or contact Impinj, Inc. at support@impinj.com for a copy of the license.

using System;
using Newtonsoft.Json;
using System.Collections;


namespace PalletBuilder
{
    class ItemEventRec
    {

        [JsonProperty("epc", NullValueHandling = NullValueHandling.Ignore)]
        public string Epc { get; set; }

        [JsonProperty("tagId", NullValueHandling = NullValueHandling.Ignore)]
        public string TagId { get; set; } = null;

        [JsonProperty("jobId", NullValueHandling = NullValueHandling.Ignore)]
        public string JobId { get; set; } = null;

        [JsonProperty("jobName", NullValueHandling = NullValueHandling.Ignore)]
        public string JobName { get; set; } = null;

        [JsonProperty("fromZone", NullValueHandling = NullValueHandling.Ignore)]
        public string FromZone { get; set; } = null;

        [JsonProperty("fromFloor", NullValueHandling = NullValueHandling.Ignore)]
        public string FromFloor { get; set; } = null;

        [JsonProperty("toZone", NullValueHandling = NullValueHandling.Ignore)]
        public string ToZone { get; set; } = null;

        [JsonProperty("toFloor", NullValueHandling = NullValueHandling.Ignore)]
        public string ToFloor { get; set; } = null;

        [JsonProperty("fromFacility", NullValueHandling = NullValueHandling.Ignore)]
        public string FromFacility { get; set; } = null;

        [JsonProperty("toFacility", NullValueHandling = NullValueHandling.Ignore)]
        public string ToFacility { get; set; } = null;

        [JsonProperty("fromX", NullValueHandling = NullValueHandling.Ignore)]
        public double FromX { get; set; } = 0;

        [JsonProperty("fromY", NullValueHandling = NullValueHandling.Ignore)]
        public double FromY { get; set; } = 0;

        [JsonProperty("toX", NullValueHandling = NullValueHandling.Ignore)]
        public double ToX { get; set; } = 0;

        [JsonProperty("toY", NullValueHandling = NullValueHandling.Ignore)]
        public double ToY { get; set; } = 0;

        [JsonProperty("observationTime")]
        public DateTime ObservationTime { get; set; }

        [JsonProperty("palletId", NullValueHandling = NullValueHandling.Ignore)]
        public long PalletId { get; set; }

        public ItemEventRec()
        {
        }

        public ItemEventRec(string epc, string tagId, string jobId, string jobName, string fromZone, string fromFloor, string toZone, string toFloor, string fromFacility, string toFacility,
            double fromX, double fromY, double toX, double toY, DateTime observationTime, long palletId)
        {
            Epc = epc;
            TagId = tagId;
            JobId = jobId;
            JobName = jobName;
            FromZone = fromZone;
            FromFloor = fromFloor;
            ToZone = toZone;
            ToFloor = toFloor;
            FromFacility = fromFacility;
            ToFacility = toFacility;
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            ObservationTime = observationTime;
            PalletId = palletId;
        }

        public object[] ToArray()
        {
            ArrayList retVal = new System.Collections.ArrayList();
            retVal.Add(Epc);
            retVal.Add(ToZone);
            retVal.Add(PalletId);
            return retVal.ToArray();
        }

        public string ItemEventRecToCsvString()
        {
            return string.Format(
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                Epc,
                TagId,
                JobId,
                JobName,
                FromZone,
                FromFloor,
                ToZone,
                ToFloor,
                FromFacility,
                ToFacility,
                FromX,
                FromY,
                ToX,
                ToY,
                ObservationTime,
                PalletId
                );
        }


    }
}
