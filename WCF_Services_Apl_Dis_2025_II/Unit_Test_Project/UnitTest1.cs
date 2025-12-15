using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using WCF_Services_Proyect;

namespace Unit_Test_Project
{
    [TestClass]
    public class _0_Insert
    {
        private Service1 client;

        // Se ejecuta antes de cada prueba
        [TestInitialize]
        public void Setup()
        {
            client = new Service1();
        }

        [TestMethod]
        public void Test_1_0_Insert_User()
        {
            var user = new Cls_Usuarios
            {
                Nombre = "Juan",
                Rol = "Administrador",
                Password = "agdhdjfk",
                Email = "juan@example.com",
                Telefono = "987654321"
            };
            var user1 = new Cls_Usuarios
            {
                Nombre = "Carlos",
                Rol = "Empleado",
                Password = "agdfadjfk",
                Email = "carlos@example.com",
                Telefono = "978967545"
            };
            var user2 = new Cls_Usuarios
            {
                Nombre = "Daniel",
                Rol = "Cliente",
                Password = "agdfadjfk",
                Email = "daniel@example.com",
                Telefono = "978865677"
            };
            var user3 = new Cls_Usuarios
            {
                Nombre = "Jose",
                Rol = "Cliente",
                Password = "agdfajffk",
                Email = "jose@example.com",
                Telefono = "978267677"
            };
            client.Insert_User(user);
            client.Insert_User(user1);
            client.Insert_User(user2);
            client.Insert_User(user3);

            var usuarios = client.Get_Users();
            Assert.IsTrue(usuarios.Exists(u => u.Email == "juan@example.com"));
        }

        [TestMethod]
        public void Test_1_1_Insert_Insumo()
        {
            var Insumo = new Cls_Insumos
            {
                Nombre = "Papa",
                Unidad_Medida = "kg",
                Stock_Disponible = 45.25f,
                Stock_Minimo = 10
            };
            var Insumo1 = new Cls_Insumos
            {
                Nombre = "Fideo",
                Unidad_Medida = "kg",
                Stock_Disponible = 30.5f,
                Stock_Minimo = 10
            };
            var Insumo2 = new Cls_Insumos
            {
                Nombre = "Tomate",
                Unidad_Medida = "kg",
                Stock_Disponible = 25.75f,
                Stock_Minimo = 10
            };
            var Insumo3 = new Cls_Insumos
            {
                Nombre = "Arroz",
                Unidad_Medida = "kg",
                Stock_Disponible = 50.25f,
                Stock_Minimo = 10
            };
            var Insumo4 = new Cls_Insumos
            {
                Nombre = "Aceite",
                Unidad_Medida = "lt",
                Stock_Disponible = 44.5f,
                Stock_Minimo = 10
            };
            var Insumo5 = new Cls_Insumos
            {
                Nombre = "Leche",
                Unidad_Medida = "lt",
                Stock_Disponible = 28.75f,
                Stock_Minimo = 10
            };
            var Insumo6 = new Cls_Insumos
            {
                Nombre = "Cebolla",
                Unidad_Medida = "kg",
                Stock_Disponible = 37.75f,
                Stock_Minimo = 10
            };
            var Insumo7 = new Cls_Insumos
            {
                Nombre = "Salchicha",
                Unidad_Medida = "kg",
                Stock_Disponible = 20.75f,
                Stock_Minimo = 10
            };
            var Insumo8 = new Cls_Insumos
            {
                Nombre = "Camote",
                Unidad_Medida = "kg",
                Stock_Disponible = 20.75f,
                Stock_Minimo = 10
            };

            client.Insert_Insumo(Insumo, 2);  // id:1  = Papa
            client.Insert_Insumo(Insumo1, 2);  // id:2  = Fideo
            client.Insert_Insumo(Insumo2, 2);  // id:3  = Tomate
            client.Insert_Insumo(Insumo3, 2);  // id:4  = Arroz
            client.Insert_Insumo(Insumo4, 2);  // id:5  = Aceite
            client.Insert_Insumo(Insumo5, 2);  // id:6  = Leche
            client.Insert_Insumo(Insumo6, 2);  // id:7  = Cebolla
            client.Insert_Insumo(Insumo7, 2);  // id:8  = Salchicha
            client.Insert_Insumo(Insumo8, 2);  // id:9  = Camote

            var insumos = client.Get_Insumos();
            Assert.IsTrue(insumos.Exists(i => i.Nombre == "Papa"));
        }

        [TestMethod]
        public void Test_1_2_Insert_Platos()
        {

            List<Cls_Recetario> receta1 = new List<Cls_Recetario>
            {
                new Cls_Recetario { Id_Insumo = 1, Cantidad_Necesaria = 0.20f },
                new Cls_Recetario { Id_Insumo = 2, Cantidad_Necesaria = 0.30f },
                new Cls_Recetario { Id_Insumo = 8, Cantidad_Necesaria = 0.15f }
            };
            var plato1 = new Cls_Platos
            {
                Nombre = "Salchi Papa",
                Descripcion = "porcion de tallarin con salchichas y papas fritas",
                Precio = 8.5f,
                Tiempo_Preparacion = 15,
                Activo = true,
                Recetario = receta1,
                Imagen = File.Exists("D:\\imagen.png") ? File.ReadAllBytes("D:\\imagen.png") : new byte[0],
                txt_Nombre_Imagen = "imagen.png"
            };
            List<Cls_Recetario> receta2 = new List<Cls_Recetario>
            {
                new Cls_Recetario { Id_Insumo = 3, Cantidad_Necesaria = 0.10f },
                new Cls_Recetario { Id_Insumo = 7, Cantidad_Necesaria = 0.10f },
                new Cls_Recetario { Id_Insumo = 4, Cantidad_Necesaria = 0.25f }
            };
            var plato2 = new Cls_Platos
            {
                Nombre = "Saltado de Res",
                Descripcion = "porcion de arroz con papas fritas y saltado de res",
                Precio = 12.5f,
                Tiempo_Preparacion = 20,
                Activo = true,
                Recetario = receta2,
                Imagen = File.Exists("D:\\imagen.png") ? File.ReadAllBytes("D:\\imagen.png") : new byte[0],
                txt_Nombre_Imagen = "imagen.png"
            };
            List<Cls_Recetario> receta3 = new List<Cls_Recetario>
            {
                new Cls_Recetario { Id_Insumo = 3, Cantidad_Necesaria = 0.10f },
                new Cls_Recetario { Id_Insumo = 7, Cantidad_Necesaria = 0.10f },
                new Cls_Recetario { Id_Insumo = 4, Cantidad_Necesaria = 0.25f }
            };
            var plato3 = new Cls_Platos
            {
                Nombre = "Caldo de arroz",
                Descripcion = "Tazon de caldo de arroz con papa y porcion de pollo",
                Precio = 9.5f,
                Tiempo_Preparacion = 20,
                Activo = true,
                Recetario = receta3,
                Imagen = File.Exists("D:\\imagen.png") ? File.ReadAllBytes("D:\\imagen.png") : new byte[0],
                txt_Nombre_Imagen = "imagen.png"
            };
            List<Cls_Recetario> receta4 = new List<Cls_Recetario>
            {
                new Cls_Recetario { Id_Insumo = 2, Cantidad_Necesaria = 0.10f },
                new Cls_Recetario { Id_Insumo = 4, Cantidad_Necesaria = 0.10f },
                new Cls_Recetario { Id_Insumo = 6, Cantidad_Necesaria = 0.25f }
            };
            var plato4 = new Cls_Platos
            {
                Nombre = "Arroz con pollo",
                Descripcion = "Tazon de arroz con papa y pollo frito",
                Precio = 10.5f,
                Tiempo_Preparacion = 20,
                Activo = true,
                Recetario = receta4,
                Imagen = File.Exists("D:\\imagen.png") ? File.ReadAllBytes("D:\\imagen.png") : new byte[0],
                txt_Nombre_Imagen = "imagen.png"
            };
            client.Insert_Plato(plato1);
            client.Insert_Plato(plato2);
            client.Insert_Plato(plato3);
            client.Insert_Plato(plato4);

            var platos = client.Get_Platos();
            Assert.IsTrue(platos.Exists(i => i.Nombre == "Salchi Papa"));
        }

        [TestMethod]
        public void Test_1_3_Insert_Promociones()
        {
            var promocion1 = new Cls_Promociones
            {
                Id_Plato = 1,
                Nombre = "Salchi Papa para compartir",
                Cantidad_Aplicable = 2,
                Descuento = 0.08f,
                Fecha_Inicio = new DateTime(2025, 7, 30),
                Fecha_Fin = new DateTime(2025, 8, 30),
                Activo = true
            };
            var promocion2 = new Cls_Promociones
            {
                Id_Plato = 2,
                Nombre = "Saltado, promocion de temporada",
                Cantidad_Aplicable = 3,
                Descuento = 0.10f,
                Fecha_Inicio = new DateTime(2025, 8, 10),
                Fecha_Fin = new DateTime(2025, 9, 1),
                Activo = true
            };
            var promocion3 = new Cls_Promociones
            {
                Id_Plato = 3,
                Nombre = "Caldo con porcion de pollo, promocion de temporada",
                Cantidad_Aplicable = 2,
                Descuento = 0.10f,
                Fecha_Inicio = new DateTime(2025, 8, 12),
                Fecha_Fin = new DateTime(2025, 9, 11),
                Activo = true
            };

            client.Insert_Promocion(promocion1);
            client.Insert_Promocion(promocion2);
            client.Insert_Promocion(promocion3);

            var promociones = client.Get_Promociones(true);
            Assert.IsTrue(promociones.Exists(i => i.Nombre == "Salchi Papa para compartir"));
        }

        [TestMethod]
        public void Test_1_4_Insert_Ventas()
        {
            List<Cls_DetalleVenta> Detalle1 = new List<Cls_DetalleVenta>
            {
                new Cls_DetalleVenta { Id_Plato = 1,Precio_Unitario = 8.50f, Cantidad = 2, Descuento = null , Id_Promocion = null },
                new Cls_DetalleVenta { Id_Plato = 2,Precio_Unitario = 12.50f, Cantidad = 3, Descuento = 0.10f ,Id_Promocion = 2}
            };
            var venta1 = new Cls_Ventas
            {
                Id_Trabajador = 2,
                Id_Cliente = 3,
                DetalleVenta = Detalle1
            };
            List<Cls_DetalleVenta> Detalle2 = new List<Cls_DetalleVenta>
            {
                new Cls_DetalleVenta { Id_Plato = 2,Precio_Unitario = 12.50f, Cantidad = 2, Descuento = null , Id_Promocion = null},
                new Cls_DetalleVenta { Id_Plato = 3,Precio_Unitario = 9.50f, Cantidad = 3, Descuento = 0.10f , Id_Promocion = 3 }
            };
            var venta2 = new Cls_Ventas
            {
                Id_Trabajador = null,
                Id_Cliente = 3,
                DetalleVenta = Detalle2
            };
            List<Cls_DetalleVenta> Detalle3 = new List<Cls_DetalleVenta>
            {
                new Cls_DetalleVenta { Id_Plato = 1,Precio_Unitario = 8.50f, Cantidad = 2, Descuento = null , Id_Promocion = null},
                new Cls_DetalleVenta { Id_Plato = 3,Precio_Unitario = 9.50f, Cantidad = 3, Descuento = 0.10f ,Id_Promocion = 3}
            };
            var venta3 = new Cls_Ventas
            {
                Id_Trabajador = 2,
                Id_Cliente = null,
                DetalleVenta = Detalle3
            };

            client.Insert_Venta(venta1);
            client.Insert_Venta(venta2);
            client.Insert_Venta(venta3);

            var ventas = client.Get_Ventas(DateTime.Today, "");
            Assert.IsTrue(ventas.Exists(i => i.Id_Venta == 1));
        }

        [TestMethod]
        public void Test_1_5_Insert_Compras()
        {
            List<Cls_DetalleCompras> Detalle1 = new List<Cls_DetalleCompras>
            {
                new Cls_DetalleCompras { Id_Insumo = 1,Costo_Unitario = 7.50f, Cantidad = 12},
                new Cls_DetalleCompras { Id_Insumo = 3,Costo_Unitario = 9.50f, Cantidad = 13},
                new Cls_DetalleCompras { Id_Insumo = 4,Costo_Unitario = 10.50f, Cantidad = 12}
            };
            var compra1 = new Cls_Compras
            {
                Id_Usuario = 1,
                DetalleCompras = Detalle1,
            };
            List<Cls_DetalleCompras> Detalle2 = new List<Cls_DetalleCompras>
            {
                new Cls_DetalleCompras { Id_Insumo = 2,Costo_Unitario = 7.50f, Cantidad = 12},
                new Cls_DetalleCompras { Id_Insumo = 3,Costo_Unitario = 8.50f, Cantidad = 15},
                new Cls_DetalleCompras { Id_Insumo = 4,Costo_Unitario = 9.50f, Cantidad = 13}
            };
            var compra2 = new Cls_Compras
            {
                Id_Usuario = 1,
                DetalleCompras = Detalle2,
            };
            List<Cls_DetalleCompras> Detalle3 = new List<Cls_DetalleCompras>
            {
                new Cls_DetalleCompras { Id_Insumo = 4,Costo_Unitario = 7.50f, Cantidad = 16},
                new Cls_DetalleCompras { Id_Insumo = 2,Costo_Unitario = 8.50f, Cantidad = 17},
                new Cls_DetalleCompras { Id_Insumo = 5,Costo_Unitario = 9.50f, Cantidad = 18},
            };
            var compra3 = new Cls_Compras
            {
                Id_Usuario = 2,
                DetalleCompras = Detalle3,
            };

            client.Insert_Compra(compra1);
            client.Insert_Compra(compra2);
            client.Insert_Compra(compra3);

            var compras = client.Get_Compras();
            Assert.IsTrue(compras.Exists(i => i.Id_Compra == 1));
        }

        [TestMethod]
        public void Test_1_6_Insert_Comentario()
        {
            var comentario1 = new Cls_Comentarios
            {
                Id_Usuario = 3,
                Contexto = "Plato",
                Id_Relacionado = 1,
                Comentario = "Comentario plato 1"
            };
            var comentario2 = new Cls_Comentarios
            {
                Id_Usuario = 3,
                Contexto = "Plato",
                Id_Relacionado = 2,
                Comentario = "Comentario plato 2"
            };
            var comentario3 = new Cls_Comentarios
            {
                Id_Usuario = 3,
                Contexto = "Plato",
                Id_Relacionado = 3,
                Comentario = "Comentario plato 3"
            };
            var comentario4 = new Cls_Comentarios
            {
                Id_Usuario = 2,
                Contexto = "Servicio",
                Comentario = "Comentario Servicio 1"
            };
            var comentario5 = new Cls_Comentarios
            {
                Id_Usuario = 2,
                Contexto = "Servicio",
                Comentario = "Comentario Servicio 2"
            };
            var comentario6 = new Cls_Comentarios
            {
                Id_Usuario = 2,
                Contexto = "Servicio",
                Comentario = "Comentario Servicio 3"
            };

            client.Insert_Comentario(comentario1);
            client.Insert_Comentario(comentario2);
            client.Insert_Comentario(comentario3);
            client.Insert_Comentario(comentario4);
            client.Insert_Comentario(comentario5);
            client.Insert_Comentario(comentario6);

            var comentarios = client.Get_Comentarios("Plato", 1);
            Assert.IsTrue((comentarios.Count) >= 1);
        }

        [TestMethod]
        public void Test_2_0_Update_User()
        {
            var user = client.Search_User(4);
            user.Nombre = "Jose1";
            user.Rol = "Cliente";
            user.Password = "agdfajffk";
            user.Email = "jose1@example.com";
            user.Telefono = "978267677";

            client.Update_User(user);

            var usuarios = client.Get_Users();
            Assert.IsTrue(usuarios.Exists(u => u.Email == "jose1@example.com"));
        }

        [TestMethod]
        public void Test_2_1_Update_Insumo()
        {
            var Insumo = client.Search_Insumo(9);
            Insumo.Nombre = "Camote blanco";
            Insumo.Unidad_Medida = "kg";
            Insumo.Stock_Minimo = 15;

            client.Update_Insumo(Insumo);

            var insumo1 = client.Search_Insumo(9);
            Assert.IsTrue(insumo1.Nombre == "Camote blanco");
        }

        [TestMethod]
        public void Test_2_2_Update_Platos()
        {
            List<Cls_Recetario> receta = new List<Cls_Recetario>
            {
                new Cls_Recetario { Id_Insumo = 1, Cantidad_Necesaria = 0.20f },
                new Cls_Recetario { Id_Insumo = 6, Cantidad_Necesaria = 0.30f },
                new Cls_Recetario { Id_Insumo = 5, Cantidad_Necesaria = 0.15f }
            };

            var plato = client.Search_Plato(4);
            plato.Nombre = "Arroz con pollo frito";
            plato.Descripcion = "Tazon de arroz con papa y pollo frito y salsas";
            plato.Precio = 11.50f;
            plato.Tiempo_Preparacion = 20;
            plato.Activo = true;
            plato.Recetario = receta;

            client.Update_Plato(plato);

            var platos = client.Get_Platos();
            Assert.IsTrue(platos.Exists(i => i.Nombre == "Arroz con pollo frito"));
        }

        [TestMethod]
        public void Test_2_3_Update_Promocion()
        {
            var promocion = client.Search_Promocion(3);
            float descuento = promocion.Descuento;
            promocion.Id_Plato = 3;
            promocion.Nombre = "Caldo con porcion de huevo, promocion de temporada";
            promocion.Cantidad_Aplicable = 2;
            promocion.Descuento = 0.05f;
            promocion.Fecha_Inicio = new DateTime(2025, 8, 13);
            promocion.Fecha_Fin = new DateTime(2025, 9, 14);
            promocion.Activo = true;

            client.Update_Promocion(promocion);

            var promocion1 = client.Search_Promocion(3);
            Assert.IsTrue(promocion1.Descuento != descuento);
        }

        [TestMethod]
        public void Test_2_4_Update_Ventas()
        {
            List<Cls_DetalleVenta> Detalle = new List<Cls_DetalleVenta>
            {
                new Cls_DetalleVenta { Id_Plato = 1,Precio_Unitario = 8.50f, Cantidad = 4, Descuento = 0.08f},
                new Cls_DetalleVenta { Id_Plato = 2,Precio_Unitario = 12.50f, Cantidad = 2, Descuento = 0.10f },
                new Cls_DetalleVenta { Id_Plato = 3,Precio_Unitario = 10.50f, Cantidad = 3, Descuento = 0.10f }
            };
            var venta = client.Search_Venta(3);
            venta.DetalleVenta = Detalle;
            float precio = venta.Costo_Total;

            client.Update_Venta(venta);

            var venta1 = client.Search_Venta(3);
            Assert.IsTrue(venta1.Costo_Total != precio);
        }

        [TestMethod]
        public void Test_2_5_Update_Compras()
        {
            List<Cls_DetalleCompras> Detalle = new List<Cls_DetalleCompras>
            {
                new Cls_DetalleCompras { Id_Insumo = 3,Costo_Unitario = 7.50f, Cantidad = 20},
                new Cls_DetalleCompras { Id_Insumo = 4,Costo_Unitario = 8.50f, Cantidad = 18},
                new Cls_DetalleCompras { Id_Insumo = 5,Costo_Unitario = 9.50f, Cantidad = 16},
                new Cls_DetalleCompras { Id_Insumo = 6,Costo_Unitario = 10.50f, Cantidad = 14}
            };
            var compra = client.Search_Compra(3);
            compra.DetalleCompras = Detalle;
            float costo = compra.Costo_Total;

            client.Update_Compra(compra);

            var compra1 = client.Search_Compra(3);
            Assert.IsTrue(compra1.Costo_Total != costo);
        }

        [TestMethod]
        public void Test_2_6_Update_Estado_Ventas()
        {
            var venta = client.Search_Venta(3);
            venta.Estado = "Procesando";

            client.State_Venta(venta);

            var venta1 = client.Search_Venta(3);
            Assert.IsTrue(venta1.Estado == "Procesando");
        }

        [TestMethod]
        public void Test_2_7_Update_Estado_Compras()
        {
            var compra = client.Search_Compra(3);
            compra.Estado = "Validado";

            client.State_Compra(compra);

            var compra1 = client.Search_Compra(3);
            Assert.IsTrue(compra1.Estado == "Validado");
        }

        [TestMethod]
        public void Test_2_8_Update_Stock_Disponible()
        {
            List<Cls_Insumos> stock = new List<Cls_Insumos>
            {
                new Cls_Insumos{Id_Insumo = 1 , Stock_Disponible = 20},
                new Cls_Insumos{Id_Insumo = 2 , Stock_Disponible = 21},
                new Cls_Insumos{Id_Insumo = 3 , Stock_Disponible = 22},
                new Cls_Insumos{Id_Insumo = 4 , Stock_Disponible = 23}
            };

            var insumo = client.Search_Insumo(1);
            float existencias = insumo.Stock_Disponible;

            client.Stock_Insumos(stock, 1);

            var insumo1 = client.Search_Insumo(1);
            Assert.IsTrue(insumo1.Stock_Disponible != existencias);

        }

        [TestMethod]
        public void Test_3_0_Delete_Usuario()
        {

            try
            {
                var user = client.Search_User(4);

                client.Delete_User(user.Id_Usuario);

                client.Search_User(4);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_3_1_Delete_Insumo()
        {

            try
            {
                var insumo = client.Search_Insumo(9);

                client.Delete_Insumo(insumo.Id_Insumo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_3_2_Delete_Plato()
        {

            try
            {
                var plato = client.Search_Plato(4);

                client.Delete_Plato(plato.Id_Plato);

                client.Search_Plato(4);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_3_3_Delete_Promocion()
        {

            try
            {
                var promocion = client.Search_Promocion(3);

                client.Delete_Promocion(promocion.Id_Plato);

                client.Search_Promocion(3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_3_4_Delete_Comentario()
        {
            client.Delete_Comentario(3);
            var comentarios = client.Get_Comentarios("Plato", 1);
            Assert.IsTrue(comentarios.Exists(i => i.Id_Comentario != 3));
        }

        [TestMethod]
        public void Test_4_0_Login_User()
        {
            try
            {
                var id = client.Login_User("daniel@example", "agdfadjfk");
                Console.WriteLine("Usuario: " + id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_4_1_Enviar_Token_User()
        {
            try
            {
                string Email = "daniel@example.com";
                string Usuario = "Daniel";
                client.Send_Token(Email, Usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_4_2_Actualizar_Password_User()
        {
            try
            {
                string Email = "daniel@example.com";
                string Token = "506198"; // codigo de 6 digitos enviado al correo
                string New_Password = "12345678";
                client.Update_Password(Email, Token, New_Password);

                var id = client.Login_User("daniel@example.com", "12345678");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
