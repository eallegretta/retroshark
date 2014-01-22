using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using RetroShark.Application.Backend.Entities;
using RetroShark.Application.Backend.NHibernate;
using RetroShark.Application.Backend.Services;
using RetroShark.Application.Hubs.Models;

namespace RetroShark.Application.Hubs
{
    public class RetrospectiveHub: Hub
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRetrospectiveService _retrospectiveService;

        public RetrospectiveHub()
        {
            _authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
            _retrospectiveService = DependencyResolver.Current.GetService<IRetrospectiveService>();
        }

        public async override Task OnConnected()
        {
            string code = Context.QueryString["code"];

            Retrospective retrospective;

            using (new NHibernateUnitOfWork())
            {
                retrospective = _retrospectiveService.GetByCode(code);
            }

            if (retrospective == null)
            {
                throw new Exception("Retrospective not found");
            }

            code = retrospective.ParticipantCode;

            bool isAdmin = _authenticationService.IsLoggedUserAdmin();

            if (isAdmin)
            {
                await Groups.Add(Context.ConnectionId, code + "-admins");
            }

            await Groups.Add(Context.ConnectionId, code);

            Clients.Group(code).userLogin(Context.ConnectionId, (Context.User.Identity as RetroSharkIdentity).User);

            await base.OnConnected();
        }

        public void SendUsername(string destinationConnectionId)
        {
            Clients.Client(destinationConnectionId).userLogin(destinationConnectionId, (Context.User.Identity as RetroSharkIdentity).User);
        }

        public async override Task OnDisconnected()
        {
            Clients.OthersInGroup(Context.QueryString["code"]).userLogout(Context.ConnectionId);

            await base.OnDisconnected();
        }

        public void AddFeedbackContainer(FeedbackModel[] feedbacks)
        {
            if (!_authenticationService.IsLoggedUserAdmin())
            {
                return;
            }
                
            Clients.OthersInGroup(Context.QueryString["code"]).addFeedbackContainer(feedbacks);
        }
    }
}