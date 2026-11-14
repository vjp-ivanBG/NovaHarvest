# Proyecto NovaHarvest

## ¿De qué trata este proyecto?

En este proyecto hice la infraestructura de una aplicación web llamada NovaHarvest. La idea es ayudar a aprovechar los productos agrícolas que sobran en el Valle del Jerte para conectar a los productores con personas o establecimientos que puedan utilizarlos

En esta fase todavía no he desarrollado las funciones de reservas ni de gestión de productos. Solo he preparado los servidores y dejar montada la estructura para que la aplicación pueda funcionar.

Para hacerlo he usado nginx, ASP.NET Core, Kestrel y Docker.

## Arranque

### Arrancar el proyecto con Docker

Para iniciar el proyecto primero abro un terminal en la carpeta principal del repositorio. Después ejecuto este comando:

```bash
docker compose up -d --build
```

Con este comando se construyen las imágenes y se ponen en marcha los dos servicios que he configurado: `web` y `backend`.

Para comprobar que están funcionando uso:

```bash
docker compose ps
```

Una vez iniciado el proyecto, puedo entrar desde el navegador con las direcciones:

- Página de inicio: http://localhost:8080/
- Acerca de: http://localhost:8080/about
- Estado del servidor: http://localhost:8080/status

Cuando quiero apagar el proyecto uso:

```bash
docker compose down
```

### Ejecutar el proyecto sin Docker

También he preparado el proyecto para poder ejecutarlo directamente con .NET

Primero entro en la carpeta del proyecto:

```bash
cd backend/NovaHarvest.Web
```

Después vuelvo a restaurar las dependencias:

```bash
dotnet restore
```

Y para iniciar la aplicación uso:

```bash
dotnet run
```

La consola me indica la dirección local en la que se está ejecutando. Desde ahí puedo abrir las mismas páginas que cuando uso Docker por ejemplo.

Para detener la aplicación tengo que hacer `Ctrl + C` en el terminal.

## Arquitectura

Para montar el NovaHarvest he uso una arquitectura cliente-servidor. He separado el servidor web del servidor de aplicaciones para que cada uno tenga solo una función.

Estos son los tres elementos principales:

- Navegador: es donde el usuario abre la página y desde donde se envían las peticiones.
- nginx (web): es el servidor web que recibe las peticiones y las pasa al backend. Lo utilizo como reverse proxy.
- ASP.NET Core (backend): es donde tengo el código C#. Kestrel recibe las peticiones y la aplicación genera las páginas HTML.

### Esquema de funcionamiento

```text
          NAVEGADOR
               |
               | Petición HTTP
               v
       localhost:8080
               |
               v
       +---------------+
       |      WEB      |
       |     nginx     |
       |   Puerto 80   |
       +---------------+
               |
               | Reverse proxy
               | backend:8080
               v
       +---------------+
       |    BACKEND    |
       | ASP.NET Core  |
       |    Kestrel    |
       |  Puerto 8080  |
       +---------------+
               |
               v
       Genera la página
          en C#
               |
               v
          NAVEGADOR
```

### ¿Cómo funciona una petición?

Por ejemplo, cuando entro en `http://localhost:8080/status`, el navegador envía una petición al servidor nginx.

nginx recibe esa petición y la reenvía al servicio `backend` por el puerto 8080. Ahí Kestrel recibe la petición y ASP.NET Core ejecuta el código de la ruta `/status`.

El código C# genera la página con la fecha y la hora del servidor y devuelve el resultado. Finalmente, nginx reenvía la respuesta al navegador para que pueda verla.

Los archivos estáticos, como la hoja de estilos `site.css` y el logotipo `logo.png`, también los sirve ASP.NET Core.

### Puertos utilizados

| Servicio | Puerto interno | Puerto del ordenador |
|---|---|---|
| web (nginx) | 80 | 8080 |
| backend (Kestrel) | 8080 | No está publicado |

He configurado el backend sin publicar ningún puerto al exterior para que las peticiones pasan primero por nginx y no acceda directamente a Kestrel desde el ordenador.

## Tecnologías que he utilizado

Estas son las herramientas y tecnologías que he usado para realizar el proyecto:

- HTML: para crear la estructura de las páginas.
- CSS: para dar estilo y mejorar el aspecto de la web.
- C#: para generar el contenido de las páginas.
- ASP.NET Core 10:** para desarrollar el backend.
- Kestrel: para recibir y responder a las peticiones HTTP.
- nginx: para recibir las peticiones y actuar como reverse proxy.
- Docker: para crear y ejecutar los contenedores.
- Docker Compose: para iniciar los servicios desde un único comando.
- Visual Studio Code: para escribir y organizar los archivos del proyecto.
- Git y GitHub: para guardar los cambios y llevar un historial del trabajo.

## Estructura del proyecto

He organizado los archivos separando la parte de nginx de la aplicación ASP.NET Core. La estructura final que pide el enunciado es la siguiente:

```text
novaharvest-web/
├── .gitignore
├── README.md
├── compose.yaml
├── cuestiones-teoricas/
│   └── cuestiones-ut2.md
├── web/
│   ├── Dockerfile
│   └── nginx.conf
└── backend/
    ├── Dockerfile
    ├── .dockerignore
    └── NovaHarvest.Web/
        ├── NovaHarvest.Web.csproj
        ├── Program.cs
        ├── appsettings.json
        ├── appsettings.Development.json
        ├── Properties/
        │   └── launchSettings.json
        └── wwwroot/
            ├── css/
            │   └── site.css
            └── img/
                └── logo.png
```

Con esta organización tengo separada la configuración del servidor web, la configuración de Docker y el código de la aplicación.