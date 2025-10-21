using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int SessionId);
        bool CreateSession(CreateSessionViewModel CreatedSession);
        UpdateSessionViewModel? GetSessionToUpdate(int  SessionId);
        bool UpdatSession(UpdateSessionViewModel UpdatedSession , int sessionId);
        bool RemoveSession(int SessionId);

    }
}
