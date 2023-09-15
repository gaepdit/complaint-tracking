using System.Text.RegularExpressions;

namespace Cts.Domain.DataProcessing;

public static partial class Regexes
{
    //-- Email RegEx: https://regex101.com/r/umVx9J/1
    private const string SimpleEmailPattern = @"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b";

    //-- Phone number RegEx: https://regex101.com/r/xbOGha/1
    private const string SimplePhonePattern = @"\b\d{3}[- .]\d{4}\b";

    [GeneratedRegex(SimpleEmailPattern)]
    public static partial Regex EmailRegex();

    [GeneratedRegex(SimplePhonePattern)]
    public static partial Regex PhoneRegex();
}
