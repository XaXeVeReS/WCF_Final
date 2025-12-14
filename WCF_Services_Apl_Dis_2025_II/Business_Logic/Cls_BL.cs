using Data_Access;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class Cls_BL
    {
        private Data_Access.DAC_Usuarios D_usuario          = new Data_Access.DAC_Usuarios();
        private Data_Access.DAC_Insumos D_insumos           = new Data_Access.DAC_Insumos();
        private Data_Access.DAC_Platos D_platos             = new Data_Access.DAC_Platos();
        private Data_Access.DAC_Promociones D_promociones   = new Data_Access.DAC_Promociones();
        private Data_Access.DAC_Ventas D_ventas             = new Data_Access.DAC_Ventas();
        private Data_Access.DAC_Compras D_compras           = new Data_Access.DAC_Compras();
        private Data_Access.DAC_Comentarios D_comentarios   = new Data_Access.DAC_Comentarios();

        private Data_Access.DAC_Reportes D_reportes         = new Data_Access.DAC_Reportes();
        private Data_Access.DAC_Alertas D_alertas           = new Data_Access.DAC_Alertas();
        private Data_Access.DAC_Pago D_Pagos                = new Data_Access.DAC_Pago();


        //----------------------------------------------USUARIOS
        #region USUARIOS 
        public int Acceso_Usuario(string User, string Password)
        {
            return D_usuario.acceso_usuario(User, Password);
        }
        public void Enviar_Token(string Email, string Usuario)
        {
            D_usuario.enviar_token(Email, Usuario);
        }
        public void Actualizar_Password(string Email, string token, string password)
        {
            D_usuario.actualizar_password(Email, token, password);
        }
        public void Insertar_Usuario(Cls_Usuarios User)
        {
            D_usuario.insertar_usuario(User);
        }
        public void Actualizar_Usuario(Cls_Usuarios User)
        {
            D_usuario.actualizar_usuario(User);
        }
        public void Eliminar_Usuario(int Id_User)
        {
            D_usuario.eliminar_usuario(Id_User);
        }
        public List<Cls_Usuarios> Listar_Usuarios()
        {
            List<Cls_Usuarios> Lista = D_usuario.listar_usuarios();
            return Lista;
        }
        public Cls_Usuarios Buscar_Usuario(int Id_User)
        {
            Cls_Usuarios Usuario = D_usuario.buscar_usuario(Id_User);
            return Usuario;
        }
        public bool Verificar_Estado_Usuario(int userId)
        {
            return D_usuario.verificar_estado_usuario(userId);
        }

        #endregion
        //----------------------------------------------INSUMOS
        #region INSUMOS
        public void Insertar_Insumo(Cls_Insumos Insumo, int Id_Usuario)
        {
            D_insumos.insertar_insumo(Insumo, Id_Usuario);
        }
        public void Actualizar_Insumo(Cls_Insumos Insumos)
        {
            D_insumos.actualizar_insumo(Insumos);
        }
        public void stock_nsumo(List<Cls_Insumos> Insumos, int Id_Usuario)
        {
            D_insumos.stock_insumo(Insumos, Id_Usuario);
        }
        public void Eliminar_Insumo(int Id_Insumo)
        {
            D_insumos.eliminar_insumo(Id_Insumo);
        }
        public List<Cls_Insumos> Listar_Insumos()
        {
            List<Cls_Insumos> Lista = D_insumos.listar_insumos();
            return Lista;
        }
        public Cls_Insumos Buscar_Insumo(int Id_Insumo)
        {
            Cls_Insumos Insumo = D_insumos.buscar_insumo(Id_Insumo);
            return Insumo;
        }
        public List<Cls_CardexInsumos> Listar_Cardex_Insumo(int Id_Insumo)
        {
            return D_insumos.listar_cardex_insumo(Id_Insumo);
        }
        #endregion
        //----------------------------------------------PLATOS
        #region PLATOS
        public void Insertar_Plato(Cls_Platos Plato)
        {
            D_platos.insertar_plato(Plato);
        }
        public void Actualizar_Plato(Cls_Platos Plato)
        {
            D_platos.actualizar_plato(Plato);
        }
        public void Eliminar_Plato(int Id_Plato)
        {
            D_platos.eliminar_plato(Id_Plato);
        }
        public List<Cls_Platos> Listar_Platos()
        {
            List<Cls_Platos> Lista = D_platos.listar_platos();
            return Lista;
        }
        public Cls_Platos Buscar_Plato(int Id_Plato)
        {
            Cls_Platos Plato = D_platos.buscar_plato(Id_Plato);
            return Plato;
        }
        public int Stock_Disponible(int Id_Plato)
        {
            return D_platos.stock_disponible(Id_Plato);
        }
        #endregion
        //----------------------------------------------PROMOCIONES
        #region PROMOCIONES
        public void Insertar_Promocion(Cls_Promociones Promocion)
        {
            D_promociones.insertar_promocion(Promocion);
        }
        public void Actualizar_Promocion(Cls_Promociones Promocion)
        {
            D_promociones.actualizar_promocion(Promocion);
        }
        public void Eliminar_Promocion(int Id_Promocion)
        {
            D_promociones.eliminar_promocion(Id_Promocion);
        }
        public List<Cls_Promociones> Listar_Promociones(bool Listar_Todo)
        {
            List<Cls_Promociones> Lista = D_promociones.listar_promociones(Listar_Todo);
            return Lista;
        }
        public List<Cls_Promocion_Plato> Listar_Promocion_Platos()
        {
            List<Cls_Promocion_Plato> Lista = D_promociones.listar_promociones_platos();
            return Lista;
        }
        public Cls_Promociones Buscar_Promocion(int Id_Promocion)
        {
            Cls_Promociones Promocion = D_promociones.buscar_promocion(Id_Promocion);
            return Promocion;
        }

        #endregion
        //----------------------------------------------VENTAS
        #region VENTAS
        public void Insertar_Venta(Cls_Ventas Venta)
        {
            D_ventas.insertar_venta(Venta);
        }
        public int Insertar_Venta_Return_Id(Cls_Ventas venta)
        {
            return D_ventas.insertar_venta_retorna_id(venta);
        }
        public void Actualizar_Venta(Cls_Ventas Venta)
        {
            D_ventas.actualizar_venta(Venta);
        }
        public void Estado_Venta(Cls_Ventas Venta)
        {
            D_ventas.estado_venta(Venta);
        }
        public Cls_Ventas Get_Estado_Venta(int id)
        {
            return D_ventas.ObtenerEstadoVenta(id);
        }
        public List<Cls_Ventas> Listar_Ventas(DateTime? Fecha, string Estado)
        {
            List<Cls_Ventas> Lista = D_ventas.listar_ventas(Fecha,Estado);
            return Lista;
        }
        public Cls_Ventas Buscar_Venta(int Id_Venta)
        {
            Cls_Ventas Venta = D_ventas.buscar_venta(Id_Venta);
            return Venta;
        }

        #endregion
        //----------------------------------------------COMPRAS
        #region COMPRAS
        public void Insertar_Compra(Cls_Compras Compra)
        {

            D_compras.insertar_compra(Compra);
        }
        public void Actualizar_Compra(Cls_Compras Compra)
        {
            D_compras.actualizar_compra(Compra);
        }
        public void Estado_Compra(Cls_Compras Compra)
        {
            D_compras.estado_compra(Compra);
        }
        public List<Cls_Compras> Listar_Compras()
        {
            List<Cls_Compras> Lista = D_compras.listar_compras();
            return Lista;
        }
        public Cls_Compras Buscar_Compra(int Id_Compra)
        {
            Cls_Compras Compra = D_compras.buscar_compra(Id_Compra);
            return Compra;
        }
        public void Eliminar_Compra(int Id_Compra)
        {
            D_compras.eliminar_compra(Id_Compra);
        }
        public List<Cls_DetalleCompras> Listar_DetallesCompra(int Id_Compra)
        {
            return D_compras.listar_detalles_compra(Id_Compra);
        }
        public void Eliminar_DetallesCompra(int Id_Compra)
        {
            D_compras.eliminar_detalles_compra(Id_Compra);
        }
        public void Insertar_DetalleCompra(Cls_DetalleCompras Detalle)
        {
            D_compras.insertar_detalle_compra(Detalle);
        }

        #endregion
        //----------------------------------------------COMENTARIOS
        #region COMENTARIOS
        public void Insertar_Comentario(Cls_Comentarios Comentario)
        {
            D_comentarios.insertar_comentario(Comentario);
        }
        public void Eliminar_Comentario(int Id_Comentario)
        {
            D_comentarios.eliminar_comentario(Id_Comentario);
        }
        public List<Cls_Comentarios> Listar_Comentarios(string Contexto, int Id_Relacionado)
        {
            List<Cls_Comentarios> Lista = D_comentarios.listar_comentarios(Contexto, Id_Relacionado);
            return Lista;
        }
        public List<Cls_Comentarios> Listar_Todos_Comentarios()
        {
            return D_comentarios.listar_todos_comentarios();
        }
        #endregion
        //----------------------------------------------REPORTES / KPIs
        #region REPORTES
        public Cls_KPI_Dinamico_Result KPI_Dinamico(
            DateTime? FechaInicio,
            DateTime? FechaFin,
            int? IdCliente,
            int? IdUsuario,
            int? IdPlato)
        {
            return D_reportes.Obtener_KPI_Dinamico(FechaInicio, FechaFin, IdCliente, IdUsuario, IdPlato);
        }
        #endregion

        public List<Cls_Alerta> Listar_Alertas()
        {
            return D_alertas.listar_alertas();
        }
        public bool ProcesarPago(Cls_Cartera objCartera, float monto, out string mensaje)
        {
            // Pasamos la llamada a la capa de datos
            return D_Pagos.procesarPago(objCartera, monto, out mensaje);
        }

    }
}
