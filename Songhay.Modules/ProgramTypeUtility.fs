namespace Songhay.Modules

module ProgramTypeUtility =

    open System
    open System.Globalization
    open System.Xml

    /// <summary>
    /// Tries to parse the specified value
    /// into <see cref="TimeSpan" />
    /// based on the assertion that the value
    /// is in ISO8601 format.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    /// For example, "P0DT1H0M0S".
    ///
    /// “Durations define the amount of intervening time
    /// in a time interval and are represented
    /// by the format P[n]Y[n]M[n]DT[n]H[n]M[n]S or P[n]W…”
    ///
    /// [ https://en.wikipedia.org/wiki/ISO_8601 ]
    /// </remarks>
    let tryParseIso8601Duration (value: string): Result<TimeSpan, exn> =
        if String.IsNullOrWhiteSpace(value) then Error (NullReferenceException "The expected date-time value is not here.")
        else
            try
                Ok (value |> XmlConvert.ToTimeSpan)
            with
            | :? FormatException as ex -> Error ex

    /// <summary>
    /// Tries to parse the specified value
    /// into <see cref="TimeSpan" />
    /// based on the assertion that the value
    /// is in RFC822 format.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    /// For example, "Fri, 22 Mar 2019 18:56:07 -0700".
    /// see https://datatracker.ietf.org/doc/html/rfc2822
    /// </remarks>
    let tryParseRfc822DateTime (value: string): Result<DateTime, exn> =
        if String.IsNullOrWhiteSpace(value) then Error (NullReferenceException "The expected date-time value is not here.")
        else
            let dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat
            let formats = [|
                dateTimeFormat.RFC1123Pattern
                "ddd',' d MMM yyyy HH:mm:ss zzz"
                "ddd',' dd MMM yyyy HH:mm:ss zzz"
            |]
            match DateTime.TryParseExact (value, formats, dateTimeFormat, DateTimeStyles.AssumeUniversal) with
            | false, _ -> Error (FormatException "The expected date-time format is not here.")
            | true, dateTimeValue -> Ok dateTimeValue
