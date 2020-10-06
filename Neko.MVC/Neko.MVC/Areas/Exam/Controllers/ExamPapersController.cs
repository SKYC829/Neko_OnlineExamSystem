using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neko.App.Interfaces.Exam;
using Neko.App.Models.Exam;
using Util.DataObject;

namespace Neko.MVC.Areas.Exam.Controllers
{
    [Area("Exam")]
    [Route("Exam/ExamPapers")]
    public class ExamPapersController : NekoControllerBase
    {
        private readonly IExamPaperApp _examPaperApp;
        private readonly IQuestionApp _questionApp;
        private readonly ILogger<ExamPapersController> _log;
        public ExamPapersController(IExamPaperApp examPaperApp,IQuestionApp questionApp,ILogger<ExamPapersController> logger)
        {
            _examPaperApp = examPaperApp;
            _questionApp = questionApp;
            _log = logger;
        }

        [Route("")]
        [Route("Index")]
        [HttpGet]
        public IActionResult Index(int pageIndex = 1)
        {
            IEnumerable<ExamPaperInfo> questions = _examPaperApp.Query(pageIndex, out int totalCount, out int totalPage, null, null);
            ViewBag.TotalPages = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPage = totalPage;
            return View(questions);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Route("Create")]
        public IActionResult Create([FromForm] EditExamPaperInfo paperInfo)
        {
            ModelState.Remove("QuestionIds");
            ModelState.Remove("Id");
            if (paperInfo.Questions == null)
            {
                paperInfo.Questions = new List<QuestionInfo>();
            }
            paperInfo.Questions.Clear();
            for (int i = 0; i < paperInfo.QuestionIds.Count; i++)
            {
                QuestionInfo question = _questionApp.Load(paperInfo.QuestionIds.ElementAt(i));
                paperInfo.Questions.Add(question);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if(paperInfo.Questions.Count <= 0)
                    {
                        throw new NotSupportedException("试卷不允许一道题目都没有哦");
                    }
                    _examPaperApp.Save(paperInfo);
                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    ModelState.AddModelError("ExcuteError", ex.Message);
                    _log.LogError(ex, ex.Message);
                }
            }
            ModelState.AddModelError("ExcuteError", "保存失败！请刷新页面后再试！");
            return View(paperInfo);
        }

        [HttpGet,Route("Edit")]
        public IActionResult Edit(int id)
        {
            EditExamPaperInfo paperInfo = _examPaperApp.Load(id);
            return View(paperInfo);
        }

        [HttpPost,Route("Edit"),ValidateAntiForgeryToken]
        public IActionResult Edit(int id,[FromForm] EditExamPaperInfo paperInfo)
        {
            ModelState.Remove("QuestionIds");
            if (paperInfo.Questions == null)
            {
                paperInfo.Questions = new List<QuestionInfo>();
            }
            paperInfo.Questions.Clear();
            for (int i = 0; i < paperInfo.QuestionIds.Count; i++)
            {
                QuestionInfo question = _questionApp.Load(paperInfo.QuestionIds.ElementAt(i));
                paperInfo.Questions.Add(question);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (paperInfo.Questions.Count <= 0)
                    {
                        throw new NotSupportedException("试卷不允许一道题目都没有哦");
                    }
                    _examPaperApp.Save(paperInfo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ExcuteError", ex.Message);
                    _log.LogError(ex, ex.Message);
                }
            }
            ModelState.AddModelError("ExcuteError", "保存失败！请刷新页面后再试！");
            return View(paperInfo);
        }

        [HttpPost,Route("Delete")]
        public IActionResult Delete([FromBody] string id)
        {
            try
            {
                int paperId = StringUtil.GetInt(id);
                if(paperId <= 0)
                {
                    return NotFound();
                }
                _examPaperApp.Remove(paperId);
                return Ok();
            }catch(Exception ex)
            {
                return Problem(detail: ex.Message, title: "删除失败", statusCode: 405);
            }
        }
    }
}
