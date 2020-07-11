using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class GroupGrade
    {
        public string Sno { get; set; }
        public string StuName { get; set; }
        public string Year { get; set; }
        public int Count { get; set; }
        public string ClassName { get; set; }
        public string AvgScore { get; set; }

        public GroupGrade(SqlDataReader reader) {

            //Sno = reader[AppSettings.PropertyPrefix + "Sno" + AppSettings.Suffix].ToString().Trim();
            //StuName = reader[AppSettings.PropertyPrefix + "Sname" + AppSettings.Suffix].ToString().Trim();
            //ClassName = reader[AppSettings.PropertyPrefix + "CLname" + AppSettings.Suffix].ToString().Trim();
            AvgScore = reader[AppSettings.PropertyPrefix + "AvgScore" + AppSettings.Suffix].ToString().Trim();
            Year = reader[AppSettings.PropertyPrefix + "year" + AppSettings.Suffix].ToString().Trim();
            Count = Convert.ToInt32(reader[AppSettings.PropertyPrefix + "count" + AppSettings.Suffix].ToString().Trim());
        }
        public static List<GroupGrade> GetGroupByYear(string sno)
        {

            string queryString = String.Format(
              "SELECT cl_year01 cl_year01,COUNT(cl_year01) cl_count01,AVG(cl_SCScore01) cl_AvgScore01 FROM [dbo].[chenl_Grade01] WHERE cl_Sno01=@Sno GROUP BY cl_year01",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<GroupGrade> GradesList = new List<GroupGrade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,sno),
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new GroupGrade(reader));
            });
            return GradesList;
        }

    }
}
