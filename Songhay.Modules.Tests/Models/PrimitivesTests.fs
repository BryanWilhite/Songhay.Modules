namespace Songhay.Modules.Tests.Models

module PrimitivesTests =

    open System
    open System.Text.Json

    open Xunit
    open FsToolkit.ErrorHandling
    open FsUnit.Xunit
    open FsUnit.CustomMatchers

    open Songhay.Modules.Models
    open Songhay.Modules.JsonDocumentUtility

    let IdentifierTestData : seq<obj[]> =
        seq {
            yield [| @"{""root"": {""id"": 4303}}"; "id"; false; Numeric 4303 |]
            yield [| @"{""root"": {""id"": ""A4303""}}"; "id"; false; Alphanumeric "A4303" |]
            yield [| @"{""root"": null}"; "id"; true; Alphanumeric "🎈" |]
            yield [| @"{}"; "id"; true; Alphanumeric "🎈" |]
        }

    [<Theory>]
    [<MemberData(nameof IdentifierTestData)>]
    let ``Identifier.fromInputElementName doc test`` (input: string) (elementName: string) (isErrorExpected: bool) (expectedOutput: Identifier) =
        let documentOrElement = JDocument (JsonDocument.Parse(input))
        let result = documentOrElement |> Identifier.fromInputElementName elementName
        if isErrorExpected then
            result |> should be (ofCase <@ Result<Identifier, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<Identifier, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal expectedOutput

    let ClientIdTestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""ClientId"": ""IDAMAQDBIDANAQDB-1""}}"; false; false; ClientId (Alphanumeric "IDAMAQDBIDANAQDB-1") |]
            yield [| @"{""presentation"": {""clientId"": ""IDAMAQDBIDANAQDB-1""}}"; true; false; ClientId (Alphanumeric "IDAMAQDBIDANAQDB-1") |]
            yield [| @"{""Presentation"": {""ClientId"": null}}"; false; false; ClientId (Alphanumeric null) |]
            yield [| @"{""Presentation"": null}"; false; true; ClientId (Alphanumeric "🎈") |]
            yield [| @"{}"; false; true; ClientId (Alphanumeric "🎈") |]
        }

    [<Theory>]
    [<MemberData(nameof ClientIdTestData)>]
    let ``ClientId.fromInput doc test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: ClientId) =
        let documentOrElement = JDocument (JsonDocument.Parse(input))
        let result = documentOrElement |> ClientId.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<ClientId, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<ClientId, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal expectedOutput

    let EndDateTestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""EndDate"": ""2005-12-10T22:19:14""}}"; false; false; "2005-12-10T22:19:14" |]
            yield [| @"{""Presentation"": {""endDate"": ""2005-12-10T22:19:14""}}"; true; false; "2005-12-10T22:19:14" |]
            yield [| @"{""Presentation"": {""EndDate"": null}}"; false; true; null |]
            yield [| @"{""Presentation"": null}"; false; true; null |]
            yield [| @"{}"; false; true; null |]
        }

    [<Theory>]
    [<MemberData(nameof EndDateTestData)>]
    let ``EndDate.fomInput doc test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string) =
        let documentOrElement = JDocument (JsonDocument.Parse(input))
        let result = documentOrElement |> EndDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<EndDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<EndDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (EndDate (DateTime.Parse(expectedOutput)))

    let InceptDateTestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""InceptDate"": ""2005-12-10T22:19:14""}}"; false; false; "2005-12-10T22:19:14" |]
            yield [| @"{""Presentation"": {""inceptDate"": ""2005-12-10T22:19:14""}}"; true; false; "2005-12-10T22:19:14" |]
            yield [| @"{""Presentation"": {""InceptDate"": null}}"; false; true; null |]
            yield [| @"{""Presentation"": null}"; false; true; null |]
            yield [| @"{}"; false; true; null |]
        }

    [<Theory>]
    [<MemberData(nameof InceptDateTestData)>]
    let ``InceptDate.fomInput doc test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string) =
        let documentOrElement = JDocument (JsonDocument.Parse(input))
        let result = documentOrElement |> InceptDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<InceptDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<InceptDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (InceptDate (DateTime.Parse(expectedOutput)))

    let ModificationDateTestDataForDoc : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""ModificationDate"": ""2005-12-10T22:19:14""}}"; false; false; "2005-12-10T22:19:14" |]
            yield [| @"{""Presentation"": {""modificationDate"": ""2005-12-10T22:19:14""}}"; true; false; "2005-12-10T22:19:14" |]
            yield [| @"{""Presentation"": {""ModificationDate"": null}}"; false; true; null |]
            yield [| @"{""Presentation"": null}"; false; true; null |]
            yield [| @"{}"; false; true; null |]
        }

    [<Theory>]
    [<MemberData(nameof ModificationDateTestDataForDoc)>]
    let ``ModificationDate.fromInput doc test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string) =
        let documentOrElement = JDocument (JsonDocument.Parse(input))
        let result = documentOrElement |> ModificationDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (ModificationDate (DateTime.Parse(expectedOutput)))

    let ModificationDateTestData : seq<obj[]> =
        seq {
            yield [| @"{""root"": {""Presentation"": {""ModificationDate"": ""2005-12-10T22:19:14""}}}"; false; false; "2005-12-10T22:19:14" |]
            yield [| @"{""root"": {""Presentation"": {""modificationDate"": ""2005-12-10T22:19:14""}}}"; true; false; "2005-12-10T22:19:14" |]
            yield [| @"{""root"": {""Presentation"": {""ModificationDate"": null}}}"; false; true; null |]
            yield [| @"{""root"": {""Presentation"": null}}"; false; true; null |]
        }

    [<Theory>]
    [<MemberData(nameof ModificationDateTestData)>]
    let ``ModificationDate.fromInput test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string) =
        let element =
            (JDocument (JsonDocument.Parse(input)))
            |> tryGetProperty "Presentation"
            |> Result.valueOr raise
            |> toJsonElement
        let result = element |> JElement |> ModificationDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (ModificationDate (DateTime.Parse(expectedOutput)))
