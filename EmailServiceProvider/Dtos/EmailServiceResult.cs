namespace EmailServiceProvider.Dtos;

public class EmailServiceResult
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
}

public class EmailServiceResult<T> : EmailServiceResult
{
    public T? Result { get; set; }
}