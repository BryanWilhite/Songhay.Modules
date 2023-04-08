namespace Songhay.Modules.Tests

module JsonDocumentUtilityTests =

    open System
    open System.Text.Json

    open Xunit
    open FsToolkit.ErrorHandling
    open FsToolkit.ErrorHandling.Operator.Result
    open FsUnit.CustomMatchers
    open FsUnit.Xunit

    open Songhay.Modules
    open Songhay.Modules.JsonDocumentUtility

    [<Theory>]
    [<InlineData(@"{""actual"": ""2005-12-10T22:19:14""}", false)>]
    [<InlineData(@"{""actual"": null}", true)>]
    let ``toResultFromStringElement DateTime test`` (input: string) (isErrorExpected: bool) =
        let result =
            input
            |> tryGetRootElement
            >>= (tryGetProperty "actual")
            |> toResultFromStringElement (fun el -> el.GetDateTime())

        if isErrorExpected then
            result |> should be (ofCase <@ Result<DateTime, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<DateTime, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData(@"{""actual"": true}", true)>]
    [<InlineData(@"{""actual"": ""true""}", false)>]
    [<InlineData(@"{""actual"": null}", true)>]
    let ``toResultFromStringElement test`` (input: string) (isErrorExpected: bool) =
        let result =
            input
            |> tryGetRootElement
            >>= (tryGetProperty "actual")
            |> toResultFromStringElement (fun el -> el.GetString())

        if isErrorExpected then
            result |> should be (ofCase <@ Result<string, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<string, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData(@"{""actual"": true}", false)>]
    [<InlineData(@"{""actual"": ""true""}", true)>]
    [<InlineData(@"{""actual"": null}", true)>]
    let ``toResultFromBooleanElement test`` (input: string) (isErrorExpected: bool) =
        let result =
            input
            |> tryGetRootElement
            >>= (tryGetProperty "actual")
            |> toResultFromBooleanElement (fun el -> el.GetBoolean())

        if isErrorExpected then
            result |> should be (ofCase <@ Result<bool, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<bool, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData(@"{""actual"": 1.56}", false)>]
    [<InlineData(@"{""actual"": ""1.56""}", true)>]
    [<InlineData(@"{""actual"": null}", true)>]
    let ``toResultFromNumericElement GetDouble test`` (input: string) (isErrorExpected: bool) =
        let result =
            input
            |> tryGetRootElement
            >>= (tryGetProperty "actual")
            |> toResultFromNumericElement (fun el -> el.GetDouble())

        if isErrorExpected then
            result |> should be (ofCase <@ Result<float, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<float, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData(@"{""actual"": 156}", false)>]
    [<InlineData(@"{""actual"": ""156""}", true)>]
    [<InlineData(@"{""actual"": null}", true)>]
    let ``toResultFromNumericElement GetInt32 test`` (input: string) (isErrorExpected: bool) =
        let result =
            input
            |> tryGetRootElement
            >>= (tryGetProperty "actual")
            |> toResultFromNumericElement (fun el -> el.GetInt32())

        if isErrorExpected then
            result |> should be (ofCase <@ Result<int, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<int, JsonException>.Ok @>)

    let jDocResult = Result.parseJsonDocument(@"
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
    let ``tryGetProperty document root element traversal test``() =
        let result = result {
            let! jDoc = jDocResult
            return!
                jDoc.RootElement
                |> tryGetProperty "top"
                >>= (tryGetProperty "one")
        }

        let elementOne = result |> Result.valueOr raise
        let expectedResult = "this is first"
        let actual = elementOne.GetString()

        actual |> should equal expectedResult

    [<Fact>]
    let ``tryGetProperty document traversal test``() =
        let result = result {
            let! jDoc = jDocResult
            return!
                jDoc.RootElement
                |> tryGetProperty "top"
                >>= (tryGetProperty "three")
                >>= (tryGetProperty "p1")
        }

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
