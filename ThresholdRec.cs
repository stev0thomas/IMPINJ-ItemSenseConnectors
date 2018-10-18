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

        [JsonProperty("observationTime")]
        public DateTime ObservationTime { get; set; }

        [JsonProperty("fromZone", NullValueHandling = NullValueHandling.Ignore)]
        public string FromZone { get; set; } = null;

        [JsonProperty("toZone", NullValueHandling = NullValueHandling.Ignore)]
        public string ToZone { get; set; } = null;

        [JsonProperty("threshold", NullValueHandling = NullValueHandling.Ignore)]
        public string Threshold { get; set; } = null;

        [JsonProperty("confidence", NullValueHandling = NullValueHandling.Ignore)]
        public double Confidence { get; set; } = 0;

        [JsonProperty("jobId", NullValueHandling = NullValueHandling.Ignore)]
        public string JobId { get; set; } = null;

        [JsonProperty("dockDoor", NullValueHandling = NullValueHandling.Ignore)]
        public string DockDoor { get; set; } = null;

        [JsonProperty("testRunNbr", NullValueHandling = NullValueHandling.Ignore)]
        public long TestRunNbr { get; set; }

        public ThresholdRec()
        {
        }

        public ThresholdRec(string epc, DateTime observationTime, string fromZone, string toZone, string threshold, double confidence, string jobId, string dockDoor, long testRunNbr)
        {
            Epc = epc;
            ObservationTime = observationTime;
            FromZone = fromZone;
            ToZone = toZone;
            Threshold = threshold;
            Confidence = confidence;
            JobId = jobId;
            DockDoor = dockDoor;
            TestRunNbr = testRunNbr;
        }

        public object[] ToArray()
        {
            ArrayList retVal = new System.Collections.ArrayList();
            retVal.Add(Epc);
            retVal.Add(ObservationTime);
            retVal.Add(FromZone);
            retVal.Add(ToZone);
            retVal.Add(Threshold);
            retVal.Add(Confidence);
            retVal.Add(JobId);
            retVal.Add(DockDoor);
            retVal.Add(TestRunNbr);
            return retVal.ToArray();
        }

        public string ThresholdRecToCsvString()
        {
            return string.Format(
                "{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                Epc,
                ObservationTime,
                FromZone,
                ToZone,
                Threshold,
                Confidence,
                JobId,
                DockDoor,
                TestRunNbr
                );
        }
    }
}

