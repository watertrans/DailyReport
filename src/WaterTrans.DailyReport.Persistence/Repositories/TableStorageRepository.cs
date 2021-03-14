using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.Exceptions;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// テーブルストレージリポジトリ
    /// </summary>
    /// <typeparam name="TEntity">エンティティの型</typeparam>
    public abstract class TableStorageRepository<TEntity> : Repository, ITableStorageRepository<TEntity>
        where TEntity : TableEntity
    {
        /// <summary>
        /// CloudStorageAccount インスタンス。
        /// </summary>
        protected CloudStorageAccount StorageAccount { get; }

        /// <summary>
        /// CloudTableClient インスタンス。
        /// </summary>
        protected CloudTableClient TableClient { get; }

        /// <summary>
        /// CloudTable インスタンス。
        /// </summary>
        protected CloudTable Table { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public TableStorageRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
            StorageAccount = CloudStorageAccount.Parse((string)DBSettings.StorageConnectionString);
            TableClient = StorageAccount.CreateCloudTableClient();
        }

        /// <inheritdoc/>
        public void Create(TEntity entity)
        {
            try
            {
                var operation = TableOperation.Insert(entity);
                var result = Table.Execute(operation);
            }
            catch (Exception ex) when (ex.Message.Contains("Conflict"))
            {
                throw new DuplicateKeyException("The primary key is duplicated.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task CreateAsync(TEntity entity)
        {
            try
            {
                var operation = TableOperation.Insert(entity);
                var result = await Table.ExecuteAsync(operation);
            }
            catch (Exception ex) when (ex.Message.Contains("Conflict"))
            {
                throw new DuplicateKeyException("The primary key is duplicated.", ex);
            }
        }

        /// <inheritdoc/>
        public TEntity Read(TEntity entity)
        {
            var operation = TableOperation.Retrieve<TEntity>(entity.PartitionKey, entity.RowKey);
            var result = Table.Execute(operation);
            return result.Result as TEntity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> ReadAsync(TEntity entity)
        {
            var operation = TableOperation.Retrieve<TEntity>(entity.PartitionKey, entity.RowKey);
            var result = await Table.ExecuteAsync(operation);
            return result.Result as TEntity;
        }

        /// <inheritdoc/>
        public bool Update(TEntity entity)
        {
            try
            {
                var operation = TableOperation.Replace(entity);
                var result = Table.Execute(operation);
                return true;
            }
            catch (StorageException ex) when (ex.Message.Contains("Not Found"))
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                var operation = TableOperation.Replace(entity);
                var result = await Table.ExecuteAsync(operation);
                return true;
            }
            catch (StorageException ex) when (ex.Message.Contains("Not Found"))
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Delete(TEntity entity)
        {
            try
            {
                var person = new DynamicTableEntity
                {
                    PartitionKey = entity.PartitionKey,
                    RowKey = entity.RowKey,
                    ETag = "*",
                };
                var operation = TableOperation.Delete(person);
                Table.Execute(operation);
                return true;
            }
            catch (StorageException ex) when (ex.Message.Contains("Not Found"))
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                var person = new DynamicTableEntity
                {
                    PartitionKey = entity.PartitionKey,
                    RowKey = entity.RowKey,
                    ETag = "*",
                };
                var operation = TableOperation.Delete(person);
                await Table.ExecuteAsync(operation);
                return true;
            }
            catch (StorageException ex) when (ex.Message.Contains("Not Found"))
            {
                return false;
            }
        }
    }
}
