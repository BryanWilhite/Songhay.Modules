module Songhay.Modules.Tests.Models.CssPrimitivesTests

open Xunit

open Songhay.Modules.Models

let CssInheritanceTestData : seq<obj[]> =
    seq {
        yield [| "inherit"; Inherit |]
        yield [| "revert-layer"; RevertLayer |]
    }

[<Theory>]
[<MemberData(nameof CssInheritanceTestData)>]
let ``CssInheritance test`` (expected: string, inheritance: CssInheritance) =
    let actual = inheritance.Value
    Assert.Equal<string>(expected, actual)
