namespace DealCloud.Common.Validators
{
    public interface IValidator<in T>
    {
        bool Validate(T t);
    }
}
