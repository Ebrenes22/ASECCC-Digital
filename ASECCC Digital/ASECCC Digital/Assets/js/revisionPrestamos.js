$(document).ready(function () {
    // Al abrir el modal, obtener los datos de la solicitud
    $('#solicitudModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Elemento que activó el modal
        var solicitudId = button.data('id'); // Obtiene el id de la solicitud

        // Realizar la llamada AJAX para obtener los detalles de la solicitud
        $.ajax({
            url: '/PrestamosController/ObtenerDetallesSolicitud', // Ruta del controlador y acción
            type: 'GET',
            data: { id: solicitudId },
            success: function (data) {
                // Llenar el modal con los datos recibidos
                $('#usuarioId').text(data.UsuarioId);
                $('#estadoCivil').text(data.EstadoCivil);
                $('#pagaAlquiler').text(data.PagaAlquiler ? 'Sí' : 'No');
                $('#montoAlquiler').text(data.MontoAlquiler || 'N/A');
                $('#nombreAcreedor').text(data.NombreAcreedor || 'N/A');
                $('#totalCredito').text(data.TotalCredito || 'N/A');
                $('#abonoSemanal').text(data.AbonoSemanal || 'N/A');
                $('#saldoCredito').text(data.SaldoCredito || 'N/A');
                $('#nombreDeudor').text(data.NombreDeudor || 'N/A');
                $('#totalPrestamo').text(data.TotalPrestamo || 'N/A');
                $('#saldoPrestamo').text(data.SaldoPrestamo || 'N/A');
                $('#tipoPrestamo').text(data.TipoPrestamo);
                $('#montoSolicitud').val(data.MontoSolicitud);
                $('#plazoMeses').val(data.PlazoMeses);
                $('#cuotaSemanalSolicitud').val(data.CuotaSemanalSolicitud);
                $('#propositoPrestamo').text(data.PropositoPrestamo);
                $('#estado').val(data.EstadoSolicitud); // Asigna el estado a la opción seleccionada
            },
            error: function () {
                alert('Error al obtener los detalles de la solicitud.');
            }
        });
    });
});
