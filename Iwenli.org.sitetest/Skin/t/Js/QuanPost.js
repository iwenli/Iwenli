var postId = 0, replyId = 0;

function addComment(me,post_id, reply_id, reply_nick_name) {
    postId = post_id;
    replyId = reply_id;
    if (parseInt(reply_id) != 0) {
        $("#com_content").attr("placeholder", "回复" + reply_nick_name + ":");
    } else {
        $("#com_content").attr("placeholder", "说一句话");
    }
    $("#comment_form").show();
    $("#com_content").focus();
      
    $('#comment_form').bind("click", function (e) {
        var target = $(e.target);
        if (target.closest(".comment_box").length == 0) {
            $("#comment_form").hide();
            
        }
    });
    //setTimeout(function () {
    //    $(me).parents(".huachu").animate({ "width": "0"})
    //}, 200)

    $(me).parents(".huachu").animate({ "width": "0" }).attr("data-flag", "0").find("a").animate({ "width": "0" }, 300);//滑进
}



//评论
function SendComment() {
    var contentInfo = $("#com_content").val();
    if ($.trim(contentInfo).length == 0) {
        return;
    } else if (contentInfo.length > 1000) {
        alert("输入内容过长！");
        return;
    }
    if (isAuth == "false") {
        //dialogAlart("您还没登录，请登录！");
        window.location.href = "//passport.93390.cn/login.html?ReturnUrl=" + encodeURIComponent(window.location.href);
        return;
    }
    $.post("/Txooo/SalesV2/Quan/Ajax/PostAjax.Ajax/AddComment", { content_info: contentInfo, post_id: postId, reply_id: replyId }, function (data) {
        var obj = eval("(" + data + ")");
        if (obj.success == true) {
            var jsonList = eval("(" + obj.info + ")");
            $(".posts_" + postId).html(template("commentTemp", { PostComment: jsonList, PostsId: postId }));
            $("#com_content").val("");
            if (replyId == 0) {//评论时数量+1
                var r_count = $(".review_count_" + postId).html();
                $(".review_count_" + postId).html(parseInt(r_count) + 1);
            }
            $(".posts_" + postId).parent().show();
            $(".all_comment_" + postId).siblings('.reply_con').removeClass('noshow');
            $(".all_comment_" + postId).html('收起查看评论');
            $("#comment_form").hide();
        } else {
            alert("操作失败，请稍后再试！");
        }
    })
}

//点赞
function AddLike(post_id, me) {
    if (isAuth == "false") {
        //dialogAlart("您还没登录，请登录！");
        window.location.href = "//passport.93390.cn/login.html?ReturnUrl=" + encodeURIComponent(window.location.href);
        return;
    }
    $.get("/Txooo/SalesV2/Quan/Ajax/PostAjax.Ajax/AddLike?post_id=" + post_id, function (data) {
        var obj = eval("(" + data + ")");
        if (obj.result != 0) {
            var g_count = $(me).find("span").html();
            if (obj.result == -1) {
                //$(me).find("span").html(parseInt(g_count) - 1);
                $(me).find("span").html("赞");
            } else {
                //$(me).find("span").html(parseInt(g_count) + 1);
                $(me).find("span").html("取消");
            }
            var jsonInfo = eval("(" + obj.info + ")");
            $(".good_" + post_id).html(template("postGoodTemp", { PostGood: jsonInfo }));
            if ($.trim($(".good_" + post_id).html()) == '' && $.trim($(".posts_" + post_id).html()) == '') {
                $(".good_" + post_id).parent().hide();
            } else {
                $(".good_" + post_id).parent().show();
            }
        } else if (obj.result == 0) {
            alert("操作失败，请稍后再试！");
        }
    })
    //setTimeout(function () {
    //    $(me).parents(".huachu").animate({ "width": "0"})
    //}, 200)
    $(me).parents(".huachu").animate({ "width": "0" }).attr("data-flag", "0").find("a").animate({ "width": "0" }, 300);//滑进
}