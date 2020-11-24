using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebUI.Hubs;
using WebUI.Models;

namespace RealTime.Infrastructure.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly IConfiguration configuration;
        private readonly IHubContext<TablaHub> tablaHub;

        public ProductoRepositorio(IConfiguration configuration, IHubContext<TablaHub> tablaHub)
        {
            this.configuration = configuration;
            this.tablaHub = tablaHub;
        }

        public IEnumerable<Producto> Get()
        {
            List<Producto> lista = null;
            Producto producto = null;
            SqlDependency dependency = null;

            var connectionString = configuration.GetConnectionString("RealTimeDB");
            using SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT [Id],   
                [nombre],[precio],[stock] FROM [dbo].[productos]", cn);

                #region SqlDependencia

                cmd.Notification = null;
                dependency = new SqlDependency(cmd);
                dependency.OnChange += new OnChangeEventHandler(DetectarCambios);
                SqlDependency.Start(connectionString);

                #endregion

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    lista = new List<Producto>();
                    while (dr.Read())
                    {
                        producto = new Producto
                        {
                            Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? -1 : dr.GetInt32(dr.GetOrdinal("Id")),
                            Nombre = dr.IsDBNull(dr.GetOrdinal("nombre")) ? "nodata" : dr.GetString(dr.GetOrdinal("nombre")),
                            Precio = dr.IsDBNull(dr.GetOrdinal("precio")) ? Convert.ToDecimal(0) : dr.GetDecimal(dr.GetOrdinal("precio")),
                            Stock = dr.IsDBNull(dr.GetOrdinal("stock")) ? -1 : dr.GetInt32(dr.GetOrdinal("stock"))
                        };
                        lista.Add(producto);
                    }
                }
                return lista;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        private void DetectarCambios(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                tablaHub.Clients.All.SendAsync("CargarTabla");
            }
            Get();
        }
    }
}
