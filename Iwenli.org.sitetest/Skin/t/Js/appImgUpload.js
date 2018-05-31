var appImgUpload = {
    //多图片上传数量
    imgCount: 9,
    //本地文件列表
    files: [],
    //选择图片后执行的函数
    getImgSrc: function (src) { },
    //上传Ajax路径
    ajaxUrl: 'http://passport.93390.cn/Txooo/SalesV2/Ajax/UploadImg.ajax/UpImgUrl',
    //上传成功后执行的
    uploadImg: function () { },
    //选择图片后初始化
    setImgInitial: function () {
        appImgUpload.files = [];
    },
    //选择单张图片
    setImg: function () {
        plus.gallery.pick(function (path) {
            //初始化
            appImgUpload.setImgInitial();
            plus.nativeUI.showWaiting("正在加载···");
            appImgUpload.getImg(path);
        }, function (e) {
            //alert("取消选择图片");
        }, { filter: "image" });
    },
    //选择多张图片上传
    setImgs: function () {
        plus.gallery.pick(function (e) {
            //初始化
            appImgUpload.setImgInitial();
            plus.nativeUI.showWaiting("正在加载···");
            for (var i = 0; i < e.files.length; i++) {
                if (i < appImgUpload.imgCount) {
                    appImgUpload.getImg(e.files[i]);
                }
            }
        }, function (e) {
            //outSet("取消选择图片");
        }, { filter: "image", multiple: true });
    },
    //获得本地图片对象
    getImg: function (path) {
        plus.io.resolveLocalFileSystemURL(path, function (entry) {
            entry.file(function (file) {
                var fileReader = new plus.io.FileReader();
                fileReader.readAsDataURL(file);
                fileReader.onloadend = function (evt) {
                    appImgUpload.files.push({ name: file.name, path: path, target: evt.target.result });
                    appImgUpload.getImgSrc(evt.target.result);
                }
            });
        }, function (e) {
            alert("获取本地图片错误: " + e.message);
        });
    },
    // 上传原图图片
    uploadOriginal: function () {
        if (appImgUpload.files.length <= 0) {
            alert('未选择图片');
            return;
        }
        plus.nativeUI.showWaiting("上传中，请稍后···");
        for (var i = 0; i < appImgUpload.files.length; i++) {
            appImgUpload.uploadAjax(i, appImgUpload.files[i]);
        }
    },
    //上传压缩图片
    uploadZip: function () {
        if (appImgUpload.files.length <= 0) {
            alert('未选择图片');
            return;
        }
        plus.nativeUI.showWaiting("上传中，请稍后···");
        for (var i = 0; i < appImgUpload.files.length; i++) {
            appImgUpload.setZipImage(i, appImgUpload.files[i]);
        }
    },
    //上传ajax函数
    uploadAjax: function (_index, path, name) {
        var task = plus.uploader.createUpload(appImgUpload.ajaxUrl, { method: "POST" },
                function (t, status) {
                    if (status == 200) {
                        appImgUpload.uploadImg(t.responseText);
                    } else {
                        alert("上传失败：" + status);
                    }
                }
            );
        var f = appImgUpload.files[_index];
        task.addFile(f.path, { key: f.name });
        task.start();
    },
    //zip压缩
    setZipImage: function (_index, file) {
        plus.zip.compressImage({
            src: file.path,
            dst: "_doc/" + file.name,
            quality: 20
        }, function (i) {
            //alert(i.target+'。'+file.path);
            appImgUpload.uploadAjax(_index, i.target, file.name);
            return;
        }, function (e) {
            appImgUpload.uploadAjax(_index, file.path, file.name);
            //plus.nativeUI.closeWaiting();
            //alert("压缩图片失败: " + JSON.stringify(e));
            return;
        });
    }
};