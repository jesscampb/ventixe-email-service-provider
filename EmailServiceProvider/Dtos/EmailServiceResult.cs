namespace EmailServiceProvider.Dtos;

public class EmailServiceResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }
}

public class EmailServiceResult<T> : EmailServiceResult
{
    public T? Result { get; set; }
}