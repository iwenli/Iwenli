﻿$(function () {
    'use strict';

    //商品详情
    publicPageInit("#page-shop-details", function (e, id, page) {
        console.log(page[0].id);
        setScrollerNum();
        downloadDiv();
        //主体信息加载
        (function () {
            var info = $.parseJSON($(page).find('.product_data').html());
            if (info.length == 0) { $.router.load('/shop/noPro.html?pid=' + getUrlParam('id')); return; }
            parameters.shop.details.list = info[0];
            parameters.shop.details.list.is_tx_app = txapp.isApp();
            parameters.shop.details.shareDesc = parameters.shop.details.list.product_name;
            //加载轮播图
            $(page).find('.details_lunbo  .swiper-wrapper').html(template('lunbo_temp', { list: parameters.shop.details.list.product_imgs.split(',') }));
            new Swiper('.details_lunbo', {
                pagination: '.swiper-pagination',
                paginationClickable: true,
                autoplay: 3000,
                loop: true
            });
            //加载主信息
            $(page).find('.shop_info').html(template('shop_info_temp', { info: parameters.shop.details.list }));
            $(page).find('.nav_collect').data('collect', parameters.shop.details.list.is_collect).addClass('is_collect_' + parameters.shop.details.list.is_collect);
        })();
        //页面内容点击事件处理
        (function () {
            //返回
            $(page).off('click', '.click_back').on('click', '.click_back', function () {
                if (txapp.isApp()) {
                    txapp.goBack();
                    return false;
                }
                $.router.back();
                return false;
            });
            //同类商品
            $(page).off('click', '.like_product').on('click', '.like_product', function () {
                txapp.likeProduct({
                    ProductType: parameters.shop.details.list.product_type,
                    ProductTypeName: parameters.shop.details.list.product_type_name,
                    MchClass: parameters.shop.details.list.mch_class,
                    NewOld: parameters.shop.details.list.new_old
                });
            });
            //联系卖家
            $(page).off('click', '.mch_chat').on('click', '.mch_chat', function () {
                if (txapp.isApp()) {
                    var _userId = $(this).data('agent') == '0' ? $(this).data('mch') : $(this).data('agent');
                    txapp.mchChat({ user_id: _userId, product_id: getUrlParam('id') });
                    return false;
                }
                $.confirm('很抱歉，您只能使用APP联系卖家，是否现在下载？', function () {
                    publicDownLoadApp();
                });
            });
            //点击收藏
            $(page).off('click', '.collect_click').on('click', '.collect_click', function () {
                if (userIsAuth() == false) return publicConfigLogin();
                var _self = $(this);
                var _type = _self.data('collect') == 'False' ? 1 : 0;
                addOrCancelCollectProduct({ pid: getUrlParam('id'), operationType: _type }, function (obj) {
                    if (obj.success == 'true') {
                        var _state = _self.data('collect') == 'False' ? 'True' : 'False';
                        parameters.shop.details.list.is_collect = _state;
                        $(page).find('.collect_click').removeClass('is_collect_' + _self.data('collect')).addClass('is_collect_' + _state).data('collect', _state);
                    } else {
                        $.toast(obj.msg);
                    }
                });
            });
            //点击推广
            $(page).off('click', '.share_click').on('click', '.share_click', function () {
                window.txapp.share({
                    ProductName: parameters.shop.details.list.product_name,
                    ShareContent: parameters.shop.details.shareDesc,
                    Url: window.location.origin + "/shop.html?id=" + parameters.shop.details.list.product_id,
                    Img: getOneProductImg(parameters.shop.details.list.product_imgs),
                    ProductId: parameters.shop.details.list.product_id
                });
            });
            //点击店铺
            $(page).off('click', '.store_click').on('click', '.store_click', function () {
                goStore(parameters.shop.details.list.mch_id);
            });
            //推广品牌点击
            $(page).off('click', '.store_brand').on('click', '.store_brand', function () {
                var mch_id = $(this).data('mch');
                setStoreBrandPop({ brand_id: $(this).data('brand'), product_id: parameters.shop.details.list.product_id }, function () {
                    goStore(mch_id);
                });
            });
            //进入店铺
            function goStore(mch_id) {
                if (txapp.isApp()) {
                    window.txapp.store({ mchid: mch_id });
                } else {
                    routerLoad('/shop_2/mchstore.html?mchid=' + mch_id);
                }
            }
            //轮播图预览
            $(page).off('click', '.swiper-slide').on('click', '.swiper-slide', function () {
                var _index = $(this).index() - 1;
                _index = _index == parameters.shop.details.list.product_imgs.split(',').length ? 0 : _index;
                _index = _index == -1 ? parameters.shop.details.list.product_imgs.split(',').length - 1 : _index;
                console.log($(this).index(), _index);
                txapp.imgPreview({ imgurls: parameters.shop.details.list.product_imgs, imgindex: _index });
            });
            //商品详情图预览
            $(page).find('.product_details').off('click', 'img').on('click', 'img', function () {
                var _index = 0;
                var imgurls = '';
                var $this = this;
                $(page).find('.product_details img').each(function (i, o) {
                    imgurls = imgurls + ',' + $(o).attr('src');
                    if ($this == o) { _index = i; }
                });
                txapp.imgPreview({ imgurls: imgurls, imgindex: _index });
            });
            //规格图片预览
            $('#product_property_list').off('click', 'img').on('click', 'img', function () {
                txapp.imgPreview({ imgurls: $(this).prop('src'), imgindex: 0 });
            });
            //点击售后
            $(page).off('click', '.shop_explain').on('click', '.shop_explain', function () {
                $.router.load('/shop_2/explain.html');
            });
            //规格属性数据
            var property_list;
            var map_id = 0;
            $('#product_property_list').html('');//该弹框是全局容器，所以要清空后使用
            //打开立即购买
            $(page).off('click', '.open_property_list').on('click', '.open_property_list', function () {
                $.popup('#product_property_list');
                if ($('#product_property_list').text().length == 0) {
                    property_list = $.parseJSON($(page).find('.product_property').html());
                    $('#product_property_list').html(template('product_property_temp', { list: property_list }));
                    var _defaultObj;
                    var _defaultData = $.grep(property_list, function (j, k) { return j.is_default == true; });
                    if (_defaultData.length == 0)
                        _defaultData = $.grep(property_list, function (j, k) { return j.remain_inventory > 0; });

                    if (_defaultData.length > 0) _defaultObj = _defaultData[0];
                    propertySelect(_defaultObj);
                }
            });
            //选择规格赋予样式变化
            function propertySelect(obj) {
                if (obj) {
                    map_id = obj.map_id;
                    $('#product_property_list .property_img').prop('src', obj.img);
                    $('#product_property_list .property_price').text(obj.price);
                    $('#product_property_list .remain_inventory').text(obj.remain_inventory);
                    //样式变化
                    $('#product_property_list .property_select').removeClass('current')
                    $('#product_property_list .map_id' + obj.map_id).addClass('current')
                }
            };
            //规格选择
            $('#product_property_list').off('click', '.property_select').on('click', '.property_select', function () {
                var _id = $(this).data('id');
                var _selectData = $.grep(property_list, function (j, k) { return j.map_id == _id; });
                if (_selectData.length == 0) return false;
                propertySelect(_selectData[0]);
            });
            //数量加减
            $('#product_property_list').off('click', '.operation_num').on('click', '.operation_num', function () {
                var _pro_count = parseInt($('#product_property_list .pro_count').val());
                if ($(this).hasClass('num_up')) {
                    if (_pro_count >= parseInt($('#product_property_list .remain_inventory').text())) {
                        $.toast('就剩这么多库存了，快联系卖家上货吧');
                        return false;
                    }
                    _pro_count++;
                }
                else if (_pro_count > 1) _pro_count--;
                $('#product_property_list .pro_count').val(_pro_count);
            });
            //规格确定
            $('#product_property_list').off('click', '.go_shop_order').on('click', '.go_shop_order', function () {
                $.closeModal();
                if (txapp.isApp()) {
                    txapp.confirmOrder({ id: getUrlParam('id'), proProperty: map_id, isVirtural: parameters.shop.details.list.is_virtual, count: $('#product_property_list .pro_count').val() });
                    return false;
                }
                $.router.load('/shop/order.html?id=' + parameters.shop.details.list.product_id + '&proProperty=' + map_id + '&count=' + $('#product_property_list .pro_count').val());
            });
        })();
        //滚动事件监听与处理
        (function () {
            //滚动事件监听
            $(page).find(".content").on('scroll', function () {
                var standard = $(page).find('.shop_info').offset().top;
                var divHeight = $(window).width();
                var dNumber = divHeight / standard - 1
                if (dNumber < 0) {
                    dNumber = 1
                } else if (standard > divHeight) {
                    dNumber = 0
                }
                $(page).find('.d_header').css({ opacity: dNumber })
                var scrollNum = $(page).find(".content").scrollTop()
                if (scrollNum > titnum2 - titHeight * 1.2 && scrollNum < titnum3 - titHeight * 1.2) {
                    $(page).find(".title2").addClass('current').siblings().removeClass('current')
                } else if (scrollNum > titnum3 - titHeight * 1.2) {
                    $(page).find(".title3").addClass('current').siblings().removeClass('current')
                } else {
                    $(page).find(".title1").addClass('current').siblings().removeClass('current')
                }
            });
            //滚动图高度
            var winWidth = $(page).find(".content").width();
            $(page).find('.carry').css('height', winWidth + 'px');

            $(page).off('click', '.title1').on('click', '.title1', function () {
                $(page).find(".content").scrollTop(0);
                $(page).find(".title1").addClass('current').siblings().removeClass('current');
            });
            $(page).off('click', '.title2').on('click', '.title2', function () {
                $(page).find(".content").scrollTop(titnum2 - titHeight);
                $(page).find(".title2").addClass('current').siblings().removeClass('current');
            });
            $(page).off('click', '.title3').on('click', '.title3', function () {
                $(page).find(".content").scrollTop(titnum3 - titHeight);
                $(page).find(".title3").addClass('current').siblings().removeClass('current');
            });
            var loading = false;
            //滑动到底部加载详情信息
            $(page).on('infinite', function () {
                if (loading) return;
                loading = true;
                $.detachInfiniteScroll($('.infinite-scroll'));
                $(page).find('.product_details').html(parameters.shop.details.list.product_details);
                setTimeout(function () {
                    getProductBrandList();
                }, 300);
            });
        })();
        //加载店铺其他商品
        parameters.shop.details.mchParams.mch_id = parameters.shop.details.list.mch_id;
        parameters.shop.details.mchParams.product_id = parameters.shop.details.list.product_id;
        setTimeout(function () {
            if ($(page).find('.store_pro_list a').length == 0) {
                loadMchPro(parameters.shop.details.mchParams, function (obj) {
                    $(page).find('.store_pro_list').html('');
                    if (obj.length > 0) {
                        $(page).find('.store_pro_list').html(template("store_temp", { list: obj }));
                    }
                    setScrollerNum();
                });
            }
        }, 300);
        //加载评价信息
        setTimeout(function () {
            if ($(page).find('.product_review_list').text().length == 0) {
                loadTopProductReview({ product_id: getUrlParam('id') }, function (obj) {
                    if (obj.length > 0) {
                        $(page).find('.product_review_list').html(template("product_review_temp", { list: obj[0].review }));
                        $(page).find('.review_count').text(obj[0].count);
                        setScrollerNum();
                    }
                });
            }
        }, 350);
        //添加商品足迹
        if (userIsAuth()) {
            setTimeout(function () {
                addOrUpdateUserBrowseFootprint(parameters.shop.details.list.product_id, parameters.shop.details.list.mch_class);
            }, 500);
        }
        //加载关联品牌店铺
        function getProductBrandList() {
            if (parameters.shop.details.list.brand_marketing == 1) {
                getProductBrand(parameters.shop.details.list.product_id, function (obj) {
                    if (obj.length > 0) {
                        $(page).find('.product_brand_list').html(template('product_brand_temp', obj[0]));
                    } else {
                        $(page).find('.product_brand_list').remove();
                    }
                    setScrollerNum();
                });
            } else {
                $(page).find('.product_brand_list').remove();
            }
        };
        var titnum2, titnum3, titHeight;
        //滚动条重新计算
        function setScrollerNum() {
            $.refreshScroller();
            titnum2 = $(page).find(".evaluate").offset().top + $(page).find(".content").scrollTop();
            titnum3 = $(page).find(".details_index").offset().top + $(page).find(".content").scrollTop();
            titHeight = $(page).find(".evaluate").outerHeight();
        }
    });
    //商家店铺
    publicPageInit("#page-shop-mchstore", function (e, id, page) {
        if (parameters.shop.isFirst) {
            parameters.shop.isFirst = false;
        }
        else {
            return;
        }
        //返回
        $(page).off('click', '.click_back').on('click', '.click_back', function () {
            if (firstLoad(window.location.href)) {
                $.router.back();
                return false;
            }
        });
        downloadDiv();
        parameters.shop.mchstore.news.pageIndex = 0;
        parameters.shop.mchstore.product = {};
        parameters.shop.mchstore.product.p_0 = { pageIndex: 0, pageSize: 10, mch_id: $('#mch_id').val(), class_id: 0 };
        parameters.shop.mchstore.news.mch_id = $('#mch_id').val();

        //商品分类加载，异步读取Swiper有问题
        var class_data = eval($('#product_class_content_data').html());
        if (class_data) {
            $('.product_class_name_list .swiper-wrapper').append(template('product_class_name_temp', { list: class_data }));
            $('.product_class_content_list').append(template('product_class_content_temp', { list: class_data }));
            $.each(class_data, function (i, o) {
                parameters.shop.mchstore.product['p_' + o.class_id] = { pageIndex: 0, pageSize: 10, mch_id: $('#mch_id').val(), class_id: o.class_id }
            });
        } new Swiper('.product_class_name_list', { slidesPerView: 4 });

        var _productOnlyShow = 1;
        //获得店铺信息
        getMchInfoByUser($('#mch_id').val(), function (obj) {
            _productOnlyShow = obj.ProductOnlyShow;
            if (obj.MchClass == 3) $('.bar .title').text('品牌展示');
            else $('.liuyan_bar').remove();
            $(page).find('.content .store_content_info').append(template('content_temp', obj));
            if (obj.NewsCount > 0) {
                var _menu = { MenuName: '资讯', MenuContent: '', MenuId: -100 };
                obj.StoreMenuInfoList[obj.StoreMenuInfoList.length] = _menu;
            }
            $(page).find('.content .menu_name_list .swiper-wrapper').append(template('menu_name_temp', { list: obj.StoreMenuInfoList }));
            $(page).find('.content .menu_content_list').append(template('menu_content_temp', { list: obj.StoreMenuInfoList }));
            var _show = getUrlParam('show');//分享过来需要定位展示板块
            if (_show != '' && parseInt(_show) <= obj.StoreMenuInfoList.length) {
                var _name = $('.content .menu_name_list .swiper-wrapper a').eq(_show);
                var _content = $('.content .menu_content_list').children().eq(_show);
                if (parseInt(_show) == obj.StoreMenuInfoList.length) {
                    _show = -100;
                    _content = $('#tab' + _show);
                    _name = $('#tab-link' + _show);
                }
                _name.addClass('active').siblings().removeClass('active');
                _content.addClass('active').siblings().removeClass('active');
            }
            if ($('#tab0.active').length > 0) loadProduct(0);
            if ($('#tab-100.active').length > 0) loadNews();
            if (obj.StoreAdImgInfoList) {
                $('.mchad_lunbo .swiper-wrapper').html(template('store_lunbo_temp', { list: obj.StoreAdImgInfoList }));
                new Swiper('.mchad_lunbo', {
                    pagination: '.swiper-pagination',
                    paginationClickable: true,
                    autoplay: 3000,
                    loop: true
                });
            }
            new Swiper('.menu_name_list', { slidesPerView: 5 });

            $('#tab0 .product_count').text(obj.ProductCount);
            $('#browse_count').text(obj.BrowseCount);

            if ($('#is_auth').val() == "false") {
                $('.is_collect_div').hide();
            }
            //关注店铺
            $('.is_collect_div').on('click', function () {
                var self = $(this);
                var collect = $(this).data('collect') == 0 ? 1 : 0;
                setMchBrowse($('#mch_id').val(), collect, function (obj) {
                    self.removeClass('is_collect_' + self.data('collect')).addClass('is_collect_' + collect).data('collect', collect);
                    $('.store_content_info .collect_count').text(parseInt($('.store_content_info .collect_count').text()) + (collect == 0 ? -1 : 1));
                });
            });
        });
        //商品分类点击加载数据
        $(page).off("click", ".product_class_name_list .tab-link").on('click', '.product_class_name_list .tab-link', function () {
            var pclass = $(this).data('pclass');
            if (parameters.shop.mchstore.product['p_' + pclass].pageIndex == 0) loadProduct(pclass);
        });
        //多个标签页下的无限滚动
        var loading = false;
        //标签滑动加载
        $(page).on('infinite', function () {
            if (loading) return;
            if ($('#tab0.active').length > 0) {
                var pclass = $('.product_class_name_list .active').data('pclass');
                if ($('#tab0-' + pclass + ' .infinite-scroll-preloader').css("display") == "block" && parameters.shop.mchstore.product['p_' + pclass].pageIndex > 0) {
                    loading = true;
                    loadProduct(pclass);
                }
            }
            if ($('#tab-100.active').length > 0) {
                if (parameters.shop.mchstore.news.pageIndex > 0 && $('#tab-100 .infinite-scroll-preloader').css("display") == "block") {
                    loading = true;
                    loadNews();
                }
            }
        });
        //点击资讯加载数据
        $(page).off("click", "#tab-link-100").on('click', '#tab-link-100', function () {
            if (parameters.shop.mchstore.news.pageIndex == 0) {
                loading = true;
                loadNews();
            }
        });
        //根据分类加载产品
        function loadProduct(pclass) {
            getMchStoreProductList2(parameters.shop.mchstore.product['p_' + pclass], function (params, obj) {
                params.pageIndex++;
                if (obj.length > 0) {
                    if (params.pageIndex == 1) $('#tab0-' + params.class_id + ' .list').html('');
                    $('#tab0-' + params.class_id + ' .list').append(template('store_product_list_temp', { list: obj, ProductOnlyShow: _productOnlyShow }));
                }
                if (obj.length < params.pageSize) {
                    $('#tab0-' + params.class_id + ' .infinite-scroll-preloader').hide();
                    $('#tab0-' + params.class_id + ' .more_container').show();
                }
                loading = false;
            });
        };
        //加载资讯信息
        function loadNews() {
            getMchNewsList(parameters.shop.mchstore.news, function (params, obj) {
                if (obj.length > 0) {
                    params.pageIndex++;
                    if (params.pageIndex == 1) $('#tab-100 .list').html('');
                    $('#tab-100 .list').append(template('store_news_temp', { list: obj }));
                }
                if (obj.length < params.pageSize) {
                    $('#tab-100 .infinite-scroll-preloader').hide();
                    $('#tab-100 .more_container').show();
                }
                loading = false;
            });
        };
    });
    //商家资讯详情
    publicPageInit("#page-shop-mchnews", function (e, id, page) {
        //返回
        $(page).off('click', '.click_back').on('click', '.click_back', function () {
            if (firstLoad(window.location.href)) {
                $.router.back();
                return false;
            }
        });
        downloadDiv();
        if (getUrlParam('showheader') == 'false') {
            $('.bar').remove();
        } else {
            $('.bar').show();
        }
        parameters.shop.mchnews.info.news_id = $('#news_id').val();
        //获得资讯详情
        getStoreNewsInfo(parameters.shop.mchnews.info.news_id, function (obj) {
            if (obj.length > 0) {
                obj[0].IsApp = txapp.isApp();
                $(page).find('.news_detail').html(template('news_data_temp', obj[0]));
                parameters.shop.mchnews.info.news_title = obj[0].NewsTitle;
                parameters.shop.mchnews.info.news_cover = obj[0].NewsCover;
                parameters.shop.mchnews.recommend.news_id = obj[0].NewsId;
                parameters.shop.mchnews.recommend.mch_id = obj[0].MchId;
                parameters.shop.mchnews.mchParams.mch_id = obj[0].MchId;
                parameters.shop.mchnews.mchParams.pageIndex = 0;
                $('#user_id').val(obj[0].UserId)
                $('.hot_shop .infinite-scroll-preloader').show();
                $(page).find('.news_detail .news_report').on('click', function () {
                    txapp.newsReport(parameters.shop.mchnews.info);
                });
                $('.browseUserCount').text(obj[0].BrowseUserCount);
                if (!txapp.isApp()) $('.news_report').hide();
                $('.liuyan').attr('href', '/shop_2/liuyan.html?mchid=' + obj[0].MchId);
                //$('.gobackstore').on('click', function () {
                //    routerLoad('/shop_2/mchstore.html?mchid=' + obj[0].MchId);
                //});
                //资讯点赞
                $(page).find('.news_detail .is_like_click').on('click', function () {
                    var _self = $(this);
                    if (_self.data('like') == "1") {
                        $.toast("你已点过赞");
                        return false;
                    }
                    setStoreNewsLike({ news_id: obj[0].NewsId }, function (obj) {
                        _self.data('like', 1);
                        _self.removeClass('is_like_0').addClass('is_like_1');
                        _self.find('span').text(parseInt(_self.find('span').text()) + 1);
                    });
                    return false;
                });
                if (txapp.isApp()) {
                    getUserInfo({ userId: $('#user_id').val() }, function (obj) {
                        if (obj.length > 0) {
                            parameters.shop.mchnews.author = obj[0];
                        }
                    });
                }
                if (obj[0].OpenReward == true) {
                    //获得打赏头像列表
                    getRewardInfo({ pageIndex: 0, pageSize: 16, onlyId: $('#news_id').val() }, function (params, obj) {
                        if (obj.length > 0) {
                            $(page).find('.reward_list').html(template('reward_list_temp', { count: obj[0].Count, list: obj[0].List }));
                        }
                    });
                }
            } else {
                $.toast('资讯不存在');
                $.router.back();
                return;
            }
            //获得推荐资讯
            getRecommendMchNews(parameters.shop.mchnews.recommend, function (obj) {
                if (obj.length > 0) $('.related_news .list').html(template('store_news_temp', { list: obj, showheader: getUrlParam('showheader') }));
                else $('.related_news').remove();
            });
        });

        parameters.shop.mchnews.bbslist.only_id = $('#news_id').val();
        //获得热门评论
        getBBSList(parameters.shop.mchnews.bbslist, function (params, obj) {
            if (obj.length > 0) {
                $(page).find('.news_bbs .list').html(template('bbs_temp', { list: obj }));
            } else {
                if (txapp.isApp()) {
                    $(page).find('.news_bbs').html('<div class="nonews_bbs">暂无评论，赶快点击抢个沙发吧！</div>');
                } else {
                    $(page).find('.news_bbs').remove();
                }
            }
        });
        //点赞
        $(page).find('.news_bbs').off('click', '.is_like_click').on('click', '.is_like_click', function () {
            var _self = $(this);
            if (_self.data('like') == "1") {
                $.toast("你已点过赞");
                return false;
            }
            setUserBBSLike({ bbs_id: _self.data('bbsid') }, function (obj) {
                if (obj.success == "true") {
                    var _span = _self.removeClass('is_like_0').addClass('is_like_1').data('like', 1).find('span');
                    _span.text(parseInt(_span.text()) + 1);
                }
                $.toast(obj.msg);
            });
            return false;
        });
        //资讯评论点击
        $(page).off('click', '.news_bbs .app').on('click', '.news_bbs .app', function () {
            if (txapp.isApp()) {
                txapp.mchNewsBBS({ news_id: parameters.shop.mchnews.info.news_id });
                return false;
            }
            $.router.load('/bbs/mchnews.html?newsid=' + parameters.shop.mchnews.bbslist.only_id);
            return false;
        });
        //查看发表的他人
        $(page).off('click', '.news_bbs .other_user').on('click', '.news_bbs .other_user', function () {
            if (txapp.isApp()) {
                txapp.otherUser({ userid: $(this).data('uid') });
                return false;
            }
        });
        //查看评论详情
        $(page).off('click', '.news_bbs .bbs_details').on('click', '.news_bbs .bbs_details', function () {
            if (!userIsAuth()) {
                publicGoLogin();
                return false;
            }
            if (txapp.isApp()) {
                txapp.bbsDetails({ bbs_id: $(this).data('bbsid'), news_id: parameters.shop.mchnews.info.news_id });
                return false;
            }
        });
        //查看评论详情添加评论
        $(page).off('click', '.news_bbs .bbs_detailsadd').on('click', '.news_bbs .bbs_detailsadd', function () {
            if (!userIsAuth()) {
                publicGoLogin();
                return false;
            }
            if (txapp.isApp()) {
                txapp.bbsDetailsAdd({ bbs_id: $(this).data('bbsid'), nick_name: $(this).data('nickname'), review_bbs_id: $(this).data('reviewid'), news_id: parameters.shop.mchnews.info.news_id });
                return false;
            }
        });
        //资讯沙发
        $(page).off('click', '.news_bbs .nonews_bbs').on('click', '.news_bbs .nonews_bbs', function () {
            txapp.addBBS({ bbs_type: 'mchnews', only_id: $('#news_id').val() });
        });
        //资讯商品跳转
        $(page).off('click', '.product_img_click').on('click', '.product_img_click', function () {
            var _url = '/shop.html?id=' + $(this).data('pid');
            if (txapp.isApp()) {
                txapp.shopDetails({ url: '' + _url });
                return false;
            }
            $.router.load(_url);
        });

        //标签滑动加载
        $(page).on('infinite', function () {
            if (parameters.shop.mchnews.mchParams.loading) return;
            parameters.shop.mchnews.mchParams.loading = true;
            //获得店铺热卖商品
            getMchStoreProductList2(parameters.shop.mchnews.mchParams, function (params, obj) {
                params.pageIndex++;
                if (obj.length > 0) {
                    if (params.pageIndex == 1) $('.hot_shop .list').html('');
                    $('.hot_shop .list').append(template('hot_product_list_temp', { list: obj }));
                    $.refreshScroller();
                }
                if (obj.length < params.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载                    
                    $.detachInfiniteScroll($('.infinite-scroll'));
                    $('.hot_shop .infinite-scroll-preloader').hide();
                }
                parameters.shop.mchnews.mchParams.loading = false;
            });
        });
        //打赏点击
        $(page).off('click', '.news_likes .reward_btn').on('click', '.news_likes .reward_btn', function () {
            if (publicDownLoadApp()) {
                txapp.rewardNews({
                    intoUserId: $('#user_id').val(), newsId: $('#news_id').val(),
                    nickName: parameters.shop.mchnews.author.nick_name,
                    headPic: parameters.shop.mchnews.author.head_pic,
                    userWord: parameters.shop.mchnews.author.user_word
                });
            }
            return false;
        });
        //打赏列表点击
        $(page).off('click', '#btn_reward_list').on('click', '#btn_reward_list', function () {
            if (txapp.isApp()) {
                txapp.rewardList({ newsId: $('#news_id').val() });
            }
            else {
                $.router.load("#page-shop-reward");
            }
            return false;
        });

    }, function () {
        txapp.onNews({ news_id: $('#news_id').val() });
    });
    //商家资讯详情预览
    publicPageInit("#page-shop-mchnewspreview", function (e, id, page) {
        //获得资讯详情
        getStoreNewsPreviewInfo($('#news_id').val(), function (obj) {
            if (obj.length > 0) {
                $(page).find('.news_detail').html(template('news_data_temp', obj[0]));
            } else {
                $.toast('资讯不存在');
                return;
            }
        });
    });
    //商家留言
    publicPageInit("#page-shop-liuyan", function (e, id, page) {
        $(page).off('click', '.gobackstore').on('click', '.gobackstore', function () {
            routerLoad('/shop_2/mchstore.html?mchid=' + $('.mch_id').val());
        });
        $(page).off('click', '.submit_liuyan').on('click', '.submit_liuyan', function () {
            if (requiredForm('#liuyan_form')) {
                setStoreLiuyan($('#liuyan_form').serialize(), function (obj) {
                    if (obj.success == "true") {
                        console.log(obj.msg);
                        $.router.load("#page-shop-liuyansuccess");
                    } else {
                        $.toast(obj.msg);
                    }
                });
            }
        });
    });
    //商家留言成功
    publicPageInit("#page-shop-liuyansuccess", function (e, id, page) {
        $(page).off('click', '.gobackstore').on('click', '.gobackstore', function () {
            routerLoad('/shop_2/mchstore.html?mchid=' + $('.mch_id').val());
        });
    });

    //资讯全部评价
    publicPageInit("#page-bbs-mchnews", function (e, id, page) {
        downloadDiv();
        parameters.bbs.mchnews.all.pageIndex = 0;
        parameters.bbs.mchnews.user.only_id = $('#news_id').val();
        parameters.bbs.mchnews.all.only_id = $('#news_id').val();

        //获得我参与的评论
        if (userIsAuth()) {
            getUserBBSList(parameters.bbs.mchnews.user, function (obj) {
                if (obj.length > 0) {
                    $(page).find('.user_bbs .list').html(template('bbs_temp', { list: obj })).show();
                }
                $(page).find('.user_bbs').hide();
            });
        } else {
            $(page).find('.user_bbs').hide();
        }
        loadBBSlist();
        //获得所有评论
        function loadBBSlist() {
            getBBSList(parameters.bbs.mchnews.all, function (params, obj) {
                params.pageIndex++;
                if (params.pageIndex == 1) $(page).find('.all_bbs .list').html('');
                if (obj.length > 0) {
                    $(page).find('.all_bbs .list').append(template('bbs_temp', { list: obj }));
                    loading = false;
                }
                if (obj.length < params.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($('.infinite-scroll'));
                    // 删除加载提示符
                    $('.infinite-scroll-preloader').remove();
                }
            });
        };
        //点赞
        $(page).find('.news_bbs').off('click', '.is_like_click').on('click', '.is_like_click', function () {
            var _self = $(this);
            if (_self.data('like') == "1") {
                $.toast("你已点过赞");
                return false;
            }
            setUserBBSLike({ bbs_id: _self.data('bbsid') }, function (obj) {
                if (obj.success == "true") {
                    var _span = _self.removeClass('is_like_0').addClass('is_like_1').data('like', 1).find('span');
                    _span.text(parseInt(_span.text()) + 1);
                }
                $.toast(obj.msg);
            });
            return false;
        });
        //无限滚动
        var loading = false;
        $(page).on('infinite', function () {
            if (loading) return;
            loading = true;
            loadBBSlist();
        });
    });
    //资讯评价详情
    publicPageInit("#page-bbs-mchnewsdetails", function (e, id, page) {
        downloadDiv();
        parameters.bbs.mchnewsdetails.all.pageIndex = 0;
        parameters.bbs.mchnewsdetails.all.bbs_id = $('#bbs_id').val();
        //加载评论楼主信息
        getBBSDetails({ bbs_id: $('#bbs_id').val() }, function (obj) {
            if (obj.length) {
                $(page).find('.user_bbs .list').html(template('bbs_details_temp', { list: obj }));
                parameters.bbs.mchnewsdetails.like_count = obj[0].LikeCount;
            }
        });
        loadBBSDetailsList();
        //加载评论回复列表
        function loadBBSDetailsList() {
            getBBSDetailsList(parameters.bbs.mchnewsdetails.all, function (params, obj) {
                params.pageIndex++;
                if (params.pageIndex == 1) $(page).find('.all_bbs .list').html('');
                if (obj.length > 0) {
                    $(page).find('.all_bbs .list').append(template('bbs_details_temp', { list: obj }));
                    loading = false;
                }
                if (obj.length < params.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(page).find('.infinite-scroll'));
                    // 删除加载提示符
                    $(page).find('.infinite-scroll-preloader').remove();
                    $(page).find('.more_container').show();
                }
            });
        };
        //无限滚动
        var loading = false;
        $(page).on('infinite', function () {
            if (loading) return;
            loading = true;
            loadBBSDetailsList();
        });
        //点赞
        $(page).find('.news_bbs').off('click', '.is_like_click').on('click', '.is_like_click', function () {
            var _self = $(this);
            if (_self.data('like') == "1") {
                $.toast("你已点过赞");
                return false;
            }
            setUserBBSLike({ bbs_id: _self.data('bbsid') }, function (obj) {
                if (obj.success == "true") {
                    var _span = _self.removeClass('is_like_0').addClass('is_like_1').data('like', 1).find('span');
                    _span.text(parseInt(_span.text()) + 1);
                }
                $.toast(obj.msg);
            });
            return false;
        });
        //赞的列表
        getBBSLikeList({ pageIndex: 0, pageSize: 6, bbs_id: $('#bbs_id').val() }, function (params, obj) {
            if (obj.length > 0) {
                $(page).find('.like_img .list').html(template('like_img_temp', { list: obj[0].List }));
            }
        })
    });
    //资讯评价点赞列表
    publicPageInit("#page-bbs-mchnewslike", function (e, id, page) {
        parameters.bbs.mchnewsdetails.like.pageIndex = 0;
        parameters.bbs.mchnewsdetails.like.bbs_id = $('#bbs_id').val();
        loadBBSLikeList();
        //加载点赞列表
        function loadBBSLikeList() {
            getBBSLikeList(parameters.bbs.mchnewsdetails.like, function (params, obj) {
                params.pageIndex++;
                if (params.pageIndex == 1) $(page).find('.like_list .list').html('');
                if (obj.length > 0) {
                    $(page).find('.like_list .list').append(template('like_list_temp', { list: obj[0].List }));
                    loading = false;
                }
                if (obj.length < params.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(page).find('.infinite-scroll'));
                    // 删除加载提示符
                    $(page).find('.infinite-scroll-preloader').remove();
                    if (obj.length > 0) {
                        var _visitorCount = parameters.bbs.mchnewsdetails.like_count - obj[0].TotalCount;
                        if (_visitorCount > 0) {
                            $(page).find('.more_container').text(_visitorCount + '位游客也赞过');
                        }
                    }
                    $(page).find('.more_container').show();
                }
            });
        };
        //无限滚动
        var loading = false;
        $(page).on('infinite', function () {
            if (loading) return;
            loading = true;
            loadBBSLikeList();
        });
    });

    //全部打赏人页面
    publicPageInit("#page-shop-reward", function (e, id, page) {
        parameters.shop.mchnews.reward.pageIndex = 0;
        parameters.shop.mchnews.reward.onlyId = $('#news_id').val();
        loadRewardList();
        //打赏列表
        function loadRewardList() {
            getRewardInfo(parameters.shop.mchnews.reward, function (params, obj) {
                params.pageIndex++;
                if (params.pageIndex == 1) {
                    $(page).find('.reward_list .list').html('');
                }
                if (obj.length > 0) {
                    $(page).find('.reward_list .list').append(template('rewardInfo_list_temp', { list: obj[0].List }));
                    loading = false;
                }
                if (obj.length < params.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(page).find('.infinite-scroll'));
                    // 删除加载提示符
                    $(page).find('.infinite-scroll-preloader').remove();
                }
                //if (obj.length > 0) {
                //    $(page).find('.reward_list').html(template('rewardInfo_list_temp', { count: obj[0].Count, list: obj[0].List }));
                //} else {
                //    //$.toast('暂无打赏记录');
                //    return;
                //}
            });
        };
        //无限滚动
        var loading = false;
        $(page).on('infinite', function () {
            if (loading) return;
            loading = true;
            loadRewardList();
        });
    });

    // 添加'refresh'监听器
    $(document).on('refresh', '.pull_content_reload', function (e) {
        window.location.reload();
    });
    $(document).on('refresh', '.pull_content_back', function (e) {
        $.router.back();
    });
    $.init();
});
//参数集合
var parameters = {
    shop: {
        isFirst: true,
        details: {
            loading: false,//防止重复加载详情
            shareDesc: '',//分享说明
            list: {},//商品数据集合
            mchParams: { pageIndex: 0, pageSize: 3, mch_id: 0, product_id: 0 }//加载店铺信息参数集合
        },
        mchnews: {
            bbslist: { bbs_type: 'mchnews', only_id: 0, pageIndex: 0, pageSize: 6 },
            recommend: { mch_id: 0, news_id: 0 },
            info: { news_id: 0, news_title: '', news_cover: '' },
            mchParams: { loading: false, pageIndex: 0, pageSize: 10, mch_id: 0 },
            reward: { onlyId: 0, pageIndex: 0, pageSize: 10 },
            author: {}
        },
        mchstore: {
            product: {},//商品列表参数
            news: { pageIndex: 0, pageSize: 10, mch_id: 0 }
        }
    },
    //评论
    bbs: {
        //资讯评论
        mchnews: {
            //用户参与的
            user: { only_id: 0, bbs_type: 'mchnews' },
            //全部
            all: { pageIndex: 0, pageSize: 10, only_id: 0, bbs_type: 'mchnews' }
        },
        //资讯评论详情
        mchnewsdetails: {
            //楼主获得点赞数量计算游客
            like_count: 0,
            //全部
            all: { pageIndex: 0, pageSize: 10, bbs_id: 0 },
            //点赞
            like: { pageIndex: 0, pageSize: 10, bbs_id: 0, }
        }
    }
};
//获取门店其他商品
function loadMchPro(params, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchStoreProductList2', params, function (data) {
        if (fun) fun(data);
    });
};
//获取门店其他商品
function loadTopProductReview(params, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetTopProductReview', params, function (data) {
        fun(data);
    });
};
//获得店铺相关信息
function getMchInfoByUser(mchId, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchInfoByUser', { mch_id: mchId }, function (data) {
        if (data.length > 0) {
            if (fun) fun(data[0]);
        } else {
            $.router.back();
            return;
        }
    });
};
//获得资讯详情信息
function getStoreNewsInfo(newsId, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetStoreNewsInfo', { news_id: newsId }, function (data) {
        if (fun) fun(data);
    });
};
//获得资讯详情信息
function getStoreNewsPreviewInfo(newsId, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/getStoreNewsPreviewInfo', { news_id: newsId }, function (data) {
        if (fun) fun(data);
    });
};
//获得店铺自定义分类
function getMchClass(mchId, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchProductClassList', { mch_id: mchId }, function (data) {
        if (fun) fun(data);
    });
};
//获得店铺售卖中的产品
function getMchStoreProductList2(params, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchStoreProductList2', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得店铺资讯列表
function getMchNewsList(params, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchNewsList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//关注店铺
function setMchBrowse(mch_id, is_collect, fun) {
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/MchBrowse', { mch_id: mch_id, is_collect: is_collect }, function (data) {
        if (fun) fun(eval(data));
    });
};
//店铺留言
function setStoreLiuyan(params, fun) {
    $.post('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetStoreLiuyan', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获得店铺推荐资讯列表
function getRecommendMchNews(params, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/RecommendMchNews', params, function (data) {
        if (fun) fun(data);
    });
};
//店铺资讯点赞
function setStoreNewsLike(params, fun) {
    if (window.localStorage["news_click_like_" + params.news_id] == 1) {
        $.toast('您已点过赞了，不能重复点赞');
        return;
    }
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetStoreNewsLike', params, function (data) {
        window.localStorage["news_click_like_" + params.news_id] = 1;
        if (fun) fun(data);
    });
};
//获得BBS评论列表
function getBBSList(params, fun) {
    $.getJSON('/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得BBS我参与的评论列表
function getUserBBSList(params, fun) {
    $.getJSON('/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetUserBBSList', params, function (data) {
        if (fun) fun(data);
    });
};
//BBS评论点赞
function setUserBBSLike(params, fun) {
    if (window.localStorage["bbs_click_like_" + params.bbs_id] == 1) {
        $.toast('您已点过赞了，不能重复点赞');
        return;
    }
    $.get('/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/SetUserBBSLike', params, function (data) {
        window.localStorage["bbs_click_like_" + params.bbs_id] = 1;
        var obj = eval(data);
        fun(obj);
    });
};
//获得BBS评论楼主详情
function getBBSDetails(params, fun) {
    $.getJSON('/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSDetails', params, function (data) {
        if (fun) fun(data);
    });
};
//获得BBS评论详情回复列表
function getBBSDetailsList(params, fun) {
    $.getJSON('/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSDetailsList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得BBS评论点赞列表
function getBBSLikeList(params, fun) {
    $.getJSON('/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSLikeList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得打赏列表
function getRewardInfo(params, fun) {
    $.getJSON('/Txooo/SalesV2/News/Ajax/RewardAjax.ajax/GetRewardInfo', params, function (data) {
        if (fun) fun(params, data);
    });
};
//查询打算是否打开
function rewardIsOpen(params, fun) {
    $.get('/Txooo/SalesV2/News/Ajax/RewardAjax.ajax/IsOpen', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//查询用户信息
function getUserInfo(params, fun) {
    $.get('/Txooo/SalesV2/Member/Ajax/UserAjax.ajax/GetUserInfo', params, function (data) {
        if (fun) fun(JSON.parse(data));
    });
};
//收藏、取消收藏商品
function addOrCancelCollectProduct(params, fun) {
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddOrCancelCollectProduct', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//添加商品足迹
function addOrUpdateUserBrowseFootprint(productId, mchClass) {
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddOrUpdateUserBrowseFootprint', { product_id: productId, mch_class: mchClass }, function (data) { });
};
//获得商品推广招商品牌信息
function getProductBrand(product_id, fun) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProdctBrand', { product_id: product_id }, function (data) {
        if (fun) fun(data);
    });
};
//设置店铺品牌推广记录
function setStoreBrandPop(params, fun) {
    $.get("/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetBrandPop", params, function (data) {
        if (fun) fun(data);
    });
};