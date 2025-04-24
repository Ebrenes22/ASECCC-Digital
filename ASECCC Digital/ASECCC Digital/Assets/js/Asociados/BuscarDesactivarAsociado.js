// Activar las sugerencias al escribir
BusquedaAsociado.obtenerSugerencias("#buscarNombre", "#sugerenciasNombres");

// Función para buscar asociado
function buscarAsociado() {
    const nombre = document.getElementById('buscarNombre').value.trim();
    if (!nombre) {
        alert("Por favor, ingrese un nombre.");
        return;
    }

    BusquedaAsociado.buscarAsociado(nombre).then(data => {
        if (data.success) {
            document.getElementById('resultadosBusqueda').style.display = 'block';
            document.getElementById('sinResultados').style.display = 'none';

            const estadoCapitalizado = data.estado.trim().charAt(0).toUpperCase() + data.estado.trim().slice(1);
            const esInactivo = data.estado.toLowerCase() === "inactivo";

            const boton = esInactivo
                ? `<button type="button" class="btn btn-secondary btn-sm" disabled>Inactivo</button>`
                : `<button type="button" class="btn btn-danger btn-sm" onclick="desactivarAsociado(${data.id})">Desactivar</button>`;

            const fila = `
                            <tr>
                                <td>${data.nombre}</td>
                                <td>${data.identificacion}</td>
                                <td>${data.correo}</td>
                                <td>${data.telefono}</td>
                                <td>${estadoCapitalizado}</td>
                                <td>${boton}</td>
                            </tr>`;

            document.getElementById("tablaResultados").innerHTML = fila;
        } else {
            document.getElementById('resultadosBusqueda').style.display = 'none';
            document.getElementById('sinResultados').style.display = 'block';
        }
    });
}

function desactivarAsociado(usuarioId) {
    Swal.fire({
        title: "¿Estás segura?",
        text: "Esta acción desactivará al asociado.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Sí, desactivar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch('/Asociados/BuscarDesactivarAsociado', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ usuarioId })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Asociado Desactivado',
                            text: 'El asociado ha sido desactivado correctamente.',
                            confirmButtonColor: '#3085d6'
                        }).then(() => buscarAsociado());
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: data.message
                        });
                    }
                })
                .catch(error => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ocurrió un problema al desactivar el usuario.'
                    });
                    console.error('Error en desactivarAsociado:', error);
                });
        }
    });
}