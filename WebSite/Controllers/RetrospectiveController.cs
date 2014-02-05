using System;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using RetroShark.Application.Backend.Commands;
using RetroShark.Application.Backend.Services;
using RetroShark.Application.Hubs;
using RetroShark.Application.Models;

namespace RetroShark.Application.Controllers
{
    [Authorize]
    public class RetrospectiveController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRetrospectiveService _retrospectiveService;

        public RetrospectiveController(IAuthenticationService authenticationService, IRetrospectiveService retrospectiveService)
        {
            _authenticationService = authenticationService;
            _retrospectiveService = retrospectiveService;
        }

        public ActionResult Index(string code)
        {
            var retrospective = _retrospectiveService.GetByCode(code);

            if (retrospective == null)
            {
                return HttpNotFound();
            }

            bool isAdmin = string.Equals(retrospective.AdminCode, code, StringComparison.OrdinalIgnoreCase);

            if (isAdmin)
            {
                _authenticationService.SetAdminRole(User.Identity.Name);
            }

            return View(new RetrospectiveViewModel(retrospective, isAdmin));
        }

        [HttpGet]
        public ActionResult Start(StartViewModel model = null)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Start(CreateRetrospectiveCommand createRetrospective)
        {
            if (ModelState.IsValid)
            {
                CommandProcessor.Process(createRetrospective);

                _authenticationService.SetAdminRole(User.Identity.Name);

                return Redirect("~/" + createRetrospective.Result.AdminCode);
            }

            return Start(new StartViewModel { Title = createRetrospective.Title });
        }

        public ActionResult SaveFeedbacks(SaveFeedbacksCommand saveFeedbacks)
        {
            if (ModelState.IsValid)
            {
                CommandProcessor.Process(saveFeedbacks);

                var retrospective = _retrospectiveService.GetByCode(saveFeedbacks.RetrospectiveCode);

                GlobalHost.ConnectionManager.GetHubContext<RetrospectiveHub>().Clients.Group(retrospective.ParticipantCode).refreshRetrospective(retrospective);
            }


        }
    }
}
