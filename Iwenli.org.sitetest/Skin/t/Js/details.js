'use strict';

var proInfo = {}; //proPropertyInfo = eval('{$ProPropertyList}');//商品信息
var proPropertyId = 0;//选择的商品属性
var proId = 0;
var postageJson;
var proPropertyInfo;

//去往商铺
function GoToStore(m, b) {
    if (m) {
        $.get("/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/SetBrandPop", { brand_id: b, product_id: proId }, function (data) { });
    }
    else {
        m = proInfo.MchId;
    }

    if (window.txservice.success()) {
        window.txservice.call('store', { mchid: m });
        return;
    }
    window.location.href = '/Shop_2/MchStore.html?mchId=' + m;
}


//获取参数集合
function getUrlArgObject() {
    var args = new Object();
    var query = location.search.substring(1);//获取查询串
    var pairs = query.split("&");//在逗号处断开
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('=');//查找name=value
        if (pos == -1) {//如果没有找到就跳过
            continue;
        }
        var argname = pairs[i].substring(0, pos);//提取name
        var value = pairs[i].substring(pos + 1);//提取value
        args[argname] = unescape(value);//存为属性
    }
    return args;//返回对象
}


$(function () {
    proPropertyInfo = eval($('#propropertyinfo').html());
    window.txservice.init();
    proId = getUrlParam("id");
    //商品详细信息
    $.get("/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProductInfoById?id=" + proId, function (data) {
        if (data != "") {
            proInfo = eval("(" + data + ")");
            proPropertyId = proInfo.MapId;
            var proHtml = template("infoTemp", { info: proInfo });

            $("#infoPro").append(proHtml);
            $("#proBottom").html(template("proBottomTemp", { info: proInfo }));
            //$("#proDetails").html(proInfo.ProductDetails);
            $("#storeInfo .mchComName").html(proInfo.MchComName);
            if (proInfo.Refund == 1) {
                $('.refund_false').hide();
            }
            if (proInfo.ProductOnlyShow == 2 && proInfo.IsVirtual != 1) {
                $('#goBuy').text('本商品只展示，不售卖');
                $("#goBuy").parent().addClass("noclick")
            }
            if (window.txservice.success()) {
                $("#infoPro .share_collect").html($('#infoPro .share_product_name').val() + '<a href="javascript:void(0);" class="toShare appshare"><i>&#xe658;</i>推广</a>');
            } else {
                if (window.sessionStorage.CloseDownLoad != "true") {
                    $('.download').css('display', 'block');
                }
                $('#mch_chat').css('display', 'none');
                $('.operation').addClass('operation_fixed');
                $('.like_product').remove();
            }

            //加载轮播图
            var imgs = proInfo.ProductImgs.split(",");
            for (var i = 0; i < imgs.length; i++) {
                $("#infoPro .swiper-wrapper").append('<div class="swiper-slide" style="background:#f0f0f0 url(' + imgs[i] + ') no-repeat center; background-size:100% auto; "></div>');
            }
            var swiper = new Swiper('.swiper-container', {
                pagination: '.swiper-pagination',
                paginationClickable: true,
                autoplay: 3000,
                loop: true
            });
            //收藏商品

            if (isCollect == "false") {//未登录或未收藏
                $('.pro_collect').attr('onclick', 'AddOrCancelCollectProduct(1)').html("<i class=''>&#xe608;</i>收藏");
            } else {//已登录 已收藏
                $('.pro_collect').attr('onclick', 'AddOrCancelCollectProduct(0)').html("<i class='red'>&#xe608;</i>已收藏");
            }
            $(".appshare").click(function () {
                window.txservice.call('share', {
                    ProductName: $('#infoPro .share_product_name').val(),
                    ShareContent: $('#infoPro .share_content').val(),
                    Img: $('#infoPro .share_product_img').val(),
                    Url: $('#infoPro .share_product_url').val(),
                    ProductId: $('#infoPro .share_product_id').val()
                });
            });
            $('.like_product').click(function () {
                window.txservice.call('likeproduct', {
                    ProductType: proInfo.ProductType,
                    ProductTypeName: proInfo.ProductTypeName,
                    MchClass: proInfo.MchClass,
                    NewOld: proInfo.NewOld
                });
            })
            loadMchPro();//加载商铺其他商品
            recordProductsClass(proInfo.ProductType);//记录商品分类
            //配送方式
            if (proInfo.IsVirtual == 0) {//虚拟商品 不需要运费
                $("#infoPro .freight").show();
                if (proInfo.ProductIspostage) {
                    $("#infoPro .postage").html("卖家包邮");
                    //var postage = eval(proInfo.ProductPostage);
                    //for (var i = 0; i < postage.length; i++) {
                    //    $("#infoPro .postage").append("<input type='radio' value='" + postage[i].id + "' name='postage' />" + postage[i].express);
                    //}
                    //$("#infoPro .postage input:eq(0)").attr("checked", true);

                } else {
                    $("#infoPro .postage").html("买家承担");
                    $('.postage_show').show();
                }
            }
            if (proInfo.IsShow == 0) {
                $("#goBuy").html("已下架");
                $("#goBuy").parent().addClass("noclick")
            } else {
                //点击立即购买按钮
                $("#goBuy").click(function () {
                    selectProperty();
                })
            }
            if (isAuth == "true") {
                //页面加载添加、编辑足迹
                AddOrUpdateUserBrowseFootprint(proInfo.ProductId, proInfo.MchClass);
            }

        } else {
            window.location.href = "/shop/noPro.html?id=" + proId;
            //var d = dialog({
            //    content: "该商品不存在，去看看其他商品！",
            //    width: document.body.clientWidth * 70 / 100,
            //    okValue: '好的',
            //    ok: function () { window.location.href = "/index.html";}
            //}).showModal();
        }
    })
    GetComment();
    //获取底部的推广语
    //推广品牌信息
    getProductBrand(proId);
});



//记录商品分类
function recordProductsClass(id) {
    $.get("/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetClassByProductType?id=" + id, function (data) {
        var obj = eval(data);
        if (obj.length > 0) {
            obj[0].mch_class = proInfo.MchClass;
            window.txservice.call('recordclass', obj[0]);
        }
    });
}
//获取门店其他商品
function loadMchPro() {
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetMchStoreProductList2', { pageIndex: 0, pageSize: 3, mch_id: proInfo.MchId, product_id: proInfo.ProductId }, function (data) {
        var obj = eval(data);
        if (obj.length > 0) {
            $("#storeInfo .mchProList").html(template("store_temp", { info: obj }));
        }
    })
}
//输入数量
function setProductCount() {
    var num = $("#property .pro_count").val();
    if (Number(num)) {
        num = parseInt(num);
        if (num > proInfo.ProductCount) {
            num = proInfo.ProductCount;
            $("#property .pro_count").val(num)
        }
        proCount = num;
        setTotalMoney();
    }
}
//选择商品规格
function selectProperty() {
    getPropertyInfoByMapId();
    $("#property").html(template("propertyTemp", { info: proInfo }));
    $("#property .pro_count").val(proCount);
    $("#property #count_show").html(proCount);
    loadProProperty();
    setTotalMoney();
    $("#property").show();

    $(".gui_box").animate({ right: '0' }, 500);
    $(".gui_btn").animate({ right: '0' }, 500);
    $(".heji").animate({ right: '0' }, 500);
    $('body').css('overflow', 'hidden')
    if ($('#radio_id_' + proPropertyId).length == 0) {
        //当默认规格库存不足时，加载第一个；
        clickPropertyRadio($('.radio_ture:eq(0)')[0]);
    }
    $('#property').on("click", function (e) {
        var target = $(e.target);
        if (target.closest(".gui_box").length == 0) {
            $(".gui_box").animate({ right: '-80%' }, 500, function () {
                $("#property").hide();
            })
            $(".gui_btn").animate({ right: '-80%' }, 500, function () {
                $("#property").hide();
            })
            $(".heji").animate({ right: '-80%' }, 500, function () {
                $("#property").hide();
            })
            $('body').css('overflow', 'auto')
        }
    });
    if (proInfo.IsVirtual == 0) {//虚拟商品 不需要运费
        $("#infoPro .freight").show();
        if (proInfo.ProductIspostage) {
            $("#infoPro .postage").html("卖家包邮");
            $('.postage_type_show p').text('包邮');
        } else {
            $("#infoPro .postage").html("买家承担");
            $('.postage_show').show();
            if (postageJson.append == "0") $('.postage_show .postage_append_div').hide();
            if (postageJson.limit == "0") $('.postage_show .postage_limit_div').hide();

            $('.postage_show .postage_postage').text(postageJson.postage);
            $('.postage_show .postage_append').text(postageJson.append);
            $('.postage_show .postage_limit').text(postageJson.limit);
        }
    }
}

//设置总价格和总V币
function setTotalMoney() {
    var postage_money = 0;
    if (proInfo.ProductIspostage == false && proInfo.IsVirtual != 1) {
        postageJson = $.parseJSON(proInfo.ProductPostage);
        postage_money = parseInt(postageJson.postage);
        if (postageJson.append != "0")
            postage_money = postage_money + (parseInt($('#property .pro_count').val()) - 1) * parseInt(postageJson.append);
        if (postageJson.limit != "0" && parseInt($('#property .pro_count').val()) >= parseInt(postageJson.limit))
            postage_money = 0;

        //postage_money = parseInt($('#property .pro_count').val()) < parseInt(postageJson.limit) ? parseInt(postageJson.postage) + (parseInt($('#pro_count').val()) - 1) * parseInt(postageJson.append) : 0;
    }
    $('.postage_type_show .postage_money').text('￥' + postage_money);
    var t_Price = proCount * parseFloat(proInfo.ProductPrice);
    $("#property #total_money").html(parseFloat((t_Price).toFixed(2)) + postage_money);
    $("#property .rebete_point").html(proCount * parseInt(proInfo.ProductPoint));
    //var t_pbr=proCount * parseFloat(getProBrokerage());
    //var t_pbo=proCount * parseFloat(getProBonus());
    //$("#property #pbr").html(parseFloat((t_pbr).toFixed(2)));
    //$("#property #pbo").html(parseFloat((t_pbo).toFixed(2)));
}

//加载不同规格的商品信息
function loadProProperty() {
    if (proPropertyInfo.length > 0) {
        var propertyRadio = "";
        var product_count = 0;//总库存数量
        for (var i in proPropertyInfo) {
            product_count += proPropertyInfo[i].remain_inventory;
            if (proPropertyInfo[i].remain_inventory > 0) {
                if (proPropertyInfo[i].map_id == proPropertyId) {
                    propertyRadio += '<span class="label_radio"><em class="radio radio_ture current" id="radio_id_' + proPropertyInfo[i].map_id + '"><input type="radio" checked="checked" name="property" id="pro_' + proPropertyInfo[i].map_id + '" value="' + proPropertyInfo[i].map_id + '" /><label for="pro_' + proPropertyInfo[i].map_id + '">' + proPropertyInfo[i].json_info + '</label></em></span>';
                } else {
                    propertyRadio += '<span class="label_radio"><em class="radio radio_ture" id="radio_id_' + proPropertyInfo[i].map_id + '"><input type="radio" name="property" id="pro_' + proPropertyInfo[i].map_id + '" value="' + proPropertyInfo[i].map_id + '" /><label for="pro_' + proPropertyInfo[i].map_id + '">' + proPropertyInfo[i].json_info + '</label></em></span>';
                }
            } else {
                propertyRadio += '<span class="property_hui" data-id="' + proPropertyInfo[i].map_id + '">' + proPropertyInfo[i].json_info + '</span>';
            }
        }
        $('.propertyCls').html(propertyRadio);
        if (product_count == 0) {
            $('#property .gui_btn').addClass('noclick').html('<a href="JavaScript:void(0);">库存不足</a>');
        }
    }
}

var proCount = 1;
//数量-
function sub() {
    if (proCount == 1) {
        return;
    }
    proCount--;
    $("#property .pro_count").val(proCount);
    $("#property #count_show").html(proCount);
    setTotalMoney();
}

//数量+
function add() {
    if (proCount >= proInfo.ProductCount) {
        return;
    }
    proCount++;
    $("#property .pro_count").val(proCount);
    $("#property #count_show").html(proCount);
    setTotalMoney();
}

//根据map_id不同获取规格数据
function getPropertyInfoByMapId() {
    if (proPropertyId != 0) {
        for (var i in proPropertyInfo) {
            if (proPropertyInfo[i].map_id == proPropertyId) {
                proInfo.ProductPrice = parseFloat(proPropertyInfo[i].price);
                proInfo.Img = proPropertyInfo[i].img;
                proInfo.ProductCount = proPropertyInfo[i].remain_inventory;
                proInfo.ProductPoint = proPropertyInfo[i].rebate_point;
                proInfo.JsonInfo = proPropertyInfo[i].json_info;
                break;
            }
        }
    }
};
//点击规格展示规格
function clickPropertyRadio(a) {
    $(a).addClass('current').find('input[type=radio]').attr('checked', true);
    $(a).parent().siblings().find('.radio').removeClass('current').find('input[type=radio]').attr('checked', false);
    proPropertyId = $(a).find("input[name=property]").val();
    getPropertyInfoByMapId();
    $("#property .proPrice").html(proInfo.ProductPrice);
    $("#property .kuCun").html(proInfo.ProductCount);
    $("#property .pro_img").attr("src", proInfo.Img);
    $("#infoPro .propertyLi").html(proInfo.JsonInfo);
    $("#property #property_show").html(proInfo.JsonInfo);

    if (proCount > proInfo.ProductCount) {
        proCount = proInfo.ProductCount;
        $("#property .pro_count").val(proCount);
        $("#property #count_show").html(proCount);
    } else if (proCount == 0 && proInfo.ProductCount > 0) {
        proCount = 1;
        $("#property .pro_count").val(proCount);
        $("#property #count_show").html(proCount);
    }
    setTotalMoney();
}
//去购买
function ConfirmBuy() {
    if (proPropertyId == 0) {
        return;
    }
    buy();
}
//点击立即购买
function buy() {
    if (proInfo.ProductCount <= 0) {
        dialogAlart("库存不足！");
        return;
    }
    if (proPropertyId == 0) {
        return;
    }
    if (window.txservice.success() && isAuth == "false") {
        window.txservice.call('login');
        return;
    }
    var postageVal = 0;
    var radioPostage = $("#infoPro .postage input");
    if (radioPostage.length > 0) {
        postageVal = $("#infoPro .postage input:checked").val();
    }
    var skipHref = "?id=" + proId + "&count=" + proCount + "&postageVal=" + postageVal + "&proProperty=" + proPropertyId;
    if (window.location.pathname.toLowerCase() == "/shop.html" || window.location.pathname.toLowerCase() == "/shop.htm") {
        window.location.href = "/shop/shareOrder.html" + skipHref;
    } else {
        window.location.href = "/shop/order.html" + skipHref;
    }
}

//查看评论
function GoReview() {
    window.location.href = "/shop/review.html?id=" + proId;
}

//商品评价
function GetComment() {
    $.post("/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProReviewRate", { proId: proId }, function (data) {
        var obj = eval("(" + data + ")");
        $(".comment_top .rate").html(obj.rate + "%");
        //$(".comment_top .tcount").html(obj.total_count);
        $(".comment_top").click(function () {
            window.location.href = "/shop/review.html?id=" + proId;
        })
    })
}
//获取推广语
function GetShareContent() {
    $.get("/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/GetShareContent?sId=" + $("#shareId").val() + "&proid=" + proId, function (data) {
        if (data != "") {
            var obj = eval("(" + data + ")");
            $(".mycomment .info").html(obj.ShareContent);
            $("#shareId").val(obj.ShareId);
        }
    })
}

function EditShare() {
    $("#share_form textarea[name=share_content]").val("");
    $(".edit_share").css("display", "block");
}
function CancelShare() {
    $("#share_form textarea[name=share_content]").val("");
    $(".edit_share").css("display", "none");
}
//提交推广语的编辑
function AddShareContent() {
    var share_content = $("#share_form textarea[name=share_content]").val();
    if ($.trim(share_content).length == 0) {
        dialogAlart("输入不能为空！");
        return;
    }
    if ($.trim(share_content).length > 30) {
        dialogAlart("输入不能超过30个汉字！");
        return;
    }
    $.post("/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddShareContent", { proId: proId, share_content: share_content }, function (data) {
        var obj = eval(data);
        if (obj.success == false) {
            dialogAlart("保存失败，请稍后再试！");
        }
        $(".edit_share").css("display", "none");
        $('.loading_box').show().delay(2000).hide(0);
    })
}
$(function () {
    window.onscroll = function () {
        if ($("#proDetails").text().length == 0 && getScrollTop() > ($('#proBottom').offset().top - getClientHeight() + $('.P_Header').height())) {
            $("#proDetails").html(proInfo.ProductDetails);
            //console.log('加载详情');
        }
        if ($(window).scrollTop() < $('#proDetails').offset().top - $('.P_Header').height()) {
            $('.pro_ware').addClass('current').siblings().removeClass('current');
            //} else if ($(window).scrollTop() > $('#cuServic').offset().top - $('.P_Header').height()) {
            //    $('.pro_service').addClass('current').siblings().removeClass('current');
        } else {
            $('.pro_info').addClass('current').siblings().removeClass('current');
        }
        //  console.log($(window).scrollTop(), $('#cuServic').offset().top, $('#proDetails').offset().top)
    }
    if (window.sessionStorage.CloseDownLoad) {
        $('.download').hide();
    }
    $('.download i').click(function () {
        window.sessionStorage.CloseDownLoad = "true";
        $('.download').hide();
    })
    //点击商品规格
    $("#property .radio_ture").live("click", function () {
        clickPropertyRadio(this);
    });
    //轮播图缩放
    $('.swiper-slide').live('click', function () {
        var _index = $(this).index() - 1;
        _index = _index == proInfo.ProductImgs.split(',').length ? 0 : _index;
        _index = _index == -1 ? proInfo.ProductImgs.split(',').length - 1 : _index;
        console.log($(this).index(), _index);
        window.txservice.call('facai', { imgurls: proInfo.ProductImgs, imgindex: _index });
    });
    //详情图缩放
    $('.details_two').find('img').live('click', function () {
        var _index = 0;
        var imgurls = '';
        var $this = this;
        $('#proDetails img').each(function (i, o) {
            imgurls = imgurls + ',' + $(o).attr('src');
            if ($this == o) { _index = i; }
        });
        console.log(_index);
        window.txservice.call('facai', { imgurls: imgurls, imgindex: _index });
    });
    $('#property').find('.pro_img').live('click', function () {
        window.txservice.call('facai', { imgurls: $(this).attr('src'), imgindex: 0 });
    });
})


//添加足迹
function AddOrUpdateUserBrowseFootprint(productId, mchClass) {
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddOrUpdateUserBrowseFootprint', { product_id: productId, mch_class: mchClass }, function (data) {

    });
}

//收藏、取消收藏
function AddOrCancelCollectProduct(operationType) {
    if (isAuth == "false") {
        if (window.txservice.success()) {
            window.txservice.call('login');
            return;
        }
        window.location.href = '//passport.93390.cn/login.html?ReturnUrl=' + encodeURIComponent(window.location.href);
        return;
    }
    $.get('/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/AddOrCancelCollectProduct?pid=' + proId + '&operationType=' + operationType, function (data) {

        if (operationType == 1) {//添加收藏
            $('.pro_collect').attr('onclick', 'AddOrCancelCollectProduct(0)').html('<i class="red">&#xe608;</i>已收藏');

        } else {//取消
            $('.pro_collect').attr('onclick', 'AddOrCancelCollectProduct(1)').html('<i>&#xe608;</i>收藏');
        }
    });
}
function towScroll(a) {
    $(a).addClass('current').siblings().removeClass('current');
    var id = $(a).attr('data-div');
    $("html,body").animate({ scrollTop: ($('#' + id).offset().top - $('.P_Header').height() + 3) }, 0);
}
function oneScroll() {
    $('.pro_ware').addClass('current').siblings().removeClass('current');
    //$('.header_content').hide();
    //$('.details_one').show();
    $(window).scrollTop(1);
}

//私聊
function mchChat() {
    if (window.txservice.success()) {
        if (isAuth == "false") {
            window.txservice.call('login'); return;
        }
        window.txservice.call('mchChat', { user_id: proInfo.MchUserId, product_id: proInfo.ProductId });
    } else {
        dialogAlart("只能在APP中聊天");
    }
}
//获得商品推广招商品牌信息
function getProductBrand(pid) {
    $.getJSON('/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProdctBrand', { product_id: pid }, function (data) {
        if (data.length > 0) {
            $('.product_brand_list').html(template('product_brand_temp', data[0])).show();
        } else {
            $('.product_brand_list').remove();
        }
    });
};