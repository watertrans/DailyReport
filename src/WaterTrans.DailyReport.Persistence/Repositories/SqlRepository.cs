using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.Exceptions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// SQLデータベースリポジトリ
    /// </summary>
    /// <typeparam name="TTableEntity">エンティティの型</typeparam>
    public abstract class SqlRepository<TTableEntity> : Repository, ISqlRepository<TTableEntity>
        where TTableEntity : SqlTableEntity
    {
        /// <summary>
        /// IDbConnection
        /// </summary>
        protected IDbConnection Connection { get; }

        /// <summary>
        /// DbProviderFactory
        /// </summary>
        protected DbProviderFactory Factory { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public SqlRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
            Factory = DBSettings.SqlProviderFactory;
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DBSettings.SqlConnectionString;
        }

        /// <inheritdoc/>
        public virtual void Create(TTableEntity entity)
        {
            try
            {
                Connection.Insert<TTableEntity>(entity, statement => statement
                    .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
            }
            catch (SqlException ex) when (ex.Number == 2601 | ex.Number == 2627)
            {
                throw new DuplicateKeyException("The primary key is duplicated.", ex);
            }
        }

        /// <inheritdoc/>
        public async virtual Task CreateAsync(TTableEntity entity)
        {
            try
            {
                await Connection.InsertAsync<TTableEntity>(entity, statement => statement
                    .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
            }
            catch (SqlException ex) when (ex.Number == 2601 | ex.Number == 2627)
            {
                throw new DuplicateKeyException("The primary key is duplicated.", ex);
            }
        }

        /// <inheritdoc/>
        public virtual TTableEntity Read(TTableEntity entity)
        {
            return Connection.Get<TTableEntity>(entity, statement => statement
                .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
        }

        /// <inheritdoc/>
        public async virtual Task<TTableEntity> ReadAsync(TTableEntity entity)
        {
            return await Connection.GetAsync<TTableEntity>(entity, statement => statement
                .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
        }

        /// <inheritdoc/>
        public virtual bool Update(TTableEntity entity)
        {
            return Connection.Update<TTableEntity>(entity, statement => statement
                .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
        }

        /// <inheritdoc/>
        public async virtual Task<bool> UpdateAsync(TTableEntity entity)
        {
            return await Connection.UpdateAsync<TTableEntity>(entity, statement => statement
                .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
        }

        /// <inheritdoc/>
        public virtual bool Delete(TTableEntity entity)
        {
            return Connection.Delete<TTableEntity>(entity, statement => statement
                .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
        }

        /// <inheritdoc/>
        public async virtual Task<bool> DeleteAsync(TTableEntity entity)
        {
            return await Connection.DeleteAsync<TTableEntity>(entity, statement => statement
                .WithTimeout(TimeSpan.FromSeconds(DBSettings.CommandTimeout)));
        }
    }
}
