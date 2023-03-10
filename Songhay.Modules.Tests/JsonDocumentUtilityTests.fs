namespace Songhay.Modules.Tests

module JsonDocumentUtilityTests =

    open System.Text.Json

    open Xunit
    open FsToolkit.ErrorHandling
    open FsUnit.Xunit

    open Songhay.Modules.JsonDocumentUtility

    let jDoc = JsonDocument.Parse(@"
        {
            ""top"": {
                ""one"": ""this is first"",
                ""two"": ""this is second"",
                ""three"": {
                    ""p1"": ""this is three-point-one"",
                    ""p2"": ""this is three-point-two""
                }
            }
        }
    ")

    [<Fact>]
    let ``tryGetProperty document root child test``() =
        let result =
            jDoc.RootElement
            |> tryGetProperty "one"
        let elementOne = result |> Result.valueOr raise

        let expectedResult = "this is first"
        let actual = elementOne.GetString()
        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document root element traversal test``() =
        let result =
            jDoc.RootElement
            |> tryGetProperty "top"
            |> Result.bind (tryGetProperty "one")
        let elementOne = result |> Result.valueOr raise

        let expectedResult = "this is first"
        let actual = elementOne.GetString()
        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document traversal test``() =
        let result =
            jDoc.RootElement
            |> tryGetProperty "three"
            |> Result.bind (tryGetProperty "p1")
        let elementP1 = result |> Result.valueOr raise

        let expectedResult = "this is three-point-one"
        let actual = elementP1.GetString()
        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document array test``() =
        let jDocArray = JsonDocument.Parse("[{},{}]")
        let result =
            jDocArray.RootElement
            |> tryGetProperty "foo"
        result |> Result.isError |> should be True

    [<Fact>]
    let ``tryGetProperty document array item test``() =
        let jDocArray = JsonDocument.Parse(@"[{""zero"": null},{""zero"": ""naught""}]")
        let result =
            jDocArray.RootElement.Item(1)
            |> tryGetProperty "zero"
        let elementZero = result |> Result.valueOr raise

        let expectedResult = "naught"
        let actual = elementZero.GetString()
        actual |> should equal expectedResult
