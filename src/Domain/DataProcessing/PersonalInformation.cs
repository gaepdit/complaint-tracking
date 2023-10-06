namespace Cts.Domain.DataProcessing;

public static class PersonalInformation
{
    public static string? RedactPii(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        // Replacement email based on https://tools.ietf.org/html/rfc2606#section-2
        input = Regexes.EmailRegex().Replace(input, "[email@removed.invalid]");
        input = Regexes.PhoneRegex().Replace(input, "[phone number removed]");

        return input;
    }
}
