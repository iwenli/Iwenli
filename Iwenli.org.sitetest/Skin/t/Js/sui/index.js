//赚钱排行
function getMoneyRank(params, fun) {
    var _url = 'https://11.u.93390.cn/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/GetMoneyRank';
    jsonp({
        url: _url,
        data: params,
        success: fun
    });
};
//获取模块信息
function getContentList(fun) {
    var _url = 'http://11.u.7518.cn/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/GetContentList';
    jsonp({
        url: _url,
        success: fun
    });
};
//推荐商品
function getRecommendProduct(params, fun) {
    var _url = 'https://11.t.93390.cn/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/GetRecommendProductForMaike';
    jsonp({
        url: _url,
        data: params,
        success: fun
    });
};
//分类
function getProductClassByParentIdV3(params, fun) {
    var _url = 'https://11.u.93390.cn/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProductClassByParentIdV3';
    jsonp({
        url: _url,
        data: params,
        success: fun
    });
};
//新版商品搜索
function searchProduct(params, fun) {
    var _url = 'https://11.t.93390.cn/Txooo/SalesV2/Shop/Ajax/ShopOpenAjaxV2.ajax/SearchProduct';
    jsonp({
        url: _url,
        data: params,
        success: fun
    });
};
$(function () {
    'use strict';
    //index
    publicPageInit('#page-index-index', function (e, id, page) {
        var _this = page,
            _advert = [],
            _rankTop = 10,  //赚钱排行轮播前多少名
            _maxIndex = 10,     //分页做多加载多少页activity-list
            //_pageIndex = 1,   //分页加载页码
            //_pageSize = 16,  //分页加载页尺寸
            _isLoadFinished = false;  //分页加载标签是否加载完毕

        parameters.product.searchParams.key = null;

        document.title = parameters.title[0];
        //进入此页面以后分类页面重新加载
        parameters.class.isFirstLoad = true;

        $(_this).find('.j-app-active.msg').eq(0).addClass('external')[0].href = '/news/index.html';
        $(_this).find('.j-search-input')[0].href = 'index/s.html';
        //网页获取用户经纬度
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (p) {
                sessionStorage['tx-coordinate'] = p.coords.latitude + ',' + p.coords.longitude;
            }, function (error) {//错误信息
                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        console.log("用户拒绝对获取地理位置的请求。");
                        break;
                    case error.POSITION_UNAVAILABLE:
                        console.log("位置信息是不可用的。");
                        break;
                    case console.log.TIMEOUT:
                        alert("请求用户地理位置超时。");
                        break;
                    case error.UNKNOWN_ERROR:
                        console.log("未知错误。");
                        break;
                }
            }, {
                    enableHighAccuracy: true,  //高经度获取
                    maximumAge: 30000,
                    timeout: 20000
                });
        }

        if (parameters.index.isFirstLoad) {
            parameters.index.isFirstLoad = false;
            init(true);
        }
        else {
            rankSwiperEvent();
            adSwiperEvent();
            templateSwiperEvent();
        }
        bindPageEvent();
        //初始化页面
        function init(isFirstLoad) {
            loadData(isFirstLoad);
        };

        function loadData(isFirstLoad) {
            loadRankDataAndBindPage(isFirstLoad);  //赚钱排行 
            loadContentDataAndBindPage(isFirstLoad);  //模块
            //loadProductsDataAndBindPage(isFirstLoad);  //商品
            $(_this).find('.user-ad-contioner').remove();
        }
        //轮播广告
        function loadAdInfoAndBindPage() {
            getIndexAdInfos(function (data) {
                if (data.count > 0) {
                    var tempname = 't-index-adinfo';
                    if (data.templateId == 200003) {
                        tempname = tempname + '-swiper';
                    } else {
                        tempname = tempname + '-other';
                    }
                    var html = template(tempname, { 'list': txapp.parseJSON(data.list) });
                    $(_this).find('.user-ad-contioner').empty().append(html);
                    if (data.templateId == 200003) {
                        //广告数量小于1  不轮播
                        if (data.count == 1) { return; }
                        adSwiperEvent();
                    }
                    else {
                        $(_this).find('.adinfo-other').addClass('adinfo-other_' + data.templateId);
                    }
                }
                else {
                    $(_this).find('.user-ad-contioner').remove();
                }
            });
        }
        //获取赚钱排行数据
        function loadRankDataAndBindPage(isFirstLoad) {
            var params = { 'top': _rankTop };
            //拉取数据填充页面
            getMoneyRank(params, function (obj) {
                //填充完数据显示页面
                if (obj.length > 0) {
                    var html = template('t-rank', { list: obj });
                    parameters.index.rankSwiper.htmlContent = html;
                    //$(_this).find('.rank-j-rank').html(html);
                    if ($.device.android || isFirstLoad) {  //ios下拉刷新不滚动
                        rankSwiperEvent();
                    }
                }
            });
        }
        //首页模块数据获取（tag & ajax）
        function loadContentDataAndBindPage(isFirstLoad) {
            var data = $('#d-home-contents').html();
            getContentList(function (obj) {
                bindContentDataToBindPage(obj);
            });
        }
        //首页模块解析
        function bindContentDataToBindPage(obj) {
            //填充完数据显示页面
            if (obj.length > 0) {
                $(_this).find('.j-tempalte-ontainer').empty();
                parameters.index.templateSwiperArray.length = 0; //如果拉取数据 先清空轮播数组数据
                for (var i = 0; i < obj.length; i++) {
                    if (obj[i].DetaiLink.length == 0) {
                        continue;
                    }
                    var templateName = 't-module-' + obj[i].TemplateId;
                    var htmlMain = template('t-module-main', { list: [{ 'ContentName': obj[i].ContentName, 'ContentNameImg': obj[i].ContentNameImg }] });
                    //处理linkURl 
                    //1：个性 商品
                    //2：分类是 商品分类
                    //3：落地直接就是网址
                    for (var j = 0; j < obj[i].DetaiLink.length; j++) {
                        switch (obj[i].DetaiLink[j].LinkClass) {
                            case 1:
                                obj[i].DetaiLink[j].src = '/shop.html?id=' + obj[i].DetaiLink[j].LinkIndex + '&source=shop';
                                break;
                            case 2:
                                obj[i].DetaiLink[j].src = '/index/market.html?q=' +
                                    obj[i].DetaiLink[j].LinkName + '&cids=' + obj[i].DetaiLink[j].LinkIndex + '&t=2';
                                break;
                            case 3:
                                obj[i].DetaiLink[j].src = obj[i].DetaiLink[j].LinkIndex;
                                break;
                        }
                    }
                    if (obj[i].TemplateId == 10047 && obj[i].ContentId == 1) {
                        //首页顶通
                        bindTopSliderSwiper({ list: obj[i].DetaiLink });
                        continue;
                    }

                    var htmlDetail = '';
                    var templateHtml = template(templateName, { list: [{ ContentId: obj[i].ContentId }] });
                    if (obj[i].TemplateId == 10040 || obj[i].TemplateId == 10041 || obj[i].TemplateId == 10043 || obj[i].TemplateId == 10044) {  //如果是轮播
                        htmlDetail = template('t-module-detail-3', { list: obj[i].DetaiLink });
                    }
                    else if (obj[i].TemplateId == 10039) {  //如果是模板5 单独处理
                        //前4条用模板2
                        //后2条用模板1
                        htmlDetail = template('t-module-detail-2', { list: obj[i].DetaiLink.slice(0, 4) });
                        htmlDetail += template('t-module-detail', { list: obj[i].DetaiLink.slice(4, 8) });
                    } else {
                        htmlDetail = template('t-module-detail', { list: obj[i].DetaiLink });
                    }
                    var $main = $(htmlMain);
                    var $dom = $(templateHtml);
                    if (obj[i].TemplateId == 10040 || obj[i].TemplateId == 10041 || obj[i].TemplateId == 10043 || obj[i].TemplateId == 10044) {  //轮播放到一个容器里面
                        $main.find('.tempalte-title').remove();
                        $main.find('.tempalte-content').addClass('template-swiper');
                    }
                    else {
                        for (var j = 0; j < $dom.find('.j-detail').length; j++) {
                            $dom.find('.j-detail').eq(j).append($(htmlDetail).find('a').eq(j));
                        }
                    }
                    $main.find('.tempalte-content').append($dom);
                    if (obj[i].TemplateId == 10039) {  //如果是模板5 单独添加样式
                        $main.find('.tempalte-content').addClass('module-5');
                    }
                    $(_this).find('.j-tempalte-ontainer').append($main);
                    //轮播处理 非轮播模块 懒加载图片
                    if (obj[i].TemplateId == 10040 || obj[i].TemplateId == 10041 || obj[i].TemplateId == 10043 || obj[i].TemplateId == 10044) {
                        var swiperContainerName = '.template-swiper-container-' + obj[i].ContentId;
                        parameters.index.templateSwiperArray.push({ content: htmlDetail, name: swiperContainerName, loop: obj[i].DetaiLink.length > 1 });
                        //templateSwiperEvent();
                    }
                    else {
                        //懒加载图片
                        $main.find('.lazy').lazyload({ threshold: 180, container: '#page-index-index .content' });
                    }
                }
                templateSwiperEvent();
                setTimeout(function () {
                    $.pullToRefreshDone('.infinite-scroll-bottom');
                }, 500);
                if (obj.length < 3) {
                    loadProductsDataAndBindPage(isFirstLoad);
                }
            }
            else {
                loadProductsDataAndBindPage(isFirstLoad);
            }
        }
        //推荐商品
        function loadProductsDataAndBindPage(isFirstLoad, callBack) {
            if (!isFirstLoad) {
                parameters.index.likeProductParams.pageIndex = 1;
                $(_this).find('.j-products').empty();
            }
            try {
                getRecommendProduct(parameters.index.likeProductParams, function (obj) {
                    if (obj.length > 0) {
                        if ($('.j-products').css('display') == 'none') {
                            $('.j-products').show();
                        }
                        var $html = $(template('t-productShow2', { Products: obj }));
                        //懒加载图片
                        $html.find('.lazy').lazyload({ threshold: 180, container: '#page-index-index .content' });
                        $(_this).find('.j-products').append($html);
                    }
                    if (obj.length == 0 || obj.length < parameters.index.likeProductParams.pageSize ||
                        ++parameters.index.likeProductParams.pageIndex > _maxIndex
                    ) {
                        _isLoadFinished = true;
                    }
                    if (callBack) callBack();
                });

            } catch (e) {
                _isLoadFinished = false;
            }
        }
        //绑定
        function rankSwiperEvent() {
            //绑定轮播图事件
            if (parameters.index.rankSwiper.swiperEvent != null) {
                parameters.index.rankSwiper.swiperEvent.destroy(true, true);
                parameters.index.rankSwiper.swiperEvent = null;
            }
            $(_this).find('.rank-j-rank').empty().append(parameters.index.rankSwiper.htmlContent);
            parameters.index.rankSwiper.swiperEvent =
                new Swiper('.rank-j-rank', {
                    autoplay: 5000,//自动滑动间隔
                    direction: 'vertical', //垂直轮播
                    height: $('.rank-scroll .c1').height() * 1 + 2,//slide高度
                    onlyExternal: true,  //值为true时，slide无法拖动，只能使用扩展API函数例如slideNext() 或slidePrev()或slideTo()等改变slides滑动。
                    autoplayDisableOnInteraction: false,  //用户控制以后是否禁止自动播放
                    loop: true //是否循环
                });
        }
        //绑定排行榜轮播
        function rankSwiperEvent() {
            //绑定轮播图事件
            if (parameters.index.rankSwiper.swiperEvent != null) {
                parameters.index.rankSwiper.swiperEvent.destroy(true, true);
                parameters.index.rankSwiper.swiperEvent = null;
            }
            $(_this).find('.rank-j-rank').empty().append(parameters.index.rankSwiper.htmlContent);
            parameters.index.rankSwiper.swiperEvent =
                new Swiper('.rank-j-rank', {
                    autoplay: 5000,//自动滑动间隔
                    direction: 'vertical', //垂直轮播
                    height: $('.rank-scroll .c1').height() * 1 + 2,//slide高度
                    onlyExternal: true,  //值为true时，slide无法拖动，只能使用扩展API函数例如slideNext() 或slidePrev()或slideTo()等改变slides滑动。
                    autoplayDisableOnInteraction: false,  //用户控制以后是否禁止自动播放
                    loop: true //是否循环
                });
        }
        //绑定模块轮播
        function templateSwiperEvent() {
            //绑定轮播图事件
            for (var i = 0; i < parameters.index.templateSwiperArray.length; i++) {
                if (parameters.index.templateSwiperArray[i].swiperEvnet != null) {
                    //如果存在先销毁
                    parameters.index.templateSwiperArray[i].swiperEvnet = null;
                }
                $(parameters.index.templateSwiperArray[i].name).find('.j-detail').empty().append(parameters.index.templateSwiperArray[i].content);
                parameters.index.templateSwiperArray[i].swiperEvnet = new Swiper(parameters.index.templateSwiperArray[i].name, {
                    autoplay: 5000,//自动滑动间隔
                    effect: 'cube',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    loop: parameters.index.templateSwiperArray[i].loop,  //是否循环
                    paginationClickable: true,  //控制器点击是否生效
                    autoplayDisableOnInteraction: false,  //用户控制以后是否禁止自动播放
                    cube: {
                        slideShadows: false,
                        shadow: false
                    }
                });
            }

        }
        //绑定用户轮播
        function adSwiperEvent() {
            //绑定轮播图事件
            if (parameters.index.adSwiper != null) {
                //如果存在先销毁
                //parameters.index.adSwiper.destroy(false);
                parameters.index.adSwiper = null;
            }
            //alert('开始初始化adSwiper');
            parameters.index.adSwiper = new Swiper('.adinfo-swiper-container', {
                autoplay: 3000,//自动滑动间隔
                direction: 'horizontal', //水平轮播
                pagination: '.swiper-pagination', //控制器容器
                paginationType: 'bullets',  //‘bullets’  圆点（默认）   ‘fraction’  分式 
                //onInit: function (swiper) {
                //    alert('adSwiper初始化了');  //提示Swiper的当前索引
                //},
                //onSlideChangeStart: function (swiper) {
                //    $.toast(swiper.activeIndex);
                //},
                loop: true,  //是否循环
                paginationClickable: true,  //控制器点击是否生效
                autoplayDisableOnInteraction: false //用户控制以后是否禁止自动播放
            });
        }
        //加载顶通轮播页面
        function bindTopSliderSwiper(data) {
            console.log('存在首页顶通');
            console.log(data);
            if (data.list.length > 0) {
                var html = template('t-index-slider', data);
                $(_this).find('.topSliderSwiper').empty().append(html);
                //轮播数量小于1  不轮播
                if (data.count == 1) { return; }
                topSliderSwiperEvent();
            }
        }
        //绑定顶通轮播事件
        function topSliderSwiperEvent() {
            //绑定轮播图事件
            if (parameters.index.topSliderSwiper != null) {
                //如果存在先销毁
                parameters.index.topSliderSwiper = null;
            }
            parameters.index.topSliderSwiper = new Swiper('.topSliderSwiper .swiper-container', {
                autoplay: 3000,//自动滑动间隔
                direction: 'horizontal', //水平轮播
                pagination: '.swiper-pagination', //控制器容器
                paginationType: 'bullets',  //‘bullets’  圆点（默认）   ‘fraction’  分式 
                loop: true,  //是否循环
                paginationClickable: true,  //控制器点击是否生效
                autoplayDisableOnInteraction: false //用户控制以后是否禁止自动播放
            });
        }

        //绑定页面事件
        function bindPageEvent() {
            //显示任务足迹和返回顶部
            var bottom = '3.2rem';
            $(_this).find('.footprint-Top').css('bottom', bottom);
            //根据机型动态设置高度
            setTimeout(function () {
                $('.footprint-Top').animate({
                    'opacity': 1,
                    'right': '0.75rem'
                }, 200);
            }, 2000);//延时加载

            //监听滚动条 显示返回顶部按钮 
            $(_this).find(".content").on('scroll', function () {
                var scrollTop = $(this).scrollTop();
                if (scrollTop > 100) {
                    $('.print_top').animate({
                        'opacity': 1,
                        'height': '2rem',
                        'margin-bottom': '0.5rem'
                    }, 400);
                } else {
                    $('.print_top').animate({
                        'opacity': 0,
                        'height': 0,
                        'margin-bottom': '0'
                    }, 400);
                }

                //下滑搜索背景变化
                var opacity = scrollTop / 183;
                opacity = opacity < 0 ? 0 : opacity > 100 ? 100 : opacity;
                $('#page-index-index .searchbar-cover').animate({
                    'opacity': opacity
                }, 10);
            });


            //动画效果 - 返回顶部
            goTopAnimate();

            //预处理懒加载图片 防止返回页面懒加载事件不触发
            $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-index-index .content' });

            ////输入框按钮  已经迁移到页面
            //$(page).off('click', '#search').on('click', '#search', function () {
            //    routerLoad('index/s.html#page-index-search');
            //});

            // 加载flag
            var loading = false;
            // 注册上拉加载数据事件
            $(document).off('infinite').on('infinite', '.infinite-scroll-bottom', function () {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                try {
                    //拉取数据填充页面
                    loadProductsDataAndBindPage(true, function () {
                        // 重置加载flag
                        loading = false;
                        if (_isLoadFinished) {//加载完成
                            // 加载完毕，则注销无限加载事件，以防不必要的加载
                            $.detachInfiniteScroll($('.infinite-scroll'));
                            // 删除加载提示符
                            $(_this).find('.infinite-scroll-preloader,.j-tagLoadToast').remove();
                            $(_this).find('.content').append(' <div class="j-tagLoadToast" style="padding: 0.5rem;text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
                            return;
                        }
                        //容器发生改变,如果是js滚动，需要刷新滚动
                        $.refreshScroller();
                    });
                } catch (e) {
                    loading = false;
                    $.toast(e.message);
                }
            });
        }
    });

    //product.search
    publicPageInit('#page-index-search', function (e, id, page) {
        var _this = page;
        init();
        //初始化页面
        function init() {
            document.title = parameters.title[3];
            $(_this).find('.j-sk').val(parameters.product.searchParams.key);
            loadData();
            bindPageEvent();
        };
        //加载页面数据
        function loadData() {
            loadSearchKeyAndBingPage();
        }
        //绑定页面事件
        function bindPageEvent() {
            //获取焦点
            if (!$.device.ios) {
                $(_this).find('.j-sk').focus();
            }

            //绑定全部删除按钮
            $(_this).find('.j-removeAll').unbind().bind('click', function () {
                deleteSearchKey($(_this).find('.j-keytag-con a'), true);
            });
            //绑定搜索按钮
            $(_this).find('.j-search').unbind().bind('click', function () {
                searchCheck($(_this).find('.j-sk').val());
            });
            //绑定移动键盘搜索
            $('#product-search-form').unbind('search').bind('search', function () {
                searchCheck($(_this).find('.j-sk').val());
            });
        }

        //绑定搜索记录到页面
        function loadSearchKeyAndBingPage() {
            //第1步先加载ajax
            loadSearchKeyByAjax();
            //第2步读取sessionStorage
            var searchKeys = loadSearchKeyByLocation();
            if (searchKeys == null || searchKeys == '') {
                return;
            }

            //第3步 绑定记录
            var html = template('t-keytag', { list: searchKeys });
            $(_this).find('.j-keytag-con').empty().append(html);
            //绑定搜索记录标签事件
            bingSearchTagEvent();
        }

        //搜索记录ajax
        function loadSearchKeyByAjax() {
            if (!userIsAuth() || sessionStorage['tx-searchKey-isLoad'] == 1) {  //未登录不执行 || 如果已经加载过不执行
                return;
            }
            getSearchKey(parameters.product.keyParams, function (obj) {
                if (obj.length > 0) {
                    sessionStorage['tx-searchKey-isLoad'] = 1;
                    if (localStorage['tx-searchKey'] != null && localStorage['tx-searchKey'].length > 0) {
                        var array = localStorage['tx-searchKey'].split(',');
                        array = array.concat(obj);
                        localStorage['tx-searchKey'] = arrayUnique(array).join();
                    }
                    else {
                        localStorage['tx-searchKey'] = obj.join();
                    }
                }
            });
        }

        //搜索记录location
        function loadSearchKeyByLocation() {
            if (localStorage['tx-searchKey'] != null) {
                return localStorage['tx-searchKey'].split(',');
            }
            else {
                return null;
            }
        }
        //绑定搜索标签事件
        function bingSearchTagEvent() {
            var $dom = $(_this).find('.j-keytag-con a');
            var timeout = 0;
            if ($.device.ios) {
                //ios
                $(_this).find('.j-keytag-con a').off().on({
                    touchstart: function (e) {
                        var $this = $(this);
                        timeout = setTimeout(function () {
                            timeout = 0;
                            //删除该关键词
                            deleteSearchKey($this, false);
                        }, 500);
                        e.preventDefault();
                    },
                    touchmove: function () {
                        clearTimeout(timeout);
                        timeout = 0;
                        return false;
                    },
                    touchend: function () {
                        clearTimeout(timeout);
                        if (timeout != 0) {
                            var $this = $(this);
                            var key = $this.text();
                            $(_this).find('.j-sk').val(key);
                            searchCheck(key);
                        }
                        return false;
                    }
                });
            }
            else {
                //安卓
                $(_this).find('.j-keytag-con a').unbind('click').bind("click", function () {
                    var $this = $(this);
                    var key = $this.text();
                    $(_this).find('.j-sk').val(key);
                    searchCheck(key);
                });
                $(_this).find('.j-keytag-con a').bind("mousedown", function () {
                    var $this = $(this);
                    timeout = setTimeout(function () {
                        //删除该关键词
                        deleteSearchKey($this, false);
                    }, 800);
                });
                $(_this).find('.j-keytag-con a').bind("mouseup", function () {
                    clearTimeout(timeout);
                });
            }
        }
        //删除搜索历史
        function deleteSearchKey(dom, isAll) {
            if (dom.length == 0) {
                return;
            }
            var msg = isAll ? '确认删除全部历史记录吗?' : '确认删除该历史记录吗?';
            $.confirm(msg, function () {
                var data = '';
                //dom.forEach(m => data = data + $(m).text() + ',');
                dom.forEach(function (m) { data = data + $(m).text() + ','; });
                data = data.substring(0, data.length - 1);
                $(dom).remove();

                var lastSearch = removeByValue(loadSearchKeyByLocation(), data).join();
                //删除本地记录
                if (isAll || lastSearch == '') {
                    localStorage.removeItem("tx-searchKey");
                }
                else {
                    localStorage["tx-searchKey"] = lastSearch;
                }
                if (!userIsAuth()) {  //未登录不执行
                    return;
                }
                parameters.product.delParams.key = data;
                $.showPreloader();
                //setTimeout(function () {
                //    $.hidePreloader();
                //}, 1000);
                deleteSearchRecord(parameters.product.delParams, function (obj) {
                    //$.hidePreloader();
                    if (obj.success == 'true') {
                        //服务器删除成功
                    }
                });
            });
        }
        //搜索入口
        function searchCheck(key) {
            if (key == null || key.length == 0) {
                $.toast('关键词不能为空!');
                return;
            }
            //记录搜索词
            addSearchKey(key);
            parameters.product.isFirstLoad = true;
            $.router.load('sr.html?key=' + key);
        }
        //记录搜索词
        function addSearchKey(key) {
            var data = loadSearchKeyByLocation();
            if (data != null && data.length > 0) {
                data = removeByValue(data, key);
                if (data.length > 0) {
                    localStorage['tx-searchKey'] = key + ',' + data.join();
                }
                else {
                    localStorage['tx-searchKey'] = key;
                }
            }
            else {
                localStorage['tx-searchKey'] = key;
            }
            loadSearchKeyAndBingPage();
        }
    });

    //product.result
    publicPageInit('#page-index-result', function (e, id, page) {
        var _this = page
            , loading = false//下拉刷新 加载flag
            , _key = decodeURI(getUrlParam('key'));

        parameters.product.searchParams.key = _key;
        document.title = parameters.title[5] + parameters.product.searchParams.key;

        bindPageEvent();
        if (parameters.product.isFirstLoad) {
            if (_key == null || _key.length == 0) {
                $.router.back();
                return;
            }
            else {
                $('#page-index-result').find('.j-sk').val(_key);
            }
            parameters.product.isFirstLoad = false;
            init();
        }

        function init() {
            //固定为首页搜索  [1首页搜索 2分类商品页面  3分类里面的推荐标签]
            parameters.product.searchParams.type = 1;
            parameters.product.searchParams.max = 0;
            parameters.product.searchParams.min = 0;
            parameters.product.searchParams.class_ids = 0;
            parameters.product.searchParams.order = 0;
            parameters.product.searchParams.is_desc = 1;
            parameters.product.searchParams.page_size = 22;

            $('.buttons-tab .btn-active').eq(0).addClass('select').siblings().removeClass('select');
            $('.btn-select .btn-active').removeClass('select').eq(0).addClass('select');
            loadData();
        };

        //加载页面数据
        function loadData() {
            s(true);
        }
        function clearMark() {
            //移除遮罩
            $(_this).removeClass('mark-warp');
            $(_this).find('.panel-warp').removeClass('active');
        }
        //重置筛选数据
        function resetFilterData() {
            $(_this).find('.panel-warp input').val('');
            $(_this).find('.panel-warp .j-val-class').text('');
        }
        //去查找数据
        function goSearch() {
            clearMark();
            //执行搜索
            s(true);
        }
        //绑定页面事件
        function bindPageEvent() {
            //更多
            $(_this).find('.more').off().on('click', function () {
                if ($('.more-warp').css('display') == 'none') {
                    $('.more-warp').show();
                } else {
                    $('.more-warp').hide();
                }
            });
            //隐藏更多操作
            $('.more-warp').off().on('click', function () {
                $('.more-warp').hide();
            });
            //返回首页
            $(_this).find('.j-back-index').off().on('click', function () {
                $.router.backLoad('/index.html');
            });
            //重新搜索
            $(_this).find('.j-sk').off().on('click', function () {
                $.router.back();
            });
            //筛选
            $(_this).find('.j-btn-filter').off().on('click', function () {
                $(_this).find('.panel-warp').addClass('active');
            });
            //重置筛选条件
            $(_this).find('.j-btn-reset').off().on('click', function () {
                resetFilterData();
            });
            //筛选条件确定
            $(_this).find('.j-btn-ok').off().on('click', function () {
                var _max = $(_this).find('.j-val-max').val() * 1;
                var _min = $(_this).find('.j-val-min').val() * 1;
                var _classId = $(_this).find('.j-val-class').attr('data-id') * 1;

                if (_max != 0 && _min != 0 && _max < _min) {
                    $.toast('最高价不能低于最低价');
                    return;
                }
                //var _className = $(_this).find('.j-val-class').text();
                //var _key = $(_this).find('.j-val-key').val();
                parameters.product.searchParams.max = _max;
                parameters.product.searchParams.min = _min;
                //parameters.product.searchParams.class_ids = _classId;
                //parameters.product.searchParams.key = _key;
                goSearch();
            });
            //绑定tab
            $(_this).find('.btn-active').off().on('click', function () {
                var _type = parseInt($(this).attr('data-type'));
                clearMark();
                //添加标识
                if (_type >= 0) {
                    $(_this).find('.buttons-tab .btn-active').removeClass('select');
                    $(this).addClass('select');
                }
                else {
                    $(_this).find('.btn-select .btn-active').removeClass('select');
                    $(this).addClass('select');
                    $('.btn-active[data-type="0"]').text($(this).text());
                }

                if (_type == 0) {  //综合
                    if ($('.mark-warp').length > 0) {
                        clearMark();
                    }
                    else {
                        $(_this).addClass('mark-warp');
                    }
                }
                else {
                    switch (_type) {
                        case 1:  //销量
                            parameters.product.searchParams.order = 2;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case 2:   //牛币
                            parameters.product.searchParams.order = 3;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case -1:   //综合
                            parameters.product.searchParams.order = 0;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case -2:   //上新
                            parameters.product.searchParams.order = 1;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case -3:    //价格升序
                            parameters.product.searchParams.order = 4;
                            parameters.product.searchParams.is_desc = 0;
                            break;
                        case -4:    //价格降序
                            parameters.product.searchParams.order = 4;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                    }
                    goSearch();
                }
            });
            //绑定下拉刷新
            bindInfiniteEvent();

            //设置右上角消息提示  延时3秒执行  
            setTimeout(function () {
                setMsgState(_this);
            }, 3000);
        }

        //商品搜索入口
        function s(isFirst, callBack) {
            if (isFirst) {
                parameters.product.searchParams.page_index = 0;
                $(_this).find('.content').scrollTop(0);
            }
            //重置搜索参数
            $.showPreloader();
            searchProduct(parameters.product.searchParams, function (obj) {
                bingProductsToPage(obj, isFirst, callBack)
            });
        }
        //搜索结果展示
        function bingProductsToPage(data, isFirst, callBack) {
            if (isFirst) {
                $(_this).find('.tempalte-content').empty();
                $(_this).find('.infinite-scroll-preloader').show();
            }
            if (isFirst && data.length == 0) {
                //没有搜索到商品 获取推荐商品
                var param = {
                    'pageIndex': 1, 'pageSize': 22
                };
                getRecommendProduct(param, function (data) {
                    if (param.pageIndex == 1) {
                        var html = template('t-searchNoProducts', { 'list': data });
                        $(_this).find('.tempalte-content').empty().append(html);
                        $(_this).find('.infinite-scroll-preloader').hide();
                    }
                    //懒加载图片
                    $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-index-result .content' });
                });
            }
            else {
                if (isFirst) {
                    var htmlCon = '<ul class="j-products" style="display: block;"><div class="j-clear" style="clear: both;"></ul>';
                    $(_this).find('.tempalte-content').empty().append(htmlCon);
                }
                var html = template('t-productShow1', { Products: data });
                $(_this).find('.tempalte-content').find('.j-products .j-clear').before(html);
                //懒加载图片
                $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-index-result .content' });
                if (callBack) callBack();
                if (data.length < parameters.product.searchParams.page_size) {
                    //加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(_this).find('.infinite-scroll'));
                    $(_this).find('.infinite-scroll-preloader').hide();
                    $(_this).find('.content .tempalte-content').append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
                }
            }
        }
        // 注册下拉加载数据事件
        function bindInfiniteEvent() {
            $(document).unbind('infinite').on('infinite', '.infinite-scroll-bottom', function () {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                parameters.product.searchParams.page_index++;  //页码+1
                //拉取数据填充页面
                s(false, function () {
                    // 重置加载flag
                    loading = false;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                });

            });
        }
    });

    //shop.index  所有店铺
    publicPageInit('#page-shop-index', function (e, id, page) {
        var _this = page
            , _type = 1
            , _pageType = 1 //1表示全部店铺 2表示分类展示店铺
            , _mchType = getUrlParam('type')
            , _key = decodeURI(getUrlParam('key'))
            , _parentKey = decodeURI(getUrlParam('parent'))
            , loading = false;//下拉刷新 加载flag

        //_mchType = 5 是企业商户   =7是微商店铺
        var _mchName = ['企业商户', '微商店铺'];
        if (_mchType != 7) {
            _mchType = 5;
        }
        $(_this).find('.title').text(_mchType == 5 ? _mchName[0] : _mchName[1]);
        document.title = _mchType == 5 ? _mchName[0] : _mchName[1];

        bindPageEvent();
        if (location.search.indexOf('parent') > -1) {  //判断页面来源
            _pageType = 2;
            parameters.allShop.searchParams.key = getUrlParam('id');
            $(_this).find('.title').text(_parentKey + '-' + _key);
            $(_this).find('.j-search').remove();
            document.title = parameters.title[2];
        }
        if (parameters.allShop.isFirstLoad || parameters.allShop.nextMchType != _mchType) {
            parameters.allShop.nextMchType = _mchType;
            parameters.allShop.isFirstLoad = false;
            document.title = parameters.title[7];
            init();
        }
        //进入此页面以后分类页面重新加载
        parameters.class.isFirstLoad = true;

        //初始化页面
        function init() {
            loadData();
        };
        //加载页面数据
        function loadData() {
            loadMchDataAndBindPage(1, true);
        }
        //获取店铺数据
        function loadMchDataAndBindPage(selectorType, isFirst, callBack) {
            if (isFirst) {
                parameters.allShop.getMchParams.pageIndex = 1;
            }
            //重置搜索参数
            parameters.allShop.getMchParams.selectorType = selectorType;
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 1000);
            parameters.allShop.searchParams.mchType =
                parameters.allShop.getMchParams.mchType = _mchType;
            if (_pageType == 1) {
                getAllMch(parameters.allShop.getMchParams, function (obj) {
                    bindMchInfoToPage(obj, isFirst, selectorType, callBack);
                });
            }
            else if (_pageType == 2) {
                search(parameters.allShop.searchParams, function (obj) {
                    bindMchInfoToPage(obj.data, isFirst, selectorType, callBack);
                });
            }
        }
        //绑定店铺数据到页面
        function bindMchInfoToPage(obj, isFirst, selectorType, callBack) {
            //$.hidePreloader();
            parameters.allShop.TotalCount = obj.TotalCount;
            if (isFirst) {
                $(_this).find('.module-tempalte').empty();
            }
            if (obj.TotalCount > 0) {  //有店铺数据  解析
                var type = selectorType - 1;
                if (isFirst) {
                    var htmlCon = '<div class="shop-info-wrapper"><ul class="j-mch-result"></ul></div>';
                    $(_this).find('.module-tempalte').eq(type).empty().append(htmlCon);
                }
                var html = template('t-mchResult', { list: obj.Models });
                $(_this).find('.j-mch-result').append(html);
                //懒加载图片
                $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-shop-index .content' });
                if (callBack) callBack();
                if (obj.Models.length < parameters.allShop.searchParams.pageSize) {
                    $(_this).find('.infinite-scroll-preloader').eq(type).hide();
                    $(_this).find('.content .module-tempalte').eq(type).append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
                }
            } else {
                var html = template('t-searchNoMchs', { list: [] });
                $(_this).find('.module-tempalte').empty().append(html);
            }
        }
        //绑定页面事件
        function bindPageEvent() {
            bindGoStore(_this);
            //绑定搜索按钮按钮
            $(_this).on('click', '.j-search', function () {
                routerLoad('#page-shop-search');
            });
            //绑定不同排序
            $(_this).on('click', '.tab-link', function () {
                _type = $(this).attr('data-type');
                if (parameters.allShop.TotalCount == 0) {
                    $(_this).find('.tab-link').unbind();
                }
                else {
                    //绑定下拉刷新
                    bindInfiniteEvent();
                    _type = $(this).attr('data-type');
                    loadMchDataAndBindPage(_type, true);
                }
            });
            //绑定下拉刷新
            bindInfiniteEvent();
        }
        // 注册下拉加载数据事件
        function bindInfiniteEvent() {
            $(document).unbind('infinite').on('infinite', '.infinite-scroll-bottom', function () {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                parameters.allShop.getMchParams.pageIndex++;  //页码+1
                //如果超过总数  取消事件
                var totalPage = parameters.allShop.TotalCount / parameters.allShop.getMchParams.pageSize;
                totalPage = Math.ceil(totalPage);//向上取整
                if (parameters.allShop.getMchParams.pageIndex > totalPage) {
                    //// 加载完毕，则注销无限加载事件，以防不必要的加载
                    //$.detachInfiniteScroll($(_this).find('.infinite-scroll'));
                    //// 删除加载提示符
                    //$(_this).find('.infinite-scroll-preloader').remove();
                    //$(_this).find('.content .j-tagLoadToast').remove();
                    //$(_this).find('.content .module-tempalte').append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
                    // 重置加载flag
                    loading = false;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                    return;
                }
                //拉取数据填充页面
                loadMchDataAndBindPage(_type, false, function () {
                    // 重置加载flag
                    loading = false;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                });

            });
        }
    });

    //shop.search
    publicPageInit('#page-shop-search', function (e, id, page) {
        var _this = page
            , _mchType = getUrlParam('type').substr(0, 1);

        init();

        //初始化页面
        function init() {
            //_mchType = 5 是企业商户   =7是微商店铺
            var _mchName = ['企业商户', '微商店铺'];
            if (_mchType != 7) {
                _mchType = 5;
            }

            document.title = (_mchType == 5 ? _mchName[0] : _mchName[1]) + '-搜索';

            loadData();
            bindPageEvent();
        };
        //加载页面数据
        function loadData() {
            loadSearchKeyAndBingPage();
        }
        //绑定页面事件
        function bindPageEvent() {
            //获取焦点
            if (!$.device.ios) {
                $(_this).find('.j-sk').focus();
            }

            //绑定全部删除按钮
            $(_this).find('.j-removeAll').unbind().bind('click', function () {
                deleteSearchKey($(_this).find('.j-keytag-con a'), true);
            });
            //绑定搜索按钮
            $(_this).find('.j-search').unbind().bind('click', function () {
                searchCheck($(_this).find('.j-sk').val());
            });
            //绑定移动键盘搜索
            $('#mch-search-form').unbind('search').bind('search', function () {
                searchCheck($(_this).find('.j-sk').val());
            });
        }

        //绑定搜索记录到页面
        function loadSearchKeyAndBingPage() {
            //第1步先加载ajax
            loadSearchKeyByAjax();
            //第2步读取sessionStorage
            var searchKeys = loadSearchKeyByLocation();
            if (searchKeys == null || searchKeys == '') {
                return;
            }

            //第3步 绑定记录
            var html = template('t-keytag', { list: searchKeys });
            $(_this).find('.j-keytag-con').empty().append(html);
            //绑定搜索记录标签事件
            bingSearchTagEvent();
        }

        //搜索记录ajax
        function loadSearchKeyByAjax() {
            if (!userIsAuth() || sessionStorage['tx-shop-searchKey-isLoad'] == 1) {  //未登录不执行 || 如果已经加载过不执行
                return;
            }
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 1000);
            getSearchKey(parameters.shop.keyParams, function (obj) {
                //$.hidePreloader();
                if (obj.length > 0) {
                    sessionStorage['tx-shop-searchKey-isLoad'] = 1;
                    if (localStorage['tx-shop-searchKey'] != null && localStorage['tx-shop-searchKey'].length > 0) {
                        var array = localStorage['tx-shop-searchKey'].split(',');
                        array = array.concat(obj);
                        localStorage['tx-shop-searchKey'] = arrayUnique(array).join();
                    }
                    else {
                        localStorage['tx-shop-searchKey'] = obj.join();
                    }
                }
            });
        }

        //搜索记录location
        function loadSearchKeyByLocation() {
            if (localStorage['tx-shop-searchKey'] != null) {
                return localStorage['tx-shop-searchKey'].split(',');
            }
            else {
                return null;
            }
        }
        //绑定搜索标签事件
        function bingSearchTagEvent() {
            var $dom = $(_this).find('.j-keytag-con a');
            var timeout = 0;
            if ($.device.ios) {
                //ios
                $(_this).find('.j-keytag-con a').on({
                    touchstart: function (e) {
                        var $this = $(this);
                        timeout = setTimeout(function () {
                            timeout = 0;
                            //删除该关键词
                            deleteSearchKey($this, false);
                        }, 500);
                        e.preventDefault();
                    },
                    touchmove: function () {
                        clearTimeout(timeout);
                        timeout = 0;
                        return false;
                    },
                    touchend: function () {
                        clearTimeout(timeout);
                        if (timeout != 0) {
                            var $this = $(this);
                            var key = $this.text();
                            $(_this).find('.j-sk').val(key);
                            searchCheck(key);
                        }
                        return false;
                    }
                });
            }
            else {
                //安卓
                $(_this).find('.j-keytag-con a').unbind('click').bind("click", function () {
                    var $this = $(this);
                    var key = $this.text();
                    $(_this).find('.j-sk').val(key);
                    searchCheck(key);
                });
                $(_this).find('.j-keytag-con a').bind("mousedown", function () {
                    var $this = $(this);
                    timeout = setTimeout(function () {
                        //删除该关键词
                        deleteSearchKey($this, false);
                    }, 800);
                });
                $(_this).find('.j-keytag-con a').bind("mouseup", function () {
                    clearTimeout(timeout);
                });
            }

        }
        //删除搜索历史
        function deleteSearchKey(dom, isAll) {
            if (dom.length == 0) {
                return;
            }
            var msg = isAll ? '确认删除全部历史记录吗?' : '确认删除该历史记录吗?';
            $.confirm(msg, function () {
                var data = '';
                dom.forEach(function (m) { data = data + $(m).text() + ',' });
                data = data.substring(0, data.length - 1);
                $(dom).remove();

                var lastSearch = removeByValue(loadSearchKeyByLocation(), data).join();
                //删除本地记录
                if (isAll || lastSearch == '') {
                    localStorage.removeItem("tx-shop-searchKey");
                }
                else {
                    localStorage["tx-shop-searchKey"] = lastSearch;
                }
                if (!userIsAuth()) {  //未登录不执行
                    return;
                }
                parameters.shop.delParams.key = data;
                $.showPreloader();
                //setTimeout(function () {
                //    $.hidePreloader();
                //}, 1000);
                deleteSearchRecord(parameters.shop.delParams, function (obj) {
                    //$.hidePreloader();
                    if (obj.success == 'true') {
                        //删除数据接口
                    }
                });
            });
        }
        //店铺搜索入口
        function searchCheck(key) {
            if (key == null || key.length == 0) {
                $.toast('关键词不能为空!');
                return;
            }
            txapp.hideInput();
            //记录搜索词
            addSearchKey(key);
            //绑定搜索参数
            parameters.shop.searchParams.mchType = _mchType;
            sessionStorage['current-shop-key'] = key;
            //另一个页面呈现结果
            $('#page-shop-result').find('.j-sk').val(parameters.product.searchParams.key);
            $('#page-shop-result').find('.module-tempalte').eq(0).empty();
            parameters.shop.isFirstLoad = true;
            routerLoad('#page-shop-result');
        }
        //记录搜索词
        function addSearchKey(key) {
            var data = loadSearchKeyByLocation();
            if (data != null && data.length > 0) {
                data = removeByValue(data, key);
                if (data.length > 0) {
                    localStorage['tx-shop-searchKey'] = key + ',' + data.join();
                }
                else {
                    localStorage['tx-shop-searchKey'] = key;
                }
            }
            else {
                localStorage['tx-shop-searchKey'] = key;
            }
            loadSearchKeyAndBingPage();
        }
    });

    //shop.result
    publicPageInit('#page-shop-result', function (e, id, page) {
        var _this = page
            , _type = 1
            , loading = false;//下拉刷新 加载flag
        bindMsg(_this); //绑定交互按钮
        if (sessionStorage['current-shop-key'] == null) {
            $.router.load("#page-shop-search");
            return;
        }
        else {
            parameters.shop.searchParams.key = sessionStorage['current-shop-key'];
            $(_this).find('.j-sk').val(parameters.shop.searchParams.key);
        }
        parameters.shop.isFirstLoad = false;
        init();

        function init() {
            document.title = parameters.title[6] + parameters.shop.searchParams.key;
            $(_this).find('.tab-link').eq(0).addClass('active').siblings().removeClass('active');
            $(_this).find('.j-search-con .tab').eq(0).addClass('active').siblings().removeClass('active');
            loadData();
            bindPageEvent();
        };

        //加载页面数据
        function loadData() {
            s(1, true);
        }
        //绑定页面事件
        function bindPageEvent() {
            bindGoStore(_this);
            //重新搜索
            $(_this).find('.j-sk').unbind().bind('click', function () {
                $.router.back();
            });
            //绑定不同排序
            $(_this).find('.tab-link').unbind().bind('click', function () {
                if (parameters.shop.TotalCount == 0) {
                    $(_this).find('.tab-link').unbind();
                }
                else {
                    //绑定下拉刷新
                    bindInfiniteEvent();
                    _type = $(this).attr('data-type');
                    s(_type, true);
                }
            });
            //绑定下拉刷新
            bindInfiniteEvent();
            //设置右上角消息提示  延时3秒执行  
            setTimeout(function () {
                setMsgState(_this);
            }, 3000);
        }

        //店铺搜索入口
        function s(selectorType, isFirst, callBack) {
            if (isFirst) {
                parameters.shop.searchParams.pageIndex = 1;
            }
            //重置搜索参数
            parameters.shop.searchParams.selectorType = selectorType;
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 1000);
            search(parameters.shop.searchParams, function (obj) {
                //$.hidePreloader();
                bingProductsToPage(obj.data, isFirst, callBack)
            });
        }
        //搜索结果展示
        function bingProductsToPage(data, isFirst, callBack) {
            parameters.shop.TotalCount = data.TotalCount;
            if (isFirst) {
                $(_this).find('.module-tempalte').empty();
            }
            if (data.TotalCount == 0) { //店铺数据为空
                var html = template('t-searchNoMchs', { list: [] });
                $(_this).find('.module-tempalte').empty().css({ 'margin-top': '20%' }).append(html);
            }
            else {
                var type = parameters.shop.searchParams.selectorType - 1;
                if (isFirst) {
                    var htmlCon = '<div class="shop-info-wrapper"><ul class="j-mch-result"></ul></div>';
                    $(_this).find('.module-tempalte').css({ 'margin-top': '0' }).eq(type).empty().append(htmlCon);
                }
                var html = template('t-mchResult', { list: data.Models });
                $(_this).find('.j-mch-result').append(html);
                //懒加载图片
                $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-shop-result .content' });
                if (callBack) callBack();
            }
        }
        // 注册下拉加载数据事件
        function bindInfiniteEvent() {
            $(document).unbind('infinite').on('infinite', '.infinite-scroll-bottom', function () {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                parameters.shop.searchParams.pageIndex++;  //页码+1
                //如果超过总数  取消事件
                var totalPage = parameters.shop.TotalCount / parameters.shop.searchParams.pageSize;
                totalPage = Math.ceil(totalPage);//向上取整
                if (parameters.shop.searchParams.pageIndex > totalPage) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(_this).find('.infinite-scroll'));
                    // 删除加载提示符
                    $(_this).find('.infinite-scroll-preloader').remove();
                    $(_this).find('.content .j-tagLoadToast').remove();
                    $(_this).find('.content').append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
                    // 重置加载flag
                    loading = false;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                    return;
                }
                //拉取数据填充页面
                s(_type, false, function () {
                    // 重置加载flag
                    loading = false;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                });

            });
        }
    });

    //class.index
    publicPageInit('#page-class-index', function (e, id, page) {
        var _this = page
            , pageType = location.search.substr(6)  //来源 2为店铺  其他重置为1
            , currentParentId = 0;  //当前显示的父级id

        if (pageType != 2) {
            pageType = 1;
        }

        if (parameters.class.isFirstLoad) {
            parameters.class.isFirstLoad = false;
            init();
        }

        bindPageEvent();


        //初始化页面
        function init() {
            document.title = parameters.title[pageType];
            $(_this).find('.j-title').text(document.title);
            loadData();
        };
        //加载页面数据
        function loadData() {
            getClassData(1, 0);
        }
        //绑定页面事件
        function bindPageEvent() {
            //绑定搜索按钮按钮
            $(_this).find('.j-search').unbind().bind('click', function () {
                routerLoad('#page-shop-search');
            });
            //绑定父级分类
            bindParentClassEvent();
            //绑定子级分类
            bindChildClassEvent();
        }
        //获取分类  
        //参数parentId=1表示父级分类，mchClass=0表示获取有商品的分类
        function getClassData(parentId, mchClass) {
            var params = {
                'parentid': parentId, 'mch_class': mchClass
            };
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 1000);
            getProductClassByParentIdV3(params, function (data) {
                //$.hidePreloader();
                if (data.length > 0) {
                    if (parentId == 1) {  //父级菜单
                        //追加推荐标签
                        var obj = [{
                            "ClassId": 15,
                            "ClassName": "推荐标签",
                            "ParentId": 1,
                            "RadioNums": []
                        }];

                        //菜单
                        var html = template('t-classParent', { list: obj.concat(data) });
                        $(_this).find('.j-class-parent').empty().append(html);
                        $(_this).find(".j-class-parent li").eq(0).addClass('current');
                        //第一项加载
                        getClassData($(_this).find(".j-class-parent .current").attr('data-id'), 0);
                        //容器
                        var html1 = template('t-classParent-Container', { list: obj.concat(data) });
                        $(_this).find('.j-class-parent-container').empty().append(html1);
                        $(_this).find(".j-class-parent-container li").hide().eq(0).show();
                    } else {  //子级
                        //菜单
                        var html = template('t-classChild', { list: data });
                        var childContainerId = '.j-container-' + parentId;
                        $(_this).find(childContainerId).empty().append(html);
                        $(_this).find(childContainerId + ' img').lazyload({ threshold: 180, container: '#page-class-index .tab-menu' });
                    }
                }
            });
        }
        //父级分类点击事件绑定
        function bindParentClassEvent() {
            $(_this).find(".j-class-parent").off('click').on('click', 'li', function () {
                if (currentParentId == $(this).attr('data-id')) {  //当前显示的页面  不执行
                    return;
                }
                currentParentId = $(this).attr('data-id');
                $(_this).find(".j-class-parent li").removeClass('current');
                $(this).addClass('current');

                var childContainerId = '.j-container-' + currentParentId;
                $(_this).find(".j-class-parent-container .tab-ul-li").hide();
                $(_this).find(childContainerId).parent().show();
                //已经加载过数据  不在加载
                if ($(_this).find(childContainerId).find('li').length > 0) {
                    return;
                }
                getClassData(currentParentId, 0);
            });
        }
        //子级分类点击事件绑定
        function bindChildClassEvent() {
            $(_this).find('.j-class-parent-container').off('click').on('click', '.tab-ul-li li', function () {
                var _id = $(this).find('a').data('id');
                var _key = $(this).find('a').data('key');
                var _parentId = $(this).parent().attr('data-parentId');
                var _type = _parentId == 15 ? 3 : 2;  //2普通分类  3热门标签
                if (pageType == 1) {
                    $.router.load('/index/market.html?q=' + _key + '&cids=' + _id + '&t=' + _type);
                } else if (pageType == 2) {
                    var parentKey = $(_this).find('.current').text();
                    //TODO:店铺分类结果页
                    window.location.href = '/index/mch.html?parent=' + parentKey + '&key=' + key + '&id=' + id;
                }
            });
        }
    });

    //class.result-product
    publicPageInit('#page-class-product', function (e, id, page) {
        var _this = page
            , key = $(_this).find('.search_id').val()
            , parentId = $(_this).find('.search_id').attr('data-parentId')
            , searchParams = {
                'key': key, 'type': 4, 'selectorType': 1, 'pageIndex': 1, 'pageSize': 20
            };//搜索

        //如果是带参数进入，参数优先
        if (getUrlParam('key') != '') {
            key = getUrlParam('key');
            $(_this).find('.j-title').text(decodeURI(getUrlParam('name')).split('#')[0]);
            //searchParams = {
            //    'key': key, 'type': 4, 'selectorType': 1, 'pageIndex': 1, 'pageSize': 20
            //};  //搜索
            searchParams.key = key;

            //如果是从其他页面进入 设置分类为页面
            $(_this).find('.icon-menu').removeClass('back')[0].href = '/index/class.html';
        } else {
            $(_this).find('.icon-menu').addClass('back')[0].href = '#';
        }
        //热门标签映射商品
        if (parentId == 15) {
            searchParams.type = 5;
        }

        if (key == null || key.length == 0) {
            $.router.load('#page-class-index');
            return;
        }

        init();

        //初始化页面
        function init() {
            document.title = parameters.title[1];
            loadData();
        };
        //加载页面数据
        function loadData() {
            $(_this).find('.j-products').append('');
            loadProductsAndBindPage(true);
        };
        var loading = false;
        $(page).off('infinite').on('infinite', function () {
            if (loading) return;
            loading = true;
            loadProductsAndBindPage(false, function (_length) {
                loading = false;
            });
        });
        //加载商品 并绑定到页面
        function loadProductsAndBindPage(isFirst, callBack) {
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 1000);
            search(searchParams, function (data) {
                if (searchParams.pageIndex == 1) $(_this).find('.j-products').empty();
                searchParams.pageIndex++;
                var obj = data;
                obj.data.Models = processProductsData(data.data.Models);
                //$.hidePreloader();
                if (obj.data.Models.length > 0) {
                    if ($('.j-products').css('display') == 'none') {
                        $('.j-products').show();
                    }
                    var $html = $(template('t-products', { list: obj.data.Models }));
                    $(_this).find('.j-products').append($html);
                    //懒加载图片
                    $html.find('.lazy').lazyload({ threshold: 180, container: '#page-class-product .content' });
                    //if (obj.data.TotalCount == 1) {  //1条隐藏底部分割线
                    //    $(_this).find('.j-products li').addClass('hide-border');
                    //}
                }
                if (obj.data.Models.length < searchParams.pageSize) {
                    $(_this).find('.j-products').append('亲，没有更多了.');
                    $.detachInfiniteScroll($(page).find('.infinite-scroll'));
                }
                if (callBack) callBack(obj.data.Models.length);
            });
        }

    });

    //商品详情
    publicPageInit("#page-shop-details", function (e, id, page) {
        $.closeModal();
        setScrollerNum();
        //返回顶部
        (function () {
            var bottom = '4.2rem';
            if ($('.download').length == 0) {
                bottom = '2.2rem';
            }
            goTopEvent(page, bottom, bottom);
        })();
        //主体信息加载
        (function () {
            var info = $.parseJSON($(page).find('.product_data').html());
            console.log(info);
            if (info.length == 0) {
                $.router.load('/shop/noPro.html?pid=' + location.search.substr(3)); return;
            }
            parameters.shop.details.list = info[0];
            parameters.shop.details.list.is_tx_app = txapp.isApp();
            parameters.shop.details.list.bundling_count = parameters.shop.details.list.is_bundling ? parseInt(JSON.parse(parameters.shop.details.list.bundling_json).count) : 1;
            parameters.shop.details.shareDesc = parameters.shop.details.list.product_name;

            var $info = $(page).find('.shop_info');
            if (parameters.shop.details.list.rebate_state > 0) {
                //$info.addClass('revate_box');
                $(page).find('.j_add_cart').remove();
                if (parameters.shop.details.list.rebate_state > 1) {
                    var $nav = $(page).find('.nav_list');
                    $nav.addClass('revate_nav');
                    var _title = parameters.shop.details.list.rebate_state == 3 ? '活动结束倒计时:' : '活动已暂停:';
                    var _sequence = parameters.shop.details.list.rebate_state == 3 ? true : false;
                    var _maxMillisecond = parameters.shop.details.list.rebate_countdown * 24 * 60 * 60 * 1000;
                    var _date = parameters.shop.details.list.rebate_state == 3 ?
                        parameters.shop.details.list.rebate_end_time : parameters.shop.details.list.suspend_statr_time;

                    var _html = ['<div class="countdown_title">',
                        _title,
                        '</div><div class="countdown">',
                        '<span>0</span>天 <span>0</span><span>0</span>:<span>0</span><span>0</span>:<span>0</span><span>0</span>',
                        '</div>'];
                    $nav.find('.li_btn').html(_html.join(''));
                    //活动倒计时 CountDownTime(时间, 所需展示的标签选择器, true/false（倒计时/正计时,默认true）,正计时触发回调的最大毫秒,回调)
                    CountDownTime(_date, $(page).find('.nav_list .countdown'), _sequence, _maxMillisecond, function () {
                        location.reload();
                    });
                }
            }

            //加载轮播图
            $(page).find('.details_lunbo  .swiper-wrapper').html(template('lunbo_temp', { list: parameters.shop.details.list.product_imgs.split(',') }));
            var swiperDetails = $(page).find('.details_lunbo');
            var swiperPagination = $(page).find('.details_lunbo .swiper-pagination');
            new Swiper(swiperDetails, {
                pagination: swiperPagination,
                paginationClickable: true,
                autoplay: 3000,
                loop: true
            });
            //加载主信息
            $info.html(template('shop_info_temp', { info: parameters.shop.details.list }));
            $(page).find('.nav_collect').data('collect', parameters.shop.details.list.is_collect).addClass('is_collect_' + parameters.shop.details.list.is_collect);
            if (txapp.isApp()) {
                $(page).find('.buttons-tab').addClass('appHeader');
            } else {
                $(page).find('.buttons-tab').removeClass('appHeader');
            }
            $(page).find(".content").scrollTop(0);
        })();
        //页面内容点击事件处理
        (function () {
            //返回
            $(page).off('click', '.click_back').on('click', '.click_back', function () {
                if (txapp.isApp() && getUrlParam('source') != 'shop') {//兼容苹果会直接返回 首页的问题
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
                addOrCancelCollectProduct({
                    pid: getUrlParam('id'), operationType: _type
                }, function (obj) {
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
                if (txapp.isApp()) {
                    window.txapp.store({ mchid: parameters.shop.details.list.mch_id });
                } else {
                    routerLoad('/shop_2/mchstore.html?mchid=' + parameters.shop.details.list.mch_id + '&show=1');
                }
            });
            //推广品牌点击
            $(page).off('click', '.store_brand').on('click', '.store_brand', function () {
                var mch_id = $(this).data('mch');
                setStoreBrandPop({
                    brand_id: $(this).data('brand'), product_id: parameters.shop.details.list.product_id
                }, function () {
                    if (txapp.isApp()) {
                        window.txapp.store({ mchid: mch_id, show: 1 });
                    } else {
                        routerLoad('/shop_2/mchstore.html?&show=1&mchid=' + mch_id);
                    }
                });
            });
            //轮播图预览
            $(page).off('click', '.swiper-slide').on('click', '.swiper-slide', function () {
                var _index = $(this).index() - 1;
                _index = _index == parameters.shop.details.list.product_imgs.split(',').length ? 0 : _index;
                _index = _index == -1 ? parameters.shop.details.list.product_imgs.split(',').length - 1 : _index;
                txapp.imgPreview({ imgurls: parameters.shop.details.list.product_imgs, imgindex: _index });
            });
            //商品详情图预览
            $(page).find('.product_details').off('click', 'img').on('click', 'img', function () {
                var _index = 0;
                var imgurls = '';
                var $this = this;
                $(page).find('.product_details img').each(function (i, o) {
                    imgurls = imgurls + ',' + $(o).attr('src');
                    if ($this == o) {
                        _index = i;
                    }
                });
                txapp.imgPreview({ imgurls: imgurls, imgindex: _index });
            });
            //规格图片预览
            $('#product_property_list').off('click', 'img').on('click', 'img', function () {
                txapp.imgPreview({ imgurls: $(this).prop('src').split(',')[0], imgindex: 0 });
            });
            //点击售后
            $(page).off('click', '.shop_explain').on('click', '.shop_explain', function () {
                $.router.load('/shop_2/explain.html');
            });
            //规格属性数据
            var property_list;
            var map_id = 0;
            $(page).find('#product_property_list').html('');//该弹框是全局容器，所以要清空后使用
            var _flag = 0;  //0加入购物车  1立即购买
            //打开立即购买和加入购物车
            $(page).off('click', '.open_property_list').on('click', '.open_property_list', function () {
                _flag = $(this).data('id');
                $.popup($(page).find('#product_property_list'));
                if ($(page).find('#product_property_list').text().length == 0) {
                    property_list = $.parseJSON($(page).find('.product_property').html());
                    $(page).find('#product_property_list').html(template('product_property_temp', { list: property_list }));
                    var _defaultObj;
                    var _defaultData = $.grep(property_list, function (j, k) { return j.is_default == true && j.remain_inventory > 0; });
                    if (_defaultData.length == 0)
                        _defaultData = $.grep(property_list, function (j, k) { return j.remain_inventory > 0; });

                    if (_defaultData.length > 0) _defaultObj = _defaultData[0];
                    propertySelect(_defaultObj);
                    $('#product_property_list .pro_count').val(parameters.shop.details.list.bundling_count);
                }
            });
            //选择规格赋予样式变化
            function propertySelect(obj) {
                if (obj) {
                    map_id = obj.map_id;
                    $('#product_property_list .property_img').prop('src', obj.img + ',1,80,80,3');
                    $('#product_property_list .property_price').text(obj.price);
                    $('#product_property_list .remain_inventory').text(obj.remain_inventory);
                    //样式变化
                    $('#product_property_list .property_select').removeClass('current');
                    $('#product_property_list .map_id' + obj.map_id).addClass('current');
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
                    _pro_count = _pro_count + parameters.shop.details.list.bundling_count;
                } else if (_pro_count > parameters.shop.details.list.bundling_count) {
                    _pro_count = _pro_count - parameters.shop.details.list.bundling_count;
                }
                $('#product_property_list .pro_count').val(_pro_count);
            });
            //规格确定
            $('#product_property_list').off('click', '.go_shop_order').on('click', '.go_shop_order', function () {
                $.closeModal();
                if ($(this).data('show') == 2) {
                    $.alert('该商品仅展示不售卖');
                    return false;
                }
                if (txapp.isApp()) {
                    txapp.confirmOrder({ id: getUrlParam('id'), proProperty: map_id, isVirtural: parameters.shop.details.list.is_virtual, count: $('#product_property_list .pro_count').val() });
                    return false;
                }
                if (userIsAuth()) {
                    var _count = $('#product_property_list .pro_count').val();
                    if (_flag == 0) {
                        //加入购物车
                        addShopCart({ property_map_id: map_id, shop_count: _count }, function (data) {
                            $.toast('成功');
                        });
                    } else {
                        var _url = '/order_2/handorder.html?map_id=' + map_id + '&shop_count=' + _count;
                        $.router.load(_url);
                    }
                } else {
                    window.location.href = _url;
                }
            });
        })();
        //滚动事件监听与处理
        (function () {
            //滚动事件监听
            $(page).find(".content").on('scroll', function () {
                $.toast('', 100, 'hide_toast');//为了 临时解决iPhone7+点击评价有白块的问题
                $('.hide_toast').hide();
                var lunboHeight = $(page).find('.details_lunbo').height();
                var lunboTop = $(page).find('.details_lunbo').offset().top;
                var dNumber = lunboTop / lunboHeight;
                $(page).find('.d_header').css({ opacity: -dNumber });
                var scrollNum = $(page).find(".content").scrollTop();
                if (scrollNum > titnum2 - titHeight * 1.2 && scrollNum < titnum3 - titHeight * 1.2) {
                    $(page).find(".title2").addClass('current').siblings().removeClass('current');
                } else if (scrollNum > titnum3 - titHeight * 1.2) {
                    $(page).find(".title3").addClass('current').siblings().removeClass('current');
                } else {
                    $(page).find(".title1").addClass('current').siblings().removeClass('current');
                }
            });
            //滚动图高度
            var winWidth = $(page).find(".content").width();
            $(page).find('.carry').css('height', winWidth + 'px');

            $(page).off('click', '.title1').on('click', '.title1', function () {
                setTimeout(function () {
                    $(page).find(".content").scrollTop(0);
                }, 200);
                $(page).find(".title1").addClass('current').siblings().removeClass('current');
            });
            $(page).off('click', '.title2').on('click', '.title2', function () {
                setTimeout(function () {
                    $(page).find(".content").scrollTop(titnum2 - titHeight);
                }, 200);
                $(page).find(".title2").addClass('current').siblings().removeClass('current');
            });
            $(page).off('click', '.title3').on('click', '.title3', function () {
                setTimeout(function () {
                    $(page).find(".content").scrollTop(titnum3 - titHeight);
                }, 200);
                $(page).find(".title3").addClass('current').siblings().removeClass('current');
            });
            var loading = false;
            //滑动到底部加载详情信息
            $(page).on('infinite', function () {
                if (loading) return;
                loading = true;
                $.detachInfiniteScroll($('.infinite-scroll'));
                if (parameters.shop.details.list.product_details_type == 0) {
                    parameters.shop.details.list.product_details = parameters.shop.details.list.product_details.replace(/\n/g, '</p><p>').replace(/<br\/>/g, '').replace(/<p><\/p>/g, '');
                }
                $(page).find('.product_details').html(parameters.shop.details.list.product_details);
                $(page).find('.footer_p_name').html(template('footer_p_name_temp', parameters.shop.details.list));
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
                loadMchPro1({ 'top': 3, 'product_id': parameters.shop.details.mchParams.product_id }, function (obj) {
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
                loadTopProductReview({
                    product_id: getUrlParam('id')
                }, function (obj) {
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
                    if (obj.length > 0 && obj[0].menu_count > 0) {
                        $(page).find('.product_brand_list').html(template('product_brand_temp', obj[0])).show();
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
            titnum2 = $(page).find(".evaluate").offset().top + $(page).find(".content").scrollTop() - (txapp.isApp() ? 20 : 0);
            titnum3 = $(page).find(".details_index").offset().top + $(page).find(".content").scrollTop() - (txapp.isApp() ? 20 : 0);
            titHeight = $(page).find(".evaluate").outerHeight();
        }


    });
    publicPageInit("#page-shop-details-v2", function (e, id, page) {
        var _productId = $(page).find('.product_id').val();
        $.closeModal();
        setScrollerNum();

        //返回顶部
        (function () {
            var bottom = '4.2rem';
            if ($('.download').length == 0) {
                bottom = '2.2rem';
            }
            goTopEvent(page, bottom, bottom);
        })();
        //主体信息加载
        (function () {
            var info = $.parseJSON($(page).find('.product_data').html());
            if (info.length == 0) {
                $.router.load('/shop/noPro.html?pid=' + location.search.substr(3)); return;
            }
            parameters.shop.details.list = info[0];
            parameters.shop.details.list.is_tx_app = txapp.isApp();
            parameters.shop.details.list.bundling_count = parameters.shop.details.list.is_bundling ? parseInt(JSON.parse(parameters.shop.details.list.bundling_json).count) : 1;
            parameters.shop.details.shareDesc = parameters.shop.details.list.product_name;
            //加载轮播图
            $(page).find('.details_lunbo  .swiper-wrapper').html(template('lunbo_temp', { list: parameters.shop.details.list.product_imgs.split(',') }));
            var swiperDetails = $(page).find('.details_lunbo');
            var swiperPagination = $(page).find('.details_lunbo .swiper-pagination');
            new Swiper(swiperDetails, {
                pagination: swiperPagination,
                paginationClickable: true,
                autoplay: 3000,
                loop: true
            });
            //加载主信息
            $(page).find('.shop_info').html(template('shop_info_temp', { info: parameters.shop.details.list }));
            $(page).find('.nav_collect').data('collect', parameters.shop.details.list.is_collect).addClass('is_collect_' + parameters.shop.details.list.is_collect);
            if (txapp.isApp()) {
                $(page).find('.buttons-tab').addClass('appHeader');
            } else {
                $(page).find('.buttons-tab').removeClass('appHeader');
            }
            $(page).find(".content").scrollTop(0);
        })();
        //页面内容点击事件处理
        (function () {
            //返回
            $(page).off('click', '.click_back').on('click', '.click_back', function () {
                if (txapp.isApp() && getUrlParam('source') != 'shop') {//兼容苹果会直接返回 首页的问题
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
                addOrCancelCollectProduct({
                    pid: getUrlParam('id'), operationType: _type
                }, function (obj) {
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
                if (txapp.isApp()) {
                    window.txapp.store({ mchid: parameters.shop.details.list.mch_id });
                } else {
                    routerLoad('/shop_2/mchstore.html?mchid=' + parameters.shop.details.list.mch_id + '&show=1');
                }
            });
            //推广品牌点击
            $(page).off('click', '.store_brand').on('click', '.store_brand', function () {
                var mch_id = $(this).data('mch');
                setStoreBrandPop({
                    brand_id: $(this).data('brand'), product_id: parameters.shop.details.list.product_id
                }, function () {
                    if (txapp.isApp()) {
                        window.txapp.store({ mchid: mch_id, show: 1 });
                    } else {
                        routerLoad('/shop_2/mchstore.html?&show=1&mchid=' + mch_id);
                    }
                });
            });
            //轮播图预览
            $(page).off('click', '.swiper-slide').on('click', '.swiper-slide', function () {
                var _index = $(this).index() - 1;
                _index = _index == parameters.shop.details.list.product_imgs.split(',').length ? 0 : _index;
                _index = _index == -1 ? parameters.shop.details.list.product_imgs.split(',').length - 1 : _index;
                txapp.imgPreview({ imgurls: parameters.shop.details.list.product_imgs, imgindex: _index });
            });
            //商品详情图预览
            $(page).find('.product_details').off('click', 'img').on('click', 'img', function () {
                var _index = 0;
                var imgurls = '';
                var $this = this;
                $(page).find('.product_details img').each(function (i, o) {
                    imgurls = imgurls + ',' + $(o).attr('src');
                    if ($this == o) {
                        _index = i;
                    }
                });
                txapp.imgPreview({ imgurls: imgurls, imgindex: _index });
            });
            //规格图片预览
            $('#product_property_list').off('click', 'img').on('click', 'img', function () {
                txapp.imgPreview({ imgurls: $(this).prop('src').split(',')[0], imgindex: 0 });
            });
            //点击售后
            $(page).off('click', '.shop_explain').on('click', '.shop_explain', function () {
                $.router.load('/shop_2/explain.html');
            });
            //规格属性数据
            var property_list;
            var map_id = 0;
            $(page).find('#product_property_list').html('');//该弹框是全局容器，所以要清空后使用
            //打开立即购买
            $(page).off('click', '.open_property_list').on('click', '.open_property_list', function () {
                $.popup($(page).find('#product_property_list'));
                if ($(page).find('#product_property_list').text().length == 0) {
                    property_list = $.parseJSON($(page).find('.product_property').html());
                    var _sum = 0;
                    var _propertyTreeList = [];
                    $.each(property_list, function (i, o) {
                        _sum += o.remain_inventory;
                        getPropertyTreeList(o.json_info.split(';'), _propertyTreeList);
                        o.property_array = [];
                        $.each(o.json_info.split(';'), function (j, k) {
                            var _array = k.split(':');
                            if (_array.length > 0) {
                                var _parentName = $.grep(_propertyTreeList, function (y) { return y.PropertyName == _array[0]; });
                                if (_parentName.length > 0) {
                                    var _childName = $.grep(_parentName[0].ChildList, function (y) { return y.PropertyName == _array[1]; });
                                    o.property_array.push({ name: k, id: _parentName[0].Id + ':' + _childName[0].Id });
                                }
                            }
                        });
                    });

                    //console.log(JSON.stringify(_propertyTreeList));
                    var _propertyShow = $(page).find('#product_property_list').html(template('product_property_temp', { list: _propertyTreeList }));
                    if (property_list.length > 0) {
                        _propertyShow.find('.property_img').prop('src', parameters.shop.details.list.img + ',1,80,80,3');
                        _propertyShow.find('.property_price').text(parameters.shop.details.list.price);
                        _propertyShow.find('.remain_inventory').text(_sum);
                    }
                }
            });
            //规格选择
            $('#product_property_list').off('click', '.property_select').on('click', '.property_select', function () {
                var _parentName = $(this).siblings('.parent_name').text();
                if ($(this).hasClass('un_click')) {
                    return false;
                }
                if ($(this).hasClass('current')) {
                    $(this).removeClass('current'); _parentName = '-++-';
                } else {
                    $(this).addClass('current').siblings('.property_select').removeClass('current');
                }
                var _selectName = [];
                $(page).find('.select_property_div .pro_spec .current').each(function (i, o) {
                    _selectName.push($(o).siblings('.parent_name').data('id') + ':' + $(o).data('id'));
                });
                var _hasList = [];
                $.each(property_list, function (i, o) {
                    var _hasCount = 0;
                    for (var i = 0; i < _selectName.length; i++) {
                        if ($.grep(o.property_array, function (y) { return y.id == _selectName[i] }).length > 0) {
                            _hasCount++;
                        }
                    }
                    if (_selectName.length == _hasCount) {
                        _hasList.push(o.property_array);
                    }
                });
                $(page).find('.select_property_div .property_select').addClass('un_click');
                //已选择规格不存在的规格组合数组
                $.each(_hasList, function (i, o) {
                    $.each(o, function (j, k) {
                        $(page).find('.select_property_div .index-' + k.id.split(':')[1]).removeClass('un_click');
                    });
                });
            });
            //规格第一项选择更换图片
            $('#product_property_list').off('click', '.select_property_div .property-1 .property_select').on('click', '.select_property_div .property-1 .property_select', function () {

            });
            //获得数组不包含的项目
            function compentArray(array1, array2) {
                var array = [];
                for (var i = 0; i < array2.length; i++) {
                    if (array1.indexOf(array2[i]) > -1) {
                        array.push(array1);
                    } else {
                        return;
                    }
                }
                return array;
            };
            //获得规格属性列表
            function getPropertyTreeList(a, _propertyTreeList) {
                $.each(a, function (j, k) {
                    var _array = k.split(':');
                    if (_array.length > 0) {
                        var _nameObj = {};
                        var _parentName = $.grep(_propertyTreeList, function (y) { return y.PropertyName == _array[0]; });
                        if (_parentName.length == 0) {
                            _nameObj = {
                                PropertyName: _array[0], Id: _propertyTreeList.length + 1, ChildList: [{
                                    PropertyName: _array[1],
                                    Id: (_propertyTreeList.length + 1) + '-' + 1
                                }]
                            };
                        } else {
                            _propertyTreeList = $.grep(_propertyTreeList, function (y) { return y.PropertyName != _array[0]; });
                            var _childName = $.grep(_parentName[0].ChildList, function (y) {
                                return y.PropertyName == _array[1];
                            });
                            if (_childName.length == 0) {
                                _parentName[0].ChildList.push({ PropertyName: _array[1], Id: _parentName[0].Id + '-' + (_parentName[0].ChildList.length + 1) });
                            }
                            _nameObj = _parentName[0];
                        }
                        _propertyTreeList.push(_nameObj);
                    }
                });
            }
            //数量加减
            $('#product_property_list').off('click', '.operation_num').on('click', '.operation_num', function () {
                var _pro_count = parseInt($('#product_property_list .pro_count').val());
                if ($(this).hasClass('num_up')) {
                    if (_pro_count >= parseInt($('#product_property_list .remain_inventory').text())) {
                        $.toast('就剩这么多库存了，快联系卖家上货吧');
                        return false;
                    }
                    _pro_count = _pro_count + parameters.shop.details.list.bundling_count;
                } else if (_pro_count > parameters.shop.details.list.bundling_count) {
                    _pro_count = _pro_count - parameters.shop.details.list.bundling_count;
                }
                $('#product_property_list .pro_count').val(_pro_count);
            });
            //规格确定
            $('#product_property_list').off('click', '.go_shop_order').on('click', '.go_shop_order', function () {
                $.closeModal();
                if ($(this).data('show') == 2) {
                    $.alert('该商品仅展示不售卖');
                    return false;
                }
                if (txapp.isApp()) {
                    txapp.confirmOrder({
                        id: getUrlParam('id'), proProperty: map_id, isVirtural: parameters.shop.details.list.is_virtual, count: $('#product_property_list .pro_count').val()
                    });
                    return false;
                }
                var _url = '/shop/order.html?id=' + parameters.shop.details.list.product_id + '&proProperty=' + map_id + '&count=' + $('#product_property_list .pro_count').val();
                if (userIsAuth()) {
                    $.router.load(_url);
                } else {
                    window.location.href = _url;
                }
            });
        })();
        //滚动事件监听与处理
        (function () {
            //滚动事件监听
            $(page).find(".content").on('scroll', function () {
                $.toast('', 100, 'hide_toast');//为了 临时解决iPhone7+点击评价有白块的问题
                $('.hide_toast').hide();
                var lunboHeight = $(page).find('.details_lunbo').height();
                var lunboTop = $(page).find('.details_lunbo').offset().top;
                var dNumber = lunboTop / lunboHeight;
                $(page).find('.d_header').css({ opacity: -dNumber });
                var scrollNum = $(page).find(".content").scrollTop();
                if (scrollNum > titnum2 - titHeight * 1.2 && scrollNum < titnum3 - titHeight * 1.2) {
                    $(page).find(".title2").addClass('current').siblings().removeClass('current');
                } else if (scrollNum > titnum3 - titHeight * 1.2) {
                    $(page).find(".title3").addClass('current').siblings().removeClass('current');
                } else {
                    $(page).find(".title1").addClass('current').siblings().removeClass('current');
                }
            });
            //滚动图高度
            var winWidth = $(page).find(".content").width();
            $(page).find('.carry').css('height', winWidth + 'px');

            $(page).off('click', '.title1').on('click', '.title1', function () {
                setTimeout(function () {
                    $(page).find(".content").scrollTop(0);
                }, 200);
                $(page).find(".title1").addClass('current').siblings().removeClass('current');
            });
            $(page).off('click', '.title2').on('click', '.title2', function () {
                setTimeout(function () {
                    $(page).find(".content").scrollTop(titnum2 - titHeight);
                }, 200);
                $(page).find(".title2").addClass('current').siblings().removeClass('current');
            });
            $(page).off('click', '.title3').on('click', '.title3', function () {
                setTimeout(function () {
                    $(page).find(".content").scrollTop(titnum3 - titHeight);
                }, 200);
                $(page).find(".title3").addClass('current').siblings().removeClass('current');
            });
            var loading = false;
            //滑动到底部加载详情信息
            $(page).on('infinite', function () {
                if (loading) return;
                loading = true;
                $.detachInfiniteScroll($('.infinite-scroll'));
                if (parameters.shop.details.list.product_details_type == 0) {
                    parameters.shop.details.list.product_details = parameters.shop.details.list.product_details.replace(/\n/g, '</p><p>').replace(/<br\/>/g, '').replace(/<p><\/p>/g, '');
                }
                $(page).find('.product_details').html(parameters.shop.details.list.product_details);
                $(page).find('.footer_p_name').html(template('footer_p_name_temp', parameters.shop.details.list));
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
                loadTopProductReview({
                    product_id: getUrlParam('id')
                }, function (obj) {
                    if (obj.length > 0) {
                        $(page).find('.product_review_list').html(template("product_review_temp", {
                            list: obj[0].review
                        }));
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
                    if (obj.length > 0 && obj[0].menu_count > 0) {
                        $(page).find('.product_brand_list').html(template('product_brand_temp', obj[0])).show();
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
            titnum2 = $(page).find(".evaluate").offset().top + $(page).find(".content").scrollTop() - (txapp.isApp() ? 20 : 0);
            titnum3 = $(page).find(".details_index").offset().top + $(page).find(".content").scrollTop() - (txapp.isApp() ? 20 : 0);
            titHeight = $(page).find(".evaluate").outerHeight();
        }
    });

    //商品详情预览
    publicPageInit("#page-shop-preview", function (e, id, page) {
        $.closeModal();
        setScrollerNum();
        //主体信息加载
        (function () {
            var info = $.parseJSON($(page).find('.product_data').html());
            if (info.length == 0) {
                $.router.load('/shop/noPro.html?pid=' + location.search.substr(3)); return;
            }
            parameters.shop.details.list = info[0];
            parameters.shop.details.list.is_tx_app = txapp.isApp();
            parameters.shop.details.shareDesc = parameters.shop.details.list.product_name;
            //加载轮播图
            $(page).find('.details_lunbo  .swiper-wrapper').html(template('lunbo_temp', { list: parameters.shop.details.list.product_imgs.split(',') }));
            var swiperDetails = $(page).find('.details_lunbo');
            var swiperPagination = $(page).find('.details_lunbo .swiper-pagination');
            new Swiper(swiperDetails, {
                pagination: swiperPagination,
                paginationClickable: true,
                autoplay: 3000,
                loop: true
            });
            //加载主信息
            $(page).find('.shop_info').html(template('shop_info_temp', { info: parameters.shop.details.list }));
            $(page).find('.nav_collect').data('collect', parameters.shop.details.list.is_collect).addClass('is_collect_' + parameters.shop.details.list.is_collect);

            $(page).find(".content").scrollTop(0);
        })();
        //页面内容点击事件处理
        (function () {
            //规格属性数据
            var property_list;
            var map_id = 0;
            $('#product_property_list').html('');//该弹框是全局容器，所以要清空后使用
            //打开立即购买
            $(page).off('click', '.open_property_list').on('click', '.open_property_list', function () {
                $.popup('#product_property_list');
                if ($('#product_property_list').text().length == 0) {
                    property_list = $.parseJSON($(page).find('.product_property').html());
                    $('#product_property_list').html(template('product_property_temp', {
                        list: property_list
                    }));
                    var _defaultObj;
                    var _defaultData = $.grep(property_list, function (j, k) {
                        return j.is_default == true && j.remain_inventory > 0;
                    });
                    if (_defaultData.length == 0)
                        _defaultData = $.grep(property_list, function (j, k) {
                            return j.remain_inventory > 0;
                        });

                    if (_defaultData.length > 0) _defaultObj = _defaultData[0];
                    propertySelect(_defaultObj);
                }
            });
            //选择规格赋予样式变化
            function propertySelect(obj) {
                if (obj) {
                    map_id = obj.map_id;
                    $('#product_property_list .property_img').prop('src', obj.img + ',1,80,80,3');
                    $('#product_property_list .property_price').text(obj.price);
                    $('#product_property_list .remain_inventory').text(obj.remain_inventory);
                    //样式变化
                    $('#product_property_list .property_select').removeClass('current');
                    $('#product_property_list .map_id' + obj.map_id).addClass('current');
                }
            };
            //规格选择
            $('#product_property_list').off('click', '.property_select').on('click', '.property_select', function () {
                var _id = $(this).data('id');
                var _selectData = $.grep(property_list, function (j, k) {
                    return j.map_id == _id;
                });
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
        })();
        //滚动事件监听与处理
        (function () {
            //滚动事件监听
            $(page).find(".content").on('scroll', function () {
                var lunboHeight = $(page).find('.details_lunbo').height();
                var lunboTop = $(page).find('.details_lunbo').offset().top;
                var dNumber = lunboTop / lunboHeight;
                $(page).find('.d_header').css({ opacity: -dNumber });
                var scrollNum = $(page).find(".content").scrollTop();
                if (scrollNum > titnum2 - titHeight * 1.2 && scrollNum < titnum3 - titHeight * 1.2) {
                    $(page).find(".title2").addClass('current').siblings().removeClass('current');
                } else if (scrollNum > titnum3 - titHeight * 1.2) {
                    $(page).find(".title3").addClass('current').siblings().removeClass('current');
                } else {
                    $(page).find(".title1").addClass('current').siblings().removeClass('current');
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
                if (parameters.shop.details.list.product_details_type == 0) {
                    parameters.shop.details.list.product_details = parameters.shop.details.list.product_details.replace(/\n/g, '</p><p>').replace(/<br\/>/g, '').replace(/<p><\/p>/g, '');
                }
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
                loadTopProductReview({
                    product_id: getUrlParam('id')
                }, function (obj) {
                    if (obj.length > 0) {
                        $(page).find('.product_review_list').html(template("product_review_temp", {
                            list: obj[0].review
                        }));
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
    //商品详情售后说明
    publicPageInit("#page-shop-explain", function (e, id, page) {
        if (txapp.isApp()) {
            $(page).find('.bar').remove();
        }
        //返回
        $(page).off('click', '.click_back').on('click', '.click_back', function () {
            if (txapp.isApp()) {
                txapp.goBack();
                return false;
            }
            $.router.back();
            return false;
        });
    });

    //商家店铺
    publicPageInit("#page-shop-mchstore", function (e, id, page) {
        //返回顶部
        (function () {
            goTopEvent(page, '2.2rem', '2.2rem');
        })();

        //返回
        $(page).off('click', '.click_back').on('click', '.click_back', function () {
            if (firstLoad(window.location.href)) {
                $.router.back();
                return false;
            }
        });

        parameters.shop.mchstore.news.pageIndex = 0;
        parameters.shop.mchstore.product = {
        };
        parameters.shop.mchstore.product.p_0 = {
            page_index: 0, page_size: 10, mch_id: $(page).find('#mch_id').val(), class_id: 0
        };
        parameters.shop.mchstore.news.mch_id = $(page).find('#mch_id').val();

        //商品分类加载，异步读取Swiper有问题
        var class_data = eval($(page).find('#product_class_content_data').html());
        if (class_data) {
            $(page).find('.product_class_name_list .extra_class_name').remove();
            $(page).find('.product_class_name_list .swiper-wrapper').append(template('product_class_name_temp', { list: class_data }));
            $(page).find('.product_class_name_list .tab-link').eq(0).addClass('active');
            $(page).find('.product_class_content_list .extra_class_content').remove();
            $(page).find('.product_class_content_list').append(template('product_class_content_temp', { list: class_data }));
            $(page).find('.product_class_content_list .tab').eq(0).addClass('active');
            $.each(class_data, function (i, o) {
                parameters.shop.mchstore.product['p_' + o.class_id] = {
                    page_index: 0, page_size: 10, mch_id: $(page).find('#mch_id').val(), class_id: o.class_id
                }
            });
        } new Swiper('.product_class_name_list', { slidesPerView: 4 });

        var _productOnlyShow = 1;
        //获得店铺信息
        getMchInfoByUser($('#mch_id').val(), function (obj) {
            _productOnlyShow = obj.ProductOnlyShow;
            if (obj.MchClass == 3) $(page).find('.bar .title').text('品牌展示');
            else $(page).find('.liuyan_bar').remove();
            $(page).find('.content .store_content_info').html(template('content_temp', obj));
            if (obj.NewsCount > 0) {
                var _menu = {
                    MenuName: '资讯', MenuContent: '', MenuId: -100
                };
                obj.StoreMenuInfoList[obj.StoreMenuInfoList.length] = _menu;
            }
            $(page).find('.menu_name_list .extra_menu_name').remove();
            $(page).find('.content .menu_name_list .swiper-wrapper').append(template('menu_name_temp', {
                list: obj.StoreMenuInfoList
            }));
            $(page).find('.content .menu_content_list').append(template('menu_content_temp', {
                list: obj.StoreMenuInfoList
            }));
            var _show = getUrlParam('show');//分享过来需要定位展示板块
            if (_show != '' && parseInt(_show) <= obj.StoreMenuInfoList.length) {
                var _name = $(page).find('.content .menu_name_list .swiper-wrapper a').eq(_show);
                var _content = $(page).find('.content .menu_content_list').children().eq(_show);
                if (parseInt(_show) == obj.StoreMenuInfoList.length) {
                    _show = -100;
                    _content = $(page).find('#tab' + _show);
                    _name = $(page).find('#tab-link' + _show);
                }
                _name.addClass('active').siblings().removeClass('active');
                _content.addClass('active').siblings().removeClass('active');
            }
            if ($(page).find('#tab0.active').length > 0) loadProduct(0);
            if ($(page).find('#tab-100.active').length > 0) loadNews();
            if (obj.StoreAdImgInfoList) {
                $(page).find('.mchad_lunbo .swiper-wrapper').html(template('store_lunbo_temp', {
                    list: obj.StoreAdImgInfoList
                }));
                new Swiper('.mchad_lunbo', {
                    pagination: '.swiper-pagination',
                    paginationClickable: true,
                    autoplay: 3000,
                    loop: true
                });
            }
            new Swiper('.menu_name_list', { slidesPerView: 5 });
            if (obj.MchClass == 3) {
                $(page).find('.icon_zi').text('招商类');
            } else if (obj.MchClass == 4) {
                $(page).find('.icon_zi').text('门店类');
            } else if (obj.MchClass == 5) {
                $(page).find('.icon_zi').text('产品类');
            } else if (obj.MchClass == 7) {
                $(page).find('.icon_zi').text('微商类');
            }
            $(page).find('#tab0 .product_count').text(obj.ProductCount);
            $(page).find('#browse_count').text(obj.BrowseCount);

            if ($('#is_auth').val() == "false") {
                $('.is_collect_div').hide();
            }
            //关注店铺
            $(page).off('click', '.is_collect_div').on('click', '.is_collect_div', function () {
                var self = $(this);
                var collect = $(this).data('collect') == 0 ? 1 : 0;
                setMchBrowse($('#mch_id').val(), collect, function (obj) {
                    self.removeClass('is_collect_' + self.data('collect')).addClass('is_collect_' + collect).data('collect', collect);
                    $(page).find('.store_content_info .collect_count').text(parseInt($(page).find('.store_content_info .collect_count').text()) + (collect == 0 ? -1 : 1));
                });
            });
        });
        //商品菜单点击
        $(page).off("click", ".menu_name_list #tab-link0").on('click', '.menu_name_list #tab-link0', function () {
            if ($(page).find('#tab0-0 .infinite-scroll-preloader').css("display") == "block" && parameters.shop.mchstore.product['p_0'].pageIndex == 0) {
                loading = true;
                loadProduct(0);
            }
        });
        //商品分类点击加载数据
        $(page).off("click", ".product_class_name_list .tab-link").on('click', '.product_class_name_list .tab-link', function () {
            var pclass = $(this).data('pclass');
            if (parameters.shop.mchstore.product['p_' + pclass].page_index == 0) loadProduct(pclass);
        });
        //多个标签页下的无限滚动
        var loading = false;
        //标签滑动加载
        $(page).on('infinite', function () {
            if (loading) return;
            if ($('#tab0.active').length > 0) {
                var pclass = $(page).find('.product_class_name_list .active').data('pclass');
                if ($(page).find('#tab0-' + pclass + ' .infinite-scroll-preloader').css("display") == "block" && parameters.shop.mchstore.product['p_' + pclass].page_index > 0) {
                    loading = true;
                    loadProduct(pclass);
                }
            }
            if ($(page).find('#tab-100.active').length > 0) {
                if (parameters.shop.mchstore.news.pageIndex > 0 && $(page).find('#tab-100 .infinite-scroll-preloader').css("display") == "block") {
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
            getMchStoreProductList3(parameters.shop.mchstore.product['p_' + pclass], function (params, obj) {
                params.page_index++;
                if (obj.length > 0) {
                    if (params.page_index == 1) $(page).find('#tab0-' + params.class_id + ' .list').html('');
                    $(page).find('#tab0-' + params.class_id + ' .list').append(template('t-productShow2', {
                        Products: obj
                    }));
                    $(page).find('.lazy').lazyload({
                        threshold: 180, container: '#page-shop-mchstore .content'
                    });
                }
                if (obj.length < params.page_size) {
                    $(page).find('#tab0-' + params.class_id + ' .infinite-scroll-preloader').hide();
                    $(page).find('#tab0-' + params.class_id + ' .more_container').show();
                }
                loading = false;
            });
        };
        //加载资讯信息
        function loadNews() {
            getMchNewsList(parameters.shop.mchstore.news, function (params, obj) {
                if (obj.length > 0) {
                    params.pageIndex++;
                    if (params.pageIndex == 1) $(page).find('#tab-100 .list').html('');
                    $(page).find('#tab-100 .list').append(template('store_news_temp', { list: obj }));
                }
                if (obj.length < params.pageSize) {
                    $(page).find('#tab-100 .infinite-scroll-preloader').hide();
                    $(page).find('#tab-100 .more_container').show();
                }
                loading = false;
            });
        };
    });
    //商家资讯详情
    publicPageInit("#page-shop-mchnews", function (e, id, page) {
        //返回顶部
        (function () {
            goTopEvent(page, '2.5rem', '10px');
        })();
        //返回
        $(page).off('click', '.click_back').on('click', '.click_back', function () {
            if (firstLoad(window.location.href)) {
                $.router.back();
                return false;
            }
        });

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
                $('#user_id').val(obj[0].UserId);
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
                    setStoreNewsLike({
                        news_id: obj[0].NewsId
                    }, function (obj) {
                        _self.data('like', 1);
                        _self.removeClass('is_like_0').addClass('is_like_1');
                        _self.find('span').text(parseInt(_self.find('span').text()) + 1);
                    });
                    return false;
                });
                if (txapp.isApp()) {
                    getUserInfo({
                        userId: $('#user_id').val()
                    }, function (obj) {
                        if (obj.length > 0) {
                            parameters.shop.mchnews.author = obj[0];
                        }
                    });
                }
                if (obj[0].OpenReward == true) {
                    //获得打赏头像列表
                    getRewardInfo({
                        pageIndex: 0, pageSize: 16, deal_type_name: 'mchnews', onlyId: $('#news_id').val()
                    }, function (params, obj) {
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
            subUserBBSLike();
            function subUserBBSLike() {
                setUserBBSLike({
                    bbs_id: _self.data('bbsid')
                }, function (obj) {
                    if (obj.errcode == -99) {
                        if (txapp.isApp()) {
                            txapp.openBindMobileModal({
                                fun: 'subUserBBSLike', success: function (obj) {
                                    if (obj.success && obj.fun == 'subUserBBSLike') {
                                        subUserBBSLike();
                                    }
                                }
                            });
                            txapp.send(function (data) {
                                $.toast(data);
                                var obj = txapp.parseJSON(data);
                                if (obj.success && obj.fun == 'subUserBBSLike') {
                                    subUserBBSLike();
                                }
                            });
                        } else {
                            openBindByThridShareCode(subUserBBSLike);
                        }
                    } else if (obj.success == "true") {
                        var _span = _self.removeClass('is_like_0').addClass('is_like_1').data('like', 1).find('span');
                        _span.text(parseInt(_span.text()) + 1);
                    }
                    $.toast(obj.msg);
                });
            };
            return false;
        });
        //资讯评论点击
        $(page).off('click', '.news_bbs .app').on('click', '.news_bbs .app', function () {
            if (txapp.isApp()) {
                txapp.mchNewsBBS({
                    news_id: parameters.shop.mchnews.info.news_id
                });
                return false;
            }
        });
        //查看发表的他人
        $(page).off('click', '.news_bbs .other_user').on('click', '.news_bbs .other_user', function () {
            if (txapp.isApp()) {
                txapp.otherUser({
                    userid: $(this).data('uid')
                });
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
                txapp.bbsDetails({
                    bbs_id: $(this).data('bbsid'),
                    news_id: parameters.shop.mchnews.info.news_id,
                    bbs_type: 'mchnews'
                });
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
                txapp.bbsDetailsAdd({
                    bbs_id: $(this).data('bbsid'),
                    nick_name: $(this).data('nickname'),
                    review_bbs_id: $(this).data('reviewid'),
                    news_id: parameters.shop.mchnews.info.news_id,
                    bbs_type: 'mchnews'
                });
                return false;
            }
        });
        //资讯沙发
        $(page).off('click', '.news_bbs .nonews_bbs').on('click', '.news_bbs .nonews_bbs', function () {
            txapp.addBBS({
                bbs_type: 'mchnews', only_id: $('#news_id').val()
            });
        });
        //资讯商品跳转
        $(page).off('click', '.product_img_click').on('click', '.product_img_click', function () {
            var _url = '/shop.html?id=' + $(this).data('pid');
            if (txapp.isApp()) {
                txapp.shopDetails({
                    url: '' + _url
                });
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
                    $(page).find('.lazy').lazyload({
                        threshold: 180, container: '#page-shop-mchnews .content'
                    });
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
                txapp.rewardList({
                    newsId: $('#news_id').val(),
                    deal_type_name: 'mchnews'
                });
            }
            else {
                $.router.load("#page-shop-reward");
            }
            return false;
        });

    }, function () {
        txapp.onNews({
            news_id: $('#news_id').val()
        });
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
        $(page).off('click', '.submit_liuyan').on('click', '.submit_liuyan', submitLiuyan);
        function submitLiuyan() {
            if (requiredForm('#liuyan_form')) {
                setStoreLiuyan($('#liuyan_form').serialize(), function (obj) {
                    if (obj.success == "true") {
                        $.router.load("#page-shop-liuyansuccess");
                    } else if (obj.errcode == -99) {
                        openBindByThridShareCode(submitLiuyan);
                    } else {
                        $.toast(obj.msg);
                    }
                });
            }
        };
    });
    //商家留言成功
    publicPageInit("#page-shop-liuyansuccess", function (e, id, page) {
        $(page).off('click', '.gobackstore').on('click', '.gobackstore', function () {
            routerLoad('/shop_2/mchstore.html?mchid=' + $('.mch_id').val());
        });
    });

    //资讯全部评价
    publicPageInit("#page-bbs-index", function (e, id, page) {

        parameters.bbs.index.all.pageIndex = 0;
        parameters.bbs.index.user.only_id = $(page).find('.news_only_id').val();
        parameters.bbs.index.user.bbs_type = $(page).find('.news_type_name').val();
        parameters.bbs.index.all.only_id = $(page).find('.news_only_id').val();
        parameters.bbs.index.all.bbs_type = $(page).find('.news_type_name').val();

        //获得我参与的评论
        if (userIsAuth()) {
            getUserBBSList(parameters.bbs.index.user, function (obj) {
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
            getBBSList(parameters.bbs.index.all, function (params, obj) {
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
            setUserBBSLike({
                bbs_id: _self.data('bbsid')
            }, function (obj) {
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
    publicPageInit("#page-bbs-details", function (e, id, page) {

        parameters.bbs.details.all.pageIndex = 0;
        parameters.bbs.details.all.bbs_id = $('#bbs_id').val();
        //加载评论楼主信息
        getBBSDetails({
            bbs_id: $('#bbs_id').val()
        }, function (obj) {
            if (obj.length) {
                $(page).find('.user_bbs .list').html(template('bbs_details_temp', { list: obj }));
                parameters.bbs.details.like_count = obj[0].LikeCount;
            }
        });
        loadBBSDetailsList();
        //加载评论回复列表
        function loadBBSDetailsList() {
            getBBSDetailsList(parameters.bbs.details.all, function (params, obj) {
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
            setUserBBSLike({
                bbs_id: _self.data('bbsid')
            }, function (obj) {
                if (obj.success == "true") {
                    var _span = _self.removeClass('is_like_0').addClass('is_like_1').data('like', 1).find('span');
                    _span.text(parseInt(_span.text()) + 1);
                }
                $.toast(obj.msg);
            });
            return false;
        });
        //赞的列表
        getBBSLikeList({
            pageIndex: 0, pageSize: 6, bbs_id: $('#bbs_id').val()
        }, function (params, obj) {
            if (obj.length > 0) {
                $(page).find('.like_img .list').html(template('like_img_temp', {
                    list: obj[0].List
                }));
            }
        })
    });
    //资讯评价点赞列表
    publicPageInit("#page-bbs-details-like", function (e, id, page) {
        parameters.bbs.details.like.pageIndex = 0;
        parameters.bbs.details.like.bbs_id = $('#bbs_id').val();
        loadBBSLikeList();
        //加载点赞列表
        function loadBBSLikeList() {
            getBBSLikeList(parameters.bbs.details.like, function (params, obj) {
                params.pageIndex++;
                if (params.pageIndex == 1) $(page).find('.like_list .list').html('');
                if (obj.length > 0) {
                    $(page).find('.like_list .list').append(template('like_list_temp', {
                        list: obj[0].List
                    }));
                    loading = false;
                }
                if (obj.length < params.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(page).find('.infinite-scroll'));
                    // 删除加载提示符
                    $(page).find('.infinite-scroll-preloader').remove();
                    if (obj.length > 0) {
                        var _visitorCount = parameters.bbs.details.like_count - obj[0].TotalCount;
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
                    $(page).find('.reward_list .list').append(template('rewardInfo_list_temp', {
                        list: obj[0].List
                    }));
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

    //商品评价
    publicPageInit("#page-review-index", function (e, id, page) {
        var _this = page
            , productId = getUrlParam('id')  //商品id
            , reviewListParams = {
                'productId': productId, 'reviewVal': -1, 'pageIndex': 1, 'pageSize': 5
            }
            , loading = false
            , loadFinished = false
            , rateData = {
            };


        bingPageEvent();

        if (productId > 0 && parameters.review.isFirstLoad) {
            parameters.review.isFirstLoad = false;
            init();
        }
        //if (productId > 0) {
        //    init();
        //}

        function init() {
            loadData();
            document.title = parameters.review.title[0];
        }
        function loadData() {
            loadRateDataAndBindPage(true);
        }
        //计算评论的好评率并返回商品基本信息
        function loadRateDataAndBindPage(first) {
            var params = {
                'productId': productId
            };
            $.showIndicator();
            //拉取数据填充页面
            getProdectReviewRate(params, function (data) {
                //填充完数据显示页面
                $.hideIndicator();
                if (data.success == true) {
                    parameters.review.product = data.product;
                    var html = template('review-index-product', {
                        list: [{
                            'product': data.product, 'rate': data.rate
                        }]
                    });
                    $(_this).find('.j-product-container').empty().append(html).show();
                    reviewListParams.reviewVal = -1;
                    loadReviewDataAndBindPage(true);
                }
            });
        }
        //获取评价列表
        function loadReviewDataAndBindPage(first) {
            if (first) {
                reviewListParams.pageIndex = 1;
                loadFinished = false;
            }
            loadReviewData(reviewListParams, function (data) {
                reviewListParams.pageIndex++;
                var container = $(_this).find('.j-review-container');
                if (first) {
                    container.removeClass('bg-white');
                    container.find('.rate-all').empty();
                    container.find('.review-no').remove();
                    container.find('.j-tagLoadToast').remove();
                }

                if (data.data.length < reviewListParams.pageSize) {
                    // 加载完毕，当前无限加载标识赋值
                    loadFinished = true;
                    container.append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
                }
                loading = false;
                //容器发生改变,如果是js滚动，需要刷新滚动
                $.refreshScroller();
                $(_this).find('.infinite-scroll-preloader').hide();

                if (data.success != 'false') {
                    if (data.totalCount == 0) {
                        container.find('.j-tagLoadToast').remove();
                        $(_this).find('.j-review-container').addClass('bg-white');
                        $(_this).find('.j-review-container .rate-all')
                            .before('<div class="review-no" > <i></i>此商品没有该类型评价！</div>');
                    }
                    else {
                        var html = template('review-index-item', data);
                        $(_this).find('.j-review-container .rate-all').append(html);
                        $(_this).find('.lazy').lazyload({
                            threshold: 180, container: '#page-review-index .content'
                        });
                        bindPhotoBrowserEvent(_this);
                    }
                    //绑定点赞
                    bindLinkEvent(_this);
                } else {
                    $.toast('服务器错误，请重试！');
                }
            });
        }

        function bingPageEvent() {
            bindRateMenuEvent();
            bindLinkAndReplyEvent();
            //无限滚动
            $(_this).unbind('infinite').on('infinite', function () {
                if (loading) return;
                if (loadFinished) return;
                loading = true;
                $(_this).find('.infinite-scroll-preloader').show();
                loadReviewDataAndBindPage(false);
            });
            //返回
            $(page).off('click', '.click_back').on('click', '.click_back', function () {
                if (txapp.isApp()) {
                    txapp.goBack();
                    return false;
                }
                $.router.back();
                return false;
            });
        }
        //绑定好评中评差评标签
        function bindRateMenuEvent() {
            $(_this).find('.j-product-container').unbind('click').on('click', '.order-rate', function () {
                if ($(this).parent().hasClass('current')) {  //当前显示的页面  不执行
                    return;
                }
                $(_this).find(".order-rate").parent().removeClass('current');
                $(this).parent().addClass('current');
                var reviewVal = parseInt($(this).attr('data-id'));
                reviewListParams.reviewVal = reviewVal;
                loadReviewDataAndBindPage(true);
            });
        }
        //绑定点赞和评价
        function bindLinkAndReplyEvent() {
            //绑定评价
            $(_this).unbind('click').on('click', '.reply', function () {
                //先判断登录，未登录跳转登录页
                if (userIsAuth() == false) {
                    return publicConfigLogin();
                }

                //预处理详情页面数据
                parameters.review.isFirstLoadDetails = true;
                parameters.review.productId = productId;
                parameters.review.details = $(this).parents('.item').html();
                parameters.review.product.PropertyName = $(parameters.review.details).find('.info-spec').text();
                parameters.review.parentId = $(this).parent().attr('data-id');
                parameters.review.isUse = $(this).parent().attr('data-use');
                $('#page-review-details .j-review-item').empty().hide();
                $('#page-review-details .product-container').empty().hide();
                $('#page-review-details .replay-detials-container').hide();
                $.router.load("#page-review-details");  //加载内联页面
            });
        }
    });

    //商品评价详情
    publicPageInit("#page-review-details", function (e, id, page) {
        var _this = page
            , reviewListParams = {
                'parentId': parameters.review.parentId, 'reviewVal': -1, 'pageIndex': 1, 'pageSize': 5
            };
        var loading = false;
        if (parameters.review.product) {
            //if (parameters.review.isFirstLoadDetails) {
            //    parameters.review.isFirstLoadDetails = false;

            //}
            init();
        }
        else {
            $.router.back()
        }

        function init() {
            loadData();
            bingPageEvent();
            document.title = parameters.review.title[1];
        }

        function loadData() {
            //评价信息
            $(_this).find('.j-review-item').empty().show().append($(parameters.review.details)).find('.info-spec,.action-wrap').remove();
            //商品信息
            var html = template('review-details-product', { list: [parameters.review.product] });
            $(_this).find('.product-container').empty().show().append(html);
            //首次加载评价列表
            loadReplyDataAndBindPage(true);
        }

        //获取评价回复列表
        function loadReplyDataAndBindPage(first) {
            loadReviewData(reviewListParams, function (data) {
                reviewListParams.pageIndex++;
                var container = $(_this).find('.j-detials-content');
                if (first) {
                    //$(_this).find('.content .j-tagLoadToast').remove();
                    container.empty().parent().show();
                }

                if (data.data.length < reviewListParams.pageSize) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(_this).find('.infinite-scroll'));
                    // $(_this).find('.content')
                    //     .append('<div class="j-tagLoadToast" style="text-align:center;margin:0.5rem 0;color:#797979;">亲，没有更多了.</div>');
                }
                loading = false;
                //容器发生改变,如果是js滚动，需要刷新滚动
                $.refreshScroller();
                $(_this).find('.infinite-scroll-preloader').hide();

                if (data.totalCount == 0) {
                    container.append('<span class="no-replay">无评论,快来抢沙发吧~</span>');
                }
                else {
                    var html = template('review-details-replay', data);
                    container.append(html);
                    bindLinkEvent(_this);//绑定点赞事件
                }
            });
        }

        function bingPageEvent() {
            //绑定进入商品详情
            $(_this).find('.product-container').unbind('click').on('click', '.product-wrap', function () {
                //$.router.load('/shop.html?id=' + parameters.review.productId + '&source=shop');
                txapp.inProductDetail(parameters.review.productId);
            });
            //无限滚动
            $(_this).unbind('infinite').on('infinite', function () {
                if (loading) return;
                loading = true;
                $(_this).find('.infinite-scroll-preloader').show();
                loadReplyDataAndBindPage(false);
            });

            //判断该回复是否赞过，如果没赞绑定事件，赞过则修改页面
            if ($('#page-review-index')
                .find("[data-id='" + parameters.review.parentId + "']").find('.like-active').hasClass('nolike')) {
                //改变颜色
                $(_this).find('.j-detials-cative .tab-item').eq(0).removeClass('like');
                //点击下方点赞事件
                $(_this).find('.j-detials-cative .tab-item').eq(0).unbind('click').bind('click', function () {
                    var that = this;
                    if ($(that).hasClass('like')) {
                        $(that).unbind();
                        return false; //如果已经点过赞 不执行
                    }
                    var parentPageDom = $('#page-review-index')
                        .find("[data-id='" + parameters.review.parentId + "']").find('.like-active');
                    var params = {
                        'reviewId': parameters.review.parentId, 'isUse': parameters.review.isUse
                    };

                    addLikeCount(params, function (data) {
                        if (data.success == 'true') {
                            //点赞成功，删除当前按钮点赞事件,当前页面改变颜色,
                            //         删除父页面点赞事件，父页面数量 + 1，父页面改变颜色
                            $(that).unbind();
                            $(that).addClass('like');
                            //下面逻辑顺序不可变，先取消事件，获取数量，改变颜色，赋值新数量
                            //取消事件
                            $(parentPageDom).unbind();
                            //点赞数量+1
                            var likeCount = 0;
                            if ($(parentPageDom).find('label').length > 0) {
                                likeCount = parseInt($(parentPageDom).find('label').text());
                            }
                            //改变颜色
                            $(parentPageDom).empty().append('<i>&#xe9d4;</i>');
                            $(parentPageDom).removeClass('nolike');

                            likeCount += 1;
                            $(parentPageDom).append('<label>' + likeCount + '</label>');
                            if ($(parentPageDom).hasClass('count') == false) {
                                $(parentPageDom).addClass('count');
                            }
                        } else {
                            $.toast(data.msg);
                        }
                    });
                });
            }
            else {
                $(_this).find('.j-detials-cative .tab-item').eq(0).addClass('like');
            }
            //点击评论事件
            $(_this).find('.j-detials-cative .tab-item').eq(1).unbind('click').on('click', function () {
                if (txapp.isApp()) {
                    txapp.showReplayInputBox();
                    txapp.send(function (data) {
                        //$.alert(data);
                        var obj = txapp.parseJSON(data);
                        if (obj.success && obj.fun == 'addreplay') {
                            postReplay(obj.data);
                        }
                    });
                }
                else {
                    $(_this).find('.j-detials-cative').hide();
                    $(_this).find('.j-detials-input').show();
                    //给输入框添加焦点
                    $(_this).find('#replayInput').focus();
                }
                return false;
            });
            if (txapp.isApp()) {
                //失去焦点时隐藏
                $(_this).find('#replayInput').unbind('blur').on('blur', function () {
                    $(_this).find('.j-detials-input').hide();
                    $(_this).find('.j-detials-cative').show();
                    return false;
                });
            } else {
                //点击其他区域 隐藏评论框
                $(_this).find('.content').on('click', function () {
                    $(_this).find('.j-detials-input').hide();
                    $(_this).find('.j-detials-cative').show();
                    return false;
                });
            }
            //提交评价按钮
            $(_this).find('.j-replaySend').unbind('click').on('click', function () {
                postReplay(replayInput.value);
            });

            //提交评价
            function postReplay(msg) {
                msg = $.trim(msg);
                if (msg == '') {
                    $.alert('内容不能为空!');
                    return;
                }
                if (msg.length > 200) {
                    $.alert('输入内容过长！');
                    return;
                }
                var params = {
                    'reviewContent': msg, 'reviewId': parameters.review.parentId, 'isUse': parameters.review.isUse
                };
                //$.showPreloader('正在提交中...');//sui弹框遮罩有问题
                subReplyReview();
                function subReplyReview() {
                    replyReview(params, function (data) {
                        //$.hidePreloader();
                        if (data.errcode == -99) {
                            openBindByThridShareCode(subReplyReview);
                        } else if (data.success == 'true') { //提交成功
                            $.toast('发布成功');
                            $(_this).find('.j-detials-input').hide();
                            $(_this).find('.j-detials-cative').show();
                            //发布成功清空值
                            replayInput.value = '';
                            //将评价数据插入页面 
                            var html = template('review-details-replay', { data: [data.data] });
                            var container = $(_this).find('.j-detials-content');
                            if (container.find('dl').length == 0) {
                                //沙发
                                container.empty().append(html);
                            }
                            else {
                                container.prepend(html);
                            }
                            //父页面评论数量+1
                            var $parentPageDom = $('#page-review-index')
                                .find("[data-id='" + parameters.review.parentId + "']").find('.reply');
                            //评论数量+1
                            var replyCount = 0;
                            if ($parentPageDom.find('label').length > 0) {
                                replyCount = parseInt($parentPageDom.find('label').text()) + 1;
                                $parentPageDom.find('label').text(replyCount);
                            }
                            else {
                                replyCount += 1;
                                $parentPageDom.append('<label>' + replyCount + '</label>');
                            }
                            if ($parentPageDom.hasClass('count') == false) {
                                $parentPageDom.addClass('count');
                            }

                            bindLinkEvent(_this);//绑定点赞事件
                        }
                        else {
                            $.toast('发布失败，请重试');
                        }
                    });
                };
            }

            bindPhotoBrowserEvent(_this);
        }
    });

    //market.index
    publicPageInit('#page-market-index', function (e, id, page) {
        var _this = page
            , loading = false
            , q = decodeURI(getUrlParam('q'))  //推广语，更改title和header用
            , cids = getUrlParam('cids')  //分类id,多个直接逗号分隔
            , t = getUrlParam('t');  //页面来源，2正常分类  3分类推荐标签

        //345-11   99-13  31-8   223-8   385-9

        if (q == '' || cids == '' || t == '') {
            $.router.back();
        } else {
            init();
            console.log(cids);
        }
        //初始化页面
        function init() {
            //固定为首页搜索  [1首页搜索 2分类商品页面  3分类里面的推荐标签]
            parameters.product.searchParams.type = t;
            parameters.product.searchParams.max = 0;
            parameters.product.searchParams.min = 0;
            parameters.product.searchParams.class_ids = cids;
            parameters.product.searchParams.order = 0;
            parameters.product.searchParams.is_desc = 1;
            parameters.product.searchParams.page_size = 22;

            $('.j-title').text(document.title = q);
            $('.buttons-tab .btn-active').eq(0).addClass('select').siblings().removeClass('select');
            $('.btn-select .btn-active').removeClass('select').eq(0).addClass('select');
            loadData();
            bindPageEvent();
        };
        //加载页面数据
        function loadData() {
            s(true);
        }
        function clearMark() {
            //移除遮罩
            $(_this).removeClass('mark-warp');
            $(_this).find('.panel-warp').removeClass('active');
        }
        //重置筛选数据
        function resetFilterData() {
            $(_this).find('.panel-warp input').val('');
            $(_this).find('.panel-warp .j-val-class').text('');
        }
        //去查找数据
        function goSearch() {
            clearMark();
            //执行搜索
            s(true);
        }
        //商品搜索入口
        function s(isFirst, callBack) {
            if (isFirst) {
                parameters.product.searchParams.page_index = 0;
                $(_this).find('.content').scrollTop(0);
            }
            $.showPreloader();
            searchProduct(parameters.product.searchParams, function (obj) {
                bingProductsToPage(obj, isFirst, callBack)
            });
        }
        //搜索结果展示
        function bingProductsToPage(data, isFirst, callBack) {
            if (isFirst) {
                $(_this).find('.infinite-scroll-preloader').show();
                var htmlCon = '<ul class="j-products" style="display: block;"><div class="j-clear" style="clear: both;"></ul>';
                $(_this).find('.tempalte-content').empty().append(htmlCon);
            }
            var html = template('t-productShow2', { Products: data });
            $(_this).find('.tempalte-content').find('.j-products .j-clear').before(html);
            //懒加载图片
            $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-market-index .content' });
            if (callBack) callBack();
            if (data.length < parameters.product.searchParams.page_size) {
                //加载完毕，则注销无限加载事件，以防不必要的加载
                $.detachInfiniteScroll($(_this).find('.infinite-scroll'));
                $(_this).find('.infinite-scroll-preloader').hide();
                $(_this).find('.content .tempalte-content').append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">亲，没有更多了.</div>');
            }
        }

        //绑定页面事件
        function bindPageEvent() {
            //更多
            $(_this).find('.more').off().on('click', function () {
                if ($('.more-warp').css('display') == 'none') {
                    $('.more-warp').show();
                } else {
                    $('.more-warp').hide();
                }
            });
            //隐藏更多操作
            $('.more-warp').off().on('click', function () {
                $('.more-warp').hide();
            });
            //筛选
            $(_this).find('.j-btn-filter').off().on('click', function () {
                $(_this).find('.panel-warp').addClass('active');
            });
            //重置筛选条件
            $(_this).find('.j-btn-reset').off().on('click', function () {
                resetFilterData();
            });
            //筛选条件确定
            $(_this).find('.j-btn-ok').off().on('click', function () {
                var _max = $(_this).find('.j-val-max').val() * 1;
                var _min = $(_this).find('.j-val-min').val() * 1;
                var _key = $(_this).find('.j-val-key').val();
                if (_max != 0 && _min != 0 && _max < _min) {
                    $.toast('最高价不能低于最低价');
                    return;
                }

                parameters.product.searchParams.max = _max;
                parameters.product.searchParams.min = _min;
                parameters.product.searchParams.key = _key;
                goSearch();
            });
            //绑定tab
            $(_this).find('.btn-active').off().on('click', function () {
                var _type = parseInt($(this).attr('data-type'));
                clearMark();
                //添加标识
                if (_type >= 0) {
                    $(_this).find('.buttons-tab .btn-active').removeClass('select');
                    $(this).addClass('select');
                }
                else {
                    $(_this).find('.btn-select .btn-active').removeClass('select');
                    $(this).addClass('select');
                    $('.btn-active[data-type="0"]').text($(this).text());
                }

                if (_type == 0) {  //综合
                    if ($('.mark-warp').length > 0) {
                        clearMark();
                    }
                    else {
                        $(_this).addClass('mark-warp');
                    }
                }
                else {
                    switch (_type) {
                        case 1:  //销量
                            parameters.product.searchParams.order = 2;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case 2:   //牛币
                            parameters.product.searchParams.order = 3;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case -1:   //综合
                            parameters.product.searchParams.order = 0;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case -2:   //上新
                            parameters.product.searchParams.order = 1;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                        case -3:    //价格升序
                            parameters.product.searchParams.order = 4;
                            parameters.product.searchParams.is_desc = 0;
                            break;
                        case -4:    //价格降序
                            parameters.product.searchParams.order = 4;
                            parameters.product.searchParams.is_desc = 1;
                            break;
                    }
                    goSearch();
                }
            });
            //绑定下拉刷新
            bindInfiniteEvent();
        }
        // 注册下拉加载数据事件
        function bindInfiniteEvent() {
            $(document).unbind('infinite').on('infinite', '.infinite-scroll-bottom', function () {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                parameters.product.searchParams.page_index++;  //页码+1
                //拉取数据填充页面
                s(false, function () {
                    // 重置加载flag
                    loading = false;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                });

            });
        }
    });

    //商品订单支付弹框
    publicPageInit('#page-order-pay_iframe', function (e, id, page) {
        $(page).find('.download').remove();
        var params = {
            order_id: getUrlParam("order_id")
        };
        var payhost = location.hostname.split('.')[1] == 't' ? 'https://paytest.93390.cn' : 'https://pay.93390.cn';

        getOrderPayMoney(params, function (data) {
            if (data.length == 0) {
                $.alert('您的订单不存在');
                return false;
            }
            $(page).find('.order_pay_content').html(template('order_pay_temp', data[0]));
        });
        //账户支付
        $(page).off('click', '.account_pay').on('click', '.account_pay', function () {
            accountPayPrompt();
        });
        $(page).off('click', '.wx_pay').on('click', '.wx_pay', function () {
            if (isWxUserAgent() == false) {
                $.toast('请在微信客户端打开');
                return false;
            }
            window.parent.location.href = payhost + "/wx/SalesOrderPay.html?order=" + params.order_id;
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 2000);
        });
        $(page).off('click', '.ali_pay').on('click', '.ali_pay', function () {
            if (isWxUserAgent()) {
                $.toast('微信客户端不能使用支付宝支付');
                return false;
            }
            window.parent.location.href = payhost + "/alipay/SalesOrderPay.html?order=" + params.order_id;
            $.showPreloader();
            //setTimeout(function () {
            //    $.hidePreloader();
            //}, 2000);
        });
        //关闭当前页面
        $(page).off('click', '.close_iframe').on('click', '.close_iframe', function () {
            window.parent.closeiframe();
        });
        function accountPayPrompt() {
            $.prompt('您的支付密码', function (value) {
                params.pay_password = value;
                accountPayOrderByWap(params, function (obj) {
                    if (obj.success == 'false') {
                        $.alert(obj.msg);
                        accountPayPrompt();
                    } else {
                        $.toast('支付成功');
                        window.parent.location.href = '/order_2/list.html';
                    }
                });
            }, function (value) {
                window.parent.location.href = 'http://passport.93390.cn/findpwd.html';
            });
            //改变按钮文本
            $('.modal-button').eq(0).text('忘记密码');
            $('.modal-button').eq(1).text('确认支付');
        };
    });

    //全部订单列表
    publicPageInit("#page-order-all", function (e, id, page) {
        var _orderType = [{
            name: '全部', id: '-1'
        }, {
            name: '待支付', id: '0'
        }, {
            name: '待收货', id: '3'
        }, {
            name: '已收货', id: '4'
        }, {
            name: '待评价', id: '40'
        }, {
            name: '退货', id: '61'
        }];
        $(page).find('.order_type_list').html(template('order_type_temp', { list: _orderType }));
        var _show = getUrlParam('show');
        if (_show == '') {
            _show = _orderType[0].id;
        }
        $(page).find('.order_type_list .tab-id-' + _show).addClass('active');
        $(page).find('.order_div_list').html(template('order_div_temp', { list: _orderType }));
        $(page).find('.order_div_list .tab-id-' + _show).addClass('active');
        for (var i = 0; i < _orderType.length; i++) {
            parameters.order['list_' + _orderType[i].id] = {
                pageIndex: 0, pageSize: 10
            };
            parameters.order['list_' + _orderType[i].id].order_state = _orderType[i].id;
        }
        //初始数据加载
        loadList(_show);
        $(page).find('.order_type_list').off('click', '.tab-link').on('click', '.tab-link', function () {
            var c = $(this).data('id');
            if (parameters.order['list_' + c].pageIndex == 0) {
                loadList(c);
            }
        });
        var loading = false;
        //页面滚动到底部
        $(page).on('infinite', function () {
            if (loading) return;
            loading = true;
            var c = $(page).find('.order_div_list .active').prop('id').replace('tab', '');
            if (parameters.order['list_' + c].pageIndex > 0 && $(page).find('.infinite-' + c).css("display") == "block") {
                loadList(c);
            }
        });
        //加载数据
        function loadList(c) {
            var _data = {
                list: [], id: c
            };
            if (c == '40') {
                getOrderUnReviewList(parameters.order['list_' + c], function (params, obj) {
                    _data.list = obj;
                    loadPageHtml(params, 'order_unreview_temp');
                });
            } else if (c == "61") {
                getOrderRefundList(parameters.order['list_' + c], function (params, obj) {
                    _data.list = obj;
                    loadPageHtml(params, 'order_content_temp');
                });
            } else {
                getOrderList(parameters.order['list_' + c], function (params, obj) {
                    _data.list = obj;
                    loadPageHtml(params, 'order_content_temp');
                });
            }
            //分组层级订单展示
            function loadPageHtml(params, temp) {
                params.pageIndex++;
                if (params.pageIndex == 1) {
                    $(page).find('.order_div_list .tab-id-' + c + ' .list').html('');
                }
                $(page).find('.order_div_list .tab-id-' + c + ' .list').append(template(temp, _data));
                if (_data.list.length < params.pageSize) {
                    $(page).find('.infinite-' + c).hide();
                    $(page).find('.order_div_list .tab-id-' + c + ' .more_container').show();
                    return;
                }
                loading = false;
            };
        };
    });

    //确认订单
    publicPageInit('#page-order-handorder', function (e, id, page) {
        var _address = {
            address_id: 0
        };
        if (Number(window.sessionStorage['order_address_id'])) {
            _address.address_id = window.sessionStorage['order_address_id'];
        }
        var _params = {
            shop_ids: getUrlParam('shop_ids')
        };
        if (_params.shop_ids == '') {
            _params = {
                shop_count: getUrlParam('shop_count'), map_id: getUrlParam('map_id')
            };
        }
        var _addressId = 0;
        getProductByHandOrder(_params, function (obj) {
            if (obj.length == 0) {
                $.alert('没有找到您购买的商品', function () {
                    //routerLoad('/index.html');
                });
                return;
            }
            $(page).find('.store_product_list').html(template('store_product_temp', { list: obj }));
            //虚拟商品不需要收货地址
            if (obj[0].Product[0].IsVirtual == 0) {
                getDefaultAddress(_address, function (addressObj) {
                    if (addressObj.length == 0) {
                        //收货地址空
                        return;
                    }
                    if ($.grep(obj, function (m) {
                        return m.MchClass == 4;
                    }).length) {
                        addressObj[0].MchClass = 4;
                    }
                    $(page).find('.address_info_list').html(template('address_info_temp', addressObj[0]));
                    _addressId = addressObj[0].AddressId;
                });
            }
            var _points = 0, _moneys = 0;
            obj.forEach(function (o, i) {
                _points += o.Point;
                _moneys += o.Money;
            });
            $(page).find('.total_point').text(_points);
            $(page).find('.total_money').text(_moneys);
        });
        //运费查询
        $(page).off('click', '.postage_click_0').on('click', '.postage_click_0', function () {
            $.popup($(page).find('.postage_explain_0'));
            var _obj = JSON.parse($(this).data('json'));
            _obj.BuyerPostage = $(this).data('buyerpostage');
            $(page).find('.postage_explain_0 .content').html(template('postage_temp', _obj));
        });
        //运费查询
        $(page).off('click', '.postage_click_1').on('click', '.postage_click_1', function () {
            $.popup($(page).find('.postage_explain_1'));
        });
        //移除商家
        $(page).off('click', '.remove-mch-click').on('click', '.remove-mch-click', function () {
            $(page).find('.card-mch-' + $(this).data('id')).remove();
        });
        //移除商品
        $(page).off('click', '.remove-map-click').on('click', '.remove-map-click', function () {
            $(page).find('.map_id_' + $(this).data('id')).remove();
        });
        //提交订单
        $(page).off('click', '.sub_hand_order').on('click', '.sub_hand_order', function () {
            var property_json = [];
            $(page).find('.store_product_list .card').each(function (i, o) {
                $(o).find('.card-content').each(function (j, k) {
                    property_json.push({
                        map_id: $(k).find('.map_id').data('id'),
                        shop_count: $(k).find('.shop_count').data('count'),
                        buyer_ly: $(o).find('.buyer_ly').val()
                    });
                });
            });
            $.post("/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/HandOrder", {
                address_id: _addressId, property_json: JSON.stringify(property_json)
            }, function (data) {
                var obj = eval('(' + data + ')');
                if (obj.success == false || obj.success == "false") {
                    var scrollNum = $(page).find(".content").scrollTop();
                    if (obj.errcode == -3 || obj.errcode == -4 || obj.errcode == -11) {
                        var _mapIds = obj.map_ids.split(',');
                        for (var i = 0; i < _mapIds.length; i++) {
                            $(page).find('.remove-map-' + _mapIds[i]).show().find('.remove-info').text(obj.msg);
                        }
                        $(page).find(".content").scrollTop($(page).find('.remove-map-' + _mapIds[0]).offset().top);
                    } else if (obj.errcode == -8 || obj.errcode == -6) {
                        var _mchIds;
                        if (obj.errcode == -6) {
                            var _json = JSON.parse(obj.map_ids);
                            _mchIds = $.grep(_json.codeList.split(',').concat(_json.areaList.split(',')), function (n) {
                                return $.trim(n).length > 0;
                            });
                            if (_json.codeList) {
                                obj.msg = '收货地址不完整';
                            } else {
                                obj.msg = '店铺不在经营范围';
                            }
                        } else {
                            _mchIds = obj.mch_id.split(',');
                        }
                        for (var i = 0; i < _mchIds.length; i++) {
                            $(page).find('.remove-mch-' + _mchIds[i]).show().find('.remove-info').text(obj.msg);
                        }
                        $(page).find(".content").scrollTop($(page).find('.remove-mch-' + _mchIds[0]).offset().top + scrollNum);
                    } else {
                        $.toast(obj.msg);
                    }
                } else {
                    $.popup($(page).find('.pay_iframe_list'));
                    setTimeout(function () {
                        $(page).find('.pay_iframe_list iframe').attr('src', '/order_2/pay_iframe.html?order_id=' + obj.order_id);
                    }, 200);
                }
            });
        });
    });

    //地址管理
    publicPageInit('#page-member-address', function (e, id, page) {
        loadList();
        function loadList() {
            getSalesAddress(function (obj) {
                $(page).find('.address_list').html(template('address_list_temp', { list: obj }));
                $(page).find('.is_main_true input').prop('checked', true);
            });
        };
        //设置默认地址
        $(page).off('click', '.set_main_address').on('click', '.set_main_address', function (a, b) {
            console.log(a, b);
            editAddressIsMain({
                address_id: $(this).data('id')
            }, function (obj) {
                if (obj.success == 'true') {
                    $.toast('设置成功');
                    //loadList();
                } else {
                    $.toast(obj.msg);
                }
            });
        });
        //删除地址
        $(page).off('click', '.del_address').on('click', '.del_address', function () {
            var _id = $(this).data('id');
            $.confirm('确认删除该地址么？', function () {
                deleteAddress({
                    address_id: _id
                }, function (obj) {
                    if (obj.success == 'true') {
                        $.toast('删除成功');
                        loadList();
                    } else {
                        $.toast(obj.msg);
                    }
                });
            });
        });
        //选择地址
        $(page).off('click', '.card-content').on('click', '.card-content', function () {
            if (getUrlParam('type') == 'select') {
                window.sessionStorage['order_address_id'] = $(this).data('id');
                $.router.back();
            }
        });
    });

    //添加收货地址
    publicPageInit('#page-member-address-add', function (e, id, page) {
        var _this = page;
        window.sessionStorage['member_address_add_url'] = window.location.pathname + window.location.search;
        parameters.member.selectArea.back = function () {
            $(_this).find('.close-popup').click();
        };
        $(page).find('.add_address_ul').html(template('add_address_temp', { list: parameters.member.addressAdd.form }));
        //获取编辑的地址信息
        if (getUrlParam('id') != '') {
            getAddress({
                addressId: getUrlParam('id')
            }, function (obj) {
                if (obj.length > 0) {
                    for (var p in obj[0]) {
                        $(page).find('.add_address_form .' + p).val(obj[0][p]);
                    }
                    $(page).find('.add_address_form .Lng-Lat').val(obj[0].Lng + ',' + obj[0].Lat);
                }
            });
        }
        //保存地址
        $(page).off('click', '.sub_form').on('click', '.sub_form', function () {
            $.showPreloader();
            for (var i = 0; i < parameters.member.addressAdd.form.length; i++) {
                var _a = parameters.member.addressAdd.form[i];
                if (_a['token'] == 'Lng-Lat' && getUrlParam('mchclass') != '4') {
                    break;
                }
                var _val = $(page).find('.add_address_form .' + _a['token']).val();
                if (!_val) {
                    $.hidePreloader();
                    $.toast(_a['text'] + '不能为空');
                    return false;
                }
            }
            subAddressForm($(page).find('.add_address_form').serialize(), function (obj) {
                $.hidePreloader();
                if (obj.success == 'true') {
                    $.toast('保存成功');
                    $.router.back();
                } else {
                    $.toast(obj.msg);
                }
            });
        });
        //点击选择地区
        $(page).off('click', '.add_address_form .Area').on('click', '.add_address_form .Area', function () {
            $.popup($(page).find('.pay_iframe_area'));
            //if ($(page).find('.pay_iframe_area iframe').prop('src') == window.location.href)
            setTimeout(function () {
                $(page).find('.pay_iframe_area iframe').prop('src', '/member_2/public_select_area.html');
            }, 300);
            parameters.member.selectArea.callback = function (code, name) {
                $(_this).find('.close-popup').click();
                $(_this).find('.add_address_form .RegionCode').val(code);
                $(_this).find('.add_address_form .Area').val(name);
            };
        });
        //地理位置
        $(page).off('click', '.add_address_form .Lng-Lat').on('click', '.add_address_form .Lng-Lat', function () {
            $.popup($(page).find('.pay_iframe_map'));
            setTimeout(function () {
                $(page).find('.pay_iframe_map iframe').prop('src', '/member_2/public_select_map.html');
            }, 300);
            parameters.member.selectMap.getLngLat = function (p) {
                $(_this).find('.close-popup').click();
                $(_this).find('.add_address_form .Lng-Lat').val(p.lng + "," + p.lat);
                $(_this).find('.add_address_form .Lng').val(p.lng);
                $(_this).find('.add_address_form .Lat').val(p.lat);
            };
        });
    });

    //绑定手机输入邀请人
    publicPageInit('#page-member-bind-iframe-code', function (e, id, page) {
        memberBindCode(page, function () {
            $.router.load("#page-member-bind-iframe-mobile");
        });
    });

    //绑定手机输入手机
    publicPageInit('#page-member-bind-iframe-mobile', function (e, id, page) {
        memberBindMobile(page, function (obj) {
            window.parent.bindMobildIframe(obj);
        });
    });

    //个人资讯详情
    publicPageInit("#page-quan-usernews", function (e, id, page) {
        //返回顶部
        (function () {
            goTopEvent(page, '2.5rem', '10px');

            if (getUrlParam('showheader') == 'false') {
                $('.bar').remove();
            } else {
                $('.bar').show();
            }
        })();
        parameters.quan.userNews.posts_id = $(page).find('.posts_id').val();
        parameters.quan.userNews.bbslist.only_id = $(page).find('.posts_id').val();
        var newsInfo;
        //获得资讯详情
        getUserNewsInfo({
            posts_id: parameters.quan.userNews.posts_id
        }, function (obj) {
            if (obj) {
                newsInfo = obj;
                obj.IsApp = txapp.isApp();
                $(page).find('.user_news_detail').html(template('user_news_data_temp', obj));
                if (obj.OpenReward == true) {
                    //获得打赏头像列表
                    getRewardInfo({
                        pageIndex: 0, pageSize: 16, deal_type_name: 'usernews', onlyId: parameters.quan.userNews.posts_id
                    }, function (params, obj) {
                        if (obj.length > 0) {
                            $(page).find('.reward_list').html(template('reward_list_temp', {
                                count: obj[0].Count, list: obj[0].List
                            }));
                        }
                    });
                }
            } else {
                $.toast('资讯不存在');
                //$.router.back();
                return;
            }
        });
        //获得热门评论
        getBBSList(parameters.quan.userNews.bbslist, function (params, obj) {
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
        //资讯本身相关点击
        (function () {
            //资讯举报
            $(page).off('click', '.news_detail .news_report').on('click', '.news_detail .news_report', function () {
                txapp.userNewsReport(newsInfo);
            });
            //资讯点赞
            $(page).off('click', '.user_news_detail .is_like_click').on('click', '.user_news_detail .is_like_click', function () {
                var _self = $(this);
                if (_self.data('like') == "1") {
                    $.toast("你已点过赞");
                    return false;
                }
                setUserNewsLike({
                    posts_id: parameters.quan.userNews.posts_id
                }, function (obj) {
                    if (obj.successs == 'false') {
                        $.toast(obj.msg);
                        return false;
                    }
                    _self.data('like', 1);
                    _self.removeClass('is_like_0').addClass('is_like_1');
                    _self.find('span').text(parseInt(_self.find('span').text()) + 1);
                });
                return false;
            });
            //打赏点击
            $(page).off('click', '.news_likes .reward_btn').on('click', '.news_likes .reward_btn', function () {
                if (publicDownLoadApp()) {
                    txapp.rewardUserNews(newsInfo);
                }
                return false;
            });
            //打赏列表点击
            $(page).off('click', '#btn_reward_list').on('click', '#btn_reward_list', function () {
                if (txapp.isApp()) {
                    txapp.rewardList({
                        newsId: parameters.quan.userNews.posts_id,
                        deal_type_name: 'usernews'
                    });
                    return false;
                }
            });
        })();
        //资讯评论相关点击
        (function () {
            //点赞
            $(page).find('.news_bbs').off('click', '.is_like_click').on('click', '.is_like_click', function () {
                var _self = $(this);
                if (_self.data('like') == "1") {
                    $.toast("你已点过赞");
                    return false;
                }
                setUserBBSLike({
                    bbs_id: _self.data('bbsid')
                }, function (obj) {
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
                    txapp.bbsIndexList({
                        only_id: parameters.quan.userNews.posts_id, bbs_type: 'usernews'
                    });
                    return false;
                }
            });
            //查看发表的他人
            $(page).off('click', '.news_bbs .other_user').on('click', '.news_bbs .other_user', function () {
                if (txapp.isApp()) {
                    txapp.otherUser({
                        userid: $(this).data('uid')
                    });
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
                    txapp.bbsDetails({
                        bbs_id: $(this).data('bbsid'),
                        news_id: parameters.quan.userNews.posts_id,
                        bbs_type: 'usernews'
                    });
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
                    txapp.bbsDetailsAdd({
                        bbs_id: $(this).data('bbsid'),
                        nick_name: $(this).data('nickname'),
                        review_bbs_id: $(this).data('reviewid'),
                        news_id: parameters.quan.userNews.posts_id,
                        bbs_type: 'usernews'
                    });
                    return false;
                }
            });
            //资讯沙发
            $(page).off('click', '.news_bbs .nonews_bbs').on('click', '.news_bbs .nonews_bbs', function () {
                txapp.addBBS({
                    bbs_type: 'usernews', only_id: parameters.quan.userNews.posts_id
                });
            });
            //资讯商品跳转
            $(page).off('click', '.product_img_click').on('click', '.product_img_click', function () {
                var _url = '/shop.html?id=' + $(this).data('pid');
                if (txapp.isApp()) {
                    txapp.shopDetails({
                        url: '' + _url
                    });
                    return false;
                }
                $.router.load(_url);
            });
        })();
    }, function () {
        txapp.onUserNews({
            posts_id: getUrlParam('postsid')
        });
    });
    //个人资讯详情预览
    publicPageInit("#page-quan-usernews-preview", function (e, id, page) {
        //返回顶部
        (function () {
            goTopEvent(page, '2.5rem', '10px');

            if (getUrlParam('showheader') == 'false') {
                $('.bar').remove();
            } else {
                $('.bar').show();
            }
        })();
        parameters.quan.userNews.posts_id = $(page).find('.posts_id').val();
        //获得资讯详情
        getUserNewsInfoPreview({
            posts_id: parameters.quan.userNews.posts_id
        }, function (obj) {
            if (obj) {
                $(page).find('.user_news_detail').html(template('user_news_data_temp', obj));
            } else {
                $.toast('资讯不存在');
                //$.router.back();
                return;
            }
        });
    });
    //选择省份
    publicPageInit("#page-member-select-area-1", function (e, id, page) {
        parameters.member.selectArea.state = 1;
        parameters.member.selectArea.level = 1;
        parameters.member.selectArea.name1 = '';
        londAreaInfo(1, 1)
        function londAreaInfo(regionId) {
            getArea({
                'regionId': regionId
            }, function (obj) {
                if (obj.length > 0) {
                    $('#page-member-select-area-' + parameters.member.selectArea.level).find('.content').html(template('public_area_temp', { list: obj }));
                }
            });
        };
        $(document).off('click', '.public_area_click').on('click', '.public_area_click', function () {
            parameters.member.selectArea['name' + parameters.member.selectArea.level] = $(this).find('.item-inner').text();
            if (parameters.member.selectArea.level == 3 && window.parent.parameters.member.selectArea.callback != null) {
                parameters.member.selectArea.state = 0;
                window.parent.parameters.member.selectArea.callback($(this).data('code'), parameters.member.selectArea.name1 + parameters.member.selectArea.name2 + parameters.member.selectArea.name3);
            } else {
                parameters.member.selectArea.level = parameters.member.selectArea.level + 1;
                $.router.load("#page-member-select-area-" + parameters.member.selectArea.level);//加载页面
                londAreaInfo($(this).data('id'));//加载数据
            }
        });
        //返回点击
        $(page).off('click', '.back_click').on('click', '.back_click', function () {
            window.parent.parameters.member.selectArea.back();
        });
    });
    publicPageInit("#page-member-select-area-2", function (e, id, page) {
        if (parameters.member.selectArea.state == 0) {
            $.router.load("#page-member-select-area-1");
        }
        parameters.member.selectArea.level = 2;
        parameters.member.selectArea.name2 = '';
        $(page).find('.title').text(parameters.member.selectArea.name1);
    });
    publicPageInit("#page-member-select-area-3", function (e, id, page) {
        if (parameters.member.selectArea.state == 0) {
            $.router.load("#page-member-select-area-1");
        }
        parameters.member.selectArea.level = 3;
        parameters.member.selectArea.name3 = '';
        $(page).find('.title').text(parameters.member.selectArea.name2);
    });

    //购物车
    publicPageInit("#page-index-cart", function (e, id, page) {
        var _this = page
            , property_list = {}
            , _pageData = []
            , _current_shop_id = 0
            , _current_sku_id = 0
            , _selectShopIdArr = [];

        //记录本地和服务器时间差
        if (localStorage['server-date-diff'] == undefined) {
            localStorage['server-date-diff'] = new Date() - new Date($('#server_time').val());
        }
        //超过2分钟刷新数据
        var _timeDiff = new Date() - new Date($('#server_time').val()) - localStorage['server-date-diff'] * 1;
        //console.log('距离上次更新数据:（' + _timeDiff + '）毫秒');
        if (_timeDiff > 1000 * 10) {
            //页面超时需要刷新
            getShopCartList({ _t: new Date() * 1 }, function (obj) {
                init(obj);
            });
        } else {
            init();
        }


        function init(cartList) {
            loadData(cartList);
            bindPageEvent();
        };

        //加载页面数据
        function loadData(cartList) {
            _pageData = JSON.parse($('#d-pageData').text().replace(/\\r\\n/ig, ''));
            _pageData.RecommendList = JSON.parse(_pageData.RecommendList);
            if (cartList != null) {
                _pageData.CartList = cartList;
            }
            if (_pageData.CartList.length == 1 && (_pageData.CartList[0].ShopList.length > 0 || _pageData.CartList[0].Discard.length > 0)) {
                //有数据
                $(_this).find('.no-cart').remove();
                var _html = template('t-cart-shop', { 'list': _pageData.CartList[0].ShopList });
                $(_this).find('.all-item').empty().append(_html);
                $(_this).find('.j-allCount').text($('.item').length);
                console.log(_pageData.CartList[0].ShopList);
            }
            else {
                //购物车无数据
                $(_this).find('.no-cart').show();
            }
            //不管有没有 都绑定推荐数据
            $('.recommend-warp').remove();
            $(_this).find('.cart-warp').after(template('t-recommendClassProduct', { 'list': _pageData.RecommendList.ClassProductData }));
            var _likeHtml = template('t-productShow1', { 'Products': _pageData.RecommendList.RecommendProductData });
            var _conHtml = ['<div class="products-con"><div class="like"><i>&#xe65c;</i>猜你喜欢</div><ul>'
                , _likeHtml
                , '<div class="j-clear" style="clear: both;"></div></ul></div>'];
            $(_this).find('.products-con').last().after(_conHtml.join(''));
            //懒加载图片
            $(_this).find('.lazy').lazyload({ threshold: 180, container: '#page-index-cart .content' });
        }

        //每次选中状态变化都执行
        function changeSelectedEvent() {
            //如果都没选中  取消全选按钮
            $('[name="all"]').prop('checked', isAllSelect($('[name="shop"]')) && isAllSelect($('[name="product"]')));

            //遍历选中商品
            var _count = 0;   //商品数
            var _totalPrice = 0;  //价格
            _selectShopIdArr.length = 0;
            $('[name="product"]').each(function (i) {
                if ($(this).is(":checked")) {
                    _selectShopIdArr.push($(this).parents('.item').attr('data-shop-id'));
                    _count++;
                    var _unitPrice = $(this).parents('.item').find('.pay-price').text().replace('¥', '').trim() * 1;
                    var _quantity = $(this).parents('.item').find('.quantity').text().replace('X', '').trim() * 1;
                    _totalPrice = _totalPrice + _unitPrice * _quantity;
                }
            });
            $('.cart-footer .btn span').eq(2).text(_count);
            $('.cart-footer .price span').text(_totalPrice.toFixed(2));
        }
        //购买数量变更事件
        function changeInputEvent(dom) {
            var _max = $(dom).prop('max') * 1;
            var _min = $(dom).prop('min') * 1;
            var _current = $(dom).val() * 1;
            var _unit = 1; //打捆销售捆数量
            if ($(dom).attr('isbundling') == '1') {
                _unit = JSON.parse($(dom).attr('bundlingjson')).count * 1;
            }
            if (_current % _unit != 0) {
                _current = _current - _current % _unit;
            }
            if (_current > _max) {
                $.toast('亲,该宝贝不能购买更多了哦~');
                var _val = _max - _max % _unit;
                $(dom).val(_val);
                $(dom).parents('.item').find('.quantity').html('<em>X</em>' + _val);
            }
            if (_current < _min * _unit) {
                $.toast('受不了了,不能再减少了啊~');
                $(dom).val(_min * _unit);
                $(dom).parents('.item').find('.quantity').html('<em>X</em>' + _min * _unit);
            }
            var _param = { 'shop_id': $(dom).parents('.item').attr('data-shop-id'), 'shop_count': $(dom).val() * 1 };
            var _arr = [];
            _arr.push(_param);
            updateCartShopCount({ 'shop_counts': JSON.stringify(_arr) }, function (data) {
                console.log(data);
            });
        }

        //选择规格赋予样式变化
        function propertySelect(obj) {
            if (obj) {
                //map_id = obj.map_id;
                $('#property-warp .property_img').prop('src', obj.img + ',1,80,80,3');
                $('#property-warp .property_price').text(obj.price);
                $('#property-warp .remain_inventory').text(obj.remain_inventory);
                //样式变化
                $('#property-warp .property_select').removeClass('current');
                $('#property-warp .map_id' + obj.map_id).addClass('current');
            }
        };

        //判断是否全部选中
        function isAllSelect(dom) {
            var _result = true;
            $(dom).each(function (i) {
                if ($(this).is(":checked") == false) {
                    _result = false;
                    return;
                }
            });
            return _result;
        }

        //绑定页面事件
        function bindPageEvent() {
            //全选触发
            $(_this).find('[name="all"]').unbind('change').on('change', function () {
                if ($('[name="all"]').is(':checked')) {
                    $('[name="shop"],.cb-shop').prop('checked', true);
                } else {
                    $('[name="shop"],.cb-shop').prop('checked', false);
                }
                changeSelectedEvent();
            });
            //店铺选中触发
            $(_this).find('[name="shop"]').unbind('change').on('change', function () {
                if ($(this).is(':checked')) {
                    $(this).parents('.bundle').find('[name="product"]').prop('checked', true);
                } else {
                    $(this).parents('.bundle').find('[name="product"]').prop('checked', false);
                }
                changeSelectedEvent();
            });
            //单个商品选中触发
            $(_this).find('[name="product"]').unbind('change').on('change', function () {
                if ($(this).is(':checked')) {
                    if (isAllSelect($(this).parents('.bundle').find('[name="product"]'))) {
                        //兄弟也都选中了  选中店铺
                        $(this).parents('.bundle').find('[name="shop"]').prop('checked', true);
                    }
                } else {
                    //取消选中店铺
                    $(this).parents('.bundle').find('[name="shop"]').prop('checked', false);
                }
                changeSelectedEvent();
            });

            //绑定编辑
            $(_this).find('.edit').unbind('click').on('click', function () {
                if ($(this).text() == '编辑') {
                    $(this).text('完成');
                    $(this).parents('.bundle').find('.edit-false').removeClass('edit-false').addClass('edit-true');
                } else {
                    $(this).text('编辑');
                    $(this).parents('.bundle').find('.edit-true').removeClass('edit-true').addClass('edit-false');
                    changeSelectedEvent();
                }
            });

            //绑定商品数量+-
            $(_this).find('.edit-quantity .btn').unbind('click').on('click', function () {
                var $dom = $(this).parents('.edit-quantity').find('.btn-input');
                var _value = 0;
                var _unit = 1;
                if ($dom.attr('isbundling') == '1') {
                    _unit = JSON.parse($dom.attr('bundlingjson')).count * 1;
                }
                if ($(this).hasClass('btn-minus')) {
                    //$dom = $(this).next();
                    _value = $dom.val() * 1 - _unit;
                } else {
                    //$dom = $(this).prev();
                    _value = $dom.val() * 1 + _unit;
                }
                $dom.val(_value);
                $(this).parents('.item').find('.quantity').html('<em>X</em>' + _value);
                changeInputEvent($dom);
            });
            //商品数变更时发生
            $(_this).find('.edit-quantity .btn-input').unbind('change').on('change', function () {
                changeInputEvent($(this))
            });
            //删除购物车商品
            $(_this).find('.item-del').unbind('click').on('click', function () {
                var $dom = $(this).parents('.item');
                var _arr = [];
                _arr.push($dom.attr('data-shop-id'));
                delShopCart(_arr, function (data) {
                    console.log(data);
                    if (data.success == 'true') {
                        if ($dom.prevAll().length < 1 && $dom.nextAll().length < 1) {
                            $dom.parents('.bundle').remove();
                        }
                        else {
                            $dom.remove();
                        }
                        if ($(_this).find('.bundle').length == 0) {
                            window.location.reload();
                        }
                        changeSelectedEvent();
                    } else {
                        $.toast(data.msg);
                    }
                });
            });
            //编辑属性
            $(_this).find('.edit-sku').unbind('click').on('click', function () {
                //$.toast('编辑属性');
                var $dom = $(this).parents('.item');
                var _sku_id = $dom.attr('data-sku-id');
                getPropertyDb($dom.attr('data-p-id'), function (data) {
                    console.log(data);
                    property_list = data;
                    if (data.length > 0) {
                        $('#property-warp').html(template('t-cart-property', { list: property_list }));
                        var _defaultObj;
                        var _defaultData = $.grep(property_list, function (j, k) { return j.map_id == _sku_id; });
                        if (_defaultData.length > 0) {
                            _current_sku_id = _sku_id;
                            _current_shop_id = $dom.attr('data-shop-id');
                            _defaultObj = _defaultData[0];
                            propertySelect(_defaultObj);
                            $.popup($('#property-warp'));
                        }
                    }
                });
            });
            //规格选择
            $('#property-warp').off('click', '.property_select').on('click', '.property_select', function () {
                var _id = $(this).data('id');
                var _selectData = $.grep(property_list, function (j, k) { return j.map_id == _id; });
                if (_selectData.length == 0) return false;
                propertySelect(_selectData[0]);
            });
            //规格确定
            $('#property-warp').off('click', '.go_shop_order').on('click', '.go_shop_order', function () {
                $.closeModal();
                var _select_sku_id = $('#property-warp .property_select.current').data('id');
                if (_select_sku_id != _current_sku_id * 1) {
                    //变更了sku
                    var _arr = [];
                    var _obj = {
                        shop_id: _current_shop_id, property_map_id: _select_sku_id
                    };
                    _arr.push(_obj);
                    updateCartMapId({ property_map_ids: JSON.stringify(_arr) }, function (data) {
                        if (data.success == 'true') {
                            //修改成功
                            $.toast('修改成功');
                            location.reload()
                        }
                    });
                }
            });

            //结算
            $(_this).find('.cart-footer .btn').unbind('click').on('click', function () {
                if (_selectShopIdArr.length > 0) {
                    var _ids = _selectShopIdArr.join(',');
                    location.href = '/order_2/handorder.html?shop_ids=' + _ids;
                }
                else {
                    $.toast('请勾选商品在提交订单.');
                }
            });
        }
    });

    //签到入口
    publicPageInit('#page-signIn-dir', function (e, id, page) {
        var _this = page;

        if (window.sessionStorage['backState'] == 1) {
            window.sessionStorage['backState'] = 2;
            $.router.back();
        }

        init();

        function init() {
            document.title = parameters.activity.signIn.title[0];
            loadDataAndBindPage();
            bindEvent();
        }
        function loadDataAndBindPage() {
            loadActivityRuleDate();
            getActivityData(false);
        }

        function showActivityDescription() {
            $(_this).show();
        }

        function getActivityData(isToast) {
            if (!isToast) {
                showActivityDescription();
            }
            if (userIsAuth() == true) {
                //获取用户信息,判断用户是否参参与活动
                initSignIn(parameters.activity.signIn.baseParams, function (obj) {
                    if (obj.success == 'true') {
                        parameters.activity.signIn.activityData = obj.data;
                        if (obj.data.SignInState == 1) {
                            if (isToast) {
                                $.router.back();
                            }
                        }
                        else {
                            $.router.load("#page-signIn-index");
                        }
                    }
                    else {
                        if (isToast) {
                            $.toast(obj.msg);
                        }
                        //setTimeout(function () {
                        //    $.router.back();
                        //}, 1200);
                    }
                });
            }
            else {
                if (isToast) {
                    $.toast('请先登录');
                    publicGoLogin();
                }
            }
        }

        //获取活动规则信息
        function loadActivityRuleDate() {
            var setting = parameters.activity.signIn.setting;
            if (setting == null) {
                getSignInSetting(parameters.activity.signIn.baseParams, function (obj) {
                    if (obj.success == 'true') {
                        parameters.activity.signIn.setting = obj.data;
                        bindActivityRuleToPage(obj.data.rule_description);
                    } else {
                        $.toast('活动已过期');
                    }
                });
            } else {
                bindActivityRuleToPage(setting.rule_description);
            }
        }

        //绑定规则到页面
        function bindActivityRuleToPage(rule) {
            $(_this).find('.rule-text').html(rule.replace(/#/g, '<br>'));
        }

        function bindEvent() {
            //参与活动
            $(_this).find('.poster-btn').unbind('click').on('click', function () {
                getActivityData(true);
            });
            //关闭
            $(_this).find('.close-poster').unbind('click').on('click', function () {
                //txapp.goIndex();
                ////$.router.backLoad(!txapp.isApp() ? '/' : '' + 'index.html');
                //location.href = !txapp.isApp() ? '/' : txapp.appHostPath + 'index.html';
                $.router.back();
            });
        }
    });

    //签到页面
    publicPageInit('#page-signIn-index', function (e, id, page) {
        var _this = page;
        var _userActivityData = parameters.activity.signIn.activityData;

        if (parameters.activity.signIn.setting == null || _userActivityData == null) {
            window.sessionStorage['backState'] = 2;
            location.href = './signin.html';
        } else {
            window.sessionStorage['backState'] = 1;
            init();
        }
        function init() {
            document.title = parameters.activity.signIn.title[1];
            //console.log(parameters.activity.signIn);
            loadDataAndBindPage();
            bindEvent();
        }
        function loadDataAndBindPage() {
            //签到规则
            $(_this).find('.rule-text').html(parameters.activity.signIn.setting.rule_description.replace(/#/g, '<br>'));
            //显示汇总信息
            showSummenyInfo(_userActivityData);
            //签到提醒
            if (!_userActivityData.IsOpenRemind) {
                $(_this).find('.checkbox-em').removeClass('current');
            }
            //截止上次签到 有漏签 提示
            if (_userActivityData.MissingCount > 0) {
                missingSignInToast(_userActivityData);
            }

            bindAndShowSigninDetails();
        }

        function bindAndShowSigninDetails() {
            //签到历史
            var daterConfig = {
                maxDate: new Date(),
                minDate: new Date(),
                container: '.shade-box4 .sign-rili'
            };
            getSignInDetailsList({ eligible_id: _userActivityData.EligibleId }, function (obj) {
                if (obj.success == 'true') {
                    if (obj.data.length > 0) {
                        parameters.activity.signIn.detailsList = obj.data;
                        daterConfig.detailsList = obj.data;  //签到记录数据 
                    }
                    daterConfig.minDate = new Date(new Date(_userActivityData.InActibityTime).getTime() - 24 * 60 * 60 * 1000);
                    daterConfig.maxDate = new Date(_userActivityData.InActibityTime);
                    daterConfig.maxDate.setDate(new Date(_userActivityData.InActibityTime).getDate() + _userActivityData.TotalCount);
                    daterConfig.HasBeenMoney = _userActivityData.HasBeenBrokerage + _userActivityData.HasBeenBonus;
                    daterConfig.TotalMissingMoney = _userActivityData.TotalMissingBrokerage + _userActivityData.TotalMissingBonus;
                    //回调
                    //更新页面日历
                    daterConfig.callBack = function () {
                        var _sliceIndex = new Date().getDate() - new Date().getDay();
                        if (_sliceIndex < 0) { _sliceIndex = 0; }
                        var riliItem = $('.md_datearea li').slice(_sliceIndex, _sliceIndex + 7);
                        $('.history_sign ul').empty();
                        riliItem.forEach(function (item) {
                            $('.history_sign ul').append(item.outerHTML);
                        });
                    }
                    $('.j-history').mdater(daterConfig);
                }
            });
        }


        //显示汇总信息
        function showSummenyInfo(userData) {
            var html = '<li><h2>签到累计获得</h2>'
                + '<p>现金<span>' + userData.HasBeenBrokerage.toFixed(2)
                + '</span>元&nbsp;&nbsp;&nbsp;&nbsp; 创业金<span>' + userData.HasBeenBonus
                + '</span>元</p></li>'
                + '<li><h2>签到冻结奖励</h2>'
                + '<p>现金<span>' + userData.FreezeBrokerage.toFixed(2)
                + '</span>元&nbsp;&nbsp;&nbsp;&nbsp; 创业金<span>' + userData.FreezeBonus
                + '</span>元</p></li>'
            $(_this).find('.sign-textinfo ul').empty().append(html);
            if (userData.TodayIsSignIn) {
                //按钮不可用
                $(_this).find('.sign-btn').addClass('sign-btn-ok');
            } else {
                $(_this).find('.sign-btn').removeClass('sign-btn-ok');
            }
        }

        //漏签提醒
        function missingSignInToast(userData) {
            var html = '<h2 class="">亲，截止上次签到您共漏签' + userData.MissingCount + '次</h2>'
                + '<p> 错失了</p>'
                + '<p>现金红包<span> ' + userData.MissingBrokerage.toFixed(2) + '</span>  元</p>'
                + '<p>创业金<span> ' + userData.MissingBonus + '</span>  元</p>'
                + '<a href="#" class="j-close-popup-box">知道了</a>';
            $(_this).find('.shade-box3 .forget-sign').append(html);
            $(_this).find('.shade-box3').show();
        }

        //分享
        function shareActivity() {
            if (txapp.isApp()) {
                $(_this).find('.j-share').off('click').on('click', function () {
                    txapp.signInShare({
                        HasBeenBrokerage: _userActivityData.HasBeenBrokerage.toFixed(2) || 0.00,
                        HasBeenBonus: _userActivityData.HasBeenBonus || 0
                    });
                });
            }
        }

        function bindEvent() {
            //分享
            shareActivity();
            //签到提醒
            $(_this).find('.checkbox-em').unbind('click').on('click', function () {
                //非app环境进行提醒
                if (!txapp.isApp()) {
                    $.confirm('前往买客APP方可设置每日签到领现金提醒?', '前往下载',
                        function () {
                            location.href = '//www.7518.cn/Index_wap.html';
                        }
                    );
                    return;
                }
                var param = {
                    eligible_id: _userActivityData.EligibleId,
                    is_open: !$('.checkbox-em').hasClass('current')
                };
                changeRemindState(param, function (obj) {
                    if (obj.success == 'true') {
                        if (!param.is_open) {
                            $.toast('签到提醒关闭了');
                            //$.alert('签到提醒关闭了,明天开始收不到通知啦,错过奖励不要怪我哦。', '签到提醒已关闭');
                            $('.checkbox-em').removeClass('current')
                            $('.checkbox-em').find('#sign').attr('checked', 'false')
                        } else {
                            $.toast('签到提醒已经开启');
                            //$.alert('从明天起，做个幸福的人，每天上网、签到、赚钱,啥都不耽误，哈哈O(∩_∩)O哈哈~', '签到提醒已开启');
                            $('.checkbox-em').addClass('current')
                            $('.checkbox-em').find('#sign').attr('checked', 'true')
                        }
                    } else {
                        $.toast(param.is_open ? '开启' : '关闭' + '提醒失败.');
                    }
                });

            });
            //签到
            $(_this).find('.j-signIn').unbind('click').on('click', function () {
                //非app环境进行提醒
                if (!txapp.isApp()) {
                    $.alert('前往APP才能继续签到领取现金奖励哦~~', '前往下载',
                        function () {
                            location.href = '//www.7518.cn/Index_wap.html';
                        }
                    );
                    return;
                }
                if (!_userActivityData.TodayIsSignIn) {
                    //今天没有签到才能签到
                    setSignIn({ eligible_id: _userActivityData.EligibleId }, function (obj) {
                        if (obj.success == 'true') {
                            _userActivityData.TodayIsSignIn = true;
                            //弹窗信息 
                            var $shadeDom = $(_this).find('.shade-box2');
                            var $span = $shadeDom.find('span');
                            var $titleContioner = $shadeDom.find('.success-top');
                            if (!obj.data.IsCrit) {
                                $titleContioner.find('h2').text('签到成功');
                                $titleContioner.removeClass('success-top-crit');
                            } else {
                                $titleContioner.find('h2').text('恭喜您获得暴击');
                                $titleContioner.addClass('success-top-crit');
                            }
                            $span.eq(0).html(obj.data.GetBrokerage + obj.data.GetBouns);
                            $span.eq(1).html(obj.data.GetBrokerage);
                            $span.eq(2).html(obj.data.GetBouns);
                            $shadeDom.show();
                            //主页面金额信息
                            parameters.activity.signIn.activityData.HasBeenBrokerage += obj.data.GetBrokerage;
                            parameters.activity.signIn.activityData.HasBeenBonus += obj.data.GetBouns;
                            parameters.activity.signIn.activityData.FreezeBrokerage += obj.data.GetBrokerage;
                            parameters.activity.signIn.activityData.FreezeBonus += obj.data.GetBouns;
                            _userActivityData = parameters.activity.signIn.activityData;
                            showSummenyInfo(_userActivityData);
                            bindAndShowSigninDetails();
                        } else {
                            $.toast('签到失败,请重试');
                        }
                    });

                }
            });
            //一键领取
            $(_this).find('.j-receive-btn').unbind('click').on('click', function () {
                var param = {
                    eligible_id: _userActivityData.EligibleId
                };
                transferSignInAssets({ eligible_id: _userActivityData.EligibleId }, function (obj) {
                    if (obj.success == 'true') {
                        $.toast('转出成功');
                        parameters.activity.signIn.activityData.FreezeBrokerage = 0.00;
                        parameters.activity.signIn.activityData.FreezeBonus = 0;
                        _userActivityData = parameters.activity.signIn.activityData;
                        $('.sign-textinfo span').eq(2).html('0.00');
                        $('.sign-textinfo span').eq(3).html('0');
                    } else {
                        $.toast(obj.msg);
                    }
                });
            });

            //关闭弹出层
            $(_this).find('.j-close-popup-box').unbind('click').on('click', function () {
                $(_this).find('.shade_box').hide();
            });
            //签到历史
            $(_this).find('.j-history').unbind('click').on('click', function () {
                $(_this).find('.shade-box4').show();
            });
            //签到规则
            $(_this).find('.j-rule').unbind('click').on('click', function () {
                $(_this).find('.shade-box1').show();
            });
            //参与活动  返回首页
            $(_this).unbind('click').on('click', '.close-poster', function () {
                $.router.back();
            });
        }
    });

    //活动规则页面
    publicPageInit('#page-rebate-rule', function (e, id, page) {
        var _this = page;
        init();
        //初始化页面
        function init() {
        };
    }, function () {
        txapp.getRebateRulePageHeight({ 'height': $('.activity-rule').height() });
    });
    //高额返利活动-活动页面
    publicPageInit('#page-activity-rebate', function (e, id, page) {
        var _this = page;
        init();
        //初始化页面
        function init() {
            if (typeof pageData == 'undefined' || pageData.success != 'true' || pageData.data.RebateState == 0) {
                if (typeof pageData == 'undefined') {
                    $.toast('网络异常，请刷新重试');
                } else {
                    $.toast(pageData.msg);
                }
                //setTimeout(function () { $.router.back(); }, 1500);
                return;
            }
            else {
                parameters.activity.pageData = pageData.data;
                var _pageData = parameters.activity.pageData;
                //RebateState
                //0商品未参加活动
                //1正常参加
                //2活动暂停
                //3活动停止倒计时
                //4活动停止

                //倒计时
                if (_pageData.RebateState == 2 || _pageData.RebateState == 3) {
                    InitCountdown(_pageData.RebateState);
                }
                //按钮状态
                if (_pageData.RebateState == 2 || _pageData.RebateState == 4) {
                    //购买按钮
                    var $btnBuy = $(_this).find('.btn_buy');
                    var _showTitle = _pageData.RebateState == 2 ? '活动暂停' : '活动停止';
                    $btnBuy.text(_showTitle).addClass('btn_dis');
                    if ($btnBuy.length > 0) {
                        $btnBuy[0].href = '';
                    }

                    //签到按钮
                    $(_this).find('.j-success').text(_showTitle).addClass('btn_dis');
                }



                /*处理按钮文本*/
                var _showText = '立即签到';
                if (_pageData.RebateState == 3 || _pageData.RebateState == 4) {
                    _showText = '活动结束';
                }
                if (_pageData.RebateState == 2) {
                    _showText = '活动暂停';
                }
                else if (_pageData.ActivityState == 4) {
                    _showText = '签到结束';
                }
                else if (_pageData.PageData.TodayIsSignIn == true) {
                    _showText = '已签到';
                }
                $(_this).find('.j-success').text(_showText);

                //ActivityState
                //0没有参与活动   
                //1已经获得返利资格,但是没有获得签到资格  
                //2已经获得签到资格可以正常签到  
                //3签到资格被冻结需重新激活
                //4本次签到已经完成

                //顶部商品
                switch (_pageData.ActivityState) {
                    case 1:
                        InitGetSignIn(_pageData.PageData);
                        break;
                    case 2:
                    case 3:
                    case 4:
                        InitSignInOK(_pageData);
                        break;
                    default:
                        break;
                }
                revotePageHeight();
                generateSignInList(_pageData.PageData.SignDetail);
                bindEvent();
            }
        };

        function generateSignInList(signDetail) {
            var _days = getCurrentWeekDays();  //当前周日期集合（周日开始，共7天）
            if (_days.length == 7) {
                var _html = '';
                var _date = new Date();
                for (var i = 0; i < _days.length; i++) {
                    var _className = 'pic0'
                    var _text = dateFormat(_days[i], 'MM.dd');
                    //小于10月去掉0
                    if (_text.substring(0, 1) == '0') {
                        _text = _text.substring(1);
                    }
                    //小于当前天的判断签到
                    if (new Date(_days[i]) < _date) {
                        _className = isSignIn(_days[i], signDetail) ? 'pic1' : 'pic2';
                    }
                    _html += '<li class="' + _className + '"><span class="sign_in_pic"></span>' + _text + '</li>';
                }
                $(_this).find('.sign_in_box>.calendar').empty().append(_html);
            }
        }
        //判断是否签到
        function isSignIn(date, list) {
            if (!list) return false;
            for (var i = 0; i < list.length; i++) {
                if (new Date(list[i].SignTime) - new Date(date) == 0) {
                    return true;
                }
            }
            return false;
        }
        //已经获得签到页面
        function InitSignInOK(data) {
            //奖励汇总 以及收益值
            var $moneyTxt = $('.j_money');
            if ($moneyTxt.length == 7) {
                $moneyTxt.eq(0).text(data.Summary[0].TotalSignInMoney.toFixed(2));
                $moneyTxt.eq(1).text('￥' + data.Summary[0].GainedSignInMoney.toFixed(2));
                $moneyTxt.eq(2).text('￥' + data.Summary[0].GainedRebateMoney.toFixed(2));
                $moneyTxt.eq(3).text('￥' + data.Summary[0].ExpectedSignInMoney.toFixed(2));
                $moneyTxt.eq(4).text('￥' + data.Summary[0].ExpectedRebateMoney.toFixed(2));
                $moneyTxt.eq(5).text('￥' + data.Summary[0].TotalSignInMoney.toFixed(2));
                $moneyTxt.eq(6).text('￥' + data.Summary[0].TotalRebateMoney.toFixed(2));
            }

            //显示历史签到和立即签到按钮
            $(_this).find('.j-success,.j-history').removeClass('dis-n');
            //签到历史
            $(_this).find('.j-history').unbind('click').on('click', function () {
                //$('.shade-box3').show();
                $.confirm('前往买客APP方可查看签到历史', '前往下载',
                    function () {
                        location.href = '//www.7518.cn/Index_wap.html';
                    }
                );
            });
            //返利汇总
            $(_this).find('.btn_summary').unbind('click').on('click', function () {
                $('.shade-box1').show();
            });
            //立即签到
            $(_this).find('.j-success').unbind('click').on('click', function () {
                //if (data.RebateState == 2 || data.RebateState == 4) {
                //    return;
                //}
                //$('.shade-box0').show();
                $.confirm('前往买客APP方可签到', '前往下载',
                    function () {
                        location.href = '//www.7518.cn/Index_wap.html';
                    }
                );
            });
        }
        //获取签到页面
        function InitGetSignIn(data) {
            var _needUsers = data.SignUserCount - data.UserList.length;
            var _html = ['<h2>已获得活动参与资格</h2>',
                '<p>还需邀请',
                _needUsers,
                '人购买本商品，可获得签到领现金资格。</p><ul>'];
            for (var i = 0; i < data.UserList.length; i++) {
                var _headPic = data.UserList[i].HeadPic;
                if (_headPic.length == 0) {
                    _headPic = '/Skin/t/Img/no_pic.png';
                }
                _html.push('<li><img class="user" src="' + _headPic + '" /></li>');
            }
            for (var i = 0; i < _needUsers; i++) {
                _html.push('<li><img src="/Skin/t/Img/Activity/headpic.png" /></li>');
            }
            _html.push('</ul>');
            _html.push('<p>我要签到赚回购买商品的本金</p>');
            _html.push('<a href="" class="rebate_btn btn_share">分享此商品给好友</a>');
            $(_this).find('.main_two').html(_html.join(''));
            //分享提示
            $(_this).find('.btn_share').unbind('click').on('click', function () {
                $.confirm('前往买客APP方可分享?', '前往下载',
                    function () {
                        location.href = '//www.7518.cn/Index_wap.html';
                    }
                );
            });
        }
        //倒计时相关
        function InitCountdown(state) {
            var $wrap = $(_this).find('.m-countDown');
            var _sequence = state == 3 ? true : false;
            var _maxMillisecond = $wrap.attr('data-maxday') * 24 * 60 * 60 * 1000;
            var _date = $wrap.attr('data-time') * 1000; //毫秒时间戳
            //活动倒计时 CountDownTime(时间, 所需展示的标签选择器, true/false（倒计时/正计时,默认true）,正计时触发回调的最大毫秒,回调)
            CountDownTime(_date, $wrap.find('.countdown'), _sequence, _maxMillisecond, function () {
                location.reload();
            });
        }

        function revotePageHeight() {
            //签到高度适应全屏
            var top_height = $(_this).find('.m-product').outerHeight(true)
            var head_height = $(_this).find('header').height()
            var html_height = $(_this).height()
            var con_height = $(_this).find('.m-main .main_two').outerHeight(true)
            $(_this).find('.main_three').height(html_height - head_height - top_height - con_height)
            //签到日期当连续签到时，日期中间横线变绿色
            var $li_name = $(_this).find('.calendar li');
            for (var i = 0; i < $li_name.length; i++) {
                $li_name.eq(i).attr('data-green', $li_name.eq(i).hasClass(''))
            }
        }
        //绑定事件
        function bindEvent() {
            //关闭弹出层
            $(document).find('.j-close-popup-box').unbind('click').on('click', function () {
                $('.shade_box').hide();
            });

            //签到规则
            $(_this).find('.j-rule').unbind('click').on('click', function () {
                $('.shade-box2').show();
            });
        }
    });
    //高额返利活动-返利收益
    publicPageInit("#page-activity-summary", function (e, id, page) {
        var _this = page;
        var _pageData;
        //无限滚动
        var loading = false;

        if (parameters.activity.pageData) {
            _pageData = parameters.activity.pageData;
            Init();
        }
        else {
            var _params = { product_id: getUrlParam('id'), is_back_product: 1 };
            initRebateEligible(_params, function (obj) {
                parameters.activity.pageData = obj.data;
                _pageData = parameters.activity.pageData;
                Init();
            });
        }

        function Init() {
            //正常入口，有全局缓存数据才能进入
            if (parameters.activity.rebateSummary.isFirstLoad) {
                parameters.activity.rebateSummary.isFirstLoad = false;
                parameters.activity.rebateSummary.params = {
                    page_index: 0, page_size: 8, product_id: _pageData.ProductInfo.Id
                };
                bindBaseData();
                loadUserData();
                loadMsgData();
                //无限滚动事件
                $(_this).on('infinite', '.infinite-scroll', function () {
                    if (loading) return;
                    loading = true;
                    loadMsgData();
                });
                //全部团队成员
                $(_this).find('.btn_all_user').unbind('click').on('click', function () {
                    $.confirm('前往买客APP方可查看全部成员?', '前往下载',
                        function () {
                            location.href = '//www.7518.cn/Index_wap.html';
                        }
                    );
                });
            }
        };
        //基础数据绑定
        function bindBaseData() {
            $(_this).find('.e_money_total').text(_pageData.Summary[0].TotalRebateMoney.toFixed(2));
            $(_this).find('.e_money_preparatory').text(_pageData.Summary[0].ExpectedRebateMoney.toFixed(2));
            //判断是否需要隐藏
            if (_pageData.IsFinish == 1) {
                //显示
                $(_this).find('.btn_buy_again').removeClass('dis-n');
            }
            else {
                $(_this).find('.btn_buy_again').remove();
            }
        }
        //活动推荐用户列表
        function loadUserData() {
            getRecommendUserList({ top: 10, product_id: _pageData.ProductInfo.Id }, function (obj) {
                if (obj.success == 'true') {
                    if (obj.data.Count > 0) {
                        if (obj.data.Count < 10) {
                            $(_this).find('.btn_all_user').removeClass('dis-n');
                        }
                        $(_this).find('.j_userCount').text(obj.data.Count);
                        $(_this).find('.j_userList').append(template('t_user_list', {
                            list: obj.data.List
                        }));
                    }
                } else {
                    $.toast(obj.msg);
                }
            });
        }
        //活动推送消息列表
        function loadMsgData() {
            getRebateOrderMsgList(parameters.activity.rebateSummary.params, function (params, obj) {
                if (params.page_index == 0) {
                    $(_this).find('.no_more').empty();
                    $(_this).find('.j_msgList').html('');
                    if (obj.length == 0) {

                        $(page).find('.j_msgList').html('<li style="text-align:center;">无记录</li>');
                    }
                }
                if (obj.length > 0) {
                    for (var i = 0; i < obj.length; i++) {
                        obj[i].MsgContent = obj[i].MsgContent.replace('{parentNickName}', obj[i].RecommentNickName);
                        obj[i].MsgContent = obj[i].MsgContent.replace('{payNickName}', obj[i].ShopNickName);
                        obj[i].MsgContent = obj[i].MsgContent.replace('{payShopCount}', obj[i].ShopCount);
                        obj[i].MsgContent = obj[i].MsgContent.replace('{getBrokearge}', obj[i].GetBrokerage);
                        obj[i].MsgContent = obj[i].MsgContent.replace('{totalBrokerage}', obj[i].TotalBrokerage);
                    }
                    $(_this).find('.j_msgList').append(template('t_msg_list', {
                        list: obj
                    }));
                    loading = false;
                }
                if (obj.length < params.page_size) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($(_this).find('.infinite-scroll'));
                    // 删除加载提示符
                    $(_this).find('.infinite-scroll-preloader').remove();
                    // 添加底部提示
                    $(_this).find('.no_more').empty();
                    $(_this).find('.content').append('<div class="no_more">----我是有底线的----</div>');
                }
                params.page_index++;
            });
        };
    });
    //高额返利活动-返利列表
    publicPageInit("#page-activity-list", function (e, id, page) {
        if (parameters.activity.rebateList.isFirstLoad) {
            parameters.activity.rebateList.isFirstLoad = false;
            parameters.activity.rebateList.params = {
                page_index: 0, page_size: 16
            };
            loadData();
        }
        //活动列表
        function loadData() {
            getRebateProductList(parameters.activity.rebateList.params, function (params, obj) {
                console.log(parameters.activity.rebateList.params);
                if (params.page_index == 0) {
                    $(page).find('.no_more').empty();
                    $(page).find('.activities_list').html('');
                }
                if (obj.length > 0) {
                    $(page).find('.activities_list').append(template('t_activities_list', {
                        list: obj
                    }));
                    loading = false;
                }
                if (obj.length < params.page_size) {
                    loadOver();
                }
                params.page_index++;
            });
        };

        function loadOver() {
            // 加载完毕，则注销无限加载事件，以防不必要的加载
            $.detachInfiniteScroll($(page).find('.infinite-scroll'));
            // 删除加载提示符
            $(page).find('.infinite-scroll-preloader').remove();
            // 添加底部提示
            $(page).find('.content').append('<div class="no_more">----我是有底线的----</div>');
        }
        //无限滚动
        var loading = false;
        $(page).on('infinite', function () {
            if (loading) return;
            loading = true;
            loadData();
        });
    });

    // 添加'refresh'监听器
    $(document).on('refresh', '.pull_content_reload', function (e) {
        window.location.reload();
    });
    var pull_content_reload_init_callback = function () {
    }
    $(document).on('refresh', '.pull_content_reload_init', function (e) {
        pull_content_reload_init_callback();
    });
    $(document).on('refresh', '.pull_content_back', function (e) {
        $.router.back();
    });

    $.init();
});

//设置右上角消息提示
function setMsgState(_this) {
    //未登录 不执行
    if (!userIsAuth()) {
        return;
    }
    $(_this).find('.msg .badge').remove();
    //判断距离上一次更新信息时间 如果超过30秒  更新一次
    var currentTime = new Date().getTime();
    if ((currentTime - parameters.memberCountInfo.lastUpdateTime) / (1000 * 30)) {
        selectMemberIndexCountV2(function (data) {
            if (data.length == 1) {
                parameters.memberCountInfo.data = data[0];
                parameters.memberCountInfo.lastUpdateTime = currentTime;
            } else {
                parameters.memberCountInfo.data = null;
            }
        });
    }
    //读取数据改变状态
    if (parameters.memberCountInfo.data != null) {
        var sysCount = parseInt(parameters.memberCountInfo.data['@SysmsgCount']);
        if (sysCount == 0) {
            return;
        }  //消息数0不提示
        var badgeTemplate = '<span class="badge"></span>';
        $(_this).find('.msg').append(badgeTemplate);
    }
}
//进入消息按钮 交互
function bindMsg(_this) {
    if (txapp.isApp()) {
        //按钮交互
        $(_this).find('.j-app-active').off('click').on('click', function () {
            //个人首页-消息
            txapp.indexMessage();
        });
    } else {
        //如果不是app中打开 不显示顶部图显菜单 消息页面绑定
        $(_this).find('.j-app-active').eq(0).addClass('external')[0].href = '/news/index.html';
    }
}

//绑定进店逛逛
function bindGoStore(_this) {
    //绑定进店逛逛
    //$(_this).off().on('click', '.j-gostore', function () {
    //    var mchid = $(this).attr('data-id');
    //    if (txapp.isApp()) {
    //        txapp.store({
    //            mchid: mchid
    //        });
    //    }
    //    else {
    //        $.router.load('/shop_2/mchstore.html?show=1&mchid=' + mchid);
    //    }
    //});
}
//评价图片浏览
function bindPhotoBrowserEvent(_this) {
    //移除上次容器  防止报错
    $(document).on('click', '.photo-browser-close-link', function () {
        $('.photo-browser-out').remove();
    });
    $(_this).find('.pic_list .review-img').off('click').on('click', function () {
        var msg = $(this).parents('.review-content').text().trim();
        var imgurls = [];
        var imgIndex = parseInt($(this).attr('data-index'));
        $(this).parents('.pic_list').find('.review-img').each(function (i) {
            imgurls.push($(this).attr('data-id'));
        });
        if (txapp.isApp()) {
            txapp.imgPreview({
                imgurls: imgurls.join(','), imgindex: imgIndex, caption: msg
            });
        }
        else {
            var imgArray = [];
            $.each(imgurls, function (index, value) {
                imgArray.push({
                    url: value,
                    caption: msg
                });
            });
            //获取数据
            var myPhotoBrowserCaptions = $.photoBrowser({
                photos: imgArray,
                initialSlide: imgIndex,
                theme: 'dark',
                type: 'standalone'
            }).open();
        }
    });
}
//从数组中删除一个元素
function removeByValue(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == val) {
            arr.splice(i, 1);
            break;
        }
    }
    return arr;
}
//数组去重
function arrayUnique(arr) {
    var res = [];
    var json = {
    };
    for (var i = 0; i < arr.length; i++) {
        if (!json[arr[i]]) {
            res.push(arr[i]);
            json[arr[i]] = 1;
        }
    }
    return res;
}
//处理商品数据，计算距离，选取展示模板  用户地理位置记录在当前session中
function processProductsData(obj) {
    //处理数据
    var objCount = obj.length;
    var coordinate = null;
    if (sessionStorage['tx-coordinate'] != null && sessionStorage['tx-coordinate'] != ''
        && sessionStorage['tx-coordinate'].split(',').length == 2) {
        coordinate = sessionStorage['tx-coordinate'].split(',');
        //alert('实际计算的经纬度：' + sessionStorage['tx-coordinate']);
    }
    for (var i = 0; i < objCount; i++) {
        //如果用户坐标不为空 计算距离
        if (coordinate != null && coordinate[0] != 0 && coordinate[1] != 0) {
            if (obj[i].Lat != 0 && obj[i].Lng != 0) {  //经纬度计算距离
                obj[i].Distance = Distance.getFlatternDistance(coordinate[0], coordinate[1], obj[i].Lat, obj[i].Lng);
            }
        }
        //如果主图小于3张
        if (obj[i].ProductImgs.split(',').length < 3) {
            obj[i].ProductImgs = obj[i].ProductImgs.split(',')[0];
            obj[i].TemplateId = 2;
        }
        else {
            //每隔3个 改变一个，并且不能有连着的2个
            if (i > 1 && i < objCount - 1 && obj[i - 1].TemplateId != 2 && obj[i + 1].ProductImgs.split(',').length > 3 && (i % 3 == 0)) {
                obj[i].TemplateId = 2;
            }
            else {
                obj[i].TemplateId = 1;
            }
        }
    }
    return obj;
}
//处理点赞事件，成功改变页面
function bindLinkEvent(_this) {
    //没有点赞的绑定点赞事件
    $(_this).find(".like-active").unbind().bind('click', function () {
        var that = this;
        var params = {
            'reviewId': $(that).parent().attr('data-id'), 'isUse': $(that).parent().attr('data-use')
        };

        addLikeCount(params, function (data) {
            if (data.success == 'true') {
                //点赞成功，删除当前按钮点赞事件，数量+1，改变颜色
                //下面逻辑顺序不可变，先取消事件，获取数量，改变颜色，赋值新数量
                //取消事件
                $(that).unbind();
                //点赞数量+1
                var likeCount = 0;
                if ($(that).find('label').length > 0) {
                    likeCount = parseInt($(that).find('label').text());
                }
                //改变颜色
                $(that).empty().append('<i>&#xe9d4;</i>');
                $(that).removeClass('nolike');

                likeCount += 1;
                $(that).append('<label>' + likeCount + '</label>');
                if ($(that).hasClass('count') == false) {
                    $(that).addClass('count');
                }
            } else {
                $.toast(data.msg);
            }
        });
    });
}
//获取评价列表 并对点赞数据做处理 然后执行回调
function loadReviewData(params, success, error) {
    getProductReview(params, function (data) {
        if (data.success) {
            //计算当前是否有点赞
            var currentLikeId = getReviewLikeList();
            $.each(data.data, function (i) {
                if (currentLikeId) {
                    data.data[i].Liked = $.inArray(data.data[i].ReviewId + '', currentLikeId) > -1;
                }
                else {
                    data.data[i].Liked = false;
                }
            });
            if (success) success(data);
        }
        else {
            if (error) error(data);
        }
    });
}
//获取已经点赞id
function getReviewLikeList() {
    var arr, reg = new RegExp('(^| )SalesV2_ReviewLike=([^;]*)(;|$)');
    arr = document.cookie.match(reg);
    if (arr) {
        return arr[2].replace(/,$/, '').split(',');
    }
    else {
        return null;
    }
}
//返回顶部事件绑定 page:当前页面容器 bottom:网页默认距离底部距离  appBottom：app距离底部距离
//调用实例：(function () {goTopEvent(page,'0.2rem','10px'); })();
//同时在对应的页面header后面添加html文件
/*<!--返回顶部-->
<section class="back-Top">
    <a class="print_top"><i>&#xe657;</i></a>
</section>*/
function goTopEvent(page, bottom, appBottom) {
    //滚动事件监听返回顶部
    $(page).find(".content").on('scroll', function () {
        //动画效果 - 返回顶部
        goTopAnimate();
        //显示任务足迹和返回顶部
        if (txapp.isApp()) {
            bottom = appBottom;
        }
        //根据机型动态设置高度
        $(page).find('.back-Top').css('bottom', bottom);

        //监听滚动条 显示返回顶部按钮 
        if ($(this).scrollTop() > 50) {
            $(page).find('.print_top').animate({
                'opacity': 1,
                'height': '2rem'
            }, 400);
        } else {
            $(page).find('.print_top').animate({
                'opacity': 0,
                'height': 0
            }, 400);
        }
    });
}
//预加载图片
function loadImage(url, callback) {
    var img = new Image(); //创建一个Image对象，实现图片的预下载
    img.src = url;

    if (img.complete) { // 如果图片已经存在于浏览器缓存，直接调用回调函数
        callback.call(img);
        return; // 直接返回，不用再处理onload事件
    }
    img.onload = function () { //图片下载完毕时异步调用callback函数。
        callback.call(img);//将回调函数的this替换为Image对象
    };
};
//返回顶部-动画效果
function goTopAnimate() {
    var goTopTimer = null;
    $('.print_top').off().on('click', function () {
        clearInterval(goTopTimer);
        goTopTimer = setInterval(function () {
            var now = $('.content').scrollTop();
            var speed = (0 - now) / 3;
            speed = speed > 0 ? Math.ceil(speed) : Math.floor(speed);
            if (now == 0) {
                clearInterval(goTopTimer);
            }
            $('.content').scrollTop(now + speed);
        }, 1);
    });
}
//页面参数集合  
//警告警告：：：：：：：：：：：：这里的参数不要随便删，有的页面逻辑会用
var parameters = {
    quan: {
        //个人资讯
        userNews: {
            posts_id: 0,
            bbslist: {
                bbs_type: 'usernews', only_id: 0, pageIndex: 0, pageSize: 6
            }
        }
    },
    member: {
        //通用选择地区
        selectArea: {
            back: null, callback: null, state: 0, level: 1, name1: '', name2: '', name3: ''
        },
        //通用选择地图
        selectMap: {
            getLngLat: null
        },
        //添加收货地址
        addressAdd: {
            form: [{
                'name': 'take_name', 'token': 'TakeName', 'text': '收货人', 'placeholder': '如：张三丰'
            },
            {
                'name': 'phone', 'token': 'Phone', 'text': '手机号', 'placeholder': '如：13030130301'
            },
            {
                'name': 'area', 'token': 'Area', 'text': '所在地区', 'placeholder': '请选择'
            },
            {
                'name': 'address', 'token': 'Address', 'text': '详细地址', 'placeholder': '如：幸福小区9号楼5单元201室'
            },
            {
                'name': 'lng-lat', 'token': 'Lng-Lat', 'text': '地理位置', 'placeholder': '非外卖门店地址可空'
            }]
        },
        //绑定手机页面
        bind: {
            shareCode: ''
        }
    },
    order: {
        //订单列表
        list: {
            pageIndex: 0, pageSize: 10
        },
        unReview: {
            pageIndex: 0, pageSize: 10
        },
        refund: {
            pageIndex: 0, pageSize: 10
        }
    },
    memberCountInfo: {
        lastUpdateTime: 0, data: null
    }
    , coordinate: {
        'lat': 0, 'lng': 0
    }  //用户当前坐标 
    , title: ['买客', '商品分类', '店铺分类', '商品搜索', '店铺搜索', '商品-', '店铺-', '全部店铺']
    , index: {
        likeProductParams: {
            pageIndex: 1, pageSize: 16
        }
        , rankIndex: 0 //赚钱排行索引
        , rankTimer: null   //赚钱排行timer
        , isFirstLoad: true
        , adSwiper: null
        , rankSwiper: {
            htmlContent: ''
            , swiperEvent: null
        }
        , templateSwiperArray: []
        , recommendClassData: null
    }
    , product: {
        isFirstLoad: true
        , TotalCount: 0
        , keyParams: {
            'type': 2, 'top': 20
        }  //关键词记录
        , searchParams: {
            'type': 2, 'page_index': 1, 'page_size': 22, 'order': 0, 'is_desc': 1, 'key': '', 'class_ids': '', 'max': 0, 'min': 0
        }  //搜索
        , delParams: {
            'type': 2, 'key': '', 'separator': ','
        }
    }
    , shop: {
        isFirstLoad: true
        , keyParams: {
            'type': 1, 'top': 20
        }  //关键词记录
        , searchParams: {
            'key': '', 'type': 1, 'selectorType': 1, 'pageIndex': 1, 'pageSize': 15
        }  //搜索
        , delParams: {
            'type': 1, 'key': '', 'separator': ','
        }
        , TotalCount: 0
        , isFirst: true,
        details: {
            loading: false,//防止重复加载详情
            shareDesc: '',//分享说明
            list: {
            },//商品数据集合
            mchParams: {
                pageIndex: 0, pageSize: 3, mch_id: 0, product_id: 0
            }//加载店铺信息参数集合
        },
        mchnews: {
            bbslist: {
                bbs_type: 'mchnews', only_id: 0, pageIndex: 0, pageSize: 6
            },
            recommend: {
                mch_id: 0, news_id: 0
            },
            info: {
                news_id: 0, news_title: '', news_cover: ''
            },
            mchParams: {
                loading: false, pageIndex: 0, pageSize: 10, mch_id: 0
            },
            reward: {
                onlyId: 0, pageIndex: 0, pageSize: 10
            },
            author: {
            }
        },
        mchstore: {
            product: {
            },//商品列表参数
            news: {
                pageIndex: 0, pageSize: 10, mch_id: 0
            }
        }
    }
    , allShop: {
        isFirstLoad: true
        , searchParams: {
            'key': '', 'type': 1, 'selectorType': 1, 'pageIndex': 1, 'pageSize': 15
        }  //搜索
        , getMchParams: {
            'selectorType': 1, 'pageIndex': 1, 'pageSize': 15
        }  //搜索
        , TotalCount: 0
    }
    , class: {
        isFirstLoad: true
    }
    //评论
    , bbs: {
        //资讯评论
        index: {
            //用户参与的
            user: {
                only_id: 0, bbs_type: 'mchnews'
            },
            //全部
            all: {
                pageIndex: 0, pageSize: 10, only_id: 0, bbs_type: 'mchnews'
            }
        },
        //资讯评论详情
        details: {
            //楼主获得点赞数量计算游客
            like_count: 0,
            //全部
            all: {
                pageIndex: 0, pageSize: 10, bbs_id: 0
            },
            //点赞
            like: {
                pageIndex: 0, pageSize: 10, bbs_id: 0,
            }
        }
    }
    , review: {
        title: ['商品评价', '评价详情']
        , details: ''
        , product: null
        , parentId: 0
        , isUse: 0
        , isFirstLoad: true
        , productId: 0
        , isFirstLoadDetails: true
    }
    , activity: {
        signIn: {
            baseParams: { activity_sn: 'sign_in_1_first_shop' },
            title: ['活动说明', '签到', '签到历史'],
            setting: null,
            activityData: null
        },
        rebateList: {
            isFirstLoad: true
        },
        rebateSummary: {
            isFirstLoad: true
        }
    }
    /// 搜索
    /// 参数：
    ///     key:关键词 | 必须
    ///     type:搜索类型 | 可空，1店铺搜索，2商品搜索，默认为2
    ///     selectorType:排序规则，
    ///                  如果是店铺 | 1综合排序，2销量，3人气 全部为降序
    ///                  如果是商品 | 1综合降序，2销量降序，3新品降序，4价格降序，5价格升序
    ///     pageIndex:分页页码 | 可空，从1开始，默认为1，表示第一页
    ///     pageSize:分页尺寸 | 可空，默认为20
};

//计算两个经纬度的距离
//调用北京-南京：Distance.getFlatternDistance(39.92,116.46,32.04,118.78)
var Distance = Distance || {
};
Distance = (function () {
    var EARTH_RADIUS = 6378137.0;    //单位M
    var PI = Math.PI;

    var getRad = function (d) {
        return d * PI / 180.0;
    };

    /**
     * 计算大圆距离，近似距离
     * @param {Object} lat1
     * @param {Object} lng1
     * @param {Object} lat2
     * @param {Object} lng2
     */
    Distance.getGreatCircleDistance = function (lat1, lng1, lat2, lng2) {
        var radLat1 = getRad(lat1 * 1);
        var radLat2 = getRad(lat2 * 1);

        var a = radLat1 - radLat2;
        var b = getRad(lng1 * 1) - getRad(lng2 * 1);

        var s = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(a / 2), 2) + Math.cos(radLat1) * Math.cos(radLat2) * Math.pow(Math.sin(b / 2), 2)));
        s = s * EARTH_RADIUS;
        s = Math.round(s * 10000) / 10000.0;

        return s;
    };

    /**
     * 近似椭球上两点之间的距离(类地球) 更精确
     * @param {Object} lat1
     * @param {Object} lng1
     * @param {Object} lat2
     * @param {Object} lng2
     */
    Distance.getFlatternDistance = function (lat1, lng1, lat2, lng2) {
        var f = getRad((lat1 * 1 + lat2 * 1) / 2);
        var g = getRad((lat1 * 1 - lat2 * 1) / 2);
        var l = getRad((lng1 * 1 - lng2 * 1) / 2);

        var sg = Math.sin(g);
        var sl = Math.sin(l);
        var sf = Math.sin(f);

        var s, c, w, r, d, h1, h2;
        var a = EARTH_RADIUS;
        var fl = 1 / 298.257;

        sg = sg * sg;
        sl = sl * sl;
        sf = sf * sf;

        s = sg * (1 - sl) + (1 - sf) * sl;
        c = (1 - sg) * (1 - sl) + sf * sl;

        w = Math.atan(Math.sqrt(s / c));
        r = Math.sqrt(s * c) / w;
        d = 2 * w * a;
        h1 = (3 * r - 1) / 2 / c;
        h2 = (3 * r + 1) / 2 / s;

        return d * (1 + fl * (h1 * sf * (1 - sg) - h2 * (1 - sf) * sg));
    };
    //将其定义方法以接口方式返回给外界引用
    return {
        getGreatCircleDistance: Distance.getGreatCircleDistance,
        getFlatternDistance: Distance.getFlatternDistance
    };

})();

//个人消息数据
function selectMemberIndexCountV2(fun) {
    $.ajax({
        type: 'get',
        url: txapp.appHostPath + '/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/SelectMemberIndexCountV2',
        async: false,
        success: function (data) {
            if (fun) fun(eval("(" + data + ")"));
        }
    });
};

//搜索记录 需登陆  同步获取数据
function getSearchKey(params, fun) {
    $.ajax({
        type: 'get',
        url: txapp.appHostPath + '/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/GetSearchKey',
        data: params,
        async: false,
        success: function (data) {
            if (fun) fun(eval(data));
        }
    });
};
//搜索
function search(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/Search', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//删除搜索记录
function deleteSearchRecord(params, fun) {
    $.post(txapp.appHostPath + '/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/DeleteSearchRecord', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//全部店铺
function getAllMch(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/GetAllMch', params, function (data) {
        if (fun) fun(data);
    });
};
//首页轮播广告
function getIndexAdInfos(fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/IndexShowMchAjax.ajax/GetIndexAdInfos', function (data) {
        if (fun) fun(data);
    });
};
//获取门店其他商品
function loadMchPro(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchStoreProductList2', params, function (data) {
        if (fun) fun(data);
    });
};
//获取门店其他商品
function loadMchPro1(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjaxV2.ajax/GetMchProductList', params, function (data) {
        if (fun) fun(data);
    });
};
//获取门店其他商品
function loadTopProductReview(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetTopProductReview', params, function (data) {
        fun(data);
    });
};
//获得店铺相关信息
function getMchInfoByUser(mchId, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchInfoByUser', {
        mch_id: mchId
    }, function (data) {
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
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetStoreNewsInfo', {
        news_id: newsId
    }, function (data) {
        if (fun) fun(data);
    });
};
//获得资讯详情信息
function getStoreNewsPreviewInfo(newsId, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/getStoreNewsPreviewInfo', {
        news_id: newsId
    }, function (data) {
        if (fun) fun(data);
    });
};
//获得店铺自定义分类
function getMchClass(mchId, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchProductClassList', {
        mch_id: mchId
    }, function (data) {
        if (fun) fun(data);
    });
};
//获得店铺售卖中的产品
function getMchStoreProductList2(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchStoreProductList2', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得店铺售卖中的产品
function getMchStoreProductList3(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjaxV2.ajax/GetMchStoreProductList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得店铺资讯列表
function getMchNewsList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchNewsList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//关注店铺
function setMchBrowse(mch_id, is_collect, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/MchBrowse', {
        mch_id: mch_id, is_collect: is_collect
    }, function (data) {
        if (fun) fun(eval(data));
    });
};
//店铺留言
function setStoreLiuyan(params, fun) {
    $.post(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetStoreLiuyan', params, function (data) {
        if (fun) fun(eval('(' + data + ')'));
    });
};
//获得店铺推荐资讯列表
function getRecommendMchNews(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/RecommendMchNews', params, function (data) {
        if (fun) fun(data);
    });
};
//店铺资讯点赞
function setStoreNewsLike(params, fun) {
    if (window.localStorage["news_click_like_" + params.news_id] == 1) {
        $.toast('您已点过赞了，不能重复点赞');
        return;
    }
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetStoreNewsLike', params, function (data) {
        window.localStorage["news_click_like_" + params.news_id] = 1;
        if (fun) fun(data);
    });
};
//获得BBS评论列表
function getBBSList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得BBS我参与的评论列表
function getUserBBSList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetUserBBSList', params, function (data) {
        if (fun) fun(data);
    });
};
//BBS评论点赞
function setUserBBSLike(params, fun) {
    if (window.localStorage["bbs_click_like_" + params.bbs_id] == 1) {
        $.toast('您已点过赞了，不能重复点赞');
        return;
    }
    $.get(txapp.appHostPath + '/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/SetUserBBSLike', params, function (data) {
        var obj = eval('(' + data + ')');
        if (obj.success == true || obj.success == 'true') {
            window.localStorage["bbs_click_like_" + params.bbs_id] = 1;
        }
        fun(obj);
    });
};
//获得BBS评论楼主详情
function getBBSDetails(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSDetails', params, function (data) {
        if (fun) fun(data);
    });
};
//获得BBS评论详情回复列表
function getBBSDetailsList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSDetailsList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得BBS评论点赞列表
function getBBSLikeList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/BBS/Ajax/BBSAjax.ajax/GetBBSLikeList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得打赏列表
function getRewardInfo(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/News/Ajax/RewardAjax.ajax/GetRewardInfo', params, function (data) {
        if (fun) fun(params, data);
    });
};
//查询打算是否打开
function rewardIsOpen(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/News/Ajax/RewardAjax.ajax/IsOpen', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//查询用户信息
function getUserInfo(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Member/Ajax/UserAjax.ajax/GetUserInfo', params, function (data) {
        if (fun) fun(JSON.parse(data));
    });
};
//收藏、取消收藏商品
function addOrCancelCollectProduct(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddOrCancelCollectProduct', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//添加商品足迹
function addOrUpdateUserBrowseFootprint(productId, mchClass) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddOrUpdateUserBrowseFootprint', { product_id: productId, mch_class: mchClass }, function (data) { });
};
//获得商品推广招商品牌信息
function getProductBrand(product_id, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProdctBrand', {
        product_id: product_id
    }, function (data) {
        if (fun) fun(data);
    });
};
//设置店铺品牌推广记录
function setStoreBrandPop(params, fun) {
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetBrandPop", params, function (data) {
        if (fun) fun(data);
    });
};

//计算评论的好评率并返回商品基本信息
function getProdectReviewRate(params, fun) {
    $.getJSON(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProdectReviewRateV4", params, function (data) {
        if (fun) fun(data);
    });
};
//获取评论列表
function getProductReview(params, fun) {
    $.getJSON(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProductReviewV4", params, function (data) {
        if (fun) fun(data);
    });
};
//点赞
function addLikeCount(params, fun) {
    //先判断登录，未登录跳转登录页
    if (userIsAuth() == false) {
        return publicConfigLogin();
    }
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV2.ajax/AddLikeCountV4", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//回复评价
function replyReview(params, fun) {
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV2.ajax/ReplyReviewV2", params, function (data) {
        if (fun) fun(eval('(' + data + ')'));
    });
};
//首页商品流分类推荐
function getRecommendClass(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Home/Ajax/IndexAjax.ajax/GetRecommendClass', params, function (data) {
        if (fun) fun(data);
    });
};
//获得订单支付金额
function getOrderPayMoney(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/GetOrderPayMoney', params, function (data) {
        if (fun) fun(data);
    });
};
//账户余额支付订单
function accountPayOrderByWap(params, fun) {
    $.post(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/AccountPayOrderByWap', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获得全部订单列表
function getOrderList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/OrderAjaxV3.ajax/GetOrderList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得待评价订单列表
function getOrderUnReviewList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/OrderAjaxV3.ajax/GetOrderUnReviewList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得退货订单列表
function getOrderRefundList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/OrderAjaxV3.ajax/GetOrderRefundList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//确认下单页获得购买商品列表
function getProductByHandOrder(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/GetProductByHandOrder', params, function (data) {
        if (fun) fun(data);
    });
};
//确认下单页获得收货地址
function getDefaultAddress(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Member/Ajax/AddressAjaxV3.ajax/GetDefaultAddress', params, function (data) {
        if (fun) fun(data);
    });
};
//获得收货地址列表
function getSalesAddress(fun) {
    $.getJSON(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/GetSalesAddress", function (data) {
        if (fun) fun(data);
    });
};
//设置收货地址默认
function editAddressIsMain(params, fun) {
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/EditAddressIsMain", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//删除收货地址
function deleteAddress(params, fun) {
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/DelteAddress", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获取用户个人资讯信息
function getUserNewsInfo(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Quan/Ajax/UserNewsAjax.ajax/OpenGetUserNewsInfo', params, function (data) {
        if (fun) fun(data[0]);
    });
};//获取用户个人资讯信息预览
function getUserNewsInfoPreview(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Quan/Ajax/UserNewsAjax.ajax/OpenGetUserNewsInfoPreview', params, function (data) {
        if (fun) fun(data[0]);
    });
};
//店铺资讯点赞
function setUserNewsLike(params, fun) {
    if (window.localStorage["user_news_click_like_" + params.posts_id] == 1) {
        $.toast('您已点过赞了，不能重复点赞');
        return;
    }
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Quan/Ajax/UserNewsAjax.ajax/OpenSetUserNewsLike', params, function (data) {
        window.localStorage["user_news_click_like_" + params.posts_id] = 1;
        if (fun) fun(eval(data));
    });
};
//获得一条地址
function getAddress(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/GetAddress', params, function (data) {
        if (fun) fun(data);
    });
};
//保存收货地址
function subAddressForm(params, fun) {
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/EditAddress", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获取省市区级联地址
function getArea(params, fun) {
    $.getJSON('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/GetArea', params, function (data) {
        if (fun) fun(data);
    });
};
//获取购物车列表
function getShopCartList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/GetShopCartList', params, function (data) {
        if (fun) fun(data);
    });
};
//修改购物车数量	
function updateCartShopCount(fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/UpdateCartShopCount', function (data) {
        if (fun) fun(eval(data));
    });
};
//获取签到活动配置信息
function getSignInSetting(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/GetSignInSetting', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//判断是否有资格 如果有资格 同步拉取个人签到信息
function initSignIn(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/InitSignIn', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//签到
function setSignIn(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/SetSignIn', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//获取签到记录
function getSignInDetailsList(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/GetSignInDetailsList', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//开启关闭签到提醒
function changeRemindState(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/ChangeRemindState', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//判断能否转出签到资产
function canTransferSignInAssets(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/CanTransferSignInAssets', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//转出签到冻结资产到可用资产
function transferSignInAssets(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/TransferSignInAssets', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//判断是否通知签到（开启通知 并且 今日未签到）
function checkSignInNoifity(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/SigninAjax.ajax/CheckSignInNoifity', params, function (data) {
        if (fun) fun(eval(data));
    });
};

//获得活动列表
function getRebateProductList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/RebateAjax.ajax/GetRebateProductList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//获得基本信息
function initRebateEligible(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/RebateAjax.ajax/InitRebateEligible', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获得推荐购买人列表
function getRecommendUserList(params, fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/RebateAjax.ajax/GetRecommendUserList', params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获得活动推送消息列表
function getRebateOrderMsgList(params, fun) {
    $.getJSON(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/RebateAjax.ajax/GetRebateOrderMsgList', params, function (data) {
        if (fun) fun(params, data);
    });
};
//判断是否参加高额返利活动
function isJoinRevateActivity(fun) {
    $.get(txapp.appHostPath + '/Txooo/SalesV2/Activity/Ajax/RebateAjax.ajax/IsJoinRevateActivity', function (data) {
        if (fun) fun(eval(data));
    });
};

//修改购物车数量
function updateCartShopCount(params, fun) {
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/UpdateCartShopCount", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//删除购物车商品
function delShopCart(ids, fun) {
    var params = { 'shop_ids': ids.join(',') };
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/DelShopCart", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//获得规格数据-参数商品id
function getPropertyDb(id, fun) {
    var params = { 'product_id': id };
    $.get(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/GetPropertyDb", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//修改购物车规格属性
function updateCartMapId(params, fun) {
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/UpdateCartMapId", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//加入购物车
function addShopCart(params, fun) {
    $.post(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/AddShopCart", params, function (data) {
        if (fun) fun(eval(data));
    });
};
//新版首页商家广告
function getIndexMchAdList(params, fun) {
    $.getJSON(txapp.appHostPath + "/Txooo/SalesV2/Shop/Ajax/ShopAjaxV4.ajax/GetIndexMchAdList", params, function (data) {
        if (fun) fun(data);
    });
};



//倒计时[输入时间大于当前时间]（时间, 所需展示的标签选择器, true/false（倒计时/正计时,默认true）,正计时触发回调的最大毫秒,回调）
function CountDownTime(times, target, sequence, maxMillisecond, fun) {
    var endtime = new Date(times);
    var cdTimer = setInterval(function () {
        var nowtime = new Date();
        var lefttime
        if (sequence == false) {
            lefttime = parseInt(nowtime.getTime() - endtime.getTime());//获取毫秒 
        } else {
            lefttime = parseInt(endtime.getTime() - nowtime.getTime());
        }
        // 获取剩下的日、小时、分钟、秒钟  
        // 一天有多少毫秒，一小时有多少毫秒，一分钟有多少毫秒，一秒钟有多少毫秒  
        var dm = 24 * 60 * 60 * 1000;
        var d = parseInt(lefttime / dm);
        var hm = 60 * 60 * 1000;
        var h = parseInt((lefttime / hm) % 24);
        var mm = 60 * 1000;
        var m = parseInt((lefttime / mm) % 60);
        var s = parseInt((lefttime / 1000) % 60);
        h = checktime(h);
        m = checktime(m);
        s = checktime(s);
        if (lefttime > 0) {
            if (d > 0) {
                target.html(timeSpan(d) + "天 " + timeSpan(h) + ":" + timeSpan(m) + ":" + timeSpan(s));
            } else {
                target.html(timeSpan(h) + ":" + timeSpan(m) + ":" + timeSpan(s));
            }
        }
        if (sequence) {
            //倒计时
            if (lefttime <= 0 && fun) {
                clearInterval(cdTimer);
                fun();
            }
        }
        else {
            //正计时
            if (maxMillisecond && maxMillisecond <= lefttime && fun) {
                clearInterval(cdTimer);
                fun();
            }
        }
    }, 1000);


    function checktime(i) {
        if (i < 10) {
            i = "0" + i;
        }
        else { i = i; }
        return i;
    }
    function timeSpan(num) {
        var arrNum = num.toString().split("");
        var returned = ""
        for (var i in arrNum) {
            returned += "<span>" + arrNum[i] + "</span>"
        }
        return returned;
    }
}