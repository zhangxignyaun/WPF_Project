using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.HttpClients
{
    /// <summary>
    /// 调用Api工具类
    /// </summary>
    public class HttpResClient
    {
        
        public readonly string baseUrl = "http://localhost:2333/api/";//请求地址公共部分
        public HttpResClient()
        {
            
        }
        public ApiResponse Excute(ApiRequest apiRequest)
        {
            RestClient Client = new RestClient() { BaseUrl = new Uri(baseUrl + apiRequest.Route) };//客户端
            RestRequest request = new RestRequest(apiRequest.Method);//请求方式
            request.AddHeader("Content-Type", apiRequest.ContentType);
            if (apiRequest.Parameters !=null)
            {
                //SerializeObject json 序列化
                request.AddParameter("param",JsonConvert.SerializeObject(apiRequest.Parameters),ParameterType.RequestBody);
            }
            var url = baseUrl + apiRequest.Route;
            //Client = new RestClient(url);
            //Client.BaseUrl = new Uri(url);
            var res = Client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)//请求成功
            {
                //DeserializeObject json 反序列化
                var aa = JsonConvert.DeserializeObject<ApiResponse>(res.Content);
                return JsonConvert.DeserializeObject<ApiResponse>(res.Content);
            }
            else
            {
                return new ApiResponse() { ResultCode = -99, Msg ="接口异常" };
            }
        }
    }
}
