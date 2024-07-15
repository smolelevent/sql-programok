﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SQL_projekt.Class
{
    internal class dbFunctions
    {
        public static string ConnectionStringSQLite
        {
            get
            {
                string database =
                AppDomain.CurrentDomain.BaseDirectory +
               "\\Database\\Contact.s3db";
                string connectionString =
                @"Data Source=" + Path.GetFullPath(database);
                return connectionString;
            }
        }
    }
}
