var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapStaticAssets();

app.MapGet("/", () =>
{
    string content = """
        <h1>Inicio</h1>
        <p>NovaHarvest nace para reducir el desperdicio de alimentos
        en el Valle del Jerte.</p>
        <p>Queremos conectar a los productores agrícolas con compradores
        interesados en aprovechar los excedentes.</p>
        <p>La plataforma permitirá dar una segunda oportunidad a frutas
        y verduras que no llegan a venderse.</p>
        <p>Nuestro objetivo es fomentar un consumo más sostenible
        y ayudar a la economía local.</p>
        """;

    return Results.Content(CreatePage("Inicio", content), "text/html");
});

app.MapGet("/about", () =>
{
    string content = """
        <h1>Acerca de</h1>
        <p><strong>Proyecto:</strong> NovaHarvest</p>
        <p><strong>Centro:</strong> IES Valle del Jerte</p>
        <p><strong>Autor:</strong> TU NOMBRE</p>
        <p><strong>Versión:</strong> 0.1.0</p>
        """;

    return Results.Content(CreatePage("Acerca de", content), "text/html");
});

app.MapGet("/status", () =>
{
    string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

    string content = $"""
        <h1>Estado del servidor</h1>
        <p><strong>Fecha y hora del servidor:</strong> {date}</p>
        <p>Página generada por ASP.NET Core (Kestrel)</p>
        """;

    return Results.Content(CreatePage("Estado del servidor", content), "text/html");
});

string CreatePage(string title, string content)
{
    return $$"""
        <!DOCTYPE html>
        <html lang="es">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>NovaHarvest - {{title}}</title>
            <link rel="stylesheet" href="/css/site.css">
        </head>
        <body>
            <header>
                <div class="brand">
                    <img src="/img/logo.png" alt="Logotipo de NovaHarvest">
                    <strong>NovaHarvest</strong>
                </div>
                <nav>
                    <a href="/">Inicio</a>
                    <a href="/about">Acerca de</a>
                    <a href="/status">Estado</a>
                </nav>
            </header>
            <main>
                {{content}}
            </main>
            <footer>
                NovaHarvest · IES Valle del Jerte · {{DateTime.Now.Year}}
            </footer>
        </body>
        </html>
        """;
}

app.Run();
