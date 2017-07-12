using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RueHelper.util
{
    class RueSqlite
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //.table
        //.database

        //private static RueSqlite instance;
        private static SQLiteConnection m_dbConnection;

        private static DbProviderFactory factory = SQLiteFactory.Instance;
        private static DbConnection g_Conn = factory.CreateConnection();

        private static object _lock = new object();
        private static bool bInit = false;
        public void Close()
        {
            SQLiteConnection.ClearAllPools();
        }
        public RueSqlite()
        {
            string DataSource = Application.StartupPath +"\\RueSqlite.db";
            string ConnectionString = "Data Source=" + DataSource + ";Version=3;";

            if (!bInit)
            {
                if(!File.Exists(DataSource))
                {
                    //File.Create(DataSource);
                    SQLiteConnection.CreateFile(DataSource);//创建
                }
                m_dbConnection = new SQLiteConnection(ConnectionString);//连接
                m_dbConnection.Open();//打开


                // 连接数据库
                g_Conn.ConnectionString = ConnectionString;
                g_Conn.Open();

                createTable_User();
                createTable_Classes();
                createTable_Login();
                createTable_School();
                createTable_CourseTime();
                //createTable_Coursetable();
                bInit = true;
            }
        }
        //public static RueSqlite GetInstance()
        //{
        //    if (instance == null)
        //    {
        //        lock (_lock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new RueSqlite();
        //            }
        //        }
        //    }
        //    return instance;
        //}
        public User getTeacher(string account)
        {
            User u = null;
            string sql = "select * from edu_user where account='" + account+"' and type in(1,4)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                u = new User();
                u.account = account;

                u.name = reader["name"].ToString();
                u.id = Util.toInt(reader["id"].ToString());
                u.schoolid = Util.toInt(reader["schoolid"].ToString());

                u.phone = reader["phone"].ToString();
                u.pwd = reader["pwd"].ToString();

                u.courseid = Util.toInt(reader["courseid"].ToString());
                u.coursename = reader["coursename"].ToString();
            } 
            return u;
        }
        public User getUser(int id)
        {
            User u = null;
            string sql = "select * from edu_user where id='" + id;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                u = new User();
                u.id = id;
                u.account = reader["account"].ToString();

                u.name = reader["name"].ToString();
                u.id = Util.toInt(reader["id"].ToString());
                u.schoolid = Util.toInt(reader["schoolid"].ToString());

                u.phone = reader["phone"].ToString();
                u.pwd = reader["pwd"].ToString();

                u.courseid = Util.toInt(reader["courseid"].ToString());
                u.coursename = reader["coursename"].ToString();
            }
            return u;
        }
        public List<User> getStudentlist(string classid)
        {
            List<User> ulist = new List<User>();

            string sql = "select * from edu_user where classid='" + classid + "' and type=2";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User u = new User();

                u.name = reader["name"].ToString();
                u.account = reader["account"].ToString();
                u.id = Util.toInt(reader["id"].ToString());
                u.classid = Util.toInt(reader["classid"].ToString());
                u.schoolid = Util.toInt(reader["schoolid"].ToString());
                u.phone = reader["phone"].ToString();
                //u.classname = reader["classname"].ToString();
                string _seat = reader["seat"].ToString().Replace("-","");
                u.seat = Util.toInt(_seat) + "";
                u.pwd = "";// reader["pwd"].ToString();
                u.pinying = Util.GetPinyin(u.name);

                ulist.Add(u);
            }
            return ulist;
        }
        public int addUser(List<User> ulist)
        {
            int ret = 0;
            // 开始计时
            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            DbTransaction trans = m_dbConnection.BeginTransaction(); // <-------------------
            DbCommand cmd = m_dbConnection.CreateCommand();
            string sqlSchma = "insert into edu_user (id,account,phone,pwd,name,classid,schoolid,type,seat,classname,courseid,coursename,imageurl) values (P1,'P2','P3','P4','P5',P6,P7,P8,'P9','PA','PB','PC','PD')";

            try
            {
                foreach(User u in ulist)
                {
                    cmd.CommandText = "insert into [test1] ([s]) values (?)";

                    if(u.imageurl.Length > 0)
                    {
                        ;
                    }
                    // 创建数据表
                    string sql = sqlSchma;
                    sql = sql.Replace("P1", u.id + "").Replace("P2", u.account).Replace("P3", u.phone).Replace("P4", u.pwd).Replace("P5", u.name);
                    sql = sql.Replace("P6", u.classid + "").Replace("P7", u.schoolid + "").Replace("P8", u.type + "").Replace("P9", u.seat);
                    sql = sql.Replace("PA", "").Replace("PB", u.courseid + "").Replace("PC", u.coursename).Replace("PD", u.imageurl);

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    ret++;
                }
                trans.Commit(); // <-------------------
            }
            catch
            {
                trans.Rollback(); // <-------------------
                ret = 0;
                throw; // <-------------------
            }

            // 停止计时
            watch.Stop();
            return ret;
        }
        public int setSeat(int uid,string seat)
        {
            int ret = 0;
            // 开始计时
            Stopwatch watch = new Stopwatch();
            watch.Start();
            seat = seat.Replace("-", "");
            DbTransaction trans = m_dbConnection.BeginTransaction(); // <-------------------
            DbCommand cmd = m_dbConnection.CreateCommand();
            string sql = "update edu_user set seat='P1' where id=P2";
            sql = sql.Replace("P1", seat).Replace("P2", uid + "");
            try
            {
                cmd.CommandText = sql;
                ret = cmd.ExecuteNonQuery();
                trans.Commit(); // <-------------------
            }
            catch
            {
                trans.Rollback(); // <-------------------
                ret = 0;
                throw; // <-------------------
            }

            // 停止计时
            watch.Stop();
            return ret;
        }
        private int addUser(User u)
        {
            u.valid();

            int ret = 0;
            string sql = "insert into edu_user (id,account,phone,pwd,name,classid,schoolid,type,seat,classname,courseid,coursename,imageurl) values (P1,'P2','P3','P4','P5',P6,P7,P8,'P9','PA','PB','PC','PD')";
            sql = sql.Replace("P1", u.id + "").Replace("P2", u.account).Replace("P3", u.phone).Replace("P4", u.pwd).Replace("P5", u.name);
            sql = sql.Replace("P6", u.classid + "").Replace("P7", u.schoolid + "").Replace("P8", u.type + "").Replace("P9", u.seat);
            sql = sql.Replace("PA", "").Replace("PB", u.courseid + "").Replace("PC", u.coursename).Replace("PD", u.imageurl);
            try{
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql+", "+e.Message);
            }
            return ret;
        }
        public int delUser(int classid,int type)
        {
            int ret = 0;
            string sql = "delete from edu_user where classid="+classid+" and type="+type;
            try{
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql+", "+e.Message);
            }
            return ret;
        }
        public int delTeachers(int schoolid)
        {
            int ret = 0;
            string sql = "delete from edu_user where schoolid=" + schoolid + " and type in (1,4)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql+", "+e.Message);
            }
            return ret;
        }
        public int addClass(int classid,int grade,int orderid, string name,int schoolid)
        {
            int ret = 0;
            string sql = "insert into edu_class (id,grade,orderid,name,schoolid) values (P1,P2,P3,'P4',P5)";
            sql = sql.Replace("P1", classid + "").Replace("P2", grade + "").Replace("P3", orderid+"");
            sql = sql.Replace("P4", name).Replace("P5", schoolid + "");
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql+", "+e.Message);
            }
            return ret;
        }
        public int addClass(Classes c)
        {
            return addClass(c.id,c.grade,c.orderid,c.name,c.schoolid);
        }
        public Classes getClassById(int classid)
        {
            Classes obj = null;
            string sql = "select * from edu_class where id=" + classid;

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                obj = new Classes();
                obj.id = Util.toInt(reader["id"].ToString());//classid
                obj.name = reader["name"].ToString();

                Log.Info("getClassById...classid=" + obj.id + ", name=" + obj.name);

                obj.grade = Util.toInt(reader["grade"].ToString());
                obj.orderid = Util.toInt(reader["orderid"].ToString());
                obj.schoolid = Util.toInt(reader["schoolid"].ToString());
            }
            return obj;
        }
        public List<Classes> getClassBySchoolid(int schoolid)
        {
            List<Classes> list = new List<Classes>();
            string sql = "select * from edu_class where schoolid=" + schoolid+" order by grade,orderid";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Classes obj = new Classes();
                obj.id = Util.toInt(reader["id"].ToString());//classid
                obj.name = reader["name"].ToString();

                Log.Info("getClassBySchoolid...classid=" + obj.id + ", name=" + obj.name);

                obj.grade = Util.toInt(reader["grade"].ToString());
                obj.orderid = Util.toInt(reader["orderid"].ToString());
                obj.schoolid = Util.toInt(reader["schoolid"].ToString());
                list.Add(obj);
            }
            return list;
        }
        public int delClassBySchoolid(int schoolid)
        {
            int ret = 0;
            string sql = "delete from edu_class where schoolid=" + schoolid;
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql + ", " + e.Message);
            }
            return ret;
        }
        public int delSchool(int schoolid)
        {
            int ret = 0;
            string sql = "delete from edu_school where id=" + schoolid;
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql + ", " + e.Message);
            }
            return ret;
        }
        public int addSchool(int id, string name)
        {
            int ret = 0;
            string sql = "insert into edu_school (id,name) values (P1,'P2')";
            sql = sql.Replace("P1", id + "").Replace("P2", name + "");
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql + ", " + e.Message);
            }
            return ret;
        }
        public int addCourseTime(string week,string index,string timeon,string timeoff)
        {
            int ret = 0;
            string sql = "insert into edu_coursetime (week,i,timeon,timeoff) values (P1,P2,'P3','P4')";
            sql = sql.Replace("P1", week).Replace("P2", index);
            sql = sql.Replace("P3", timeon).Replace("P4", timeoff);
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Info(sql + ", " + e.Message);
            }
            return ret;
        }
        public int addCourseTime(string coursetimes)
        {
            string[] szCourseTime = coursetimes.Split(';');

            int ret = 0;
            // 开始计时
            Stopwatch watch = new Stopwatch();
            watch.Start();

            DbTransaction trans = m_dbConnection.BeginTransaction(); // <-------------------
            DbCommand cmd = m_dbConnection.CreateCommand();
            string sqlSchma = "insert into edu_coursetime (week,i,timeon,timeoff) values (P1,P2,'P3','P4')";

            try
            {
                foreach (string str in szCourseTime)
                {
                    string[] szItem = str.Split('-');
                    string week = szItem[0];
                    string index = szItem[1];
                    string timeon = szItem[2];
                    string timeoff = szItem[3];
                    string sql = sqlSchma;
                    sql = sql.Replace("P1", week).Replace("P2", index);
                    sql = sql.Replace("P3", timeon).Replace("P4", timeoff);

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    ret++;
                }
                trans.Commit(); // <-------------------
            }
            catch
            {
                trans.Rollback(); // <-------------------
                ret = 0;
                throw; // <-------------------
            }

            // 停止计时
            watch.Stop();
            return ret;
        }
        public string getCourseTimeOff()
        {
            string timeoff = "";
            string tmnow = DateTime.Now.ToString("HHmm");
            int week = (int)DateTime.Now.DayOfWeek;
            string sql = "select timeoff from edu_coursetime where week=" + week+" and timeon<'P1' and timeoff>'P1'";
            sql = sql.Replace("P1", tmnow);

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                timeoff = reader["timeoff"].ToString();
            }
            return timeoff;
        }
        public CourseTime getCourseTime(int index)
        {
            CourseTime ct = new CourseTime();
            string tmnow = DateTime.Now.ToString("HHmm");
            int week = (int)DateTime.Now.DayOfWeek;
            string sql = "select * from edu_coursetime where week=" + week + " and i="+index;
            sql = sql.Replace("P1", tmnow);

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                ct.week = week;
                ct.index = index;
                ct.timeOn = reader["timeon"].ToString();
                ct.timeOff = reader["timeoff"].ToString();
                return ct;
            }
            else
            {
                return null;
            }
        }
        public int delCourseTime()
        {
            int ret = 0;
            string sql = "delete from edu_coursetime";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
            return ret;
        }
        public string getSchoolname(int id)
        {
            string name = "";
            
            string sql = "select * from edu_school where id=" + id;

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                name = reader["name"].ToString();
            }
            return name;
        }

        public int setLastlogin(User u)
        {
            delLastlogin();

            int ret = 0;
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sql = "insert into edu_login (id,uptime) values ('P1','P2')";
            sql = sql.Replace("P1", u.id + "").Replace("P2", time);
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
            return ret;
        }
        public User getLastlogin()
        {
            User u = null;
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sql = "select * from edu_login";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int id = Util.toInt(reader["id"].ToString());
                    string uptime = reader["uptime"].ToString();
                    Log.Debug("getLastlogin...id=" + id + ", uptime=" + uptime);

                    u = this.getUser(id);
                }
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
            return u;
        }
        private int delLastlogin()
        {
            int ret = 0;
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sql = "delete from edu_login";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //在指定数据库中创建一个table
        private static void createTable_User()
        {
            string _sql = "select imageurl from edu_user limit 1";
            try
            {
                SQLiteCommand command = new SQLiteCommand(_sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                return;
            }
            catch (Exception e)
            {
                dropTable_User();
            }

            string sql = "create table edu_user (id int,account varchar(50) ,phone varchar(50),pwd varchar(50),name varchar(50),classid int,classname varchar(20),schoolid int,type int,seat varchar(10),courseid int, coursename varchar(20), imageurl varchar(256))";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                int ret = command.ExecuteNonQuery();
                Log.Debug(sql + ", ret=" + ret);
            }
            catch (Exception e)
            {
                Log.Error(sql+", "+e.Message);
            }

            string sql_index = "create index user_classid on edu_user(classid)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql_index, m_dbConnection);
                int ret = command.ExecuteNonQuery();
                Log.Debug(sql_index + ", ret=" + ret);
            }
            catch (Exception e)
            {
                Log.Error(sql_index + ", " + e.Message);
            }
            //create index user_classid on edu_user(classid);
            //查询表的索引： .indices edu_user
            //查询库的索引： SELECT * FROM sqlite_master WHERE type = 'index';
        }
        private static void dropTable_User()
        {
            string _sql = "drop table edu_user";
            int ret = 0;
            try
            {
                SQLiteCommand command = new SQLiteCommand(_sql, m_dbConnection);
                ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Debug(_sql + ", ret=" + ret+", e="+e.Message);
            }
        }

        private static void createTable_Classes()
        {
            string sql = "create table edu_class (id int,grade int,name varchar(50),schoolid int,orderid int)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                int ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
        }
        private static void createTable_Login()
        {
            string sql = "create table edu_login(id int,uptime varchar(50))";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                int ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
        }
        private static void createTable_School()
        {
            string sql = "create table edu_school(id int,name varchar(50))";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                int ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
        }
        private static void createTable_CourseTime()
        {
            string sql = "create table edu_coursetime(week int,i int,timeon varchar(50),timeoff varchar(50))";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                int ret = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Error(sql + ", " + e.Message);
            }
        }

        ////插入一些数据
        //private static void fillTable()
        //{
        //    string sql = "insert into highscores (name, score) values ('Me', 3000)";
        //    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        //    command.ExecuteNonQuery();

        //    sql = "insert into highscores (name, score) values ('Myself', 6000)";
        //    command = new SQLiteCommand(sql, m_dbConnection);
        //    command.ExecuteNonQuery();

        //    sql = "insert into highscores (name, score) values ('And I', 9001)";
        //    command = new SQLiteCommand(sql, m_dbConnection);
        //    command.ExecuteNonQuery();
        //}

        ////使用sql查询语句，并显示结果
        //private static void printHighscores()
        //{
        //    string sql = "select * from highscores order by score desc";
        //    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        //    SQLiteDataReader reader = command.ExecuteReader();
        //    while (reader.Read())
        //        Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
        //    Console.ReadLine();
        //}
    }
}
