using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Grade
    {
        public string Sno { get; set; }
        public string StuName { get; set; }
        public string CLno { get; set; }
        public string ClassName { get; set; }
        public string Tno { get; set; }
        public string TeacherName { get; set; }
        public string Score { get; set; }
        public string Year { get; set; }
        public string Credit { get; set; }
        public string Cno { get; set; }
        public string CourseName { get; set; }

        public Grade() { }
        public Grade(SqlDataReader reader)
        {
            Sno = reader[AppSettings.PropertyPrefix + "Sno" + AppSettings.Suffix].ToString().Trim();
            StuName = reader[AppSettings.PropertyPrefix + "Sname" + AppSettings.Suffix].ToString().Trim();
            ClassName = reader[AppSettings.PropertyPrefix + "CLname" + AppSettings.Suffix].ToString().Trim();
            TeacherName = reader[AppSettings.PropertyPrefix + "Tname" + AppSettings.Suffix].ToString().Trim();
            Score = reader[AppSettings.PropertyPrefix + "SCScore" + AppSettings.Suffix].ToString().Trim();
            Credit = reader[AppSettings.PropertyPrefix + "Ccredits" + AppSettings.Suffix].ToString().Trim();
            Year = reader[AppSettings.PropertyPrefix + "year" + AppSettings.Suffix].ToString().Trim();
            CourseName = reader[AppSettings.PropertyPrefix + "Cname" + AppSettings.Suffix].ToString().Trim();
        }

        public static List<Grade> GetGradesBySno(string sno,int pageIndex=1,int pageSize=100) {

            Student st = new Student();
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Grade{1} Where {2}Sno{3}=@Sno order by cl_Sno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Grade> GradesList = new List<Grade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,sno),
                    new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Grade(reader));
            });
            return GradesList;
        }



        public static List<Grade> GetGradesByTno(string tno, int pageIndex = 1, int pageSize = 100)
        {

            Student st = new Student();
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Grade{1} Where {2}Tno{1}=@Tno order by cl_Sno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Grade> GradesList = new List<Grade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,tno),
                    new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Grade(reader));
            });
            return GradesList;
        }

        public static List<Grade> GetGradesByCno(string cno, int pageIndex = 1, int pageSize = 100)
        {

            Student st = new Student();
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Grade{1} Where {2}Cno{3}=@Cno order by cl_Sno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Grade> GradesList = new List<Grade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,cno),
                    new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Grade(reader));
            });
            return GradesList;
        }

        public static string GetGradesAvgByCno(string cno)
        {

            string avg =null;
            string queryString = String.Format(
              "SELECT Avg(cl_SCScore01) FROM dbo.{0}Grade{1} Where {2}Cno{3}=@Cno And cl_SCScore01 is not null;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Grade> GradesList = new List<Grade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,cno),
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    avg = reader[0].ToString();
            });
            return avg;
        }

        public List<Grade> GetGradesByCLno(string clno, int pageIndex = 1, int pageSize = 100)
        {

            Student st = new Student();
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Grade{1} Where {2}CLno{1}=@CLno order by cl_Sno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Grade> GradesList = new List<Grade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,clno),
                    new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Grade(reader));
            });
            return GradesList;
        }

    }
}
