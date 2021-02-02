using GBW.DataAccess;
using GBW.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GBW.Controllers
{
    public class BaseController : ApiController
    {
        private UnitOfWork unitOfWork;
        public ApplicationDbContext db;

        public BaseController()
        {
            db = new ApplicationDbContext();
        }

        public UnitOfWork UnitOfWork => unitOfWork ?? (unitOfWork = new UnitOfWork());


        private string ExecuteQuery(string query)
        {
            using (SqlConnection con = new SqlConnection(@DBGateway()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        ModelState.AddModelError("", e.Message);
                        return e.Message;

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return "success";
        }

        private string DBGateway()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["ApplicationDbContext"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            var conString = mySetting.ConnectionString;
            return conString;
        }
    }
}
