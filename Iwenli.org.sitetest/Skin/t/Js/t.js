
//初始化js api对象（调用app时)
(function (window, $) {
    if (navigator.userAgent.toLowerCase().indexOf("txooo.app") > -1) {
        var txservice = {
            share: function (Argus, callback) {
               return OCModel.share(Argus, callback);
            }
        };
        window.txservice = txservice;
    }

})(window, jQuery);

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