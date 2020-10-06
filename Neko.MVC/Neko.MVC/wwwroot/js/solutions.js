$(function () {

    $('#btnAddSolution').click(function () {
        //复制li
        var from = $(this).data('target');
        var to = $(this).data('to');
        var target = $(from).clone(true).removeAttr('hidden');
        var name = target.find("input[placeholder$='答案']");
        var score = target.find("input[placeholder$='分值']")
        name.attr('id', "SolutionNames");
        name.attr('name', "SolutionNames");
        score.attr('id', "SolutionScore");
        score.attr('name', "SolutionScore");
        target.appendTo(to);
    });

    $('.btn-danger').click(function () {
        //删除父级li
        $(this).closest('.nav-item').remove();
    });
});