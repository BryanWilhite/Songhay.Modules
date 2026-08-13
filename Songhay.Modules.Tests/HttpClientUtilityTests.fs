namespace Songhay.Modules.Tests

module HttpClientUtilityTests =

    open System
    open System.IO
    open System.Net.Http
    open System.Reflection

    open FSharp.Data
    open FsToolkit.ErrorHandling

    open Xunit

    open Songhay.Modules.HttpClientUtility
    open Songhay.Modules.HttpRequestMessageUtility
    open Songhay.Modules.HttpResponseMessageUtility
    open Songhay.Modules.Models
    open Songhay.Modules.ProgramFileUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let client = new HttpClient()

    [<Literal>]
    let LIVE_API_BASE_URI = "http://jsonplaceholder.typicode.com"

    let isJsonResult isExpectedJson response = task {

        let! jsonResult = response |> tryDownloadToStringAsync

        match jsonResult with
        |Error _ -> return false
        | Ok json ->
            String.IsNullOrWhiteSpace(json) |> Assert.False

            return isExpectedJson json
    }

    type providerGet = JsonProvider<"""[{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }]""">

    [<Theory>]
    [<InlineData("/posts")>]
    let ``client should get`` (location: string) =

        let isExpectedJson json =
            let docs = json |> providerGet.Parse
            docs.Length > 0 |> Assert.True
            docs |> Array.forall (fun doc -> doc.Id > 0 && doc.UserId > 0) |> Assert.True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (get uri)
                |> Async.AwaitTask

            responseResult.IsOk |> Assert.True

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            Assert.Equal(
                response.RequestMessage.Method.ToString().ToUpperInvariant(),
                HttpMethod.Get.ToUpperInvariant()
            )

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> Assert.True
        }

    [<Theory>]
    [<InlineData("/posts/1")>]
    let ``client should delete`` (location: string) =

        let isExpectedJson json =
            Assert.Equal("{}", json)
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (delete uri)
                |> Async.AwaitTask

            responseResult.IsOk |> Assert.True

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            Assert.Equal(
                 response.RequestMessage.Method.ToString().ToUpperInvariant(),
                 HttpMethod.Delete.ToUpperInvariant()
            )

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> Assert.True
        }

    type providerPost = JsonProvider<"""{ "id": 101, "title": "foo", "body": "bar", "userId": 1 }""">

    [<Theory>]
    [<InlineData("/posts", """{ "title": "foo", "body": "bar", "userId": 1 }""")>]
    let ``client should post`` (location: string, data: string) =
        let isExpectedJson json =
            let doc = json |> providerPost.Parse
            (doc.Id > 0 && doc.Title = "foo" && doc.Body = "bar" && doc.UserId = 1) |> Assert.True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (post uri |> withJsonStringContent data)
                |> Async.AwaitTask

            responseResult.IsOk |> Assert.True

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            Assert.Equal(
                int response.StatusCode,
                HttpStatusCodes.Created
            )

            Assert.Equal(
                response.RequestMessage.Method.ToString().ToUpperInvariant(),
                HttpMethod.Post.ToUpperInvariant()
            )

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> Assert.True
        }

    type providerPut = JsonProvider<"""{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }""">

    [<Theory>]
    [<InlineData("/posts/1", """{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }""")>]
    let ``client should put`` (location: string, data: string) =
        let isExpectedJson json =
            let doc = json |> providerPost.Parse
            (doc.Id = 1 && doc.Title = "foo" && doc.Body = "bar" && doc.UserId = 1) |> Assert.True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (put uri |> withJsonStringContent data)
                |> Async.AwaitTask

            responseResult.IsOk |> Assert.True

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            Assert.Equal(
                response.RequestMessage.Method.ToString().ToUpperInvariant(),
                HttpMethod.Put.ToUpperInvariant()
            )

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> Assert.True
        }
    type providerPatch = JsonProvider<"""{ "body": "bar" }""">

    [<Theory>]
    [<InlineData("/posts/1", """{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }""")>]
    let ``client should patch`` (location: string, data: string) =
        let isExpectedJson json =
            let doc = json |> providerPost.Parse
            (doc.Id = 1 && doc.Body = "bar") |> Assert.True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (patch uri |> withJsonStringContent data)
                |> Async.AwaitTask

            responseResult.IsOk |> Assert.True

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            Assert.Equal(
                response.RequestMessage.Method.ToString().ToUpperInvariant(),
                HttpMethod.Patch.ToUpperInvariant()
            )

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> Assert.True
        }