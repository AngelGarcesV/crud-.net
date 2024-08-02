namespace webApi.models;

public class orden{
    public Guid IdOrden{get;set;}
    public Guid IdCliente{get;set;}
    public Guid IdLibro{get;set;}
    public DateTime dateOrden {get;set;}
    public DateOnly dateDevolucion {get;set;}
    public virtual libro libros {get;set;}
    public virtual cliente clientes {get;set;}
}