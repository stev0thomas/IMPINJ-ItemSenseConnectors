// Copyright ©2018 Impinj, Inc. All rights reserved.
// You may use and modify this code under the terms of the Impinj Software Tools License & Disclaimer.
// Visit https://support.impinj.com/hc/en-us/articles/360000468370-Software-Tools-License-Disclaimer
// for full license details, or contact Impinj, Inc. at support@impinj.com for a copy of the license.

using System;
using Newtonsoft.Json;
using System.Collections;

namespace ItemSense
{
    class ThresholdRec
    {

        [JsonProperty("epc", NullValueHandling = NullValueHandling.Ignore)]
        public string Epc { get; set; }

        [JsonProperty("fromZone", NullValueHandling = NullValueHandling.Ignore)]
        public string FromZone { get; set; } = null;

        [JsonProperty("toZone", NullValueHandling = NullValueHandling.Ignore)]
        public string ToZone { get; set; } = null;

        [JsonProperty("threshold", NullValueHandling = NullValueHandling.Ignore)]
        public string Threshold { get; set; } = null;

        [JsonProperty("thresholdId", NullValueHandling = NullValueHandling.Ignore)]
        public long ThresholdId { get; set; }

        [JsonProperty("confidence", NullValueHandling = NullValueHandling.Ignore)]
        public double Confidence { get; set; } = 0;

        [JsonProperty("jobId", NullValueHandling = NullValueHandling.Ignore)]
        public string JobId { get; set; } = null;

        [JsonProperty("jobName", NullValueHandling = NullValueHandling.Ignore)]
        public string JobName { get; set; } = null;

        [JsonProperty("observationTime")]
        public DateTime ObservationTime { get; set; }

        [JsonProperty("palletId", NullValueHandling = NullValueHandling.Ignore)]
        public long PalletId { get; set; }

        public ThresholdRec()
        {
        }

        public ThresholdRec(string epc, string fromZone, string toZone, string threshold, long thresholdId, double confidence,
            string jobId, string jobName, DateTime observationTime)
        {
            Epc = epc;
            FromZone = fromZone;
            ToZone = toZone;
            Threshold = threshold;
            ThresholdId = thresholdId;
            Confidence = confidence;
            JobId = jobId;
            JobName = jobName;
            ObservationTime = observationTime;
        }

        public object[] ToArray()
        {
            ArrayList retVal = new System.Collections.ArrayList();
            retVal.Add(Epc);
            retVal.Add(Threshold);
            retVal.Add(PalletId);
            return retVal.ToArray();
        }

        public string ThresholdRecToCsvString()
        {
            return string.Format(
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                Epc,
                FromZone,
                ToZone,
                Threshold,
                ThresholdId,
                Confidence,
                JobId,
                JobName,
                ObservationTime,
                PalletId
                );
        }
    }
}

