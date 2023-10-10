using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
//---------------------------//

namespace Sitio.Models
{
    public class UsuariosDB
    {
        public void Logueo(Usuario U)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("Logueo", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@UsuL", U.UsuLog);
            _comando.Parameters.AddWithValue("@PassL", U.PassLog);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (!_lector.HasRows)
                {
                    throw new Exception("Error - No es correcto el usuario y/o la contraseña");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
        }

        public Usuario BuscarUsuario(string pNomUsu)
        {
            string _nombre;
            string _pass;
            Usuario u = null;

            SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            SqlCommand oComando = new SqlCommand("Exec BuscoUsuario '" + pNomUsu + "'", oConexion);

            SqlDataReader oReader;

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.Read())
                {
                    _nombre = (string)oReader["Usulog"];
                    _pass = (string)oReader["PassLog"];
                    u = new Usuario(_nombre, _pass);
                }

                oReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }
            return u;
        }

    }
}