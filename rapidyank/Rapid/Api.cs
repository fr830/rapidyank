using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace rapidyank.Rapid
{
    public class Api
    {
        public class DataString { public string d { get; set; } }

        public string BaseUrl { get; set; }
        public RestClient Client { get; set; }

        public Api(string baseUrl)
        {
            BaseUrl = baseUrl;
            Client = new RestClient(BaseUrl)
            {
                CookieContainer = new CookieContainer()
            };
        }

        /// <summary>
        /// Log in to the system
        /// </summary>
        /// <remarks>The method requires that an HTTP context exists.
        /// Returns the bool, packed in DataTransferObject, in JSON format</remarks>
        public bool Login(string username, string password)
        {
            var request = new RestRequest("Login", Method.GET)
                    .AddParameter("username", username, ParameterType.GetOrPost)
                    .AddParameter("password", password, ParameterType.GetOrPost);
            var resp = Execute<Resp<bool>>(request);
            return resp.Data;
        }
        /// <summary>
        /// Verify that the user is logged on
        /// </summary>
        /// <remarks>Returns the bool, packed in DataTransferObject, in JSON format</remarks>
        public bool CheckLoggedOn()
        {
            var request = new RestRequest("CheckLoggedOn", Method.GET);
            var resp = Execute<Resp<bool>>(request);
            return resp.Data;
        }
        /// <summary>
        /// Get current input channel data
        /// </summary>
        /// <remarks>Returns SrezTableLight.CnlData, packed in DataTransferObject, in JSON format</remarks>
        public CnlData GetCurCnlData(int cnlNum)
        {
            var request = new RestRequest("GetCurCnlData", Method.GET)
                .AddParameter("cnlNum", cnlNum, ParameterType.GetOrPost);
            var resp = Execute<Resp<CnlData>>(request);
            return resp.Data;
        }
        /// <summary>
        /// Get extended current data for a given filter
        /// </summary>
        /// <remarks>Returns CnlDataExt [], packed in DataTransferObject, in JSON format.
        /// If a filter is specified by the view, then it must already be loaded into the cache</remarks>
        public CnlDataExt[] GetCurCnlDataExt(string cnlNums, string viewIDs, int viewID)
        {
            var request = new RestRequest("GetCurCnlDataExt", Method.GET)
                .AddParameter("cnlNums", cnlNums, ParameterType.GetOrPost)
                .AddParameter("viewIDs", viewIDs, ParameterType.GetOrPost)
                .AddParameter("viewID", viewID, ParameterType.GetOrPost);
            var resp = Execute<Resp<CnlDataExt[]>>(request);
            return resp.Data;
        }
        /// <summary>
        /// Get hourly data for a given filter
        /// </summary>
        /// <remarks>Returns HourCnlDataExt [], packaged in ArcDTO, in JSON format.
        /// If a filter is specified by the view, then it must already be loaded into the cache</remarks>
        public (HourCnlData[] Data, long[] DataAge) GetHourCnlData(int year, int month, int day, int startHour, int endHour, string cnlNums, string viewIDs, int viewID, bool existing, string dataAge)
        {
            var request = new RestRequest("GetHourCnlData", Method.GET)
                .AddParameter("year", year, ParameterType.GetOrPost)
                .AddParameter("month", month, ParameterType.GetOrPost)
                .AddParameter("day", day, ParameterType.GetOrPost)
                .AddParameter("startHour", startHour, ParameterType.GetOrPost)
                .AddParameter("endHour", endHour, ParameterType.GetOrPost)
                .AddParameter("cnlNums", cnlNums, ParameterType.GetOrPost)
                .AddParameter("viewIDs", viewIDs, ParameterType.GetOrPost)
                .AddParameter("viewID", viewID, ParameterType.GetOrPost)
                .AddParameter("existing", existing, ParameterType.GetOrPost)
                .AddParameter("dataAge", dataAge, ParameterType.GetOrPost);
            var resp = Execute<ArcResp<HourCnlData[], long[]>>(request);
            return (resp.Data, resp.DataAge);
        }
        /// <summary>
        /// Get events according to the specified filter
        /// </summary>
        /// <remarks>Returns DispEventProps [], packaged in ArcDTO, in a format in JSON.
        /// If a filter is specified by the view, then it must already be loaded into the cache</remarks>
        public (DispEvent[] Data, long DataAge) GetEvents(int year, int month, int day, string cnlNums, string viewIDs, int viewID, int lastCount, int startEvNum, long dataAge)
        {
            var request = new RestRequest("GetEvents", Method.GET)
                .AddParameter("year", year, ParameterType.GetOrPost)
                .AddParameter("month", month, ParameterType.GetOrPost)
                .AddParameter("day", day, ParameterType.GetOrPost)
                .AddParameter("cnlNums", cnlNums, ParameterType.GetOrPost)
                .AddParameter("viewIDs", viewIDs, ParameterType.GetOrPost)
                .AddParameter("viewID", viewID, ParameterType.GetOrPost)
                .AddParameter("lastCount", lastCount, ParameterType.GetOrPost)
                .AddParameter("startEvNum", startEvNum, ParameterType.GetOrPost)
                .AddParameter("dataAge", dataAge, ParameterType.GetOrPost);
            var resp = Execute<ArcResp<DispEvent[], long>>(request);
            return (resp.Data, resp.DataAge);
        }
        /// <summary>
        /// Get the view label from the cache
        /// </summary>
        /// <remarks>Returns long, packed in DataTransferObject, in JSON format</remarks>
        public long GetViewStamp(int viewID)
        {
            var request = new RestRequest("GetViewStamp", Method.GET)
                .AddParameter("viewID", viewID, ParameterType.GetOrPost);
            var resp = Execute<Resp<long>>(request);
            return resp.Data;
        }
        /// <summary>
        /// Convert a string to a date
        /// </summary>
        /// <remarks>Returns a long or null packed in the DataTransferObject, in a format in JSON.
        /// Number means the number of milliseconds to create a date in Javascript or 0 if an error occurs</remarks>
        public long ParseDateTime(string s)
        {
            var request = new RestRequest("ParseDateTime", Method.GET)
                .AddParameter("s", s, ParameterType.GetOrPost);
            var resp = Execute<Resp<long>>(request);
            return resp.Data;
        }

        private T Execute<T>(IRestRequest request) where T : IResp
        {
            Logger.Info($"Calling {request.Resource} ...");
            var rest_resp = Client.Execute<DataString>(request);
            if (rest_resp.ErrorException != null)
                throw new ApplicationException("Error retrieving response. Check inner details for more info.", rest_resp.ErrorException);
            var rapid_resp = JsonConvert.DeserializeObject<T>(rest_resp.Data.d);
            if (!rapid_resp.Success)
                throw new RapidException(rapid_resp.ErrorMessage);
            Logger.Info($"Call to {request.Resource} OK");
            return rapid_resp;
        }
    }
}
