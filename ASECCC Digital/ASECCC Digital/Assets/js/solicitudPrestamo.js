    // Sección de las validaciones y campos
function mostrarCampoAlquiler() {
    document.getElementById("campoMontoAlquiler").style.display = "block";
}

function ocultarCampoAlquiler() {
    document.getElementById("campoMontoAlquiler").style.display = "none";
}

function mostrarCamposDeudas() {
    document.getElementById('camposDeudas').style.display = "block";
}

function ocultarCamposDeudas() {
    document.getElementById('camposDeudas').style.display = "none";
}

function mostrarCamposFiador() {
    document.getElementById('camposFiador').style.display = "block";
}

function ocultarCamposFiador() {
    document.getElementById('camposFiador').style.display = "none";
}

// Función para formatear monto
function formatearMonto(id) {
    const montoInput = document.getElementById(id);
    let monto = montoInput.value;
    monto = monto.replace(/[^\d\.]/g, "");
    if (monto.indexOf('.') !== -1) {
        let partes = monto.split('.');
        partes[1] = partes[1].substring(0, 2);
        monto = partes.join('.');
    }
    monto = monto.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    montoInput.value = monto;
    if (monto.length > 10) {
        montoInput.value = monto.substring(0, 10);
    }
}

// Función para ajustar el plazo
function ajustarPlazo() {
    const tipoPrestamo = document.getElementById("tipoPrestamo").value;
    const plazoMeses = document.getElementById("plazoMeses");

    const fechaIngresoString = window.fechaIngresoString;
    if (fechaIngresoString) {
        const fechaIngresoParts = fechaIngresoString.split("-");
        const fechaIngreso = new Date(
            parseInt(fechaIngresoParts[2]),
            parseInt(fechaIngresoParts[1]) - 1,
            parseInt(fechaIngresoParts[0])
        );
        const fechaActual = new Date();
        let antiguedadAnios = fechaActual.getFullYear() - fechaIngreso.getFullYear();
        if (
            fechaActual.getMonth() < fechaIngreso.getMonth() ||
            (fechaActual.getMonth() === fechaIngreso.getMonth() && fechaActual.getDate() < fechaIngreso.getDate())
        ) {
            antiguedadAnios--;
        }

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
    }
}


// Función para validar aceptación de reglamento
const formulario = document.getElementById("solicitudForm");
const checkbox = document.getElementById("aceptoReglamento");
const mensajeError = document.getElementById("mensajeError");

formulario.addEventListener("submit", function (event) {
    if (!checkbox.checked) {
        event.preventDefault();
        mensajeError.style.display = "block";
    } else {
        mensajeError.style.display = "none";
    }
});
