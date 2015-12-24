using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class SQLServerBD
    {

        private String mCadenaConexion;
        private SqlConnection mConection;

        public SQLServerBD(String cadenaCon)
        {
            this.mCadenaConexion = cadenaCon;
            this.mConection = new SqlConnection(this.mCadenaConexion);
        }

        private DataSet buildError(String error)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable tabla = new DataTable("ERROR");
                tabla.Columns.Add("Mensaje", typeof(string));
                DataRow fila = tabla.NewRow();
                fila["Mensaje"] = error;
                tabla.Rows.Add(fila);
                ds.Tables.Add(tabla);
            }
            catch { }
            return ds;
        }

        public DataSet fillDataSet(SqlCommand comando)
        {
            try
            {
                comando.Connection = this.mConection;
                SqlDataAdapter da = new SqlDataAdapter(comando);
                DataSet responseData = new DataSet();
                da.Fill(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                return buildError(ex.Message);
            }
        }

        public int executeUpdate(SqlCommand comando)
        {
            try
            {
                comando.Connection = this.mConection;
                if (comando.Connection.State != ConnectionState.Open) comando.Connection.Open();
                int res = comando.ExecuteNonQuery();
                comando.Connection.Close();
                return res;
            }
            catch 
            {
                if (comando.Connection.State != ConnectionState.Closed) comando.Connection.Close();
                return -1;
            }
        }
    }
}
