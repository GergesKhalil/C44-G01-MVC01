using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace GymManagementSystemPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController( IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion

        #region Get Member Data
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);

        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var HealthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord);
        }

        #endregion

        #region Create Member
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid","Check Data And Missing Fields" );
                return View(nameof(Create), CreatedMember);
            }

            bool Result = _memberService.CreateMember(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
                
            }
            else
            {
                ModelState.AddModelError("CreationFailed", "Failed To Create Member , Check Phone And Email");
               
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Edit Member
        public ActionResult MemberEdit(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"]="Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var MemberToUpdate = _memberService.GetMemberToUpdate(id);
            if (MemberToUpdate is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(MemberToUpdate);

        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id,MemberToUpdateViewModel MemberToEdit) 
        {
            if(!ModelState.IsValid)
            {            
                return View( MemberToEdit);
            }

            var Result = _memberService.UpdateMemberDetails(id, MemberToEdit);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        public ActionResult Delete(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                  return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetails(id);
            if(Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemeberId = id;
            ViewBag.MemberName = Member.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Delete";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion





    }
}
