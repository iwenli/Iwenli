var proId = getUrlParam("id");//商品id
var postageVal = getUrlParam("postageVal");//邮费标识
var proCount = getUrlParam("count");//购买数量

var proPropertyInfo;//规格属性
var addressId = getUrlParam("addressid");
var regionCode = 0, lng = 0, lat = 0;
if (addressId == "") {
    addressId = 0;
}

//获取默认地址
function getDefaultAddr() {
    $.get("/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/GetAddress", { addressId: addressId, mch_id: proInfo.mch_id }, function (data) {
        $("#address").show();
        if (data != "[]") {
            //if (window.txservice) {
            //    window.txservice.call('dialog', { msg: data });//app调试弹框
            //}
            var addr = eval(data)[0];
            var addrHtml = '<ul onclick="selectAddress()">';
            addrHtml += "<li class='address_icon'><i>&#xe655;</i></li><li class='address'>" + addr.Area + addr.Address + "</li>";
            addrHtml += "<li class='name'>" + addr.TakeName + "</li><li class='name'>" + addr.Phone + "</li>";
            addrHtml += "<li class='more'><i>&#xe603;</i></li>";
            addrHtml += "</ul>";
            $("#address").html(addrHtml);
            addressId = addr.AddressId;
            regionCode = addr.RegionCode;
            lng = addr.Lng;
            lat = addr.Lat;
        } else {
            $("#address").html("<div class='no_address' onclick='addSelectAddress()' ><i>&#xe645;</i><p>您还没有添加收货地址</p></div>");
            //addressId = 0;
            //$("#address").load("/shop/temp/addAddress.html");
        }
    })
}
//选择地址
function selectAddress() {
    if (window.txservice.success()) {
        window.txservice.call('addressSelect', { mch_class: proInfo.mch_class }, function (data) {
            if (data != "") {
                addressId = data;
                getDefaultAddr();
            }
        });
    } else {
        var cookieVal = proId + "|" + proCount + "|" + postageVal + "|" + addressId + "|" + proPropertyId;
        cookie("Cookie_SalesProInfo", encodeURIComponent(cookieVal), { path: "/" });
        if (addressId != 0) {
            window.location.href = "address.html";
        } else {
            window.location.href = "/member/addressModify.html?url_flag=1";
        }
    }
}
//添加地址
function addSelectAddress() {
    if (window.txservice.success()) {
        window.txservice.call('addressAdd', { mch_class: proInfo.mch_class }, function (data) {
            if (data != "") {
                addressId = data;
                getDefaultAddr();
            }
        });
    } else {
        var cookieVal = proId + "|" + proCount + "|" + postageVal + "|" + addressId + "|" + proPropertyId;
        cookie("Cookie_SalesProInfo", encodeURIComponent(cookieVal), { path: "/" });
        window.location.href = "/member/addressModify.html?url_flag=1&mch_class=" + proInfo.mch_class;
    }
}
//添加编辑地址
function BtnAddressModify() {
    if ($("#addForm").valid()) {
        $.post("/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/AddAddress?proId=" + proId,
              $("#addForm").serialize(), function (data) {
                  var obj = eval(data);
                  if (obj.success == "true") {
                      window.location.reload();
                  } else {
                      dialogAlart(obj.msg);
                  }
              });
    }
};

function pay_hide() {
    $(".pay_method").hide();
}

var proInfo = {};//商品信息

var totalMoney = 0.0;//商品总价格
var totalPostage = 0.0;//总邮费
var proPropertyId = getUrlParam("proProperty");




//设置金额
function setMoney(postageValue) {
    totalMoney = parseFloat((proCount * parseFloat(proPropertyInfo.price)).toFixed(2));
    var postage_money = 0;
    if (proInfo.product_ispostage == false && proInfo.is_virtual == '0') {//虚拟商品 不需要邮费
        postageJson = $.parseJSON(proInfo.product_postage);
        postage_money = parseFloat(postageJson.postage);
        if (postageJson.append != "0")
            postage_money = postage_money + (proCount - 1) * parseFloat(postageJson.append);
        if (postageJson.limit != "0" && parseInt($('#pro_count').val()) >= parseFloat(postageJson.limit))
            postage_money = 0;
        //postage_money = proCount < parseFloat(postageJson.limit) ? parseFloat(postageJson.postage) + (proCount - 1) * parseFloat(postageJson.append) : 0;
        $('.postage_money').text('￥' + postage_money);

        if (postageJson.append == "0") $('.order_dc_explain .postage_append_div').hide();
        if (postageJson.limit == "0") $('.order_dc_explain .postage_limit_div').hide();

        $('.order_dc_explain .postage_postage').text(postageJson.postage);
        $('.order_dc_explain .postage_append').text(postageJson.append);
        $('.order_dc_explain .postage_limit').text(postageJson.limit);
    } else {
        $('.order_dc_explain').hide();
        $('.order_express div').text("包邮");
    }
    $('.gongji .total_money').text('￥' + totalMoney);
    $("#totalMoney .total_money").html(totalMoney + postage_money + ' 元 ');
    $("#totalMoney .total_point").html(proCount * parseInt(proPropertyInfo.rebate_point));
}

//数量-
function sub() {
    if (proCount == 1) {
        return;
    }
    proCount--;
    $("#pro_count").val(proCount);
    $(".total_count").html(proCount);
    setMoney(postageVal);
}

//数量+
function add() {
    if (proCount >= parseInt(proPropertyInfo.remain_inventory)) {
        return;
    }
    proCount++;
    $("#pro_count").val(proCount);
    $(".total_count").html(proCount);
    setMoney(postageVal);
}


var userAgent = window.navigator.userAgent.toLowerCase();
var orderId = 0;
//提交订单post
function postOrder() {
    //checkPayMoney();
    var property_json = [{ map_id: proPropertyId, shop_count: proCount, buyer_ly: $.trim($("#liuyan").val()) }];
    $.post("/Txooo/SalesV2/Shop/Ajax/ShopAjaxV3.ajax/HandOrder", { address_id: addressId, property_json: JSON.stringify(property_json) }, function (data) {
        var obj = eval('(' + data + ')');
        if (obj.success == false || obj.success == "false") {
            if (obj.errcode == -99) {
                openBindByThridShareCode(function (type) {
                    $(".bind_method_login").show().find('.bind_show iframe').attr('src', '/member_2/bind/iframe.html#page-member-bind-iframe-' + type);
                });
            } else {
                dialogAlart(obj.msg);
            }
        } else {
            //window.location.href = "/shop_2/pay.html?order_id=" + obj.order_id;
            $(".pay_method").show().find('.pay_show iframe').attr('src', '/order_2/pay_iframe.html?r=123&order_id=' + obj.order_id);
        }
    });
};
//绑定手机后执行的代码
window.bindMobildIframe = function (obj) {
    //$('.bind_method_login').hide();
    //postOrder();
    window.location.reload();
};

function ForgetPayWap() {
    window.location.href = "/member/phoneCheck.html?ReturnUrl=" + encodeURIComponent('/shop/payAccount.html?orderid=' + orderId);
}
//输入账号支付密码
function inputPwd() {
    $(".pay_method").hide();
    $(".pay_password").show();
};
//关闭支付密码
function closePay() {
    $(".pay_password").hide();
}
//账号支付
function GoToPay() {
    dialogAlart("请使用新版支付流程");
};

$(function () {
    proPropertyInfo = eval($('#propropertyinfo').html())[0];
    window.txservice.register = function (bridge) {
        bridge.registerHandler("backorder", function (data, responseCallback) {
            if (data != "") {
                if (window.txservice) {
                    window.txservice.call('dialog', { msg: data });//app调试弹框
                }
                addressId = data;
                getDefaultAddr();
            }
        });
    };
    window.txservice.init();
    $('.order_ly input').bind('input propertychange', function () {
        if ($(this).val() !== "") {
            $('.order_ly i').show();
        } else {
            $('.order_ly i').hide();
        }
    })
    $('.order_ly i').click(function () {
        $('.order_ly input').val("")
        $(this).hide();
    })

    $("#pro_count").val(proCount);
    $(".total_count").html(proCount);
    $(".remain_count").html(proPropertyInfo.remain_inventory);

    //商品详细信息
    $.get("/Txooo/SalesV2/Shop/Ajax/ShopOpenAjax.ajax/GetProductInfoById2?id=" + proId, function (data) {
        var obj = eval(data);
        if (obj.length > 0) {
            proInfo = obj[0];
            if (proInfo.is_virtual == 0) {
                getDefaultAddr();
                $("#express").show();
                if (proInfo.product_ispostage == false) {
                    //var postageInfo = eval(proInfo.ProductPostage);
                    //$("#express .postage").html(template("postageTemp", { list: postageInfo }));
                    //$("#express .postage input[name=postage][value=" + postageVal + "]").attr("checked", true).parent().addClass('chosen');

                } else {
                    $("#express .postage").html("<span style='float:right;' >快递 包邮</span>");
                }
            }
            proInfo.ProImg = proPropertyInfo.img;
            proInfo.Price = parseFloat(proPropertyInfo.price);
            proInfo.JsonInfo = proPropertyInfo.json_info;

            $("#infoPro").html(template("infoTemp", { info: proInfo }));
            if (proInfo.refund == 0) {//不支持退货功能 提示
                $("#refundMsg").html("<div class='no_change'><i>&#xe63a;</i>购物提示：您购买的商品属于<span>不可退换</span>类别！");
            }
            //注册提交订单的点击事件
            $("#handOrder").on("click", function () {
                if (proInfo.is_virtual == 0) {//虚拟商品 不需要选择地址
                    if (addressId == 0) {
                        dialogAlart('请选择收货地址！');
                        return;
                    } else if (proInfo.mch_class == 4 && (regionCode == 0 || lng == 0 || lat == 0)) {
                        dialogAlart('选择的地址信息不完整，请完善该地址！');
                        return;
                    }
                }
                if (parseInt(proPropertyInfo.remain_inventory) < proCount) {
                    dialogAlart('库存不足！');
                    //dialogAlart('');
                    //window.location.href = "/index.html";
                    return;
                }
                if ($.trim($("#liuyan").val()).length > 150) {
                    dialogAlart("留言字数过长！");
                    return;
                }
                //if(proInfo.refund==0){//不支持退货功能 提示
                //    $(".shade_box").show();
                //}else{
                postOrder();
                //}
            });

        } else {
            dialogAlart('该商品不存在，去看看其他商品！');
            window.location.href = "/index.html";
            return;
        }
        setMoney(postageVal);
    })

    //输入数量
    $("#pro_count").live("blur", function () {
        var reg = /^\d+$/;
        var inputCount = $("#pro_count").val();
        if ($.trim(inputCount) == "" || !reg.test(inputCount) || inputCount <= 0) {
            $("#pro_count").val(proCount);
            return;
        } else if (parseInt(inputCount) > parseInt(proPropertyInfo.remain_inventory)) {
            $("#pro_count").val(proCount);
            return;
        }
        proCount = inputCount;
        $(".total_count").html(proCount);
        setMoney(postageVal);
    })
    //给单选框加单击事件 //更换配送方式
    $(document).delegate(".radio_box", "click", function () {
        $(this).addClass('chosen').find('input[type=radio]').attr('checked', true);
        $(this).siblings().removeClass('chosen').find('input[type=radio]').attr('checked', false);
        postageVal = $(this).find('input[type=radio]').val();
        setMoney(postageVal);
    });


    $('.pay_method').click(function () {
        $(this).hide();
    });

    $('.bind_method_login').click(function () {
        $(this).hide();
    });
    //setTimeout(function () {
    //    $.get('/Txooo/SalesV2/Shop/Ajax/ShopAjax.ajax/GetUserCanMoney', function (data) {
    //        var obj = eval(data);
    //        if (obj.success == "true") {
    //            totalBalance = parseFloat(obj.msg);
    //            $("#pay .yu_e").html(totalBalance);
    //            checkPayMoney();
    //        }
    //    });
    //}, 200);
});

function closeiframe() {
    $('.pay_method').hide();
};
//var totalBalance = 0;
//检查余额是否充足
//function checkPayMoney() {
//    $(document).off('click', '#accountPay');
//    if (totalMoney > totalBalance) {
//        $("#pay .user_balance").html("余额不足不能选择此支付方式");//.parents("li").addClass("none_pay");
//        $("#pay #accountPay").find(".right").remove();
//    } else {
//        //$("#pay .user_balance").addClass("money");
//        //$("#pay #accountPay").unbind("click").attr("onclick", "inputPwd()");
//        $(document).on('click', '#accountPay', function () {
//            inputPwd();
//        });
//    }
//};