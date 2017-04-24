namespace uClock
{
    using System;
    using System.Data;
    using System.Xml;
    using Newtonsoft.Json;
    using System.Data.SqlClient;

    /// <summary>
    /// Low level SQL queries for Value IT application
    /// </summary>
    public class SqlQuery
    {
        private DataTable dt = new DataTable();
        private string[] connection = new string[2];

        SocketSQL sql;
        SqlCommand com = new SqlCommand();
        Util tool = new Util();

        public SqlQuery()
        {
            connection = getConnection();
            sql = new SocketSQL(connection[0], int.Parse(connection[1]));
        }

        public string[] getConnection()
        {
            string[] connect = new string[2];

            XmlReader doc = XmlReader.Create("datasource.xml");
            doc.Read();
            doc.ReadStartElement("connection");

            doc.ReadStartElement("address");
            connect[0] = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadStartElement("port");
            connect[1] = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadEndElement();

            return connect;
        }

        private int getId(string table, int location)
        {
            DataTable dt = new DataTable();
            var enu = 0;

            try
            {
                string com = "SELECT * FROM @table ORDER BY id DESC";

                string[] param = new string[1];
                param[0] = table;
                string usr = this.sql.NetSQLCommand(3, com, param);

                dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

                string i = dt.Rows[0][location].ToString();
                enu = int.Parse(i);
            }
            catch (Exception ex)
            {
                enu = 1;
            }

            return enu;
        }

        public void ClockIn(string name)
        {
            Util time = new Util();
            string finDate = time.CurrDate();
            string finTime = time.CurrTime();

            string command = sql.NetSQLCommand("SELECT * FROM TimeClock ORDER BY number DESC LIMIT 1;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));
            string num = this.dt.Rows[0][7].ToString();
            int enu = int.Parse(num);
            enu++;

            string com2 = "INSERT INTO TimeClock(dt, employee, clockIn, number) Values('@finDate', '@name', '@finTime', @enu);";

            string[] param = new string[4];
            param[0] = finDate;
            param[1] = name;
            param[2] = finTime;
            param[3] = enu.ToString();

            this.sql.NetSQLCommand(6, com2, param);


        }

        public void ClockOut()
        {
            string command = this.sql.NetSQLCommand("SELECT * FROM timeclock ORDER BY number DESC LIMIT 1;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));
            string lastClock = this.dt.Rows[0][2].ToString();
            string lastC = this.dt.Rows[0][7].ToString();

            string com = "UPDATE TimeClock SET clockout=CURTIME() WHERE number = '@lastC';";
            this.sql.NetSQLCommand(com, lastC);

            string com2 = "UPDATE TimeClock SET totalHours = TIMEDIFF(CURTIME(),'@lastClock') WHERE number = '@lastC';";

            string[] param = new string[2];
            param[0] = lastClock;
            param[1] = lastC;

            this.sql.NetSQLCommand(4, com2, param);
        }

        public void CreateTask(string title, string description, string employee, string deadline)
        {
            int id = this.getId("projectschedule", 8);

            string cdt = this.tool.CurrDate();
            string com = "INSERT INTO ProjectSchedule (category, title, description, emp, scheduled, deadline, completed, id) VALUES('Task' , '@title', '@description', '@employee', '@cdt' , '@deadline' , false, @id);";

            string[] param = new string[6];
            param[0] = title;
            param[1] = description;
            param[2] = employee;
            param[3] = cdt;
            param[4] = deadline;
            param[5] = id.ToString();

            this.sql.NetSQLCommand(8, com, param);
        }

        public void CreateProject(string title, string description, string startDate)
        {
            int id = this.getId("projectschedule", 8);

            string cdt = this.tool.CurrDate();

            string com = "INSERT INTO ProjectSchedule (category, title, description, scheduled, deadline, completed, id) VALUES('Project', '@title', '@description' , '@cdt' , '@startDate' , false, @id);";

            string[] param = new string[5];
            param[0] = title;
            param[1] = description;
            param[2] = cdt;
            param[3] = startDate;
            param[4] = id.ToString();

            this.sql.NetSQLCommand(7, com, param);
        }

        public void AddEmployee(string name, string title, string address, string phoneNum, string email)
        {
            int id = this.getId("projectschedule", 9);

            string com = "INSERT INTO employeeData (empName, title, address, phoneNum, email, id) VALUES('@name', '@title', '@address', '@phoneNum' , '@email' , @id);";

            string[] param = new string[6];
            param[0] = name;
            param[1] = title;
            param[2] = address;
            param[3] = phoneNum;
            param[4] = email;
            param[5] = id.ToString();

            this.sql.NetSQLCommand(8, com, param);
        }

        public DataTable selectEmployee()
        {
            try
            {
                string usr = this.sql.NetSQLCommand("SELECT * FROM employeeData;");
                dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public bool useDb()
        {
            try
            {
                string command = sql.NetSQLCommand("USE valueitdatabase;");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }


        public DataTable Login(string user, string password)
        {

            try
            {
                string com = "SELECT * FROM Login WHERE user_name LIKE '@user' AND pwd LIKE '@password';";
                string[] param = new string[2];
                param[0] = user;
                param[1] = password;

                string command1 = this.sql.NetSQLCommand(4, com, param);

                this.dt = (DataTable)JsonConvert.DeserializeObject(command1, typeof(DataTable));

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
            }

            return this.dt;
        }

        public void NewUsr(string nnm, string usrnm, string pass, string lev)
        {
            string com = "INSERT INTO Login (name, user_name, pwd, level) VALUES('@nnm', '@usrnm', '@pass', @lev);";
            string[] param = new string[4];
            param[0] = nnm;
            param[1] = usrnm;
            param[2] = pass;
            param[3] = lev;

            this.sql.NetSQLCommand(6, com, param);
        }

        public void DeleteEmployee(string name)
        {
            try
            {
                string com = "DELETE FROM employeeData WHERE empName = '@name';";
                sql.NetSQLCommand(com, name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
            }
        }

        public DataTable SelectAllTime()
        {
            string command4 = this.sql.NetSQLCommand("SELECT * FROM (SELECT * FROM TimeClock ORDER BY number DESC LIMIT 0,7) r ORDER by clockin ASC;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command4, typeof(DataTable));

            return this.dt;
        }

        public DataTable SelectEmpTask(string emp)
        {
            string com = "SELECT * FROM ProjectSchedule WHERE emp = '@emp';";
            string usr = this.sql.NetSQLCommand(com, emp);

            dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            return dt;
        }

        public DataTable SelectLogin()
        {
            string usr = this.sql.NetSQLCommand("SELECT * FROM Login;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            return this.dt;
        }

        public DataTable SelectTask(string emp)
        {
            string com = "SELECT * FROM ProjectSchedule WHERE emp = '@emp'; ";
            string usr = this.sql.NetSQLCommand(com, emp);
            dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            return dt;
        }

        public DataTable SelectAllTask()
        {
            DataTable dt = new DataTable();
            string usr = this.sql.NetSQLCommand("SELECT * FROM ProjectSchedule WHERE category = 'Task';");
            dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            return dt;
        }

        public DataTable SelectAllProject()
        {
            DataTable dt = new DataTable();
            string usr = this.sql.NetSQLCommand("SELECT * FROM ProjectSchedule WHERE category = 'Project';");
            dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            return dt;
        }

        public DataTable SelectTime(string name)
        {
            string com = "SELECT * FROM (SELECT * FROM TimeClock WHERE employee= '@name' ORDER BY number DESC LIMIT 0,7) r ORDER by clockin ASC;";
            string command = this.sql.NetSQLCommand(com, name);

            this.dt = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));

            return this.dt;
        }

        public DataTable SelectTimeM()
        {
            string Month = DateTime.Now.Month.ToString();

            string com = "SELECT COUNT(*) FROM TimeClock WHERE Month(dt) = @Month;";
            string command = this.sql.NetSQLCommand(com, Month);

            DataTable dt2 = new DataTable();
            dt2 = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));
            int num = 0;
            string num1 = dt2.Rows[0][0].ToString();
            int.TryParse(num1, out num);

            com = "SELECT * FROM (SELECT * FROM TimeClock WHERE MONTH(dt) = @Month ORDER BY dt DESC LIMIT) sub ORDER by number ASC;";
            string command2 = this.sql.NetSQLCommand(com, Month);

            dt = (DataTable)JsonConvert.DeserializeObject(command2, typeof(DataTable));
            this.dt.Merge(dt);

            com = "SELECT * FROM (SELECT * FROM TimeClock WHERE MONTH(dt) = @Month ORDER BY dt DESC LIMIT 7 OFFSET 14) sub ORDER by number ASC;";
            string command3 = this.sql.NetSQLCommand(com, Month);

            dt = (DataTable)JsonConvert.DeserializeObject(command3, typeof(DataTable));
            this.dt.Merge(dt);

            return this.dt;
        }

        public DataTable SelectTimeY()
        {
            string year = DateTime.Now.Year.ToString();
            int i = 0;

            string com = "SELECT * FROM TimeClock WHERE MONTH(dt) = @year ORDER BY dt DESC LIMIT 7;";
            string command = this.sql.NetSQLCommand(com, year);

            dt = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));
            this.dt.Merge(dt);


            return this.dt;
        }

        public DataTable SqlShell(string command)
        {
            string usr = this.sql.NetSQLCommand(command);

            this.dt = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));

            return this.dt;
        }
    }
}