using DBUntity;
using Deepleo.Weixin.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;

namespace Web
{
    public class WeixinExecutor : IWeixinExecutor
    {
        public WeixinExecutor()
        {
        }

        public string Execute(WeixinMessage message)
        {
            var result = string.Empty;
            switch (message.Type)
            {
                case WeixinMessageType.Text:
                    {
                        result = RepayText(message.Body);
                    }
                    break;
                case WeixinMessageType.Image:
                    {
                        result = RepayImage(message.Body);
                    }
                    break;
                case WeixinMessageType.Event:
                    #region event
                    string eventType = message.Body.Event.Value.ToLower();
                    string eventKey = message.Body.EventKey.Value;
                    string openId = message.Body.FromUserName.Value;
                    var myUserName = message.Body.ToUserName.Value;
                    switch (eventType)
                    {
                        case "subscribe":
                            result = transferText(message.Body, "欢迎订阅");
                            break;
                        case "unsubscribe":
                            result = transferText(message.Body, "欢迎再来");
                            break;
                        case "scan":
                            result = transferText(message.Body, "欢迎使用");
                            break;
                        case "location"://用户进入应用时记录用户地理位置
                            #region location
                            var lat = message.Body.Latitude.Value.ToString();
                            var lng = message.Body.Longitude.Value.ToString();
                            var pcn = message.Body.Precision.Value.ToString();
                            //在此处将经纬度记录在数据库
                            #endregion
                            break;
                        case "click":
                            switch (eventKey)//refer to： Recources/menu.json
                            {
                                case "myaccount":
                                    #region 我的账户
                                    result = transferText(message.Body, "我的账户.");
                                    #endregion
                                    break;
                                default:
                                    result = string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName>" +
                                         "<FromUserName><![CDATA[{1}]]></FromUserName>" +
                                         "<CreateTime>{2}</CreateTime>" +
                                         "<MsgType><![CDATA[text]]></MsgType>" +
                                         "<Content><![CDATA[{3}]]></Content>" + "</xml>",
                                         openId, myUserName, DateTime.Now.ToBinary(), "没有响应菜单事件");
                                    break;
                            }
                            break;
                    }
                    #endregion
                    break;
                default:
                    result = string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName>" +
                                         "<FromUserName><![CDATA[{1}]]></FromUserName>" +
                                         "<CreateTime>{2}</CreateTime>" +
                                         "<MsgType><![CDATA[text]]></MsgType>" +
                                         "<Content><![CDATA[{3}]]></Content>" + "</xml>",
                                         message.Body.FromUserName.Value, message.Body.ToUserName.Value, DateTime.Now.ToBinary(), "不支持");
                    break;
            }
            return result;
        }

        private string RepayImage(dynamic body)
        {
            string message = "<xml>" +
                            "< ToUserName >< ![CDATA[{0}]] ></ ToUserName >" +
                            "< FromUserName >< ![CDATA[{1}]] ></ FromUserName >" +
                            "< CreateTime >{2}</ CreateTime >" +
                            "< MsgType >< ![CDATA[image]] ></ MsgType >" +
                            "< Image >< MediaId >< ![CDATA[{3}]]></ MediaId ></ Image >" +
                            "</ xml > ";
            return string.Format(message, body.ToUserName.Value,
                body.FromUserName.Value, DateTime.Now.ToBinary(), body.MediaId.Value);
        }

        private string RepayText(dynamic body)
        {
            string keyword = body.Content.Value.Trim();
            if (keyword.StartsWith("参与抽奖"))
            {
                string UserName = keyword.Replace("参与抽奖", string.Empty);
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    return transferText(body, "名字不可为空");
                }
                if (UserName.Trim().Length > 5)
                {
                    return transferText(body, "名字不可超过5个字（不含空格）");
                }
                try
                {
                    string token = WeixinInterface.tokenhelper.GetToken(false);
                    var userinfo = UserAdminAPI.GetInfo(token, body.FromUserName.Value);
                    DbCommand cmd = DBHelper.Database.GetStoredProcCommand("P_CYFlow");
                    DBHelper.Database.AddInParameter(cmd, "OP", DbType.String, "cycj");
                    DBHelper.Database.AddInParameter(cmd, "WXZH", DbType.String, body.FromUserName.Value);
                    DBHelper.Database.AddInParameter(cmd, "WXName", DbType.String, userinfo.nickname);
                    DBHelper.Database.AddInParameter(cmd, "HeadImgUrl", DbType.String, userinfo.headimgurl);
                    DBHelper.Database.AddInParameter(cmd, "UserName", DbType.String, UserName);
                    DBHelper.Database.ExecuteNonQuery(cmd);
                    //return transferText(body, userinfo.nickname + "谢谢参与");
                    //return transmitImage(body);
                    return transmitNews(body, new List<WeixinNews>()
                    {
                        new WeixinNews() {
                            Description="谢谢您参与本次抽奖活动",
                            PicUrl=userinfo.headimgurl,
                            Title="参与成功",
                            Url="http://www.baidu.com"
                        }
                    });
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Class != 11 || ex.State != 1)
                        return transferText(body, "服务出现异常");
                    return transferText(body, ex.Message);
                }
            }
            else if (keyword == "更改抽奖姓名")
            {
                string UserName = keyword.Replace("更改抽奖姓名", string.Empty);
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    return transferText(body, "名字不可为空");
                }
                if (UserName.Trim().Length > 5)
                {
                    return transferText(body, "名字不可超过5个字（不含空格）");
                }
                try
                {
                    DbCommand cmd = DBHelper.Database.GetStoredProcCommand("P_CYFlow");
                    DBHelper.Database.AddInParameter(cmd, "OP", DbType.String, "ggxm");
                    DBHelper.Database.AddInParameter(cmd, "WXZH", DbType.String, body.FromUserName.Value);
                    DBHelper.Database.AddInParameter(cmd, "WXNo", DbType.String, body.FromUserName.Value);
                    DBHelper.Database.AddInParameter(cmd, "WXName", DbType.String, body.FromUserName.Value);
                    DBHelper.Database.AddInParameter(cmd, "UserName", DbType.String, UserName);
                    DBHelper.Database.ExecuteNonQuery(cmd);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Class != 11 || ex.State != 1)
                        return transferText(body, "服务出现异常");
                    return transferText(body, ex.Message);
                }
                return transferText(body, "更改成功");
            }
            else if (keyword == "查询审核状态")
            {
                return transferText(body, "该功能还未实现");
            }
            else
            {
                return transferText(body, "回复 参与抽奖+姓名 参与本次抽奖");
            }
        }
        private string RepayNews(string toUserName, string fromUserName, List<WeixinNews> news)
        {
            var couponesBuilder = new StringBuilder();
            couponesBuilder.Append(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName>" +
            "<FromUserName><![CDATA[{1}]]></FromUserName>" +
            "<CreateTime>{2}</CreateTime>" +
            "<MsgType><![CDATA[news]]></MsgType>" +
            "<ArticleCount>{3}</ArticleCount><Articles>",
             toUserName, fromUserName,
             DateTime.Now.ToBinary(),
             news.Count
                ));
            foreach (var c in news)
            {
                couponesBuilder.Append(string.Format("<item><Title><![CDATA[{0}]]></Title>" +
                    "<Description><![CDATA[{1}]]></Description>" +
                    "<PicUrl><![CDATA[{2}]]></PicUrl>" +
                    "<Url><![CDATA[{3}]]></Url>" +
                    "</item>",
                   c.Title, c.Description, c.PicUrl, c.Url
                 ));
            }
            couponesBuilder.Append("</Articles></xml>");
            return couponesBuilder.ToString();
        }

        private string transferText(dynamic body, string message)
        {
            return string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName>" +
                                                   "<FromUserName><![CDATA[{1}]]></FromUserName>" +
                                                   "<CreateTime>{2}</CreateTime>" +
                                                   "<MsgType><![CDATA[text]]></MsgType>" +
                                                   "<Content><![CDATA[{3}]]></Content>" + "</xml>",
                                                   body.FromUserName.Value, body.ToUserName.Value, DateTime.Now.ToBinary(), message);
        }


        //回复图文消息
        private string transmitNews(dynamic body, List<WeixinNews> newsarray)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", body.FromUserName.Value);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", body.ToUserName.Value);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", DateTime.Now.ToBinary());
            sb.Append("<MsgType><![CDATA[news]]></MsgType>");
            sb.AppendFormat("<ArticleCount>{0}</ArticleCount>", newsarray.Count);
            sb.Append("<Articles>");
            foreach (WeixinNews item in newsarray)
            {
                sb.Append("<item>");
                sb.AppendFormat("<Title><![CDATA[{0}]]></Title>", item.Title);
                sb.AppendFormat("<Description><![CDATA[{0}]]></Description>", item.Description);
                sb.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl>", item.PicUrl);
                sb.AppendFormat("<Url><![CDATA[{0}]]></Url>", item.PicUrl);
                sb.Append("</item>");
            }
            sb.Append("</Articles>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        //回复图片消息
        private string transmitImage(dynamic body)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", body.FromUserName.Value);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", body.ToUserName.Value);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", DateTime.Now.ToBinary());
            sb.Append("<MsgType><![CDATA[image]]></MsgType>");
            sb.AppendFormat("<Image><MediaId>< ![CDATA[{0}]] ></MediaId></Image>", body.MediaId.Value);
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
    public class WeixinNews
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string PicUrl { set; get; }
        public string Url { set; get; }
    }
}