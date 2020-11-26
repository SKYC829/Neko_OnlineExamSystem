using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neko.App.Interfaces.Exam;
using Neko.App.Models.Exam;
using Neko.App.Models.System;
using Util.DataObject;
using System.IO;
using Util.Threading;
using Neko.Unity;
using System.Data;
using Util.IO;

namespace Neko.MVC.Areas.Exam.Controllers
{
    [Area("Exam")]
    [Route("Exam/Questions")]
    public class QuestionsController : NekoControllerBase
    {
        private readonly IQuestionApp _questionApp;

        public QuestionsController(IQuestionApp questionApp)
        {
            _questionApp = questionApp;
        }

        [Route("Index")]
        public IActionResult Index(int pageIndex = 1)
        {
            IEnumerable<QuestionInfo> questions = _questionApp.Query(pageIndex, out int totalCount, out int totalPage, null, null);
            ViewBag.TotalPages = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPage = totalPage;
            return View(questions);
        }

        [Route("UploadFile")]
        public IActionResult UploadQuestionFile()
        {
            return View();
        }

        [HttpPost, Route("UploadFile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadQuestionFile([FromForm] FileUpload fileUpload, [FromServices] IWebHostEnvironment enviroment)
        {
            if (ModelState.IsValid)
            {
                var filePath = Path.Combine(enviroment.WebRootPath, "Upload", loginUser.UserName, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var fileName = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(fileUpload.UploadFile.FileName));
                fileName = Path.Combine(filePath, fileName);
                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    fileUpload.UploadFile.CopyTo(fs);
                }
                IEnumerable<DataSet> excelData = ReadQuestionFromExcel.ReadExcelFile(fileName);
                //解析Excel数据
                foreach (DataSet ds in excelData)
                {
                    DataTable excelTable = ds.Tables[0];
                    if (excelTable == null || excelTable.Rows.Count < 2)
                    {
                        continue;
                    }
                    DataRow titleRow = excelTable.Rows[0]; //拿到标题行
                    int columnNum = excelTable.Columns.Count; //得到列数
                    int questionColumnIndex = 0, answerColumnIndex = 0;
                    List<int> solutionColumnIndexs = new List<int>();
                    //得到题目列，答案列，正确答案列的索引
                    for (int i = 0; i < columnNum; i++)
                    {
                        string rowValue = RowUtil.GetString(titleRow, excelTable.Columns[i].ColumnName);
                        if (rowValue.Equals("题目"))
                        {
                            questionColumnIndex = i;
                        }
                        if (rowValue.StartsWith("选项"))
                        {
                            if (!solutionColumnIndexs.Contains(i))
                            {
                                solutionColumnIndexs.Add(i);
                            }
                        }
                        if (rowValue.Equals("答案"))
                        {
                            answerColumnIndex = i;
                        }
                    }
                    DataColumn questionColumn = excelTable.Columns[questionColumnIndex];
                    DataColumn answerColumn = excelTable.Columns[answerColumnIndex];
                    DataColumn[] solutionColumns = new DataColumn[solutionColumnIndexs.Count];
                    for (int i = 0; i < solutionColumnIndexs.Count; i++)
                    {
                        solutionColumns[i] = excelTable.Columns[solutionColumnIndexs[i]];
                    }
                    //生成问题和答案
                    for (int i = 1; i < excelTable.Rows.Count - 1; i++)
                    {
                        DataRow currentRow = excelTable.Rows[i];
                        //得到题目
                        string questionName = RowUtil.GetString(currentRow, questionColumn.ColumnName);
                        if (string.IsNullOrEmpty(questionName))
                        {
                            continue;
                        }
                        //得到正确答案序号
                        string answer = RowUtil.GetString(currentRow, answerColumn.ColumnName);
                        List<string> solutions = new List<string>();
                        //得到所有答案
                        foreach (DataColumn column in solutionColumns)
                        {
                            string solution = RowUtil.GetString(currentRow, column.ColumnName);
                            solutions.Add(solution);
                        }
                        CreateQuestionInfo questionInfo = new CreateQuestionInfo();
                        questionInfo.Name = questionName;
                        questionInfo.QuestionType = QuestionType.Radio;
                        questionInfo.QuestionTypeInt = (int)questionInfo.QuestionType;
                        questionInfo.QuestionScore = 1;
                        questionInfo.QuestionGroupName = fileUpload.UploadFile.FileName.Replace(Path.GetExtension(fileUpload.UploadFile.FileName), null);
                        questionInfo.SolutionNames = solutions.AsEnumerable();
                        List<double> solutionScore = new List<double>();
                        string regexStr = @"^[A-Za-z]+";
                        //计算选项得分
                        foreach (string item in questionInfo.SolutionNames)
                        {
                            //如果正确答案是A~Z，就判断所有选项的开头，否则判断答案是否包含
                            if (RegularUtil.Regex(regexStr, answer))
                            {
                                if (item.StartsWith(answer)) //选项的开头和答案一致，证明是正确答案
                                {
                                    solutionScore.Add(1);
                                }
                                else
                                {
                                    solutionScore.Add(0);
                                }
                            }
                            else
                            {
                                if (item.Contains(answer)) //选项包含答案，就认为是正确答案
                                {
                                    solutionScore.Add(1);
                                }
                                else
                                {
                                    solutionScore.Add(0);
                                }
                            }
                        }
                        questionInfo.SolutionScore = solutionScore.AsEnumerable();
                        //去掉选项前边的序号
                        List<string> solutons1 = new List<string>();
                        foreach (string solution in solutions)
                        {
                            int index = solution.IndexOf('.');
                            string solutionValue = solution.Substring(index + 1, solution.Length - index - 1);
                            solutons1.Add(solutionValue);
                        }
                        questionInfo.SolutionNames = solutons1.AsEnumerable();
                        CreateQuestion(questionInfo);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("UploadFile", "目前只支持Excel文件(.xls/.xlsx)哦");
            return View(fileUpload);
        }

        [HttpGet, Route("Create")]
        public IActionResult QuestionCreate(int questionType = 0)
        {
            ViewBag.QuestionType = questionType;
            return View();
        }

        [HttpPost, Route("Create"), ValidateAntiForgeryToken]
        public IActionResult QuestionCreate([FromForm] CreateQuestionInfo questionInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CreateQuestion(questionInfo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ExecuteError", ex.Message);
                }
            }
            ViewBag.QuestionType = questionInfo.QuestionType;
            return View(questionInfo);
        }

        private void CreateQuestion(CreateQuestionInfo questionInfo)
        {
            QuestionInfo saveInfo = new QuestionInfo();
            saveInfo.Name = questionInfo.Name;
            saveInfo.QuestionType = (QuestionType)questionInfo.QuestionTypeInt;
            saveInfo.QuestionScore = questionInfo.QuestionScore;
            saveInfo.QuestionGroupName = questionInfo.QuestionGroupName;
            List<SolutionInfo> solutions = new List<SolutionInfo>();
            for (int i = 0; i < questionInfo.SolutionNames.Count(); i++)
            {
                SolutionInfo solution = new SolutionInfo();
                solution.Name = questionInfo.SolutionNames.ElementAt(i);
                solution.Score = questionInfo.SolutionScore.ElementAt(i);
                solution.IsCorrect = solution.Score > 0;
                solutions.Add(solution);
            }
            saveInfo.Solutions = solutions;
            _questionApp.Save(saveInfo);
        }

        [HttpGet, Route("Edit/{id?}")]
        public IActionResult QuestionEdit(int id)
        {
            QuestionInfo questionInfo = _questionApp.Load(id);
            if (questionInfo == null)
            {
                return NotFound();
            }
            CreateQuestionInfo editInfo = new CreateQuestionInfo();
            editInfo.Name = questionInfo.Name;
            editInfo.QuestionType = questionInfo.QuestionType;
            editInfo.QuestionScore = questionInfo.QuestionScore;
            List<string> solutionNames = new List<string>();
            List<double> solutionScore = new List<double>();
            foreach (SolutionInfo solution in questionInfo.Solutions)
            {
                solutionNames.Add(solution.Name);
                solutionScore.Add(solution.Score);
            }
            editInfo.SolutionNames = solutionNames;
            editInfo.SolutionScore = solutionScore;
            ViewBag.QuestionType = editInfo.QuestionType;
            return View(editInfo);
        }

        [HttpPost, Route("Edit/{id?}"), ValidateAntiForgeryToken]
        public IActionResult QuestionEdit(int id, [FromForm] CreateQuestionInfo questionInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    QuestionInfo editInfo = new QuestionInfo();
                    editInfo.Id = id;
                    editInfo.Name = questionInfo.Name;
                    editInfo.QuestionType = questionInfo.QuestionType;
                    editInfo.QuestionScore = questionInfo.QuestionScore;
                    List<SolutionInfo> solutions = new List<SolutionInfo>();
                    for (int i = 0; i < questionInfo.SolutionNames.Count(); i++)
                    {
                        SolutionInfo solution = new SolutionInfo();
                        solution.Name = questionInfo.SolutionNames.ElementAt(i);
                        solution.Score = questionInfo.SolutionScore.ElementAt(i);
                        solution.IsCorrect = solution.Score > 0;
                        solutions.Add(solution);
                    }
                    editInfo.Solutions = solutions;
                    _questionApp.Save(editInfo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ExecuteError", ex.Message);
                }
            }
            ViewBag.QuestionType = questionInfo.QuestionType;
            return View(questionInfo);
        }

        [HttpPost, Route("Delete/{id?}"), ValidateAntiForgeryToken]
        public IActionResult QuestionDelete([FromBody] string id)
        {
            try
            {
                int questionId = StringUtil.GetInt(id);
                if (questionId <= 0)
                {
                    return NotFound();
                }
                _questionApp.Remove(questionId);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "删除失败", statusCode: 405);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, Route("GetQuestion")]
        public JsonResult GetAllQuestion(string nameFilter = null,string groupFilter = null)
        {
            IQueryable<QuestionInfo> questions = _questionApp.Query(p => p.Id > 0);
            if (!string.IsNullOrEmpty(nameFilter))
            {
                questions = questions.Where(p => p.Name.Contains(nameFilter));
            }
            if (!string.IsNullOrEmpty(groupFilter))
            {
                questions = questions.Where(p => p.QuestionGroupName.Contains(groupFilter));
            }
            return Json(questions);
        }
    }
}
