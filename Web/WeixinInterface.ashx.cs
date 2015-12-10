using Codeplex.Data;
using Deepleo.Weixin.SDK;
using Deepleo.Weixin.SDK.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

namespace Web
{
    public class WeixinInterface : IHttpHandler
    {
        HttpContext context = null;
        string echostr;
        public static TokenHelper tokenhelper = new TokenHelper(3600, AppConfig.AppId, AppConfig.AppSecret);
        public void ProcessRequest(HttpContext param_context)
        {
            context = param_context;
            echostr = context.Request["echoStr"];
            if (!string.IsNullOrWhiteSpace(echostr))
            {
                valid();//用于验证
            }
            else
            {
                if (context.Request.HttpMethod.ToLower() == "post")
                {
                    WeixinMessage message = null;
                    using (var streamReader = new StreamReader(context.Request.InputStream))
                    {
                        message = AcceptMessageAPI.Parse(streamReader.ReadToEnd());
                    }
                    var response = new WeixinExecutor().Execute(message);
                    //context.Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
                    //context.Response.ContentType = "text/xml";
                    context.Response.Write(response);
                    context.Response.End();
                }
            }
        }

        public void valid()
        {
            var signature = context.Request["signature"].ToString();
            var timestamp = context.Request["timestamp"].ToString();
            var nonce = context.Request["nonce"].ToString();
            var token = AppConfig.Token;
            if (string.IsNullOrEmpty(token))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("请先设置Token！");
                context.Response.End();//推送...不然微信平台无法验证token
                return;
            }
            var ent = "";
            if (!BasicAPI.CheckSignature(signature, timestamp, nonce, token, out ent))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("参数错误");
                context.Response.End();//推送...不然微信平台无法验证token
                return;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(echostr);
            context.Response.End();//推送...不然微信平台无法验证token
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}