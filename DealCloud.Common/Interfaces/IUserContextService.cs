namespace DealCloud.Common.Interfaces
{
    public interface IUserContextService
    {
        /// <summary>
        ///     Get ClientId for currently logged in User
        /// </summary>
        int GetCurrentClientId();

        /// <summary>
        ///     Gets UserId for currently logged user
        /// </summary>
        int GetCurrentUserId();

        /// <summary>
        ///     Gets name for currently logged used
        /// </summary>
        string GetCurrentUserName();

        /// <summary>
        ///     Gets client name for currently logged in user
        /// </summary>
        string GetCurrentClientName(bool fullname = false);
    }
}