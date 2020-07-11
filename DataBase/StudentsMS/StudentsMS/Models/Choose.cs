using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Choose
    {
        public string SC { get; set; }
        public string Sno { get; set; }
        public string Cno { get; set; }
        public string Score { get; set; }

        public Choose()
        {

        }
        public Choose(SqlDataReader sqldata)
        {
            SC = sqldata[AppSettings.PropertyPrefix + "SC" + AppSettings.Suffix].ToString().Trim();
            Sno = sqldata[AppSettings.PropertyPrefix + "Sno" + AppSettings.Suffix].ToString().Trim();
            Cno = sqldata[AppSettings.PropertyPrefix + "Cno" + AppSettings.Suffix].ToString().Trim();
            Score = sqldata[AppSettings.PropertyPrefix + "SCScore" + AppSettings.Suffix].ToString().Trim();
        }


        public static Choose Get(string id)
        {
            Choose st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}StudentCourse{1} WHERE {2}SC{3}= @SC;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@SC", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (reader.Read())
                         st = new Choose(reader);
                 });
            return st;
        }
        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}StudentCourse{1} WHERE {2}SC{3}= @SC;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@SC", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}StudentCourse{1} ({2}Sno{3},{2}Cno{3},{2}SCScore{3})
                                    VALUES(@Sno,@Cno,@SCScore);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,Sno),
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,Cno),
                    new SqlPrepareContent("@SCScore", System.Data.SqlDbType.VarChar,Score),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}StudentCourse{1} 
                SET     {2}Sno{3}=@Sno,
                        {2}Cno{3}=@Cno,
                        {2}SCScore{3}=@SCScore
                WHERE {2}SC{3}= @SC;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Sno", System.Data.SqlDbType.VarChar,Sno),
                    new SqlPrepareContent("@Cno", System.Data.SqlDbType.VarChar,Cno),
                     new SqlPrepareContent("@SCScore", System.Data.SqlDbType.VarChar,Score),                 
                    new SqlPrepareContent("@SC", System.Data.SqlDbType.Int,Convert.ToInt32( SC)),
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
              "SELECT count(*) FROM dbo.{0}StudentCourse{1};",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            int result = 0;

            SqlHelper.SqlQueryPrepare(queryString, new List<SqlPrepareContent>() { }, (SqlDataReader reader) =>
            {
                while (reader.Read())
                    result = Convert.ToInt32(reader[0]);
            });
            return result;
        }
        public static List<Choose> Gets(int pageIndex = 1, int pageSize = 100)
        {

            if (pageIndex == -1)
            {
                string queryString = String.Format(
               "SELECT * FROM dbo.{0}StudentCourse{1} order by cl_Sno01;",
               AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Choose> ClassesList = new List<Choose>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() { },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Choose(reader));
                      });
                return ClassesList;
            }
            else
            {
                string queryString = String.Format(
              "SELECT * FROM dbo.{0}StudentCourse{1} order by cl_Sno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Choose> ClassesList = new List<Choose>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                       new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                        new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)},
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              ClassesList.Add(new Choose(reader));
                      });
                return ClassesList;
            }
        }

    }
}
