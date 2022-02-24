﻿using microservice.Infrastructure.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Data.SQL
{
    public class AppointmentsContext : DbContext
    {
        public AppointmentsContext(DbContextOptions options) : base(options)
        {
        }

        //Tables
        public DbSet<User> Users { get; set; }

    }
}