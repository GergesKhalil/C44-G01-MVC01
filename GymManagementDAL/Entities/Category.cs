﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementDAL.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;


        public ICollection<Session> sessions { get; set; } = null!;

    }
}
