namespace DealCloud.Common.Interfaces
{
    public interface IDeepClonable<out T>
    {
        T Clone();
    }
}