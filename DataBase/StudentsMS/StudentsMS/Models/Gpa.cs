using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Gpa
    {
        public string Sno { get; set; }
        public string StuName { get; set; }

        public string Year { get; set; }
        public string Rank { get; set; }
        public string GPA { get; set; }


        public Gpa() { }
        public Gpa(SqlDataReader reader)
        {
            Sno = reader[AppSettings.PropertyPrefix + "Sno" + AppSettings.Suffix].ToString().Trim();
            StuName = reader[AppSettings.PropertyPrefix + "Sname" + AppSettings.Suffix].ToString().Trim();
            try
            {
                Year = reader[AppSettings.PropertyPrefix + "SCterm" + AppSettings.Suffix].ToString().Trim();
            }
            catch (Exception)
            {

              
            }
            
       
            try 
            {
                Rank = reader["rank"]?.ToString()?.Trim();
            }
            catch { }
            GPA = String.Format("{0:N2}",Convert.ToDouble( reader["GPA"]?.ToString()?.Trim()));
        }

        public static List<Gpa> GetGPA(string sno)
        {
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}GPA{1} Where {2}Sno{3}=@Sno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Gpa> GradesList = new List<Gpa>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,sno)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Gpa(reader));
            });
            return GradesList;
        }

        public static List<Gpa> GetGPAByCLno(string CLno)
        {
            string queryString = String.Format(
              "SELECT *,RANK () OVER ( ORDER BY GPA DESC) rank FROM dbo.chenl_GPAV01 Where cl_Sno01 in (SELECT cl_Sno01 FROM chenl_Students01 WHERE cl_Sclass01=@CLno)",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Gpa> GradesList = new List<Gpa>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,CLno)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Gpa(reader));
            });
            return GradesList;
        }




    }
}
