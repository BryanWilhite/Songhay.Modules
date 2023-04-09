module Songhay.Modules.Tests.CssDeclarationUtilityTests

open Xunit

open Songhay.Modules.Models
open Songhay.Modules.CssDeclarationUtility

let fontVariantTestData : seq<obj[]> =
    seq {
        yield [| "font-variant: small-caps slashed-zero;"; [ SmallCaps; NumericFontVariant FontVariantNumericSlashedZero ] |]
        yield [| "font-variant: common-ligatures tabular-nums;"; [ Ligatures FontVariantLigaturesCommon; NumericFontVariant FontVariantTabular ] |]
        yield [| "font-variant: no-common-ligatures proportional-nums;"; [ Ligatures FontVariantLigaturesNoCommon; NumericFontVariant FontVariantProportional ] |]
    }

[<Theory>]
[<MemberData(nameof fontVariantTestData)>]
let ``fontVariant test`` (expected: string, input: CssFontVariant list) =
    let actual = fontVariant input
    Assert.Equal(expected, actual)
