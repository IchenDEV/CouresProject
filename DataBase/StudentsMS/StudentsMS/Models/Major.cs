using StudentsMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class Major
    {
        public string No { get; set; }
        public string Name { get; set; }


        private Major(SqlDataReader reader)
        {
            No = reader[AppSettings.PropertyPrefix + "Mno" + AppSettings.Suffix].ToString().Trim();
            Name = reader[AppSettings.PropertyPrefix + "Mname" + AppSettings.Suffix].ToString().Trim();
        }
        public Major() { }


        public static Major Get(string id)
        {
            Major st = null;
            string queryString = String.Format(
              "SELECT * FROM dbo.{0}Majors{1} WHERE {2}Mno{3}= @Mno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            SqlHelper.SqlQueryPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Mno", System.Data.SqlDbType.VarChar,id)
                 }, (SqlDataReader reader) =>
                 {
                     if (reader.Read())
                         st = new Major(reader);
                 });
            return st;
        }
        public static bool Delete(string id)
        {
            string queryString = String.Format(
              "DELETE FROM {0}Majors{1} WHERE {2}Mno{3}= @Mno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Mno", System.Data.SqlDbType.VarChar,id)
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Insert()
        {
            string queryString = String.Format(
              @"INSERT INTO {0}Majors{1} ({2}Mno{3},{2}Mname{3})
                                    VALUES(@Mno,@Mname);",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Mno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Mname", System.Data.SqlDbType.VarChar,Name),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public bool Update()
        {
            string queryString = String.Format(
              @"Update {0}Majors{1} 
                SET  {2}Mname{3}=@Mname,
                WHERE {2}Mno{3}= @Mno;",
              AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

            var res = SqlHelper.SqlTPrepare(queryString,
                 new List<SqlPrepareContent>() {
                    new SqlPrepareContent("@Mno", System.Data.SqlDbType.VarChar,No),
                    new SqlPrepareContent("@Mname", System.Data.SqlDbType.VarChar,Name),
                 });
            if (res != 0)
                return true;
            else
                return false;
        }

        public static List<Major> Gets(int pageIndex = 1, int pageSize = 100)
        {
            if (pageIndex == -1)
            {
                string queryString = String.Format(
            "SELECT * FROM dbo.{0}Majors{1} order by cl_Mno01;;",
            AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Major> MajorsList = new List<Major>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() { },
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              MajorsList.Add(new Major(reader));
                      });
                return MajorsList;
            }
            else
            {
                string queryString = String.Format(
            "SELECT * FROM dbo.{0}Majors{1} order by cl_Mno01 offset ((@pageIndex-1)*@pageSize) rows fetch next @pageSize rows only;",
            AppSettings.TablePrefix, AppSettings.Suffix, AppSettings.PropertyPrefix, AppSettings.Suffix);

                List<Major> MajorsList = new List<Major>();

                SqlHelper.SqlQueryPrepare(queryString,
                     new List<SqlPrepareContent>() {
                                         new SqlPrepareContent("@pageIndex", System.Data.SqlDbType.Int,pageIndex),
                    new SqlPrepareContent("@pageSize", System.Data.SqlDbType.Int,pageSize)},
                      (SqlDataReader reader) =>
                      {
                          while (reader.Read())
                              MajorsList.Add(new Major(reader));
                      });
                return MajorsList;
            }

        }
        public static int Count()
        {
            Student st = new Student();
            string queryString = String.Format(
              "SELECT count(*) FROM dbo.{0}Majors{1};",
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
