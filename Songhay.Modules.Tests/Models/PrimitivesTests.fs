namespace Songhay.Modules.Tests.Models

open System
open System.Text.Json

open Xunit
open FsToolkit.ErrorHandling
open FsUnit.Xunit
open FsUnit.CustomMatchers

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility

module PrimitivesTests =

    let IdentifierTestData : seq<obj[]> =
        seq {
            yield [| @"{""root"": {""id"": 4303}}"; "id"; false; Numeric 4303 |> Some |]
            yield [| @"{""root"": {""id"": ""A4303""}}"; "id"; false; Alphanumeric "A4303" |> Some |]
            yield [| @"{""root"": null}"; "id"; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof IdentifierTestData)>]
    let ``Identifier.fromInputElementName root test`` (input: string) (elementName: string) (isErrorExpected: bool) (expectedOutput: Identifier option) =
        let element =
            input
            |> tryGetRootElement
            |> Result.bind (tryGetProperty "root")
            |> Result.valueOr raise
        let result = element |> Identifier.fromInputElementName elementName
        if isErrorExpected then
            result |> should be (ofCase <@ Result<Identifier, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<Identifier, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal expectedOutput.Value

    let ClientIdTestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""ClientId"": ""IDAMAQDBIDANAQDB-1""}}"; false; false; ClientId (Alphanumeric "IDAMAQDBIDANAQDB-1") |> Some |]
            yield [| @"{""Presentation"": {""clientId"": null}}"; true; true; None |]
            yield [| @"{""Presentation"": null}"; false; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof ClientIdTestData)>]
    let ``ClientId.fromInput root test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: ClientId option) =
        let element =
            input
            |> tryGetRootElement
            |> Result.bind (tryGetProperty "Presentation")
            |> Result.valueOr raise
        let result = element |> ClientId.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<ClientId, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<ClientId, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal expectedOutput.Value

    let EndDateTestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""EndDate"": ""2005-12-10T22:19:14""}}"; false; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""Presentation"": {""endDate"": ""2005-12-10T22:19:14""}}"; true; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""Presentation"": {""EndDate"": null}}"; false; true; None |]
            yield [| @"{""Presentation"": null}"; false; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof EndDateTestData)>]
    let ``EndDate.fomInput root test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string option) =
        let element =
            input
            |> tryGetRootElement
            |> Result.bind (tryGetProperty "Presentation")
            |> Result.valueOr raise
        let result = element |> EndDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<EndDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<EndDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (EndDate (DateTime.Parse(expectedOutput.Value)))

    let InceptDateTestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""InceptDate"": ""2005-12-10T22:19:14""}}"; false; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""Presentation"": {""inceptDate"": ""2005-12-10T22:19:14""}}"; true; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""Presentation"": {""InceptDate"": null}}"; false; true; None |]
            yield [| @"{""Presentation"": null}"; false; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof InceptDateTestData)>]
    let ``InceptDate.fomInput root test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string option) =
        let element =
            input
            |> tryGetRootElement
            |> Result.bind (tryGetProperty "Presentation")
            |> Result.valueOr raise
        let result = element |> InceptDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<InceptDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<InceptDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (InceptDate (DateTime.Parse(expectedOutput.Value)))

    let ModificationDateTestDataForDoc : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""ModificationDate"": ""2005-12-10T22:19:14""}}"; false; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""Presentation"": {""modificationDate"": ""2005-12-10T22:19:14""}}"; true; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""Presentation"": {""ModificationDate"": null}}"; false; true; None |]
            yield [| @"{""Presentation"": null}"; false; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof ModificationDateTestDataForDoc)>]
    let ``ModificationDate.fromInput root test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string option) =
        let element =
            input
            |> tryGetRootElement
            |> Result.bind (tryGetProperty "Presentation")
            |> Result.valueOr raise
        let result = element |> ModificationDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (ModificationDate (DateTime.Parse(expectedOutput.Value)))

    let ModificationDateTestData : seq<obj[]> =
        seq {
            yield [| @"{""root"": {""Presentation"": {""ModificationDate"": ""2005-12-10T22:19:14""}}}"; false; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""root"": {""Presentation"": {""modificationDate"": ""2005-12-10T22:19:14""}}}"; true; false; "2005-12-10T22:19:14" |> Some |]
            yield [| @"{""root"": {""Presentation"": {""ModificationDate"": null}}}"; false; true; None |]
            yield [| @"{""root"": {""Presentation"": null}}"; false; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof ModificationDateTestData)>]
    let ``ModificationDate.fromInput test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string option) =
        let element =
            input
            |> tryGetRootElement
            |> Result.bind (tryGetProperty "root")
            |> Result.bind(tryGetProperty "Presentation")
            |> Result.valueOr raise
        let result = element |> ModificationDate.fromInput useCamelCase
        if isErrorExpected then
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Error @>)
        else
            result |> should be (ofCase <@ Result<ModificationDate, JsonException>.Ok @>)
            let actual = result |> Result.valueOr raise
            actual |> should equal (ModificationDate (DateTime.Parse(expectedOutput.Value)))
