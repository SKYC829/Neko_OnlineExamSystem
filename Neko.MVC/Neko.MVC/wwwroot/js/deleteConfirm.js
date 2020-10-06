$(function () {
    $(".remove").click(function () {
        var deleteId = $(this).data('id');
        var reqUrl = $(this).data('url');
        var fallBackUrl = $(this).data('fallback');
        if (confirm("确定要删除这条数据吗？")) {
            $.ajax({
                type: "post",
                url: reqUrl,
                data: '"'+deleteId+'"',
                contentType: "application/json",
                success: function (data) {
                    window.location.href = fallBackUrl
                },
                error: function (data) {
                    if (data.responseJSON === undefined) {
                        alert('删除失败!' + data.status);
                    } else {
                        alert('删除失败!' + data.responseJSON.detail);
                    }
                }
            });
        }
        return false;
    });
});