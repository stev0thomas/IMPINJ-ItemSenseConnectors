using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ItemSense;
using System.Configuration;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

namespace PalletBuilder
{
    public partial class PalletBldForm : Form
    {
        //Instanciate the logging component
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static AutoResetEvent exit_event = new AutoResetEvent(false);
        private static System.Collections.ArrayList g_thrRecords = null;
        private static System.Collections.ArrayList g_itemEventRecords = null;
        private static System.Collections.ArrayList g_palletRecords = null;

        private static Boolean bRunning = false;
        private static Boolean waitOne = false;
        private static Boolean badConfig = false;

        private static ArrayList unique = null;
        private static ArrayList assigned = null;

        private static String ObsvThreshold = string.Empty;

        public PalletBldForm()
        {
            InitializeComponent();
        }

        private void PalletBldForm_Load(object sender, EventArgs e)
        {
            //Initialize the combo boxes with app.config values
            cmbFacility.Text = ConfigurationManager.AppSettings["LastFacility"];
            cmbDoor.Text = ConfigurationManager.AppSettings["LastZone"];
            txtRun.Text = ConfigurationManager.AppSettings["LastPalletId"];

            btnStart.Enabled = true;
            btnConfig.Enabled = true;
            btnStop.Enabled = false;
            btnExit.Enabled = true;
            btnInsert.Enabled = false;
            btnExport.Enabled = false;

            g_thrRecords = new ArrayList();
            log.Info("Starting AMQP Listening Thread");

            //kick off thread to get Threshold Mode Reads
            Thread threshThread = new Thread(new ThreadStart(InitiateThresholdAMQP));
            threshThread.Start();

            g_itemEventRecords = new ArrayList();
            //kick off thread to get Item Event Reads
            Thread itemThread = new Thread(new ThreadStart(InitiateItemEventAMQP));
            itemThread.Start();

            g_palletRecords = new ArrayList();

            // Set up a timer to trigger refresh of the data grid view.  
            DbTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["EventProcessingInterval(msecs)"]);
            DbTimer.Tick += DbTimer_Tick;
            unique = new ArrayList();
            assigned = new ArrayList();
        }

 
        private void DbTimer_Tick(object sender, EventArgs e)
        {
            if (badConfig)
            {
                ProcessErrorBadConfig();
                DbTimer.Stop();
            }
            else
            {
                waitOne = true;
                if (g_thrRecords.Count > 0)
                {
                    //Do a deep copy of the global array list
                    ArrayList thrRecs = new ArrayList();
                    foreach (ThresholdRec rec in g_thrRecords)
                    {
                        ThresholdRec dup = new ThresholdRec(rec.Epc, rec.ObservationTime, rec.FromZone, rec.ToZone, rec.Threshold, rec.Confidence, rec.JobId, rec.DockDoor, rec.PalletId);
                        thrRecs.Add(dup);
                    }
                    g_thrRecords.Clear();

                    waitOne = false;

                    try
                    {
                        //Use new array to update dataset and gridview in form
                        foreach (ThresholdRec rec in thrRecs)
                        {
                            if (unique.Contains(rec.Epc) == false)
                            {
                                unique.Add(rec.Epc);
                                if (assigned.Contains(rec.Epc) == false)
                                    dataSetTagEvents.Tables[0].LoadDataRow(rec.ToArray(), true);
                            }
                        }
                        gridResults.Update();
                        ProcessAndUpdateCalculations();
                    }
                    catch (Exception ex)
                    {
                        string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                        if (null != ex.InnerException)
                            errMsg += Environment.NewLine + ex.InnerException.Message;
                        log.Error(errMsg);
                        MessageBox.Show("Error in DbTimer_Tick", errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                if (g_itemEventRecords.Count > 0)
                {
                    //Do a deep copy of the global array list
                    ArrayList itemRecs = new ArrayList();
                    foreach (ItemEventRec rec in g_itemEventRecords)
                    {
                        ItemEventRec itm = new ItemEventRec(rec.Epc, rec.TagId, rec.JobId, rec.FromZone, rec.FromFloor,  
                            rec.ToZone, rec.ToFloor, rec.FromFacility, rec.ToFacility, rec.FromX, rec.FromY, rec.ToX, rec.ToY, rec.ObservationTime, rec.PalletId);
                        itemRecs.Add(itm);
                    }
                    g_itemEventRecords.Clear();

                    waitOne = false;

                    try
                    {
                        //Use new array to update dataset and gridview in form
                        foreach (ItemEventRec rec in itemRecs)
                        {
                            if (unique.Contains(rec.Epc) == false)
                            {
                                unique.Add(rec.Epc);
                                if (assigned.Contains(rec.Epc) == false)
                                    dataSetTagEvents.Tables[0].LoadDataRow(rec.ToArray(), true);
                            }
                        }
                        gridResults.Update();
                        ProcessAndUpdateCalculations();
                    }
                    catch (Exception ex)
                    {
                        string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                        if (null != ex.InnerException)
                            errMsg += Environment.NewLine + ex.InnerException.Message;
                        log.Error(errMsg);
                        MessageBox.Show("Error in DbTimer_Tick", errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                waitOne = false;
            }
        }

        private void ProcessAndUpdateCalculations()
        {
            DataTable dtReads = dataSetTagEvents.Tables[0];
            txtTotalTags.Text = dtReads.Rows.Count.ToString();

            DataTable dtAssigned = dataSetPalletTags.Tables[0];
            PalletCntTxt.Text = dtAssigned.Rows.Count.ToString();
        }

        private static void InitiateThresholdAMQP()
        {
            // Define an object that will contain the AMQP Message Queue details
            ItemSense.AmqpMessageQueueDetails MsgQueueDetails = null;

            try
            {
                // Create a JSON object for configuring a Threshold Transition
                // Message Queue
                ThresholdTransitionMessageQueueConfig msgQConfig = new ThresholdTransitionMessageQueueConfig();
                msgQConfig.Threshold = ConfigurationManager.AppSettings["ThresholdTransitionThresholdFilter"];
                msgQConfig.JobId = ConfigurationManager.AppSettings["ThresholdTransitionJobIdFilter"];

                // Create a string-based JSON object of the object
                string objectAsJson = JsonConvert.SerializeObject(msgQConfig);
                // Now translate the JSON into bytes
                byte[] objectAsBytes = Encoding.UTF8.GetBytes(objectAsJson);

                // Create the full path to the configure Threshold Transitions
                // Message Queu endpoint from default ItemSense URI
                string ThresholdTransitionMessageQueueConfigApiEndpoint = ConfigurationManager.AppSettings["ItemSenseUri"] +
                    "/data/v1/items/queues/threshold";

                // Create a WebRequest, identifying it as a PUT request
                // with a JSON payload, and assign it the specified
                // credentials.
                WebRequest ItemSensePutRequest =
                     WebRequest.Create(ThresholdTransitionMessageQueueConfigApiEndpoint);

                ItemSensePutRequest.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["ItemSenseUsername"],
                    ConfigurationManager.AppSettings["ItemSensePassword"]);
                ItemSensePutRequest.Proxy = null;
                ItemSensePutRequest.Method = "PUT";
                ItemSensePutRequest.ContentType = "application/json";
                ItemSensePutRequest.ContentLength = objectAsBytes.Length;

                // Create an output data stream representation of the
                // POST WebRequest to output the data
                Stream dataStream = ItemSensePutRequest.GetRequestStream();
                dataStream.Write(objectAsBytes, 0, objectAsBytes.Length);
                dataStream.Close();

                // Execute the PUT request and retain the response.
                using (HttpWebResponse ItemSenseResponse = (HttpWebResponse)ItemSensePutRequest.GetResponse())
                {
                    // The response StatusCode is a .NET Enum, so convert it to
                    // integer so that we can verify it against the status
                    // codes that ItemSense returns
                    ItemSense.ResponseCode ResponseCode = (ItemSense.ResponseCode)ItemSenseResponse.StatusCode;

                    // In this instance, we are only interested in whether
                    // the ItemSense response to the PUT request was a "Success".
                    if (ItemSense.ResponseCode.Success == ResponseCode)
                    {
                        // Open a stream to access the response data which
                        // contains the AMQP URL and queue identifier
                        Stream ItemSenseDataStream = ItemSenseResponse.GetResponseStream();

                        // Only continue if an actual response data stream exists
                        if (null != ItemSenseDataStream)
                        {
                            // Create a StreamReader to access the resulting data
                            StreamReader objReader = new StreamReader(ItemSenseDataStream);

                            // Read the complete PUT request results as a raw string
                            string itemSenseData = objReader.ReadToEnd();

                            // Now convert the raw JSON into a 
                            // AmqpMessageQueueDetails class
                            // representation
                            MsgQueueDetails = JsonConvert.DeserializeObject<ItemSense.AmqpMessageQueueDetails>(itemSenseData);

                            MsgQueueDetails.ServerUrl = MsgQueueDetails.ServerUrl.Replace(":5672/%2F", string.Empty);

                            string infoMsg = "Message Queue details: " + Environment.NewLine + "URI: " + MsgQueueDetails.ServerUrl +
                                Environment.NewLine + "QueueID: " + MsgQueueDetails.Queue;
                            log.Info(infoMsg);

                            // Close the data stream. If we have got here,
                            // everything has gone well and there are no
                            // errors.
                            ItemSenseDataStream.Close();
                        }
                        else
                        {
                            log.Error("null ItemSense data stream.");
                        }
                    }
                    else
                    {
                        throw (new Exception(string.Format("ItemSense PUT Response returned status of {0}", ItemSenseResponse.StatusCode)));
                    }
                }

                // Now that we have our MessageQueue, we can create a RabbitMQ
                // factory to handle connections to ItemSense AMQP broker
                ConnectionFactory factory = new ConnectionFactory()
                {
                    Uri = MsgQueueDetails.ServerUrl,
                    AutomaticRecoveryEnabled = true,
                    UserName = ConfigurationManager.AppSettings["ItemSenseUsername"],
                    Password = ConfigurationManager.AppSettings["ItemSensePassword"]
                };

                // Now connect to the ItemSense factory
                using (var connection = factory.CreateConnection())

                // Create a fresh channel to handle message queue interactions
                using (var channel = connection.CreateModel())
                {
                    // Create an event consumer to receive incoming events
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    // Bind an event handler to the message received event
                    consumer.Received += Threshold_Received;

                    // Initiate consumption of data from the ItemSense queue
                    channel.BasicConsume(queue: MsgQueueDetails.Queue, noAck: true, consumer: consumer);

                    // Hang on here until exit_event is signaled
                    exit_event.WaitOne();
                }
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
                MessageBox.Show("This application needs to exit as ItemSense AMQP port could not be reached: " + errMsg, "Incorrect Configuration for ItemSense", MessageBoxButtons.OK, MessageBoxIcon.Error);
                badConfig = true;
            }
        }

        private static void InitiateItemEventAMQP()
        {
            // Define an object that will contain the AMQP Message Queue details
            ItemSense.AmqpMessageQueueDetails MsgQueueDetails = null;

            try
            {
                // Create a JSON object for configuring a zoneTransition
                // Message Queue
                ZoneTransitionMessageQueueConfig msgQConfig = new ZoneTransitionMessageQueueConfig();
                if (ConfigurationManager.AppSettings["ZoneTransitionDistanceFilter"].Length > 0)
                    msgQConfig.Distance = Convert.ToInt32(ConfigurationManager.AppSettings["ZoneTransitionDistanceFilter"]);
                if (ConfigurationManager.AppSettings["ZoneTransitionJobIdFilter"].Length > 0)
                    msgQConfig.JobId = ConfigurationManager.AppSettings["ZoneTransitionJobIdFilter"];
                if (ConfigurationManager.AppSettings["ZoneTransitionFromZoneFilter"].Length > 0)
                    msgQConfig.FromZone = ConfigurationManager.AppSettings["ZoneTransitionFromZoneFilter"];
                if (ConfigurationManager.AppSettings["ZoneTransitionToZoneFilter"].Length > 0)
                    msgQConfig.ToZone = ConfigurationManager.AppSettings["ZoneTransitionToZoneFilter"];
                if (ConfigurationManager.AppSettings["ZoneTransitionFromFacilityFilter"].Length > 0)
                    msgQConfig.FromFacility = ConfigurationManager.AppSettings["ZoneTransitionFromFacilityFilter"];
                if (ConfigurationManager.AppSettings["ZoneTransitionToFacilityFilter"].Length > 0)
                    msgQConfig.ToFacility = ConfigurationManager.AppSettings["ZoneTransitionToFacilityFilter"];
                if (ConfigurationManager.AppSettings["ZoneTransitionEpcFilter"].Length > 0)
                    msgQConfig.EPC = ConfigurationManager.AppSettings["ZoneTransitionEpcFilter"];
                if (ConfigurationManager.AppSettings["ZoneTransitionsOnlyFilter"].Length > 0)
                    msgQConfig.ZoneTransitionsOnly = Convert.ToBoolean(ConfigurationManager.AppSettings["ZoneTransitionsOnlyFilter"]);

                // Create a string-based JSON object of the object
                string objectAsJson = JsonConvert.SerializeObject(msgQConfig);
                // Now translate the JSON into bytes
                byte[] objectAsBytes = Encoding.UTF8.GetBytes(objectAsJson);

                // Create the full path to the configure zoneTransitions
                // Message Queu endpoint from default ItemSense URI
                string ZoneTransitionMessageQueueConfigApiEndpoint =
                    ConfigurationManager.AppSettings["ItemSenseUri"] +
                    "/data/v1/items/queues";

                // Create a WebRequest, identifying it as a PUT request
                // with a JSON payload, and assign it the specified
                // credentials.
                WebRequest ItemSensePutRequest =
                     WebRequest.Create(ZoneTransitionMessageQueueConfigApiEndpoint);

                ItemSensePutRequest.Credentials =
                    new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["ItemSenseUsername"],
                        ConfigurationManager.AppSettings["ItemSensePassword"]
                        );
                ItemSensePutRequest.Proxy = null;
                ItemSensePutRequest.Method = "PUT";
                ItemSensePutRequest.ContentType = "application/json";
                ItemSensePutRequest.ContentLength = objectAsBytes.Length;

                // Create an output data stream representation of the
                // POST WebRequest to output the data
                Stream dataStream = ItemSensePutRequest.GetRequestStream();
                dataStream.Write(objectAsBytes, 0, objectAsBytes.Length);
                dataStream.Close();

                // Execute the PUT request and retain the response.
                using (HttpWebResponse ItemSenseResponse = (HttpWebResponse)ItemSensePutRequest.GetResponse())
                {
                    // The response StatusCode is a .NET Enum, so convert it to
                    // integer so that we can verify it against the status
                    // codes that ItemSense returns
                    ItemSense.ResponseCode ResponseCode =
                        (ItemSense.ResponseCode)ItemSenseResponse.StatusCode;

                    // In this instance, we are only interested in whether
                    // the ItemSense response to the PUT request was a "Success".
                    if (ItemSense.ResponseCode.Success == ResponseCode)
                    {
                        // Open a stream to access the response data which
                        // contains the AMQP URL and queue identifier
                        Stream ItemSenseDataStream = ItemSenseResponse.GetResponseStream();

                        // Only continue if an actual response data stream exists
                        if (null != ItemSenseDataStream)
                        {
                            // Create a StreamReader to access the resulting data
                            StreamReader objReader = new StreamReader(ItemSenseDataStream);

                            // Read the complete PUT request results as a raw string
                            string itemSenseData = objReader.ReadToEnd();

                            // Now convert the raw JSON into a 
                            // AmqpMessageQueueDetails class
                            // representation
                            MsgQueueDetails =
                                JsonConvert.DeserializeObject<ItemSense.AmqpMessageQueueDetails>(
                                itemSenseData
                                );

                            MsgQueueDetails.ServerUrl = MsgQueueDetails.ServerUrl.Replace(":5672/%2F", string.Empty);

                            string infoMsg = "Message Queue details: " + Environment.NewLine + "URI: " + MsgQueueDetails.ServerUrl + Environment.NewLine + "QueueID: " + MsgQueueDetails.Queue;
                            log.Info(infoMsg);

                            // Close the data stream. If we have got here,
                            // everything has gone well and there are no
                            // errors.
                            ItemSenseDataStream.Close();
                        }
                        else
                        {
                            log.Error("null ItemSense data stream.");
                        }
                    }
                    else
                    {
                        throw (new Exception(string.Format(
                            "ItemSense PUT Response returned status of {0}",
                            ItemSenseResponse.StatusCode
                            )));
                    }
                }

                // Now that we have our MessageQueue, we can create a RabbitMQ
                // factory to handle connections to ItemSense AMQP broker
                ConnectionFactory factory = new ConnectionFactory()
                {
                    Uri = MsgQueueDetails.ServerUrl,
                    AutomaticRecoveryEnabled = true,
                    UserName = ConfigurationManager.AppSettings["ItemSenseUsername"],
                    Password = ConfigurationManager.AppSettings["ItemSensePassword"]
                };

                // Now connect to the ItemSense factory
                using (var connection = factory.CreateConnection())

                // Create a fresh channel to handle message queue interactions
                using (var channel = connection.CreateModel())
                {
                    // Create an event consumer to receive incoming events
                    EventingBasicConsumer consumer =
                        new EventingBasicConsumer(channel);
                    // Bind an event handler to the message received event
                    consumer.Received += ItemEvent_Received;

                    // Initiate consumption of data from the ItemSense queue
                    channel.BasicConsume(queue: MsgQueueDetails.Queue,
                                         noAck: true,
                                         consumer: consumer);

                    // Hang on here until exit_event is signaled
                    exit_event.WaitOne();
                }
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
            }
        }
        private static void Threshold_Received(object sender, BasicDeliverEventArgs e)
        {
            #region debug_amqp_event_kpi
            DateTime blockTmSt = System.DateTime.Now;
            log.Debug("AMQP Message Received: " + blockTmSt.ToLongTimeString());
            #endregion

            #region debug_amqp_event_kpi
            DateTime blockTmEnd = System.DateTime.Now;
            TimeSpan blockSpan = blockTmEnd.Subtract(blockTmSt);
            log.Debug("AMQP Queue WaitTime(ms): " + blockSpan.TotalMilliseconds.ToString());
            #endregion

            bool bAdd = false;

            if (bRunning)
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    string msg = message.Replace("\\", string.Empty);
                    string cln = msg.Replace("\"", string.Empty);
                    string[] msgFields = cln.Split(',');
                    int recCnt = msgFields.Count();
                    switch (recCnt)
                    {
                        case 8:  //It's a threshold job record
                            {
                                ThresholdRec rec = new ThresholdRec();
                                for (int i = 0; i < msgFields.Count(); i++)
                                {
                                    string[] parms = null;
                                    switch (i)
                                    {
                                        case 0: parms = msgFields[i].Split(':'); rec.Epc = parms[1]; break;
                                        case 1:
                                            {
                                                string x = msgFields[i].Replace("observationTime:", string.Empty);
                                                rec.ObservationTime = Convert.ToDateTime(x);
                                                break;
                                            }
                                        case 2: parms = msgFields[i].Split(':'); rec.FromZone = parms[1]; break;
                                        case 3:
                                            {
                                                parms = msgFields[i].Split(':');
                                                rec.ToZone = parms[1];
                                                break;
                                            }
                                        case 4:
                                            {
                                                parms = msgFields[i].Split(':');
                                                rec.Threshold = parms[1];
                                                break;
                                            }
                                        case 5:
                                            {
                                                parms = msgFields[i].Split(':');
                                                if (parms[1].Length > 0 & parms[1] != @"null")
                                                    rec.Confidence = Convert.ToDouble(parms[1]);
                                                break;
                                            }
                                        case 6: parms = msgFields[i].Split(':'); rec.JobId = parms[1]; break;
                                        case 7:
                                            {
                                                parms = msgFields[i].Split(':');
                                                string x = parms[1];
                                                rec.DockDoor = x.Replace("}", string.Empty);
                                                break;
                                            }
                                        default: break;
                                    }
                                }

                                //Check to see if timer is processing the global array list
                                if (waitOne)
                                {
                                    do
                                        Thread.Sleep(50);
                                    while (waitOne);
                                }

                                //check to see if we want to process this event 
                                if (ObsvThreshold.ToUpper() == "ANY")
                                    bAdd = true;
                                else
                                {
                                    if (rec.Threshold.ToUpper() == ObsvThreshold.ToUpper())
                                        bAdd = true;
                                    else
                                        bAdd = false;
                                }

                                if (bAdd)
                                {
                                    rec.PalletId = Convert.ToInt64(ConfigurationManager.AppSettings["LastPalletId"]);
                                    g_thrRecords.Add(rec);
                                }

                                break;
                            }
                        default: log.Error("Unexpected number of fields received in Threshold AMQP Event Handler.  Expected 8 Received " + msgFields.Count()); break;
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                    if (null != ex.InnerException)
                        errMsg += Environment.NewLine + ex.InnerException.Message;
                    log.Fatal(errMsg);
                }

                #region debug_amqp_event_kpi
                DateTime procEndTm = DateTime.Now;
                TimeSpan procTmSpan = procEndTm.Subtract(blockTmEnd);
                log.Debug("Received: " + message + " Completed(ms): " + procTmSpan.TotalMilliseconds.ToString());
                #endregion
            }
        }

        /// <summary>
        /// Message received event handler - AMQP Callback Item and Item Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ItemEvent_Received(object sender, BasicDeliverEventArgs e)
        {
            #region debug_amqp_event_kpi
            DateTime blockTmSt = System.DateTime.Now;
            log.Debug("AMQP Message Received: " + blockTmSt.ToLongTimeString());
            #endregion

            if (waitOne)
            {
                do
                    Thread.Sleep(50);
                while (waitOne);
            }

            #region debug_amqp_event_kpi
            DateTime blockTmEnd = System.DateTime.Now;
            TimeSpan blockSpan = blockTmEnd.Subtract(blockTmSt);
            log.Debug("AMQP Queue WaitTime(ms): " + blockSpan.TotalMilliseconds.ToString());
            #endregion

            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            bool bAdd = false;

            if (bRunning)
            {
                try
                {
                    string msg = message.Replace("\\", string.Empty);
                    string cln = msg.Replace("\"", string.Empty);
                    string[] msgFields = cln.Split(',');
                    int recCnt = msgFields.Count();
                    switch (recCnt)
                    {
                        case 14:  //It's an Item Event record
                            {
                                ItemEventRec rec = new ItemEventRec();
                                for (int i = 0; i < msgFields.Count(); i++)
                                {
                                    string[] parms = null;
                                    switch (i)
                                    {
                                        case 0: parms = msgFields[i].Split(':'); rec.Epc = parms[1]; break;
                                        case 1: parms = msgFields[i].Split(':'); rec.TagId = parms[1]; break;
                                        case 2: parms = msgFields[i].Split(':'); rec.JobId = parms[1]; break;
                                        case 3: parms = msgFields[i].Split(':'); rec.FromZone = parms[1]; break;
                                        case 4: parms = msgFields[i].Split(':'); rec.FromFloor = parms[1]; break;
                                        case 5:
                                            {
                                                parms = msgFields[i].Split(':');
                                                rec.ToZone = parms[1];
                                                break;
                                            }
                                        case 6: parms = msgFields[i].Split(':'); rec.ToFloor = parms[1]; break;
                                        case 7: parms = msgFields[i].Split(':'); rec.FromFacility = parms[1]; break;
                                        case 8: parms = msgFields[i].Split(':'); rec.ToFacility = parms[1]; break;
                                        case 9:
                                            {
                                                parms = msgFields[i].Split(':');
                                                if (parms[1].Length > 0 & parms[1] != @"null")
                                                    rec.FromX = Convert.ToDouble(parms[1]);
                                                break;
                                            }
                                        case 10:
                                            {
                                                parms = msgFields[i].Split(':');
                                                if (parms[1].Length > 0 & parms[1] != @"null")
                                                    rec.FromY = Convert.ToDouble(parms[1]);
                                                break;
                                            }
                                        case 11:
                                            {
                                                parms = msgFields[i].Split(':');
                                                if (parms[1].Length > 0 & parms[1] != @"null")
                                                    rec.ToX = Convert.ToDouble(parms[1]);
                                                break;
                                            }
                                        case 12:
                                            {
                                                parms = msgFields[i].Split(':');
                                                if (parms[1].Length > 0 & parms[1] != @"null")
                                                    rec.ToY = Convert.ToDouble(parms[1]);
                                                break;
                                            }
                                        case 13:
                                            {
                                                string y = msgFields[i].Replace("observationTime:", string.Empty);
                                                rec.ObservationTime = Convert.ToDateTime(y.Replace("}", string.Empty));
                                                break;
                                            }
                                        default: break;
                                    }
                                }

                                //Check to see if timer is processing the global array list
                                if (waitOne)
                                {
                                    do
                                        Thread.Sleep(50);
                                    while (waitOne);
                                }

                                //check to see if we want to process this event 
                                if (ObsvThreshold.ToUpper() == "ANY")
                                    bAdd = true;
                                else
                                {
                                    if (rec.ToZone.ToUpper() == ObsvThreshold.ToUpper())
                                        bAdd = true;
                                    else
                                        bAdd = false;
                                }

                                if (bAdd)
                                {
                                    rec.PalletId = Convert.ToInt64(ConfigurationManager.AppSettings["LastPalletId"]);
                                    g_itemEventRecords.Add(rec);
                                }

                                break;
                            }
                        default: log.Error("Unexpected number of fields received in Item_Event AMQP Event Handler.  Expected 14 Received " + msgFields.Count()); break;
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                    if (null != ex.InnerException)
                        errMsg += Environment.NewLine + ex.InnerException.Message;
                    log.Fatal(errMsg);
                }
            }

            #region debug_amqp_event_kpi
            DateTime procEndTm = DateTime.Now;
            TimeSpan procTmSpan = procEndTm.Subtract(blockTmEnd);
            log.Debug("Received: " + message + " Completed(ms): " + procTmSpan.TotalMilliseconds.ToString());
            #endregion
        }


        private void PalletBldForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit_event.Set();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (badConfig)
                ProcessErrorBadConfig();

            unique.Clear();
            assigned.Clear();
            g_palletRecords.Clear();
            g_itemEventRecords.Clear();
            g_thrRecords.Clear();

            bRunning = true;
            EnableControls(false);
            ObsvThreshold = cmbDoor.Text;
            txtTotalTags.Text = "0";
            PalletCntTxt.Text = "0";

            dataSetTagEvents.Clear();
            gridResults.Refresh();
            dataSetPalletTags.Clear();
            gridPallet.Refresh();

            DbTimer.Start();
        }

        private void ProcessErrorBadConfig()
        {
            EnableControls(false);
            btnStop.Enabled = false;
            btnConfig.Enabled = true;
            MessageBox.Show("Could not connect to ItemSense.  Please check configuration and ItemSense health.", "Bad ItemSense Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void EnableControls(bool enabled)
        {
            cmbFacility.Enabled = enabled;
            btnStart.Enabled = enabled;
            btnStop.Enabled = !enabled;
            btnInsert.Enabled = enabled;
            btnExit.Enabled = enabled;
            btnExport.Enabled = enabled;
            btnConfig.Enabled = enabled;
            cmbDoor.Enabled = enabled;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (badConfig)
                ProcessErrorBadConfig();

            DbTimer.Stop();
            bRunning = false;
            EnableControls(true);
            gridResults.Refresh();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (badConfig)
                ProcessErrorBadConfig();

            long testNum = Convert.ToInt64(txtRun.Text);

            if (dataSetTagEvents.Tables[0].Rows.Count > 0)
            {
                string dbms = ConfigurationManager.AppSettings["TypeRDBMS"];
                switch (dbms.ToUpper())
                {
                    case "POSTGRESQL":
                        {
                            if (PostgreSqlInsertPallet())
                            {
                                //Insert test results into Database
                                if (PostgreSqlInsertPalletAssociations())
                                {
                                    testNum++;
                                    txtRun.Text = testNum.ToString();
                                    txtRun.Update();
                                    SaveTestRunParams();
                                }
                            }
                            break;
                        }
                    case "SQLSERVER":
                        {
                            if (SqlServerInsertPallet())
                            {
                                //Insert test results into Database
                                if (SqlServerInsertPalletAssociations())
                                {
                                    testNum++;
                                    txtRun.Text = testNum.ToString();
                                    txtRun.Update();
                                    SaveTestRunParams();
                                }
                            }

                            break;
                        }
                    default:
                        {
                            log.Error("App.config has incorrect database name defined.  POSTGRESQL or SQLSERVER are only valid options currently...");
                            break;
                        }
                }

            }
            else
            {
                MessageBox.Show("Nothing to insert", "No Data to insert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            btnInsert.Enabled = false;
        }

        private bool PostgreSqlInsertPallet()
        {
            bool retVal = false;

            try
            {
                #region PostgreSQL DDL
                //Do Not Alter - These strings are modified via the app.cfg
                //Drop and Create "updatedb_cmd"
                const string cmdText = @"CREATE TABLE IF NOT EXISTS {is_pallet_bld} (palletId Bigint, facility character varying(48) NOT NULL, " +
                    @"zoneName character varying(48), createTm timestamptz, PRIMARY KEY(palletId))WITH(OIDS= FALSE); ";
                string cfgCmdText = cmdText.Replace("{is_pallet_bld}", ConfigurationManager.AppSettings["ItemSensePalletBuildTableName"]);

                //Insert TestRun "insertRun_cmd"
                const string postCmdText = @"INSERT INTO {is_pallet_bld} (palletId, facility, zoneName, createTm) " +
                    @"VALUES(@palletId, @facility, @version, @zoneName, @createTm); ";
                string postCfgCmdText = postCmdText.Replace("{is_pallet_bld}", ConfigurationManager.AppSettings["ItemSensePalletBuildTableName"]);

                #endregion

                string connStr = ConfigurationManager.AppSettings["DbConnectionString"];
                using (NpgsqlConnection conn = new NpgsqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand createdb_cmd = new NpgsqlCommand(cfgCmdText, conn);
                        //Create history table if necessary, drop and recreate the temporary threshold table
                        createdb_cmd.ExecuteNonQuery();

                        //Now build the insert cmd
                        NpgsqlCommand insertRun_cmd = new NpgsqlCommand(postCfgCmdText, conn);
                        insertRun_cmd.CommandType = CommandType.Text;
                        insertRun_cmd.Parameters.Add("@palletId", NpgsqlDbType.Bigint).Value = Convert.ToInt64(txtRun.Text);
                        insertRun_cmd.Parameters.Add("@facility", NpgsqlDbType.Varchar, 48).Value = cmbFacility.Text;
                        insertRun_cmd.Parameters.Add("@zoneName", NpgsqlDbType.Varchar, 48).Value = cmbDoor.Text;
                        insertRun_cmd.Parameters.Add("@createTm", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;

                        //insert the new test run params into the table 
                        insertRun_cmd.ExecuteNonQuery();

                        log.Info("InsertPallet rows inserted: 1");

                    }
                    catch (Exception ex)
                    {
                        string errMsg = "PostgreSqlInsertPallet Exception: " + ex.Message + "(" + ex.GetType() + ")";
                        if (null != ex.InnerException)
                            errMsg += Environment.NewLine + ex.InnerException.Message;
                        log.Error(errMsg);
                        MessageBox.Show("Error in PostgreSqlInsertPallet: " + errMsg, "Error while inserting to database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
            }

            return retVal;
        }

        private bool SqlServerInsertPallet()
        {
            bool retVal = false;

            try
            {
                #region SqlServer DDL
                //Do Not Alter - These strings are modified via the app.cfg
                //Drop and Create "updatedb_cmd"
                const string cmdText = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{is_pallet_bld}' AND xtype = 'U') CREATE TABLE " +
                    @"{is_pallet_bld} (palletId BigInt, facility VARCHAR(48) NOT NULL, zoneName VARCHAR(48), createTm DateTime, PRIMARY KEY(palletId)); ";
                string cfgCmdText = cmdText.Replace("{is_pallet_bld}", ConfigurationManager.AppSettings["ItemSensePalletBuildTableName"]);

                //Insert TestRun "insertRun_cmd"
                const string postCmdText = @"INSERT INTO {is_pallet_bld} (palletId, facility, zoneName, create_time) " +
                    @"VALUES(@palletId, @facility, @zoneName, @createTm); ";
                string postCfgCmdText = postCmdText.Replace("{is_pallet_bld}", ConfigurationManager.AppSettings["ItemSensePalletBuildTableName"]);

                #endregion

                string connStr = ConfigurationManager.AppSettings["DbConnectionString"];
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand createdb_cmd = new SqlCommand(cfgCmdText, conn);
                        //Create history table if necessary, drop and recreate the temporary threshold table
                        createdb_cmd.ExecuteNonQuery();

                        //Now build the insert cmd
                        SqlCommand insertRun_cmd = new SqlCommand(postCfgCmdText, conn);
                        insertRun_cmd.CommandType = CommandType.Text;
                        insertRun_cmd.Parameters.Add("@palletId", SqlDbType.BigInt).Value = Convert.ToInt64(txtRun.Text);
                        insertRun_cmd.Parameters.Add("@facility", SqlDbType.VarChar, 48).Value = cmbFacility.Text;
                        insertRun_cmd.Parameters.Add("@zoneName", SqlDbType.VarChar, 48).Value = cmbDoor.Text;
                        insertRun_cmd.Parameters.Add("@createTm", SqlDbType.DateTime).Value = DateTime.UtcNow;
                        
                        //insert the new test run params into the table 
                        insertRun_cmd.ExecuteNonQuery();

                        log.Info("SqlServerInsertPallet rows inserted: 1");

                    }
                    catch (Exception ex)
                    {
                        string errMsg = "SqlServerInsertPallet Exception: " + ex.Message + "(" + ex.GetType() + ")";
                        if (null != ex.InnerException)
                            errMsg += Environment.NewLine + ex.InnerException.Message;
                        log.Error(errMsg);
                        MessageBox.Show("Error in InsertPallet: " + errMsg, "Error while inserting to database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
            }

            return retVal;
        }

        private void SaveTestRunParams()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;

                settings["LastZone"].Value = cmbDoor.Text;
                settings["LastFacility"].Value = cmbFacility.Text;
                settings["LastPalletId"].Value = txtRun.Text;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Error(errMsg);
                MessageBox.Show(errMsg, "Failed to save test run parameters!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SqlServerInsertPalletAssociations()
        {
            bool retVal = false;

            try
            {
                #region SqlServer DDL
                //Do Not Alter - These strings are modified via the app.cfg
                //Drop and Create "updatedb_cmd"
                const string cmdText = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{is_pallet_asc}' AND xtype = 'U') CREATE TABLE {is_pallet_asc} (epcNbr VARCHAR(128) NOT NULL,  " +
                    @"palletId BigInt, PRIMARY KEY(palletId, epcNbr); " +
                    @"IF EXISTS (SELECT * FROM sysobjects WHERE name='{is_pallet_asc}_TMP' AND xtype = 'U') DROP TABLE {is_pallet_asc}_TMP; CREATE TABLE {is_pallet_asc}_TMP (epcNbr VARCHAR(128) NOT NULL, " +
                    @"palletId BigInt); ";
                string cfgCmdText = cmdText.Replace("{is_pallet_asc}", ConfigurationManager.AppSettings["ItemSensePalletAssociationTableName"]);

                //Update TestResults "updateResults_cmd"
                const string postCmdText = @"INSERT INTO {is_pallet_asc} SELECT DISTINCT epcNbr, palletId FROM {is_pallet_asc}_TMP; ";
                string postCfgCmdText = postCmdText.Replace("{is_pallet_asc}", ConfigurationManager.AppSettings["ItemSensePalletAssociationTableName"]);

                #endregion

                string connStr = ConfigurationManager.AppSettings["DbConnectionString"];
                System.Data.DataTableReader reader = dataSetPalletTags.Tables[0].CreateDataReader();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand createdb_cmd = new SqlCommand(cfgCmdText, conn);
                        //Create history table if necessary, drop and recreate the temporary threshold table
                        createdb_cmd.ExecuteNonQuery();
                        //Bulk insert into the threshold table events just read
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                        {
                            bulkCopy.DestinationTableName = ConfigurationManager.AppSettings["ItemSensePalletAssociationTableName"];
                            bulkCopy.WriteToServer(reader);
                        }

                        SqlCommand updateResults_cmd = new SqlCommand(postCfgCmdText, conn);
                        //update the threshold_history table with whatever is in threshold table
                        updateResults_cmd.ExecuteNonQuery();

                        log.Info("InsertTestResult rows inserted: " + dataSetPalletTags.Tables[0].Rows.Count.ToString());

                    }
                    catch (Exception ex)
                    {
                        string errMsg = "InsertPalletAssociations Exception: " + ex.Message + "(" + ex.GetType() + ")";
                        if (null != ex.InnerException)
                            errMsg += Environment.NewLine + ex.InnerException.Message;
                        log.Error(errMsg);
                        MessageBox.Show("Error in InsertPalletAssociations: " + errMsg, "Error while inserting to database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        reader.Close();
                        conn.Close();
                    }
                }


                retVal = true;
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
            }

            return retVal;
        }

        private bool PostgreSqlInsertPalletAssociations()
        {
            bool retVal = false;

            try
            {
                #region PostgreSql DDL
                //Do Not Alter - These strings are modified via the app.cfg
                //Drop and Create "updatedb_cmd"
                const string cmdText = @"CREATE TABLE IF NOT EXISTS {is_pallet_asc} (epcNbr character varying(128) NOT NULL, " +
                    @"palletId Bigint, PRIMARY KEY(palletId, epcNbr))WITH(OIDS= FALSE); " +
                    @"DROP TABLE IF EXISTS {is_pallet_asc}_TMP; CREATE TABLE {is_pallet_asc}_TMP (epcNbr character varying(128) NOT NULL, " +
                    @"palletId Bigint)WITH(OIDS= FALSE); ";

                string cfgCmdText = cmdText.Replace("{is_pallet_asc}", ConfigurationManager.AppSettings["ItemSensePalletAssociationTableName"]);

                //Bulk Insert
                string tmpTxt = "COPY {is_pallet_asc}_TMP (epcNbr, palletId) FROM STDIN WITH DELIMITER ',' CSV";
                string impText = tmpTxt.Replace("{is_pallet_asc}", ConfigurationManager.AppSettings["ItemSensePalletAssociationTableName"]);

                //Update TestResults "updateResults_cmd"
                const string postCmdText = @"INSERT INTO {is_pallet_asc} SELECT DISTINCT epcNbr, palletId FROM {is_pallet_asc}_TMP; ";
                string postCfgCmdText = postCmdText.Replace("{is_pallet_asc}", ConfigurationManager.AppSettings["ItemSensePalletAssociationTableName"]);

                #endregion

                string connStr = ConfigurationManager.AppSettings["DbConnectionString"];
                System.Data.DataTableReader reader = dataSetPalletTags.Tables[0].CreateDataReader();
                using (NpgsqlConnection conn = new NpgsqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand createdb_cmd = new NpgsqlCommand(cfgCmdText, conn);
                        //Create history table if necessary, drop and recreate the temporary threshold table
                        createdb_cmd.ExecuteNonQuery();

                        //Bulk insert into the threshold table events just read
                        using (var writer = conn.BeginTextImport(impText))
                        {
                            DataTable datatable = dataSetPalletTags.Tables[0];
                            foreach (DataRow dr in datatable.Rows)
                            {
                                string rw = string.Empty;
                                for (int i = 0; i < datatable.Columns.Count; i++)
                                {
                                    rw = rw + dr[i].ToString();

                                    if (i < datatable.Columns.Count - 1)
                                        rw = rw + ",";
                                }
                                writer.WriteLine(rw);
                            }
                        }


                        NpgsqlCommand updateResults_cmd = new NpgsqlCommand(postCfgCmdText, conn);
                        //update the threshold_history table with whatever is in threshold table
                        updateResults_cmd.ExecuteNonQuery();

                        log.Info("PostgreSqlInsertPalletAssociations rows inserted: " + dataSetPalletTags.Tables[0].Rows.Count.ToString());

                    }
                    catch (Exception ex)
                    {
                        string errMsg = "PostgreSqlInsertPalletAssociations Exception: " + ex.Message + "(" + ex.GetType() + ")";
                        if (null != ex.InnerException)
                            errMsg += Environment.NewLine + ex.InnerException.Message;
                        log.Error(errMsg);
                        MessageBox.Show("Error in PostgreSqlInsertPalletAssociations: " + errMsg, "Error while inserting to database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        reader.Close();
                        conn.Close();
                    }
                }


                retVal = true;
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
            }

            return retVal;
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Exit Application", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SaveTestRunParams();
                Application.Exit();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (badConfig)
                ProcessErrorBadConfig();

            try
            {
                if (dataSetPalletTags.Tables[0].Rows.Count > 0)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(dlg.FileName))
                        {
                            DataTable datatable = dataSetPalletTags.Tables[0];
                            string ln = string.Empty;
                            for (int i = 0; i < datatable.Columns.Count; i++)
                            {
                                ln = ln + datatable.Columns[i].ToString();
                                if (i < datatable.Columns.Count - 1)
                                    ln = ln + ",";
                            }
                            sw.WriteLine(ln);
                            foreach (DataRow dr in datatable.Rows)
                            {
                                string rw = string.Empty;
                                for (int i = 0; i < datatable.Columns.Count; i++)
                                {
                                    rw = rw + dr[i].ToString();

                                    if (i < datatable.Columns.Count - 1)
                                        rw = rw + ",";
                                }
                                sw.WriteLine(rw);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No results to export", "Please start a run and capture pallet tag events before trying to export.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnExport.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Fatal(errMsg);
                MessageBox.Show("Error Exporting CSV", "Error:" + errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

 
        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfigurationForm dlg = new ConfigurationForm();
            if (dlg.ShowDialog() == DialogResult.OK)
                Application.Exit();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            gridResults.SelectAll();
        }

        private void MoveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in gridResults.SelectedRows)
                {
                    if (assigned.Contains(row.Cells[0].Value.ToString()) == false)
                    {
                        PalletRec rec = new PalletRec(row.Cells[0].Value.ToString(), Convert.ToInt64(txtRun.Text));
                        assigned.Add(rec.Epc); 
                        g_palletRecords.Add(rec);
                        //Use new array to update dataset and gridview in form
                        dataSetPalletTags.Tables[0].LoadDataRow(rec.ToArray(), true);
                        gridPallet.Update();
                    }
                }
                PalletCntTxt.Text = dataSetPalletTags.Tables[0].Rows.Count.ToString();

                //Now clear the dataSetTagEvents table and allow it to repopulate with unique reads and not assigned
                dataSetTagEvents.Tables[0].Clear();
                gridResults.Update();
                unique.Clear();
                txtTotalTags.Text = "0";
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Error(errMsg);
                MessageBox.Show("Error in MoveBtn_Click", errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            try
            {
                g_palletRecords.Clear();
                assigned.Clear();
                dataSetPalletTags.Tables[0].Clear();
                gridPallet.Update();
            }
            catch (Exception ex)
            {
                string errMsg = "Exception: " + ex.Message + "(" + ex.GetType() + ")";
                if (null != ex.InnerException)
                    errMsg += Environment.NewLine + ex.InnerException.Message;
                log.Error(errMsg);
                MessageBox.Show("Error in ClearBtn_Click", errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
