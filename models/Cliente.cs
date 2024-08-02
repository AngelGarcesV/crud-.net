namespace webApi.models;

public class cliente{
    public Guid IdCliente {get;set;}
    public string name{get;set;}
    public int age{get;set;}
    public string adress {get;set;}

    public virtual ICollection<orden> ordenes {get;set;}
}