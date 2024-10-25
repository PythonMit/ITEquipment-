using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Services
{
    public class CommonMethodsService : ICommonMethods
    {
        public string _Constring;
        private IConfiguration _configuration;
        public CommonMethodsService(IConfiguration configuration)
        {
            _Constring = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        public async Task<Guid> InsertErrorLogs(Exception exception, string errorURL, string browserDetails, HttpContext context = null)
        {
            try
            {
               // var userSessionDetail = GetCurrentLoginUserDetailAsJson(context);
                Guid ErrorGuid = Guid.NewGuid();
                int result = 0;
                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GLOBAL_SAVEERRORLOG, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ErrorGUID", ErrorGuid);
                        cmd.Parameters.AddWithValue("@ErrorURL", errorURL);
                        cmd.Parameters.AddWithValue("@ErrorMessage", exception.Message);
                        cmd.Parameters.AddWithValue("@InnerException", exception?.InnerException != null? exception?.InnerException?.Message :null);
                        cmd.Parameters.AddWithValue("@StackTrace", exception?.StackTrace);
                        cmd.Parameters.AddWithValue("@TargetSite", exception.TargetSite?.Name);
                        cmd.Parameters.AddWithValue("@ErrorSource", exception.Source);
                        cmd.Parameters.AddWithValue("@BrowserDetails", browserDetails);
                        cmd.Parameters.AddWithValue("@CookiesDetails", (string?)null);
                        cmd.Parameters.AddWithValue("@IPAddress", IPConfig.GetClientLocalIPAddress());
                        cmd.Parameters.AddWithValue("@UserSession", "user");
                        //cmd.Parameters.AddWithValue("@CreatedDatetime", DateTime.UtcNow);

                        result = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                if (result > 0)
                {
                    return ErrorGuid;
                }
                return Guid.Empty;

                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new NotImplementedException();
        }
      
        public static string GetCurrentLoginUserDetailAsJson(HttpContext httpContext)
        {
            try
            {
                // Retrieve user details from the session
                UserMasterViewModel userDetail = httpContext.Session.GetObjectFromJson<UserMasterViewModel>(SessionKeys.LoginUserData);

                // Check if userDetail is null
                if (userDetail == null)
                {
                    // Handle the case where user details are not available
                    return "{}"; // or throw an exception, log, or return a default JSON object
                }

                // Create an anonymous object with UserId and Email properties
                var userJson = new
                {
                    UserId = userDetail.Id,
                    Email = userDetail.Email
                   // BusinessId = userDetail.BusinessId
                };

                // Convert the anonymous object to a JSON string
                string jsonString = JsonConvert.SerializeObject(userJson);

                return jsonString;
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, rethrow, return default value, etc.)
                // For simplicity, rethrow the exception in this example
                throw ex;
            }
        }

        public async Task HandleThrownErrorAsync(Exception ex, HttpContext context)
        {
            if (ex is UnauthorizedAccessException)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Add("X-Redirect", "/ErrorHandling/Unauthorized");
                context.Response.Redirect("/ErrorHandling/Unauthorized");
            }
            else
            {
                Guid res = await InsertErrorLogs(ex, context.Request.Path, context.Request.Headers["User-Agent"].ToString(), context);
                if (res != Guid.Empty)
                {
                    string recipientEmail = context.User.Identity.Name;
                    //bool emailSent = _emailServices.SendEmail(recipientEmail, "Guardian Error", res.ToString());
                }
                context.Response.StatusCode = 500;
                context.Response.Headers.Add("X-Redirect", "/ErrorHandling/TechnicalError");
                context.Response.Redirect("/ErrorHandling/TechnicalError");
            }
           // throw new NotImplementedException();
        }
        public static string ReadFileAsString(string path)
        {
            string result = "";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    using (StreamReader streamReader = new StreamReader(path))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                //throw;
            }
            return result;
        }

        public int SendEmailInThread(EmailFormModel objModel)
        {
            int result=1;
            new Thread(() =>
            {
              result=  EmailSend(objModel);
            }).Start();
            return result;
        }
        public int EmailSend(EmailFormModel model, List<string> attachmentFilesList = null) // async Task<int> , IEnumerable<dynamic> dynamicListTo=null
        {
            string _toAddress = String.Empty;
            string _ccAddress = String.Empty;
            string _bccAddress = String.Empty;

            _toAddress = _toAddress.TrimEnd(',');
            model.ToAddress = (!string.IsNullOrEmpty(model.ToAddress)) ? model.ToAddress : _toAddress;

            _ccAddress = _ccAddress.TrimEnd(',');
            model.CcAddress = (!string.IsNullOrEmpty(model.CcAddress)) ? model.CcAddress : _ccAddress;
           // model.CcAddress = "dhaval.d.kholiya@tretainfotech.com";

            _bccAddress = _bccAddress.TrimEnd(',');
            model.BccAddress = (!string.IsNullOrEmpty(model.BccAddress)) ? model.BccAddress : _bccAddress;
           // model.BccAddress = "dhaval.d.kholiya@tretainfotech.com";

            int _statusMail = 0;

            try
            {
                MailMessage message = new MailMessage();
                if (!string.IsNullOrEmpty(model.ToAddress))
                    message.To.Add(model.ToAddress.Trim());
                if (!string.IsNullOrEmpty(model.CcAddress))
                {
                    message.CC.Add(model.CcAddress.Trim());
                    //message.CC.Add("denish.r.patel@tretainfotech.com");
                }
                if (!string.IsNullOrEmpty(model.BccAddress))
                    message.Bcc.Add(model.BccAddress.Trim());
                message.Subject = model.Subject;
                message.IsBodyHtml = true;
             
               
                message.Body = model.Message.Replace("{URL}", _configuration["APIHostURL"]).Replace("{userName}", model.UserName).Replace("{UserName}", model.ToAddress).Replace("{EquipmentName}", model.EquName).Replace("{StartDate}", model.StartDate).Replace("{EndDate}", model.EndDate).Replace("{Status}", model.ReqType == "Approved" ? "Approval Status:" : "Reject Status:").Replace("{Comment}", !string.IsNullOrEmpty(model.Comment) ? model.Comment : (model.ReqType == "Approved" ? "Approved" : "Rejected")); ;
                //message.Body = "test Email";
               
                #region File Attachments 
                if (attachmentFilesList != null)
                {
                    foreach (string attachFile in attachmentFilesList)
                    {
                        if (!string.IsNullOrEmpty(attachFile))
                            message.Attachments.Add(new System.Net.Mail.Attachment(attachFile));
                    }
                }
                #endregion File Attachments
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;//Resolve some security related issue
                SmtpClient smtpClient = new SmtpClient();
                //smtpClient.UseDefaultCredentials = true;
                
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_configuration["AppSettings:LoginEmailId"], _configuration["AppSettings:LoginPassword"]);
                smtpClient.Host = _configuration["AppSettings:SMTP"];
                smtpClient.Port = Convert.ToInt32(_configuration["AppSettings:Port"]);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                message.From = new MailAddress(_configuration["AppSettings:FromEmailAddress"].Trim(), _configuration["AppSettings:'MailDisplayName"]);

                smtpClient.Send(message);
                _statusMail = 1;
            }
            catch (Exception ex)
            {
                //CommonMethods.ProcessLogException("EmailSend", ex);
                _statusMail = 0;
                throw;
            }
            return _statusMail;
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class IPConfig
    {
        public static string GetClientLocalIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string HostName = null;
            HostName = Environment.MachineName;
            Host = Dns.GetHostEntry(HostName);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = IP.ToString();
                }
            }
            return IPAddress;
        }

    }

}
