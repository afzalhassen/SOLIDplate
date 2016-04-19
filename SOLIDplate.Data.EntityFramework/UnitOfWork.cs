using SOLIDplate.Data.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SOLIDplate.Data.EntityFramework
{
	public class UnitOfWork : IUnitOfWork
	{
		private static readonly object GlobalInitialiseLockObj = new object();
		private static bool _globalContextInitialised;
		private readonly IDatabaseContextFactory _databaseContextFactory;
		private readonly object _instanceInstantiateLockObj;
		private bool _contextInstantiated;
		private IDatabaseContext _databaseContext;
		private bool _localContextInitialised;

		public UnitOfWork(IDatabaseContextFactory databaseContextFactory)
		{
			_instanceInstantiateLockObj = new object();
			_databaseContextFactory = databaseContextFactory;
		}

		public IDatabaseContext ThreadSafeDatabaseContext
		{
			get
			{
				EnsureThreadSafeDatabaseContextInstance();
				return _databaseContext;
			}
		}

		public void SaveChanges()
		{
			ThreadSafeDatabaseContext.SaveChanges();
		}

		public void DiscardChanges()
		{
			DisposeContext();
		}

		public bool IsDirty()
		{
			return ThreadSafeDatabaseContext.IsDirty();
		}

		public void Add<TEntity>(TEntity entity) where TEntity : class, new()
		{
			ThreadSafeDatabaseContext.EntitySet<TEntity>().Add(entity);
		}

		public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
		{
			ThreadSafeDatabaseContext.EntitySet<TEntity>().AddRange(entities);
		}

		public IQueryable<TEntity> All<TEntity>() where TEntity : class, new()
		{
			return ThreadSafeDatabaseContext.EntitySet<TEntity>().AsQueryable();
		}

		public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, new()
		{
			return ThreadSafeDatabaseContext.Entry(entity);
		}

		public void Delete<TEntity>(TEntity entity) where TEntity : class, new()
		{
			ThreadSafeDatabaseContext.EntitySet<TEntity>().Remove(entity);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void EnsureThreadSafeDatabaseContextInstance()
		{
			if (!_contextInstantiated)
			{
				lock (_instanceInstantiateLockObj)
				{
					if (!_contextInstantiated)
					{
						_databaseContext = _databaseContextFactory.CreateDatabaseContext();
						_contextInstantiated = true;
					}
				}
			}

			if (_localContextInitialised)
			{
				return;
			}
			if (!_globalContextInitialised)
			{
				lock (GlobalInitialiseLockObj)
				{
					if (!_globalContextInitialised)
					{
						// http://msdn.microsoft.com/en-us/library/cc853327(v=vs.100).aspx
						// Execute a query to ensure View Generation occurs
						//var count = _databaseContext.EntitySet<EntityAudit>().Count();
						_globalContextInitialised = true;
					}
				}
			}

			_localContextInitialised = true;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				DisposeContext();
			}
		}

		private void DisposeContext()
		{
			if (_databaseContext == null)
			{
				return;
			}

			_databaseContext.DetachAll();
			_databaseContext.Dispose();
			_databaseContext = null;
		}
	}
}