$.ajaxSetup({ cache: false, contentType: "application/x-www-form-urlencoded; charset=utf-8", error: function () { } });

function getUrlParam(paras) {
    var url = location.search.length > 0 ? location.search.substring(1) : ""
    var paraString = url.split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

/** 
* 时间对象的格式化 
*/
function formatTime(date, format) {
    /* 
    * format="yyyy-MM-dd hh:mm:ss"; 
    */
    var myDate = new Date(date);
    var year = myDate.getFullYear();
    var month = ("0" + (myDate.getMonth() + 1)).slice(-2);
    var day = ("0" + myDate.getDate()).slice(-2);
    var h = ("0" + myDate.getHours()).slice(-2);
    var m = ("0" + myDate.getMinutes()).slice(-2);
    var s = ("0" + myDate.getSeconds()).slice(-2);
    if (format == true) {
        return year + "-" + month + "-" + day + " " + h + ":" + m;
    }
    if (format == "ymd") {
        return year + "年" + month + "月" + day + "日";
    }
    if (format == "day") {
        var diffDays = (new Date() - myDate) / 3600 / 1000 / 24;
        return parseInt(diffDays);
    }
    if (format == "now") {
        var diffDays = (new Date() - myDate) / 3600 / 1000 / 24;
        if (diffDays < 1) {
            diffDays = diffDays * 24;
            if (diffDays < 1) {
                diffDays = diffDays * 60;
                if (diffDays < 1) {
                    return '刚刚';
                }
                return parseInt(diffDays) + '分钟前';
            }
            return parseInt(diffDays) + '小时前';
        } else if (diffDays < 7) {
            return parseInt(diffDays) + '天前';
        }
        return year + "-" + month + "-" + day;
    }

    return year + "-" + month + "-" + day;
}

//获取用户的基本信息
function GetUserInfo(callback) {
    //try {
    //从缓存中获取
    //if (window.localStorage.ZQUSERSTORE) {
    //    var userStore = eval(window.localStorage.ZQUSERSTORE)[0];           
    //    if (callback) { callback(userStore); }
    //} else {
    var u = '';
    //数据库获取
    GetUserInfoByDb(callback, u);
    //    }
    //} catch (e) {
    //    window.localStorage.clear();
    //}
};
function GetUserInfoByDb(callback, userid) {
    $.get('/Txooo/SalesV2/Member/Ajax/UserAjax.ajax/GetUserInfo', 'userid=' + userid, function (data) {
        if (data != 'nouser') {
            if (!userid) { window.localStorage.ZQUSERSTORE = data; }
            if (callback) { callback(eval(data)[0]); }
        }
    });
};

//按钮倒计时
function btnInterval(btn) {
    var count = 60;
    btn.val(count + '秒后重新发送');
    var countdown = function () {
        btn.val(count - 1 + '秒后重新发送');
        if (count == 0) {
            btn.val('获取验证码').removeAttr('disabled');
            $('.error').html(" ");
            clearInterval(timer);
        }
        count--;
    }
    var timer = setInterval(countdown, 1000);
};


function cookie(name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        var path = options.path ? '; path=' + options.path : '';
        var domain = options.domain ? '; domain=' + options.domain : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};


//获取滚动条当前的位置 
function getScrollTop() {
    var scrollTop = 0;
    if (document.documentElement && document.documentElement.scrollTop) {
        scrollTop = document.documentElement.scrollTop;
    }
    else if (document.body) {
        scrollTop = document.body.scrollTop;
    }
    return scrollTop;
};

//获取当前可是范围的高度 
function getClientHeight() {
    var clientHeight = 0;
    if (document.body.clientHeight && document.documentElement.clientHeight) {
        clientHeight = Math.min(document.body.clientHeight, document.documentElement.clientHeight);
    }
    else {
        clientHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight);
    }
    return clientHeight;
};

//获取文档完整的高度 
function getScrollHeight() {
    return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
};
//退出登录
function LoginOut() {
    window.localStorage.clear();
    $.get('/Txooo/SalesV2/Passport/Ajax/SalesAjax.ajax/LoginOut', function (data) {
        window.location.href = "//0.u.93390.cn/index.html";//"//passport.93390.cn/Quit.html";
    });
};

//绑定不同浏览器处理函数，可多重绑定
(function (window, $) {

    var user_agent = navigator.userAgent.toLowerCase();

    var wxAry = [], appAry = [], browserAry = [];

    window.opensource = {};

    window.opensource.wx = {
        bind: function (cb) {
            wxAry.push(cb);
        }
    };

    window.opensource.app = {
        bind: function (cb) {
            appAry.push(cb);
        }
    };

    window.opensource.browser = {
        bind: function (cb) {
            browserAry.push(cb);
        }
    };

    window.opensource.init = function () {

        if (user_agent.indexOf("micromessenger") > -1) {
            $.each(wxAry, function (i, v) {
                v();
            });
            return;
        }

        if (user_agent.indexOf("txooo.app") > -1) {
            if (window.plus) {
                $.each(appAry, function (i, v) {
                    v();
                });
            } else {
                document.addEventListener("plusready", function () {
                    $.each(appAry, function (i, v) {
                        v();
                    });
                }, false);
            }
            return;
        }

        $.each(browserAry, function (i, v) {
            v();
            return;
        });
    }

})(window, jQuery);

function dialogAlart(msg, callback, title) {
    title = title || '买客';
    callback = callback || function () { };
    var d = dialog({
        content: msg,
        title: title,
        width: document.body.clientWidth * 70 / 100,
        okValue: '好的',
        ok: callback
    }).showModal();
    //setTimeout(function () {
    //    d.close().remove();
    //}, 2000);
};
function dialogComfirm(msg, callback) {
    dialog({
        title: '买客',
        content: msg,
        width: document.body.clientWidth * 70 / 100,
        okValue: '确定',
        ok: function () {
            callback();
            //return false;
        },
        cancelValue: '取消',
        cancel: function () { }
    }).showModal();

};


//初始化js api对象（调用app时)
(function (window) {
    if (!window.txservice) {
        window.txservice = {};
    }
    //注册事件监听
    window.txservice.connectWebViewJavascriptBridge = function (callback) {
        if (window.WebViewJavascriptBridge) {
            callback(WebViewJavascriptBridge)
        } else {
            document.addEventListener(
                'WebViewJavascriptBridgeReady'
                , function () {
                    callback(WebViewJavascriptBridge)
                },
                false
            );
        }
    };

    //外部判断是否可调用APP交互
    window.txservice.success = function () {
        if (navigator.userAgent.toLowerCase().indexOf("txooo.app") > -1) { return true; }
        else { return false; }
    };

    //初始化app与JS交互
    window.txservice.init = function () {
        console.log('开始初始化');
        if (window.txservice.success()) {
            window.txservice.connectWebViewJavascriptBridge(function (bridge) {
                bridge.init(function (message, responseCallback) {
                    window.txservice.initCallback(message);
                });
                window.txservice.register(bridge);
            });
        }
    };

    //初始化window的JS成功回调    
    window.txservice.initCallback = function (msg) {
        console.log('app交互初始化成功');
    };

    //初始化js开放函数，按照下面的格式重写    
    window.txservice.register = function (bridge) {
        bridge.registerHandler("functionInJs", function (data, responseCallback) {
            //调用app的方法
            responseCallback(data);
        });
    };

    //调用本地app   
    window.txservice.call = function (url, data, fun) {
        if (window.txservice.success()) {
            window.txservice.connectWebViewJavascriptBridge(function (bridge) {
                bridge.callHandler(url, data, fun);
            });
            //window.WebViewJavascriptBridge.callHandler(url, data, fun);
        } else { console.log('app交互未初始化'); }
    };
})(window);

//页面加载完后执行
(function () {
    if (typeof (openTxappByShare) == 'function') { openTxappByShare(); }
    sessionStorage['Tx-ShareCode'] = location.hostname.split('.')[0];

    function URLencode(sStr) {
        return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F').replace(/\#/g, '%23');
    }
    $(function () {
        if (window.txservice.success()) {
            $('.P_Header').addClass('appPage')
            $('body').addClass('appBody')
        }
    });
    //下载提示操作
    function downloadDiv(fun) {
        if (window.txservice.success()) {
            $('.download').remove();
        } else {
            if (window.sessionStorage.CloseDownLoad != "true") $('.download').css('display', 'block');
            else $('.download').remove();
            $(document).off('click', '.download i').on('click', '.download i', function () {
                window.sessionStorage.CloseDownLoad = "true";
                $('.download').remove();
                if (fun) fun();
            });
        }
    };
    //页面加载完后执行
    $(function () {
        if ($('.download').length == 0) {
            $('body').append($('.download_div').html());
        }
        downloadDiv();
        $(document).off('click', '.download_app').on('click', '.download_app', function () {
            publicDownLoadApp();
            return false;
        });
        //隐藏下载提示
        $(document).off('click', '.download>p>i').on('click', '.download>p>i', function () {
            $('.download').hide();
            //$('.content').css('top', '2.2rem');
            return false;
        });
    });
    //去下载app提醒
    function publicDownLoadApp() {
        if (window.txservice.success() == false) {
            if (userIsAuth()) {
                window.location.href = '//www.93390.cn/Index_wap.html';
            } else {
                publicGoLogin();
            }
            return false;
        }
        return true;
    };
    //去登录
    function publicGoLogin() {
        if (window.txservice.success()) {
            window.txservice.call('login');
        } else {
            sessionStorage['Tx-ShareCode'] = location.hostname.split('.')[0];
            if (userIsAuth()) {  //如果登陆不执行
                return;
            }
            var _host = '//passport.93390.cn/';
            var _page = 'Register.html?';
            //if (location.href.indexOf('.t.7518') > 0) {  //测试站 修改host
            //    _host = "//p.93390.cn/";
            //}
            var _url = _host + _page; //unescape

            var returnUrl = location.href;
            //如果是分享的页面 携带当前信息跳转到注册页面  || 商品分享使用

            if (sessionStorage['Tx-ShareSource'] != null && sessionStorage['Tx-ShareUrl'] != null) {
                returnUrl = URLencode(sessionStorage['Tx-ShareUrl']);
            }
            location.href = _url + 'shareCode=' + sessionStorage['Tx-ShareCode'] + '&ReturnUrl=' + returnUrl;
            return false;
        }
    };

})();
//页面加载时用户是否登陆
function userIsAuth() {
    if ($('#user_is_auth').val() == 'false') { return false; }
    return true;
};
function openBindByThridShareCode(fun) {
    $.get("/Txooo/SalesV2/Member/Ajax/MemberAjaxV3.ajax/GetUserThridShareCode", function (data) {
        if (fun) fun(eval(data).msg == '' ? 'code' : 'mobile');
    });
}
var imageProcessParams = ',1,250,250,3'; //图片处理参数
template.helper('headPic', function (data) {
    return data.length > 0 ? data + imageProcessParams : '/Skin/t/Img/no_pic.png';
});
