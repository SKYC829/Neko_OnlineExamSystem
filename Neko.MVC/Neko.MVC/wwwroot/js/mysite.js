function StringFormat() {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}

function GetDateTime() {
    var nowtime = new Date();
    var year = nowtime.getFullYear();
    var month = (nowtime.getMonth() + 1);
    if (month.toString().length == 1) {
        month = '0' + month;
    }
    var day = (nowtime.getDate());
    if (day.toString().length == 1) {
        day = '0' + day;
    }
    var hour = (nowtime.getHours());
    if (hour.toString().length == 1) {
        hour = '0' + hour;
    }
    var minute = (nowtime.getMinutes());
    if (minute.toString().length == 1) {
        minute = '0' + minute;
    }
    var second = (nowtime.getSeconds());
    if (second.toString().length == 1) {
        second = '0' + second;
    }
    var millisecond = nowtime.getMilliseconds(); millisecond = millisecond.toString().length == 1 ? "00" + millisecond : millisecond.toString().length == 2 ? "0" + millisecond : millisecond;
    return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second /*+ "." + millisecond*/;
}
