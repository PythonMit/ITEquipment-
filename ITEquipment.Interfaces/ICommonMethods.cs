using ITEquipment.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Interfaces
{
    public interface ICommonMethods
    {
        //Task<IEnumerable<ErrorLogInsertViewModel>> SaveErrorLog(ErrorLogModel errorLogModel);
        Task<Guid> InsertErrorLogs(Exception exception, string errorURL, string browserDetails, HttpContext context = null);
        Task HandleThrownErrorAsync(Exception ex, HttpContext context);
        int EmailSend(EmailFormModel model, List<string> attachmentFilesList = null);
        int SendEmailInThread(EmailFormModel objModel);
    }
}
