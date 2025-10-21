using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        //JoinDate == CreatedAt from BaseEntity
        public string? Photo { get; set; }

        #region Relationships
        #region  Member - HealthRecored
        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion
        #region Member - MemberShip
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion
        #region Member - Member Session
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion
        #endregion

    }
}
