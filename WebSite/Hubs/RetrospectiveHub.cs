using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using RetroShark.Application.Backend.Entities;
using RetroShark.Application.Backend.NHibernate;
using RetroShark.Application.Backend.Services;
using RetroShark.Application.Hubs.Models;

namespace RetroShark.Application.Hubs
{
    public class RetrospectiveHub : Hub
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRetrospectiveService _retrospectiveService;

        private string _code;

        public RetrospectiveHub()
            : this(DependencyResolver.Current.GetService<IAuthenticationService>(), DependencyResolver.Current.GetService<IRetrospectiveService>())
        {

        }

        public RetrospectiveHub(IAuthenticationService authenticationService, IRetrospectiveService retrospectiveService)
        {
            _authenticationService = authenticationService;
            _retrospectiveService = retrospectiveService;
        }

        private string GetParticipantsGroupName()
        {
            if (string.IsNullOrWhiteSpace(_code))
            {
                using (new NHibernateUnitOfWork())
                {
                    var retrospective = _retrospectiveService.GetByCode(Context.QueryString["code"]);

                    if (retrospective == null)
                    {
                        throw new Exception("Retrospective not found");
                    }

                    _code = retrospective.ParticipantCode;
                }
            }

            return _code;
        }

        private string GetAdminsGroupName()
        {
            return GetParticipantsGroupName() + "-admins";
        }

        public async override Task OnConnected()
        {
            string group = GetParticipantsGroupName();
            string adminGroup = GetAdminsGroupName();

            bool isAdmin = _authenticationService.IsLoggedUserAdmin();

            if (isAdmin)
            {
                await Groups.Add(Context.ConnectionId, adminGroup);
            }

            await Groups.Add(Context.ConnectionId, group);

            // Inform other users we've just logged in
            Clients.Group(group).userLogin(Context.ConnectionId, (Context.User.Identity as RetroSharkIdentity).User);

            // Request from admins the current feedbacks 
            Clients.OthersInGroup(group).requestFeedbacks(Context.ConnectionId);

            await base.OnConnected();
        }

        public async override Task OnDisconnected()
        {
            // Inform other users we've just logged out
            Clients.OthersInGroup(GetParticipantsGroupName()).userLogout(Context.ConnectionId);

            await base.OnDisconnected();
        }

        public void SendUsername(string destinationConnectionId)
        {
            Clients.Client(destinationConnectionId).userLogin(destinationConnectionId, (Context.User.Identity as RetroSharkIdentity).User);
        }

        public void SendRequestedFeedbacks(string destinationConnectionId, string[] positiveFeedbackIds, string[] negativeFeedbackIds)
        {
            if (!string.IsNullOrWhiteSpace(destinationConnectionId))
            {
                Clients.Client(destinationConnectionId).addRequestedFeedbacks(positiveFeedbackIds.Where(x => x != null), negativeFeedbackIds.Where(x => x != null));
            }
            else
            {
                Clients.OthersInGroup(GetParticipantsGroupName()).addRequestedFeedbacks(positiveFeedbackIds.Where(x => x != null), negativeFeedbackIds.Where(x => x != null));
            }
        }

        public void RemoveFeedback(string feedbackId)
        {
            if (_authenticationService.IsLoggedUserAdmin())
            {
                Clients.OthersInGroup(GetParticipantsGroupName()).removeFeedback(feedbackId);
            }
        }
    }
}