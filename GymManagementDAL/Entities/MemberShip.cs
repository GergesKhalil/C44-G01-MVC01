using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberShip : BaseEntity
    {
        //startdate - createdAt of baseentity
        public DateTime EndDate { get; set; }
        //Read only property
        public string status
        {
            get
            {
                if (EndDate <= DateTime.Now)
                    return "Expired";
                else
                    return "Active";

            }
        }
        #region Membership - member
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        #endregion
        #region Membership - plan
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        #endregion
    }
}
