using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Backend.Services
{
    public interface IRetrospectiveService
    {
        Retrospective GetByCode(string code);
    }
}