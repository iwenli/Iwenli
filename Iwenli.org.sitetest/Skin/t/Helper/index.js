$(function () {
    //window.txservice.init();
    'use strict';
    publicPageInit('#page-helper-index', function (e, id, page) {

    });

    // 添加'refresh'监听器
    $(document).on('refresh', '.pull-to-refresh-content', function (e) {
        window.location.reload();
    });
    $.init();
});