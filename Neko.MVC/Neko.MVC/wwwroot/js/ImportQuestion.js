$(document).ready(function () {
    InitModal();
});

function InitModal() {
    $.ajax({
        url: "/Exam/Questions/GetQuestion",
        method: "get",
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
                var row = StringFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td hidden>{3}</td></tr>", ck, head, type, score);
                target.append(row);
            });
        },
        error: function (data) {
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
        template.find("label[id='lbScore']").text(selectRow.children[3].innerHTML);
        $('#pasted').append(template);
    });
    $('#ImportQuestion').modal('hide');
});

$('#selectAll').click(function () {
    $('input[name="checkbox"]').prop('checked', this.checked);
});

$('.remove').click(function () {
    $(this).closest('tr').remove();
});