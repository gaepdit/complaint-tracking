namespace ComplaintTracking
{
    public static class RegexPatterns
    {
        // Official DNR email addresses:
        //   user.name@dnr.ga.gov
        //   user.name@dnr.state.ga.us
        //   user.name@gadnr.org
        //   user.name@gaepd.com
        //   user.name@gaepd.org
        //   user.name@georgiaepd.com
        //   user.name@georgiaepd.org
        // -- Test regex here: https://regexr.com/3gpi3
        public const string DnrEmailPattern = @"^[\w\-\.]+@((dnr|gema)\.(ga\.gov|state\.ga\.us)|gadnr\.org|(ga|georgia)epd\.(com|org))$";

        // Postal Codes
        // -- Test regex here: https://regexr.com/3geep
        public const string USPostalCodePattern = @"^\d{5}(\-\d{4})?$";
        // -- https://forta.com/2003/11/28/regex-patterns-to-match-zip-codes-and-postal-codes/
        public const string CAPostalCodePattern = @"^[ABCEGHJKLMNPRSTVXY]\d[A-Z] \d[A-Z]\d$";
        public const string UKPostalCodePattern = @"^[A-Z]{1,2}\d[A-Z\d]? \d[ABD-HJLNP-UW-Z]{2}$";

        // PII removal (CTS-109)
        //-- Email RegEx: https://regexr.com/3h4ho
        public const string SimpleEmailPattern = @"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b";
        //-- Replacement email based on https://tools.ietf.org/html/rfc2606#section-2
        public const string EmailReplacementText = @"[email@removed.invalid]";
        //-- Phone number RegEx: https://regexr.com/3h4h0
        public const string SimplePhonePattern = @"\b\d{3}[- .]\d{4}\b";
        public const string PhoneReplacementText = @"xxx-xxxx";

    }
}
