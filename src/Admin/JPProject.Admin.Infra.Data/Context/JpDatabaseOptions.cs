﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JPProject.Admin.Infra.Data.Context
{
    public class JpDatabaseOptions
    {
        public bool MustThrowExceptionIfDatabaseDontExist { get; set; } = true;
    }
}
