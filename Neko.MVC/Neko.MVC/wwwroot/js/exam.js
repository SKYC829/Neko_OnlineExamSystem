var examCountdown;
$(document).ready(function () {
    examCountdown = window.setInterval(function () {
        var examMinute = $('label[name="countDown"]').html();
        var a = examMinute.split(':');
        var minute = a[0];
        var sec = a[1];
        if (sec <= 0) {
            sec = 60;
            minute -= 1;
        }
        if (sec > 0) {
            sec -= 1;
        }
        $('label[name="countDown"]').html(minute + ":" + sec);
        if (minute <= 0 && sec <= 0) {
            clearInterval(cd);
            autoSub();
        }
    }, 1000);
});
$('.finish').click(function () {
    sub(false);
    return false;
});
function valInput() {
    var result = false;
    $('.qdiv').each(function (qi, qitem) {
        var aw = $(qitem).find('input:checked');
        if (aw.length <= 0) {
            result = false;
            return false;
        } else {
            result = true;
        }
    });
    return result;
}
$('.text-danger').click(function () {
    if (!confirm("是否要放弃作答离开考试?")) {
        return false;
    }
});
function sub(skipConfirm) {
    if (!skipConfirm) {
        if (!confirm("是否要交卷离开?")) {
            return false;
        }
        if (!valInput()) {
            if (!confirm("当前有题目尚未填写，是否继续交卷?")) {
                return false;
            }
        }
    }
    var url = $('form').attr('action');
    var method = $('form').attr('method');
    var examId = $($('form').find('div[name="formDiv"]')).data('id');
    var root = [];
    var data = [];
    $('.qdiv').each(function (qi, qitem) {
        var aw = $(qitem).find('input:checked');
        var key = $(qitem).data('qid');
        var subData = [];
        for (var i = 0; i < aw.length; i++) {
            var ck = $(aw)[i];
            if (ck.checked) {
                subData.push($(ck).data("sid"));
            }
        }
        data.push({ "questionId": key, "solutionId": subData });
    });
    root.push({ "examId": examId, "leftTime": $('label[name="countDown"]').html(), "details": data });
    clearInterval(examCountdown);
    var cd;
    $.ajax({
        url: url,
        type: method,
        contentType: 'application/json',
        data: JSON.stringify(root),
        beforeSend: function () {
            $('#loadingModal').modal('show');
            var len = 0;
            cd = window.setInterval(function () {
                if (len < 100) {
                    len += 10;
                } else if (len >= 100) {
                    len = 0;
                }
                $('#loading').css('width', len + '%');
            }, 1000);
        },
        success: function (data) {
            clearInterval(cd);
            finalModal(data.examScore, data.examPaper.examScore, data.isPassed);
            return false;
        },
        error: function (data) {
            alert("err:" + data.responseText);
            return false;
        }
    });
};
function autoSub() {
    $('#autoSubModal').modal('show');
    var cd = window.setInterval(function () {
        var a = $('label[name="autoSubCountDown"]').html();
        a -= 1;
        $('label[name="autoSubCountDown"]').html(a);
        if (a <= 0) {
            clearInterval(cd);
        }
    }, 1000);
    window.setTimeout(function () {
        sub(true);
        $('#autoSubModal').modal('hide');
    }, 5000);
};

function finalModal(score, totalScore, isPassed) {
    $('#loadingModal').modal('hide');
    $('label[name="score"]').html(score);
    var passedLine = (score / totalScore).toFixed(2);
    var slogan;
    var passedStr;
    if (passedLine > 0.6 || score == totalScore) {
        $('label[name="score"]').removeClass('text-warning');
        $('label[name="score"]').css('color', 'green');
        slogan = '考得不错，下次继续努力！';
        if (score == totalScore) {
            slogan = '<strong>真棒，这是个满分</strong>';
        }
    } else {
        slogan = '还要加油，下次还有机会！';
    }
    if (isPassed) {
        passedStr = '考试已通过！';
        $('label[name="ispassed"]').css('color', 'green');
    } else {
        passedStr = '差一点就及格了！继续加油！';
        $('label[name="ispassed"]').addClass('text-warning');
    }
    $('label[name="slogan"]').html(slogan);
    $('label[name="ispassed"]').html(passedStr);
    $('#finishSubModal').modal('show');
}