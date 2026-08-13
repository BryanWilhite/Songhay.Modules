namespace Songhay.Modules.Tests.Models

open System

open Xunit
open FsToolkit.ErrorHandling

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
            result.IsError |> Assert.True
        else
            result.IsOk |> Assert.True
            let actual = result |> Result.valueOr raise
            Assert.Equal(expectedOutput.Value, actual)

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
            result.IsError |> Assert.True
        else
            result.IsOk |> Assert.True
            let actual = result |> Result.valueOr raise
            Assert.Equal(expectedOutput.Value, actual)

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
            result.IsError |> Assert.True
        else
            result.IsOk |> Assert.True
            let actual = result |> Result.valueOr raise
            Assert.Equal((EndDate (DateTime.Parse(expectedOutput.Value))), actual)

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
            result.IsError |> Assert.True
        else
            result.IsOk |> Assert.True
            let actual = result |> Result.valueOr raise
            Assert.Equal((InceptDate (DateTime.Parse(expectedOutput.Value))), actual)

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
            result.IsError |> Assert.True
        else
            result.IsOk |> Assert.True
            let actual = result |> Result.valueOr raise
            Assert.Equal((ModificationDate (DateTime.Parse(expectedOutput.Value))), actual)

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
            result.IsError |> Assert.True
        else
            result.IsOk |> Assert.True
            let actual = result |> Result.valueOr raise
            Assert.Equal((ModificationDate (DateTime.Parse(expectedOutput.Value))), actual)
