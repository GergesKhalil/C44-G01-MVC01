using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementDAL.Entities
{
    public class MemberSession : BaseEntity
    {
        // BookingDate - createdAt of baseEntity
        public bool IsAttended { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;


        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

    }
}
