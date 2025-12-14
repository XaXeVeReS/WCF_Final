using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Cls_Usuarios
    {
        public int Id_Usuario {  get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public bool Activo { get; set; }

        public string Password_Hash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {              
                byte[] bytes = Encoding.UTF8.GetBytes(this.Password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    public class Cls_Insumos
    {
        public int Id_Insumo { get; set; }
        public string Nombre { get; set; }
        public string Unidad_Medida { get; set; }
        public float Stock_Disponible { get; set; }
        public float Stock_Minimo { get; set; }
        public string Json_Insumo()
        {
            return $"{{ \"Id_Insumo\": {this.Id_Insumo}, \"Stock_Disponible\": {this.Stock_Disponible} }}";
        }
    }
    public class Cls_Platos
    {
        public int Id_Plato { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Precio { get; set; }
        public int Tiempo_Preparacion { get; set; }
        public bool Activo { get; set; }
        public byte[] Version { get; set; }
        public byte[] Imagen { get; set; }
        public string txt_Nombre_Imagen { get; set; }
        public string Direc_Imagen { get; set; }
        public List<Cls_Recetario> Recetario { get; set; }
        public string json_recetario()
        {
            string json_recetario = "[";
            foreach (Cls_Recetario i in this.Recetario)
            {
                json_recetario += i.Json_Recetario() + ",";
            }
            json_recetario = json_recetario.Substring(0, json_recetario.Length - 1);

            return json_recetario += "]";
        }
    }
    public class Cls_Recetario
    {
        public int Id_Plato { get; set; }
        public int Id_Insumo { get; set; }
        public float Cantidad_Necesaria { get; set; }
        public string txt_Insumo { get; set; }
        public string txt_Unidad_Medida {  get; set; }
        public string Json_Recetario()
        {
            return $"{{ \"Id_Insumo\": {this.Id_Insumo}, \"Cantidad_Necesaria\": {this.Cantidad_Necesaria} }}";
        }
    }
    public class Cls_Promociones
    {
        public int Id_Promocion { get; set; }
        public int Id_Plato { get; set; }
        public string Nombre { get; set; }
        public int Cantidad_Aplicable { get; set; }
        public float Descuento { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public bool Activo { get; set; }
        public byte[] Version { get; set; }
    }
    public class Cls_Ventas
    {
        public int Id_Venta { get; set; }
        public int? Id_Trabajador { get; set; }
        public int? Id_Cliente { get; set; }
        public DateTime Fecha_Pedido { get; set; }
        public float Costo_Total { get; set; }
        public float Monto_Total { get; set; }
        public string Metodo_Pago { get; set; }
        public string Estado { get; set; }
        public byte[] Version { get; set; } 
        public string txt_Trabajador { get; set; }
        public string txt_Cliente { get; set; }
        public List<Cls_DetalleVenta> DetalleVenta { get; set; }
        public string json_DetalleVenta()
        {
            if (this.DetalleVenta == null || this.DetalleVenta.Count ==0)
            { return "[]"; }

            var detalle = new List<string>();
            foreach (Cls_DetalleVenta i in this.DetalleVenta)
            {
                detalle.Add(i.Json_Detalle());
            }

            return "["+ string.Join(",",detalle) + "]";
        }
    }
    public class Cls_DetalleVenta
    {
        public int Id_Venta { get; set; }
        public int Id_Plato { get; set; }
        public float Precio_Unitario { get; set; }
        public int Cantidad { get; set; }
        public float? Descuento { get; set; }
        public int? Id_Promocion {  get; set; }  
        public string txt_Plato { get; set; }
        public string Json_Detalle()
        {
            string Descuento = this.Descuento.HasValue ? this.Descuento.Value.ToString() : "null";
            string Id_Promocion = this.Id_Promocion.HasValue ? this.Id_Promocion.Value.ToString() : "null";
            return $"{{ \"Id_Plato\": {this.Id_Plato}, \"Precio_Unitario\": {this.Precio_Unitario}, \"Cantidad\": {this.Cantidad}, \"Descuento\": {Descuento}, \"Id_Promocion\": {Id_Promocion} }}";
        }
    }
    public class Cls_Comentarios
    {
        public int Id_Comentario { get; set; }
        public int Id_Usuario { get; set; }
        public string Contexto { get; set; }
        public int Id_Relacionado { get; set; }
        public string Comentario { get; set; }
        public string txt_Usuario { get; set; }
        public DateTime Fecha_Comentario { get; set; }
        public int? Id_Plato { get; set; }
        public int? Id_Venta { get; set; }
        public int Valoracion { get; set; }
        public string tipo { get; set; }
    }
    public class Cls_Compras
    {
        public int Id_Compra { get; set; }
        public int Id_Usuario { get; set; }
        public DateTime Fecha_Compra { get; set; }
        public float Costo_Total { get; set; }
        public string Estado { get; set; }
        public byte[] Version { get; set; }
        public string txt_Usuario { get; set; }
        public string Proveedor { get; set; }
        public float Total { get; set; }
        public string Observaciones { get; set; }
        public List<Cls_DetalleCompras> DetalleCompras { get; set; }
        public string json_DetalleCompras()
        {
            if (this.DetalleCompras == null || this.DetalleCompras.Count == 0)
            { return "[]"; }

            var detalle = new List<string>();
            foreach (Cls_DetalleCompras i in this.DetalleCompras)
            {
                detalle.Add(i.Json_Detalle());
            }

            return "[" + string.Join(",", detalle) + "]";
        }
    }
    public class Cls_DetalleCompras
    {
        public int Id_Compra { get; set; }
        public int Id_Insumo { get; set; }
        public float Costo_Unitario { get; set; }
        public float Precio_Unitario { get; set; }
        public float Cantidad { get; set; }
        public string txt_Insumo { get; set; }
        public string txt_Unidad_Medida { get; set; }
        public string Json_Detalle()
        {
            return $"{{ \"Id_Insumo\": {this.Id_Insumo}, \"Costo_Unitario\": {this.Costo_Unitario}, \"Cantidad\": {this.Cantidad} }}";
        }
    }
    // ===================== KPIs / REPORTES =====================
    public class Cls_KPI_TicketPromedio
    {
        public decimal PromedioDeVentas { get; set; }
    }

    public class Cls_KPI_PlatoMasVendido
    {
        public int Id_Plato { get; set; }
        public string Nombre { get; set; }
        public int CantidadVendida { get; set; }
    }

    public class Cls_KPI_InsumoUsado
    {
        public int Id_Insumo { get; set; }
        public string Nombre { get; set; }
        public decimal CantidadUsada { get; set; }
    }

    public class Cls_KPI_InsumoCritico
    {
        public int Id_Insumo { get; set; }
        public string Nombre { get; set; }
        public decimal Stock_Disponible { get; set; }
        public decimal Stock_Minimo { get; set; }
    }

    public class Cls_KPI_VentaDia
    {
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }

    public class Cls_KPI_VentaPlato
    {
        public int Id_Plato { get; set; }
        public string Nombre { get; set; }
        public int CantidadVendida { get; set; }
    }

    public class Cls_KPI_ClienteFrecuente
    {
        public int Id_Cliente { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public int Compras { get; set; }
    }

    public class Cls_KPI_PlatoMenosVendido
    {
        public int Id_Plato { get; set; }
        public string Nombre { get; set; }
        public int Vendidos { get; set; }
    }

    public class Cls_KPI_Inventario
    {
        public decimal TotalInventario { get; set; }
    }

    // Contenedor general del resultado dinámico
    public class Cls_KPI_Dinamico_Result
    {
        public decimal PromedioDeVentas { get; set; }

        public List<Cls_KPI_PlatoMasVendido> TopPlatos { get; set; }
        public List<Cls_KPI_InsumoUsado> InsumosUsados { get; set; }
        public List<Cls_KPI_InsumoCritico> InsumosCriticos { get; set; }
        public List<Cls_KPI_VentaDia> VentasPorDia { get; set; }
        public List<Cls_KPI_VentaPlato> VentasPorPlato { get; set; }
        public List<Cls_KPI_ClienteFrecuente> ClientesFrecuentes { get; set; }
        public List<Cls_KPI_PlatoMenosVendido> PlatosMenosVendidos { get; set; }
        public decimal TotalInventario { get; set; }

        public Cls_KPI_Dinamico_Result()
        {
            TopPlatos = new List<Cls_KPI_PlatoMasVendido>();
            InsumosUsados = new List<Cls_KPI_InsumoUsado>();
            InsumosCriticos = new List<Cls_KPI_InsumoCritico>();
            VentasPorDia = new List<Cls_KPI_VentaDia>();
            VentasPorPlato = new List<Cls_KPI_VentaPlato>();
            ClientesFrecuentes = new List<Cls_KPI_ClienteFrecuente>();
            PlatosMenosVendidos = new List<Cls_KPI_PlatoMenosVendido>();
        }
    }
    public class Cls_Alerta
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Icono { get; set; }
        public string Color { get; set; }
        //public int? IdRelacionado { get; set; }
    }

    public class Cls_Promocion_Plato
    {
        public int Id_Promocion { get; set; }
        public int Id_Plato { get; set; }
        public string PlatoNombre { get; set; }
        public string Descripcion { get; set; }
        public float PrecioNormal { get; set; }
        public float PrecioConDescuento { get; set; }
        public string PromocionNombre { get; set; }
        public int Cantidad_Aplicable { get; set; }
        public float Descuento { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public int Activo { get; set; }
        public byte[] Imagen { get; set; }
        public string Direc_Imagen { get; set; }
    }

    public class Cls_CardexInsumos
    {
        public int Id_Insumo { get; set; }
        public string Tipo_Movimiento { get; set; }
        public float Cantidad { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha_Movimiento { get; set; }
        public int Id_Usuario { get; set; }
    }
    public class Cls_Cartera
    {

        public string Tipo { get; set; }
        public string Nro_Tarjeta { get; set; }
        public string FechaVencimiento { get; set; }
        public string Clave_Pin { get; set; }
    }
}


