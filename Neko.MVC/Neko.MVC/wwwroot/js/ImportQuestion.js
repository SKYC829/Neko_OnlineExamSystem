$(document).ready(function () {
    InitModal();
});

$('#groupFilter').bind("input propertychange", function () {
    InitModal();
});

$('#nameFilter').bind("input propertychange", function () {
    InitModal();
});

function InitModal() {
    var groupFilter = $('#groupFilter').val();
    var nameFilter = $('#nameFilter').val();
    var url = "/Exam/Questions/GetQuestion?";
    if (groupFilter != '') {
        url = url + 'groupFilter=' + groupFilter+'&'
    }
    if (nameFilter != '') {
        url = url + 'nameFilter=' + nameFilter + '&'
    }
    $.ajax({
        url: url,
        method: "get",
        beforeSend: function () {
            $('#queryTip').removeAttr('hidden');
        },
        success: function (data) {
            var target = $('#questionTableBody');
            target.empty();
            $.each(data, function (index, item) {
                var ck = StringFormat('<input type="checkbox" name="checkbox" class="form-check-inline" data-id="{0}"/>', item.id);
                var head = item.name;
                var type;
                switch (item.questionType) {
                    case 1:
                        type = "多选题";
                        break;
                    case 0:
                    default:
                        type = "单选题";
                        break;
                }
                var score = item.questionScore;
                var row = StringFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td hidden>{4}</td></tr>", ck, head, item.questionGroupName ,type, score);
                target.append(row);
            });
            $('#queryTip').prop('hidden', 'hidden');
        },
        error: function (data) {
            $('#queryTip').attr('hidden');
            alert("数据加载失败，请刷新页面！");
            window.location.reload();
        }
    })
}

$('#ImportQuestion').on('show.bs.modal', function (e) {
    $('#pasted').find('input[name="QuestionIds"]').each(function (id, item) {
        $('#questionTableBody').find('input:checkbox').each(function (sid, sitem) {
            if ($(item).attr('value') == $(sitem).data('id')) {
                $(sitem).prop('checked', $(item).attr('value') == $(sitem).data('id'));
            }
        });
    });
});

$('#btnCancel').click(function () {
    $('#cscount').html('当前题库考题数:' + $('#pasted').children().length + '题');
    $('#ImportQuestion').modal('hide');
});

$('#btnConfirm').click(function () {
    $('#pasted').empty();
    $('#questionTableBody').find('input:checkbox:checked').each(function (id, item) {
        var selectRow = item.parentElement.parentElement;
        var template = $('#copyed').clone(true);
        template.removeAttr('hidden');
        var id = $(item).data('id');
        var ck = template.find('input[name="QuestionIds"]');
        ck.attr('value', id);
        ck.removeClass('input-validation-error');
        template.find("label[id='lbName']").text(selectRow.children[1].innerHTML);
        template.find("label[id='lbScore']").text(selectRow.children[4].innerHTML);
        $('#pasted').append(template);
    });
    $('#cscount').html('当前题库考题数:' + $('#pasted').children().length + '题');
    $('#ImportQuestion').modal('hide');
});

$('#selectAll').click(function () {
    $('input[name="checkbox"]').prop('checked', this.checked);
});

$('.remove').click(function () {
    $(this).closest('tr').remove();
});