﻿using System.Web.Mvc;

namespace ExpenseLink
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //Require authrization globally
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
