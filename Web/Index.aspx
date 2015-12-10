<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="css/m.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="css/n.css" type="text/css" media="screen" />    <script type="text/javascript" src="js/jq.js"></script>
    <script type="text/javascript" src="js/m.js"></script>
    <title></title>
    <script type="text/javascript">
        ref_time = 12;//刷新时间
        run = false;//是否进入就播放
        init_qrcode = false;//是否进入显示二维码
        lid = "";//起始id
        colors = "";//颜色
        site_name = "";
        wechat_name = "";
        act_word = "";
        re_luck = "";
        vote_auto_zoom = "";
    </script>
</head>
<body>
    <div class="main">
        <!-- 头部 -->
        <div class="head">
            <div class="head_left">
                <div class="head_info">
                    <h1>site_name</h1>
                </div>
                <div class="head_flag"></div>
            </div>

            <div class="head_right">
                <img alt="bababa" src="img/bullhorn.png" />
                <h2>添加</h2>
                <h1>wechat_name</h1>
                <h3>发送<span>act_word 你想说的话</span>即可上墙</h3>
            </div>
            <div class="clear"></div>
        </div>
        <!-- 内容区 -->
        <div class="contents">
            <ul id="items">
            </ul>
        </div>

    </div>
    <!-- 底部控制 -->
    <div id="control">
        <div class="ctrl_button" title="显示/隐藏" id="ctrl_hide">
            <img alt="qrcode" src="img/arrow-left.png" /></div>

        <div class="ctrl_button" title="二维码" id="ctrl_qrcode">
            <img alt="qrcode" src="img/qrcode-ico.png" /></div>
        <div class="ctrl_button" title="开始/暂停" id="ctrl_run">
            <img alt="run" src="img/play.png" /></div>
        <div class="ctrl_button" title="手动更新" id="ctrl_ref">
            <img alt="ref" src="img/spinner.png" /></div>
        <div class="ctrl_button" title="抽奖" id="ctrl_luck">
            <img alt="luck" src="img/spinner2.png" /></div>
        <div class="ctrl_button" title="实时投票" id="ctrl_vote">
            <img alt="vote" src="img/vote.png" /></div>
        <em class="load_text right">载入中……</em>
    </div>
    <!-- 抽奖层 -->
    <div id="luck">
        <div>
            <img class="close_luck right" title="关闭" alt="close" src="img/close.png" />
            <div class="goodluck">
                <h1>幸运抽奖</h1>
                <ul id="luck_now">
                </ul>
                <button class="btn_ex" id="luck_start">开始</button>
                <button class="btn_ex" id="luck_com">确认</button>
                <ul id="luck_result">
                </ul>
                <button class="btn_ex" id="luck_clear">清空结果</button><b id="luck_count" class="right"></b>
            </div>
        </div>
    </div>
    <!-- 投票层 -->
    <div id="vote">
        <div>
            <div class="vote_head">
                <div class="head">
                    <div class="head_left">
                        <div class="head_info">
                            <h1>vote_name</h1>
                        </div>
                        <div class="head_flag"></div>
                    </div>
                    <div class="head_right">
                        <img alt="bababa" src="img/bullhorn.png" />
                        <h2>添加</h2>
                        <h1>wechat_name</h1>
                        <h3>发送<span> vote_key </span>即可投票</h3>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div id="vote_ctrl" class="right">
                <img id="order_vote" class="bnt_vote" title="排序" alt="order_vote" src="img/numbered-list.png" />
                <img id="ref_vote" class="bnt_vote" title="刷新" alt="ref_vote" src="img/spinner.png" />
                <img id="close_vote" class="bnt_vote" title="关闭" alt="close" src="img/close.png" />

            </div>

            <div id="vote_result">
                <ul id="vote_result_ul">
                    <li id="vote_explain" class="vote_item" title="100%">
                        <div class="vote_bar">
                            <div style="height: 100%; border-bottom: 2px rgba(10,106,54,.8) solid">
                                <div class="vote_num">0</div>
                                <div class="vote_bar_o" style="background: rgba(10,106,54,.8)"></div>
                            </div>
                        </div>
                        <div class="vote_id">︵编号︶</div>
                        <div class="vote_name">总数</div>
                    </li>
                </ul>
            </div>

        </div>
    </div>
    <!-- 二维码图片 -->
    <div id="qrcode">
        <div>
            <img class="close_qrcode right" title="关闭" alt="close" src="img/close.png" />
            <img class="qrcode_big" alt="qrcode" src="img/qrcode.jpg" />
        </div>
    </div>
    <!-- 背景图  -->
    <img class="bg" src="img/bg.jpg" alt="bg" />
</body>
</html>
