using F88.Digital.Application.Constants;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Extensions
{
    public class ResponseApiData : ResponseApi
    {
        public object data { get; set; }

        public string status { get; set; }
    }

    public class ResponseApi
    {
        public int code { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
    public class ResponseShareServiceApiData
    {
        public string data { get; set; }
        public int error_code { get; set; }
        public string error_message { get; set; }
        public string error_detail { get; set; }
    }
    public class ResponseShareServiceApi
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }

    public static class RestApiPerform
    {
        /// <summary>
        /// Rests the API get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="dicCriterial">The dic criterial.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T RestApiGet<T>(string apiUrl, Dictionary<string, string> dicCriterial)
        {
            var obj = (ResponseApiData)RestApiGet(apiUrl, dicCriterial);
            T result = default(T);

            if (obj.code == ApiConstants.Code.Success)
            {
                result = JsonConvert.DeserializeObject<T>(obj.data.ToString());
            }
            else if (obj.code == ApiConstants.Code.NotFound)
            {
                return result;
            }
            else
            {
                throw new Exception(obj.message);
            }

            return result;
        }
        public static T RestApiGetWithHeader<T>(string apiUrl, string header ,Dictionary<string, string> dicCriterial)
        {
            var obj = (ResponseShareServiceApiData)RestApiGetWithHeader(apiUrl, header, dicCriterial);
            T result = default(T);

            if (obj.error_code == ApiConstants.Code.Success)
            {
                result = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj).ToString());
            }
            else if (obj.error_code == ApiConstants.Code.NotFound)
            {
                return result;
            }
            else
            {
                throw new Exception(obj.error_message);
            }

            return result;
        }
        /// <summary>
        /// Rests the API get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl">The API URL.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T RestApiGetWithBearerToken<T>(string apiUrl, string bearerToken)
        {
            var obj = (ResponseApiData)RestApiGetWithBearerToken(apiUrl, bearerToken);
            T result;

            if (obj.code == ApiConstants.Code.Success)
            {
                result = JsonConvert.DeserializeObject<T>(obj.data.ToString());
            }
            else
            {
                throw new Exception();
            }

            return result;
        }

        #region ---code maintainance---
        ///// <summary>
        ///// Rests the API get single value.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="apiUrl">The API URL.</param>
        ///// <param name="dicCriterial">The dic criterial.</param>
        ///// <returns></returns>
        ///// <exception cref="System.Exception"></exception>
        //public static T RestApiGetSingleValue<T>(string apiUrl, Dictionary<string, string> dicCriterial)
        //{
        //    var obj = (ResponseApiData)RestApi.RestApiGet(apiUrl, dicCriterial);
        //    T result = default(T);

        //    if (obj.code == ApiConstants.Code.Success)
        //    {
        //        result = (T)(obj.data);
        //    }
        //    else if (obj.code == ApiConstants.Code.NotFound)
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }

        //    return result;
        //}



        ///// <summary>
        ///// Rests the API get list.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="apiUrl">The API URL.</param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static IEnumerable<T> RestApiGetList<T>(string apiUrl)
        //{
        //    var obj = (ResponseApiData)RestApi.RestApiGet(apiUrl);

        //    if (obj.code == ApiConstants.Code.Success)
        //    {
        //        return JsonConvert.DeserializeObject<List<T>>(obj.data.ToString());
        //    }

        //    if (obj.code == ApiConstants.Code.Fail)
        //    {
        //        throw new TimeoutException();
        //    }

        //    return default(List<T>);
        //}
        //public static IEnumerable<T> RestApiPostList<T>(string apiUrl, Dictionary<string, string> dicCriterial)
        //{
        //    var obj = (ResponseApiData)RestApi.RestApiPost(apiUrl, dicCriterial);

        //    if (obj.code == ApiConstants.Code.Success)
        //    {
        //        return JsonConvert.DeserializeObject<List<T>>(obj.data.ToString());
        //    }

        //    if (obj.code == ApiConstants.Code.Fail)
        //    {
        //        throw new TimeoutException();
        //    }

        //    throw new Exception();
        //}
        //public static IEnumerable<T> RestApiGetList<T>(string apiUrl, Dictionary<string, string> dicCriterial)
        //{
        //    var obj = (ResponseApiData)RestApi.RestApiGet(apiUrl, dicCriterial);

        //    if (obj.code == ApiConstants.Code.Success)
        //    {
        //        return JsonConvert.DeserializeObject<List<T>>(obj.data.ToString());
        //    }

        //    if (obj.code == ApiConstants.Code.Fail)
        //    {
        //        throw new TimeoutException();
        //    }

        //    throw new Exception();
        //}

        ///// <summary>
        ///// Rests the API put.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="apiUrl">The API URL.</param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static T RestApiPut<T>(string apiUrl, Dictionary<string, string> dctParams = null)
        //{
        //    var obj = (ResponseApiData)RestApi.RestApiPut(apiUrl, dctParams);
        //    T result;

        //    if (obj.code == ApiConstants.Code.Success)
        //    {
        //        result = JsonConvert.DeserializeObject<T>(obj.data.ToString());
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }

        //    return result;
        //}

        //public static object RestApiPostPOS(string url, object obj, string posUser, string posKey)
        //{
        //    try
        //    {
        //        ResponseApiData objResponse = new ResponseApiData();
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiRootUrl"]);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Add("apiUsername", posUser);
        //            client.DefaultRequestHeaders.Add("secretKey", posKey);
        //            //var content = new FormUrlEncodedContent(new[]
        //            //{
        //            //     new KeyValuePair<string, string>("Name", "F88 Hội sở")
        //            //});

        //            var jsonRequest = JsonConvert.SerializeObject(obj);
        //            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

        //            var response = client.PostAsync(url, content).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string responseString = response.Content.ReadAsStringAsync().Result;

        //                objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
        //            }
        //        }
        //        return objResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static string TCP_NOTIFY_POL(byte type, object obj)
        //{
        //    try
        //    {
        //        var jsonRequest = JsonConvert.SerializeObject(obj);
        //        var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
        //        string[] arr_param = { jsonRequest };
        //        var ret = Notify.Send(type, arr_param);

        //        return ret.data;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static string TCP_NOTIFY_POL_FEADBACK(int obj)
        //{
        //    try
        //    {
        //        //var jsonRequest = JsonConvert.SerializeObject(obj);
        //        //var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
        //        //string[] arr_param = { jsonRequest };
        //        var ret = Notify.Send(NOTI_TYPE.FEADBACK_NVKD_AFF, obj);

        //        return ret.data;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        public static object RestApiGet(string url, Dictionary<string, string> dctParams)
        {
            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30);
                if (!url.Contains("?"))
                {
                    BuildUrlParams(ref url, dctParams);
                }       
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return obj;
        }
        public static object RestApiGetWithHeader(string url, string header, Dictionary<string, string> dctParams)
        {
            ResponseShareServiceApiData obj = new ResponseShareServiceApiData();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", header);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30);
                //if (!url.Contains("?"))
                //{
                //    BuildUrlParams(ref url, dctParams);
                //}
                var response = client.PostAsJsonAsync(url, dctParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseShareServiceApiData>(responseString);
                }
            }

            return obj;
        }

        public static ResponseApiData RestApiPost(string url, object obj)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(10);          
                
                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
             
                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }
        public static ResponseApiData RestApiPostWithHeader(string url, string header,object obj)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            ResponseApiData objResponse = new ResponseApiData();
            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Add("x-api-key", header);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(10);

                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                }
            }

            return objResponse;
        }
        private static void BuildUrlParams(ref string url, Dictionary<string, string> dctParams)
        {
            int index = 0;
            if (dctParams != null && dctParams.Count > 0)
            {
                foreach (var item in dctParams)
                {
                    if (index == 0)
                    {
                        url += "?" + item.Key + "=" + item.Value;
                    }
                    else
                    {
                        url += "&" + item.Key + "=" + item.Value;
                    }
                    index += 1;
                }
            }
        }

        private static object RestApiGetWithBearerToken(string url, string bearerToken)
        {

            ResponseApiData obj = new ResponseApiData();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", bearerToken));
                client.Timeout = TimeSpan.FromMinutes(10);

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseApiData>(responseString);
                    obj.code = Convert.ToInt32(response.StatusCode);
                }
            }

            return obj;
        }
    }
}
