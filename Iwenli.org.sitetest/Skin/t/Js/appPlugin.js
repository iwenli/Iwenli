if (navigator.userAgent.toLowerCase().indexOf("txooo.app") > -1) {

    document.addEventListener("plusready", function () {
        // 声明的JS“扩展插件别名”
        var _BARCODE = 'txservice',
            B = window.plus.bridge;

        var txservice = {
            // 声明异步返回方法
            shareAsync: function (Argus, successCallback, errorCallback) {
                var success = typeof successCallback !== 'function' ? null : function (args) {
                    successCallback(args);
                },
                fail = typeof errorCallback !== 'function' ? null : function (code) {
                    errorCallback(code);
                };
                callbackID = B.callbackId(success, fail);
                return B.exec(_BARCODE, "share", [callbackID, Argus]);
            },
            share: function (Argus) {
                return B.execSync(_BARCODE, "share", [Argus]);
            },
            openQuanlink: function (Argus) {
                return B.exeSync(_BARCODE, "openQuanlink", [Argus])
            }
        };

        window.plus.txservice = txservice;

    }, true);
}