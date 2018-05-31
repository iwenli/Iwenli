$(function () {
    'use strict';
    //签到入口
    publicPageInit('#page-signIn-dir', function (e, id, page) {
        var _this = page;
        init();

        function init() {
            document.title = parameters.activity.signIn.title[0];
            loadDataAndBindPage();
            bindEvent();
        }
        function loadDataAndBindPage() {
            loadActivityRuleDate();
            getActivityData(false);
            showActivityDescription();
        }

        function showActivityDescription() {
            $(_this).show();
        }

        function getActivityData(isToast) {
            if (userIsAuth() == true) {
                //获取用户信息,判断用户是否参参与活动
                initSignIn(parameters.activity.signIn.baseParams, function (obj) {
                    if (obj.success == 'true') {
                        parameters.activity.signIn.activityData = obj.data;
                        if (obj.data.SignInState == 1) {
                            showActivityDescription();
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
                location.href = !txapp.isApp() ? '/' : '' + 'index.html';
                //$.router.backLoad(!txapp.isApp() ? '/' : '' + 'index.html');
            });
        }
    });

    //签到页面
    publicPageInit('#page-signIn-index', function (e, id, page) {
        var _this = page;
        var _userActivityData = parameters.activity.signIn.activityData;

        if (parameters.activity.signIn.setting == null || _userActivityData == null) {
            location.href = './signin.html';
        } else {
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
                        daterConfig.minDate = new Date(_userActivityData.InActibityTime);
                        daterConfig.maxDate.setDate(new Date(_userActivityData.InActibityTime).getDate() + _userActivityData.TotalCount);
                        daterConfig.detailsList = obj.data;  //签到记录数据
                        daterConfig.HasBeenMoney = _userActivityData.HasBeenBrokerage + _userActivityData.HasBeenBonus;
                        daterConfig.TotalMissingMoney = _userActivityData.TotalMissingBrokerage + _userActivityData.TotalMissingBonus;

                        //回调
                        //更新页面日历
                        daterConfig.callBack = function () {
                            var _sliceIndex = new Date().getDate() - new Date().getDay();
                            var riliItem = $('.md_datearea li').slice(_sliceIndex - 1, _sliceIndex + 8);
                            riliItem.forEach(function (item) {
                                $('.history_sign ul').append(item.outerHTML);
                            });
                        }
                    }
                    $('.j-history').mdater(daterConfig);
                }
            });
        }

        //显示汇总信息
        function showSummenyInfo(userData) {
            var html = '<li><h2>签到累计获得</h2>'
                + '<p>现金<span>' + userData.HasBeenBrokerage
                + '</span>元&nbsp;&nbsp;&nbsp;&nbsp; 创业金<span>' + userData.HasBeenBonus
                + '</span>元</p></li>'
                + '<li><h2>签到冻结奖励</h2>'
                + '<p>现金<span>' + userData.FreezeBrokerage
                + '</span>元&nbsp;&nbsp;&nbsp;&nbsp; 创业金<span>' + userData.FreezeBonus
                + '</span>元</p></li>'
            $(_this).find('.sign-textinfo ul').empty().append(html);
        }

        //漏签提醒
        function missingSignInToast(userData) {
            var html = '<h2 class="">亲，截止上次签到您共漏签' + userData.MissingCount + '次</h2>'
                + '<p> 错失了</p>'
                + '<p>现金红包<span> ' + userData.MissingBrokerage + '</span>  元</p>'
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
                        HasBeenBrokerage: _userActivityData.HasBeenBrokerage || 0.00,
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
                            location.href = '//www.93390.cn/Index_wap.html';
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
                            location.href = '//www.93390.cn/Index_wap.html';
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
                location.href = !txapp.isApp() ? '/' : '' + 'index.html';
            });
        }
    });
    $.init();
});

var parameters = {
    activity: {
        signIn: {
            baseParams: { activity_sn: 'sign_in_1_first_shop' },
            title: ['活动说明', '签到', '签到历史'],
            setting: null,
            activityData: null
        }
    }
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