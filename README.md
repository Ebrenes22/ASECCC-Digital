ASECCC Digital

Sistema web desarrollado para la gestión integral de servicios y beneficios de los asociados de la Asociación Solidarista ASECCC. La plataforma permite la administración de préstamos, ahorros, aportes,
beneficios, estados de cuenta y gestión de usuarios mediante una interfaz web moderna y segura.

Descripción

ASECCC Digital es una aplicación web desarrollada con ASP.NET MVC y SQL Server, diseñada para digitalizar y optimizar los procesos administrativos de la asociación,
facilitando el acceso a la información tanto para asociados como para administradores.

Características Principales

Módulo de Asociados
Consulta de estado de cuenta personal.
Consulta de ahorros y aportes.
Consulta de préstamos activos.
Visualización de beneficios y servicios.
Actualización de información personal.
Recuperación de contraseña.

Módulo Administrativo
Administración de usuarios.
Gestión de asociados.
Gestión de préstamos.
Administración de ahorros y aportes.
Control de beneficios y servicios.
Generación de estados de cuenta.
Reportes administrativos.
Gestión de solicitudes.

Seguridad
Autenticación de usuarios.
Control de acceso basado en roles.
Protección de datos sensibles.
Gestión segura de sesiones.
Recuperación de credenciales.

Tecnologías Utilizadas

Backend
ASP.NET MVC 5
C#
Entity Framework
SQL Server

Frontend
HTML5
CSS3
Bootstrap
JavaScript
jQuery
SweetAlert2

Infraestructura
Microsoft Azure App Service
Azure SQL Database
GitHub
GitHub Actions (CI/CD)

Arquitectura del Proyecto
ASECCC Digital
│
├── Controllers/
│   ├── Administracion
│   ├── Prestamos
│   ├── Ahorros
│   ├── Aportes
│   └── Usuarios
│
├── Models/
│   ├── Servicios
│   ├── ViewModels
│   └── LogicaNegocio
│
├── Identity/
│   ├── Entidades
│   └── ModelosBD
│
├── Views/
│   ├── Administracion
│   ├── Prestamos
│   ├── EstadoCuenta
│   └── Usuarios
│
├── Scripts/
├── Content/
└── App_Data/

Roles del Sistema

Administrador

Permite:
Administración de asociados.
Gestión de préstamos.
Administración de ahorros.
Administración de aportes.
Gestión de beneficios.
Generación de reportes.
Gestión de usuarios.

Asociado

Permite:
Consultar estado de cuenta.
Revisar ahorros.
Consultar préstamos.
Visualizar beneficios.
Actualizar datos personales.
Estado de Cuenta

El sistema permite generar estados de cuenta que integran:

Información general del asociado.
Ahorros acumulados.
Aportes realizados.
Préstamos activos.
Beneficios y servicios.
Saldos y movimientos.

También ofrece exportación a PDF para consulta y respaldo.

Despliegue

El sistema se encuentra preparado para ser desplegado en:

Azure App Service

Autor

Eliecer Brenes Madrigal
Ingeniero en Sistemas de Computación
Desarrollador Full Stack .NET

Tecnologías principales:

ASP.NET MVC
.NET Core
C#
SQL Server
Azure
REST APIs

ASECCC Digital nace con el objetivo de modernizar los servicios de la asociación, 
mejorar la experiencia de los asociados y optimizar la gestión administrativa 
mediante herramientas digitales seguras, escalables y accesibles.
