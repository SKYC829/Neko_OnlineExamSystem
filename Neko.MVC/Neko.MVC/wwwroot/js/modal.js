$('#btnConfirm').click(function () {
    var questionType = $('#questionType').val();
    var url = $('.modal').data('url');
    //$.ajax({
    //    url: '/' + url + '?questionType=' + questionType,
    //    type: "get",
    //    success: function (data) {
    //        $('.modal').modal('hide');
    //    },
    //    error: function (data) {
    //        alert(data.responseText);
    //    }
    //})
    //window.location.href = url + '?questionType=' + questionType;
    $(location).attr('href', url + '?questionType=' + questionType);
    $('.modal').modal('hide');
});