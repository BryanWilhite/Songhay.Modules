namespace Songhay.Modules

module JsonDocumentUtility =

    open System.Linq
    open System.Text.Json

    let internal resultError (elementName: string) =
        Error(JsonException $"the expected `{elementName}` element is not here.")

    /// <summary>
    /// Converts the <see cref="JsonElement" />
    /// to its property name or returns <see cref="None" />.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    let toPropertyName (element: JsonElement) =
        if element.ValueKind <> JsonValueKind.Object then None
        else
            try
                Some (element.EnumerateObject().First().Name)
            with | _ -> None

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonElement" /> object.
    /// </summary>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    let tryGetProperty (elementName: string) (element: JsonElement) =
        match element.TryGetProperty elementName with
        | false, _ -> resultError elementName
        | true, el -> Ok el

    /// <summary>
    /// Tries to return the <see cref="JsonDocument.RootElement" />
    /// after parsing the specified JSON document.
    /// </summary>
    /// <param name="rawDocument">The JSON document.</param>
    let tryGetRootElement (rawDocument: string) =
        try
            let document = rawDocument |> JsonDocument.Parse
            Ok document.RootElement
        with | exn -> Error exn
