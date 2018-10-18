﻿// Copyright ©2018 Impinj, Inc. All rights reserved.
// You may use and modify this code under the terms of the Impinj Software Tools License & Disclaimer.
// Visit https://support.impinj.com/hc/en-us/articles/360000468370-Software-Tools-License-Disclaimer
// for full license details, or contact Impinj, Inc. at support@impinj.com for a copy of the license.

////////////////////////////////////////////////////////////////////////////////
//
//    Threshold Transition Message Queue Config
//
////////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;

namespace ItemSense
{
    internal class ThresholdTransitionMessageQueueConfig
    {
       [JsonProperty("threshold", NullValueHandling = NullValueHandling.Ignore)]
        public string Threshold { get; set; }

       [JsonProperty("jobId", NullValueHandling = NullValueHandling.Ignore)]
        public string JobId { get; set; }


    }
}
