using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Azure.Lib.DocumentDB
{
    
    public class DocumentDBClient : IDisposable
    {
        private DocumentClient _client;

        private DocumentDBSettings _settings;

        private bool _initialized = false;

        public DocumentDBClient(DocumentDBSettings settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<T>> WhereAsync<T>(Func<T, bool> predicate)
        {
            await Init();

            return
                _client.CreateDocumentQuery<T>(GetCollectionUri<T>())
                    .Where(predicate)
                    .ToList();

        }

        public async Task<T> FirstOrDefaultAsync<T>(Func<T, bool> predicate)
        {
            await Init();

            return
                _client.CreateDocumentQuery<T>(GetCollectionUri<T>())
                    .Where(predicate)
                    .AsEnumerable()
                    .FirstOrDefault();
        }

        public async Task<T> Get<T>(string id) where T : BaseEntity
        {
            await Init();

            return _client.CreateDocumentQuery<T>(GetCollectionUri<T>()).Where(d => d.Id == id.ToString()).AsEnumerable().FirstOrDefault();
        }

        public async Task<IList<T>> GetAll<T>() where T : BaseEntity
        {
            await Init();

            return _client.CreateDocumentQuery<T>(GetCollectionUri<T>()).ToList();
        }

        public async Task<IList<T>> Get<T>(IList<string> ids) where T : BaseEntity
        {
            await Init();

            return _client.CreateDocumentQuery<T>(GetCollectionUri<T>()).Where(x => ids.Contains(x.Id)).ToList();
        }

        public async Task InsertAsync<T>(T entity) where T : BaseEntity
        {
            await Init();

            await _client.CreateDocumentCollectionIfNotExistsAsync(GetDBUri(), new DocumentCollection { Id = GetTypeName<T>() });

            await _client.CreateDocumentAsync(GetCollectionUri<T>(), entity);
        }

        public async Task InsertBatchAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            await Init();

            await _client.CreateDocumentCollectionIfNotExistsAsync(GetDBUri(), new DocumentCollection { Id = GetTypeName<T>() });

            foreach (var entity in entities)
            {
                await _client.CreateDocumentAsync(GetCollectionUri<T>(), entity);
            }
        }

        public async Task UpsertAsync<T>(T entity) where T : BaseEntity
        {
            await Init();

            await _client.UpsertDocumentAsync(GetCollectionUri<T>(), entity);
        }

        public async Task DeleteAsync<T>(string id) where T : BaseEntity
        {
            await Init();

            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_settings.DatabaseName, GetTypeName<T>(), id.ToString()));

        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private async Task Init()
        {
            if (!_initialized)
            {
                _client = new DocumentClient(new Uri(_settings.EndpointUri), _settings.PrimaryKey);
                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _settings.DatabaseName });
                _initialized = true;
            }
        }

        private string GetTypeName<T>()
        {
            var collectionName = typeof(T).Name;
            var attrs = typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs.Length > 0)
            {
                collectionName = ((TableAttribute)attrs[0]).Name;
            }
            return collectionName;
        }

        private Uri GetCollectionUri<T>()
        {

            return UriFactory.CreateDocumentCollectionUri(_settings.DatabaseName, GetTypeName<T>());
        }

        private Uri GetDBUri()
        {
            return UriFactory.CreateDatabaseUri(_settings.DatabaseName);
        }
    }
}
