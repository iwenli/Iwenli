//分享回调app协议规范   
//统一协议地址：com.txooo.maike93390://myapp/share?
//参数：
//  shareCode | [必有] 邀请码，注册成为下线用
//  type | [必有] 分享类型：1、商品分享    2、店铺分享  3、资讯分享   4、团队圈分享
//  id | [必有] 唯一标识 , 不同类型代表业务唯一id
//  附加业务标识 | [可空] 如果分享出来的URL有，则有，否则没有(key和分享URL统一)，如店铺分享附加show,回调同样使用show

//例：  
//  app分享的url://133332.u.93390.cn/shop_2/mchStore.html?mchid=100000018&show=1&title=%E5%A4%A7%E5%93%81%E7%89%8C&desc=%E9%98%BF%E5%B0%94%E5%93%A6%E7%BA%A2%E7%B1%B3%E5%AF%82%E5%AF%9E%E8%BF%9B%E9%AD%84%E5%8A%9B&imgurl=https%3A%2F%2Fimg.txooo.com%2F2017%2F03%2F21%2F3d2b77bbd88bec34cd09c31da380c638.jpg
//  店铺分享回调：com.txooo.maike93390://myapp/share?shareCode=32351&type=2&id=100000018&show=1

// 1、商品分享 
// //133332.u.93390.cn/shop.html?id=20679&title=%E6%99%BA%E5%8A%9B%E9%97%AE%E7%AD%94%E3%80%90%E6%99%BA%E5%8A%9B%E3%80%91%E4%BA%92%E5%8A%A8%EF%BC%8C%E4%BD%A0%E8%83%BD%E7%AD%94%E5%AF%B9%E5%87%A0%E9%81%93%E9%A2%98%EF%BC%9F&desc=%E6%99%BA%E5%8A%9B%EF%BC%8C%E4%BD%A0%E7%9B%B8%E4%BF%A1%E8%87%AA%E5%B7%B1%E7%9A%84%E6%99%BA%E5%8A%9B%E4%B9%88%EF%BC%9F&imgurl=https%3A%2F%2Fimg.txooo.com%2F2016%2F12%2F26%2F17f560bef41e5db936b541299a8ed0d7.jpg
// 2、店铺分享
// //133332.u.93390.cn/shop_2/mchStore.html?mchid=100000018&show=1&title=%E5%A4%A7%E5%93%81%E7%89%8C&desc=%E9%98%BF%E5%B0%94%E5%93%A6%E7%BA%A2%E7%B1%B3%E5%AF%82%E5%AF%9E%E8%BF%9B%E9%AD%84%E5%8A%9B&imgurl=https%3A%2F%2Fimg.txooo.com%2F2017%2F03%2F21%2F3d2b77bbd88bec34cd09c31da380c638.jpg
// 3、资讯分享
// //133332.u.93390.cn/shop_2/mchnews.html?newsid=354&title=Cheshire%20&desc=%E9%98%BF%E5%B0%94%E5%93%A6%E7%BA%A2%E7%B1%B3%E5%AF%82%E5%AF%9E%E8%BF%9B%E9%AD%84%E5%8A%9B&imgurl=https%3A%2F%2Fimg.txooo.com%2F2017%2F03%2F16%2Fbb937d804615d49b511bcc9be0a5d2ee.jpg
// 4、团队圈分享
// //133332.u.93390.cn/Quan/share.html?id=11331&title=%E8%80%81%E5%86%9C%E6%B0%91%E4%BD%A0&desc=%E6%88%91%E6%98%AF%E5%88%9B%E4%B8%9A%E8%B5%9A%E9%92%B1%E4%B9%88%E4%B9%88%E5%93%92%21&imgurl=

(function () {
    var openUrl = '';
    //sessionStorage['Tx-IsSharePage'] = 'true';//location.search.indexOf('imgurl') > 0;
    //分享注册成为下线
    function ShareReg(type, shareCode, shareUrl) {
        sessionStorage['Tx-ShareSource'] = type;
        sessionStorage['Tx-ShareCode'] = shareCode;
        //注册回跳页面区分二维码和正常分享路径，二维码跳转到首页，其他跳转到来源页面
        sessionStorage['Tx-ShareUrl'] = location.search.indexOf('imgurl') > 0 ? shareUrl : location.hostname;
    }
    //分享URL跳转app当前页
    function Init(type) {
        var args = getUrlArgObject();
        var callBackParams = '';
        for (var item in args) {
            if (item != 'imgurl' && item != 'title' && item != 'desc') {
                callBackParams = callBackParams + '&' + item + '=' + args[item];
            }
        }
        //获取邀请码
        var shareCodeStr = location.hostname.split('.')[0];
        shareCodeStr = shareCodeStr.length == 1 ? '0' : shareCodeStr;
        var shareCode = parseInt(shareCodeStr);
        ShareReg(type, shareCode, location.href.split('?')[0] + '?ac=shareReg' + callBackParams);
        //将第一个参数替换为id
        callBackParams = callBackParams.replace(location.search.substr(1, location.search.indexOf('=') - 1), 'id');
        openUrl = 'com.txooo.maike93390://myapp/share?shareCode=' + shareCode + '&type=' + type + callBackParams;
    }
    function openApp() {
        //console.log(openUrl);
        //埋点 调取协议
        var count = 1;
        var timer = setInterval(function () {
            var docA = document.createElement('a');
            docA.href = openUrl;
            docA.id = 'j-openApp';
            //添加class=external  禁用sui路由
            docA.className = 'external';
            document.body.appendChild(docA);
            document.getElementById("j-openApp").click();
            console.log(openUrl);
            setTimeout(function () {
                document.body.removeChild(docA);
            }, 100);

            count--;
            if (count == 0) {
                clearInterval(timer);
            }
        }, 200);

        ////在iframe 中打开APP
        //var ifr = document.createElement('iframe');
        //ifr.src = openUrl;
        //ifr.style.display = 'none';
        //document.body.appendChild(ifr);
        //setTimeout(function () {
        //    document.body.removeChild(ifr);
        //}, 100);

        ////通过localtion
        //location.href = openUrl;
    }
    //获取参数集合
    function getUrlArgObject() {
        var args = new Object();
        var query = location.search.substring(1);//获取查询串
        var pairs = query.split("&");//在逗号处断开
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('=');//查找name=value
            if (pos == -1) {//如果没有找到就跳过
                continue;
            }
            var argname = pairs[i].substring(0, pos);//提取name
            var value = pairs[i].substring(pos + 1);//提取value
            args[argname] = unescape(value);//存为属性
        }
        return args;//返回对象
    }

    //待后期增加微信 QQ  微博内打开跳转提示时使用
    var browser = {
        versions: function () {
            var u = navigator.userAgent, app = navigator.appVersion;
            return {         //移动终端浏览器版本信息
                trident: u.indexOf('Trident') > -1, //IE内核
                presto: u.indexOf('Presto') > -1, //opera内核
                webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或uc浏览器
                iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
                iPad: u.indexOf('iPad') > -1, //是否iPad
                webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
            };
        }(),
        language: (navigator.browserLanguage || navigator.language).toLowerCase(),  //语言
        mobileType: function () {  //手机端打开平台
            var ua = navigator.userAgent.toLowerCase();//获取判断用的对象
            return {
                isWinXin: ua.match(/MicroMessenger/i) == "micromessenger",
                isWeibo: ua.match(/weibo/i) == "weibo",
                isQQ: ua.match(/qq\//i) == "qq/"
            }
        }()
    };

    var util = {
        // 动态加载css文件
        loadStyles: function (url) {
            var link = document.createElement("link");
            link.type = "text/css";
            link.rel = "stylesheet";
            link.href = url;
            document.getElementsByTagName("head")[0].appendChild(link);

        },
        // 动态加载js脚本文件
        loadScript: function (url) {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = url;
            document.body.appendChild(script);
        }
    };

    var userParam = {
        ready: function () {
            openApp();
        },
        init: function (tyep) {
            Init(tyep);
        }
    };
    window.Browser = browser;
    window.jOpengApp = userParam.ready;
    window.jOpengApp.Init = userParam.init;
    window.jOpengApp.Util = util;
})();


// 分享类型
function getShareType() {      // 判断类型
    var type = 0;
    if (location.href.match(/\w+\.html/i)) {
        switch (location.href.match(/\w+\.html/i)[0].toLocaleLowerCase()) {
            case 'shop.html':
                type = 1;         // 1、商品分享
                break;
            case 'mchstore.html':
                type = 2;         // 2、店铺分享
                break;
            case 'mchnews.html':
                type = 3;         // 3、资讯分享
                break;
            case 'share.html':
                type = 4;         // 4、团队圈分享
                break;
        }
    }
    return type;
}

//整体入口
function openTxappByShare() {
    //先初始化脚本
    var type = getShareType();
    if (type < 1 || type > 4) { return; }
    jOpengApp.Init(type);
    ////如果不是分享页面
    //if (sessionStorage['Tx-IsSharePage'] != 'true') { return; }
    //如果是PC不执行
    if (!Browser.versions.mobile) { return; }
    //如果是空间|微信|微博不执行
    if (Browser.mobileType.isWinXin || Browser.mobileType.isWeibo || Browser.mobileType.isQQ) {
        openAppPrompt();
        return;
    }
    //如果是APP内置页面不执行
    //第1版
    if (typeof (txservice) == 'object' && txservice.success()) { return; }
    //第2版
    if (typeof (txapp) == 'object' && txapp.isApp()) { return; }
    //拉取app
    jOpengApp();
};

//$(window).on("load", function () {
//    ////测试期间 统计userAgent  上线前删除
//    //var _ip = location.href + 'local-host' + navigator.userAgent;
//    //$.post("http://iwenli.org/api/0.axd", { msg: _ip }, function (d) {
//    //});
//    var type = getShareType();
//    if (type < 1 || type > 4) { return; }

//});

function openAppPrompt() {
    console.log('拉取遮罩成功');
    //动态加载需要的css
    jOpengApp.Util.loadStyles('/Skin/t/Css/txappOpenShare.css?_r=1');
    var openModalBtnDom = document.createElement('div');
    openModalBtnDom.className = 'openModal';
    openModalBtnDom.innerHTML = '<span class="openModalBtn">APP内打开</span>';
    document.body.appendChild(openModalBtnDom);

    var openModalBtn = document.getElementsByClassName('openModalBtn')[0];
    openModalBtn.onclick = function () {
        modal_layer();
        openModalBtn.style.display = 'none';
    };
}

//遮罩
function modal_layer() {
    function createDom(type) {
        return document.createElement(type);
    }
    //'<img src="/Skin/t/Img/arrows.png"/>' 
    var str = '/Skin/t/Img/live_weixin';
    if (Browser.versions.ios) str += '_ios';
    var html = '<img src="' + str + '.png"/>';
    /* +
        '<div class="one">点击右上角的 <span><i></i><i></i><i></i></span> 按钮</div>' +
        '<div class="two">选择 在浏览器中打开</div>';*/

    var maskCont = createDom('div');
    maskCont.className = 'mask-cont';
    maskCont.innerHTML = html;

    var closeDom = createDom('div');
    closeDom.className = 'close-btn';

    var maskLayer = createDom('div');
    maskLayer.className = 'mask-layer';
    maskLayer.appendChild(maskCont);
    maskLayer.appendChild(closeDom);

    document.body.appendChild(maskLayer);

    var closeBtn = document.getElementsByClassName('close-btn')[0];

    closeBtn.onclick = function () {
        document.body.removeChild(maskLayer);
    };
}