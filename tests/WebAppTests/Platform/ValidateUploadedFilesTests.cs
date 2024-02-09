using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

// namespace WebAppTests.Platform;

// [TestFixture]
// [TestOf(typeof(ValidationHelper))]
// public class ValidateUploadedFilesTests
// {
//     private static readonly byte[] TestData = Encoding.UTF8.GetBytes(TextData.ShortName);
//     private static readonly Stream TestStream = new MemoryStream(TestData);
//
//     [Test]
//     public void ValidateUploadedFiles_Valid_ReturnsValidResult()
//     {
//         // Arrange
//         List<IFormFile> formFiles =
//         [
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileExtension),
//         ];
//
//         var modelState = new ModelStateDictionary();
//
//         // Act
//         formFiles.ValidateUploadedFiles(modelState);
//
//         // Assert
//         modelState.IsValid.Should().BeTrue();
//     }
//
//     [Test]
//     public void ValidateUploadedFiles_ZeroLengthFile_ReturnsValidResult()
//     {
//         // Arrange
//         List<IFormFile> formFiles =
//         [
//             new FormFile(Stream.Null, 0, 0, TextData.ValidName, TextData.ValidFileName),
//         ];
//
//         var modelState = new ModelStateDictionary();
//
//         // Act
//         formFiles.ValidateUploadedFiles(modelState);
//
//         // Assert
//         modelState.IsValid.Should().BeTrue();
//     }
//
//     [Test]
//     public void ValidateUploadedFiles_DisallowedType_ReturnsWrongTypeResult()
//     {
//         // Arrange
//         List<IFormFile> formFiles =
//         [
//             new FormFile(TestStream, 0, 0, TextData.ValidName, TextData.InValidFileName),
//             new FormFile(TestStream, 0, 0, TextData.ValidName, TextData.ValidFileName),
//         ];
//
//         var modelState = new ModelStateDictionary();
//
//         // Act
//         formFiles.ValidateUploadedFiles(modelState);
//
//         // Assert
//         using var scope = new AssertionScope();
//         modelState.IsValid.Should().BeFalse();
//         modelState.ErrorCount.Should().Be(1);
//         modelState[string.Empty]!.Errors[0].ErrorMessage.Should().StartWith("Invalid");
//     }
//
//     [Test]
//     public void ValidateUploadedFiles_TooManyFiles_ReturnsTooManyResult()
//     {
//         // Arrange
//         List<IFormFile> formFiles =
//         [ // Eleven files is more than ten allowed.
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//             new FormFile(TestStream, 0, 1, TextData.ValidName, TextData.ValidFileName),
//         ];
//
//
//         var modelState = new ModelStateDictionary();
//
//         // Act
//         formFiles.ValidateUploadedFiles(modelState);
//
//         // Assert
//         using var scope = new AssertionScope();
//         modelState.IsValid.Should().BeFalse();
//         modelState.ErrorCount.Should().Be(1);
//         modelState[string.Empty]!.Errors[0].ErrorMessage.Should().StartWith("No more");
//     }
// }
