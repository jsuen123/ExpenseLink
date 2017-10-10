﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseLink.Models
{
    public class User
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public IList<UserInterest> Interests { get; set; }
    }
    public class UserInterest
    {
        public int Id { set; get; }
        public string InterestText { set; get; }
        public bool IsExperienced { set; get; }
    }
}