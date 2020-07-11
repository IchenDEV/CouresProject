using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Course
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Term { get; set; }
        public string Time { get; set; }
        public bool Exam { get; set; }
        public double Credits { get; set; }
        public Course() { }

        public Course(SqlDataReader reader)
        {

            No = reader[AppSettings.PropertyPrefix + "Cno" + AppSettings.Suffix].ToString().Trim();
            Name = reader[AppSettings.PropertyPrefix + "Cname" + AppSettings.Suffix].ToString().Trim();
            Term = reader[AppSettings.PropertyPrefix + "Cterm" + AppSettings.Suffix].ToString().Trim();
            Time = reader[AppSettings.PropertyPrefix + "Ctime" + AppSettings.Suffix].ToString().Trim();
            Exam = Convert.ToBoolean(reader[AppSettings.PropertyPrefix + "Cexam" + AppSettings.Suffix].ToString().Trim());
            Credits = Convert.ToDouble(reader[AppSettings.PropertyPrefix + "Ccredits" + AppSettings.Suffix].ToString().Trim());
        }


        public static Course Get(string id)
        {
            Course st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Courses{1} WHERE {2}Cno{3}= @Cno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (reader.Read())
                         st = new Course(reader);
                 });
            return st;
        }

        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}Courses{1} WHERE {2}Cno{3}= @Cno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}Courses{1} ({2}Cno{3},{2}Cname{3}, {2}Cterm{3},{2}Ctime{3},{2}Cexam{3},{2}Ccredits{3})
                                    VALUES(@Cno,@Cname,@Cterm,@Ctime,@Cexam,@Ccredits);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Cname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@Cterm", System.Data.SqlDbType.VarChar,Term),
                    new SqlPrepareContent("@Ctime", System.Data.SqlDbType.VarChar,Time),
                    new SqlPrepareContent("@Cexam", System.Data.SqlDbType.Bit,Exam),
                    new SqlPrepareContent("@Ccredits", System.Data.SqlDbType.Float,Credits),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}Courses{1} 
                SET  {2}Cname{3}=@Cname,
                     {2}Cterm{3}=@Cterm,
                     {2}Ctime{3}=@Ctime,
                     {2}Cexam{3}=@Cexam,
                     {2}Ccredits{3}=@Ccredits
                WHERE {2}Cno{3}= @Cno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Cname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@Cterm", System.Data.SqlDbType.VarChar,Term),
                    new SqlPrepareContent("@Ctime", System.Data.SqlDbType.VarChar,Time),
                    new SqlPrepareContent("@Cexam", System.Data.SqlDbType.Bit,Exam),
                    new SqlPrepareContent("@Ccredits", System.Data.SqlDbType.Float,Credits),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public static List<Course> Gets(int pageIndex = 1, int pageSize = 100)
        {
            if (pageIndex == -1)
            {
                string queryString = String.Format(
        "SELECT * FROM dbo.{0}Courses{1} order by cl_Cno01;",
        AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Course> CoursesList = new List<Course>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() { },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              CoursesList.Add(new Course(reader));
                      });
                return CoursesList;
            }
            else
            {
                string queryString = String.Format(
        "SELECT * FROM dbo.{0}Courses{1} order by cl_Cno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
        AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Course> CoursesList = new List<Course>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                                     new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
                     },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              CoursesList.Add(new Course(reader));
                      });
                return CoursesList;
            }

        }
        public static int Count()
        {
      
            string queryString = String.Format(
              "SELECT count(*) FROM dbo.{0}Courses{1};",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }
    }
}
