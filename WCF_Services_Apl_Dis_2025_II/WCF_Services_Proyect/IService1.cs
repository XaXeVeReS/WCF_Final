using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCF_Services_Proyect
{
    [ServiceContract]
    public interface IService1
    {

        //----------------------------------------------USUARIOS
        #region USUARIOS 

        [OperationContract]
        int Login_User(string User, string Password);

        [OperationContract]
        void Send_Token(string Email, string Usuario);

        [OperationContract]
        void Update_Password(string Email, string token, string password);

        [OperationContract]
        void Insert_User(Entities.Cls_Usuarios User);

        [OperationContract]
        void Update_User(Entities.Cls_Usuarios User);

        [OperationContract]
        void Delete_User(int Id_User);

        [OperationContract]
        List<Entities.Cls_Usuarios> Get_Users();

        [OperationContract]
        Entities.Cls_Usuarios Search_User(int Id_User);

        [OperationContract]
        bool Check_User_Status(int userId);
        #endregion
        //----------------------------------------------INSUMOS
        #region INSUMOS
        [OperationContract]
        void Insert_Insumo(Entities.Cls_Insumos Insumo, int Id_Usuario);

        [OperationContract]
        void Update_Insumo(Entities.Cls_Insumos Insumo);

        [OperationContract]
        void Stock_Insumos(List<Entities.Cls_Insumos> Insumos, int Id_Usuario);

        [OperationContract]
        void Delete_Insumo(int Id_Insumo);

        [OperationContract]
        List<Entities.Cls_Insumos> Get_Insumos();

        [OperationContract]
        Entities.Cls_Insumos Search_Insumo(int Insumo);

        [OperationContract]
        List<Entities.Cls_CardexInsumos> Get_CardexByInsumo(int Id_Insumo);
        #endregion
        //----------------------------------------------PLATOS
        #region PLATOS
        [OperationContract]
        void Insert_Plato(Entities.Cls_Platos Plato);

        [OperationContract]
        void Update_Plato(Entities.Cls_Platos Plato);

        [OperationContract]
        void Delete_Plato(int Id_Plato);

        [OperationContract]
        List<Entities.Cls_Platos> Get_Platos();

        [OperationContract]
        Entities.Cls_Platos Search_Plato(int Id_Plato);

        [OperationContract]
        int Check_Stock(int Id_Plato);
        #endregion
        //----------------------------------------------PROMOCIONES
        #region PROMOCIONES
        [OperationContract]
        void Insert_Promocion(Entities.Cls_Promociones Promocion);

        [OperationContract]
        void Update_Promocion(Entities.Cls_Promociones Promocion);

        [OperationContract]
        void Delete_Promocion(int Id_Promocion);

        [OperationContract]
        List<Entities.Cls_Promociones> Get_Promociones(bool Listar_Todo);

        [OperationContract]
        List<Entities.Cls_Promocion_Plato> Get_Promocion_Platos();

        [OperationContract]
        Entities.Cls_Promociones Search_Promocion(int Id_Promocion);
        #endregion
        //----------------------------------------------VENTAS
        #region VENTAS
        [OperationContract]
        void Insert_Venta(Entities.Cls_Ventas Venta);

        [OperationContract]
        int Insert_Venta_Return_Id(Cls_Ventas venta);

        [OperationContract]
        void Update_Venta(Entities.Cls_Ventas Venta);

        [OperationContract]
        void State_Venta(Entities.Cls_Ventas Venta);

        [OperationContract]
        Cls_Ventas Get_Estado_Venta(int idVenta);

        [OperationContract]
        List<Entities.Cls_Ventas> Get_Ventas(DateTime? Fecha, string Estado);

        [OperationContract]
        Entities.Cls_Ventas Search_Venta(int Id_Venta);

        [OperationContract]
        int Search_id_venta_activa(int Id_Venta);
        #endregion
        //----------------------------------------------COMPRAS
        #region COMPRAS
        [OperationContract]
        void Insert_Compra(Entities.Cls_Compras Compra);

        [OperationContract]
        void Update_Compra(Entities.Cls_Compras Compra);

        [OperationContract]
        void State_Compra(Entities.Cls_Compras Compra);

        [OperationContract]
        List<Entities.Cls_Compras> Get_Compras();

        [OperationContract]
        Entities.Cls_Compras Search_Compra(int Id_Compra);

        [OperationContract]
        void Delete_Compra(int Id_Compra);

        [OperationContract]
        List<Entities.Cls_DetalleCompras> Get_DetallesCompraByCompraId(int Id_Compra);

        [OperationContract]
        void Delete_DetallesCompra(int Id_Compra);

        [OperationContract]
        void Insert_DetalleCompra(Entities.Cls_DetalleCompras Detalle);
        #endregion
        //----------------------------------------------COMENTARIOS
        #region COMENTARIOS
        [OperationContract]
        void Insert_Comentario(Entities.Cls_Comentarios Comentario);

        [OperationContract]
        void Delete_Comentario(int Id_Comentario);


        [OperationContract]
        List<Entities.Cls_Comentarios> Get_Comentarios(string Contexto, int Id_Relacionado);

        [OperationContract(Name = "Get_Comentarios_All")]
        List<Entities.Cls_Comentarios> Get_Comentarios_All();
        #endregion
        //---------------------------------------------REPORTES
        #region Reportes
        [OperationContract]
        Cls_KPI_Dinamico_Result Get_KPI_Dinamico(DateTime? FechaInicio, DateTime? FechaFin, int? IdCliente, int? IdUsuario, int? IdPlato);
        #endregion
        //----------------------------------------------ALERTAS
        #region ALERTAS
        [OperationContract]
        List<Cls_Alerta> Get_Alertas();
        #endregion
        //----------------------------------------------CARTERA
        #region CARTERA
        [OperationContract]
        bool Procesar_Pago(Cls_Cartera objCartera, float monto, out string mensaje);
        #endregion
    }
}
