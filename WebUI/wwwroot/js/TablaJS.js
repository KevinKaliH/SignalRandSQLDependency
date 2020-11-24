
$(function () {
    crearTabla();
});

function crearTabla() {
    var tabla = $('#tbl-produccion');

    $.ajax({
        url: "/Home/GetProductos",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            tabla.empty();
            if (data.length > 0) {
                var thead = "<tr>";
                thead += "<th>id Producto</th>";
                thead += "<th>Nombre</th>";
                thead += "<th>Precio</th>";
                thead += "<th>Stock</th>";
                thead += "</tr>";
                tabla.append(thead);

                var array = [];
                var tbody = "";
                for (var i = 0; i < data.length; i++) {
                    tbody += "<tr>";
                    tbody += "<td>" + data[i].id + "</td>";
                    tbody += "<td>" + data[i].nombre + "</td>";
                    tbody += "<td>" + data[i].precio + "</td>";
                    tbody += "<td>" + data[i].stock + "</td>";
                    tbody += "</tr>";
                }
                array.push(tbody);
                tabla.append(array.join(""));
            }
        },
        error: function (e) {
            console.log(e);
        }
    });
}

var connection = new signalR.HubConnectionBuilder().withUrl("/TrabajoHub").build();

connection.on("CargarTabla", function (user, message) {
    crearTabla();
    console.log("tuani pues");
});

connection.start().then(function () {
    console.log("Ok funcionando bien");
}).catch(function (err) {
    return console.error(err.toString());
});
