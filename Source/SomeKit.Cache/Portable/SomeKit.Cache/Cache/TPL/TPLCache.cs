namespace SomeKit.Cache.Cache.TPL
{
    public sealed class TPLCache<T> /*: ICache<T>*/
        where T : IHasKey<int>
    {
        /*private readonly int _partitionSize;
        private readonly Record<T>[][] _innerData;
        private readonly TransformBlock<RequestBase<T>, ResponseBase<T>>[] _handlers;

        private readonly IDictionary<int, Record<T>> _innerDictionary =
            new ConcurrentDictionary<int, Record<T>>();

        public TPLCache(int partitionSize)
        {
            if (partitionSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(partitionSize));

            _partitionSize = partitionSize;
            _innerData = new Record<T>[_partitionSize][];
            _handlers = Enumerable.Range(0, _partitionSize)
                .Select(
                    n =>
                        new TransformBlock<RequestBase<T>, ResponseBase<T>>(
                            (Func<RequestBase<T>, ResponseBase<T>>)Handle))
                .ToArray();
        }

        #region Simple CRUD
        public IQueryable<T> GetAll()
        {
            ParallelEnumerable.Range(0, _partitionSize)
                .Select(n => _handlers[n].Post(new GetAllRequest<T>(_innerData[n])));
            var result = ParallelEnumerable.Range(0, _partitionSize).Select(n => _handlers[n].Receive() as GetAllResponse<T>);
            if (result.Any(x => x.Error != null))
                throw new AggregateException(result.Where(x => x.Error != null).Select(x => x.Error));
            return result
                .SelectMany(x => x.Result)
                .ToArray()
                .AsQueryable();
        }
        public T Get(int key)
        {
            var divisor = key / _partitionSize;
            var quotient = key % _partitionSize;
            _handlers[quotient].Post(new GetItemRequest<T>(_innerData[quotient], divisor));
            var response = _handlers[quotient].Receive() as GetItemResponse<T>;
            if (response?.Error != null)
                throw response.Error;
            return response.Result;
        }
        public void Upsert(T value)
        {
            var divisor = value.Key / _partitionSize;
            var quotient = value.Key % _partitionSize;
            _handlers[quotient].Post(new UpsertItemRequest<T>(_innerData[quotient], divisor, value));
            var response = _handlers[quotient].Receive() as UpsertItemResponse<T>;
            if (response?.Error != null)
                throw response.Error;
        }
        public void Delete(int key)
        {
            var divisor = key / _partitionSize;
            var quotient = key % _partitionSize;
            _handlers[quotient].Post(new DeleteItemRequest<T>(_innerData[quotient], divisor));
            var response = _handlers[quotient].Receive() as DeleteItemResponse<T>;
            if (response?.Error != null)
                throw response.Error;
        }
        #endregion

        #region Advanced CRUD
        public IQueryable<T> Query(Predicate<T> filter)
        {
            ParallelEnumerable.Range(0, _partitionSize)
                .Select(n => _handlers[n].Post(new QueryRequest<T>(_innerData[n], filter)));
            var result = ParallelEnumerable.Range(0, _partitionSize).Select(n => _handlers[n].Receive() as QueryResponse<T>);
            if (result.Any(x => x.Error != null))
                throw new AggregateException(result.Where(x => x.Error != null).Select(x => x.Error));
            return result
                .SelectMany(x => x.Result)
                .ToArray()
                .AsQueryable();
        }
        public IQueryable<U> Query<U>(Predicate<T> filter, Func<T, U> transform) // Would be nice to have the transform baked in request, but Handle knows nothing about <U>'s
        {
            ParallelEnumerable.Range(0, _partitionSize)
                .Select(n => _handlers[n].Post(new QueryRequest<T>(_innerData[n], filter)));
            var result = ParallelEnumerable.Range(0, _partitionSize).Select(n => _handlers[n].Receive() as QueryResponse<T>);
            if (result.Any(x => x.Error != null))
                throw new AggregateException(result.Where(x => x.Error != null).Select(x => x.Error));
            return result
                .SelectMany(x => x.Result.Select(transform))
                .ToArray()
                .AsQueryable();
        }
        public void Modify(Action<T> handler, Predicate<T> filter = null)
        {
            ParallelEnumerable.Range(0, _partitionSize)
                .Select(n => _handlers[n].Post(new ModifyRequest<T>(_innerData[n], handler, filter)));
            var result = ParallelEnumerable.Range(0, _partitionSize).Select(n => _handlers[n].Receive() as ModifyResponse<T>);
            if (result.Any(x => x.Error != null))
                throw new AggregateException(result.Where(x => x.Error != null).Select(x => x.Error));
        }
        #endregion

        #region Handler

        private ResponseBase<T> Handle(RequestBase<T> request)
        {
            if (request is GetAllRequest<T>)
                return GetAll(request as GetAllRequest<T>);
            if (request is GetItemRequest<T>)
                return GetItem(request as GetItemRequest<T>);
            if (request is UpsertItemRequest<T>)
                return Upsert(request as UpsertItemRequest<T>);
            if (request is DeleteItemRequest<T>)
                return Delete(request as DeleteItemRequest<T>);
            if (request is QueryRequest<T>)
                return Query(request as QueryRequest<T>);
            if (request is ModifyRequest<T>)
                return Modify(request as ModifyRequest<T>);
            throw new ArgumentException($"Unknown request type {request.GetType().Name}", nameof(request));
        }

        private GetAllResponse<T> GetAll(GetAllRequest<T> getAllRequest)
        {
            try
            {
                return new GetAllResponse<T>(getAllRequest.Source.Select(x => x.Item).ToArray());
            }
            catch (Exception error)
            {
                return new GetAllResponse<T>(error);
            }
        }
        private GetItemResponse<T> GetItem(GetItemRequest<T> getItemRequest)
        {
            try
            {
                return new GetItemResponse<T>(getItemRequest.Source[getItemRequest.Key].Item);
            }
            catch (Exception error)
            {
                return new GetItemResponse<T>(error);
            }
        }
        private UpsertItemResponse<T> Upsert(UpsertItemRequest<T> upsertRequest)
        {
            try
            {
                var source = upsertRequest.Source;
                if (source.Length < upsertRequest.Key)
                    Array.Resize(ref source,
                        System.Math.Max(upsertRequest.Key, source.Length * 2));

                if (source[upsertRequest.Key] == null)
                    source[upsertRequest.Key] = new Record<T>(upsertRequest.Value);
                else source[upsertRequest.Key].Item = upsertRequest.Value;
                return new UpsertItemResponse<T>();
            }
            catch (Exception error)
            {
                return new UpsertItemResponse<T>(error);
            }
        }
        private DeleteItemResponse<T> Delete(DeleteItemRequest<T> deleteItemRequest)
        {
            try
            {
                if (deleteItemRequest.Source.Length < deleteItemRequest.Key)
                    deleteItemRequest.Source[deleteItemRequest.Key] = null;
                return new DeleteItemResponse<T>();
            }
            catch (Exception error)
            {
                return new DeleteItemResponse<T>(error);
            }
        }
        private QueryResponse<T> Query(QueryRequest<T> queryRequest)
        {
            try
            {
                return new QueryResponse<T>(queryRequest.Source
                    .Where(x => x != null && queryRequest.Filter(x.Item))
                    .Select(x => x.Item)
                    .ToArray());
            }
            catch (Exception error)
            {
                return new QueryResponse<T>(error);
            }
        }
        private ModifyResponse<T> Modify(ModifyRequest<T> modifyRequest)
        {
            try
            {
                if (modifyRequest.Filter == null)
                    foreach (var record in modifyRequest.Source)
                        modifyRequest.Handler(record.Item);
                else
                    foreach (var record in modifyRequest.Source.Where(x => modifyRequest.Filter(x.Item)))
                        modifyRequest.Handler(record.Item);
                return new ModifyResponse<T>();
            }
            catch (Exception error)
            {
                return new ModifyResponse<T>(error);
            }
        }
        #endregion*/
    }
}
