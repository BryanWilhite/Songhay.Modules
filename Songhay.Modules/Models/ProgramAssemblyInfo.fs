namespace Songhay.Modules.Models

open System
open System.IO
open System.Reflection
open System.Text

open Songhay.Modules

/// <summary>
/// Defines Assembly information.
/// </summary>
type ProgramAssemblyInfo =
    {
        AssemblyCompany: string
        AssemblyCopyright: string
        AssemblyDescription: string
        AssemblyProduct: string
        AssemblyTitle: string
        AssemblyVersion: string
        AssemblyVersionDetail: string
        AssemblyFileName: string
        AssemblyPath: string
    }

    /// <summary>
    /// Returns <see cref="ProgramAssemblyInfo" />
    /// from <see cref="Assembly" /> input.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member fromInput(assembly: Assembly): ProgramAssemblyInfo =
        {
            AssemblyCompany = assembly |> ProgramAssemblyInfo.getAssemblyCompany
            AssemblyCopyright = assembly |> ProgramAssemblyInfo.getAssemblyCopyright
            AssemblyDescription = assembly |> ProgramAssemblyInfo.getAssemblyDescription
            AssemblyProduct = assembly |> ProgramAssemblyInfo.getAssemblyProduct
            AssemblyTitle = assembly |> ProgramAssemblyInfo.getAssemblyTitle
            AssemblyVersion = assembly |> ProgramAssemblyInfo.getAssemblyVersion
            AssemblyVersionDetail = assembly |> ProgramAssemblyInfo.getAssemblyVersionDetail
            AssemblyFileName = assembly |> ProgramAssemblyInfo.getAssemblyFileName
            AssemblyPath = assembly |> ProgramAssemblyInfo.getAssemblyPath
        }

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="Assembly.Company" />
    /// or <see cref="System.String.Empty" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyCompany(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyCompanyAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyCompanyAttribute
            attribute.Company
        else String.Empty

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="Assembly.Copyright" />
    /// or <see cref="System.String.Empty" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyCopyright(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyCopyrightAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyCopyrightAttribute
            attribute.Copyright
        else String.Empty

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="Assembly.Description" />
    /// or <see cref="System.String.Empty" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyDescription(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyDescriptionAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyDescriptionAttribute
            attribute.Description
        else String.Empty

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="Assembly.Product" />
    /// or <see cref="System.String.Empty" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyProduct(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyProductAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyProductAttribute
            attribute.Product
        else String.Empty

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="Assembly.Title" />
    /// or the <see cref="Assembly" /> file name.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyTitle(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyTitleAttribute>, false)

        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyTitleAttribute
            let hasTitle = not (String.IsNullOrWhiteSpace(attribute.Title))
            if hasTitle then attribute.Title
            else Path.GetFileNameWithoutExtension(assembly.Location)
        else Path.GetFileNameWithoutExtension(assembly.Location)

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="AssemblyName.Version" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyVersion(assembly: Assembly) =
        let name = assembly.GetName()
        name.Version.ToString()

    /// <summary>
    /// Returns the <see cref="System.String" />
    /// of <see cref="AssemblyName.Version.Major" />
    /// and <see cref="AssemblyName.Version.Minor" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyVersionDetail(assembly: Assembly) =
        let name = assembly.GetName()
        $"{name.Version.Major:D}.{name.Version.Minor:D2}"

    /// <summary>
    /// Gets the <see cref="Assembly" /> file name.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyFileName(assembly: Assembly) =
        Path.GetFileName(assembly.Location)

    /// <summary>
    /// Gets the <see cref="Assembly" /> path.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    static member getAssemblyPath(assembly: Assembly) =
        Path.GetDirectoryName(assembly.Location)

    /// <summary>
    /// Tries to get a combined path
    /// from the specified path
    /// and the <see cref="Assembly" /> path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="assembly">The <see cref="Assembly" />.</param>
    /// <returns></returns>
    static member getPathFromAssembly path (assembly: Assembly) =
        match path |> ProgramFileUtility.tryRelativePath with
        | Error err -> Error err
        | Ok relativePath ->
            let pathNormalized = relativePath |> ProgramFileUtility.normalizePath

            let root = assembly |> ProgramAssemblyInfo.getAssemblyPath
            let levels = pathNormalized |> ProgramFileUtility.countParentDirectoryChars
            if (levels > 0) then
                match root |> ProgramFileUtility.tryGetParentDirectory levels with
                | Error err -> Error err
                | Ok parentDirectory -> (parentDirectory, pathNormalized) ||> ProgramFileUtility.tryGetCombinedPath
            else
                Ok root

    /// <summary>
    /// Returns the <see cref="System.String" /> representation
    /// of this <see cref="ProgramAssemblyInfo" />.
    /// </summary>
    member this.getAssemblyInfo =
        StringBuilder()
            .AppendFormat($"{this.AssemblyTitle} {this.AssemblyVersion}")
            .Append(Environment.NewLine)
            .AppendLine(this.AssemblyDescription)
            .AppendLine(this.AssemblyCopyright)
            .ToString()
