using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Class
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string MajorNo { get; set; }
        public Class()
        {

        }
        public Class(SqlDataReader sqldata)
        {
            No = sqldata[AppSettings.PropertyPrefix + "CLno" + AppSettings.Suffix].ToString().Trim();
            Name = sqldata[AppSettings.PropertyPrefix + "CLname" + AppSettings.Suffix].ToString().Trim();
            MajorNo = sqldata[AppSettings.PropertyPrefix + "CLmajor" + AppSettings.Suffix].ToString().Trim();
        }


        public static Class Get(string id)
        {
            Class st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Classes{1} WHERE {2}CLno{3}= @CLno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (reader.Read())
                         st = new Class(reader);
                 });
            return st;
        }
        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}Classes{1} WHERE {2}CLno{3}= @CLno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}Classes{1} ({2}CLno{3},{2}CLname{3},{2}CLmajor{3})
                                    VALUES(@CLno,@CLname,@CLmajor);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@CLname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@CLmajor", System.Data.SqlDbType.VarChar,MajorNo),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}Classes{1} 
                SET  {2}CLname{3}=@CLname,
                    {2}CLmajor{3}=@CLmajor
                WHERE {2}CLno{3}= @CLno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Clname", System.Data.SqlDbType.VarChar,Name),
                     new SqlPrepareContent("@CLmajor", System.Data.SqlDbType.VarChar,MajorNo),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public static int Count()
        {
            Student st = new Student();
            string queryString = String.Format(
              "SELECT count(*) FROM dbo.{0}Classes{1};",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }
        public static List<Class> Gets(int pageIndex = 1, int pageSize = 100)
        {

            if (pageIndex == -1)
            {
                string queryString = String.Format(
               "SELECT * FROM dbo.{0}Classes{1} order by cl_CLno01;",
               AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Class> ClassesList = new List<Class>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() { },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Class(reader));
                      });
                return ClassesList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.{0}Classes{1} order by cl_CLno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Class> ClassesList = new List<Class>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                       new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                        new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)},
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Class(reader));
                      });
                return ClassesList;
            }
        }


        public static int CountClassStudents(string CLno)
        {
            Student st = new Student();
            string queryString = String.Format(
              "SELECT count(*) FROM dbo.{0}Students{1} WHERE {2}Sclass{3} = @CLno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar, CLno) }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }
        public static List<Student> GetsClassStudents(string CLno, int pageIndex = 1, int pageSize = 100)
        {

            if (pageIndex <=0)
            {
                string queryString = String.Format(
               "SELECT * FROM dbo.{0}Students{1} order by cl_Sno01 WHERE {2}Sclass{3} = @CLno;",
               AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Student> ClassesList = new List<Student>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                      new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,CLno)
                     },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Student(reader));
                      });
                return ClassesList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.{0}Students{1}  WHERE {2}Sclass{3} = @CLno order by cl_Sno01  offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Student> ClassesList = new List<Student>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                       new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                        new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize),
                        new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar, CLno)},
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Student(reader));
                      });
                return ClassesList;
            }
        }




        public static int CountClassCourses(string CLno)
        {
            Student st = new Student();
            string queryString = String.Format(
              @"SELECT count(*) FROM dbo.chenl_Courses01 JOIN chenl_ClassCourse01 ON chenl_Courses01.cl_Cno01 = chenl_ClassCourse01.cl_Cno01 WHERE cl_CLno01 =@CLno",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar, CLno) }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }
        public static List<Course> GetsClassCourses(string CLno, int pageIndex = 1, int pageSize = 100)
        {

            if (pageIndex == -1)
            {
                string queryString = String.Format(
               "SELECT * FROM dbo.chenl_Courses01 JOIN chenl_ClassCourse01 ON chenl_Courses01.cl_Cno01 = chenl_ClassCourse01.cl_Cno01 WHERE cl_CLno01 =@CLno order by chenl_Courses01.cl_Cno01",
               AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Course> ClassesList = new List<Course>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                      new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,CLno)
                     },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Course(reader));
                      });
                return ClassesList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.chenl_Courses01 JOIN chenl_ClassCourse01 ON chenl_Courses01.cl_Cno01 = chenl_ClassCourse01.cl_Cno01 WHERE cl_CLno01 =@CLno order by chenl_Courses01.cl_Cno01  offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Course> ClassesList = new List<Course>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                       new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                        new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize),
                        new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar, CLno)},
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Course(reader));
                      });
                return ClassesList;
            }
        }
    }
}
