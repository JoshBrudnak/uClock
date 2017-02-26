namespace ValueITapplication
{
    using System;
    using System.Data;
    using SystemWon;
    using System.Xml;
    using Newtonsoft.Json;

    public interface ISqlCommand
    {
        void ClockIn();

        void ClockOut();

        DataTable Login(string user, string password);

    }

    /// <summary>
    /// Low level SQL commands for Value IT application
    /// </summary>
    public class SqlCommand
    {
        private DataTable dt = new DataTable();
        
        private static string[] connection = getConnection();

        SocketSQL sql = new SocketSQL( connection[0], int.Parse(connection[1]));

        Util tool = new Util();

        public static string[] getConnection()
        {
            string[] connect = new string[2];

            XmlReader doc = XmlReader.Create("datasource.xml");
            doc.Read();
            doc.ReadStartElement("connection");

            doc.ReadStartElement("address");
            connect[0] = doc.ReadString();
            doc.ReadEndElement();

            doc.ReadStartElement("uid");
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
                string usr = this.sql.NetSQLCommand("SELECT * FROM " + table + " ORDER BY id DESC");
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
            string command = this.sql.NetSQLCommand("SELECT * FROM TimeClock ORDER BY number DESC LIMIT 1;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));
            string num = this.dt.Rows[0][7].ToString();
            int enu = int.Parse(num);
            enu++;
            string command2 = this.sql.NetSQLCommand( "INSERT INTO TimeClock(dt, employee, clockIn, number) Values('" + finDate + "', '" + name + "', '" + finTime + "', " + enu + ");");
        }

        public void ClockOut()
        {
            string command = this.sql.NetSQLCommand("SELECT * FROM timeclock ORDER BY number DESC LIMIT 1;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command, typeof(DataTable));
            string lastClock = this.dt.Rows[0][2].ToString();
            string lastC = this.dt.Rows[0][7].ToString();

            string command2 = this.sql.NetSQLCommand("UPDATE TimeClock SET clockout=CURTIME() WHERE number = '" + lastC + "';");

            string command3 = this.sql.NetSQLCommand("UPDATE TimeClock SET totalHours = TIMEDIFF(CURTIME(),'" + lastClock + "') WHERE number = '" + lastC + "';");
        }

        

        public void CreateTask(string title, string description, string employee, string deadline)
        {
            int id = this.getId("projectschedule", 8);

            string cdt = this.tool.CurrDate();
            string u =
                this.sql.NetSQLCommand("INSERT INTO ProjectSchedule (category, title, description, emp, scheduled, deadline, completed, id) VALUES('Task' , '"+ title + "', '" + description + "', '" + employee + "', '" + cdt + "' , '" + deadline + "' , false, " + id + ");");
        }

        public void CreateProject(string title, string description, string startDate)
        {
            int id = this.getId("projectschedule", 8);

            string cdt = this.tool.CurrDate();
            string u = this.sql.NetSQLCommand("INSERT INTO ProjectSchedule (category, title, description, scheduled, deadline, completed, id) VALUES('Project', '" + title + "', '" + description + "' , '" + cdt + "' , '" + startDate + "' , false, " + id + ");");
        }

        public void AddEmployee(string name, string title, string address, string phoneNum, string email)
        {
            int id = this.getId("projectschedule", 9);

            string u = this.sql.NetSQLCommand("INSERT INTO employeeData (empName, title, address, phoneNum, email, id) VALUES('" + name + "', '" + title + "', '" + address + "', '" + phoneNum + "' , '" + email+ "' , " + id + ");");
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }
        

        public DataTable Login(string user, string password)
        {

            try
            {
                string command1 =
                    this.sql.NetSQLCommand(
                        "SELECT * FROM Login WHERE user_name LIKE '" + user + "' AND pwd LIKE '" + password + "';");
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
            string usr =
                this.sql.NetSQLCommand(
                    "INSERT INTO Login (name, user_name, pwd, level) VALUES('" + nnm + "', '" + usrnm + "', '" + pass + "', " + lev + ");");
        }
        
        public void DeleteEmployee(string name)
        {
            try
            {
                this.sql.NetSQLCommand("DELETE FROM employeeData WHERE empName = '" + name + "';");

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
            }
        }
        
        public DataTable SelectAllTime()
        {
            string command4 =
                this.sql.NetSQLCommand(
                    "SELECT * FROM (SELECT * FROM TimeClock ORDER BY number DESC LIMIT 0,7) r ORDER by clockin ASC;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command4, typeof(DataTable));

            return this.dt;
        }

        public DataTable SelectEmpTask(string emp)
        {
            DataTable dt = new DataTable();
            string usr = this.sql.NetSQLCommand("SELECT * FROM ProjectSchedule WHERE emp = '" + emp + "'; ");
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
            DataTable dt = new DataTable();
            string usr = this.sql.NetSQLCommand("SELECT * FROM ProjectSchedule WHERE emp = '" + emp + "'; ");
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
            
            string command4 = this.sql.NetSQLCommand("SELECT * FROM (SELECT * FROM TimeClock WHERE employee= '" + name + "' ORDER BY number DESC LIMIT 0,7) r ORDER by clockin ASC;");
            this.dt = (DataTable)JsonConvert.DeserializeObject(command4, typeof(DataTable));

            return this.dt;
        }

        public DataTable SelectTimeM()
        {
            string Month = DateTime.Now.Month.ToString();
            int i = 0;
            int g = 7;
            
            string comm = this.sql.NetSQLCommand("SELECT COUNT(*) FROM TimeClock WHERE Month(dt) = " + Month + ";");

            DataTable dt2 = new DataTable();
            dt2 = (DataTable)JsonConvert.DeserializeObject(comm, typeof(DataTable));
            int num = 0;
            string num1 = dt2.Rows[0][0].ToString();
            int.TryParse(num1, out num);
            string usr1 = this.sql.NetSQLCommand("SELECT * FROM (SELECT * FROM TimeClock WHERE MONTH(dt) = " + Month + " ORDER BY dt DESC LIMIT) sub ORDER by number ASC;");

            dt = (DataTable)JsonConvert.DeserializeObject(usr1, typeof(DataTable));
            this.dt.Merge(dt);

            string usr2 =
                this.sql.NetSQLCommand(
                    "SELECT * FROM (SELECT * FROM TimeClock WHERE MONTH(dt) = " + Month
                    + " ORDER BY dt DESC LIMIT 7 OFFSET 14) sub ORDER by number ASC;");
            dt = (DataTable)JsonConvert.DeserializeObject(usr2, typeof(DataTable));
            this.dt.Merge(dt);

            return this.dt;
        }

        public DataTable SelectTimeY()
        {
            string year = DateTime.Now.Year.ToString();
            int i = 0;
            DataTable dt1 = new DataTable();
            string usr =
                this.sql.NetSQLCommand(
                    "SELECT * FROM TimeClock WHERE MONTH(dt) = " + year + " ORDER BY dt DESC LIMIT 7;");

            dt1 = (DataTable)JsonConvert.DeserializeObject(usr, typeof(DataTable));
            this.dt.Merge(dt1);

            /*
                        do
                        {
                            string usr = sql.NetSQLCommand("SELECT * FROM TimeClock WHERE MONTH(dt) = " + year + " ORDER BY dt DESC LIMIT " + i + "," + i + 7 + ";");
            
                            dt1 = (DataTable)JsonConvert.DeserializeObject(usr, (typeof(DataTable)));
                            dt.Merge(dt1);
                            i = i + 7;
                        }
                        while (i < 365);
                        */
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