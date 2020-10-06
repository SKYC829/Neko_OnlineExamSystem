using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Neko.App.Interfaces.Exam;
using Neko.App.Interfaces.Identity;
using Neko.App.Models.Exam;
using Neko.App.Models.Identity;
using Neko.Unity;
using Util.Common;

namespace Neko.MVC.Areas.Exam.Controllers
{
    [Area("Exam")]
    [Route("")]
    [Route("Exam/Home")]
    public class HomeController : NekoControllerBase
    {
        private readonly IExamPaperApp _examPaperApp;
        private readonly ICorrectExamApp _correctApp;
        private readonly IExamRecordApp _recordApp;
        private readonly IUserApp _userApp;
        private readonly ILogger<HomeController> _log;
        private IQueryable<UserInfo> _users;
        public HomeController(IExamPaperApp examPaperApp, ICorrectExamApp correctApp, IExamRecordApp recordApp, IUserApp userApp,ILogger<HomeController> logger)
        {
            _examPaperApp = examPaperApp;
            _recordApp = recordApp;
            _correctApp = correctApp;
            _userApp = userApp;
            _log = logger;
            _users = _userApp.Query();
        }

        [HttpGet]
        [Route(""),Route("Index")]
        public IActionResult Index()
        {
            IEnumerable<ExamPaperInfo> paperInfos = _examPaperApp.Query(p=>p.ExamDateFrom<=DateTime.Now && DateTime.Now < p.ExamDateTo);
            return View(paperInfos);
        }

        [HttpGet]
        [Route("Exam")]
        public IActionResult Exam(int paperId)
        {
            ExamPaperInfo examPaper = _examPaperApp.Load(paperId);
            RandomUtil.CanRepeat = true;
            RandomUtil.RandomCount = (uint)examPaper.QuestionRank;
            examPaper.Questions = RandomUtil.Draw(examPaper.Questions.ToList());
            return View(examPaper);
        }

        [HttpPost]
        [Route("Finish")]
        public async Task<IActionResult> Finish([FromBody] IEnumerable<ExamSubmitInfo> info)
        {
            foreach (ExamSubmitInfo item in info)
            {
                ExamRecordInfo result = await _correctApp.CorrectExam(item);
                return Ok(result);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Records")]
        public IActionResult Records(int userId = 0,int examPaperId = 0,DateTime? fromDate = null,DateTime? toDate = null)
        {
            if(loginUser != null && loginUser.Role.RoleType > RoleType.Teacher)
            {
                userId = loginUser.Id;
            }
            IQueryable<ExamPaperInfo> examPapers = _examPaperApp.Query();
            ViewBag.Users = new SelectList(_users, "Id", "UserName", userId);
            ViewBag.Exams = new SelectList(examPapers, "Id", "Name", examPaperId);
            fromDate = fromDate.HasValue?fromDate.Value : DateTime.Today;
            toDate = toDate.HasValue ? toDate.Value : DateTime.Today;
            ViewBag.FromDate = string.Format("{0:yyyy-MM-dd}", fromDate);
            ViewBag.ToDate = string.Format("{0:yyyy-MM-dd}", toDate);
            IQueryable<ExamRecordInfo> records = _recordApp.QueryRecordByUser(userId).Reverse();
            if (examPaperId > 0)
            {
                records = records.Where(p => p.ExamPaperId.Equals(examPaperId));
            }
            records = records.Where(p => p.CreateDate >= fromDate && p.CreateDate <= toDate.Value.AddDays(1).AddSeconds(-1));

            return View(records.AsEnumerable());
        }

        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> Details(int recordId)
        {
            ExamRecordInfo recordInfo = await _recordApp.Load(recordId);
            return View(recordInfo);
        }
    }
}
