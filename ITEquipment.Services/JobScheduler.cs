using Hangfire;
using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Services
{
    public class JobScheduler
    {
        public string _Constring;
        private IConfiguration _configuration;
        private readonly ICommonMethods _commonMethods;
        private readonly IRecurringJobManager _recurringJobManager;
        public JobScheduler(IConfiguration configuration, ICommonMethods commonMethods, IRecurringJobManager recurringJobManager)
        {
            _Constring = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
            _commonMethods = commonMethods;
            _recurringJobManager = recurringJobManager;
        }
        public void ScheduleDailyEmailJob()
        {
            _recurringJobManager.AddOrUpdate("DailyEmailJob",  () => SendBookingEndEmailReminders(), Cron.Daily);
        }

        public async Task<int> SendBookingEndEmailReminders()
        {
            try
            {

                // BookingListDTOModel BookingList = new BookingListDTOModel();

                List<BookingDTOViewModel> BookingList = await GetRecords();
                EmailFormModel objEmail = new EmailFormModel();
                int mailSent = 0;
                foreach (var item in BookingList)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplate", "Overdue_Alert.html");
                    string emailContent = CommonMethodsService.ReadFileAsString(filePath);
                    var subject = "Equipment Return Reminder";
                    //var body = $"Dear {item.FirstName + ' ' + item.lastName},<br>Your Equipment booking for {item.Name} ended on {item.Enddate:yyyy-MM-dd}. " +
                    //           "Please return the equipment or extend the booking.<br>Thank you!";
                    objEmail.ToAddress = item.Email;
                    objEmail.Subject = subject;
                    objEmail.Message = emailContent;
                    objEmail.CcAddress = "mitesh.n.gandhi@tretainfotech.com";
                    objEmail.EquName = item.Name;
                    objEmail.UserName = item.FirstName + ' ' + item.lastName;
                    objEmail.StartDate = item.Startdate.ToString("d");
                    objEmail.EndDate = item.Enddate.ToString("d");
                    mailSent = _commonMethods.EmailSend(objEmail);
                }


                return mailSent;
            }
            catch (Exception)
            {

                throw;
            }

            return 0;

        }
        public async Task<List<BookingDTOViewModel>> GetRecords()
        {
            try
            {

                //BookingListDTOModel BookingList = new BookingListDTOModel();
                List<BookingDTOViewModel> BookingList = new List<BookingDTOViewModel>();

                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_OVERDUEBOOKINGTIGGER, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            BookingList = Util.DataReaderMapToList<BookingDTOViewModel>(dataReader);
                        }

                        con.Close();


                        return BookingList;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }



        }
        //public IActionResult SendBookingEndEmailReminders()
        //{

        //    return null;
        //}
    }
}
