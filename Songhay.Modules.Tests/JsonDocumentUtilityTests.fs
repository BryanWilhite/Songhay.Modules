namespace Songhay.Modules.Tests

module JsonDocumentUtilityTests =

    open System.Text.Json

    open Xunit
    open FsToolkit.ErrorHandling
    open FsUnit.Xunit

    open Songhay.Modules.Models
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
            JDocument jDoc
            |> tryGetProperty "one"
            |> Result.map toJsonElement
        let elementOne = result |> Result.valueOr raise

        let expectedResult = "this is first"
        let actual = elementOne.GetString()
        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document root element traversal test``() =
        let result =
            JElement jDoc.RootElement
            |> tryGetProperty "top"
            |> Result.bind (tryGetProperty "one")
            |> Result.map toJsonElement
        let elementOne = result |> Result.valueOr raise

        let expectedResult = "this is first"
        let actual = elementOne.GetString()
        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document traversal test``() =
        let result =
            JDocument jDoc
            |> tryGetProperty "three"
            |> Result.bind (tryGetProperty "p1")
            |> Result.map toJsonElement
        let elementP1 = result |> Result.valueOr raise

        let expectedResult = "this is three-point-one"
        let actual = elementP1.GetString()
        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document array test``() =
        let jDocArray = JsonDocument.Parse("[{},{}]")
        let result =
            JDocument jDocArray
            |> tryGetProperty "foo"
            |> Result.map toJsonElement
        result |> Result.isError |> should be True

    [<Fact>]
    let ``tryGetProperty document array item test``() =
        let jDocArray = JsonDocument.Parse(@"[{""zero"": null},{""zero"": ""naught""}]")
        let result =
            JElement (jDocArray.RootElement.Item(1))
            |> tryGetProperty "zero"
            |> Result.map toJsonElement
        let elementZero = result |> Result.valueOr raise

        let expectedResult = "naught"
        let actual = elementZero.GetString()
        actual |> should equal expectedResult
