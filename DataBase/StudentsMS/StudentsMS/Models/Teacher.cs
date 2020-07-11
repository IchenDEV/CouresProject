using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Teacher
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }

        public Teacher() { }

        private Teacher(SqlDataReader reader)
        {
            No = reader[AppSettings.PropertyPrefix + "Tno" + AppSettings.Suffix].ToString().Trim();
            Name = reader[AppSettings.PropertyPrefix + "Tname" + AppSettings.Suffix].ToString().Trim();
            Sex = reader[AppSettings.PropertyPrefix + "Tsex" + AppSettings.Suffix].ToString().Trim();
            Age = Convert.ToInt32(reader[AppSettings.PropertyPrefix + "Tage" + AppSettings.Suffix].ToString());
            Title = reader[AppSettings.PropertyPrefix + "Ttitle" + AppSettings.Suffix].ToString().Trim();
            Phone = reader[AppSettings.PropertyPrefix + "Tphone" + AppSettings.Suffix].ToString().Trim();
        }
        public static Teacher Get(string id)
        {
            Teacher st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Teachers{1} WHERE {2}Tno{3}= @Tno; ",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (!reader.Read())
                     {
                         st = null;
                     }
                     else
                     {
                         st = new Teacher(reader);
                     }
                 });
            return st;
        }
        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}Teachers{1} WHERE {2}Tno{3}= @Tno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}Teachers{1} ({2}Tno{3},{2}Tname{3}, {2}Tsex{3},{2}Tage{3},{2}Ttitle{3},{2}Tphone{3})
                                    VALUES(@Tno,@Tname,@Tsex,@Tage,@Ttitle,@Tphone);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Tname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@Tsex", System.Data.SqlDbType.VarChar,Sex),
                    new SqlPrepareContent("@Tage", System.Data.SqlDbType.Int,Age),
                    new SqlPrepareContent("@Ttitle", System.Data.SqlDbType.VarChar,Title),
                    new SqlPrepareContent("@Tphone", System.Data.SqlDbType.VarChar,Phone),

                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}Teachers{1} 
                SET  {2}Tname{3}=@Tname,
                     {2}Tsex{3}=@Tsex,
                     {2}Tage{3}=@Tage,
                     {2}Ttitle{3}=@Ttitle,
                     {2}Tphone{3}=@Tphone
                WHERE {2}Tno{3}= @Tno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Tname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@Tsex", System.Data.SqlDbType.VarChar,Sex),
                    new SqlPrepareContent("@Tage", System.Data.SqlDbType.Int,Age),
                    new SqlPrepareContent("@Ttitle", System.Data.SqlDbType.VarChar,Title),
                    new SqlPrepareContent("@Tphone", System.Data.SqlDbType.VarChar,Phone),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public static List<Teacher> Gets(int pageIndex = 1, int pageSize = 100)
        {
            if (pageIndex == -1)
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.{0}Teachers{1} order by cl_Tno01 ",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Teacher> TeachersList = new List<Teacher>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() { },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              TeachersList.Add(new Teacher(reader));
                      });
                return TeachersList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.{0}Teachers{1} order by cl_Tno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Teacher> TeachersList = new List<Teacher>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                                     new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
                     },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              TeachersList.Add(new Teacher(reader));
                      });
                return TeachersList;
            }
        }

        public static int Count()
        {
            Student st = new Student();
            string queryString = String.Format(
              "SELECT count(*) FROM dbo.{0}Teachers{1};",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }




        public static List<Course> GetCourses(string Tno,int pageIndex = 1, int pageSize = 100)
        {
            if (pageIndex == -1)
            {
                string queryString = String.Format(
              "SELECT  * FROM dbo.{0}ClassCourse{1} JOIN {2}Courses{3} ON {0}ClassCourse{1}.{2}Cno{3} = {0}Courses{1}.{2}Cno{3} WHERE cl_Tno01=@Tno ",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Course> TeachersList = new List<Course>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                            new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,Tno),
                     },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              TeachersList.Add(new Course(reader));
                      });
                return TeachersList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT  * FROM {0}ClassCourse{1} JOIN {0}Courses{1} ON {0}ClassCourse{1}.{2}Cno{3} = {0}Courses{1}.{2}Cno{3} WHERE cl_Tno01=@Tno order by chenl_Courses01.cl_Cno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Course> TeachersList = new List<Course>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                                         new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,Tno),
                                     new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
                     },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              TeachersList.Add(new Course(reader));
                      });
                return TeachersList;
            }
        }

        public static int CountCourse(string Tno)
        {
            Student st = new Student();
            string queryString = String.Format(
              "SELECT count(DISTINCT cl_Cno01) FROM dbo.{0}ClassCourse{1} WHERE {2}Tno{3}=@Tno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar, Tno)}, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }









    }
}
