using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Extensions
{
    public static class PushNotificationExtension
    {
        public static string SendNotification(string title, string msg)
        {
            string serverKey = "AAAAb4qVgLg:APA91bEg_NuSTKY3o-k3p7IxUQsXqk6M7DadAGu0TV76SL7osHVgkib6AOUE_KQrhtMvGZ6aV3lm-LU9i2sAn3yLLpYb4NonfQbbPIo3zIu3NzivsTj3CzubXILsor50HlWK_1J7Xwrq";
            string senderId = "479066423480";
            string webAddr = "https://fcm.googleapis.com/fcm/send";

            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                to = "/topics/testTopic",
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = msg,
                    pushtype = "testTopic",
                    title = title
                },

                mutableContent = true
            };

            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
