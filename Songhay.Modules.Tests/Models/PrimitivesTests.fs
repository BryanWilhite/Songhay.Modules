namespace Songhay.Modules.Tests.Models

module PrimitivesTests =

    open System.Text.Json

    open Xunit
    open FsToolkit.ErrorHandling
    open FsUnit.Xunit

    open Songhay.Modules.Models

    let TestData : seq<obj[]> =
        seq {
            yield [| @"{""Presentation"": {""ClientId"": ""IDAMAQDBIDANAQDB-1""}}"; false; false; Some "IDAMAQDBIDANAQDB-1" |]
            yield [| @"{""presentation"": {""clientId"": ""IDAMAQDBIDANAQDB-1""}}"; true; false; Some "IDAMAQDBIDANAQDB-1" |]
            yield [| @"{""Presentation"": {""ClientId"": null}}"; false; false; None |]
            yield [| @"{""Presentation"": null}"; false; true; None |]
            yield [| @"{}"; false; true; None |]
        }

    [<Theory>]
    [<MemberData(nameof TestData)>]
    let ``ClientId fromInput test``(input: string) (useCamelCase: bool) (isErrorExpected: bool) (expectedOutput: string option) =
        let documentOrElement = JDocument (JsonDocument.Parse(input))
        let result = documentOrElement |> ClientId.fromInput useCamelCase
        if isErrorExpected then
            result |> Result.isError |> should be True
        else
            let actual = result |> Result.valueOr raise
            match expectedOutput with
            | Some expected -> actual.toIdentifier.StringValue |> should equal expected
            | None -> actual.toIdentifier.StringValue |> Option.ofObj |> should equal None
