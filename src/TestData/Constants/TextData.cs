using System.Diagnostics.CodeAnalysis;

namespace Cts.TestData.Constants;

[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded")]
public static class TextData
{
    // Text constants
    public const string ValidName = "abc";
    public const string ShortName = "z";
    public const string NewValidName = "def";
    public const string NonExistentName = "zzz";
    public const string ValidEmail = "test@example.net";
    public const string ValidUrl = "https://example.net";
    public const string ValidPdfFileName = "abc.pdf";
    public const string ValidPdfFileExtension = ".pdf";
    public const string ValidTextFileName = "abc.txt";
    public const string ValidTextFileExtension = ".txt";
    public const string ValidImageFileName = "abc.png";
    public const string ValidImageFileExtension = ".png";
    public const string InValidFileName = "abc.zip";
    public const string ValidPhoneNumber = "404-555-1212";
    public const string AlternatePhoneNumber = "678-555-1212";
    public const string AdditionalPhoneNumber = "770-555-1212";

    // GUIDs

    public static readonly Guid TestGuid = new("99999999-0000-0000-0000-000000000009");

    // Words and phrases generated from [Cupcake Ipsum](https://cupcakeipsum.com/).
    public const string Word = "Cupcake";
    public const string AnotherWord = "Cookie";
    public const string ThirdWord = "Apple";
    public const string EmojiWord = "🙁 + 🕶 = 😎";

    public const string ShortPhrase = "Chocolate bar cookie";
    public const string AnotherShortPhrase = "Brownie croissant";
    public const string Phrase = "Pudding bear claw I love liquorice pie fruitcake.";

    public static readonly string ShortMultiline = "Cake pastry pie I love chocolate cake." +
        Environment.NewLine +
        "Pudding ice cream chocolate sweet roll jelly.";

    public const string Paragraph = "Oat cake gummi bears danish I love tart muffin bonbon I love. Danish tiramisu " +
        "tootsie roll tart marshmallow icing tootsie roll. Shortbread tiramisu tiramisu chocolate bar biscuit. " +
        "Liquorice I love biscuit bonbon jujubes croissant.";

    public static readonly string MultipleParagraphs = "Dessert cheesecake ice cream fruitcake chocolate bar cookie." +
        Environment.NewLine +
        Environment.NewLine +
        "Lemon drops brownie croissant sesame snaps marshmallow caramels. Gummy bears lollipop icing jelly-o toffee " +
        "candy. Biscuit tootsie roll ice cream muffin macaroon. Brownie donut toffee danish sugar plum " +
        "candy. Oat cake muffin tart bear claw bonbon lollipop. Marshmallow donut icing chocolate bar dessert." +
        Environment.NewLine +
        Environment.NewLine +
        "Dessert cheesecake ice cream fruitcake chocolate bar pie cookie. Croissant marzipan jelly cupcake cupcake " +
        "lemon drops jelly-o. Shortbread donut pie.";
}
