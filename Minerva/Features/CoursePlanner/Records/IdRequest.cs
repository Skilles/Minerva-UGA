namespace Minerva.Features.CoursePlanner.Records;

public class IdRequest<T>
{
    public T Id { get; set; }
    
    public IdRequest()
    {
        Id = default!;
    }
}