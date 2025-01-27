//Funciones para seccion de alquiler
function mostrarCampoAlquiler() {
    document.getElementById("campoMontoAlquiler").style.display = "block";
}

// Función para ocultar el campo de monto de alquiler
function ocultarCampoAlquiler() {
    document.getElementById("campoMontoAlquiler").style.display = "none";
}

//Funciones para seccion de deudas
function mostrarCamposDeudas() {
    document.getElementById('camposDeudas').style.display = "block";
}

function ocultarCamposDeudas() {
    document.getElementById('camposDeudas').style.display = "none";
}

//Funciones para seccion de fiador

function mostrarCamposFiador() {
    document.getElementById('camposFiador').style.display = "block";
}

function ocultarCamposFiador() {
    document.getElementById('camposFiador').style.display = "none";
}



/*Funcion para calcular cuota de prestamo
Validar monto de aporte personal, antiguedad y tipo de prestamo
Urgente a 6 meses plazo, personal y 150%
Personal es al 100% de lo que tiene en aporte personal y maximo y plazo maximo 5 años
150% es 100% sobre aporte personal y 50% sobre aporte patronal,
validar antiguedad de asociado, si tiene mas de 10 años puede maximo 7 años , si no 5 años
*/

function formatearMonto(id) {
    const montoInput = document.getElementById(id);
    let monto = montoInput.value;

    // Eliminar cualquier carácter que no sea número o punto
    monto = monto.replace(/[^\d\.]/g, "");

    // Limitar el número de decimales a dos
    if (monto.indexOf('.') !== -1) {
        let partes = monto.split('.');
        partes[1] = partes[1].substring(0, 2);  // Limita a 2 decimales
        monto = partes.join('.');
    }

    // Formatear con separación de miles
    monto = monto.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    // Actualiza el valor del input
    montoInput.value = monto;

    // Limitar el número total de caracteres a 10 (incluyendo el punto y los decimales)
    if (monto.length > 10) {
        montoInput.value = monto.substring(0, 10);
    }
}
function ajustarPlazo() {
    const tipoPrestamo = document.getElementById("tipoPrestamo").value;
    const plazoMeses = document.getElementById("plazoMeses");

    // Verifica que la fechaIngreso esté disponible
    const fechaIngresoString = window.fechaIngresoString;
    if (fechaIngresoString) {
        const fechaIngresoParts = fechaIngresoString.split("-");
        const fechaIngreso = new Date(
            parseInt(fechaIngresoParts[2]), // Año
            parseInt(fechaIngresoParts[1]) - 1, // Mes
            parseInt(fechaIngresoParts[0]) // Día
        );

        const fechaActual = new Date();
        let antiguedadAnios = fechaActual.getFullYear() - fechaIngreso.getFullYear();

        // Ajuste por mes y día
        if (
            fechaActual.getMonth() < fechaIngreso.getMonth() ||
            (fechaActual.getMonth() === fechaIngreso.getMonth() && fechaActual.getDate() < fechaIngreso.getDate())
        ) {
            antiguedadAnios--;
        }

        // Ajustes según el tipo de préstamo
        if (tipoPrestamo === "Urgente") {
            plazoMeses.max = 6;
            plazoMeses.value = Math.min(plazoMeses.value, 6);
            plazoMeses.disabled = true;
        } else if (tipoPrestamo === "Personal") {
            plazoMeses.max = 60;
            plazoMeses.disabled = false;
        } else if (tipoPrestamo === "150%") {
            plazoMeses.max = antiguedadAnios >= 10 ? 84 : 60;
            plazoMeses.disabled = false;
        } else {
            plazoMeses.disabled = false;
        }
    } else {
        console.error("Fecha de ingreso no disponible en la sesión.");
    }
}

// Ejecutar ajuste inicial al cargar la página
document.addEventListener("DOMContentLoaded", () => {
    ajustarPlazo();
});



function calcularCuota() {
    const montoInput = document.getElementById("montoSolicitud");
    const plazoInput = document.getElementById("plazoMeses");
    const cuotaOutput = document.getElementById("cuotaSemanal");

    const monto = parseFloat(montoInput.value.replace(/,/g, ''));
    const plazo = parseInt(plazoInput.value);

    if (!isNaN(monto) && !isNaN(plazo) && plazo > 0) {

        //Calcular el interes
        const interes = monto * 0.12;
        // Calcula la cuota mensual
        const montoTotal = monto + interes;
        //Calcula semanas plazo
        const semanas = plazo * 4.34;
        // Divide entre 4 para obtener la cuota semanal
        const cuotaSemanal =  montoTotal/semanas;

        // Actualiza el campo de cuota semanal
        cuotaOutput.value = cuotaSemanal.toFixed(2);
    } else {
        // Si hay errores en el ingreso de datos, vacía el campo
        cuotaOutput.value = '';
    }
}




//Funcion para validar aceptacion de reglamento
const formulario = document.getElementById("solicitudForm");
const checkbox = document.getElementById("aceptoReglamento");
const mensajeError = document.getElementById("mensajeError");

formulario.addEventListener("submit", function (event) {
    // Verifica si el checkbox está marcado
    if (!checkbox.checked) {
        // Evita el envío del formulario
        event.preventDefault();

        // Muestra el mensaje de error
        mensajeError.style.display = "block";
    } else {
        // Oculta el mensaje si está marcado
        mensajeError.style.display = "none";
    }
});