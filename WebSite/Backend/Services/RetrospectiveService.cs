using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Backend.Services
{
    public class RetrospectiveService : IRetrospectiveService
    {
        private readonly ISessionFactory _sessionFactory;

        public RetrospectiveService(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Retrospective GetByCode(string code)
        {
            var query = from retro in _sessionFactory.GetCurrentSession().Query<Retrospective>()
                        where retro.AdminCode == code || retro.ParticipantCode == code
                        select retro;

            return query.FirstOrDefault();
        }
    }
}