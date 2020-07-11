using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Student
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string From { get; set; }
        public double Credits { get; set; }
        public string Place { get; set; }
        public string StudentClassNo { get; set; }


        public Student()
        {

        }
        public Student(SqlDataReader reader)
        {
            No = reader[AppSettings.PropertyPrefix + "Sno" + AppSettings.Suffix].ToString().Trim();
            Name = reader[AppSettings.PropertyPrefix + "Sname" + AppSettings.Suffix].ToString().Trim();
            Sex = reader[AppSettings.PropertyPrefix + "Ssex" + AppSettings.Suffix].ToString().Trim();
            Age = Convert.ToInt32(reader[AppSettings.PropertyPrefix + "Sage" + AppSettings.Suffix].ToString());
            From = reader[AppSettings.PropertyPrefix + "Sfrom" + AppSettings.Suffix].ToString().Trim();
            if (reader[AppSettings.PropertyPrefix + "Scredits" + AppSettings.Suffix].ToString().Trim() != "")
                Credits = Convert.ToDouble(reader[AppSettings.PropertyPrefix + "Scredits" + AppSettings.Suffix].ToString().Trim());
            Place = reader[AppSettings.PropertyPrefix + "SPlace" + AppSettings.Suffix].ToString().Trim();
            StudentClassNo = reader[AppSettings.PropertyPrefix + "Sclass" + AppSettings.Suffix].ToString().Trim();
        }

        public static Student Get(string id)
        {
            Student st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Students{1} WHERE {2}Sno{3}= @Sno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (reader.Read())
                         st = new Student(reader);
                 });
            return st;
        }
        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}Students{1} WHERE {2}Sno{3}= @Sno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}Students{1} ({2}Sno{3},{2}Sname{3}, {2}Ssex{3},{2}Sage{3},{2}Sfrom{3},{2}SPlace{3},{2}Sclass{3})
                                    VALUES(@Sno,@Sname,@Ssex,@Sage,@Sfrom,@SPlace,@Sclass);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Sname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@Ssex", System.Data.SqlDbType.VarChar,Sex),
                    new SqlPrepareContent("@Sage", System.Data.SqlDbType.Int,Age),
                    new SqlPrepareContent("@Sfrom", System.Data.SqlDbType.VarChar,From),
                    new SqlPrepareContent("@SPlace", System.Data.SqlDbType.VarChar,Place),
                    new SqlPrepareContent("@Sclass", System.Data.SqlDbType.VarChar,StudentClassNo),

                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}Students{1} 
                SET  {2}Sname{3}=@Sname,
                     {2}Ssex{3}=@Ssex,
                     {2}Sage{3}=@Sage,
                     {2}Sfrom{3}=@Sfrom,
                     {2}SPlace{3}=@SPlace,
                     {2}Sclass{3}=@Sclass
                WHERE {2}Sno{3}= @Sno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Sname", System.Data.SqlDbType.VarChar,Name),
                    new SqlPrepareContent("@Ssex", System.Data.SqlDbType.VarChar,Sex),
                    new SqlPrepareContent("@Sage", System.Data.SqlDbType.Int,Age),
                    new SqlPrepareContent("@Sfrom", System.Data.SqlDbType.VarChar,From),
                    new SqlPrepareContent("@SPlace", System.Data.SqlDbType.VarChar,Place),
                    new SqlPrepareContent("@Sclass", System.Data.SqlDbType.VarChar,StudentClassNo),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public static List<Student> Gets(int pageIndex = 1, int pageSize = 100)
        {
            if (pageIndex == -1)
            {
                string queryString = String.Format(
            "SELECT * FROM dbo.{0}Students{1} order by cl_Sno01;",
            AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Student> StudentsList = new List<Student>();

                SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { },
                    (SqlDataReader reader) =>
                    {
                        while (reader.Read())
                            StudentsList.Add(new Student(reader));
                    });
                return StudentsList;
            }
            else
            {
                string queryString = String.Format(
             "SELECT * FROM dbo.{0}Students{1} order by cl_Sno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
             AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Student> StudentsList = new List<Student>();

                SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    StudentsList.Add(new Student(reader));
            });
                return StudentsList;
            }

        }


        public static int Count()
        {
            Student st = new Student();
            string queryString = String.Format(
              "SELECT count(*) FROM dbo.{0}Students{1};",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { }, (SqlDataReader reader) =>
             {
                 while (reader.Read())
                     result = Convert.ToInt32(reader[0]);
             });
            return result;
        }

        public Class GetStudentClass()
        {
            return Class.Get(StudentClassNo);
        }

        public List<Class> GetStudentCourse()
        {
            string queryString = String.Format(
                "SELECT * FROM dbo.{0}StudentCourse{1} WHERE {2}Sno{3}= @Sno;",
                AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Class> classList = new List<Class>();

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,No)
                 }, (SqlDataReader reader) =>
                 {
                     while (reader.Read())
                         classList.Add(new Class(reader));
                 });
            return classList;
        }

        public static List<Grade> GetGroupByYear(string sno)
        {

            string queryString = String.Format(
              "SELECT cl_year01,COUNT(cl_year01),AVG(cl_SCScore01) FROM [dbo].[chenl_Grade01] WHERE cl_Sno01=@Sno GROUP BY cl_year01",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<Grade> GradesList = new List<Grade>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                 new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,sno),
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new Grade(reader));
            });
            return GradesList;
        }

        public static List<FromCount> GetGroupByFrom()
        {

            string queryString = String.Format(
              "SELECT cl_Sfrom01 ,COUNT(cl_Sfrom01) count FROM chenl_Students01 GROUP BY cl_Sfrom01",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            List<FromCount> GradesList = new List<FromCount>();

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() {
                
            }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    GradesList.Add(new FromCount(reader));
            });
            return GradesList;
        }


    }
    public class FromCount
    {
       public string From { get; set; }
        public string Count { get; set; }
        public FromCount(SqlDataReader reader)
        {
           From =  reader[AppSettings.PropertyPrefix + "Sfrom" + AppSettings.Suffix].ToString().Trim();
            Count = reader["count"].ToString().Trim();
        }

    }
}
