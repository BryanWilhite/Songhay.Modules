namespace Songhay.Modules.Tests.Models

open System
open System.Collections.Generic
open Xunit
open Xunit.Abstractions

open FsUnit.Xunit
open FsUnit.CustomMatchers
open FsToolkit.ErrorHandling

open Songhay.Modules.Models

type RestApiPrimitivesTests(testOutputHelper: ITestOutputHelper) =

    static member UriFromClaimTestData : seq<obj[]> =
        seq {
            yield
                [|
                    RestApiMetadata (
                        ApiBase "http://localhost:3001",
                        ClaimsSet <| Dictionary<_,_>(dict [
                            ("cdn-route-for-manifest", "{presentationKey}/{presentationKey}_presentation.json")
                        ])
                    )
                    "cdn-route-for-manifest"
                    [| "foo" |]
                    "http://localhost:3001/foo/foo_presentation.json"
                |]
            yield
                [|
                    RestApiMetadata (
                        ApiBase "http://localhost:3001",
                        ClaimsSet <| Dictionary<_,_>(dict [
                            ("endpoint-prefix", "api/player/v1")
                            ("route-for-video-yt-playlist", "video/youtube/playlist/{subFolder}/{blobName}|placeholder")
                        ])
                    )
                    "route-for-video-yt-playlist"
                    [| "foo"; "bar" |]
                    "http://localhost:3001/api/player/v1/video/youtube/playlist/foo/bar?code=placeholder"
                |]
            yield
                [|
                    RestApiMetadata (
                        ApiBase "http://localhost:3001/",
                        ClaimsSet <| Dictionary<_,_>(dict [
                            ("endpoint-prefix", "/api/player/v1/")
                            ("route-for-video-yt-playlist", "/video/youtube/playlist/{subFolder}/{blobName}/|placeholder")
                        ])
                    )
                    "route-for-video-yt-playlist"
                    [| "foo"; "bar" |]
                    "http://localhost:3001/api/player/v1/video/youtube/playlist/foo/bar?code=placeholder"
                |]
            yield
                [|
                    RestApiMetadata (
                        ApiBase "http://localhost:3001/",
                        ClaimsSet <| Dictionary<_,_>(dict [
                            ("endpoint-prefix", "/api/player/v1/")
                            ("route-for-video-yt-playlist", "/video/youtube/playlist/{subFolder}/{blobName}/|placeholder")
                        ])
                    )
                    "route-for-video-yt-playlist"
                    [| "foo" |]
                    None
                |]
            yield
                [|
                    RestApiMetadata (
                        ApiBase "http://localhost:3001/",
                        ClaimsSet <| Dictionary<_,_>(dict [
                            ("endpoint-prefix", "/api/player/v1/")
                        ])
                    )
                    "route-for-video-yt-playlist"
                    [| "foo" |]
                    None
                |]
        }

    [<Theory>]
    [<MemberData(nameof RestApiPrimitivesTests.UriFromClaimTestData)>]
    member this.``RestApiMetadata.ToUriFromClaim test``(input: RestApiMetadata, key: string, args: string[], expectedOriginalString: string option) =

        let actual = input.ToUriResultFromClaim(key, args)

        if expectedOriginalString.IsNone then
            actual |> should be (ofCase <@ Result<Uri,exn>.Error @>)
            testOutputHelper.WriteLine $"{nameof Error} expected"
        else
            actual |> should be (ofCase <@ Result<Uri,exn>.Ok @>)
            testOutputHelper.WriteLine $"{nameof actual}: {actual |> Result.valueOr raise}"

            Assert.Equal(expectedOriginalString.Value, (actual |> Result.valueOr raise).OriginalString)
