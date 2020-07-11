using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class ClassCourse
    {
        public string CC { get; set; }
        public string CLno { get; set; }
        public string Tno { get; set; }
        public string Cno { get; set; }
        public string Term { get; set; }
        public ClassCourse()
        {

        }
        public ClassCourse(SqlDataReader sqldata)
        {
            CC = sqldata[AppSettings.PropertyPrefix + "cc" + AppSettings.Suffix].ToString().Trim();
            CLno = sqldata[AppSettings.PropertyPrefix + "CLno" + AppSettings.Suffix].ToString().Trim();
            Cno = sqldata[AppSettings.PropertyPrefix + "Cno" + AppSettings.Suffix].ToString().Trim();
            Tno = sqldata[AppSettings.PropertyPrefix + "Tno" + AppSettings.Suffix].ToString().Trim();
            Term = sqldata[AppSettings.PropertyPrefix + "SCterm" + AppSettings.Suffix].ToString().Trim();
        }


        public static ClassCourse Get(string id)
        {
            ClassCourse st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}ClassCourse{1} WHERE {2}cc{3}= @cc;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@cc", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (reader.Read())
                         st = new ClassCourse(reader);
                 });
            return st;
        }
        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}ClassCourse{1} WHERE {2}cc{3}= @cc;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@cc", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}ClassCourse{1} ({2}CLno{3},{2}Tno{3},{2}Cno{3},{2}SCterm{3})
                                    VALUES(@CLno,@Tno,@Cno,@Term);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,CLno),
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,Cno),
                    new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,Tno),
                     new SqlPrepareContent("@Term", System.Data.SqlDbType.VarChar,Term),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}ClassCourse{1} 
                SET     {2}CLno{3}=@CLno,
                        {2}Tno{3}=@Tno,
                        {2}Cno{3}=@Cno,
                        {2}SCterm{3}=@Term
                WHERE {2}cc{3}= @cc;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                            new SqlPrepareContent("@CLno", System.Data.SqlDbType.VarChar,CLno),
                            new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,Cno),
                            new SqlPrepareContent("@Tno", System.Data.SqlDbType.VarChar,Tno),
                            new SqlPrepareContent("@cc", System.Data.SqlDbType.VarChar,CC),
                                   new SqlPrepareContent("@Term", System.Data.SqlDbType.VarChar,Term),
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
              "SELECT count(*) FROM dbo.{0}ClassCourse{1};",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }
        public static List<ClassCourse> Gets(int pageIndex = 1, int pageSize = 100)
        {

            if (pageIndex == -1)
            {
                string queryString = String.Format(
               "SELECT * FROM dbo.{0}ClassCourse{1} order by cl_cc01;",
               AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<ClassCourse> ClassesList = new List<ClassCourse>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() { },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new ClassCourse(reader));
                      });
                return ClassesList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.{0}ClassCourse{1} order by cl_cc01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<ClassCourse> ClassesList = new List<ClassCourse>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                       new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                        new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)},
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new ClassCourse(reader));
                      });
                return ClassesList;
            }
        }

    }
}
