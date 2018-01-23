namespace DealCloud.Common.Extensions
{
    public static class IdExtentions
    {
        public static bool IsExistingId(this int id)
        {
            return id > 0;
        }

        public static int? ExistingIdOrNull(this int id)
        {
            return id.IsExistingId() ? id : (int?) null;
        }

        public static long Combine(this int id, int otherId)
        {
            return (((long)id) << 32) + otherId;
        }

        public static int GetUpperId(this long combined)
        {
            return (int)(combined >> 32);
        }
    }
}