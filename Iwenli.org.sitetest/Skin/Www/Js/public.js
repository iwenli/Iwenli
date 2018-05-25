//禁止ajax缓存
$.ajaxSetup({
    cache: false,
    contentType: "application/x-www-form-urlencoded; charset=utf-8",
    error: function () {

    }
});
