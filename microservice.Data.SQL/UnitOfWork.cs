using microservice.Core;
using microservice.Core.IRepositories;
using microservice.Data.SQL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Data.SQL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppointmentsContext _context;
        public UnitOfWork(AppointmentsContext context)
        {
            this._context = context;
        }

        private IUserRepository _userRepository;



        public IUserRepository Users => _userRepository ??= new UserRepository(_context);


        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}
