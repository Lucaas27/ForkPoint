namespace ForkPoint.Application.Models.Exceptions;
public record CustomException(bool IsSuccess, int StatusCode, string Message);
