using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cinchcast.Framework.Commands;
using NHibernate;
using RetroShark.Application.Backend.Entities;
using RetroShark.Application.Backend.Services;

namespace RetroShark.Application.Backend.Commands
{
    public class SaveFeedbacksCommand: Command
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IRetrospectiveService _retrospectiveService;

        public SaveFeedbacksCommand(ISessionFactory sessionFactory, IRetrospectiveService retrospectiveService)
        {
            _sessionFactory = sessionFactory;
            _retrospectiveService = retrospectiveService;
        }

        [Required]
        public string RetrospectiveCode { get; set; }

        [Required]
        public IList<string> PositiveFeedbacks { get; set; }

        [Required]
        public IList<string> NegativeFeedbacks { get; set; }

        public override bool Validate()
        {
            var isValid = base.Validate();

            if (isValid)
            {
                if (_retrospectiveService.GetByCode(RetrospectiveCode) == null)
                {
                    AddValidationResult("The retrospective does not exist", "RetrospectiveCode");
                    isValid = false;
                }
            }

            return isValid;
        }

        protected override void OnExecute()
        {
            var retrospective = _retrospectiveService.GetByCode(RetrospectiveCode);

             foreach (var feedback in PositiveFeedbacks.Select(x => new Feedback
                 {
                         Retrospective = retrospective,
                         Text = x,
                         Type = FeedbackType.Positive
                 }).Concat(NegativeFeedbacks.Select(x => new Feedback
                     {
                             Retrospective = retrospective,
                             Text = x,
                             Type = FeedbackType.Negative
                     })))
             {
                 retrospective.Feedbacks.Add(feedback);
             }

            _sessionFactory.GetCurrentSession().Update(retrospective);
        }
    }
}