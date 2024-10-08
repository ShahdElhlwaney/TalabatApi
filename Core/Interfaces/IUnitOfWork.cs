﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        public  Task<int> Complete();
    }
}
