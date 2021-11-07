using NopApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.DAL.Repositories
{
    public class UserRepository
    {
        private NopAppContext _dbContext;

        public UserRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
