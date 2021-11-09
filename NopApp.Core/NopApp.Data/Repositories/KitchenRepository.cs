using NopApp.Data;
using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.DAL.Repositories
{
    public class KitchenRepository
    {
        private NopAppContext _dbContext;

        public KitchenRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task GetKitchenById(int id)
        {
            //return await _dbContext.FindAsync(id);

        }
    }
}
