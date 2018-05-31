function ajaxError(errorType) {
    if (errorType === 'timeout') {
        $.toast('请求超时，请重试！');
        console.log('请求超时，请重试！');
    }
    if (errorType === 'abort') {
        $.toast('网络异常，请查看网络是否连接！');
        console.log('网络异常，请查看网络是否连接！');
    }
}
//公共页面加载监听
function publicPageInit(id, fun, ready, register) { 
    //加载全局ajax错误配置   10秒超时
    $.ajaxSettings = $.extend($.ajaxSettings, {
        timeout: 10000,
        //cache: false,
        //beforeSend: function () {
        //    $.showIndicator();
        // $.hideIndicator();
        //},
        error: function (xhr, errorType, error) {
            ajaxError(errorType);
        },
        complete: function (xhr, status) {
            if ($('.modal-in').css('display') === 'block') {
                $.hidePreloader();
            }
            //console.log('ok！');
        }
    });
    $(document).on("pageInit", id, function (e, id, page) {
        if ($(page).find('.download').length == 0) {
            $(page).append($('.download_div').html());
        }
        downloadDiv();
         
        fun(e, id, page);
        if (isWxUserAgent()) {
            wxShare.title = $('.wx_share_content .share_title').val();
            wxShare.desc = $('.wx_share_content .share_content').val();
            wxShare.link = window.location.href.replace('/shop/details.html', '/shop.html');
            wxShare.imgUrl = $('.wx_share_content .share_img').val();
            wxShare.accountType = 'sales';
            wxShare.init();
        }
        var _hmt = _hmt || [];
        (function () {   //modifi by zhangyulong  at 2017年6月12日17:35:58  防止多次注入页面
            if (document.getElementById('baidu-script') == null) {
                var hm = document.createElement("script");
                hm.id = 'baidu-script';
                hm.src = "//hm.baidu.com/hm.js?0cfd4b17fbf55da167db2804208645ee";
                var s = document.getElementsByTagName("script")[0];
                s.parentNode.insertBefore(hm, s);
            }
        })();
    });
};
//模板替换
function jsparse(template, data) {
    return template.replace(/\{([\w\.]*)\}/g, function (str, key) {
        var keys = key.split("."), v = data[keys.shift()];
        for (var i = 0, l = keys.length; i < l; i++) v = v[keys[i]];
        return (typeof v !== "undefined" && v !== null) ? v : "";
    });
};
//生成年月
function createYearMonth() {
    var year_month = [];
    var today_year = new Date().getFullYear();//获取今年
    for (var i = today_year; i > 2015; i--) {
        for (var j = 12; j > 0; j--) {
            year_month.push(i + '年' + j + '月')
        }
    }
    return year_month;
};
//格式化小数部分
function formatMoney(a) {
    var money = parseFloat(a).toFixed(2);
    if (money >= 10000) {
        return parseFloat((money / 10000)).toFixed(1) + '万';
    }
    return money;
}
//格式化时间
function dateFormat(dateTime, format, diy) {
    if (dateTime) {
        var date = new Date(dateTime);
        if (diy == 'diy') {
            var diffDays = (new Date($('#sys_service_time').val()) - date) / 3600 / 1000 / 24;
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
            } else if (diffDays < 3) {
                return parseInt(diffDays) + '天前';
            }
        }
        var map = {
            "M": date.getMonth() + 1, //月份 
            "d": date.getDate(), //日 
            "h": date.getHours(), //小时 
            "m": date.getMinutes(), //分 
            "s": date.getSeconds(), //秒 
            "q": Math.floor((date.getMonth() + 3) / 3), //季度 
            "S": date.getMilliseconds() //毫秒 
        };
        format = format.replace(/([yMdhmsqS])+/g, function (all, t) {
            var v = map[t];
            if (v !== undefined) {
                if (all.length > 1) {
                    v = '0' + v;
                    v = v.substr(v.length - 2);
                }
                return v;
            }
            else if (t === 'y') {
                return (date.getFullYear() + '').substr(4 - all.length);
            }
            return all;
        });
        return format;
    }
}

//活得当前周(周日计第一天)
function getCurrentWeekDays() {
    var _date = new Date();
    var _week = _date.getDay();
    _date = addDate(_date, _week * -1);

    var _days = [];
    for (var i = 0; i < 7; i++) {
        var _day = i == 0 ? _date : addDate(_date, 1);
        _days.push(_day.toLocaleDateString());
    }
    return _days;
}
function addDate(date, n) {
    date.setDate(date.getDate() + n);
    return date;
};

//获得地址参数
function getUrlParam(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
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
};
var imageProcessParams = ',1,250,250,3'; //图片处理参数
template.helper('dateFormat', function (data, format, diy) {
    return dateFormat(data, format, diy);
});
template.helper('productImgs', function (data) {
    return getOneProductImg(data);
});
template.helper('headPic', function (data) {
    return data.length > 0 ? data + imageProcessParams : '/Skin/t/Img/no_pic.png';
});
template.helper('noLogo', function (data) {
    return data.length > 0 ? data + imageProcessParams : '/Skin/t/Img/nologo.png';
});
template.helper('replaceBomContent', function (data) {
    return data.replace(/\ufeff/g, '');
});
template.helper('replaceBrContent', function (data) {
    return data.replace(/\n/g, '<br/>');
});
template.helper('bbsClickLike', function (data, bbsid) {
    if (window.localStorage["bbs_click_like_" + bbsid] == 1) {
        return 1;
    }
    return data;
});
template.helper('newsClickLike', function (data, newsid) {
    if (window.localStorage["news_click_like_" + newsid] == 1) {
        return 1;
    }
    return data;
});
template.helper('userNewsClickLike', function (data, postsid) {
    if (window.localStorage["user_news_click_like_" + postsid] == 1) {
        return 1;
    }
    return data;
});
//格式化小数
template.helper('toFloat', function (a) {
    var money = parseFloat(a);
    if (money >= 10000) {
        return parseFloat((money / 10000)).toFixed(1) + '万';
    } else {
        return (parseFloat(a));
    }
});
//格式化小数点位数
template.helper('priceToFixed', function (a) {
    return (parseFloat(a)).toFixed(2);
});
//获得一张图片
function getOneProductImg(data) {
    var array = data.split(',');
    return $.trim(array).length > 0 ? array[0] + imageProcessParams : '/Skin/t/Img/nologo.png';
};
//小数格式化百分数
template.helper("percentage", function (a) {
    return Math.round(a * 100) + '%'
});

//生成商品主图展示  最多取前3个
template.helper("productImages", function (a) {
    var imgHtml = '';
    if (a) {
        var imgArr = a.split(",");
        var len = imgArr.length > 3 ? 3 : imgArr.length;
        for (var i = 0; i < len; i++) {
            if (imgArr[i]) {
                //imgHtml += '<img class="col-33 lazy" ></img>';
                imgHtml += '<img  class="col-33 lazy"data-original="' + imgArr[i] + imageProcessParams + '"/>';
            }
        }
    }
    return imgHtml;
});
//生成商品距离
template.helper("productDistance", function (a) {
    var html = '';
    if (a) {
        if (a > 1000) {
            html = '<span class="col-50 pull-right"><i>&#xe63d;</i>' + (parseFloat(a / 1000)).toFixed(2) + 'km</span>';
        } else {
            html = '<span class="col-50 pull-right"><i>&#xe63d;</i>' + (parseFloat(a)).toFixed(2) + 'km</span>';
        }
    }
    return html;
});
//商品评价数量格式化
template.helper('formatReplyCount', function (a) {
    var count = parseInt(a);
    if (count > 999) {
        return '999+';
    } else {
        return count;
    }
});
//商品评价图片展示
template.helper("reviewImages", function (a) {
    var imgHtml = '';
    if (a) {
        var imgArr = a.split(",");
        if (imgArr.length == 0) { return imgHtml; }
        imgHtml = '<ul class="pic_list">';
        for (var i = 0; i < imgArr.length; i++) {
            if (imgArr[i]) {
                imgHtml += '<li><img class="review-img lazy" data-index="' + i + '" data-id="' + imgArr[i] + '" data-original="' + imgArr[i] + imageProcessParams + '" alt="商品评价Image" /></li>'
            }
        }
        imgHtml += '</ul>';
    }
    return imgHtml;
});
//首页模板图片展示
template.helper("templageImgs", function (a) {
    var imgHtml = '';
    if (a) {
        var imgArr = a.split(",");
        if (imgArr.length == 0) { return imgHtml; }
        if (imgArr.length == 1) {
            return '<img class="img lazy" data-original="' + imgArr[0] + imageProcessParams + '" />';

        }
        imgHtml = '<div class="row row-two">';
        imgArr.length = 2; //防止图片个数多余2个
        for (var i = 0; i < imgArr.length; i++) {
            if (imgArr[i]) {
                imgHtml += '<img class="img lazy" data-original="' + imgArr[i] + imageProcessParams + '" />';
            }
        }
        imgHtml += '</div>';
    }
    return imgHtml;
});
//商品规格是否可选择判断
template.helper("ProductPropertyIsSelect", function (parent, child, data) {
    var _pList = $.grep(data, function (o) { return o.PropertyName == parent; });
    if (_pList.length > 0) {
        if ($.grep(_pList[0].ChildList, function (y) { return y.PropertyName == child; }).length > 0) {
            return '';
        }
    }
    return 'un_click'
});

$.page = {};
function firstLoad(url) {
    if ($.router.cache[url]) {
        if ($.page[url]) {
            return false;
        } else {
            $.page[url] = true;
            return true;
        }
    }
    return true;
};
//异步加载不走缓存
function routerLoad(url) {
    $.page[window.location.origin + url] = false;
    $.router.load(url, true);
};
//页面加载时用户是否登陆
function userIsAuth() {
    return false;
    if ($('#user_is_auth').val() == 'false') { return false; }
    return true;
};
//验证非空表单，表单标识、每次提醒
function requiredForm(form, warnFun) {
    var _result = '';
    var array = $(form).serialize().split('&');
    for (var i = 0; i < array.length; i++) {
        var para = array[i].split('=');
        if (para[1] == '') {
            var msg = $(form).find('[name=' + para[0] + ']').data('msg');
            if (!msg) msg = para[0];
            _result = _result + ',' + msg;
        }
    }
    if (_result) {
        _result = _result.substr(1, _result.length) + '不能为空';
        if (warnFun) warnFun(_result);
        else $.toast(_result);
        return false;
    }
    return true;
};
//下载提示操作
function downloadDiv(fun) {
    if (window.sessionStorage.CloseDownLoad != "true") $('.download').css('display', 'block');
    else $('.download').remove();
    $(document).off('click', '.download i').on('click', '.download i', function () {
        window.sessionStorage.CloseDownLoad = "true";
        $('.download').remove();
        if (fun) fun();
    });
};
//页面加载完后执行
(function () {
    $(function () {
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
})();
//去下载app提醒
function publicDownLoadApp() {
    if (userIsAuth()) {
        window.location.href = '//www.93390.cn/Index_wap.html';
    } else {
        publicGoLogin();
    }
    return false;
};
//去登录
function publicGoLogin(retUrl) {
    sessionStorage['Tx-ShareCode'] = location.hostname.split('.')[0];
    if ($('#is_auth').val() == "true") {  //如果登陆不执行
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
    else {
        _url = _host + 'login.html?';
    }
    location.href = _url + 'shareCode=' + sessionStorage['Tx-ShareCode'] + '&ReturnUrl=' + returnUrl;
    return false;
};
//url特殊字符处理(&# \/=...)
function URLencode(sStr) {
    return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F').replace(/\#/g, '%23');
}
//执行配置信息去登录
function publicConfigLogin() {
    window.location.href = $('#config_login_page_url').val() + encodeURIComponent(window.location.href);
        //SUI跳转登录页面会弹错误窗，登录页面回跳不需要sui效果，暂时改href调  by 张玉龙  at 2017年6月6日09:44:59
        //$.router.load($('#config_login_page_url').val() + encodeURIComponent(window.location.href));
    return false;
};
//按钮倒计时
function btnInterval(btn) {
    var count = 60;
    btn.text(count + '秒后重新发送').addClass('btn_disabled');
    var countdown = function () {
        btn.text(count - 1 + '秒后重新发送');
        if (count == 0) {
            btn.text('获取验证码').removeClass('btn_disabled');
            clearInterval(timer);
        }
        count--;
    }
    var timer = setInterval(countdown, 1000);
};

//空账户会员绑定手机输入邀请人
function memberBindCode(page, bindFun) {
    //点击跳过邀请人步骤
    $(page).off('click', '.go_to_mobile').on('click', '.go_to_mobile', bindFun);
    //检测输入手机号
    $(page).off('keyup', '.input_share_code').on('keyup', '.input_share_code', function () {
        if ($(this).val().replace(/ /g, '').length > 6) {
            $(page).find('.get_share_code').removeClass('disabled');
        } else {
            $(page).find('.get_share_code').addClass('disabled');
        }
    });
    //下一步
    $(page).off('click', '.get_share_code').on('click', '.get_share_code', function () {
        var _mobile = $(page).find('.input_share_code').val().replace(/ /g, '');
        if (_mobile == '') { return; }
        getShareCodeByTxId({ mobile: _mobile }, function (obj) {
            if (obj.success == 'false') {
                $.toast(obj.msg);
            } else {
                parameters.member.bind.shareCode = obj.msg;
                bindFun();
            }
        });
    });
};
//空账户会员绑定手机输入验证码
function memberBindMobile(page, fun) {
    sendMobileCodeEvent(page, function () {
        //提交手机号
        $(page).off('click', '.sub_bind_mobile').on('click', '.sub_bind_mobile', function () {
            var _code = $(page).find('.input_mobile_code').val();
            if (_code == '') { return; }
            setSecurityPhoneByThird({ mobilecode: _code }, function (obj) {
                if (obj.errcode == -1) {
                    $.toast(obj.msg);
                } else {
                    $.closeModal();
                    $.toast('绑定成功');
                    if (fun) fun(obj);
                }
            });
        });
    });
};

function sendMobileCodeEvent(page, fun) {
    //检测验证码输入
    $(page).off('keyup', '.input_mobile_code').on('keyup', '.input_mobile_code', function () {
        if ($(this).val().replace(/ /g, '').length > 0) {
            $(page).find('.sub_bind_mobile').removeClass('disabled');
        } else {
            $(page).find('.sub_bind_mobile').addClass('disabled');
        }
    });
    //获取短信验证码
    $(page).off('click', '.get_msg_code').on('click', '.get_msg_code', function () {
        sendMobileCodeAfter('msg', function () { });
    });
    //获得语音验证码
    $(page).off('click', '.get_voice_code').on('click', '.get_voice_code', function () {
        sendMobileCodeAfter('voice', function () {
            $.alert('请注意接听电话');
        });
    });
    function sendMobileCodeAfter(type, fun) {
        if ($(page).find('.get_msg_code').hasClass('btn_disabled')) {
            $.toast('倒计时结束后，可再次点击发送验证码');
            return false;
        }
        sendMobileCode({ send_type: type, mobile: $(page).find('.user_mobile').val() }, function (obj) {
            if (obj.success == 'true') {
                btnInterval($(page).find('.get_msg_code'));
                fun();
            } else {
                $.toast(obj.msg);
            }
        });
    };
    if (fun) fun();
}

//检测打开绑定手机的弹框步骤
function openBindByThridShareCode(fun) {
    getUserThridShareCode(function (obj) {
        openBindPhoneModal(obj.msg == '' ? 'code' : 'mobile', fun);
    });
};
//打开绑定手机的弹框
function openBindPhoneModal(type, fun) {
    var _modal;
    $('.bind_mobile_html_div').load(txapp.appHostPath + '/member_2/bind/' + type + '.html', function (data) {
        $.closeModal();
        _modal = $.modal({
            extraClass: "bind_mobile_modal",
            text: data
        });
        switch (type) {
            case 'code':
                memberBindCode('.bind_mobile_modal', function () {
                    $.closeModal(_modal);
                    openBindPhoneModal('mobile', fun);
                });
                break;
            case 'mobile':
                memberBindMobile('.bind_mobile_modal', fun);
                break;
        }
    });
    $(document).off('click', '.modal-overlay').on('click', '.modal-overlay', function () {
        $.closeModal(_modal);
    });
};
//获得用户第三方登录时的推广码
function getUserThridShareCode(fun) {
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjaxV3.ajax/GetUserThridShareCode", function (data) {
        if (fun) fun(eval(data));
    });
};

//获得用户邀请码
function getShareCodeByTxId(params, fun) {
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Passport/Ajax/SalesAjax.ajax/GetShareCodeByTxId", params, function (data) {
        if (fun) fun(eval(data));
    })
};
//绑定手机号到空账户中
function setSecurityPhoneByThird(params, fun) {
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjaxV3.ajax/SetSecurityPhoneByThird", params, function (data) {
        if (fun) fun(JSON.parse(data));
    })
};
//发送手机验证码
function sendMobileCode(params, fun) {
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjaxV3.ajax/SendMobileCode", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//是否是微信客户端
function isWxUserAgent() {
    if (navigator.userAgent.toLowerCase().indexOf("micromessenger") > -1) {
        return true;
    }
    return false;
};