/** 
 * 对日期进行格式化， 
 * @param date 要格式化的日期 
 * @param format 进行格式化的模式字符串
 *     支持的模式字母有： 
 *     y:年, 
 *     M:年中的月份(1-12), 
 *     d:月份中的天(1-31), 
 *     h:小时(0-23), 
 *     m:分(0-59), 
 *     s:秒(0-59), 
 *     S:毫秒(0-999),
 *     q:季度(1-4)
 * @return String
 * 例子：{{time | dateFormat:'yyyy年MM月dd日 hh时mm分ss秒'}} --> 2015年12月10日 16时18分15秒
 */
template.helper('dateFormat', function (date, format) {

    /**/date = new Date(date);
    var ary = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二"]
    var map = {
        "Y": ary[date.getMonth() + 1],
        "M": date.getMonth() + 1, //月份 
        "d": date.getDate(), //日 
        "h": date.getHours(), //小时 
        "m": date.getMinutes(), //分 
        "s": date.getSeconds(), //秒 
        "q": Math.floor((date.getMonth() + 3) / 3), //季度 
        "S": date.getMilliseconds() //毫秒 
    };
    format = format.replace(/([yMdhmsqSY])+/g, function (all, t) {
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
});
//图片地址链接组建img图片
template.helper("ImgsHelper", function (a) {
    var imgHtml = '';
    if (a) {
        var imgArr = a.split(",");
        for (var i = 0; i < imgArr.length; i++) {
            if (imgArr[i]) {
                imgHtml += "<img src='" + imgArr[i] + ",1,480,480,3' />";
            }
        }
    }
    return imgHtml;
});
//逗号分隔的图片地址取第一张
template.helper("ProductImgsHelper", function (a) {
    return a.split(",")[0];
});
template.helper('dateFormatNow', function (date, format) {

    /**/date = new Date(date);

    var diffDays = (new Date() - date) / 3600 / 1000 / 24;
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

    var ary = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二"]
    var map = {
        "Y": ary[date.getMonth() + 1],
        "M": date.getMonth() + 1, //月份 
        "d": date.getDate(), //日 
        "h": date.getHours(), //小时 
        "m": date.getMinutes(), //分 
        "s": date.getSeconds(), //秒 
        "q": Math.floor((date.getMonth() + 3) / 3), //季度 
        "S": date.getMilliseconds() //毫秒 
    };
    format = format.replace(/([yMdhmsqSY])+/g, function (all, t) {
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
});

//格式化小数点位数
template.helper('toFixed', function (a, b) {
    return (parseFloat(a)).toFixed(b);
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
//解码
template.helper('decode', function (a) {
    return decodeURIComponent(a);
});
template.helper('getProductImgs', function (a) {
    if (a) {
        var b = a.split(',')[0];
        if (b) { return b; }
    }
    return '/Skin/t/Img/nologo.png';
});
//app/n转换成br换行
template.helper('appBrChange', function (a) {
    return a.replace(/\n/g, '<br/>');
});

