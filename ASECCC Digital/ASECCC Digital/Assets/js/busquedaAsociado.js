const BusquedaAsociado = (() => {
    const buscarAsociado = async (nombre) => {
        try {
            const response = await fetch('/Asociados/BuscarAsociado', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ buscarNombre: nombre })
            });

            return await response.json();
        } catch (error) {
            console.error("Error en buscarAsociado:", error);
            return { success: false, message: "Error en la solicitud." };
        }
    };

    const obtenerSugerencias = (inputSelector, listaSelector, callback = null) => {
        const $input = $(inputSelector);
        const $lista = $(listaSelector);

        $input.on("input", function () {
            const texto = $(this).val().trim();
            if (texto.length >= 2) {
                $.ajax({
                    url: '/Asociados/BuscarSugerencias',
                    method: 'GET',
                    data: { texto },
                    success: function (data) {
                        $lista.empty();
                        if (data.length > 0) {
                            data.forEach(nombre => {
                                $lista.append(`<li class="list-group-item list-group-item-action">${nombre}</li>`);
                            });
                            $lista.show();
                        } else {
                            $lista.hide();
                        }
                    }
                });
            } else {
                $lista.hide();
            }
        });

        $lista.on("click", "li", function () {
            $input.val($(this).text());
            $lista.hide();
            if (callback) callback($(this).text());
        });

        $(document).on("click", function (e) {
            if (!$(e.target).closest(inputSelector + ", " + listaSelector).length) {
                $lista.hide();
            }
        });
    };

    return {
        buscarAsociado,
        obtenerSugerencias
    };
})();
