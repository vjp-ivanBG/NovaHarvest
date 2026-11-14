1. **Cliente y servidor.** Indica qué se ejecuta en el **cliente** y qué en el **servidor**

Cuando entro a la página de /status, el navegador es el cliente y lo que hace es pedirle la página al servidor, este recibe la peticion y luego ejecuta el código que escribí en C# para la respuesta, luego el navegador recibe el HTML y lo muestra con el estilo

cuando abres `/status`. Cuando NovaHarvest permita reservar producto, la regla *"no se
pueden reservar más kilos de los disponibles"*, ¿dónde debe comprobarse
obligatoriamente? ¿Por qué?

Porque sonp un usuario podria modificar lo que le sale en pantalla y saltarselo

2. **Estático o dinámico.** ¿Cuál de tus tres páginas podría ser un HTML estático y cuál no?
Justifícalo. Pon dos ejemplos de páginas que NovaHarvest necesitará y que **no** podrían
ser estáticas.

el /about no podria ser estatico porque tiene informacion del NovaHarvest que no cambia, el que si necesita ser dinamica es el /status porque muestra la fecha y hora

3. **Ejecución en el servidor.** Explica paso a paso cómo una petición a
`http://localhost:8080/status` acaba ejecutando tu código C#. ¿Qué cambiaría si
NovaHarvest funcionara con **CGI**? ¿Y con un **módulo embebido** en el servidor web?

Cuando escribo http://localhost:8080/status en el navegador, primero llega la peticion a nginx, nginx recibe la conexion y se la pasa al backend, que es donde tengo funcionando el asp.net con kestrel

si usara CGI, el servidor tendria que arrancar otro programa para atender la petición, con un módulo embebido, el código estaría integrado dentro del propio servidor web, en mi proyecto he separado nginx y Kestrel para que cada uno se encargue de una parte.

4. **Reverse proxy.** Haz `curl -I` (o mira la pestaña Red del navegador) a `/status` con
Docker y con `dotnet run`. ¿Qué cabecera `Server` aparece en cada caso y por qué? Para
el backend (`docker compose stop backend`) y pide `/status` y `/css/site.css`: ¿qué código
HTTP recibes y por qué? ¿Por qué `backend` no publica ningún puerto?

cuando entro en la web usando Docker, la petición llega primero a nginx y este la manda al backend, por eso al mirar la cabecera Server, debería salir nginx, aunque cuando arranco la aplicación directamente con dotnet run, la petición llega a Kestrel y debería aparecer Kestrel.

Para comprobarlo, he usado el comando curl -I y he hecho estas pruebas:

Con Docker: HTTP/1.1 200 OK

Con dotnet run: HTTP/1.1 200 OK
Server: Kestrel

Después de parar el backend, al entrar en /status y /css/site.css: HTTP/1.1 502 Bad Gateway

El backend no tiene un puerto publicado porque no quiero que se pueda entrar directamente desde fuera, es que todas las peticiones pasen primero por nginx, que es el que hace de entrada a la aplicación.

5. **nginx.** ¿Por qué nginx no sirve `site.css` ni `logo.png` por su cuenta? Cita dos
funciones que nginx podría aportar si NovaHarvest se publicara en Internet.

nginx no se encarga de servir el CSS ni el logo porque en mi configuración le he dicho que mande todas las peticiones al backend, es asp.net core quien busca esos archivos dentro de wwwroot y los envía al navegador

He separado estas tareas para que nginx se encargue de recibir las peticiones y el backend de generar las páginas y servir los archivos de la web.

Si en un futuro subiera NovaHarvest a Internet, nginx también me podría servir para poner HTTPS y proteger la conexión

6. **Tecnologías.** Si la cooperativa pidiera hacer el backend con otra de las tecnologías
*server-side* vistas en clase, ¿qué ficheros de tu repositorio cambiarían y cuáles se
quedarían igual?

Si tuviera que cambiar el asp.net core por otra tecnología de servidor que haya visto en clase, tendría que cambiar la parte del backend. Tendría que adaptar ese código y también el Dockerfile, porque cada tecnología necesita sus propios archivos y su forma de arrancar

La parte de nginx se queda prácticamente igual si el nuevo backend siguiera escuchando en el mismo puerto y mantuviera el nombre del servicio backend. También podría mantener la estructura de Docker Compose

7. **Marcado y código.** Copia de tu `Program.cs` el código de una de las páginas y señala
**dos** problemas concretos de generar HTML a base de cadenas. ¿Qué parte se repite en
las tres páginas?

En mi proyecto he escrito el HTML directamente dentro de Program.cs, usando C#. Por ejemplo, aquí puedo poner el código que uso para crear una de mis páginas:
a href="/status"

Haciendolo de esta manera tengo algunas desventajas, como que si me equivoco al poner una comilla o una etiqueta HTML, puede no saber encontrar dónde está el fallo. Otro es que tengo el HTML mezclado con el código C#, y cuando quiera cambiar el diseño puede resultar ser más difícil

También hay partes que se repiten en las distintas páginas como el encabezado, el logo, el menú y el pie de página, para no tener que escribir toda esa estructura cada vez, he usado una función que me ayuda a crear las páginas con el mismo diseño

8. **Docker.** ¿Cuántas imágenes y cuántos contenedores tienes con el entorno levantado?
Explica la diferencia usando tu proyecto. ¿Por qué la imagen final del backend no incluye el
SDK? ¿Qué pasaría sin el `.dockerignore`?

La imagen final no incluye el sdk porque solo lo necesito para compilar el proyecto. Una vez que ya tengo la aplicación preparada, no hace falta tener todas esas herramientas para ejecutarla

el archivo .dockerignore lo tengo para evitar que se copien carpetas como bin y obj. Sin el el Docker podría mandar esos archivos al preparar la imagen, ocupando más espacio y metiendo archivos que no hacen falta.

9. **Herramientas.** Haz una tabla con las herramientas que has usado en el proyecto y el
problema que resuelve cada una en él.

herramientas:

Visual Studio Code: Para escribir el código y tener todos los archivos ordenados.

C#: Para escribir el código que genera las páginas.

ASP.NET Core: Para crear la parte del servidor de mi web.

.NET 10 SDK: Para compilar el proyecto y comprobar que no tiene errores.

Kestrel: Para recibir las peticiones y poner en marcha la aplicación.

nginx: Para recibir las visitas y pasarlas al backend.

Docker: Para ejecutar cada parte del proyecto en su contenedor.

Docker Compose: Para arrancar los dos servicios con un solo comando.

Git y GitHub: Para guardar los cambios y poder volver a versiones anteriores.

HTML y CSS: Para montar las páginas y darles su aspecto.

Terminal de VS Code: Para escribir comandos y probar que todo funciona.

10. **Observación.** Compara la hora de `/status` con `dotnet run` y con Docker.
¿Coinciden? Explica por qué

No coinciden, en dotnet run si sale la hora que tengo exacta y con docker mucho antes, puede ser porque el ordenador usa la hora de España y el docker la utc porque me da una hora antes