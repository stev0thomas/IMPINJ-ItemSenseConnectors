// Copyright ©2018 Impinj, Inc. All rights reserved.
// You may use and modify this code under the terms of the Impinj Software Tools License & Disclaimer.
// Visit https://support.impinj.com/hc/en-us/articles/360000468370-Software-Tools-License-Disclaimer
// for full license details, or contact Impinj, Inc. at support@impinj.com for a copy of the license.

using System;
using Newtonsoft.Json;
using System.Collections;

namespace ItemSense
{
    class PalletRec
    {

        [JsonProperty("epc", NullValueHandling = NullValueHandling.Ignore)]
        public string Epc { get; set; }


        [JsonProperty("palletId", NullValueHandling = NullValueHandling.Ignore)]
        public long PalletId { get; set; }

        public PalletRec()
        {
        }

        public PalletRec(string epc, long palletId)
        {
            Epc = epc;
            PalletId = palletId;
        }

        public object[] ToArray()
        {
            ArrayList retVal = new System.Collections.ArrayList();
            retVal.Add(Epc);
            retVal.Add(PalletId);
            return retVal.ToArray();
        }

        public string PalletRecToCsvString()
        {
            return string.Format(
                "{0},{1}",
                Epc,
                PalletId
                );
        }
    }
}
