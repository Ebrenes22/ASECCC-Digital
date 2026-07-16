    $(function () {
        const loginError = window.loginConfig.loginError;

    if (loginError) {
        Swal.fire({
            icon: 'error',
            title: 'Inicio de sesión incorrecto',
            text: loginError,
            confirmButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
        }

    const $identificacion = $('#identificacion');
    const $tipoIdentificacion = $('#tipoIdentificacion');
    const $identificacionError = $('#identificacionError');
    const $resetIdentificacion = $('#ResetCedula');
    const $resetTipoIdentificacion = $('#ResetTipoIdentificacion');

    function limpiarNumeros(value) {
            return String(value || '').replace(/[^0-9]/g, '');
        }

    function formatearIdentificacion(value, tipo) {
        let limpio = limpiarNumeros(value);

    if (tipo === 'Cedula' || tipo === 'Nacional') {
        limpio = limpio.slice(0, 9);
    return limpio.replace(/^(\d{1})(\d{4})(\d+)/, '$1-$2-$3');
            }

    return limpio.slice(0, 12);
        }

    function validarIdentificacion(value, tipo) {
            if (tipo === 'Cedula' || tipo === 'Nacional') {
                return /^\d{1}-\d{4}-\d{4}$/.test(value);
            }

    if (tipo === 'DIMEX' || tipo === 'Dimex') {
                return /^\d{12}$/.test(value);
            }

    return false;
        }

    function actualizarPlaceholder($input, tipo) {
        $input.attr('placeholder', (tipo === 'DIMEX' || tipo === 'Dimex') ? '000000000000' : '0-0000-0000');
        }

    $identificacion.on('input', function () {
        $identificacion.val(formatearIdentificacion($identificacion.val(), $tipoIdentificacion.val()));
        });

    $tipoIdentificacion.on('change', function () {
        $identificacion.val('');
    actualizarPlaceholder($identificacion, $tipoIdentificacion.val());
        });

    $('#loginForm').on('submit', function (event) {
            const tipo = $tipoIdentificacion.val();
    const identificacion = $identificacion.val();

    if (!validarIdentificacion(identificacion, tipo)) {
        $identificacion.addClass('is-invalid');
    $identificacionError.text('Ingrese un número de identificación válido.');
    event.preventDefault();
    event.stopPropagation();
            } else {
        $identificacion.removeClass('is-invalid');
            }

    if (!this.checkValidity()) {
        event.preventDefault();
    event.stopPropagation();
            }

    this.classList.add('was-validated');
        });

    $('#togglePassword').on('click', function () {
            const $password = $('#contrasena');
    const $icon = $(this).find('i');
    const showing = $password.attr('type') === 'text';

    $password.attr('type', showing ? 'password' : 'text');
    $icon.toggleClass('bi-eye', showing);
    $icon.toggleClass('bi-eye-slash', !showing);
    $(this)
    .attr('aria-label', showing ? 'Mostrar contraseña' : 'Ocultar contraseña')
    .attr('title', showing ? 'Mostrar contraseña' : 'Ocultar contraseña');
        });

    $resetIdentificacion.on('input', function () {
        $resetIdentificacion.val(formatearIdentificacion($resetIdentificacion.val(), $resetTipoIdentificacion.val()));
        });

    $resetTipoIdentificacion.on('change', function () {
        $resetIdentificacion.val('');
    actualizarPlaceholder($resetIdentificacion, $resetTipoIdentificacion.val());
        });

    $('#sendResetEmailBtn').on('click', function () {
            const tipoIdentificacion = $resetTipoIdentificacion.val();
    const cedula = $resetIdentificacion.val();

    if (!validarIdentificacion(cedula, tipoIdentificacion)) {
        $resetIdentificacion.addClass('is-invalid');
    Swal.fire({
        icon: 'warning',
    title: 'Identificación inválida',
    text: 'Revise el formato del número de identificación.',
    confirmButtonColor: '#d33',
    confirmButtonText: 'Aceptar'
                });
    return;
            }

    $resetIdentificacion.removeClass('is-invalid');

    $.ajax({
            url: window.loginConfig.resetPasswordUrl,
    type: 'POST',
    data: {
        ResetTipoIdentificacion: tipoIdentificacion,
    ResetCedula: cedula
                },
    success: function (response) {
                    if (response.success) {
        $('#forgotPasswordModal').modal('hide');
    Swal.fire({
        icon: 'success',
    title: 'Correo enviado',
    text: 'Se ha enviado un correo con las instrucciones para restablecer su contraseña.',
    confirmButtonColor: '#0563bb',
    confirmButtonText: 'Aceptar'
                        });
                    } else {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: response.message,
            confirmButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
                    }
                },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error de conexión',
            text: 'Hubo un problema al procesar su solicitud. Intente nuevamente.',
            confirmButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
                }
            });
        });
    });