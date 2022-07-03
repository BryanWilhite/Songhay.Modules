namespace Songhay.Modules

module ProgramFileUtility =

    open System
    open System.IO
    open System.Text.RegularExpressions

    type ProgramFileError = ProgramFileError of exn

    let internal backSlash = '\\'

    let internal forwardSlash = '/'

    /// <summary>
    /// Returns <c>true</c> when the current OS
    /// uses forward-slash (<c>/</c>) paths or <c>false</c>.
    /// </summary>
    let isForwardSlashSystem = Path.DirectorySeparatorChar.Equals(forwardSlash)

    /// <summary>
    /// Counts the parent directory chars.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    /// <remarks>
    /// WARNING: call <see cref="NormalizePath(string)"/> to prevent incorrectly returning <c>0</c>
    /// in cross-platform scenarios.
    /// </remarks>
    let countParentDirectoryChars path =
        if String.IsNullOrWhiteSpace path then Unchecked.defaultof<int>
        else
            let parentDirectoryCharsPattern = @$"\.\.\{Path.DirectorySeparatorChar}"
            let matches = Regex.Matches(path, parentDirectoryCharsPattern)
            matches.Count

    /// <summary>
    /// Tries to find the parent <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="parentName">Name of the parent.</param>
    /// <param name="levels">The levels.</param>
    /// <param name="path">The path.</param>
    let rec tryFindParentDirectoryInfo parentName levels path =
        if (String.IsNullOrWhiteSpace path) then
            Error (ProgramFileError (DirectoryNotFoundException "The expected directory is not here."))
        else
            let info = DirectoryInfo(path)

            let doesNotExist = (not info.Exists)
            let isParent = (info.Name = parentName)
            let hasNullParent = (info.Parent = null)
            let hasTargetParent = not hasNullParent && (info.Parent.Name = parentName)

            match info with
            | _ when doesNotExist || hasNullParent ->
                Error (ProgramFileError (DirectoryNotFoundException "Directory does not exist."))
            | _ when isParent -> Ok info
            | _ when hasTargetParent -> Ok info.Parent
            | _ ->
                let nextLevels = (abs levels) - 1
                let hasNoMoreLevels = (nextLevels = 0)

                if hasNoMoreLevels then Error (ProgramFileError (DirectoryNotFoundException "Has no more levels"))
                else (parentName, nextLevels, info.Parent.FullName) |||> tryFindParentDirectoryInfo

    /// <summary>
    /// Tries to get the parent <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="levels">The levels.</param>
    /// <param name="path">The path.</param>
    let rec tryGetParentDirectoryInfo levels path =
        if String.IsNullOrWhiteSpace(path) then
            Error (ProgramFileError (DirectoryNotFoundException "The expected path is not here."))
        else
            let info = DirectoryInfo(path)
            match levels with
            | _ when (abs levels) = 0 -> Ok info
            | _ ->
                match info with
                | _ when info = null -> Ok info
                | _ ->
                    let nextLevels = levels - 1
                    match nextLevels with
                    | _ when nextLevels >= 1 -> tryGetParentDirectoryInfo nextLevels info.Parent.FullName
                    | _ -> Ok info.Parent

    /// <summary>
    /// Tries to get the parent directory.
    /// </summary>
    /// <param name="levels">The levels.</param>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// A recursive wrapper for <see cref="Directory.GetParent(string)" />.
    /// </remarks>
    let rec tryGetParentDirectory levels path =
        if String.IsNullOrWhiteSpace(path) then Error (ProgramFileError (NullReferenceException "The expected path is not here."))
        else
            match levels with
            | _ when (abs levels) = 0 -> Ok path
            | _ ->
                let info = Directory.GetParent(path)
                match info with
                | _ when info = null -> Ok path
                | _ ->
                    let nextLevels = levels - 1
                    match nextLevels with
                    | _ when nextLevels >= 1 -> tryGetParentDirectory nextLevels info.FullName
                    | _ -> Ok info.FullName

    /// <summary>
    /// Normalizes the specified path with respect
    /// to the ambient value of <see cref="Path.DirectorySeparatorChar"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    let normalizePath path =
        if String.IsNullOrWhiteSpace(path) then path
        elif isForwardSlashSystem  then path.Replace(backSlash, forwardSlash)
        else path.Replace(forwardSlash, backSlash)

    /// <summary>
    /// Raise <see cref="DirectoryNotFoundException"/>
    /// for the specified path or pass through the path.
    /// </summary>
    /// <param name="path">The path.</param>
    let raiseExceptionForExpectedDirectory path =
        if String.IsNullOrWhiteSpace(path) then raise (DirectoryNotFoundException $"The expected directory is not here.")
        elif File.Exists(path) then path
        else raise (DirectoryNotFoundException $"The expected directory, `{path}`, is not here.")

    /// <summary>
    /// Raise <see cref="FileNotFoundException"/>
    /// for the specified path or pass through the path.
    /// </summary>
    /// <param name="path">The path.</param>
    let raiseExceptionForExpectedFile path =
        if String.IsNullOrWhiteSpace(path) then raise (FileNotFoundException $"The expected file is not here.")
        elif File.Exists(path) then path
        else raise (FileNotFoundException $"The expected file, `{path}`, is not here.")

    /// <summary>
    /// Raise <see cref="ProgramFileError"/>.
    /// </summary>
    let raiseProgramFileError (ProgramFileError e) = raise e

    /// <summary>
    /// Removes conventional Directory prefixes
    /// for relative paths, e.g. <c>..\</c> or <c>.\</c>
    /// </summary>
    /// <param name="path">The path.</param>
    let  removeBackslashPrefixes (path: string) =
        if String.IsNullOrWhiteSpace(path) then path
        else
            path
                .TrimStart(backSlash)
                .Replace($"..{backSlash}", String.Empty)
                .Replace($".{backSlash}", String.Empty)

    /// <summary>
    /// Removes conventional Directory prefixes
    /// for relative paths, e.g. <c>../</c> or <c>./</c>.
    /// </summary>
    /// <param name="path">The path.</param>
    let removeForwardSlashPrefixes (path: string) =
        if String.IsNullOrWhiteSpace(path) then path
        else
            path
                .TrimStart(forwardSlash)
                .Replace($"..{forwardSlash}", String.Empty)
                .Replace($".{forwardSlash}", String.Empty)

    /// <summary>
    /// Removes conventional Directory prefixes
    /// for relative paths based on the ambient value\
    /// of <see cref="Path.DirectorySeparatorChar"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    let removeConventionalPrefixes path =
        if String.IsNullOrWhiteSpace(path) then path
        elif isForwardSlashSystem then removeForwardSlashPrefixes path
        else removeBackslashPrefixes path

    /// <summary>
    /// Trims the leading directory separator chars.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// Trims leading <see cref="Path.AltDirectorySeparatorChar"/> and/or <see cref="Path.DirectorySeparatorChar"/>,
    /// formatting relative paths for <see cref="Path.Combine(string, string)"/>.
    /// </remarks>
    let trimLeadingDirectorySeparatorChars path =
        if String.IsNullOrWhiteSpace(path) then path
        else path.TrimStart(backSlash, forwardSlash)

    /// <summary>
    /// Tries to ensure that the specified file segment is a relative path.
    /// </summary>
    /// <param name="fileSegment">The file segment.</param>
    /// <remarks>
    /// This function will call <see cref="trimLeadingDirectorySeparatorChars(string)"/>.
    /// </remarks>
    let tryRelativePath fileSegment =
        if String.IsNullOrWhiteSpace(fileSegment) then
            Error (ProgramFileError (NullReferenceException "The expected path is not here."))
        else
            fileSegment
                |> trimLeadingDirectorySeparatorChars
                |> fun p ->
                    if Path.IsPathRooted(p) then
                        Error (ProgramFileError (FormatException "The expected relative path is not here."))
                    else Ok p

    /// <summary>
    /// Tries to get the relative path from the specified file segment
    /// without leading dots (<c>.</c>) or <see cref="Path.DirectorySeparatorChar" /> chars.
    /// </summary>
    /// <param name="fileSegment">The file segment.</param>
    /// <remarks>
    /// This function is the equivalent of calling:
    ///  * <see cref="trimLeadingDirectorySeparatorChars(string)"/>
    ///  * <see cref="normalizePath(string)"/>
    ///  * <see cref="removeBackslashPrefixes(string)"/>
    ///  * <see cref="removeForwardSlashPrefixes(string)"/>
    /// </remarks>
    let tryGetRelativePath fileSegment =
        if String.IsNullOrWhiteSpace(fileSegment) then
            Error (ProgramFileError (NullReferenceException "The expected path is not here."))
        else
            fileSegment
                |> trimLeadingDirectorySeparatorChars
                |> normalizePath
                |> removeBackslashPrefixes
                |> removeForwardSlashPrefixes
                |> Ok

    /// <summary>
    /// Tries to combine path and root based
    /// on the ambient value of <see cref="Path.DirectorySeparatorChar"/>
    /// of the current OS.</summary>
    /// <param name="root">The root.</param>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// For detail, see:
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/14
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/32
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/97
    /// </remarks>
    let tryGetCombinedPath root path =
        if String.IsNullOrWhiteSpace(root) then
            Error (ProgramFileError (NullReferenceException $"The expected {nameof(root)} is not here."))
        else if String.IsNullOrWhiteSpace(path) then
            Error (ProgramFileError (NullReferenceException $"The expected {nameof(path)} is not here."))
        else
            let relativePathResult = path |> tryGetRelativePath
            match relativePathResult with
            | Error err -> Error err
            | Ok relativePath ->
                if Path.IsPathRooted(relativePath) then Ok relativePath
                else Ok (Path.Combine(normalizePath(root), relativePath))
