using System;

namespace DealCloud.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static int GenerateReferenceId(this Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            int referenceId;

            if (!exception.Data.Contains(Constants.ERROR_PARAMETER_REFERENCEID) || exception.Data[Constants.ERROR_PARAMETER_REFERENCEID] == null)
            {
                //reference id was not generated for this exception yet, we have to generate it
                referenceId = new Random().Next(10000, 100000);
                exception.Data[Constants.ERROR_PARAMETER_REFERENCEID] = referenceId;
            }
            else // reference id already exists
            {
                referenceId = Convert.ToInt32(exception.Data[Constants.ERROR_PARAMETER_REFERENCEID]);
            }

            return referenceId;
        }
    }
}
