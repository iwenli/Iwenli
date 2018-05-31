//初始化js api对象（调用app时)
(function (window) {
    //内部
    var tx_init_service = {
        //初始化成功
        state: {
            init: false,
            config: false,
            configKeys: null,
            app: function () {
                if (navigator.userAgent.toLowerCase().indexOf("txooo.app") > -1) {
                    return true;
                } else {
                    return false;
                }
            }
        },
        //注册事件监听
        connectWebViewJavascriptBridge: function (callback) {
            tx_init_service.debugfun('正在初始化');
            if (window.WebViewJavascriptBridge) {
                callback(WebViewJavascriptBridge);
            } else {
                document.addEventListener('WebViewJavascriptBridgeReady',
                    function () {
                        callback(WebViewJavascriptBridge);
                    },
                    false);
            }
        },
        ready: function (callback) {
            if (!callback) return false;
            if (tx_init_service.state.app()) {
                try {
                    tx_init_service.connectWebViewJavascriptBridge(function (bridge) {
                        callback(bridge);
                    });
                } catch (e) {
                    tx_init_service.debugfun(e.message);
                }
            } else {
                tx_init_service.debugfun('请在客户端打开');
            }
        },
        //初始化app与JS交互
        init: function () {
            tx_init_service.connectWebViewJavascriptBridge(function (bridge) {
                tx_init_service.debugfun('通道初次初始化信息');
                for (var i = 0; i < txapp.registerApiList.length; i++) {
                    tx_init_service.debugfun('注册app事件-' + (i + 1) + '，方法：' + txapp.registerApiList[i].funName);
                    bridge.registerHandler(txapp.registerApiList[i].funName, function (data, responseCallback) {
                        tx_init_service.debugfun('完成注册方法' + txapp.registerApiList[i].funName);
                        responseCallback({ success: true });
                        txapp.registerApiList[i].funCallBack(data);
                    });
                }
                bridge.init(function (message, responseCallback) {
                    tx_init_service.initCallback(message);
                });
                tx_init_service.state.config = true;
                tx_init_service.state.configKeys = 'asdf';
                tx_init_service.configCallBack();
                //window.WebViewJavascriptBridge.callHandler('config', tx_init_service.config, function (res) {
                //    var obj = txapp.parseJSON(res);
                //    if (obj.success) {
                //        tx_init_service.state.config = true;
                //        tx_init_service.state.configKeys = obj.msg;
                //        tx_init_service.configCallBack();
                //    } else {
                //        tx_init_service.error(obj.msg);
                //    };
                //});
            });
        },
        //初始化window的JS成功回调
        initCallback: function (msg) {
            tx_init_service.debugfun('第一次交互成功：' + msg);
            tx_init_service.state.init = true;
        },
        //增加配置信息
        config: {
            debug: false,
            appId: '',
            timestamp: '',
            signature: ''
        },
        //配置信息验证后
        configCallBack: function (obj) { },
        //初始化成功后，被用户定义的立刻执行的函数
        readyArray: [],
        //初始化错误后，被用户重新定义的函数
        error: function (res) { },
        //用户触发调用本地app
        call: function (url, data) {
            try {
                if (tx_init_service.state.app() && tx_init_service.state.config) {
                    data = txapp.extend({
                        success: function (res) { },
                        cancel: function (res) { }
                    }, data || {});
                    data.keys = tx_init_service.state.configKeys;
                    window.WebViewJavascriptBridge.callHandler(url, data,
                        function (res) {
                            tx_init_service.debugfun('app的' + url + '执行完毕，返回参数：' + res);
                            var obj = txapp.parseJSON(res);
                            if (obj.success) {
                                data.success(obj);
                            } else {
                                data.cancel(obj);
                            }
                        });
                } else {
                    tx_init_service.debugfun('交互条件不满足，原因是 是否APP环境' + tx_init_service.state.app() + '。是否初始化' + tx_init_service.state.init + '。是否有配置' + tx_init_service.state.config);
                }
            } catch (e) {
                console.log(e.message, e);
            }
        },
        debugfun: function (msg) {
            if (tx_init_service.config.debug) {
                txapp.debugfun(msg);
            }
        }
    };
    if (!window.txapp) {
        window.txapp = {};
    }
    //对外接口
    window.txapp = {
        send: function (callback) {
            if (callback) {
                tx_init_service.initCallback = callback;
            }
        },
        //调试提醒函数
        debugfun: function (msg) {
            console.log(msg);
            alert(msg);
        },
        parseJSON: function (data) {
            return eval('(' + data + ')');
        },
        extend: function (target, source) {
            for (var i in source) {//不使用过滤
                target[i] = source[i];
            }
            return target;
        },
        //定义外部要使用的api列表
        //callApiList: [],
        //定义外部要注册的api列表，格式[{funName: 'functionInJs',funCallBack: function () { }}]
        registerApiList: [],
        //增加配置信息
        config: function (data) {
            tx_init_service.config = txapp.extend(tx_init_service.config, data || {});
            if (tx_init_service.state.init || tx_init_service.state.config) {
                return;
            }
            tx_init_service.init();
        },
        //外部判断是否可调用APP交互
        isApp: function () {
            return tx_init_service.state.app();
        },
        //初始化成功后，需要执行的函数
        ready: function (callback) {
            tx_init_service.ready(callback);
        },
        //初始化错误后，需要执行的函数
        error: function (callback) {
            if (callback) {
                tx_init_service.error = callback;
            }
        },
        //手动添加脚本中不存在的功能性函数
        addToolFun: function (funName, data) {
            tx_init_service.call(funName, data);
        },
        /*
         * ===============================================================
         *  以下为功能性公共函数，特定需求可使用txpp.addToolFun来实现
         * ===============================================================
         */
        //微信支付
        wxPay: function (data) {
            tx_init_service.call("wxpay", data);
        },
        //阿里支付
        aliPay: function (data) {
            tx_init_service.call("alipay", data);
        },
        //分享
        share: function (data) {
            tx_init_service.call('share', data);
        },
        //返回上一页，某人非app环境自动返回上一页
        goBack: function (data) {
            if (txapp.isApp()) {
                tx_init_service.call('goback', data);
            } else {
                history.go(-1);
            }
        }
    };
})(window);