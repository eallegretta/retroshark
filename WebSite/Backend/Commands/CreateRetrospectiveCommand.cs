
using System;
using System.ComponentModel.DataAnnotations;
using Cinchcast.Framework.Commands;
using NHibernate;
using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Backend.Commands
{
    public class CreateRetrospectiveCommand : Command<Retrospective>
    {
        private readonly ISessionFactory _sessionFactory;

        [Required]
        public string Title { get; set; }

        public CreateRetrospectiveCommand(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        protected override void OnExecute()
        {
            var retrospective = new Retrospective { Title = Title, Date = DateTime.UtcNow, AdminCode = Retrospective.GenerateUrlCode(), ParticipantCode = Retrospective.GenerateUrlCode() };

            _sessionFactory.GetCurrentSession().Save(retrospective);

            Result = retrospective;
        }
    }
}