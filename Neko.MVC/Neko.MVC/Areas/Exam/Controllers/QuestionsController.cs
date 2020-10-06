using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neko.App.Interfaces.Exam;
using Neko.App.Models.Exam;
using Util.DataObject;

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
                    QuestionInfo saveInfo = new QuestionInfo();
                    saveInfo.Name = questionInfo.Name;
                    saveInfo.QuestionType = (QuestionType)questionInfo.QuestionTypeInt;
                    saveInfo.QuestionScore = questionInfo.QuestionScore;
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
        public JsonResult GetAllQuestion()
        {
            IQueryable<QuestionInfo> questions = _questionApp.Query(p => p.Id > 0);
            return Json(questions);
        }
    }
}
