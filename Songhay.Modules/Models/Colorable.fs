namespace Songhay.Modules.Models

/// <summary>
/// Represents a Hexadecimal value.
/// </summary>
type HexValue =
    | HexValue of string

    /// <summary>
    /// Unwraps the underlying <see cref="System.String" /> value.
    /// </summary>
    member this.Value = let (HexValue v) = this in v

/// <summary>
/// Represents a Background Color Hexadecimal value.
/// </summary>
type BackgroundColorHexValue = 
    | BackgroundColorHexValue of HexValue

    /// <summary>
    /// Returns <see cref="BackgroundColorHexValue" />
    /// from <see cref="System.String" /> input.
    /// </summary>
    static member fromString s = BackgroundColorHexValue (HexValue s)

    /// <summary>
    /// Unwraps the underlying <see cref="HexValue" /> value.
    /// </summary>
    member this.Value = let (BackgroundColorHexValue v) = this in v

    /// <summary>
    /// Unwraps the underlying <see cref="System.String" /> value.
    /// </summary>
    member this.Unwrapped = this.Value.Value

/// <summary>
/// Represents a Foreground Color Hexadecimal value.
/// </summary>
type ForegroundColorHexValue = 
    | ForegroundColorHexValue of HexValue

    /// <summary>
    /// Returns <see cref="ForegroundColorHexValue" />
    /// from <see cref="System.String" /> input.
    /// </summary>
    static member fromString s = ForegroundColorHexValue (HexValue s)

    /// <summary>
    /// Unwraps the underlying <see cref="HexValue" /> value.
    /// </summary>
    member this.Value = let (ForegroundColorHexValue v) = this in v

    /// <summary>
    /// Unwraps the underlying <see cref="System.String" /> value.
    /// </summary>
    member this.Unwrapped = this.Value.Value

/// <summary>
/// Defines a colorable visual.
/// </summary>
type Colorable =
    {
        /// <summary>
        /// Gets or sets the background hexadecimal value.
        /// </summary>
        /// <value>The background hexadecimal value.</value>
        backgroundHex: BackgroundColorHexValue

        /// <summary>
        /// Gets or sets the foreground hexadecimal value.
        /// </summary>
        /// <value>The foreground hexadecimal value.</value>
        foregroundHex: ForegroundColorHexValue
    }
