using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Data_Access
{
    public static class Conexion
    {
        private static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            DataSource = ".",
            InitialCatalog = "ProyectoDistribuidas",
            //UserID = "Julio",
            //Password = "123456",
            IntegratedSecurity = true, // Si usas UserID y Password, debe ser false
            MultipleActiveResultSets = true
        };

        public static string Connect = builder.ConnectionString;
    }

    //----------------------------------------------USUARIOS
    public class DAC_Usuarios
    {
        public int acceso_usuario(string User, string Password)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                int Id_User = -1;
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Login_User]", Connection))
                    {
                        Cls_Usuarios Usuario = new Cls_Usuarios { Password = Password };
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", User);
                        cmd.Parameters.AddWithValue("@Password_Hash", Usuario.Password_Hash());

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows && dr.Read())
                            {
                                Id_User = int.Parse(dr["Id_Usuario"].ToString()); ;
                            }
                        }
                    }

                    return Id_User;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("bloqueado")) return -2;
                    return 0;
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void insertar_usuario(Cls_Usuarios User)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Usuario]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", User.Nombre);
                        cmd.Parameters.AddWithValue("@Rol", User.Rol);
                        cmd.Parameters.AddWithValue("@Password_Hash", User.Password_Hash());
                        cmd.Parameters.AddWithValue("@Email", User.Email);
                        cmd.Parameters.AddWithValue("@Telefono", User.Telefono);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar Usuario.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void actualizar_usuario(Cls_Usuarios User)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Usuario]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Usuario", User.Id_Usuario);
                        cmd.Parameters.AddWithValue("@Nombre", User.Nombre);
                        cmd.Parameters.AddWithValue("@Rol", User.Rol);
                        cmd.Parameters.Add("@Password_Hash", SqlDbType.VarChar).Value = string.IsNullOrWhiteSpace(User.Password)? (object)DBNull.Value: User.Password_Hash();
                        cmd.Parameters.AddWithValue("@Email", User.Email);
                        cmd.Parameters.AddWithValue("@Telefono", User.Telefono);
                        cmd.Parameters.AddWithValue("@Activo", User.Activo);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();

                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar Usuario.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void eliminar_usuario(int Id_User)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Usuario]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Usuario", Id_User);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    // Si el SP hace RAISERROR, caes aquí
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al eliminar Usuario.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public List<Cls_Usuarios> listar_usuarios()
        {
            List<Cls_Usuarios> Lista = new List<Cls_Usuarios>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Usuarios]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Usuarios u = new Cls_Usuarios();
                                u.Id_Usuario = int.Parse(dr["Id_Usuario"].ToString());
                                u.Rol = dr["Rol"].ToString();
                                u.Nombre = dr["Nombre"].ToString();
                                u.Email = dr["Email"].ToString();
                                u.Telefono = dr["Telefono"].ToString();
                                u.Activo = Convert.ToInt32(dr["Activo"]) == 1;
                                u.Fecha_Registro = DateTime.Parse(dr["Fecha_Registro"].ToString());
                                Lista.Add(u);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }

            return Lista;
        }
        public Cls_Usuarios buscar_usuario(int Id_User)
        {
            Cls_Usuarios Usuario = null;

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Buscar_Usuario]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Usuario", Id_User);
                        cmd.CommandTimeout = 30; // Timeout de 30 segundos

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Usuario = new Cls_Usuarios();
                                
                                // Leer Id_Usuario
                                Usuario.Id_Usuario = dr["Id_Usuario"] != DBNull.Value 
                                    ? Convert.ToInt32(dr["Id_Usuario"]) 
                                    : 0;

                                // Leer Rol
                                Usuario.Rol = dr["Rol"] != DBNull.Value 
                                    ? dr["Rol"].ToString() 
                                    : string.Empty;

                                // Leer Nombre
                                Usuario.Nombre = dr["Nombre"] != DBNull.Value 
                                    ? dr["Nombre"].ToString() 
                                    : string.Empty;

                                // Leer Email
                                Usuario.Email = dr["Email"] != DBNull.Value 
                                    ? dr["Email"].ToString() 
                                    : string.Empty;

                                // Leer Telefono
                                Usuario.Telefono = dr["Telefono"] != DBNull.Value 
                                    ? dr["Telefono"].ToString() 
                                    : string.Empty;

                                // Leer Activo
                                if (dr["Activo"] != DBNull.Value)
                                {
                                    object activoObj = dr["Activo"];
                                    if (activoObj is bool)
                                    {
                                        Usuario.Activo = (bool)activoObj;
                                    }
                                    else
                                    {
                                        string activoStr = activoObj.ToString();
                                        Usuario.Activo = activoStr == "1" || activoStr.Equals("true", StringComparison.OrdinalIgnoreCase);
                                    }
                                }
                                else
                                {
                                    Usuario.Activo = false;
                                }

                                // Leer Fecha_Registro
                                Usuario.Fecha_Registro = dr["Fecha_Registro"] != DBNull.Value 
                                    ? Convert.ToDateTime(dr["Fecha_Registro"]) 
                                    : DateTime.MinValue;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Error SQL en buscar_usuario: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error en buscar_usuario: {ex.Message}");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }

                return Usuario;
            }
        }
        private void Enviar_Correo(string EmailUser, string token)
        {
            string remitente = "apptokenpruebarecuperacion642@gmail.com";
            string clave = "lxmsfxwjimatvjej"; // contraseña de aplicación

            try
            {
                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress(remitente, "Proyecto Aplicaciones Distribuidas");
                mensaje.To.Add(EmailUser);
                mensaje.Subject = "Recuperación de contraseña";
                mensaje.Body = $"Tu código de recuperación es: {token} \r\nPruebas de funcionamiento: envió del token de recuperación.";
                mensaje.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(remitente, clave);
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 10000; // 10 segundos

                    smtp.Send(mensaje);
                }
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception("Error al enviar el correo: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general en Enviar_Correo: " + ex.Message);
            }
        }
        public void enviar_token(string Email, string Usuario)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Token_User]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Usuario", Usuario);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows && dr.Read())
                            {
                                string token = dr["Token"]?.ToString();
                                if (!string.IsNullOrEmpty(token))
                                {
                                    Enviar_Correo(Email, token);
                                }
                                else
                                {
                                    throw new Exception("No se genero ningun token para el usuario.");
                                }
                            }
                            else
                            {
                                throw new Exception("Usuario o correo no encontrado.");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al enviar Token. : " + ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void actualizar_password(string Email, string token, string password)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Password_User]", Connection, transaction))
                    {
                        Cls_Usuarios Usuario = new Cls_Usuarios { Password = password };
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Token", token);
                        cmd.Parameters.AddWithValue("@Password_Hash", Usuario.Password_Hash());

                        cmd.ExecuteNonQuery();

                        transaction.Commit();

                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar contrasena.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }

        public bool verificar_estado_usuario(int userId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.Connect))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Activo FROM Usuarios WHERE Id_Usuario = @userId", connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string resStr = result.ToString();
                        return resStr == "1" || resStr.Equals("true", StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                }
            }
        }
    }
    //----------------------------------------------INSUMOS
    public class DAC_Insumos
    {
        public void insertar_insumo(Cls_Insumos Insumo, int Id_Usuario)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Insumo]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_usuario", Id_Usuario);
                        cmd.Parameters.AddWithValue("@Nombre", Insumo.Nombre);
                        cmd.Parameters.AddWithValue("@Unidad_Medida", Insumo.Unidad_Medida);
                        cmd.Parameters.AddWithValue("@Stock_Disponible", Insumo.Stock_Disponible);
                        cmd.Parameters.AddWithValue("@Stock_Minimo", Insumo.Stock_Minimo);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar el Insumo.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void actualizar_insumo(Cls_Insumos Insumo)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Insumo]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Insumo", Insumo.Id_Insumo);
                        cmd.Parameters.AddWithValue("@Nombre", Insumo.Nombre);
                        cmd.Parameters.AddWithValue("@Unidad_Medida", Insumo.Unidad_Medida);
                        cmd.Parameters.AddWithValue("@Stock_Minimo", Insumo.Stock_Minimo);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el Insumo.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void stock_insumo(List<Cls_Insumos> Insumos, int Id_Usuario)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Ajustar_Stock_Insumo]", Connection, transaction))
                    {
                        string json_Insumos = "[";
                        foreach (Cls_Insumos i in Insumos)
                        {
                            json_Insumos += i.Json_Insumo() + ",";
                        }
                        json_Insumos = json_Insumos.Substring(0, json_Insumos.Length - 1);
                        json_Insumos += "]";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_usuario", Id_Usuario);
                        cmd.Parameters.AddWithValue("@Json_Ajustes", json_Insumos);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al Actualizar el Stock de Insumos.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void eliminar_insumo(int Id_Insumo)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Insumo]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Insumo", Id_Insumo);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al eliminar registro de Insumo.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }

        public List<Cls_CardexInsumos> listar_cardex_insumo(int Id_Insumo)
        {
            List<Cls_CardexInsumos> lista = new List<Cls_CardexInsumos>();
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Listar_Cardex_Insumo]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Insumo", Id_Insumo);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_CardexInsumos c = new Cls_CardexInsumos();
                                c.Id_Insumo = int.Parse(dr["Id_Insumo"].ToString());
                                c.Tipo_Movimiento = dr["Tipo_Movimiento"].ToString();
                                c.Cantidad = float.Parse(dr["Cantidad"].ToString());
                                c.Motivo = dr["Motivo"].ToString();
                                lista.Add(c);
                            }
                        }
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            return lista;
        }

        public List<Cls_Insumos> listar_insumos()
        {
            List<Cls_Insumos> Lista = new List<Cls_Insumos>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Insumos]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Insumos i = new Cls_Insumos();
                                i.Id_Insumo = int.Parse(dr["Id_Insumo"].ToString());
                                i.Nombre = dr["Nombre"].ToString();
                                i.Unidad_Medida = dr["Unidad_Medida"].ToString();
                                i.Stock_Disponible = float.Parse(dr["Stock_Disponible"].ToString());
                                i.Stock_Minimo = float.Parse(dr["Stock_Minimo"].ToString());
                                Lista.Add(i);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }

            return Lista;
        }
        public Cls_Insumos buscar_insumo(int Id_Insumo)
        {
            Cls_Insumos insumo = new Cls_Insumos();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Buscar_Insumo]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Insumo", Id_Insumo);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                insumo.Id_Insumo = int.Parse(dr["Id_Insumo"].ToString());
                                insumo.Nombre = dr["Nombre"].ToString();
                                insumo.Unidad_Medida = dr["Unidad_Medida"].ToString();
                                insumo.Stock_Disponible = float.Parse(dr["Stock_Disponible"].ToString());
                                insumo.Stock_Minimo = float.Parse(dr["Stock_Minimo"].ToString());
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }

                return insumo;
            }
        }
    }

    //----------------------------------------------PLATOS
    public class DAC_Platos
    {
        public void insertar_plato(Cls_Platos Plato)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Plato]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", Plato.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", Plato.Descripcion);
                        cmd.Parameters.AddWithValue("@Precio", Plato.Precio);
                        cmd.Parameters.AddWithValue("@Tiempo_Preparacion", Plato.Tiempo_Preparacion);
                        cmd.Parameters.AddWithValue("@Activo", Plato.Activo);
                        cmd.Parameters.AddWithValue("@Json_Recetario", Plato.json_recetario());
                        cmd.Parameters.AddWithValue("@Direc_Imagen", Guardar_Imagen(Plato.Imagen,Plato.txt_Nombre_Imagen));

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar el Plato.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void actualizar_plato(Cls_Platos Plato)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Plato]", Connection, transaction))
                    {
                        string rutaImagen;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Plato", Plato.Id_Plato);
                        cmd.Parameters.AddWithValue("@Nombre", Plato.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", Plato.Descripcion);
                        cmd.Parameters.AddWithValue("@Precio", Plato.Precio);
                        cmd.Parameters.AddWithValue("@Tiempo_Preparacion", Plato.Tiempo_Preparacion);
                        cmd.Parameters.AddWithValue("@Activo", Plato.Activo);
                        cmd.Parameters.Add("@Version", SqlDbType.Binary, 8).Value = Plato.Version;
                        cmd.Parameters.AddWithValue("@Json_Recetario", Plato.json_recetario());
                        if (Plato.Imagen == null || Plato.Imagen.Length == 0)
                        {
                            rutaImagen = Plato.Direc_Imagen;
                        }
                        else
                        {
                            rutaImagen = Guardar_Imagen(Plato.Imagen, Plato.txt_Nombre_Imagen);
                        }

                        cmd.Parameters.AddWithValue("@Direc_Imagen", rutaImagen);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el registro del Plato."+ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void eliminar_plato(int Id_Plato)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Plato]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Plato", Id_Plato);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al eliminar el Plato.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public List<Cls_Platos> listar_platos()
        {
            List<Cls_Platos> Lista = new List<Cls_Platos>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Platos]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            string rutaProyecto = ObtenerRutaProyecto();
                            while (dr.Read())
                            {                               
                                Cls_Platos p = new Cls_Platos();
                                p.Id_Plato = int.Parse(dr["Id_Plato"].ToString());
                                p.Nombre = dr["Nombre"].ToString();
                                p.Descripcion = dr["Descripcion"].ToString();
                                p.Precio = float.Parse(dr["Precio"].ToString());
                                p.Activo = bool.Parse(dr["Activo"].ToString());
                                p.Tiempo_Preparacion = int.Parse(dr["Tiempo_Preparacion"].ToString());
                                p.Imagen = Obtener_Imagen(rutaProyecto, dr["Direc_Imagen"].ToString());
                                Lista.Add(p);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return Lista;
        }
        public Cls_Platos buscar_plato(int Id_Plato)
        {
            Cls_Platos Plato = new Cls_Platos();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Buscar_Plato]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Plato", Id_Plato);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Plato.Id_Plato = int.Parse(dr["Id_Plato"].ToString());
                                Plato.Nombre = dr["Nombre"].ToString();
                                Plato.Descripcion = dr["Descripcion"].ToString();
                                Plato.Precio = float.Parse(dr["Precio"].ToString());
                                Plato.Activo = bool.Parse(dr["Activo"].ToString());
                                Plato.Tiempo_Preparacion = int.Parse(dr["Tiempo_Preparacion"].ToString());
                                Plato.Version = (byte[])dr["Version"];

                                var direcBD = dr["Direc_Imagen"] == DBNull.Value ? null : dr["Direc_Imagen"].ToString();

                                // Si no hay imagen, evitar explotar
                                if (string.IsNullOrWhiteSpace(direcBD))
                                {
                                    Plato.txt_Nombre_Imagen = null;
                                    Plato.Imagen = null;
                                }
                                else
                                {
                                    Plato.txt_Nombre_Imagen = Path.GetFileName(direcBD);
                                    Plato.Imagen = Obtener_Imagen(ObtenerRutaProyecto(), direcBD);
                                    Plato.Direc_Imagen = dr["Direc_Imagen"].ToString();
                                }

                                List<Cls_Recetario> Recetario = new List<Cls_Recetario>();
                                if (dr.NextResult())
                                {
                                    while (dr.Read())
                                    {
                                        Cls_Recetario i = new Cls_Recetario();
                                        i.Id_Insumo = int.Parse(dr["Id_Insumo"].ToString());
                                        i.Cantidad_Necesaria = float.Parse(dr["Cantidad_Necesaria"].ToString());
                                        i.txt_Insumo = dr["Nombre"].ToString();
                                        i.txt_Unidad_Medida = dr["Unidad_Medida"].ToString();
                                        Recetario.Add(i);
                                    }
                                }

                                Plato.Recetario = Recetario;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return Plato;
        }
        public string Guardar_Imagen(byte[] imagen, string nombre)
        {
            if (imagen == null || imagen.Length == 0)
                throw new Exception("Archivo de imagen vacío.");

            try
            {
                string rutaProyecto = ObtenerRutaProyecto();
                string carpetaDestino = Path.Combine(rutaProyecto, "Imagenes");

                if (!Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);

                string nombreSinExt = Path.GetFileNameWithoutExtension(nombre);
                string extension = Path.GetExtension(nombre);

                // Ruta base del archivo
                string rutaArchivo = Path.Combine(carpetaDestino, nombre);

                //  Verificar si ya existe una imagen igual ----
                foreach (var archivoExistente in Directory.GetFiles(carpetaDestino))
                {
                    byte[] imagenExistente = File.ReadAllBytes(archivoExistente);

                    if (imagen.SequenceEqual(imagenExistente))
                    {
                        // devolver el nombre existente
                        return $"Imagenes\\{Path.GetFileName(archivoExistente)}";
                    }
                }

                int contador = 1;
                while (File.Exists(rutaArchivo))
                {
                    string nuevoNombre = $"{nombreSinExt}({contador}){extension}";
                    rutaArchivo = Path.Combine(carpetaDestino, nuevoNombre);
                    contador++;
                }

                // Guardar la nueva imagen
                File.WriteAllBytes(rutaArchivo, imagen);

                return $"Imagenes\\{Path.GetFileName(rutaArchivo)}";
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la imagen: " + ex.Message);
            }
        }
        private byte[] Obtener_Imagen(string rutaProyecto, string direcImagen)
        {
            try
            {
                string rutaArchivo = Path.Combine(rutaProyecto, direcImagen);
                if (rutaArchivo == null || !File.Exists(rutaArchivo)) { return null; }

                byte[] Imagen = File.ReadAllBytes(rutaArchivo);
                return Imagen;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la imagen: " + ex.Message);
            }
        }
        private string ObtenerRutaProyecto()
        {
            string nombreProyecto = "WCF_Services_Apl_Dis_2025_II";
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(basePath);

            while (dir != null && dir.Name != nombreProyecto)
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception($"No se encontró la carpeta del proyecto '{nombreProyecto}'.");

            return dir.FullName;
        }
        public int stock_disponible(int Plato)
        {
            int Stock_disponible = 0;
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Stock_Disponible]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Plato", Plato);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Stock_disponible = int.Parse(dr["Disponible"].ToString());
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }

                return Stock_disponible;
            }
        }
    }
    //----------------------------------------------PROMOCIONES
    public class DAC_Promociones
    {
        public void insertar_promocion(Cls_Promociones Promocion)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Promocion]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Plato", Promocion.Id_Plato);
                        cmd.Parameters.AddWithValue("@Nombre", Promocion.Nombre);
                        cmd.Parameters.AddWithValue("@Cantidad_Aplicable", Promocion.Cantidad_Aplicable);
                        cmd.Parameters.AddWithValue("@Descuento", Promocion.Descuento);
                        cmd.Parameters.AddWithValue("@Fecha_Inicio", Promocion.Fecha_Inicio);
                        cmd.Parameters.AddWithValue("@Fecha_Fin", Promocion.Fecha_Fin);
                        cmd.Parameters.AddWithValue("@Activo", Promocion.Activo);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar el Promocion.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void actualizar_promocion(Cls_Promociones Promocion)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Promocion]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Promocion", Promocion.Id_Promocion);
                        cmd.Parameters.AddWithValue("@Id_Plato", Promocion.Id_Plato);
                        cmd.Parameters.AddWithValue("@Nombre", Promocion.Nombre);
                        cmd.Parameters.AddWithValue("@Cantidad_Aplicable", Promocion.Cantidad_Aplicable);
                        cmd.Parameters.AddWithValue("@Descuento", Promocion.Descuento);
                        cmd.Parameters.AddWithValue("@Fecha_Inicio", Promocion.Fecha_Inicio);
                        cmd.Parameters.AddWithValue("@Fecha_Fin", Promocion.Fecha_Fin);
                        cmd.Parameters.AddWithValue("@Activo", Promocion.Activo);
                        cmd.Parameters.Add("@Version", SqlDbType.Binary, 8).Value = Promocion.Version;

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el registro de Promocion.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void eliminar_promocion(int Id_Promocion)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Promocion]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Promocion", Id_Promocion);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al eliminar el registro de Promocion.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public List<Cls_Promociones> listar_promociones(bool Listar_Todo)
        {
            List<Cls_Promociones> Lista = new List<Cls_Promociones>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Promociones]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Promociones i = new Cls_Promociones();
                                i.Id_Promocion = int.Parse(dr["Id_Promocion"].ToString());
                                i.Id_Plato = int.Parse(dr["Id_Plato"].ToString());
                                i.Nombre = dr["Nombre"].ToString();
                                i.Cantidad_Aplicable = int.Parse(dr["Cantidad_Aplicable"].ToString());
                                i.Descuento = float.Parse(dr["Descuento"].ToString());
                                i.Fecha_Inicio = DateTime.Parse(dr["Fecha_Inicio"].ToString());
                                i.Fecha_Fin = DateTime.Parse(dr["Fecha_Fin"].ToString());
                                i.Activo = bool.Parse(dr["Activo"].ToString());
                                Lista.Add(i);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }

            return Lista;
        }
        public List<Cls_Promocion_Plato> listar_promociones_platos()
        {
            List<Cls_Promocion_Plato> Lista = new List<Cls_Promocion_Plato>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Promociones_Platos_Listar]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }
                        string rutaProyecto = ObtenerRutaProyecto();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                
                                Cls_Promocion_Plato i = new Cls_Promocion_Plato();

                                i.Id_Promocion = Convert.ToInt32(dr["Id_Promocion"]);
                                i.Id_Plato = Convert.ToInt32(dr["Id_Plato"]);
                                i.PlatoNombre = dr["PlatoNombre"].ToString();
                                i.Descripcion = dr["Descripcion"].ToString();
                                i.PrecioNormal = float.Parse(dr["PrecioNormal"].ToString());
                                i.PrecioConDescuento = float.Parse(dr["PrecioConDescuento"].ToString());
                                i.PromocionNombre = dr["PromocionNombre"].ToString();
                                i.Cantidad_Aplicable = Convert.ToInt32(dr["Cantidad_Aplicable"]);
                                i.Descuento = float.Parse(dr["Descuento"].ToString());
                                i.Fecha_Inicio = Convert.ToDateTime(dr["Fecha_Inicio"]);
                                i.Fecha_Fin = Convert.ToDateTime(dr["Fecha_Fin"]);
                                i.Activo = Convert.ToInt32(dr["Activo"]);
                                i.Direc_Imagen = dr["Direc_Imagen"].ToString();
                                i.Imagen = Obtener_Imagen(rutaProyecto, dr["Direc_Imagen"].ToString());
                                Lista.Add(i);

                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }

            return Lista;
        }
        private byte[] Obtener_Imagen(string rutaProyecto, string direcImagen)
        {
            try
            {
                string rutaArchivo = Path.Combine(rutaProyecto, direcImagen);
                if (rutaArchivo == null || !File.Exists(rutaArchivo)) { return null; }

                byte[] Imagen = File.ReadAllBytes(rutaArchivo);
                return Imagen;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la imagen: " + ex.Message);
            }
        }
        private string ObtenerRutaProyecto()
        {
            string nombreProyecto = "WCF_Services_Apl_Dis_2025_II";
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(basePath);

            while (dir != null && dir.Name != nombreProyecto)
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception($"No se encontró la carpeta del proyecto '{nombreProyecto}'.");

            return dir.FullName;
        }

        public Cls_Promociones buscar_promocion(int Id_Promocion)
        {
            Cls_Promociones promocion = new Cls_Promociones();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Buscar_Promocion]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Promocion", Id_Promocion);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                promocion.Id_Promocion = int.Parse(dr["Id_Promocion"].ToString());
                                promocion.Id_Plato = int.Parse(dr["Id_Plato"].ToString());
                                promocion.Nombre = dr["Nombre"].ToString();
                                promocion.Cantidad_Aplicable = int.Parse(dr["Cantidad_Aplicable"].ToString());
                                promocion.Descuento = float.Parse(dr["Descuento"].ToString());
                                promocion.Fecha_Inicio = DateTime.Parse(dr["Fecha_Inicio"].ToString());
                                promocion.Fecha_Fin = DateTime.Parse(dr["Fecha_Fin"].ToString());
                                promocion.Activo = bool.Parse(dr["Activo"].ToString());
                                promocion.Version = (byte[])dr["Version"];
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return promocion;
        }
    }
    //----------------------------------------------VENTAS
    public class DAC_Ventas
    {
        public void insertar_venta(Cls_Ventas Venta)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Venta]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (string.IsNullOrWhiteSpace(Venta.Id_Trabajador.ToString()))
                        {
                            cmd.Parameters.AddWithValue("@Id_Trabajador", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Id_Trabajador", Venta.Id_Trabajador);
                        }
                        if (string.IsNullOrWhiteSpace(Venta.Id_Cliente.ToString()))
                        {
                            cmd.Parameters.AddWithValue("@Id_Cliente", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Id_Cliente", Venta.Id_Cliente);
                        }

                        cmd.Parameters.AddWithValue("@Json_Detalle", Venta.json_DetalleVenta());

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar la Venta.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public int insertar_venta_retorna_id(Cls_Ventas Venta)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;
                try
                {
                    if (Connection.State == ConnectionState.Closed)
                        Connection.Open();

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Venta_ReturnId]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Id_Trabajador", (object)Venta.Id_Trabajador ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Metodo_de_Pago", (object)Venta.Metodo_Pago ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id_Cliente", (object)Venta.Id_Cliente ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Monto_Total", (object)Venta.Monto_Total ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Json_Detalle", Venta.json_DetalleVenta());

                        SqlParameter paramOutput = new SqlParameter("@IdVentaGenerado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(paramOutput);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();

                        return Convert.ToInt32(paramOutput.Value);
                    }
                }
                catch (Exception)
                {
                    transaction?.Rollback();
                    throw;
                }
            }
        }
        public void actualizar_venta(Cls_Ventas Venta)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_DetalleVenta]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Venta", Venta.Id_Venta);
                        cmd.Parameters.AddWithValue("@Json_Detalle", Venta.json_DetalleVenta());
                        cmd.Parameters.Add("@Version", SqlDbType.Binary, 8).Value = Venta.Version;

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el registro de la Venta.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void estado_venta(Cls_Ventas Venta)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Estado_Venta]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Venta", Venta.Id_Venta);
                        cmd.Parameters.AddWithValue("@Estado", Venta.Estado);
                        cmd.Parameters.Add("@Version", SqlDbType.Binary, 8).Value = Venta.Version;

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el estado de la Venta.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public Cls_Ventas ObtenerEstadoVenta(int idVenta)
        {
            Cls_Ventas venta = null;

            using (SqlConnection con = new SqlConnection(Conexion.Connect))
            {
                SqlCommand cmd = new SqlCommand("Proc_Get_Estado_Venta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Venta", idVenta);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        venta = new Cls_Ventas
                        {
                            Id_Venta = (int)dr["Id_Venta"],
                            Estado = dr["Estado"].ToString(),
                            Fecha_Pedido = Convert.ToDateTime(dr["Fecha_Pedido"]),
                            Version = (byte[])dr["Version"]
                        };
                    }
                }
            }

            return venta;
        }
        public List<Cls_Ventas> listar_ventas(DateTime? Fecha, string Estado)
        {
            List<Cls_Ventas> Lista = new List<Cls_Ventas>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Ventas]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Fecha", Fecha.HasValue ? Fecha.Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(Estado) ? (object)DBNull.Value : Estado);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Ventas p = new Cls_Ventas();
                                p.Id_Venta = int.Parse(dr["Id_Venta"].ToString());
                                p.Id_Trabajador = dr["Id_Trabajador"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["Id_Trabajador"]);
                                p.Id_Cliente = dr["Id_Cliente"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["Id_Cliente"]);
                                p.Fecha_Pedido = DateTime.Parse(dr["Fecha_Pedido"].ToString());
                                p.Costo_Total = dr["Costo_Total"] == DBNull.Value ? 0 : float.Parse(dr["Costo_Total"].ToString());
                                p.Estado = dr["Estado"].ToString();
                                p.txt_Trabajador = dr["Nombre_Trabajador"].ToString();
                                p.txt_Cliente = dr["Nombre_Cliente"].ToString();
                                Lista.Add(p);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return Lista;
        }
        public Cls_Ventas buscar_venta(int Id_Venta)
        {
            Cls_Ventas venta = new Cls_Ventas();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Buscar_Venta]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Venta", Id_Venta);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                venta.Id_Venta = int.Parse(dr["Id_Venta"].ToString());
                                venta.Id_Trabajador = dr["Id_Trabajador"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["Id_Trabajador"]);
                                venta.Id_Cliente = dr["Id_Cliente"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["Id_Cliente"]);
                                venta.Fecha_Pedido = DateTime.Parse(dr["Fecha_Pedido"].ToString());
                                venta.Costo_Total = dr["Costo_Total"] == DBNull.Value ? 0 : float.Parse(dr["Costo_Total"].ToString());
                                venta.Estado = dr["Estado"].ToString();
                                venta.Version = (byte[])dr["Version"];
                                venta.txt_Trabajador = dr["Nombre_Trabajador"].ToString();
                                venta.txt_Cliente = dr["Nombre_Cliente"].ToString();

                                List<Cls_DetalleVenta> detalle = new List<Cls_DetalleVenta>();
                                if (dr.NextResult())
                                {
                                    while (dr.Read())
                                    {
                                        Cls_DetalleVenta i = new Cls_DetalleVenta();
                                        i.Id_Plato = int.Parse(dr["Id_Plato"].ToString());
                                        i.Precio_Unitario = float.Parse(dr["Precio_Unitario"].ToString());
                                        i.Cantidad = int.Parse(dr["Cantidad"].ToString());
                                        i.Descuento = dr["Descuento"] == DBNull.Value ? 0 : float.Parse(dr["Descuento"].ToString());
                                        i.txt_Plato = dr["Nombre"].ToString();
                                        i.Id_Promocion = dr["Id_Promocion"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["Id_Promocion"]);
                                        detalle.Add(i);
                                    }
                                }

                                venta.DetalleVenta = detalle;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return venta;
        }
        public int buscar_id_venta_activa(int Id_Cliente)
        {
            int idVentaEncontrada = 0; 
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == ConnectionState.Closed) Connection.Open();

                    using (SqlCommand cmd = new SqlCommand("Proc_Buscar_Venta_Activa_Por_Cliente", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            idVentaEncontrada = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar venta activa: " + ex.Message);
                }
            }
            return idVentaEncontrada;
        }
    }
    //----------------------------------------------COMPRAS
    public class DAC_Compras
    {
        public void insertar_compra(Cls_Compras Compra)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Compra]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Usuario", Compra.Id_Usuario);
                        cmd.Parameters.AddWithValue("@Json_Detalle", Compra.json_DetalleCompras());

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar la Compra.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void actualizar_compra(Cls_Compras Compra)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_DetalleCompra]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Compra.Id_Compra);
                        cmd.Parameters.AddWithValue("@Json_Detalle", Compra.json_DetalleCompras());
                        cmd.Parameters.Add("@Version", SqlDbType.Binary, 8).Value = Compra.Version;

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el registro de la Compra.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void estado_compra(Cls_Compras Compra)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;

                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Update_Estado_Compra]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Compra.Id_Compra);
                        cmd.Parameters.AddWithValue("@Estado", Compra.Estado);
                        cmd.Parameters.Add("@Version", SqlDbType.Binary, 8).Value = Compra.Version;

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al actualizar el estado de la Compra.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public List<Cls_Compras> listar_compras()
        {
            List<Cls_Compras> Lista = new List<Cls_Compras>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Compras]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Compras p = new Cls_Compras();
                                p.Id_Compra = int.Parse(dr["Id_Compra"].ToString());
                                p.Id_Usuario = int.Parse(dr["Id_Usuario"].ToString());
                                p.Fecha_Compra = DateTime.Parse(dr["Fecha_Compra"].ToString());
                                p.Estado = dr["Estado"].ToString();
                                p.Costo_Total = dr["Costo_Total"] == DBNull.Value ? 0 : float.Parse(dr["Costo_Total"].ToString());
                                p.txt_Usuario = dr["Nombre"].ToString();
                                Lista.Add(p);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return Lista;
        }
        public Cls_Compras buscar_compra(int Id_Compra)
        {
            Cls_Compras compra = new Cls_Compras();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Buscar_Compra]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Id_Compra);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                compra.Id_Compra = int.Parse(dr["Id_Compra"].ToString());
                                compra.Id_Usuario = int.Parse(dr["Id_Usuario"].ToString());
                                compra.Fecha_Compra = DateTime.Parse(dr["Fecha_Compra"].ToString());
                                compra.Estado = dr["Estado"].ToString();
                                compra.Costo_Total = dr["Costo_Total"] == DBNull.Value ? 0 : float.Parse(dr["Costo_Total"].ToString()); ;
                                compra.Version = (byte[])dr["Version"];
                                compra.txt_Usuario = dr["Nombre"].ToString();

                                List<Cls_DetalleCompras> detalle = new List<Cls_DetalleCompras>();
                                if (dr.NextResult())
                                {
                                    while (dr.Read())
                                    {
                                        Cls_DetalleCompras i = new Cls_DetalleCompras();
                                        i.Id_Insumo = int.Parse(dr["Id_Insumo"].ToString());
                                        i.Costo_Unitario = float.Parse(dr["Costo_Unitario"].ToString());
                                        i.Cantidad = float.Parse(dr["Cantidad"].ToString());
                                        i.txt_Insumo = dr["Nombre"].ToString();
                                        i.txt_Unidad_Medida = dr["Unidad_Medida"].ToString();
                                        detalle.Add(i);
                                    }
                                }

                                compra.DetalleCompras = detalle;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
            return compra;
        }

        public void eliminar_compra(int Id_Compra)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Compra]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Id_Compra);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
        }

        public List<Cls_DetalleCompras> listar_detalles_compra(int Id_Compra)
        {
            List<Cls_DetalleCompras> lista = new List<Cls_DetalleCompras>();
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Listar_Detalle_Compra]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Id_Compra);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_DetalleCompras i = new Cls_DetalleCompras();
                                i.Id_Insumo = int.Parse(dr["Id_Insumo"].ToString());
                                i.Costo_Unitario = float.Parse(dr["Costo_Unitario"].ToString());
                                i.Cantidad = float.Parse(dr["Cantidad"].ToString());
                                i.txt_Insumo = dr["Nombre"].ToString();
                                // i.txt_Unidad_Medida = dr["Unidad_Medida"].ToString();
                                lista.Add(i);
                            }
                        }
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            return lista;
        }

        public void eliminar_detalles_compra(int Id_Compra)
        {
             using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Detalle_Compra_All]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Id_Compra);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
        }

        public void insertar_detalle_compra(Cls_DetalleCompras Detalle)
        {
             using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Detalle_Compra_Individual]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Compra", Detalle.Id_Compra);
                        cmd.Parameters.AddWithValue("@Id_Insumo", Detalle.Id_Insumo);
                        cmd.Parameters.AddWithValue("@Costo_Unitario", Detalle.Costo_Unitario);
                        cmd.Parameters.AddWithValue("@Cantidad", Detalle.Cantidad);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
        }
    }
    //----------------------------------------------COMENTARIOS
    public class DAC_Comentarios
    {
        public void insertar_comentario(Cls_Comentarios Comentario)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Insert_Comentario]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Venta", Comentario.Id_Venta);
                        cmd.Parameters.AddWithValue("@Id_Usuario", Comentario.Id_Usuario);
                        cmd.Parameters.AddWithValue("@Comentario", Comentario.Comentario);
                        cmd.Parameters.AddWithValue("@tipo", Comentario.tipo);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al registrar el Comentario.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public void eliminar_comentario(int Id_Comentario)
        {
            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                SqlTransaction transaction = null;
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                    transaction = Connection.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Delete_Comentario]", Connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_Comentario", Id_Comentario);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    if (transaction != null) { transaction.Rollback(); }
                    throw new Exception("Error al eliminar el registro de Comentario.");
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }
        }
        public List<Cls_Comentarios> listar_comentarios(string Contexto,int Id_Relacionado)
        {
            List<Cls_Comentarios> Lista = new List<Cls_Comentarios>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Comentarios]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Contexto", Contexto);
                        cmd.Parameters.AddWithValue("@Id_Relacionado", Id_Relacionado);

                        if (Connection.State == System.Data.ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Comentarios u = new Cls_Comentarios();
                                u.Id_Comentario = int.Parse(dr["Id_Comentario"].ToString());
                                u.txt_Usuario = dr["Usuario"].ToString();
                                u.Fecha_Comentario = DateTime.Parse(dr["Fecha_Comentario"].ToString());
                                u.Comentario = dr["Comentario"].ToString();
                                Lista.Add(u);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open) { Connection.Close(); }
                }
            }

            return Lista;
        }

        public List<Cls_Comentarios> listar_todos_comentarios()
        {
            List<Cls_Comentarios> Lista = new List<Cls_Comentarios>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    // Llamamos al procedimiento que ya filtra solo 'VENTA'
                    using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_Lista_Todos_Comentarios]", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (Connection.State == ConnectionState.Closed) { Connection.Open(); }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Cls_Comentarios c = new Cls_Comentarios();

                                // 1. Mapeo de datos básicos
                                c.Id_Comentario = Convert.ToInt32(dr["Id_Comentario"]);
                                c.Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]);
                                c.txt_Usuario = dr["Usuario"].ToString(); // Nombre del usuario
                                c.Comentario = dr["Comentario"].ToString();
                                c.Fecha_Comentario = Convert.ToDateTime(dr["Fecha_Comentario"]);

                                // 2. Mapeo de datos de Venta
                                // Como tu tabla tiene Id_Venta NOT NULL, siempre vendrá dato
                                c.Id_Venta = Convert.ToInt32(dr["Id_Venta"]);

                                // Asignamos Id_Venta a Id_Relacionado para compatibilidad con tu vista
                                c.Id_Relacionado = (int)c.Id_Venta;

                                // 3. Contexto
                                if (dr["Tipo"] != DBNull.Value)
                                {
                                    c.Contexto = dr["Tipo"].ToString();
                                }

                                Lista.Add(c);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error SQL al listar comentarios de ventas: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error general: " + ex.Message);
                }
                finally
                {
                    if (Connection.State == ConnectionState.Open) { Connection.Close(); }
                }
            }
            return Lista;
        }
    }
    //----------------------------------------------REPORTES / KPIs
    public class DAC_Reportes
    {
        public Cls_KPI_Dinamico_Result Obtener_KPI_Dinamico(
            DateTime? FechaInicio,
            DateTime? FechaFin,
            int? IdCliente,
            int? IdUsuario,
            int? IdPlato)
        {
            var result = new Cls_KPI_Dinamico_Result();

            result.TopPlatos = new List<Cls_KPI_PlatoMasVendido>();
            result.InsumosUsados = new List<Cls_KPI_InsumoUsado>();
            result.InsumosCriticos = new List<Cls_KPI_InsumoCritico>();
            result.VentasPorDia = new List<Cls_KPI_VentaDia>();
            result.VentasPorPlato = new List<Cls_KPI_VentaPlato>();
            result.ClientesFrecuentes = new List<Cls_KPI_ClienteFrecuente>();
            result.PlatosMenosVendidos = new List<Cls_KPI_PlatoMenosVendido>();

            using (SqlConnection Connection = new SqlConnection(Conexion.Connect))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Proc_KPI_Dinamico]", Connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value =
                    FechaInicio.HasValue ? FechaInicio.Value : (object)DBNull.Value;

                    cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value =
                        FechaFin.HasValue ? FechaFin.Value : (object)DBNull.Value;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value =
                        IdCliente.HasValue ? IdCliente.Value : (object)DBNull.Value;

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value =
                        IdUsuario.HasValue ? IdUsuario.Value : (object)DBNull.Value;

                    cmd.Parameters.Add("@IdPlato", SqlDbType.Int).Value =
                        IdPlato.HasValue ? IdPlato.Value : (object)DBNull.Value;

                    if (Connection.State == ConnectionState.Closed) { Connection.Open(); }

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // 1) Ticket promedio
                        if (dr.Read() && !dr.IsDBNull(0))
                            result.PromedioDeVentas = dr.GetDecimal(0);
                        dr.NextResult();

                        // 2) Top platos más vendidos
                        while (dr.Read())
                        {
                            result.TopPlatos.Add(new Cls_KPI_PlatoMasVendido
                            {
                                Id_Plato = dr.GetInt32(dr.GetOrdinal("Id_Plato")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                CantidadVendida = dr.GetInt32(dr.GetOrdinal("CantidadVendida"))
                            });
                        }
                        dr.NextResult();

                        // 3) Insumos más usados
                        while (dr.Read())
                        {
                            result.InsumosUsados.Add(new Cls_KPI_InsumoUsado
                            {
                                Id_Insumo = dr.GetInt32(dr.GetOrdinal("Id_Insumo")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                CantidadUsada = dr.GetDecimal(dr.GetOrdinal("CantidadUsada"))
                            });
                        }
                        dr.NextResult();

                        // 4) Insumos críticos
                        while (dr.Read())
                        {
                            result.InsumosCriticos.Add(new Cls_KPI_InsumoCritico
                            {
                                Id_Insumo = dr.GetInt32(dr.GetOrdinal("Id_Insumo")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Stock_Disponible = dr.GetDecimal(dr.GetOrdinal("Stock_Disponible")),
                                Stock_Minimo = dr.GetDecimal(dr.GetOrdinal("Stock_Minimo"))
                            });
                        }
                        dr.NextResult();

                        // 5) Ventas por día
                        while (dr.Read())
                        {
                            result.VentasPorDia.Add(new Cls_KPI_VentaDia
                            {
                                Fecha = dr.GetDateTime(dr.GetOrdinal("Fecha")),
                                Total = dr.GetDecimal(dr.GetOrdinal("Total"))
                            });
                        }
                        dr.NextResult();

                        // 6) Ventas por plato
                        while (dr.Read())
                        {
                            result.VentasPorPlato.Add(new Cls_KPI_VentaPlato
                            {
                                Id_Plato = dr.GetInt32(dr.GetOrdinal("Id_Plato")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                CantidadVendida = dr.GetInt32(dr.GetOrdinal("CantidadVendida"))
                            });
                        }
                        dr.NextResult();

                        // 7) Clientes frecuentes
                        while (dr.Read())
                        {
                            result.ClientesFrecuentes.Add(new Cls_KPI_ClienteFrecuente
                            {
                                Id_Cliente = dr.GetInt32(dr.GetOrdinal("Id_Cliente")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Rol = dr.GetString(dr.GetOrdinal("Rol")),
                                Compras = dr.GetInt32(dr.GetOrdinal("Compras"))
                            });
                        }
                        dr.NextResult();

                        // 8) Platos menos vendidos
                        while (dr.Read())
                        {
                            result.PlatosMenosVendidos.Add(new Cls_KPI_PlatoMenosVendido
                            {
                                Id_Plato = dr.GetInt32(dr.GetOrdinal("Id_Plato")),
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Vendidos = dr.GetInt32(dr.GetOrdinal("Vendidos"))
                            });
                        }
                        dr.NextResult();

                        // 9) Total inventario
                        if (dr.Read() && !dr.IsDBNull(0))
                            result.TotalInventario = dr.GetDecimal(0);
                    }
                }
            }

            return result;
        }
    }
    public class DAC_Alertas
    {
        public List<Cls_Alerta> listar_alertas()
        {
            var lista = new List<Cls_Alerta>();

            using (var cn = new SqlConnection(Conexion.Connect))
            {
                cn.Open();

                // 1. INSUMOS SIN STOCK
                string sql1 = @"SELECT Id_Insumo, Nombre, Stock_Disponible, Stock_Minimo 
                                FROM Insumos 
                                WHERE Stock_Disponible = 0";

                using (var cmd = new SqlCommand(sql1, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "STOCK",
                            Mensaje = $"{dr["Nombre"]} sin existencias",
                            Detalle = $"Stock disponible: 0, mínimo requerido: {dr["Stock_Minimo"]}",
                            Icono = "alerta_roja.png",
                            Color = "Red",
                            //IdRelacionado = Convert.ToInt32(dr["Id_Insumo"])
                        });
                    }
                }

                // 2. INSUMOS BAJO MINIMO
                string sql2 = @"SELECT Id_Insumo, Nombre, Stock_Disponible, Stock_Minimo 
                                FROM Insumos 
                                WHERE Stock_Disponible < Stock_Minimo AND Stock_Disponible > 0";

                using (var cmd = new SqlCommand(sql2, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "STOCK",
                            Mensaje = $"{dr["Nombre"]} por debajo del mínimo",
                            Detalle = $"Stock disponible: {dr["Stock_Disponible"]}, mínimo requerido: {dr["Stock_Minimo"]}",
                            Icono = "alerta_amarilla.png",
                            Color = "Orange",
                            //IdRelacionado = Convert.ToInt32(dr["Id_Insumo"])
                        });
                    }
                }

                // 3. PEDIDOS CANCELADOS
                string sql3 = @"SELECT Id_Venta, Fecha_Pedido 
                                FROM Ventas 
                                WHERE Estado = 'Cancelado' 
                                AND CAST(Fecha_Pedido AS DATE) = CAST(GETDATE() AS DATE)";

                using (var cmd = new SqlCommand(sql3, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "PEDIDO",
                            Mensaje = $"Pedido #{dr["Id_Venta"]} cancelado",
                            Detalle = $"Fecha: {dr["Fecha_Pedido"]}",
                            Icono = "alerta_roja.png",
                            Color = "Red",
                            //IdRelacionado = Convert.ToInt32(dr["Id_Venta"])
                        });
                    }
                }
                // 4. PROMOCIONES POR VENCER
                string sql4 = @"SELECT Id_Promocion, Nombre, Fecha_Fin
                                FROM Promociones
                                WHERE DATEDIFF(DAY, GETDATE(), Fecha_Fin) BETWEEN 0 AND 2";

                using (var cmd = new SqlCommand(sql4, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var dias = (int)(Convert.ToDateTime(dr["Fecha_Fin"]) - DateTime.Now).TotalDays;

                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "PROMO",
                            Mensaje = $"Promoción '{dr["Nombre"]}' por vencer",
                            Detalle = dias == 0
                                     ? "Vence HOY"
                                     : $"Vence en {dias} día(s)",
                            Color = dias == 0 ? "Red" : "Orange",
                            Icono = "alerta_amarilla.png"
                        });
                    }
                }
                // 5. PLATOS IMPOSIBLES DE PREPARAR
                string sql5 = @"SELECT p.Id_Plato, p.Nombre AS Plato,i.Nombre AS Insumo, r.Cantidad_Necesaria, i.Stock_Disponible
                                FROM Platos p
                                JOIN Recetario r ON p.Id_Plato = r.Id_Plato
                                JOIN Insumos i ON r.Id_Insumo = i.Id_Insumo
                                WHERE i.Stock_Disponible < r.Cantidad_Necesaria";

                using (var cmd = new SqlCommand(sql5, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "PLATO",
                            Mensaje = $"No se puede preparar '{dr["Plato"]}'",
                            Detalle = $"{dr["Insumo"]}: Stock {dr["Stock_Disponible"]}, requiere {dr["Cantidad_Necesaria"]}",
                            Color = "Red",
                            Icono = "alerta_roja.png"
                        });
                    }
                }
                // 6. USUARIOS BLOQUEADOS
                string sql6 = @"SELECT Id_Usuario, Nombre
                                FROM Usuarios
                                WHERE Activo = 0 OR Bloqueado = 1";

                using (var cmd = new SqlCommand(sql6, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "USUARIO",
                            Mensaje = $"Usuario {dr["Nombre"]} está bloqueado o inactivo",
                            Detalle = "Debe verificarse su estado",
                            Color = "Orange",
                            Icono = "alerta_amarilla.png"
                        });
                    }
                }
                // 7. COMPRAS PENDIENTES
                string sql7 = @"SELECT Id_Compra, Fecha_Compra, Estado
                                FROM Compras
                                WHERE Estado IN ('Guardado', 'Validado')";

                using (var cmd = new SqlCommand(sql7, cn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cls_Alerta
                        {
                            Tipo = "COMPRA",
                            Mensaje = $"Compra #{dr["Id_Compra"]} pendiente",
                            Detalle = $"Estado: {dr["Estado"]}",
                            Color = "Orange",
                            Icono = "alerta_amarilla.png"
                        });
                    }
                }



            }

            return lista;
        }
    }
    public class DAC_Pago
    {
        public bool procesarPago(Cls_Cartera objCartera, float monto, out string mensaje)
        {
            mensaje = "";
            bool exito = false;

            using (SqlConnection cn = new SqlConnection(Conexion.Connect))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Proc_ProcesarPagoCartera", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregamos los parámetros tal cual pide tu SQL
                        cmd.Parameters.AddWithValue("@Tipo", objCartera.Tipo);
                        cmd.Parameters.AddWithValue("@Nro_Tarjeta", objCartera.Nro_Tarjeta);
                        cmd.Parameters.AddWithValue("@FechaVencimiento", objCartera.FechaVencimiento);
                        cmd.Parameters.AddWithValue("@Clave_Pin", objCartera.Clave_Pin);
                        cmd.Parameters.AddWithValue("@Monto", monto);

                        // LEEMOS EL RESULTADO DEL SELECT
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read()) // Si devuelve filas
                            {
                                // Leemos las columnas "Exito" y "Mensaje" que definiste en tu SP
                                exito = Convert.ToBoolean(dr["Exito"]);
                                mensaje = dr["Mensaje"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error en Base de Datos: " + ex.Message;
                    exito = false;
                }
            }
            return exito;
        }
    }

}

