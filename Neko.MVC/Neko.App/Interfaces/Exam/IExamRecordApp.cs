using Neko.App.Models.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Interfaces.Exam
{
    public interface IExamRecordApp
    {
        //添加记录,添加问题记录，添加选择的答案记录
        //查询记录,查询问题记录，查询选择的答案记录
        //根据试卷编号和用户编号查询考试记录
        IQueryable<ExamRecordInfo> QueryRecord();

        Task<ExamRecordInfo> Load(int recordId);

        Task<ExamRecordInfo> Load(Expression<Func<DbModel.ExamRecordInfo, bool>> expression);

        IQueryable<ExamRecordInfo> QueryRecordByUser(int userId);

        IQueryable<ExamRecordInfo> QueryRecordByUser(int userId, Expression<Func<DbModel.ExamRecordInfo, bool>> expression);

        IQueryable<ExamQuestionRecordInfo> QueryRecordQuestion(int recordId);

        IQueryable<ExamQuestionRecordInfo> QueryRecordQuestion(int recordId, Expression<Func<DbModel.ExamRecordQuestionDetailInfo, bool>> expression);

        IQueryable<ExamSolutionRecordInfo> QueryRecordSolution(int recordId, int detailId);

        IQueryable<ExamSolutionRecordInfo> QueryRecordSolution(int recordId, int dedtailId, Expression<Func<DbModel.ExamRecordSolutionDetailInfo, bool>> expression);

        ExamRecordInfo AddRecord(ExamRecordInfo recordInfo);

        ExamQuestionRecordInfo AddQuestionRecord(int recordId, ExamQuestionRecordInfo recordInfo);

        ExamSolutionRecordInfo AddSolutionRecord(int recordId, int detailId, ExamSolutionRecordInfo recordInfo);
    }
}
