<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Web.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>苏州康力-2015年抽奖活动</title>
    <link href="css/imgbox.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        #lottery {
            width: auto;
            height: auto;
            margin: 20px auto 0;
            /*background: url(images/bg.jpg) no-repeat;*/
            padding: 50px 55px;
        }

            #lottery table td {
                width: 142px;
                height: 142px;
                text-align: center;
                vertical-align: middle;
                font-size: 24px;
                color: #333;
                font-index: -999;
            }

        #start {
            width: 284px;
            height: 284px;
            line-height: 150px;
            display: block;
            text-decoration: none;
        }

        #lottery table td.active {
            background-color: #ea0000;
        }
    </style>

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery.imgbox.js"></script>
    <script type="text/javascript">

        $(function () {
            $("#example1").imgbox({
                'speedIn': 2,
                'fixwidth': 300,
                'fixheight': 500,
                'fixtop': 200,
                'fixleft': 100
            });
            $("#example2").imgbox({
                'speedIn': 2,
                'fixwidth': 300,
                'fixheight': 500,
                'fixtop': 200,
                'fixleft': 700
            });
        });
     
        var lottery = {
            createNew: function(){
                var cat = {};
                cat.index = -1,	//当前转动到哪个位置，起点位置
                cat.count = 0,	//总共有多少个位置
                cat.timer = 0,	//setTimeout的ID，用clearTimeout清除
                cat.speed = 20,	//初始转动速度
                cat.times = 0,	//转动次数
                cat.runing = false,	//转动基本次数：即至少需要转动多少次再进入抽奖环节
                cat.prize = -1,	//中奖位置
                cat.init = function (id) {
                    if ($("#" + id).find(".lottery-unit").length > 0) {
                        $lottery = $("#" + id);
                        $units = $lottery.find(".lottery-unit");
                        this.obj = $lottery;
                        this.count = $units.length;
                        $lottery.find(".lottery-unit-" + this.index).addClass("active");
                    };
                },
                cat.roll = function () {
                    var index = this.index;
                    var count = this.count;
                    var lottery = this.obj;
                    $(lottery).find(".lottery-unit-" + index).removeClass("active");
                    index += 1;
                    if (index > count - 1) {
                        index = 0;
                    };
                    $(lottery).find(".lottery-unit-" + index).addClass("active");
                    this.index = index;
                    return false;
                },
                cat.stop = function (index) {
                    this.prize = index;
                    alert(this.prize);
                    clearTimeout(lottery.timer);
                    return false;
                }

                return cat;
            }
        };
        var lotterys = new Array(0)
        lotterys.push(lottery.createNew());
        lotterys.push(lottery.createNew());
        lotterys.push(lottery.createNew());

        function _roll(_lottery) {
            return function () {
                roll(_lottery);
            }
        }
        function roll(_lottery) {
            if (!_lottery.runing) {
                return false;
            }
            _lottery.times += 1;
            _lottery.roll();
            _lottery.timer = setTimeout(_roll(_lottery), lottery.speed);
            var myDate = new Date();
            $("#mydiv").text(myDate.toLocaleTimeString());
            return false;
        }
        function start() {
            var speed = 3000;
            for (var i = 0, _lottery; _lottery = lotterys[i++];) {
                if (!_lottery.runing) {
                    _lottery.init('lottery');
                    _lottery.speed = speed;
                    _lottery.runing = true;
                    roll(_lottery);
                    speed += 30;
                }
            }
        }
        function stop() {
            for (var i = 0, _lottery; _lottery = lotterys[i++];) {
                _lottery.runing = false;
                _lottery.stop(_lottery.index);
            }
        }

        var click = false;
        window.onload = function () {
            for (var i = 0, _lottery; _lottery = lotterys[i++];) {
                _lottery.init('lottery');
            }
            $("#start").click(function () {
                if (click) {
                    stop();
                    click = false;
                } else {
                    start();
                    click = true;
                }
                return false;
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="lottery">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="lottery-unit lottery-unit-0"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/1.png" /></a></td>
                    <td class="lottery-unit lottery-unit-1"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/2.png" /></a></td>
                    <td class="lottery-unit lottery-unit-2"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/4.png" /></a></td>
                    <td class="lottery-unit lottery-unit-3"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/3.png" /></a></td>
                </tr>
                <tr>
                    <td class="lottery-unit lottery-unit-4"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/1.png" /></a></td>
                    <td class="lottery-unit lottery-unit-5"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/2.png" /></a></td>
                    <td class="lottery-unit lottery-unit-6"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/4.png" /></a></td>
                    <td class="lottery-unit lottery-unit-7"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/3.png" /></a></td>
                </tr>
                <tr>
                    <td class="lottery-unit lottery-unit-8"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/7.png" /></a></td>
                    <td colspan="2" rowspan="2"><a id="start" href="#">点我开始吧</a></td>
                    <td class="lottery-unit lottery-unit-9"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/5.png" /></a></td>
                </tr>
                <tr>
                    <td class="lottery-unit lottery-unit-10"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/1.png" /></a></td>
                    <td class="lottery-unit lottery-unit-11"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/6.png" /></a></td>
                </tr>
                <tr>
                    <td class="lottery-unit lottery-unit-12"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/3.png" /></a></td>
                    <td class="lottery-unit lottery-unit-13"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/6.png" /></a></td>
                    <td class="lottery-unit lottery-unit-14"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/8.png" /></a></td>
                    <td class="lottery-unit lottery-unit-15"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/7.png" /></a></td>
                </tr>
                <tr>
                    <td class="lottery-unit lottery-unit-16"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/3.png" /></a></td>
                    <td class="lottery-unit lottery-unit-17"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/6.png" /></a></td>
                    <td class="lottery-unit lottery-unit-18"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/8.png" /></a></td>
                    <td class="lottery-unit lottery-unit-19"><a href="images/3793633099_3e1e53e4ac_o.jpg"><img src="images/7.png" /></a></td>
                </tr>
            </table>
        </div>
        <div id="mydiv" style="text-align: center; margin: 50px 0; font: normal 14px/24px 'MicroSoft YaHei';">
            dsfdsf
        </div>
    </form>
</body>
</html>
