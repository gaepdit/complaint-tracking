using System.Text.RegularExpressions;

namespace Cts.Domain.DataProcessing;

public static class PersonalInformation
{
    //-- Email RegEx: https://regex101.com/r/umVx9J/1
    private const string SimpleEmailPattern = @"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b";

    //-- Replacement email based on https://tools.ietf.org/html/rfc2606#section-2
    private const string EmailReplacementText = "[email@removed.invalid]";

    //-- Phone number RegEx: https://regex101.com/r/xbOGha/1
    private const string SimplePhonePattern = @"\b\d{3}[- .]\d{4}\b";
    private const string PhoneReplacementText = "[phone number removed]";

    public static string? RedactPii(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        input = Regex.Replace(input, SimpleEmailPattern, EmailReplacementText);
        input = Regex.Replace(input, SimplePhonePattern, PhoneReplacementText);

        return input;
    }
}
