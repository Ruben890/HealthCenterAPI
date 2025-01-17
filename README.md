# Documentación de la API de HealthCenter

## Descripción General
La **API de HealthCenter** automatiza el proceso de descarga y procesamiento de datos de centros de salud desde el SNS (Sistema Nacional de Salud). Realiza web scraping al SNS para obtener la información de forma automatizada y rápida. Proporciona una respuesta JSON estructurada con detalles completos sobre los centros de salud y admite filtrado y configuración de la fuente de datos. Además, la API está configurada para actualizarse automáticamente a la última versión disponible utilizando un job programado diario.

---

## Características
- **Gestión Automatizada de Excel:**
  - Descarga diaria automatizada del archivo Excel del SNS.
  - Conversión del archivo Excel a un formato JSON.
- **Opciones Flexibles de Fuente de Datos:**
  - Uso directo del archivo Excel del SNS como fuente.
  - Mapeo e importación de datos en una base de datos relacional para consultas mejoradas.
- **Capacidades de Filtrado:**
  - Filtros basados en ubicación (por ejemplo, Provincia, Municipio).
  - Filtros basados en servicios (por ejemplo, Internet, Emergencias).
- **Fuente de Datos Configurable:**
  - Opciones de configuración para alternar entre fuentes de datos basadas en archivos o bases de datos.
- **Paginación Integrada:**
  - Respuestas paginadas configurables con los parámetros `pageNumber` y `pageSize`.

---

## Formato de Respuesta JSON
### Respuesta Estándar
La API devuelve datos con la siguiente estructura:

```json
{
    "Name": "CENTRO DE PRIMER NIVEL DE ATENCION LOS PERALEJOS",
    "Level": "PRIMER NIVEL",
    "TypeCenter": "CENTRO DE PRIMER NIVEL",
    "SRS": "METROPOLITANA",
    "Tel": "809-561-5749",
    "RNC": "NULL",
    "Email": "GERENCIADNO@HOTMAIL.COM",
    "Fax": "NULL",
    "OpeningYear": 2009,
    "lastRenovationYear": 0,
    "Managed_By": "SNS",
    "ServiceComplexity": "N/A",
    "Location": {
        "Province": "DISTRITO NACIONAL",
        "Municipality": "SANTO DOMINGO DE GUZMAN",
        "MunicipalDistrict": "SANTO DOMINGO DE GUZMÁN",
        "Sector": "PERALEJOS 1",
        "Address": "CALLE 33 NO 5  KM 13 AUTOPISTA DUARTE LOS PERALEJOS",
        "Neighborhood": "LOS PERALEJOS",
        "SubNeighborhood": "LOS PERALEJOS",
        "Area": "DISTRITO NACIONAL OESTE",
        "Zone": "LOS PERALEJOS",
        "Latitud": 18.504775,
        "Longitud": -69.996329
    },
    "Services": {
        "isOffices": true,
        "isDentistry": false,
        "isEmergency": true,
        "isLaboratory": false,
        "isSonography": false,
        "isPhysiotherapy": false,
        "isInternet": true,
        "Xray": false
    }
}
```

---

### Respuesta Paginada
Cuando se utiliza la paginación, la estructura incluye un objeto adicional `Pagination` que contiene información sobre el estado actual de la paginación:

```json
{
    "Details": [
        {
            "Name": "CENTRO DE PRIMER NIVEL DE ATENCION LOS PERALEJOS",
            "Level": "PRIMER NIVEL",
            "TypeCenter": "CENTRO DE PRIMER NIVEL",
            "SRS": "METROPOLITANA",
            "Tel": "809-561-5749",
            ...
        }
    ],
    "Pagination": {
        "TotalCount": 1866,
        "PageSize": 20,
        "CurrentPage": 1,
        "TotalPages": 94,
        "NextPage": 2
    }
}
```

---

## Configuración de Paginación
La paginación permite controlar cuántos elementos se devuelven por solicitud y en qué página se encuentran. Para ello, se utilizan los parámetros:
- **`pageNumber`**: Define el número de página a recuperar. (Por defecto: `1`).
- **`pageSize`**: Define el número de elementos por página. (Por defecto: `20`).

### Ejemplo de Solicitud con Paginación
**Endpoint:**
```
GET /api/HealthCenter/GetAllHealthCenter?pageNumber=2&pageSize=20
```

**Respuesta:**
```json
{
    "Details": [...],
    "Pagination": {
        "TotalCount": 1866,
        "PageSize": 20,
        "CurrentPage": 2,
        "TotalPages": 94,
        "NextPage": 3
    }
}
```

---


## Configuración
### Selección de Fuente de Datos
La fuente de datos puede configurarse en el archivo `application` segun el entorno que valla a utilizar:
```json
"DataSourceType": {
  "Database": false,
  "File": true
}
```
- **Database:** Usa datos importados a una base de datos relacional.
- **File:** Usa datos directamente del archivo Excel descargado del SNS.

### Parámetros Genéricos para Filtrado
Se admiten los siguientes parámetros de consulta:
- **SourceType:** Especifica la fuente de datos (`File` o `Database`).
- **Province:** Filtra por provincia.
- **Municipality:** Filtra por municipio.
- **Sector:** Filtra por sector.
- **Level:** Filtra por el nivel del centro de salud (por ejemplo, "PRIMER NIVEL").
- **TypeCenter:** Filtra por el tipo de centro (por ejemplo, "CENTRO DE PRIMER NIVEL").
- **Area:** Filtra por área geográfica.
- **Services:** Filtra por servicios disponibles (por ejemplo, `isOffices`, `isDentistry`).

Ejemplo de Objeto de Parámetros:
```json
{
    "SourceType": "File",
    "Province": "DISTRITO NACIONAL",
    "Level": "PRIMER NIVEL",
    "isEmergency": true
}
```

---

## Definiciones de Enum
### DataSourceType
Define las fuentes de datos disponibles:
- **File:** Archivos locales o en la nube.
- **Database:** Base de datos relacional.

---
