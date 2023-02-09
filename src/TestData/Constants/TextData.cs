namespace Cts.TestData.Constants;

// Words and phrases generated from [Cupcake Ipsum](https://cupcakeipsum.com/).
public static class TextData
{
    // Basics
    public const string? NullString = null;
    public const string Empty = "";

    // Words and phrases
    public const string Word = "Croissant";
    public const string AnotherWord = "Lollipop";
    public const string ThirdWord = "Pie";

    public const string Phrase = "Soufflé croissant caramels gummi bears marzipan.";

    public static readonly string ShortMultiline = "Soufflé jelly gummies shortbread." +
        Environment.NewLine +
        "Pudding liquorice I love apple pie.";

    public const string Paragraph = "Icing toffee bear claw caramels soufflé croissant. Marshmallow " +
        "candy cotton candy brownie chocolate candy wafer. Powder candy chocolate bar cake bonbon. " +
        "Caramels lemon drops caramels jelly gummies cupcake chocolate bar gummi bears icing.";

    public static readonly string MultipleParagraphs = "Gummies powder macaroon powder biscuit. Cake marzipan " +
        Environment.NewLine +
        Environment.NewLine +
        "Bear claw lemon drops macaroon halvah gingerbread oat cake powder tiramisu biscuit. Powder " +
        "sesame snaps biscuit dessert cotton candy I love macaroon cake. Ice cream cookie topping " +
        "carrot cake sugar plum ice cream. Shortbread I love ice cream pudding gummi bears." +
        Environment.NewLine +
        Environment.NewLine +
        "Biscuit carrot cake macaroon marzipan jelly tiramisu danish.";
}
