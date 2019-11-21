using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TasksERP.Models;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace TasksERP.Controllers
{
    public class AssessmentStatisticsController : Controller
    {
        private AssessCenterEntities db = new AssessCenterEntities();

       // GET: AssessmentStatistics
       //[HttpPost]
       // public ActionResult Index(string StartDate, string EndDate, string EName, string JobTitle)
       // {


       //     DateTime sd = (StartDate.Length > 0) ? DateTime.ParseExact(StartDate, "dd.MM.yyyy", new CultureInfo("ru-RU")) : DateTime.ParseExact("01.01.2019", "dd.MM.yyyy", new CultureInfo("ru-RU"));
       //     DateTime ed = (EndDate.Length > 0) ? DateTime.ParseExact(EndDate, "dd.MM.yyyy", new CultureInfo("ru-RU")) : DateTime.ParseExact("31.12.2019", "dd.MM.yyyy", new CultureInfo("ru-RU"));
       //     string jt = JobTitle.ToLower() ?? "bookkeeper";

       //     var dbResult = db.AllEmployees.Where(x => jt == x.JobTitle.ToLower()).ToList()
       //         .Where(x => Convert.ToDateTime(x.AssessmentPeriod, new CultureInfo("ru-RU")) >= sd && Convert.ToDateTime(x.AssessmentPeriod, new CultureInfo("ru-RU")) <= ed)
       //         .OrderBy(x => x.AppraiseeName);




       //     return View(dbResult);
       // }

       // [HttpGet]
       // public async Task<ActionResult> Index()
       // {
       //     return View(await db.AllEmployees.ToListAsync());
       // }


        public ActionResult   Index(string fstatus)
        {
            IEnumerable<IDictionary<string, object>> result;
            var sql = "statAll";
            //ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["AssessCenterEntities"];
            using (var cnn = new SqlConnection("data source=sqlmis01;initial catalog=AssessCenter;persist security info=True;user id=aspnet;password=EMcua7TH;MultipleActiveResultSets=True;"))
            {
                cnn.Open();
                DynamicParameters p = new DynamicParameters();
                if (String.IsNullOrEmpty(fstatus) ==false) 
                    {
                    if (fstatus.ToLower().Contains("payroll")) { p.Add("@JobFilter", "%payroll%"); } else p.Add("@JobFilter", fstatus);

                    }
                else
                {
                    p.Add("@JobFilter", "bookkeeper");
                }
               
                result = (IEnumerable<IDictionary<string, object>>)
                cnn.Query(sql,p,commandType: CommandType.StoredProcedure);
                DataTable nt = ToDataTable(result);
                //DataRow r1 = nt.Columns("AppraiseeMail");
                //nt.Columns.Remove.Remove(r1);
                
                nt.Columns.Remove("AppraiseeMail");
                nt.Columns.Remove("SeniorMail");
                nt.Columns.Remove("ManagerMail");
                

                return View(nt);

            }

            
            
        }
        public ActionResult Consultant()
        {
            IEnumerable<IDictionary<string, object>> result;
            var sql = "statAll";
            //ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["AssessCenterEntities"];
            using (var cnn = new SqlConnection("data source=sqlmis01;initial catalog=AssessCenter;persist security info=True;user id=aspnet;password=EMcua7TH;MultipleActiveResultSets=True;"))
            {
                cnn.Open();

                DynamicParameters p = new DynamicParameters();

                result = (IEnumerable<IDictionary<string, object>>)
                cnn.Query(sql, null, commandType: CommandType.StoredProcedure);
                DataTable nt = ToDataTable(result);
                //DataRow r1 = nt.Columns("AppraiseeMail");
                //nt.Columns.Remove.Remove(r1);
                nt.Columns.Remove("AppraiseeMail");
                nt.Columns.Remove("SeniorMail");
                nt.Columns.Remove("ManagerMail");

                return View(nt);

            }



        }
        
         
        // GET: AssessmentStatistics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllEmployee allEmployee = await db.AllEmployees.FindAsync(id);
            if (allEmployee == null)
            {
                return HttpNotFound();
            }
            return View(allEmployee);
        }

        // GET: AssessmentStatistics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssessmentStatistics/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CreationDate,ApproveDate,AssessmentDate,AssessmentPeriod,FilingDeadline,AppraiseeMail,AppraiseeName,AppraiseeMail2,AppraiseeName2,Department,JobTitle,SeniorMail,SeniorName,Grade,ManagerMail,ManagerName,PayrollCaclMark,PayrollCaclComments,PreparationOfTaxReturnMark,PreparationOfTaxReturnComments,ReportingToHeadOfficeMark,ReportingToHeadOfficeComments,EnglishSkillsMark,EnglishSkillsComments,ConsultingOfClientsMark,ConsultingOfClientsComments,SpeedOfWorkMark,SpeedOfWorkComments,QualityOfWorkMark,QualityOfWorkComments,ComplianceWithInternalProceduresMark,ComplianceWithInternalProceduresComments,CommunicationWithClientsMark,CommunicationWithClientsComments,TeamWorkMark,TeamWorkComments,TeachingOfSubordinatesMark,TeachingOfSubordinatesComments,ControlOfTeamWorkMark,ControlOfTeamWorkComments,OrganizationOfTeamWorkMark,OrganizationOfTeamWorkComments,ProactiveApproachMark,ProactiveApproachComments,ConsultingMark,ConsultingComments,DifficultyOfClientsMark,DifficultyOfClientsComments,EnglishLanguageMark,EnglishLanguageComments,MarkByManager,MarkByManagerComments")] AllEmployee allEmployee)
        {
            if (ModelState.IsValid)
            {
                db.AllEmployees.Add(allEmployee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(allEmployee);
        }

        // GET: AssessmentStatistics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllEmployee allEmployee = await db.AllEmployees.FindAsync(id);
            if (allEmployee == null)
            {
                return HttpNotFound();
            }
            return View(allEmployee);
        }

        // POST: AssessmentStatistics/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CreationDate,ApproveDate,AssessmentDate,AssessmentPeriod,FilingDeadline,AppraiseeMail,AppraiseeName,AppraiseeMail2,AppraiseeName2,Department,JobTitle,SeniorMail,SeniorName,Grade,ManagerMail,ManagerName,PayrollCaclMark,PayrollCaclComments,PreparationOfTaxReturnMark,PreparationOfTaxReturnComments,ReportingToHeadOfficeMark,ReportingToHeadOfficeComments,EnglishSkillsMark,EnglishSkillsComments,ConsultingOfClientsMark,ConsultingOfClientsComments,SpeedOfWorkMark,SpeedOfWorkComments,QualityOfWorkMark,QualityOfWorkComments,ComplianceWithInternalProceduresMark,ComplianceWithInternalProceduresComments,CommunicationWithClientsMark,CommunicationWithClientsComments,TeamWorkMark,TeamWorkComments,TeachingOfSubordinatesMark,TeachingOfSubordinatesComments,ControlOfTeamWorkMark,ControlOfTeamWorkComments,OrganizationOfTeamWorkMark,OrganizationOfTeamWorkComments,ProactiveApproachMark,ProactiveApproachComments,ConsultingMark,ConsultingComments,DifficultyOfClientsMark,DifficultyOfClientsComments,EnglishLanguageMark,EnglishLanguageComments,MarkByManager,MarkByManagerComments")] AllEmployee allEmployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(allEmployee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(allEmployee);
        }

        // GET: AssessmentStatistics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllEmployee allEmployee = await db.AllEmployees.FindAsync(id);
            if (allEmployee == null)
            {
                return HttpNotFound();
            }
            return View(allEmployee);
        }

        // POST: AssessmentStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AllEmployee allEmployee = await db.AllEmployees.FindAsync(id);
            db.AllEmployees.Remove(allEmployee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            if (items == null) return null;
            var data = items.ToArray();
            if (data.Length == 0) return null;

            var dt = new DataTable();
            foreach (var pair in ((IDictionary<string, object>)data[0]))
            {
                dt.Columns.Add(pair.Key, (pair.Value ?? string.Empty).GetType());
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }
    }
}
