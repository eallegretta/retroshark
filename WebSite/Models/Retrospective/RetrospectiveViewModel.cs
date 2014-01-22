using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Models
{
    public class RetrospectiveViewModel
    {
        public RetrospectiveViewModel(Retrospective retrospective, bool isAdmin)
        {
            Retrospective = retrospective;
            IsAdmin = isAdmin;
        }

        public Retrospective Retrospective { get; private set; }
        public bool IsAdmin { get; private set; }
    }
}