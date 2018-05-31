!(function (global, factory) {

    "function" == typeof define && (define.amd || define.cmd) ? define(function () {
        return factory(global);
    }) : factory(global);

})(typeof window !== "undefined" ? window : this,
    function (win) {

        var WebBridge, Methods, confDebug,
            configState = {
                state: 0,
                apiList: {}
            };

        var allStateMsg = {
            // "200": "连接成功",      // be connected successfully
            // "201": "连接失败",
            //
            //"202": "调用成功",      // The call is successful
            "203": "调用成功, 但没有返回值",
            "204": "事件没有注册 or 没有响应",
            "205": "registerHandlerg事件名称不符",
            "206": "请使用客户端打开"
        }

        // ready --------------------------------------------------------------------------------
        function ready(callback) {
            if (win.WebViewJavascriptBridge) {
                callback && callback(WebViewJavascriptBridge)
            } else {
                document.addEventListener('WebViewJavascriptBridgeReady', function () {
                    callback && callback(WebViewJavascriptBridge);
                }, false);
            }
        }

        function init(data) {
            WebBridge['init'](function (message, responseCallback) {
                debugMsg('init', data, message);
                responseCallback && responseCallback(data);
            });
        }

        //send
        function send(opts) {
            var data = newCopy(opts) || {};
            WebBridge['send'](data, function (responseData) {
                opts.success && opts.success(responseData);
                debugMsg('send', data, responseData);
            });
        }

        // app与JS交互
        function registerHandler(evs, opts) {
            if (!isApp()) return;
            var respData = newCopy(opts) || {};
            var apiList = configState.apiList.registerHandler;
            var evs = evs === 'registerHandler' ? opts.type : evs;
            evs === apiList[evs]
                ? WebBridge['registerHandler'](evs, function (data, responseCallback) {
                    opts.success && opts.success(data);
                    responseCallback && responseCallback(respData);
                    debugMsg(evs, respData, data);
                })
                : debugMsg(evs + ' => ' + allStateMsg['205'] + evs, !0);
        }

        // js与app交互
        function callHandler(evs, opts) {
            if (!isApp()) return;
            var data = newCopy(opts) || {};
            var apiList = configState.apiList.callHandler;
            var evs = evs === 'callHandler' ? opts.type : evs;

            if (evs === apiList[evs]) {
                WebBridge['callHandler'](evs, data, function (responseData) {
                    errMsg(evs, responseData, opts);
                    debugMsg(evs, data, responseData);
                });
            }
            else {
                debugMsg('CH => ' + evs + ' => ' + allStateMsg['205'], !0);
            }
        }

        //
        function errMsg(evs, responseData, opts) {
            if (responseData) {
                opts.success && opts.success(responseData);
            } else {
                opts.cancel && opts.cancel(responseData);
            }
        }

        // debugMsg
        function debugMsg(ev, data, respData) {
            if (confDebug) {
                if (typeof data === 'object') data = JSON.stringify(data);
                if (typeof responseData === 'object') responseData = JSON.stringify(responseData);

                //document.getElementById('test-content').innerHTML = ev + (data ? '\n APP参数:\n ' + data : '') + (respData ? '\n 返回参数:\n ' + respData : '');

                $.alert(ev + (data ? '\n APP参数:\n ' + data : '') + (respData ? '\n 返回参数:\n ' + respData : ''));
                console.log(ev + (data ? '\n APP参数:\n ' + data : '') + (respData ? '\n 返回参数:\n ' + respData : ''));
            }
        }

        // 传递参数过滤
        function newCopy(obj) {
            if (!obj) return false;
            var temp = {};
            for (var key in obj) {
                if (key !== 'type' && key !== 'success' && key !== 'error' && key !== 'cancel') temp[key] = obj[key];
            }
            return temp;
        }

        // 备份API
        function copyApiList(opts) {
            var newObj = {};
            for (var key in opts) {
                var arr = opts[key],
                    temp = {};
                for (var i = 0; i < arr.length; i++) {
                    temp[arr[i]] = arr[i];
                }
                newObj[key] = temp;
            }
            return newObj;
        }

        // 根据 ApiList 生成方法
        function createMethods(apiList) {
            for (var key in apiList) {
                if (key === 'registerHandler' && apiList[key].constructor === Array) {
                    createMethod(key, apiList[key]);
                    continue;
                }
                else if (key === 'callHandler' && apiList[key].constructor === Array) {
                    createMethod(key, apiList[key]);
                    continue;
                }
            }
        }
        function createMethod(evTypeStr, apiList) {
            for (var i = 0; i < apiList.length; i++) {
                if (!Methods[apiList[i]]) {
                    Methods[apiList[i]] = (function (evsStr) {
                        return function (opts) {
                            evTypeStr === 'registerHandler' ? registerHandler(evsStr, opts) : callHandler(evsStr, opts);
                        }
                    })(apiList[i]);
                }
            }
        }

        // extend 自定义方法
        function extend(opts) {
            if (typeof opts !== 'object') return opts;
            for (var key in opts) {
                if (!configState.apiList.callHandler[key]) {
                    configState.apiList.callHandler[key] = key;
                    Methods[key] = opts[key];
                } else {
                    debugMsg('该方法已存在!');
                }
            }
        }

        // 是否是当前APP
        function isApp() {
            if (navigator.userAgent.toLowerCase().indexOf("txooo.app") > -1) {
                return true;
            } else {
                return false;
            }
        }

        // 对外暴露接口
        Methods = {
            // 初始化 配置信息
            config: function (opts) {
                confDebug = opts.debug;
                configState.apiList = copyApiList(opts.jsApiList);
                createMethods(opts.jsApiList || {});
                configState.state = !0;
            },
            error: function (callback) {
                if (callback && configState.state) {
                    callback('OK：已通过验证');
                } else {
                    callback('未通过验证，暂没有权限调用接口');
                }
            },
            ready: function (callback) {
                if (!callback) return false;
                if (!configState.state) return false;
                if (!isApp()) {
                    debugMsg('IA => ' + allStateMsg['206']);
                    return false;
                }

                ready(function (bridge) {
                    WebBridge = bridge;
                    callback && callback();
                });
            },
            // 方法
            init: init,
            send: send,
            registerHandler: function (opts) {
                if (typeof opts !== 'object') return opts;
                registerHandler('registerHandler', opts);
            },
            callHandler: function (opts, name) {
                if (typeof name == 'object' && typeof opts == 'string') {
                    var temp = opts;
                    opts = name;
                    name = temp;
                }
                if (typeof opts !== 'object') return opts;
                name = opts.type ? opts.type : name;
                callHandler(name, opts);
            },
            //手动添加脚本中不存在的功能性函数
            extend: function (opts) {
                extend(opts);
            },
            // 是否是当前APP
            isApp: isApp,
            // 自定义方法
            //返回上一页，某人非app环境自动返回上一页
            goBack: function (data) {
                if (txapp.isApp()) {
                    this.callHandler(data);
                } else {
                    history.go(-1);
                }
            }
        };

        return (win.jBridge = Methods);
    });