using System.Collections.Generic;
using WebUI.Models;

namespace RealTime.Infrastructure.Repositorios
{
    public interface IProductoRepositorio
    {
        IEnumerable<Producto> Get();
    }
}