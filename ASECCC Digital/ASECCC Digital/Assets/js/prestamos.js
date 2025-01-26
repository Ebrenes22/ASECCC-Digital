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
function calcularCuota() {
    var monto = parseFloat(document.getElementById("montoSolicitud").value);
    var plazo = parseFloat(document.getElementById("plazoMeses").value);

    if (!isNaN(monto) && !isNaN(plazo) && plazo > 0) {
        var cuota = (monto / (plazo * 4)) + (monto * 0.12);
        document.getElementById("cuota").value = cuota.toFixed(2);
    } else {
        document.getElementById("cuota").value = ''
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