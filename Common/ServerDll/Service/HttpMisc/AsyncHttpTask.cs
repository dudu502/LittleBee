using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.HttpMisc
{
    public class AsyncHttpTask
    {
        public static async void HttpGetRequest<RESULT>(string url,Action<RESULT> onResult,Action<Exception> onError = null)
        {
            var task = HttpGetRequestAsync(url, onError);
            if(task!=null)
            {
                try
                {
                    WebResponse result = await task;
                    if (onResult != null)
                    {
                        string strRet = await ParseResponseToString(result);
                        RESULT jsonData = LitJson.JsonMapper.ToObject<RESULT>(strRet);
                        onResult.Invoke(jsonData);
                    }
                }
                catch(Exception exc)
                {
                    onError?.Invoke(exc);
                }                
            }                     
        }
        public static async void HttpGetRequest(string url,Action<string> onResult,Action<Exception> onError=null)
        {
            var task = HttpGetRequestAsync(url, onError);
            if (task != null)
            {
                try
                {
                    WebResponse result = await task;
                    if (onResult != null)
                    {
                        string strRet = await ParseResponseToString(result);
                        onResult.Invoke(strRet);
                    }
                }catch(Exception exc)
                {
                    onError?.Invoke(exc);
                }                
            }
        }
        public static Task<string> ParseResponseToString(WebResponse webResponse)
        {
            return Task.Run(()=> {
                using (Stream myResponseStream = webResponse.GetResponseStream())
                {
                    using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8")))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        myStreamReader.Close();
                        myResponseStream.Close();
                        return retString;
                    }
                }
            });
        }
        public static Task<WebResponse> HttpGetRequestAsync(string url,Action<Exception> onError = null)
        {
            string retString = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1000;
            request.Method = "GET";
            request.ContentType = "application/json;charset=UTF-8";
            return request.GetResponseAsync();
        }      
    }
}
