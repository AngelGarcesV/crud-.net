using System.Formats.Tar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi;
using webApi.models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

app.MapGet("/", () => DateTime.Now);

app.MapGet("/db", async ([FromServices] Context dbContext) => {
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos postgres: " + dbContext.Database.IsNpgsql());
});

///------CLIENTES------///

//crear cliente
app.MapPost("/cliente", async([FromServices] Context dbContext, [FromBodyAttribute] cliente clientes)=>{
    clientes.IdCliente = Guid.NewGuid();
    await dbContext.AddAsync(clientes);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});
//obtener todos los clientes
app.MapGet("/cliente", async ([FromServices] Context dbContext) => {
    var info = Results.Ok(dbContext.clientes);
    return info;
});

//actualizar clientes

app.MapPut("/cliente/{id}", async([FromServices] Context dbContext, [FromBodyAttribute] cliente clientes, [FromRoute] Guid id)=>{
    var info = dbContext.clientes.Find(id);
    if(info != null){
        info.IdCliente = clientes.IdCliente;
        info.age = clientes.age;
        info.name = clientes.name;
        await dbContext.SaveChangesAsync();
        return Results.Ok(dbContext.clientes.Find(id));
    }
    return Results.NotFound();
});


//Obtener por id de cliente
app.MapGet("/cliente/{id}", async ([FromServices] Context dbContext, [FromRoute] Guid id) => {
    var info = Results.Ok(dbContext.clientes.Find(id));
    return info;
});

//----------LIBROS----------//
//crear libro
app.MapPost("/libro", async([FromServices] Context dbContext, [FromBodyAttribute] libro libros)=>{
    libros.IdLibro = Guid.NewGuid();
    await dbContext.AddAsync(libros);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});
//obtener todos los libros
app.MapGet("/libro", async ([FromServices] Context dbContext) => {
    var info = Results.Ok(dbContext.libros);
    return info;
});

//obtener libro por id
app.MapGet("/libro/{id}", async([FromServices] Context dbContext, [FromRoute] Guid id )=>{
    var info = dbContext.libros.Find(id);
    if(info != null){
        return Results.Ok(info);
    }
    else{
        return Results.NotFound("No se encontro el cliente con id: "+id);
    }
});

//ACTUALIZAR CLEINTES
app.MapPut("cliente/{id}", async ([FromServices] Context dbContext, [FromRoute] Guid id, [FromBody] cliente clienteUpdate  )=>{
    var info = dbContext.clientes.Find(id);
    if(info != null){
        info.adress = clienteUpdate.adress;
        info.age = clienteUpdate.age;
        info.name = clienteUpdate.name;
        await dbContext.SaveChangesAsync();
        return Results.Ok(info);
    }
    return Results.NotFound("No se encontro el cliente con id: "+id);
});
///---------ORDENES---------///

//crear orden
app.MapPost("/orden", async([FromServices] Context dbContext, [FromBodyAttribute] orden ordenes)=>{
    ordenes.IdOrden = Guid.NewGuid();
    ordenes.dateOrden= DateTime.UtcNow;
    await dbContext.AddAsync(ordenes);
    await dbContext.SaveChangesAsync();
    return Results.Ok(dbContext.ordenes.Find(ordenes.IdOrden));
});

//actualizar una orden
app.MapPut("/orden/{id}", async([FromServices] Context dbContext, [FromBodyAttribute] orden ordenes, [FromRoute] Guid id)=>{
    var info = dbContext.ordenes.Find(id);
    if(info != null){
        info.IdCliente = ordenes.IdCliente;
        info.IdLibro = ordenes.IdLibro;
        info.dateDevolucion = ordenes.dateDevolucion;
        await dbContext.SaveChangesAsync();
        return Results.Ok(dbContext.ordenes.Find(id));
    }
    return Results.NotFound();
    
});
//obtener todas las ordenes
app.MapGet("/orden", async ([FromServices] Context dbContext) => {
    var info = Results.Ok(dbContext.ordenes);
    return info;
});

//Obtener por id de orden
app.MapGet("/orden/{id}", async ([FromServices] Context dbContext, [FromRoute] Guid id) => {
    var info = Results.Ok(dbContext.ordenes.Find(id));
    return info;
});

app.MapGet("/orden/cliente/{id}", async ([FromServices] Context dbContext, [FromRoute] Guid id) => {
    var info = Results.Ok(dbContext.ordenes.Where(p=>p.IdCliente == id));
    return info;
});

app.MapGet("/orden/cliente/{id}", async ([FromServices] Context dbContext, [FromRoute] Guid id) => {
    var info = Results.Ok(dbContext.ordenes.Where(p=>p.IdCliente == id));
    return info;
});


app.MapGet("/orden/menores", async ([FromServices] Context dbContext) => {
    var thresholdDate = new DateOnly(2024, 7, 1); // Fecha límite para comparación
    var info = dbContext.ordenes
                        .Where(p => p.dateDevolucion < thresholdDate) // Comparación con DateOnly
                        .ToList(); // Obtiene la lista de órdenes que cumplen con la condición

    return Results.Json(info);
});

app.MapDelete("orden/{id}", async([FromServices] Context dbContext, [FromRoute] Guid id)=>{
    var ordenActual = dbContext.ordenes.Find(id);
    if (ordenActual != null){
        dbContext.Remove(ordenActual);
        await dbContext.SaveChangesAsync();
        return Results.Ok("se elimino el registro con id: "+ id);
    }
    return Results.NotFound();
});
app.Run();
