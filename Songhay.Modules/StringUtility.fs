namespace Songhay.Modules

/// <summary>
/// Utility functions for <see cref="System.String" />.
/// </summary>
module StringUtility =

    open System
    open System.Text.RegularExpressions

    let internal defaultRegexOptions = RegexOptions.IgnoreCase

    let internal regexReplace (pattern: string) (replacement: string) (options: RegexOptions) (input: string) =
        Regex.Replace(input, pattern, replacement, options)

    /// <summary>
    /// Tries to call <see cref="Regex.Replace"/>
    /// for a successful find-change operation.
    /// </summary>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="replacement">The replacement string.</param>
    /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
    let tryRegexReplace (pattern: string) (replacement: string) (options: RegexOptions) (input: string) =
        try
            Ok (input |> regexReplace pattern replacement options)
        with
        | ex -> Error ex

    /// <summary>
    /// Calls <see cref="String.ToLowerInvariant"/>
    /// for valid input or returns <see cref="None" />.
    /// </summary>
    let toLowerInvariant (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else Some (input.ToLowerInvariant())

    /// <summary>
    /// Converts the <see cref="String"/> into a blog slug
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="input">The input.</param>
    let toBlogSlug (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else
            let removeEntities s =
                s
                |> regexReplace @"\&\w+\;" String.Empty defaultRegexOptions
                |> regexReplace @"\&\#\d+\;" String.Empty defaultRegexOptions

            let replaceNonAlphanumericWithHyphen s =
                s
                |> regexReplace "[^a-z^0-9]" "-" defaultRegexOptions

            let rec removeDoubleHyphens s =
                let pattern = "--"
                if Regex.IsMatch(s, pattern) then
                    s
                    |> regexReplace pattern "-" defaultRegexOptions
                    |> removeDoubleHyphens
                else
                    s

            let removeTrailingAndLeadingHyphens s =
                s
                |> regexReplace "^-|-$" String.Empty defaultRegexOptions

            input
                .Replace("&amp;", "and")
                |> removeEntities
                |> replaceNonAlphanumericWithHyphen
                |> removeDoubleHyphens
                |> removeTrailingAndLeadingHyphens
                |> toLowerInvariant

    /// <summary>
    /// Demarcates the <see cref="string"/>
    /// by the specified demarcation <see cref="char"/>
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="demarcation">The demarcation <see cref="char"/>.</param>
    /// <param name="input">The input.</param>
    /// <remarks>
    /// See <see cref="toKabobCase" /> and <see cref="toSnakeCase" />
    /// for specific applications of this generalized function.
    /// </remarks>
    let demarcateByCase (demarcation: char) (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else
            let processChar i c =
                if (i > 0) && Char.IsUpper(c) then $"{demarcation}{Char.ToLower(c)}"
                else $"{Char.ToLower(c)}"
            let stringArray = input.ToCharArray() |> Array.mapi processChar
            Some (String.Join(String.Empty, stringArray))

    /// <summary>
    /// Converts the <see cref="string"/> into a numeric format for parsing
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="input">The input.</param>
    let toNumericString (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else
            let chars =
                input.Trim().ToCharArray()
                |> Array.filter (fun c -> Char.IsDigit(c) || c.Equals('.') || c.Equals('-'))
            Some (chars |> String)

    /// <summary>
    /// Demarcates the <see cref="string"/>
    /// by the demarcation <see cref="char"/>, <c>'-'</c>,
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="input">The input.</param>
    let toKabobCase (input: string) = input |> demarcateByCase '-'

    /// <summary>
    /// Demarcates the <see cref="string"/>
    /// by the demarcation <see cref="char"/>, <c>'_'</c>,
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="input">The input.</param>
    let toSnakeCase (input: string) = input |> demarcateByCase '_'
