using Lab4.BLL;
using Lab4.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Test
{
    static class Utils
    {
        const string connectionString =
           @"Data Source = localhost\\SQLEXPRESS; Initial Catalog = SocialNet; User ID = Admin; Password = Admin; Integrated security = false";

        public static string ConnectionString => connectionString;

        public static SocialNetDbContext BuildDbContext()
        {
            return new SocialNetDbContext(ConnectionString);
        }
    }
}
