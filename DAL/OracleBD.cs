using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace DataAccess
{
    public class OracleBD
    {

        private String mCadenaConexion;
        private OracleConnection mConection;

        public OracleBD(String cadenaCon)
        {
            this.mCadenaConexion = cadenaCon;
            this.mConection = new OracleConnection(this.mCadenaConexion);
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

        public DataSet fillDataSet(OracleCommand comando)
        {
            try
            {
                comando.Connection = this.mConection;
                OracleDataAdapter da = new OracleDataAdapter(comando);
                DataSet responseData = new DataSet();
                da.Fill(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                return buildError(ex.Message);
            }
        }

        public int executeUpdate(OracleCommand comando)
        {
            try
            {
                comando.Connection = this.mConection;
                comando.Connection.Open();
                int res = comando.ExecuteNonQuery();
                comando.Connection.Close();
                return res;
            }
            catch
            {
                return -1;
            }
        }
    }
}
