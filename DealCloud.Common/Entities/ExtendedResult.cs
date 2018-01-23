namespace DealCloud.Common.Entities
{
    public class ExtendedResult<T, TData>
    {
        public T Result { get; set; }

        public TData Data { get; set; }

        public ExtendedResult(T result, TData data)
        {
            Result = result;
            Data = data;
        }
    }
}