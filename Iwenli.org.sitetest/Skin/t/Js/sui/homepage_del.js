﻿
$(function() {
    'use strict';

    //修改个人资料
    publicPageInit('#page-homepage-info', function(e, id, page) {

        let _this = page;

        //返回不刷新页面
        if (parameters.info.isFirstLoad) {
            parameters.info.isFirstLoad = false;
            init();
        }

        //初始化页面
        function init() {
            initPageFrame();
            loadPageDate();
            //获取违禁词
            loadProhibitWord();
        };

        //初始化页面框架
        function initPageFrame() {
            let html = template('t-item-block', parameters.info.initBaseData);
            $(_this).find('.list-block').eq(0).after(html);
            let html1 = template('t-tag-block', parameters.info.initTagData);
            $(_this).find('.j-edu').before(html1);
        }
        //拉取数据填充页面
        function loadPageDate() {
            $.showPreloader();
            //拉取数据填充页面
            initEditPageData(function(obj) {
                if (obj.success && obj.data.length > 0) {
                    //处理基本数据
                    if (obj.data[0].baseInfo.length > 0) {
                        bindBaseData(obj.data[0].baseInfo[0], bindPageEvent);
                    }

                    //处理标签数据
                    if (obj.data[0].tagList.length > 0) {
                        //1.遍历分类标签
                        obj.data[0].tagList.forEach(o => parameters.info.selectTagData[o.tag_type - 1].tagList
                            += o.tag_name + ',');
                        //2.移除最后一个,
                        parameters.info.selectTagData.forEach(o => {
                            if (o.tagList.length > 0) { o.tagList = o.tagList.substring(0, o.tagList.length - 1); }
                        });
                        //3.填充用户标签
                        fillUserTag(parameters.info.selectTagData);
                    }

                    //填充完数据显示页面
                    $.hidePreloader();
                    $('.j-content').show();
                }
            });
        }

        //绑定基本数据到页面
        function bindBaseData(data, event) {
            //板块1
            {
                //头像
                if (data.head_pic.length > 0) {
                    callBankEdit(1, data.head_pic);
                }
                //昵称
                if (data.nick_name != null) {
                    callBankEdit(2, data.nick_name);
                }
            }
            //板块2 板块3
            {
                //性别
                if (data.sex != null) {
                    callBankEdit(3, parameters.info.sex[data.sex]);
                }
                //现居地
                if (data.user_address != null) {
                    callBankEdit(4, data.user_address);
                }
                //个性签名
                if (data.user_word != null) {
                    callBankEdit(5, data.user_word);
                }
                //手机
                if (data.show_phone != null) {
                    callBankEdit(6, data.show_phone);
                }
            }
            //板块4
            {
                //行业
                if (data.industry_name != null) {
                    callBankEdit(7, data.industry_name);
                }
                //工作领域
                if (data.work_field != null) {
                    callBankEdit(8, data.work_field);
                    parameters.info.workField = data.work_field;
                    //window.sessionStorage['workField'] = data.work_field;
                }

                //公司名称
                if (data.company_name != null) {
                    callBankEdit(9, data.company_name);
                }
                //职位身份
                if (data.job_status != null) {
                    callBankEdit(10, data.job_status);
                }
                //公司地址
                if (data.company_address != null) {
                    callBankEdit(11, data.company_address + data.company_address_detail);
                    parameters.info.address = data.company_address + '##' + data.company_address_detail;
                    //window.sessionStorage['address'] = data.company_address + '##' + data.company_address_detail;
                }
            }
            //板块5
            {
                //出生日期
                if (data.birth_date != null) {
                    callBankEdit(12, dateFormat(data.birth_date, "yyyy/MM/dd"));
                }
                //故乡
                if (data.home_address != null) {
                    callBankEdit(13, data.home_address);
                }
                //出生地
                if (data.birthplace_address != null) {
                    callBankEdit(14, data.birthplace_address);
                }
                //名片公开
                if (data.card_status != null) {
                    callBankEdit(15, parameters.info.cardStatus[data.card_status]);
                }
            }
            //板块6
            {
                //存储当前值 起始日期##结束日期##学校##专业 
                //window.sessionStorage['edu'] =
                parameters.info.edu = dateFormat(data.edu_start_time, "yyyy/MM/dd") +
                    '##' + dateFormat(data.edu_end_time, "yyyy/MM/dd") +
                    '##' + data.edu_school_name + '##' + data.edu_specialty_name;
                //学校
                if (data.edu_school_name != null) {
                    callBankEdit(16, data.edu_school_name);
                }
                //日期
                if (data.edu_start_time != null) {
                    callBankEdit(17, dateFormat(data.edu_start_time, "yyyy/MM/dd") + ' - ' +
                        dateFormat(data.edu_end_time, "yyyy/MM/dd"));
                }
                //专业
                if (data.edu_specialty_name != null) {
                    callBankEdit(18, data.edu_specialty_name);
                }
                //我的需求
                if (data.my_needs != null) {
                    callBankEdit(19, data.my_needs);
                }
                //我能做到
                if (data.i_can != null) {
                    callBankEdit(20, data.i_can);
                }
            }

            //回调
            if (event) event();
        }

        //调用用户标签模板填充页面
        function fillUserTag(date) {
            //解析tag数据，遍历到页面
            date.forEach((o, i) => {
                if (o.tagList.length > 0) {
                    var tag = o.tagList.split(',');
                    let html = template('t-tag-li', { list: tag });
                    $('.j-tag').eq(i).empty().append(html);
                }
            });
        }

        //绑定页面事件
        function bindPageEvent() {
            let _now = new Date(),
                _y = _now.getFullYear(),
                _m = _now.getMonth(),
                _d = _now.getDate();

            //修改性别
            $(_this).find('.j-input').eq(1).unbind().bind('click', editSex);
            //出生日期  最晚可以选择今天 最早100年前的今天
            $(_this).find('.j-input').eq(12).calendar({
                value: [$(_this).find('.j-input').eq(12)[0].value],
                dateFormat: 'yyyy/mm/dd',
                minDate: dateFormat(new Date(_y - 100, _m, _d), 'yyyy/MM/dd'),
                maxDate: dateFormat(new Date(), 'yyyy/MM/dd'),
                onClose: p => updateCardInfo(6, dateFormat(new Date(p.value[0]), 'yyyy/MM/dd'))
            });
            //修改名片公开状态
            $(_this).find('.j-input').eq(15).unbind().bind('click', editCradStatus);
            //修改工作领域
            $(_this).find('.j-edu').unbind().bind('click', function() {
                $.router.load('#page-homepage-info-edu');
            });

            //修改单项详细
            let _idArr = [0, 3, 7, 9, 10, 3, 4],
                _titleArr = ['昵称', '个性签名', '行业', '公司名称', '职位身份', '我的需求', '我能做到'];
            //循环遍历绑定事件
            _idArr.forEach((o, i) => {
                let _obj = {}
                _obj.id = i;
                _obj.key = _idArr[i];
                _obj.title = _titleArr[i];

                let _dom = null;
                if (i <= 4) {
                    _dom = $(_this).find('.j-input');
                }
                else {  //需求和我能做到
                    _dom = $(_this).find('.j-text');
                }

                $(_dom).eq(_obj.key).unbind().bind('click', function() {
                    if (i <= 4) {
                        _obj.value = $(this).val();
                    } else {
                        _obj.value = $(this).text();
                    }
                    editOneInfo(_obj);
                });

            });

            //修改头像
            //绑定不同浏览器处理函数，可多重绑定
            (function(window, $) {
                var user_agent = navigator.userAgent.toLowerCase();
                var wxAry = [];
                window.opensource = {};
                window.opensource.wx = {
                    bind: function(cb) {
                        wxAry.push(cb);
                    }
                };
                window.opensource.init = function() {
                    if (user_agent.indexOf("micromessenger") > -1) {
                        $.each(wxAry, function(i, v) {
                            v();
                        });
                        return;
                    }
                }
            })(window, $);
            //微信下可修改
            window.opensource.wx.bind(function() {
                wxShare.wxImgCount = 1;
                wxShare.wxImgSrc = function(src) {
                    $('.j-head-img').attr('src', wxShare.wxLocalIds[0]);
                };
                wxShare.wxUpImgAjax = function(src) {
                    //上传图片
                    uploadHeadImg({ pics: src }, function(obj) {
                        if (obj.success == "true") {
                            $.toast('上传成功！');
                        } else {
                            $.toast('上传失败！');
                        }
                    });
                };
                $('.j-head-img').parent().parent().click(function() {
                    wxShare.wxSetImg();
                });
            });
            window.opensource.init();
        }

        //名片状态修改
        function editCradStatus() {
            var buttonsStatusSelect = [
                {
                    text: '请选择',
                    label: true
                },
                {
                    text: parameters.info.cardStatus[0],
                    onClick: function() {
                        updateCardInfo(7, 0);
                    }
                },
                {
                    text: parameters.info.cardStatus[1],
                    onClick: function() {
                        updateCardInfo(7, 1);
                    }
                },
                {
                    text: parameters.info.cardStatus[2],
                    onClick: function() {
                        updateCardInfo(7, 2);
                    }
                }
            ];
            var buttonsStatusCancel = [
                {
                    text: '取消',
                    bg: 'danger'
                }
            ];
            var groups = [buttonsStatusSelect, buttonsStatusCancel];
            $.actions(groups);
        }

        //名片状态修改-交互
        function editSex() {
            var buttonsStatusSelect = [
                {
                    text: '请选择',
                    label: true
                },
                {
                    text: parameters.info.sex[1],
                    onClick: function() {
                        updateBaseInfo(2, 1);
                    }
                },
                {
                    text: parameters.info.sex[0],
                    onClick: function() {
                        updateBaseInfo(2, 0);
                    }
                }
            ];
            var buttonsStatusCancel = [
                {
                    text: '取消',
                    bg: 'danger'
                }
            ];
            var groups = [buttonsStatusSelect, buttonsStatusCancel];
            $.actions(groups);
        }

        //名片信息修改-提交数据-修改页面值
        function updateCardInfo(id, data) {
            let param = { 'colnmuId': id, 'colnmuData': data };
            //更新信息并提示
            updateUserCard(param, function(obj) {
                if (obj.success == "true") {
                    //回调修改页面并提示用户
                    switch (id) {
                        case 0://行业
                            callBankEdit(7, data);
                            $.router.back();
                            break;
                        //case 1://工作领域
                        //    callBankEdit(8, data);
                        //    break;
                        case 2://公司名称
                            callBankEdit(9, data);
                            $.router.back();
                            break;
                        case 3://职位身份
                            callBankEdit(10, data);
                            $.router.back();
                            break;
                        case 4://故乡
                            callBankEdit(13, data);
                            break;
                        case 5://出生地
                            callBankEdit(14, data);
                            break;
                        case 6://出生日期
                            callBankEdit(12, dateFormat(data, "yyyy/MM/dd"));
                            break;
                        case 7://名片状态
                            callBankEdit(15, parameters.info.cardStatus[data]);
                            break;
                        case 8://我的需求
                            callBankEdit(19, data);
                            $.router.back();
                            break;
                        case 9://我能做到
                            callBankEdit(20, data);
                            $.router.back();
                            break;
                    }
                    $.toast('操作成功');
                }
                else {
                    $.toast('操作失败');
                }
            });
        }

        //基本修改-提交数据-修改页面值
        function updateBaseInfo(id, data) {
            let param = { 'colnmuId': id, 'colnmuData': data };
            //更新信息并提示
            $.showPreloader();
            updateUserBase(param, function(obj) {
                if (obj.success == "true") {
                    //回调修改页面并提示用户
                    $.hidePreloader();
                    switch (id) {
                        case 0://头像-头像url
                            callBankEdit(1, data);
                            break;
                        case 1://昵称
                            callBankEdit(2, data);
                            $.router.back();
                            break;
                        case 2://性别 传0或1，1：男   0：女
                            callBankEdit(3, parameters.info.sex[data]);
                            break;
                        case 3://现居地 | 格式：code#value，例如310101#上海市市辖区黄浦区
                            callBankEdit(4, data.split('#')[1]);
                            break;
                        case 4://个性签名
                            callBankEdit(5, data);
                            $.router.back();
                            break;
                    }
                    $.toast('操作成功');
                }
                else {
                    $.toast('操作失败');
                }
            });
        }

        //设置违禁词
        function loadProhibitWord() {
            getProhibitWordList(function(obj) {
                if (obj.length > 0) {
                    let prohibitWord = '';
                    obj.forEach(o => prohibitWord = prohibitWord + o.prohibit_word + '|');
                    prohibitWord = prohibitWord.substr(0, prohibitWord.length - 1);
                    parameters.info.prohibitWordReg = '/' + prohibitWord + '/g';
                }
            });
        }

        //修改单项信息事件
        function editOneInfo(obj) {
            let _oneDom = $('#page-homepage-info-one');
            //给标题赋值
            $(_oneDom).find('h1').text(obj.title);
            //水印赋值
            $(_oneDom).find('.j-input').attr('placeholder', '请设置' + obj.title)
            //给文本框赋值
            $(_oneDom).find('.j-input').val(obj.value);

            //绑定保存事件
            $(_oneDom).find('.j-save').unbind().bind('click', function() {
                //['昵称', '个性签名', '行业', '公司名称', '职位身份', '我的需求', '我能做到']
                //取值
                let _value = $(_oneDom).find('.j-input').val();
                //判空
                if (obj.id < 2 && _value == '') {
                    $.toast(obj.title + '不能为空');
                    return;
                }
                //判断是否修改
                if (_value != obj.value) {
                    switch (obj.id) {
                        case 0:
                            updateBaseInfo(1, _value);
                            break;
                        case 1:
                            updateBaseInfo(4, _value);
                            break;
                        case 2:
                            updateCardInfo(0, _value);
                            break;
                        case 3:
                            updateCardInfo(2, _value);
                            break;
                        case 4:
                            updateCardInfo(3, _value);
                            break;
                        case 5:
                            updateCardInfo(8, _value);
                            break;
                        case 6:
                            updateCardInfo(9, _value);
                            break;
                    }
                }
                //$.router.load('#page-homepage-info');
            });
        }
    });

    //省市区级联 - 1
    publicPageInit('#page-homepage-info-area-1', function(e, id, page) {
        init();
        //初始化页面
        function init() {
            londAreaInfo(1, 1);
        };

        //拉取省市区级联数据
        function londAreaInfo(level, regionId) {
            $.showPreloader();
            let param = { 'regionId': regionId };
            getArea(param, function(obj) {
                if (obj.length > 0) {
                    //绑定当前页面数据
                    bingPageData(level, level == 3 ? 2 : 1, obj);
                    $.hidePreloader();
                    //绑定事件
                    $('.j-area-page').eq(level - 1).unbind('click').bind('click', 'li', function() {
                        //修改页面nav
                        $('.j-area-page').eq(level).find('.bar h1').empty().append($.trim($(this).text()))
                        //保存取值
                        window.sessionStorage['#area_' + level] = $.trim($(this).text());
                        //递归事件
                        if (level == 3) {
                            let regionName = window.sessionStorage["#area_1"] +
                                window.sessionStorage["#area_2"] + window.sessionStorage["#area_3"];
                            //更新并返回
                            updateArea($(this).attr('data-id'), regionName);
                        }
                        else {
                            //递归调用
                            londAreaInfo(level + 1, $(this).attr('data-id'));
                            //跳转路由
                            let pageId = level + 1;
                            $.router.load("#page-homepage-info-area-" + pageId);
                        }
                    });
                }
            });
        }
        //填充数据到页面
        function bingPageData(level, templateId, data) {
            var html = template('t-area-' + templateId, { 'list': data });
            $('.j-area-page .content').eq(level - 1).empty().append(html);
        }
        //地址更新相关
        function updateArea(regionId, regionName) {
            $.showPreloader();
            let param = { 'colnmuId': -1, 'colnmuData': '' };
            switch (parameters.info.areaId) {
                case 0://现居地
                    param.colnmuId = 3;
                    param.colnmuData = regionId + '#' + regionName;
                    //更新信息并提示
                    updateUserBase(param, function(obj) {
                        if (obj.success == "false") {
                            $.toast('操作失败');
                            return;
                        } else {
                            $.toast('操作成功');
                            location.href = 'info.html';
                        }
                        $.hidePreloader();
                    });
                    break;
                case 1://公司地址
                    window.sessionStorage['@address'] = regionId + '##' + regionName;
                    parameters.info.areaId = 11; //更新过的公司地址
                    $.router.load('#page-homepage-info-address');
                    $.hidePreloader();
                    break;
                case 2://故乡
                case 3://出生地
                    param.colnmuId = parameters.info.areaId + 2;
                    param.colnmuData = regionName;
                    //更新信息并提示
                    updateUserCard(param, function(obj) {
                        if (obj.success == "false") {
                            $.toast('操作失败');
                            return;
                        }
                        else {
                            $.toast('操作成功');
                            location.href = 'info.html';
                        }
                        $.hidePreloader();
                    });
                    break;
            }
        }

    });

    //省市区级联 - 2
    publicPageInit('#page-homepage-info-area-2', function(e, id, page) {
        if ($(page).find('li').length == 0) {
            $.router.load("#page-homepage-info-area-1");
        }
    });
    //省市区级联 - 3
    publicPageInit('#page-homepage-info-area-3', function(e, id, page) {
        if ($(page).find('li').length == 0) {
            $.router.load("#page-homepage-info-area-1");
        }
    });

    //标签
    publicPageInit('#page-homepage-info-tag', function(e, id, page) {
        let tagTypeName = ['我喜欢的运动', '音乐', '食物', '电影', '书籍', '旅行', '工作领域'],
            tagType = -1, //1-我喜欢的运动,2-音乐,3-食物,4-电影,5-书籍,6-旅行,7-工作领域
            oldTagNameList = '',
            _this = page,
            _pageIndex = 1,   //分页加载标签页码
            _pageSize = 15,  //分页加载标签页尺寸
            _isLoadFinished = false;  //分页加载标签是否加载完毕

        init();

        //初始化页面
        function init() {
            //清除上一次标签页面提示
            $('#page-homepage-info-tag .content ul li').not('.j-add').remove();
            $('.j-tagLoadToast').remove();
            //获取标识
            tagType = parameters.info.tagType;
            //设置标题
            $(_this).find('header h1').text(tagTypeName[tagType - 1]);

            bindPageEvent();
            //拉取数据填充页面
            londTagInfo(tagType);
        };

        //拉取数据
        function londTagInfo(tagType, callback) {
            $.showPreloader();
            let param = { 'tagType': tagType, 'pageIndex': _pageIndex++, 'pageSize': _pageSize };
            getTagListByTypeId(param, function (obj) {
                if (obj.length > 0) {
                    //转换数据
                    if (tagType == 7) {
                        let arr1 = [], tempObj = null;
                        //将已经选择的数据分离出来
                        obj.forEach(o => {
                            if (parameters.info.workField == o.class_name) {
                                tempObj = o;
                                tempObj.is_select = 1;
                            } else {
                                arr1.push(o);
                            }
                        });
                        //清空obj
                        obj.length = 0
                        //重新填充
                        //1.如果有选择的数据，优先添加
                        if (tempObj != null) {
                            obj.push(tempObj);
                        }
                        //2.如果有自定义选择的数据，次级优先添加
                        else if (parameters.info.workField != null) {
                            let tagData = {
                                "tag_id": 0,
                                "class_name": parameters.info.workField,
                                "is_prohibit": 0,
                                "is_select": 1
                            };
                            obj.push(tagData);
                        }
                        //3.追加未选择的数据
                        obj = obj.concat(arr1);
                    }

                    obj.forEach(o => {
                        o.is_select = o.is_select == 1 ? true : false;
                    });

                    //绑定当前页面数据
                    bingPageData(obj);

                    //绑定选择标签事件
                    $('#page-homepage-info-tag ul').unbind().bind('click', 'li:not(.j-add)', function () {
                        //违禁词检测
                        if ($(this).attr('data-prohibit') == '1') {
                            $.confirm('标签[' + $.trim($(this).text()) + ']含有违禁词，是否删除?', function () {
                                $.alert('删除成功！');
                            });
                            return;
                        }
                        //选择标签
                        if ($(this).find('.icon').hasClass('icon-check')) {
                            $(this).find('.icon').removeClass('icon-check');
                        }
                        else {
                            let count = 8;
                            if (tagType == 7) {
                                count = 1;
                            }
                            if (tagType == 6) {
                                count = 50;   //旅游标签50个
                            }
                            //个数检测
                            if ($('#page-homepage-info-tag ul .icon-check').length > count - 1) {
                                $.alert('最多只能选择' + count + '个标签！');
                                return;
                            }
                            $(this).find('.icon').addClass('icon-check');
                        }
                    });

                    //拉取数据之后记录初始选择状态
                    $('#page-homepage-info-tag .icon-check').each(
                        (i, o) => oldTagNameList = oldTagNameList + $.trim($(o).parent().text()) + '#');
                }
                else {
                    // 删除加载提示符
                    $('.infinite-scroll-preloader,j-tagLoadToast').remove();
                    $(_this).find('.content').append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">没有更多了.</div>');
                }
                //结束loader
                $.hidePreloader();
                if (obj.length == 0 || obj.length < _pageSize) {
                    _isLoadFinished = true;
                }
                if (callback) callback();
            });
        }
        //填充数据到页面
        function bingPageData(data) {
            var html = template('t-tagList', { 'list': data });
            $('#page-homepage-info-tag .content ul').append(html);
        }
        //添加新标签
        function addNewTag(tagType) {
            $.prompt(tagTypeName[tagType - 1] + '标签', function(value) {
                //不能为空
                if ($.trim(value) == '') {
                    $.toast('不能为空!');
                    return;
                }
                //违禁词检测
                if (parameters.info.prohibitWordReg != null &&
                    new RegExp(parameters.info.prohibitWordReg).test(value)) {
                    $.toast('含有违禁词!');
                    return;
                }
                //添加到标签列表
                $.toast('添加成功');
                let _tagData = [{
                    "tag_id": 0,
                    "class_name": value,
                    "is_prohibit": 0,
                    "is_select": 0
                }];
                let _html = template('t-tagList', { 'list': _tagData });
                $('#page-homepage-info-tag .content .j-add').after(_html);
            });
        }
        //更新标签
        function saveTag(tagType) {
            $.showPreloader();

            if (tagType == 7) {   //更新工作领域
                let data = $.trim($('#page-homepage-info-tag .icon-check').parent().text());
                let param = { 'colnmuId': 2, 'colnmuData': data };
                //更新信息并提示
                updateUserCard(param, function(obj) {
                    if (obj.success == "false") {
                        $.toast('操作失败');
                        return;
                    }
                    else {
                        //更新页面显示
                        callBankEdit(8, data)
                        $.toast('操作成功');
                    }
                });
            }
            else {
                //href += '#';
                let tagNameList = '';
                $('#page-homepage-info-tag .icon-check').each((i, o) => tagNameList = tagNameList + $.trim($(o).parent().text()) + '#');
                if (oldTagNameList != tagNameList) {
                    //有修改
                    tagNameList = tagNameList.substr(0, tagNameList.length - 1);
                    let param = { 'tagType': tagType, 'tagNameList': tagNameList };
                    //更新信息并提示
                    updateUserTag(param, function(obj) {
                        $.hidePreloader();
                        if (obj.success == "false") {
                            $.toast('操作失败');
                            return;
                        }
                        else {
                            //更新页面显示
                            updateShowTag(tagType, tagNameList.split('#'));
                            $.toast('操作成功');
                        }
                    });
                }
            }
            $.hidePreloader();
            $.router.back();
        }
        //绑定页面事件相关
        function bindPageEvent() {
            //绑定添加标签事件
            $(_this).find('.j-add-tag').unbind().bind('click', function() {
                addNewTag(tagType);
            });
            //绑定保存标签事件
            $(_this).find('.j-save').unbind().bind('click', function() {
                saveTag(tagType);
            });


            // 加载flag
            var loading = false;
            // 注册下拉加载数据事件
            $(document).unbind('infinite').on('infinite', '.infinite-scroll-bottom', function() {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;

                //拉取数据填充页面
                londTagInfo(tagType, function() {
                    // 重置加载flag
                    loading = false;
                    if (_isLoadFinished) {//加载完成
                        // 加载完毕，则注销无限加载事件，以防不必要的加载
                        $.detachInfiniteScroll($('.infinite-scroll'));
                        // 删除加载提示符
                        $('.infinite-scroll-preloader').remove();
                        $(_this).find('.content').append(' <div class="j-tagLoadToast" style="text-align:center;margin-bottom:0.5rem;color:#797979;">没有更多了.</div>');
                        return;
                    }
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                });

            });
        }
        //更新页面标签显示
        function updateShowTag(tagType, data) {
            let _dom = $('#page-homepage-info .list-block').find('.j-tag').eq(tagType - 1);
            $(_dom).empty();

            if (data.length > 0 && data[0] != '') {
                let html = template('t-tag-li', { 'list': data });
                $(_dom).append(html);
            } else {
                $(_dom).append(parameters.info.initTagData.list[tagType - 1].defualtMsg);
            }
        }
    });

    //修改教育经历
    publicPageInit('#page-homepage-info-edu', function(e, id, page) {

        let _this = page;

        init();

        //初始化页面
        function init() {
            loadData();
            bindPageEvent();
        };

        //拉取数据
        function loadData() {
            if (parameters.info.edu != null) {
                let eduInfo = parameters.info.edu.split('##');
                bindPage(eduInfo);
            }
        }
        //绑定页面
        function bindPage(data) {
            data.forEach((o, i) => {
                if (o != 'undefined' && (o != 'null')) {
                    $(_this).find('.j-input').eq(i).val(o)
                }
            });
        }
        //绑定事件
        function bindPageEvent() {
            let _now = new Date(),
                _y = _now.getFullYear(),
                _m = _now.getMonth(),
                _d = _now.getDate();
            //_startDateValue = $('#page-homepage-info-edu .j-input').eq(0).val() == '' ?
            //    dateFormat(_now, 'yyyy/MM/dd') : $('#page-homepage-info-edu .j-input').eq(0).val(),
            //_endDateValue = $('#page-homepage-info-edu .j-input').eq(1).val() == '' ?
            //    dateFormat(_now, 'yyyy/MM/dd') : $('#page-homepage-info-edu .j-input').eq(1).val();

            //起始日期  最晚可以选择今天 最早100年前的今天
            let _startTimestamp = Date.parse($(_this).find('.j-input').eq(0).val());
            if ($(_this).find('.j-input').eq(0).val() == '') { //如果起始日期为空，必须设置起始日期之后才能设置结束日期
                $(_this).find('.j-input').eq(0).calendar({
                    dateFormat: 'yyyy/mm/dd',
                    minDate: dateFormat(new Date(_y - 100, _m, _d), 'yyyy/MM/dd'),
                    maxDate: dateFormat(new Date(), 'yyyy/MM/dd'),
                    onChange: function(p, values, displayValues) {
                        //记录起始时间时间戳
                        _startTimestamp = values;
                        //选择完起始日期才行生效
                        //结束日期  最晚可以选择今天 最早100年前的今天
                        $(_this).find('.j-input').eq(1).unbind().calendar({
                            dateFormat: 'yyyy/mm/dd',
                            minDate: displayValues[0],
                            maxDate: dateFormat(new Date(), 'yyyy/MM/dd'),
                            onChange: function(p, values, displayValues) {
                                if (_startTimestamp > values) {
                                    $.toast('结束日期不能小于开始日期！');
                                }
                            }
                        });
                    }
                });
            } else { //否则可以同时选择
                $(_this).find('.j-input').eq(0).calendar({
                    value: [$(_this).find('.j-input').eq(0)[0].value],
                    dateFormat: 'yyyy/mm/dd',
                    minDate: dateFormat(new Date(_y - 100, _m, _d), 'yyyy/MM/dd'),
                    maxDate: dateFormat(new Date(), 'yyyy/MM/dd'),
                    onChange: function(p, values, displayValues) {
                        //记录起始时间时间戳
                        _startTimestamp = values;
                    }
                });
                //结束日期  最晚可以选择今天 最早100年前的今天
                $(_this).find('.j-input').eq(1).unbind().calendar({
                    value: [$(_this).find('.j-input').eq(1)[0].value],
                    dateFormat: 'yyyy/mm/dd',
                    minDate: dateFormat(new Date(_y - 100, _m, _d), 'yyyy/MM/dd'),
                    maxDate: dateFormat(new Date(), 'yyyy/MM/dd'),
                    onChange: function(p, values, displayValues) {
                        if (_startTimestamp > values) {
                            $.toast('结束日期不能小于开始日期！');
                        }
                    }
                });
            }

            //绑定保存标签事件
            $(_this).find('.j-save').unbind().bind('click', function() {
                saveEdu();
            });

        }
        //保存数据
        function saveEdu() {
            //如果数据没有调整不调取接口
            let _edu = dateFormat($(_this).find('.j-input').eq(0).val(), 'yyyy/MM/dd') +
                '##' + dateFormat($(_this).find('.j-input').eq(1).val(), "yyyy/MM/dd") +
                '##' + $(_this).find('.j-input').eq(2).val() +
                '##' + $(_this).find('.j-input').eq(3).val();
            //如果有修改则提交数据
            if (parameters.info.edu != _edu) {
                let isnull = false,
                    param = {
                        'school': null,
                        'specialty': null,
                        'startTime': null,
                        'endTime': null
                    },
                    msgArr = ['起始日期不能为空！', '结束日期不能为空！', '学校不能为空！', '专业不能为空！'];
                //非空检测
                $(_this).find('.j-input').each((i, o) => {
                    if (isNullOrEmpty($(o).val(), msgArr[i])) {
                        isnull = true;
                        return false; //跳出循环
                    }
                });

                if (isnull) return;  //存在空值返回

                //赋值
                param.school = $(_this).find('.j-input').eq(2).val();
                param.specialty = $(_this).find('.j-input').eq(3).val();
                param.startTime = dateFormat($(_this).find('.j-input').eq(0).val(), 'yyyy-MM-dd');
                param.endTime = dateFormat($(_this).find('.j-input').eq(1).val(), 'yyyy-MM-dd');

                //判断日期符合规则
                if (Date.parse(param.endTime) < Date.parse(param.startTime)) {
                    $.toast('结束日期不能小于开始日期！');
                    return;
                }

                //更新信息并提示
                $.showPreloader();
                updateUserEducation(param, function(obj) {
                    $.hidePreloader();
                    if (obj.success == "false") {
                        $.toast('操作失败');
                        return;
                    }
                    else {
                        $.toast('操作成功');
                        parameters.info.edu = _edu;  //更新固化数据
                        //更新首页
                        callBankEdit(16, param.school);
                        callBankEdit(17, param.startTime + '-' + param.endTime);
                        callBankEdit(18, param.specialty);
                    }
                    $.router.back();
                });
            }
            else {
                $.toast('操作成功');
                $.router.back();
            }
        }

        //判断是否为空并提示
        function isNullOrEmpty(data, msg) {
            if (data == null || data == undefined || data == 'null' || data == 'undefined' || data == '') {
                $.toast(msg);
                return true;
            }
            return false;
        }
    });

    //修改公司地址
    publicPageInit('#page-homepage-info-address', function(e, id, page) {
        let _this = page;

        init();

        //初始化页面
        function init() {
            //页面来源
            loadData();
            bindPageEvent();
        };

        //拉取数据
        function loadData() {
            //首页数据
            if (parameters.info.areaId = 1 && parameters.info.address != null) {
                let adddressInfo = parameters.info.address.split('##');
                parameters.info.address = null;
                bindPage(adddressInfo);
            }
            //选择地区数据
            if (parameters.info.areaId = 11 && window.sessionStorage['@address'] != null) {
                let _adddressInfo = window.sessionStorage['@address'].split('##')[1];
                window.sessionStorage.removeItem("@address");
                bindPage(_adddressInfo);
            }
        }
        //绑定页面
        function bindPage(data) {
            if (typeof (data) === 'string') {
                $(_this).find('.j-input').eq(0).val(data);
            }
            else {
                data.forEach((o, i) => {
                    if (o != 'undefined' && (o != 'null')) {
                        $(_this).find('.j-input').eq(i).val(o);
                    }
                });
            }
        }
        //绑定事件
        function bindPageEvent() {
            //绑定选择地区时间
            $(_this).find('.j-input').eq(0).unbind().bind('click', function() {
                parameters.info.areaId = 1;
                $.router.load('#page-homepage-info-area-1');
                //location.href = '?id=1#page-homepage-info-area-1';
            });

            //绑定保存标签事件
            $(_this).find('.j-save').unbind().bind('click', function() {
                saveAddress();
            });
        }
        //保存数据
        function saveAddress() {
            //如果数据没有调整不调取接口
            let _address = $(_this).find('.j-input').eq(0).val() + '##' + $(_this).find('.j-input').eq(1).val();

            if (parameters.info.address != _address) {
                //如果地区为空 而 详细地址不为空 则提示
                if ($(_this).find('.j-input').eq(0).val() == '' && $(_this).find('.j-input').eq(1).val() != '') {
                    $.toast('所在地区不能为空');
                    return;
                }

                let param = {
                    'address': '',
                    'addressDetail': ''
                };

                //赋值
                param.address = $(_this).find('.j-input').eq(0).val();
                param.addressDetail = $(_this).find('.j-input').eq(1).val();

                //更新信息并提示
                $.showPreloader();
                updateUserCompanyAddress(param, function(obj) {
                    if (obj.success == "false") {
                        $.toast('操作失败');
                        return;
                    } else {
                        $.toast('操作成功');
                        //更新首页数据
                        callBankEdit(11, param.address + param.addressDetail);
                        //更新固话数据
                        parameters.info.address = _address;
                    }
                    $.hidePreloader();
                });

            }
            $.router.load('#page-homepage-info');
        }
    });

    //主页
    publicPageInit('#page-homepage-index', function(e, id, page) {
        let _this = page;

        init();

        //初始化页面
        function init() {
            //url格式 url?uid=12345
            let user = location.hostname.split('.')[0];
            let uid = location.search.substr(5);
            let isMySelf = true, isShow = false;
            //如果未登录
            if (user == '0' && uid == '') {     //未登录 未传值
                isMySelf = false;
            }
            else if (user == '0' && uid != '') {   //未登录 已传值
                isMySelf = false;
                isShow = true;
            }
            else if (user != '0' && uid == '') {   //已登录 未传值
                isShow = true;
                uid = user.substr(1);
            }
            else if (user != '0' && uid != '') {   //已登录 已传值
                if (user != '1' + uid) {  //传入的uid不等于登录userId
                    isMySelf = false;
                }
                isShow = true;
            }

            if (isShow) {
                //初始化页面结构
                initPageFrame(isMySelf);
                //拉取数据更新页面
                loadData(uid);
            }
        };
        //初始化页面框架
        function initPageFrame(isMySelf) {
            let param = isMySelf ? '我' : 'TA',
                param1 = isMySelf ? '您' : 'TA';
            $(_this).find('.j-title').text(param + '的主页');
            $(_this).find('.j-btn-mch').text(param + '的店铺');
            $(_this).find('.j-word').text(param1 + '还没有签名');
            $(_this).find('.j-title-need').text(param + '的需求');
            $(_this).find('.j-title-can').text(param + '能办到');
            $(_this).find('.j-title-tag').text(param + '的兴趣爱好');
            $(_this).find('.j-title-edu').text(param + '的教育经历');
            $(_this).find('.j-title-attendHim').text('关注' + param + '的');
            $(_this).find('.j-attendHim').text('最近关注' + param + '的是');
            $(_this).find('.j-title-hisAttend').text(param + '关注的');
            $(_this).find('.j-hisAttend').text('最近' + param + '关注的是');
            if (isMySelf) {
                //如果是自己 删除关注按钮 发信息按钮  商品模块 资讯模块
                $(_this).find('.j-btn-attend').remove();
                $(_this).find('.j-sendMsg').remove();
                $(_this).find('.j-product').remove();
                $(_this).find('.j-brandNew').remove();
            }
            hide();
        }
        //拉取数据
        function loadData(uid) {
            let param = { 'uid': uid };
            $.showPreloader();
            initHomePageData(param, function(obj) {
                $.hidePreloader();
                if (obj.success.toString() == "true") {
                    //如果成功执行回调绑定数
                    bindPage(obj.data[0], bindPageEvent);
                }
                else {
                    $.toast('加载失败，请刷新重试！');
                }
            });

        }
        //绑定页面
        function bindPage(data, event) {
            if (!isNullOrEmpty(data.BaseInfo[0].head_pic)) {  //头像
                $(_this).find('.j-headImg')[0].src = data.BaseInfo[0].head_pic;
            }
            if (!isNullOrEmpty(data.BaseInfo[0].nick_name)) {  //昵称
                $(_this).find('.j-userNick').text(data.BaseInfo[0].nick_name);
            }
            if (data.BaseInfo[0].sex == 1 || data.BaseInfo[0].sex == 0) {  //性别
                $(_this).find('.j-sex').addClass('sex' + data.BaseInfo[0].sex);
            }
            if (data.IsMch == 1) {  //是否商户light
                $(_this).find('.j-ismch').addClass('light');
            }
            if (data.BaseInfo[0].total_assets_rank > 0) {  //赚钱排名
                $(_this).find('.j-rank').text(data.BaseInfo[0].total_assets_rank);
            }
            if (data.IsAttend == 1) {  //关注按钮
                $(_this).find('.j-btn-attend').text('已关注');
            }
            if (data.MchId > 0) {  //他的店铺按钮
                $(_this).find('.j-btn-mch').css('display', 'block');
                $(_this).find('.j-btn-mch')[0].href = "/Shop/MchStore.html?mchId=" + data.MchId;
            }
            else {
                $(_this).find('.j-btn-mch').remove();
            }
            if (!isNullOrEmpty(data.BaseInfo[0].user_word)) {  //签名
                $(_this).find('.j-word').text(data.BaseInfo[0].user_word);
            }

            //相册
            if (data.Posts.length == 1) {
                $(_this).find('.j-quan-title').val(data.Posts[0].posts_content);
                let html = '', _imgUrlList = data.Posts[0].posts_imgs.split(',');
                _imgUrlList.length = _imgUrlList.length > 3 ? 3 : _imgUrlList.length;  //最多显示3张
                _imgUrlList.forEach(o => html += '<li class="col-33"><img src="' + o + '"/></li>');
                $(_this).find('.j-quan-row').empty().append(html);
                $(_this).find('.j-quan-date').text(dateFormat(data.Posts[0].add_time, 'yyyy/MM/dd hh:mm'));
                if (data.Posts[0].review_count > 0) {
                    $(_this).find('.j-quan-review').text(data.Posts[0].review_count);
                }
                if (data.Posts[0].good_count > 0) {
                    $(_this).find('.j-quan-good').text(data.Posts[0].good_count);
                }
                $(_this).find('.j-quan-review,.j-quan-good,.j-quan-state-bar').show()
            }

            //商品和资讯
            if (data.IsMch == 0) {  //如果是个人身份不显示
                $(_this).find('.j-product,.j-brandNew').remove();
            }
            else {
                //商品信息
                //处理主图
                if (data.ProductList.length > 0) {
                    data.ProductList.forEach(o => {
                        o.product_imgs = o.product_imgs.split(',')[0];
                        o.price = parseFloat(o.price).toFixed(2).toString();
                    });
                    let html = template('t-product', { 'list': data.ProductList });
                    $(_this).find('.j-product-content').empty().append(html);
                }
                //资讯
                if (data.News.length == 1) {
                    $(_this).find('.j-news-title').text(data.MchClass == 3 ? '品牌咨询' : '店铺咨询');
                    //处理日期
                    data.News[0].add_time = dateFormat(data.News[0].add_time, 'yyyy/MM/dd');
                    let html = template('t-news', { 'list': data.News });
                    $(_this).find('.j-news-content').empty().append(html);
                }
            }
            //需求  为空不显示
            if (!isNullOrEmpty(data.BaseInfo[0].my_needs)) {
                $(_this).find('.j-need-content').text(data.BaseInfo[0].my_needs);
                $(_this).find('.j-need-date').text(dateFormat(data.BaseInfo[0].update_needs_time, 'yyyy/MM/dd hh:mm'));
            }
            else {
                $(_this).find('.j-need').remove();
            }
            //他能做到  为空不显示
            if (!isNullOrEmpty(data.BaseInfo[0].i_can)) {
                $(_this).find('.j-can-content').text(data.BaseInfo[0].i_can);
                $(_this).find('.j-can-date').text(dateFormat(data.BaseInfo[0].update_can_time, 'yyyy/MM/dd hh:mm'));
            }
            else {
                $(_this).find('.j-can').remove();
            }

            if (data.CardStatus == 0) { //卡片状态不可见 个人信息 兴趣爱好 教育经历 删除
                $(_this).find('.j-info,.j-tag,j-edu').remove();
            }
            else {
                //个人信息
                let $infoDom = $(_this).find('.j-info-content input');
                if (!isNullOrEmpty(data.BaseInfo[0].company_name)) {  //公司名称
                    $infoDom.eq(0).val(data.BaseInfo[0].company_name);
                }
                if (!isNullOrEmpty(data.BaseInfo[0].job_status)) {  //职位身份
                    $infoDom.eq(1).val(data.BaseInfo[0].job_status);
                }
                if (!isNullOrEmpty(data.BaseInfo[0].birth_date)) {  //出生日期=>年龄
                    $infoDom.eq(2).val(dateToAge(new Date(data.BaseInfo[0].birth_date)));
                }
                if (!isNullOrEmpty(data.BaseInfo[0].company_address)) {  //公司地址
                    $infoDom.eq(3).val(data.BaseInfo[0].company_address + data.BaseInfo[0].company_address_detail);
                }
                if (!isNullOrEmpty(data.BaseInfo[0].user_address)) {  //现居地
                    $infoDom.eq(4).val(data.BaseInfo[0].user_address);
                }
                if (!isNullOrEmpty(data.BaseInfo[0].birthplace_address)) {  //出生地
                    $infoDom.eq(5).val(data.BaseInfo[0].birthplace_address);
                }
                if (!isNullOrEmpty(data.BaseInfo[0].home_address)) {  //故乡
                    $infoDom.eq(6).val(data.BaseInfo[0].home_address);
                }
                //教育经历
                let $eduDom = $(_this).find('.j-edu-content input');
                if (!isNullOrEmpty(data.BaseInfo[0].edu_school_name)) {  //学校名称
                    $eduDom.eq(0).val(data.BaseInfo[0].edu_school_name);
                    $(_this).find('.j-edu').show();
                }
                if (!isNullOrEmpty(data.BaseInfo[0].edu_start_time)) {  //时间
                    $eduDom.eq(1).val(dateFormat(data.BaseInfo[0].edu_start_time, 'yyyy/MM/dd') + '-' +
                        dateFormat(data.BaseInfo[0].edu_end_time, 'yyyy/MM/dd'));
                }
                if (!isNullOrEmpty(data.BaseInfo[0].edu_specialty_name)) {  //专业
                    $eduDom.eq(2).val(data.BaseInfo[0].edu_specialty_name);
                }

                //标签
                if (data.TagList.length > 0) {
                    let _tagList = [], _map = [];
                    //分组标签
                    data.TagList.forEach(o => {
                        if (!_map[o.tag_type]) {
                            _tagList.push({
                                tagType: o.tag_type,
                                list: [o.tag_name]
                            });
                            _map[o.tag_type] = o;
                        } else {
                            for (var j = 0; j < _tagList.length; j++) {
                                if (_tagList[j].tagType == o.tag_type) {
                                    _tagList[j].list.push(o.tag_name);
                                    break;
                                }
                            }
                        }
                    });
                    //渲染页面
                    let html = template('t-tag', { 'list': _tagList });
                    $(_this).find('.j-tag-content').empty().append(html);
                    $(_this).find('.j-tag').show();
                }
                else {

                    $(_this).find('.j-can').remove();
                }
            }

            //关注
            if (data.AttendHim.Count > 0) {
                $(_this).find('.j-count').eq(0).text(data.AttendHim.Count);
                let html = template('t-attend', data.AttendHim);
                $(_this).find('.j-attendHid-content').empty().append(html);
            }
            if (data.HisAttend.Count > 0) {
                $(_this).find('.j-count').eq(1).text(data.HisAttend.Count);
                let html = template('t-attend', data.HisAttend);
                $(_this).find('.j-hisAttend-content').empty().append(html);
            }
            show();
            $(_this).find('.j-homepage-index').show();

            //绑定页面事件
            if (event) event(data);
        }

        //绑定事件
        function bindPageEvent(data) {
            //相册事件绑定
            $(_this).find('.j-quan-content').unbind().bind('click', function() {
                $.router.load('/Quan/othersPost.html?uId=' + data.BaseInfo[0].user_id);
            });
            //关注事件
            $(_this).find('.j-btn-attend').unbind('click').bind('click', function() {
                let param = { 'uId': data.BaseInfo[0].user_id },
                    $this = $(this);
                updateAttention(param, function(obj) {
                    $.showPreloader();
                    if (obj.success == 'true') {
                        let txt = $this.text();
                        $this.text(txt == '未关注' ? '已关注' : '未关注');
                    }
                    $.hidePreloader();
                });
            });
            //商户图标点击事件 如果不是商户身份才绑定
            if (data.IsMch == 0) {
                $(_this).find('.j-ismch').unbind().bind('click', function() {
                    $.toast(data.IsOpenShop == 1 ? '切换商户身份可查看店铺' : '您还没有开通商户');
                });
            }

            //如果有资讯并且数量大于1 绑定更多
            if (data.MchId > 0 && data.IsMch == 1 && data.News.length == 1 && data.News[0].total_news_count > 0) {
                $(_this).find('.j-news-more').unbind().bind('click', function() {
                    //进入资讯列表
                    parameters.index.MchId = data.MchId;
                    $.router.load('#page-homepage-index-newslist');
                });
            }
        }

        //隐藏项目
        function hide() {
            $(_this).find('.j-btn-mch,.j-quan-review,.j-quan-good,.j-product,.j-brandNew,.j-need,.j-can,.j-tag,.j-edu,.j-quan-state-bar').hide();
        }
        //显示项目
        function show() {
            $(_this).find('.j-product,.j-brandNew,.j-need,.j-can').show();
        }

        //判空
        function isNullOrEmpty(obj) {
            if (obj == null || obj.toString() == 'null' || obj.toString() == '') { return true; }
            return false;
        }
        //日期转年龄 返回周岁
        function dateToAge(date) {
            let age = new Date().getFullYear() - date.getFullYear();
            return age < 0 ? 0 : age;
        }
    });

    //资讯列表
    publicPageInit('#page-homepage-index-newslist', function(e, id, page) {
        let _this = page, hasMore = true, pageindex = 1, pageSize = 7, loading = false;
        init();
        //初始化页面
        function init() {
            let _mchId = parameters.index.MchId;
            _mchId = 100000041;
            load(_mchId);
        }

        //第一次拉取数据
        function load(_mchId) {
            getData(_mchId, pageindex++ - 1, pageSize, true, null);
            //绑定事件
            bindPageEvent(_mchId);
        }

        //拉取数据  分页
        function getData(_mchId, index, size, isEmpty, callback) {
            if (_mchId <= 0) {
                $.toast('没有数据！');
                return;
            }
            if (!hasMore) {
                $.toast('没有更多了！');
                return;
            }
            $.showPreloader();
            let param = { 'mchId': _mchId, 'pageIndex': index, 'pageSize': size };
            getNewsListByMchid(param, function(obj) {
                if (obj.length > 0) {
                    if (obj.length < size) {
                        //没有更多的数据了
                        hasMore = false;
                    }
                    //解析数据，添加到页面
                    obj.forEach(o => o.add_time = dateFormat(o.add_time, 'yyyy/MM/dd'));
                    fullNewsList(isEmpty, obj);
                }
                else {
                    //没有更多了
                    hasMore = false;
                }
                $.hidePreloader();
                if (callback) callback();
            });
        }
        //填充模板
        function fullNewsList(isEmpty, data) {
            let $parent = $(_this).find('.j-newsList');
            if (isEmpty) {
                $parent.empty();
            }
            let html = template('t-newsList', { 'list': data });
            $parent.append(html);
        }
        //绑定页面事件
        function bindPageEvent(mchId) {
            //绑定无限加载
            // 注册'infinite'事件处理函数
            $(document).on('infinite', '.infinite-scroll-bottom', function() {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                getData(mchId, pageindex++ - 1, pageSize, false, function() {
                    // 重置加载flag
                    loading = false;
                    if (!hasMore) {
                        // 加载完毕，则注销无限加载事件，以防不必要的加载
                        $.detachInfiniteScroll($('.infinite-scroll'));
                        // 删除加载提示符
                        $('.infinite-scroll-preloader').remove();
                        return;
                    }
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                });
            });
        }
    });

    // 添加'refresh'监听器  下拉刷新
    let $content = $(document).find('.pull-to-refresh-content').on('refresh', function(e) {
        window.location.reload();
        // 加载完毕需要重置
        $.pullToRefreshDone($content);
    });

    $.init();
});

//页面参数集合
var parameters = {
    info: {
        sex: ['女', '男'],
        cardStatus: ['所有人不可见', ' 仅团队可见', '所有人可见'],
        isFirstLoad: true,
        tagType: -1,
        areaId: -1,//0:现居地 1：公司地址 2：故乡 3：出生地
        initBaseData: {
            list: [
                {
                    item: {
                        cList: [
                            { 'clickFun': '', 'key': '性别', 'value': '未设置' },
                            { 'clickFun': 'pageRoute("#page-homepage-info-area-1","area",0)', 'key': '现居地', 'value': '未设置' },
                            { 'clickFun': 'router("#page-homepage-info-one")', 'key': '个性签名', 'value': '未设置' }
                        ]
                    }
                },
                {
                    item: {
                        cList: [
                            { 'clickFun': 'router("../member/phone.html")', 'key': '手机', 'value': '未设置' },
                            //{ 'clickFun': 'router("../member/bank.html")', 'key': '银行卡管理', 'value': '' },
                            { 'clickFun': 'router("../member/address.html")', 'key': '地址管理', 'value': '' }
                        ]
                    }
                },
                {
                    item: {
                        cList: [
                            { 'clickFun': 'router("#page-homepage-info-one")', 'key': '行业', 'value': '添加行业信息' },
                            { 'clickFun': 'pageRoute("#page-homepage-info-tag","tag",7)', 'key': '工作领域', 'value': '添加工作领域信息' },
                            { 'clickFun': 'router("#page-homepage-info-one")', 'key': '公司名称', 'value': '请添加公司/店铺名称' },
                            { 'clickFun': 'router("#page-homepage-info-one")', 'key': '职位身份', 'value': '请填写职位/身份' },
                            { 'clickFun': 'router("#page-homepage-info-address")', 'key': '公司地址', 'value': '请填写公司地址' }
                        ]
                    }
                },
                {
                    item: {
                        cList: [
                            { 'clickFun': '', 'key': '出生日期', 'value': '请填写出生日期' },
                            { 'clickFun': 'pageRoute("#page-homepage-info-area-1","area",2)', 'key': '故乡', 'value': '请选择故乡' },
                            { 'clickFun': 'pageRoute("#page-homepage-info-area-1","area",3)', 'key': '出生地', 'value': '请选择出生地' },
                            { 'clickFun': '', 'key': '名片公开', 'value': '未设置' }
                        ]
                    }
                }
            ]
        },
        initTagData: {
            list: [
                {
                    'tagType': '1',
                    'clickFun': 'pageRoute("#page-homepage-info-tag","tag",1)',
                    'defualtMsg': '我喜欢的运动'
                },
                {
                    'tagType': '2',
                    'clickFun': 'pageRoute("#page-homepage-info-tag","tag",2)',
                    'defualtMsg': '我喜欢的音乐'
                },
                {
                    'tagType': '3',
                    'clickFun': 'pageRoute("#page-homepage-info-tag","tag",3)',
                    'defualtMsg': '我喜欢的美食'
                },
                {
                    'tagType': '4',
                    'clickFun': 'pageRoute("#page-homepage-info-tag","tag",4)',
                    'defualtMsg': '我喜欢的电影'
                },
                {
                    'tagType': '5',
                    'clickFun': 'pageRoute("#page-homepage-info-tag","tag",5)',
                    'defualtMsg': '我喜欢的书籍'
                },
                {
                    'tagType': '6',
                    'clickFun': 'pageRoute("#page-homepage-info-tag","tag",6)',
                    'defualtMsg': '我的旅行足迹'
                }
            ]
        },
        selectTagData: [
            { tagList: '' }, { tagList: '' }, { tagList: '' }, { tagList: '' }, { tagList: '' }, { tagList: '' }
        ]
    },
    index: {
        'MchId': 0
    }
};

//异步更新页面信息
function callBankEdit(id, data) {
    switch (id) {
        case 1://头像
            $('#page-homepage-info .j-head-img')[0].src = data;
            break;
        case 2: //昵称
        case 3://性别
        case 4://现居地
        case 5://个性签名
        case 6://手机
            $('#page-homepage-info .j-input').eq(id - 2).val(data);
            break;
        case 7://行业
        case 8://工作领域
        case 9://公司名称
        case 10://职位身份
        case 11://公司地址
        case 12://出生日期
        case 13://故乡
        case 14://出生地
        case 15://名片公开
            $('#page-homepage-info .j-input').eq(id).val(data);
            break;
        case 16://学校
        case 17://日期
        case 18://专业
        case 19://我的需求
        case 20://我能做到
            $('#page-homepage-info .j-text').eq(id - 16).empty().append(data);
            break;
    }
}
//添加标签路由 fram|tag or area
function pageRoute(url, from, id) {
    if (from == 'tag') {
        $('#page-homepage-info-tag').find('li').not('.j-add').remove();
        parameters.info.tagType = id;
    }
    else if (from == 'area') {
        parameters.info.areaId = id;
    }
    router(url);
}
function router(url) {
    $.router.load(url);
}

//获取个人资料页面基本信息
function initEditPageData(fun) {
    $.getJSON('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/InitEditPageData', function(data) {
        if (fun) fun(data);
    });
};
//更新指定的用户名片信息
function updateUserCard(params, fun) {
    $.post('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/UpdateUserCard', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//更新指定的用户基本信息
function updateUserBase(params, fun) {
    $.post('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/UpdateUserBase', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//获取省市区级联地址
function getArea(params, fun) {
    $.getJSON('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/GetArea', params, function(data) {
        if (fun) fun(data);
    });
};
//获取指定标签类型的所有可用标签
function getTagListByTypeId(params, fun) {
    $.getJSON('/Txooo/SalesV2/Member/Ajax/TagAjax.ajax/GetTagListByTypeId', params, function (data) {
        if (fun) fun(data);
    });
};
//更新指定类型的用户标签
function updateUserTag(params, fun) {
    $.get('/Txooo/SalesV2/Member/Ajax/TagAjax.ajax/UpdateUserTag', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//获取违禁词列表
function getProhibitWordList(fun) {
    $.getJSON('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/GetProhibitWordList', function(data) {
        if (fun) fun(data);
    });
};
//更新用户教育信息
function updateUserEducation(params, fun) {
    $.post('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/UpdateUserEducation', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//更新公司地址
function updateUserCompanyAddress(params, fun) {
    $.post('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/UpdateUserCompanyAddress', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//上传头像
function uploadHeadImg(params, fun) {
    $.post('/Txooo/SalesV2/Member/Ajax/MemberAjax.ajax/UpdateHeadImg', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//获取首页数据
function initHomePageData(params, fun) {
    $.get('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/InitHomePageData', params, function(data) {
        if (data.indexOf('({"success":') > -1) {
            data = eval(data);
        } else {
            data = JSON.parse(data);
        }
        if (fun) fun(data);
    });
};
//关注
function updateAttention(params, fun) {
    $.get('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/UpdateAttention', params, function(data) {
        if (fun) fun(eval(data));
    });
};
//获取资讯列表
function getNewsListByMchid(params, fun) {
    $.get('/Txooo/SalesV2/Member/Ajax/MemberV2Ajax.ajax/GetNewsListBy', params, function(data) {
        if (fun) fun(JSON.parse(data));
    });
};
