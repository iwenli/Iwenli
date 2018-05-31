/*
    调用：
    jsonp({
        url: _url,
        data: params,
        success: fun
    });
 */

function jsonp(options) {
    var url = options.url;
    var data = options.data || {};
    var fun = options.success || function (result) {
        console.log(result);
    };

    url = url + '?' + buildUri(data);
    url = '/Iwenli/Org/Ajax/OpenRequest.ajax/jsonp?call='
        + encodeURIComponent(url) + '_=' + new Date() * 1;
    $.getJSON(url, fun);
}

function buildUri(data) {
    var uri = '';
    for (var key in data) {
        if (isEmpty(data[key])) {
            uri += encodeURIComponent(key) + '=' + encodeURIComponent(data[key]) + '&'
        }
        else {
            uri += buildUri(data[key]);
        }
    }
    return uri;
}

function isEmpty(obj) {
    if (typeof obj == 'string') return true;
    for (var i in obj) {
        return false;
    }
    return true;
}

// 获取6位随机数   
function random() {
    return Math.random().toString(16).substr(2, 6);
}
