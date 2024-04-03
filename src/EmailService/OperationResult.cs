namespace GaEpd.EmailService;

public record OperationResult
{
    private OperationResult() => Success = true;
    private OperationResult(string failureMessage) => FailureMessage = failureMessage;

    public bool Success { get; }
    public string FailureMessage { get; } = string.Empty;

    public static OperationResult SuccessResult() => new();
    public static OperationResult FailureResult(string failureMessage) => new(failureMessage);
}
