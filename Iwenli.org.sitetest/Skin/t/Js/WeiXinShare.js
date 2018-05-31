var wxShare = {
    //标题
    title: null,
    //描述
    desc: null,
    //链接
    link: null,
    //图标
    imgUrl: null,
    //账户类型（平台类型）
    accountType: null,
    //微信上传图片数量
    wxImgCount: 9,
    //微信选择图片后的图片地址
    wxImgSrc: function () { },
    //微信选择图片后是否立即执行上传
    wxSetImgOpinion: true,
    //微信上传图片的自定义异步函数
    wxUpImgAjax: function () { },
    //已选择的图片本地列表
    wxLocalIds: [],
    //开放目录
    jsApiList: ['onMenuShareTimeline',
                'onMenuShareAppMessage',
                'onMenuShareQQ',
                'onMenuShareWeibo',
                'onMenuShareQZone',
                'chooseImage',
                'previewImage',
                'uploadImage',
                'downloadImage'],
    //分享完成后执行的代码
    shareFinish: function () { },
    //分享失败后执行的代码
    shareError: function () { },
    //获得公众号身份异步地址
    configAsynUrl: null,
    //分享异步代码
    init: function () {
        var data = '?callbackparam=wxShare.jsonpSuccess&selfurl=' + encodeURIComponent(window.location.href) + '&accountType=' + wxShare.accountType;
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.src = (this.configAsynUrl ? this.configAsynUrl : "//passport.txooo.com/Member/TxMemberHandler.ashx/WXShareParams") + data;
        document.getElementsByTagName('head')[0].appendChild(script);
    },
    //异步成功后
    jsonpSuccess: function (json) {
        wxShare.wxShareParams(json[0]);
    },
    //微信内部分享函数
    wxShareParams: function (jsonObj) {
        wx.config({
            debug: false,  // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: jsonObj.appId, // 必填，公众号的唯一标识
            timestamp: jsonObj.timestamp, // 必填，生成签名的时间戳
            nonceStr: jsonObj.nonceStr, // 必填，生成签名的随机串
            signature: jsonObj.signature,// 必填，签名，见附录1
            jsApiList: wxShare.jsApiList//开放目录
        });
        wx.ready(function () {
            // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
            wx.onMenuShareTimeline({
                title: wxShare.title, // 分享标题
                link: wxShare.link, // 分享链接
                imgUrl: wxShare.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    wxShare.shareFinish();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                    wxShare.shareError();
                }
            });
            wx.onMenuShareAppMessage({
                title: wxShare.title, // 分享标题
                desc: wxShare.desc, // 分享描述
                link: wxShare.link, // 分享链接
                imgUrl: wxShare.imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数
                    wxShare.shareFinish();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                    wxShare.shareError();
                }
            });
            wx.onMenuShareQQ({
                title: wxShare.title, // 分享标题
                desc: wxShare.desc, // 分享描述
                link: wxShare.link, // 分享链接
                imgUrl: wxShare.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    wxShare.shareFinish();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                    wxShare.shareError();
                }
            });
            wx.onMenuShareWeibo({
                title: wxShare.title, // 分享标题
                desc: wxShare.desc, // 分享描述
                link: wxShare.link, // 分享链接
                imgUrl: wxShare.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    wxShare.shareFinish();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                    wxShare.shareError();
                }
            });
            wx.onMenuShareQZone({
                title: wxShare.title, // 分享标题
                desc: wxShare.desc, // 分享描述
                link: wxShare.link, // 分享链接
                imgUrl: wxShare.imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    wxShare.shareFinish();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                    wxShare.shareError();
                }
            });
        });
        wx.error(function (res) {
            // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。

        });
    },
    //微信选择图片内部函数，拍照或从手机相册中选图接口  只上传一张
    wxSetImg: function () {
        wx.chooseImage({
            count: wxShare.wxImgCount,// 默认9
            sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
            sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
            success: function (res) {
                wxShare.wxLocalIds = res.localIds;// 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片;
                wxShare.wxImgSrc();//选择完成后调用
                //选择图片后立即执行上传功能
                if (wxShare.wxSetImgOpinion) {
                    wxShare.wxUpImg();
                }
            }
        });
    },
    //微信上传图片内部函数
    wxUpImg: function () {
        //上传图片接口
        var serverIds = [];
        var i = 0, len = wxShare.wxLocalIds.length;
        function upload() {
            wx.uploadImage({
                localId: wxShare.wxLocalIds[i],
                isShowProgressTips: 1,
                success: function (res) {
                    i++;
                    serverIds.push(res.serverId);
                    if (i < len) {
                        upload();
                    }
                    if (i == len) {
                        wxShare.wxUpImgAjax(serverIds.join(","));
                    }
                },
                fail: function (res) {
                    alert(JSON.stringify(res));
                }
            });
        }
        upload();
    }
};