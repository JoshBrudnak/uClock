using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace uClock
{
    class SqlBuilder
    {
        private StringBuilder rq = new StringBuilder();
        private SqlCommand cmd = new SqlCommand();
        private int seq;

        public SqlBuilder Append(String str)
        {
            rq.Append(str);
            return this;
        }

        public SqlBuilder Value(Object value, SqlDbType type)
        {
            //get param name
            string paramName = "@SqlBuilderParam" + seq++;
            //append condition to query
            rq.Append(paramName);
            cmd.Parameters.Add(paramName, type).Value = value;
            return this;
        }
        public SqlBuilder FuzzyValue(Object value, SqlDbType type)
        {
            //get param name
            string paramName = "@SqlBuilderParam" + seq++;

            //append condition to query
            rq.Append("'%' + " + paramName + " + '%'");
            cmd.Parameters.Add(paramName, type).Value = value;
            return this;
        }

        public override string ToString()
        {
            return rq.ToString();
        }
    }
}
