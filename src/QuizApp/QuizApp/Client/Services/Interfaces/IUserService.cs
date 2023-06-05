using QuizApp.Shared.DTO;

namespace QuizApp.Client.Services.Interfaces;

public interface IUserService
{
    List<UserDTO> Users { get; set; }

    Task<List<UserDTO>> GetUsersByGroupId(Guid groupId);
}
