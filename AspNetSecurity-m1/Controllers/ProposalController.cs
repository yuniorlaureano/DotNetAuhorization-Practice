using AspNetSecurity_m1.Models;
using AspNetSecurity_m1.Repositories;
using AspNetSecurityNoSecurity.AuthorizationPractice.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSecurity_m1.Controllers
{
    public class ProposalController: Controller
    {
        private readonly ConferenceRepo conferenceRepo;
        private readonly ProposalRepo proposalRepo;
        private readonly IDataProtector dataProtector;

        public ProposalController(ConferenceRepo conferenceRepo, ProposalRepo proposalRepo, IDataProtectionProvider dataProtectionProvider, PurposeStringConstant purposeStringConstant)
        {
            dataProtector = dataProtectionProvider.CreateProtector(purposeStringConstant.ConferenceIdQueryStirng);
            this.conferenceRepo = conferenceRepo;
            this.proposalRepo = proposalRepo;
        }

        public IActionResult Index(int conferenceId)
        {
            var _conferenceId = int.Parse(dataProtector.Unprotect(conferenceId.ToString()));
            var conference = conferenceRepo.GetById(_conferenceId);      
            ViewBag.Title = $"Speaker - Proposals For Conference {conference.Name} {conference.Location}";
            ViewBag.ConferenceId = _conferenceId;

            return View(proposalRepo.GetAllForConference(_conferenceId));
        }

        public IActionResult AddProposal(int conferenceId)
        {
            var _conferenceId = int.Parse(dataProtector.Unprotect(conferenceId.ToString()));
            ViewBag.Title = "Speaker - Add Proposal";
            return View(new ProposalModel {ConferenceId = _conferenceId });
        }

        [HttpPost]
        public IActionResult AddProposal(ProposalModel proposal)
        {
            if (ModelState.IsValid)
                proposalRepo.Add(proposal);
            return RedirectToAction("Index", new {conferenceId = proposal.ConferenceId});
        }

        public IActionResult Approve(int proposalId)
        {
            var proposal = proposalRepo.Approve(proposalId);
            return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId });
        }
    }
}
