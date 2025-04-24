# API RESTful para Carga y Descarga de Archivos

Este proyecto es una API RESTful desarrollada con **.NET 8**, diseñada para permitir la **carga y descarga de archivos** en una base de datos SQL Server. La solución está contenida en un entorno **Dockerizado** y lista para ser ejecutada con un solo comando.

## 🚀 Ejecución Rápida (sin .env)

### 📋 Prerrequisitos

Asegúrate de tener instalado en tu máquina:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/) (normalmente incluido con Docker Desktop)

Una vez instalados, puedes ejecutar el proyecto desde la raíz usando uno de los siguientes scripts, pasando las variables como parámetros `clave=valor`:

### 🐧 macOS / Linux
```bash
bash ./run.sh db_user=sa db_password='TuPassword123!' db_name=FilesDb app_port=5201
```

### 🪟 Windows CMD
```cmd
run.bat db_user=sa db_password=TuPassword123! db_name=FilesDb app_port=5201
```

### 🆘 Ayuda
```bash
bash ./run.sh --help
```
```cmd
run.bat --help
```

⚠️ **Nota:** El script `run.bat` fue escrito para usuarios de Windows, pero no ha sido probado directamente en ese sistema operativo. Si encuentras inconvenientes, puedes adaptarlo libremente o reportar el problema.


### 🐳 Corriendo directamente usando Docker Compose

Si prefieres no usar los scripts (`run.sh` o `run.bat`), puedes ejecutar el proyecto directamente desde la raíz con Docker Compose:

#### 🐚 Unix/macOS/Linux/WSL

```bash
DB_USER=sa \
DB_PASSWORD='yourPassword123!' \
DB_NAME=FilesDb \
DB_PORT=1433 \
APP_PORT=5201 \
docker-compose up --build
```

#### 🪟 Windows (PowerShell)

```powershell
$env:DB_USER="sa"
$env:DB_PASSWORD="yourPassword123!"
$env:DB_NAME="FilesDb"
$env:DB_PORT="1433"
$env:APP_PORT="5201"
docker-compose up --build
```

> ✅ Esta alternativa ejecuta el proyecto sin necesidad de scripts, usando directamente Docker Compose con variables de entorno.

## 🔁 Restablecer entorno de Docker (opcional)

En caso de que necesites reiniciar completamente los contenedores y volúmenes de Docker para empezar desde cero, puedes ejecutar:

```bash
docker-compose down -v 
```

---

## 📑 Endpoints Disponibles

- `POST /api/files/upload` → Carga un archivo
- `GET /api/files/download/{id}` → Descarga un archivo por ID
- `GET /api/files/metadata/{id}` → Muestra la metadata de un archivo por ID (bonus)

Puedes probarlos desde Swagger:

👉 [http://localhost:5201/swagger/index.html](http://localhost:5201/swagger/index.html)

> 💡 Recuerda ajustar el puerto en la URL si usaste un valor diferente para `app_port`.

## 📌 Consideraciones de Idioma

Aunque este proyecto fue desarrollado en un contexto de habla hispana, los nombres de los endpoints y estructuras están en inglés porque fueron definidos así en los **requisitos del reto técnico**. Se optó por mantener consistencia con esos nombres para facilitar la evaluación.

## 🔐 Seguridad y manejo de credenciales

Para evitar exponer contraseñas o credenciales en archivos de configuración, este proyecto:

- No utiliza archivos `.env`
- Recibe las variables de entorno directamente como argumentos de ejecución
- Documenta claramente cómo pasar contraseñas que contienen caracteres especiales (usa comillas simples en bash)

## 🛠️ Notas de diseño y cumplimiento de requerimientos

Este proyecto fue desarrollado siguiendo buenas prácticas de arquitectura en capas, principios SOLID y orientado al cumplimiento de los requisitos del reto técnico. A continuación se detallan las decisiones clave de diseño y cómo se cumplieron tanto los requerimientos mandatorios como los extras opcionales.

### ✔️ Decisiones de diseño

- Separación entre capas: controladores, servicios, entidades, DTOs, etc.
- Principio de inversión de dependencias mediante `AddScoped<IFileService, FileService>()`
- Uso de `ILogger<T>` para trazabilidad de acciones
- Validaciones robustas con DataAnnotations y FluentValidation
- Middleware para manejo de errores global
- Documentación automática con Swagger (`/swagger/index.html`)
- Diseño simple sin patrones formales por simplicidad del reto, pero fácilmente extensible
- Proyecto contenedorizado con Docker y Docker Compose para facilitar su ejecución y despliegue local sin necesidad de instalaciones manuales

> 🔧 **Nota:** La carpeta `Migrations/` está incluida en el repositorio porque la base de datos se inicializa automáticamente al ejecutar la aplicación en Docker. Esto asegura que la estructura de la base de datos esté lista sin necesidad de pasos manuales adicionales, facilitando la revisión y la ejecución inmediata del proyecto.


### ✔️ Aplicación de principios SOLID

- S (Single Responsibility Principle): Cada clase tiene una única responsabilidad. Por ejemplo, FileService gestiona la lógica de archivos y FilesController solo se encarga del manejo HTTP.

- O (Open/Closed Principle): El diseño permite extender funcionalidades sin modificar código existente, como en las validaciones o el servicio de archivos.

- L (Liskov Substitution Principle): Las dependencias son accedidas vía interfaces (IFileService), lo que permite intercambiarlas sin romper la funcionalidad.

- I (Interface Segregation Principle): Las interfaces definidas son específicas y no fuerzan métodos innecesarios.

- D (Dependency Inversion Principle): Se inyectan abstracciones (interfaces) en lugar de clases concretas, configuradas desde Program.cs.

### 📌 Requerimientos funcionales implementados

| Funcionalidad requerida                                        | Implementación                                                                                      |
|----------------------------------------------------------------|------------------------------------------------------------------------------------------------------|
| Subir archivo (`POST /api/files/upload`)                       | Implementado en `FilesController.cs`, valida y guarda el archivo con metadata en base de datos     |
| Descargar archivo (`GET /api/files/download/{id}`)             | Implementado en `FilesController.cs`, devuelve archivo en formato binario como archivo adjunto (Content-Disposition: attachment)|
| Consultar metadata (`GET /api/files/metadata/{id}`)            | Implementado como funcionalidad adicional, devuelve nombre, tipo MIME y fecha de subida             |

### ✅ Requerimientos de diseño (mandatorios)

| Requerimiento                                              | Implementación                                                                                      |
|------------------------------------------------------------|------------------------------------------------------------------------------------------------------|
| Uso de patrones de diseño                                  | No se implementaron patrones formales debido a la simplicidad del reto, pero se aplicaron principios SOLID |
| Inyección de dependencias                                  | Configurada en `Program.cs` con `AddScoped<IFileService, FileService>()`                            |
| Separación clara entre capas                               | Controladores, servicios, DTOs, modelos de base de datos definidos y separados                      |
| Validaciones con DataAnnotations o FluentValidation        | Usado `[Required]` y reglas en `FileUploadDtoValidator`                             |
| Manejo de errores estructurado                             | Middleware para captura global de excepciones con respuesta consistente                             |
| Uso de `ILogger<T>`                                        | Inyectado y utilizado en `FileService` y `FilesController`                                          |
| Documentación con Swagger                                  | Configurado en `Program.cs` con Swashbuckle                                                        |

### 🌟 Extras valorados implementados

| Extra opcional                                             | Implementación                                                                                      |
|------------------------------------------------------------|------------------------------------------------------------------------------------------------------|
| Pruebas unitarias                                          | Pruebas básicas implementadas con `xUnit` para `FileService` y  `FileUploadDtoValidator`                                      |
| Archivo README                                             | README completo con instrucciones de ejecución, diseño y dependencias                               |
| Script SQL para tabla                                      | No incluido: se delega a EF Core para creación de esquema con migraciones                           |
| Repositorio de código                                      | Compartido en GitHub                                            |
| Autenticación básica                                       | No implementada: fuera del alcance del requerimiento base                                           |
| Endpoint adicional (bonus)                                 | Implementado `GET /api/files/metadata/{id}` para consultar la metadata del archivo                  |

## 📦 Tecnologías utilizadas

- .NET 8 (ASP.NET Core)
- Entity Framework Core
- SQL Server (contenedor)
- Docker / Docker Compose
- Swagger / Swashbuckle
- xUnit + FluentAssertions

---

📬 Para más detalles o comentarios sobre este proyecto, no dudes en contactarme