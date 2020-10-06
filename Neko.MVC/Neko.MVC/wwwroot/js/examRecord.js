$('.condition').change(function () {
    var url = '/Records';
    var parameter='?';
    $('.condition').each(function (i, item) {
        parameter += (i == 0 ? '' : '&') + $(item).attr('name') + '=' + $(item).val();
    });
    console.log(url + parameter);
    window.location.href = url + parameter;
});