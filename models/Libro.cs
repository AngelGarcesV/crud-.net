namespace webApi.models;

public class libro{
    public Guid IdLibro {get;set;}
    public string name {get;set;}
    public string author {get;set;}
    public DateOnly date {get;set;}
    public virtual ICollection<orden> ordenes {get;set;}
}