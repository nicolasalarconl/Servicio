using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using CandyCrush.Modelos;


namespace servicio
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        //Conecta con la base de datos "candy"
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conectar = new MySqlConnection("server=127.0.0.1; database=candy; Uid=root; pwd=;");

            conectar.Open();
            return conectar;
        }

        //Crea una cuenta en la base de datos
        public static int AgregarCuenta(String userName, String password)
        {

            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("Insert into usuarios (user, password) values ('{0}','{1}')",
                userName, password), CompositeType.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();

            return retorno;
        }


        public static List<String> ObteneUsuario(String usuarios, String password)
        {
            List<String> data = new List<String>();
            MySqlConnection conexion = CompositeType.ObtenerConexion();

            MySqlCommand _comando = new MySqlCommand(String.Format("SELECT * FROM usuarios where user='" + usuarios + "' and password='" + password + "'"), conexion);
            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                data.Add(_reader.GetString(1));                 //Username
                data.Add(_reader.GetString(2));                 //Password
                data.Add(_reader.GetInt32(3).ToString());       //Score
                data.Add(_reader.GetInt32(0).ToString());       //ID

            }
            conexion.Close();
            return data;

        }

        public static List<String> ObteneUsuarioregistrado(String usuarios)
        {

            List<String> data = new List<String>();
            MySqlConnection conexion = CompositeType.ObtenerConexion();

            MySqlCommand _comando = new MySqlCommand(String.Format("SELECT * FROM usuarios where user='" + usuarios + "'"), conexion);
            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                data.Add(_reader.GetString(1));   //Username
            }
            conexion.Close();
            return data;

        }

        public static List<ModeloUsuario> BuscarRank()
        {
            int count = 0;
            List<ModeloUsuario> _lista = new List<ModeloUsuario>();
            MySqlCommand _comando = new MySqlCommand(String.Format(
           "SELECT user, score, password FROM usuarios order by score DESC"), ModeloConexion.ObtenerConexion());
            MySqlDataReader _reader = _comando.ExecuteReader();
            while (_reader.Read())
            {
                count += 1;
                ModeloUsuario pUsuario = new ModeloUsuario();

                pUsuario.Pos = count;
                pUsuario.UserName = _reader.GetString(0);
                pUsuario.Score = _reader.GetInt32(1);
                _lista.Add(pUsuario);

            }


            return _lista;
        }


        public static int ActualizaPuntaje(int id, int Score)
        {
            int retorno = 0;
            MySqlCommand comando = new MySqlCommand(String.Format("UPDATE usuarios SET score = " + Score + " where idusuario=" + id + ""), ModeloConexion.ObtenerConexion());
            retorno = comando.ExecuteNonQuery();

            return retorno;

        }
    }
}
