# 🔒 Forzar ruta correcta del proyecto
$root = "C:\ReposGit\ASECCC-Digital\ASECCC Digital"
Set-Location $root

$version = Read-Host "Version (ej: 1.1.0)"
$fecha = Get-Date -Format "yyyy-MM-dd"

$modulo = Read-Host "Modulo (aportes, prestamos, ahorros)"
$rol = Read-Host "Rol (admin, asociado)"

$titulo = Read-Host "Titulo del cambio"
$added = Read-Host "Added (coma)"
$changed = Read-Host "Changed (coma)"
$fixed = Read-Host "Fixed (coma)"
$detalle = Read-Host "Descripcion tecnica"

function FormatearLista($texto) {
    if ([string]::IsNullOrWhiteSpace($texto)) { return "" }
    return ($texto -split ",") | ForEach-Object { "- " + $_.Trim() }
}

# ======================
# CHANGELOG
# ======================
$nuevoChangelog = @"
## [$version] - $fecha

### Added
$(FormatearLista $added)

### Changed
$(FormatearLista $changed)

### Fixed
$(FormatearLista $fixed)


"@

$changelogPath = "CHANGELOG.md"

if (!(Test-Path $changelogPath)) {
    Set-Content $changelogPath "# Changelog`n"
}

$contenidoActual = Get-Content $changelogPath -Raw
Set-Content $changelogPath ($nuevoChangelog + $contenidoActual)

# ======================
# DOCS POR MODULO Y ROL
# ======================
$rutaBase = "docs\cambios\$modulo\$rol"

if (!(Test-Path $rutaBase)) {
    New-Item -ItemType Directory -Path $rutaBase -Force | Out-Null
}

$titulo = $titulo -replace '[\\/:*?"<>|]', ''
$nombreArchivo = "$fecha-$($titulo -replace ' ','-').md"
$rutaFinal = Join-Path $rutaBase $nombreArchivo

$nuevoDoc = @"
# $titulo

## Fecha
$fecha

## Version
$version

## Modulo
$modulo

## Rol
$rol

## Cambios

### Added
$(FormatearLista $added)

### Changed
$(FormatearLista $changed)

### Fixed
$(FormatearLista $fixed)

## Descripcion tecnica
$detalle
"@

Set-Content -Path $rutaFinal -Value $nuevoDoc

Write-Host "`n CHANGELOG actualizado"
Write-Host "Documento creado en: $rutaFinal"