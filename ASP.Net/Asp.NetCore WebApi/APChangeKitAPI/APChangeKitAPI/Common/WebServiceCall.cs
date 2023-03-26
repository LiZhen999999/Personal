using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace APChangeKitAPI.Common
{
    public class WebServiceCall
    {
        public WebServiceCall()
        {
        }

        public static string _mUrl = string.Empty;
        public WebServiceCall(string url)
        {
            _mUrl = url;
        }

        /// <summary>
        /// 调用接口
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="param">参数名称</param>
        /// <returns>返回值</returns>
        public string callWebService(string methodName, Dictionary<string, string> param)
        {
            ///获取请求数据
            byte[] data = getRequestData(methodName, param); // getRequestData(methodName, param);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_mUrl);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            string mSoapAction = "http://tempuri.org/" + methodName;
            request.Headers.Add("SOAPAction", mSoapAction);
            request.ContentLength = data.Length;
            Stream rStream = request.GetRequestStream();
            rStream.Write(data, 0, data.Length);
            rStream.Close();

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();

            dataStream.Close();
            response.Close();
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public byte[] getRequestData(string methodName, Dictionary<string, string> param)
        {
            StringBuilder requestData = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>")
                   .Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">")
                   .Append("  <soap:Body>")
                   .Append("<").Append(methodName)
                   .Append(" xmlns=\"http://tempuri.org/\">");
            foreach (KeyValuePair<string, string> item in param)
            {
                requestData.Append("<").Append(item.Key).Append(">")
                .Append(item.Value)
                .Append("</").Append(item.Key).Append(">");
            }
            requestData.Append("</").Append(methodName).Append(">")
            .Append("</soap:Body>")
            .Append("</soap:Envelope>");
            string val = requestData.ToString();
            byte[] data = Encoding.UTF8.GetBytes(val);
            return data;
        }

        ///  <summary>
        ///  远程证书验证
        ///  </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
