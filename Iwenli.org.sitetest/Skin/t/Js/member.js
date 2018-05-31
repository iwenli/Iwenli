$(function () {
    window.txservice.init();
    'use strict';

    //商品详情头部
    $(document).on("pageInit", "#page-shop-index", function (e, id, page) {
        $.pullToRefreshDone('.pull-to-refresh-content');

        //滑动到底部加载
        $(page).on('infinite', function () {
            if (page_index_loading) return;
            page_index_loading = true;

        });
    });

    //商品详情图文
    $(document).on("pageInit", "#page-shop-details", function (e, id, page) {
        $.pullToRefreshDone('.pull-to-refresh-content');

    });
    // 添加'refresh'监听器
    $(document).on('refresh', '.pull-to-refresh-content-reload', function (e) {
        window.location.reload();
    });
    $(document).on('refresh', '.pull-to-refresh-content-back', function (e) {
        $.router.back();
    });
    $.init();
});