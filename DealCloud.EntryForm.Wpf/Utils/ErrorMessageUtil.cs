using System;
using System.Linq;
using System.Runtime.InteropServices;
using DealCloud.AddIn.Common.Controls;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Extensions;

namespace DealCloud.AddIn.Excel.Utils
{
    public class ErrorMessageUtil : ErrorMessageBaseUtil
    {
        public static string Combine(params string[] errors)
        {
            return string.Join(",", errors?.Where(e => !e.IsNullOrEmpty()) ?? new string[0]);
        }

        public void ShowError(string error)
        {
            using (MessageErrorDialog dlg = new MessageErrorDialog() )
            {
                dlg.Message = error;
                dlg.ShowDialog();
            }
        }

        //public override ErrorMessage GetErrorMessage(Exception e)
        //{
        //    Exception error = ExtractFromAggregateException(e);
        //    COMException comException = error as COMException;
        //    if ((uint?)comException?.HResult == (uint)NativeMethods.HRESULT.EXCEL_E_SHIFT_CELLS)
        //    {
        //        return new ErrorMessage()
        //        {
        //            Message = "This operation is not allowed. The operation is attempting to shift cells in a table on your worksheet",
        //            Details = e.ToString()
        //        };
        //    }
        //    return base.GetErrorMessage(e);
        //}

        public override void LoginAction()
        {
           // Contexts.ExecuteCommandBroadCast(new LoginCommand(), false);
        }

        public override void LogoutAction()
        {
            //Contexts.ExecuteCommandBroadCast(new LogoutCommand(), false);
        }

        public override MessageErrorDialog CreateMessageErrorDialog()
        {
            return new MessageErrorDialog() { /*Icon = Resources.excel_m*/ };
        }

        public override MessageDialog CreateMessageDialog()
        {
            return new MessageDialog() { /*Icon = Resources.excel_m*/ };
        }

        public override bool IsLoogedIn => Model.ModelGlobal.Instance.IsLoggedIn;
    }
}
