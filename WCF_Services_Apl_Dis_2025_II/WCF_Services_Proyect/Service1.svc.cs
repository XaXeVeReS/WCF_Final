using Business_Logic;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WCF_Services_Proyect
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1 : IService1
    {
        private Business_Logic.Cls_BL B_Logic = new Business_Logic.Cls_BL();

        //----------------------------------------------USUARIOS
        #region USUARIOS 
        public int Login_User(string User, string Password)
        {
            try
            {
                    Logger.LogInfo($"Intento de login con Email: {User}");
                int result = B_Logic.Acceso_Usuario(User, Password);
                
                if (result > 0)
                {
                    Logger.LogInfo($"Login exitoso para usuario ID: {result}");
                }
                else if (result == -2)
                {
                    Logger.LogWarning($"Usuario bloqueado: {User}");
                }
                else
                {
                    Logger.LogWarning($"Credenciales inválidas para: {User}");
                }
                
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError($"Error en Login_User: {User}", e);
                throw new FaultException($"Error al iniciar sesión: {e.Message}");
            }
        }
        public void Send_Token(string Email, string Usuario)
        {
            try
            {
                B_Logic.Enviar_Token(Email, Usuario);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Update_Password(string Email, string token, string password)
        {
            try
            {
                B_Logic.Actualizar_Password(Email, token, password);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Insert_User(Cls_Usuarios User)
        {
            try
            {
                B_Logic.Insertar_Usuario(User);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Update_User(Cls_Usuarios User)
        {
            try
            {
                B_Logic.Actualizar_Usuario(User);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_User(int Id_User)
        {
            try
            {
                B_Logic.Eliminar_Usuario(Id_User);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Usuarios> Get_Users()
        {
            try
            {
                List<Cls_Usuarios> Lista = B_Logic.Listar_Usuarios();
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Usuarios Search_User(int Id_User)
        {
            try
            {
                Logger.LogDebug($"Buscando usuario con ID: {Id_User}");
                
                if (Id_User <= 0)
                {
                    Logger.LogWarning($"ID de usuario inválido: {Id_User}");
                    throw new FaultException("ID de usuario inválido");
                }

                Cls_Usuarios Usuario = B_Logic.Buscar_Usuario(Id_User);
                
                if (Usuario == null)
                {
                    Logger.LogError($"Usuario no encontrado con ID: {Id_User}");
                    throw new FaultException($"No se encontró el usuario con ID: {Id_User}");
                }

                if (string.IsNullOrWhiteSpace(Usuario.Nombre) || Usuario.Id_Usuario <= 0)
                {
                    Logger.LogError($"Datos de usuario incompletos. ID: {Usuario.Id_Usuario}, Nombre: '{Usuario.Nombre}'");
                    throw new FaultException("Los datos del usuario están incompletos o dañados");
                }

                Logger.LogInfo($"Usuario encontrado exitosamente. ID: {Id_User}, Nombre: {Usuario.Nombre}, Rol: {Usuario.Rol}");
                return Usuario;
            }
            catch (FaultException fex)
            {
                Logger.LogWarning($"FaultException en Search_User (ID: {Id_User}): {fex.Message}");
                throw;
            }
            catch (Exception e)
            {
                Logger.LogError($"Error al buscar usuario con ID {Id_User}", e);
                throw new FaultException($"Error al buscar usuario: {e.Message}");
            }
        }
        public bool Check_User_Status(int userId)
        {
            try
            {
                Logger.LogDebug($"Verificando estado del usuario ID: {userId}");
                bool status = B_Logic.Verificar_Estado_Usuario(userId);
                Logger.LogInfo($"Estado del usuario {userId}: {(status ? "Activo" : "Inactivo")}");
                return status;
            }
            catch (Exception e)
            {
                Logger.LogError($"Error al verificar estado del usuario {userId}", e);
                throw new FaultException($"Error al verificar estado: {e.Message}");
            }
        }
        #endregion
        //----------------------------------------------INSUMOS
        #region INSUMOS
        public void Insert_Insumo(Cls_Insumos Insumo, int Id_Usuario)
        {
            try
            {
                B_Logic.Insertar_Insumo(Insumo, Id_Usuario);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Update_Insumo(Cls_Insumos Insumo)
        {
            try
            {
                B_Logic.Actualizar_Insumo(Insumo);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Stock_Insumos(List<Cls_Insumos> Insumos, int Id_Usuario)
        {
            try
            {
                B_Logic.stock_nsumo(Insumos, Id_Usuario);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_Insumo(int Id_Insumo)
        {
            try
            {
                B_Logic.Eliminar_Insumo(Id_Insumo);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Insumos> Get_Insumos()
        {
            try
            {
                List<Cls_Insumos> Lista = B_Logic.Listar_Insumos();
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Insumos Search_Insumo(int Id_Insumo)
        {
            try
            {
                Cls_Insumos Insumo = B_Logic.Buscar_Insumo(Id_Insumo);
                return Insumo;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_CardexInsumos> Get_CardexByInsumo(int Id_Insumo)
        {
            try
            {
                return B_Logic.Listar_Cardex_Insumo(Id_Insumo);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------PLATOS
        #region PLATOS
        public void Insert_Plato(Cls_Platos Plato)
        {
            try
            {
                B_Logic.Insertar_Plato(Plato);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Update_Plato(Cls_Platos Plato)
        {
            try
            {
                B_Logic.Actualizar_Plato(Plato);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_Plato(int Id_Plato)
        {
            try
            {
                B_Logic.Eliminar_Plato(Id_Plato);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Platos> Get_Platos()
        {
            try
            {
                List<Cls_Platos> Lista = B_Logic.Listar_Platos();
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Platos Search_Plato(int Id_Plato)
        {
            try
            {
                Cls_Platos Plato = B_Logic.Buscar_Plato(Id_Plato);
                return Plato;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public int Check_Stock(int Id_Plato)
        {
            try
            {
                return B_Logic.Stock_Disponible(Id_Plato); ;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------PROMOCIONES
        #region PROMOCIONES
        public void Insert_Promocion(Cls_Promociones Promocion)
        {
            try
            {
                B_Logic.Insertar_Promocion(Promocion);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Update_Promocion(Cls_Promociones Promocion)
        {
            try
            {
                B_Logic.Actualizar_Promocion(Promocion);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_Promocion(int Id_Promocion)
        {
            try
            {
                B_Logic.Eliminar_Promocion(Id_Promocion);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Promociones> Get_Promociones(bool Listar_Todo)
        {
            try
            {
                List<Cls_Promociones> Lista = B_Logic.Listar_Promociones(Listar_Todo);
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }

        public List<Cls_Promocion_Plato> Get_Promocion_Platos()
        {
            try
            {
                List<Cls_Promocion_Plato> Lista = B_Logic.Listar_Promocion_Platos();
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Promociones Search_Promocion(int Id_Promocion)
        {
            try
            {
                Cls_Promociones Promocion = B_Logic.Buscar_Promocion(Id_Promocion);
                return Promocion;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------VENTAS
        #region VENTAS
        public void Insert_Venta(Cls_Ventas Venta)
        {
            try
            {
                B_Logic.Insertar_Venta(Venta);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public int Insert_Venta_Return_Id(Cls_Ventas venta)
        {
            try
            {
                return B_Logic.Insertar_Venta_Return_Id(venta);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
            
        }

        public void Update_Venta(Cls_Ventas Venta)
        {
            try
            {
                B_Logic.Actualizar_Venta(Venta);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void State_Venta(Cls_Ventas Venta)
        {
            try
            {
                B_Logic.Estado_Venta(Venta);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Ventas Get_Estado_Venta(int idVenta)
        {
            return B_Logic.Get_Estado_Venta(idVenta);
        }
        public List<Cls_Ventas> Get_Ventas(DateTime? Fecha, string Estado)
        {
            try
            {
                List<Cls_Ventas> Lista = B_Logic.Listar_Ventas(Fecha,Estado);
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Ventas Search_Venta(int Id_Venta)
        {
            try
            {
                Cls_Ventas Venta = B_Logic.Buscar_Venta(Id_Venta);
                return Venta;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------COMPRAS
        #region COMPRAS
        public void Insert_Compra(Cls_Compras Compra)
        {
            try
            {
                B_Logic.Insertar_Compra(Compra);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Update_Compra(Cls_Compras Compra)
        {
            try
            {
                B_Logic.Actualizar_Compra(Compra);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void State_Compra(Cls_Compras Compra)
        {
            try
            {
                B_Logic.Estado_Compra(Compra);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Compras> Get_Compras()
        {
            try
            {
                List<Cls_Compras> Lista = B_Logic.Listar_Compras();
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public Cls_Compras Search_Compra(int Id_Compra)
        {
            try
            {
                Cls_Compras Compra = B_Logic.Buscar_Compra(Id_Compra);
                return Compra;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_Compra(int Id_Compra)
        {
            try
            {
                B_Logic.Eliminar_Compra(Id_Compra);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_DetalleCompras> Get_DetallesCompraByCompraId(int Id_Compra)
        {
            try
            {
                return B_Logic.Listar_DetallesCompra(Id_Compra);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_DetallesCompra(int Id_Compra)
        {
            try
            {
                B_Logic.Eliminar_DetallesCompra(Id_Compra);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Insert_DetalleCompra(Cls_DetalleCompras Detalle)
        {
            try
            {
                B_Logic.Insertar_DetalleCompra(Detalle);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------COMENTARIOS
        #region COMENTARIOS
        public void Insert_Comentario(Cls_Comentarios Comentario)
        {
            try
            {
                B_Logic.Insertar_Comentario(Comentario);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public void Delete_Comentario(int Id_Comentario)
        {
            try
            {
                B_Logic.Eliminar_Comentario(Id_Comentario);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Comentarios> Get_Comentarios(string Contexto, int Id_Relacionado)
        {
            try
            {
                List<Cls_Comentarios> Lista = B_Logic.Listar_Comentarios(Contexto, Id_Relacionado);
                return Lista;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        public List<Cls_Comentarios> Get_Comentarios_All()
        {
            try
            {
                return B_Logic.Listar_Todos_Comentarios();
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------REPORTES
        #region REPORTES / KPIs
        public Cls_KPI_Dinamico_Result Get_KPI_Dinamico(DateTime? FechaInicio, DateTime? FechaFin, int? IdCliente, int? IdUsuario, int? IdPlato)
        {
            try
            {
                return B_Logic.KPI_Dinamico(FechaInicio, FechaFin, IdCliente, IdUsuario, IdPlato);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------ALERTAS
        #region Alertas
        public List<Cls_Alerta> Get_Alertas()
        {
            try
            {
                return B_Logic.Listar_Alertas();
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
        #endregion
        //----------------------------------------------PAGOS CARTERA
        #region Pagos Cartera
        public bool Procesar_Pago(Cls_Cartera objCartera, float monto, out string mensaje)
        {
            return B_Logic.ProcesarPago(objCartera, monto, out mensaje);
        }
        #endregion
    }
}
