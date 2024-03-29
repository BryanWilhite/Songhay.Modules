namespace Songhay.Modules.Models

open Songhay.Modules.StringUtility

///<summary>
/// Summarizes the CSS color property names.
///</summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Colors/Applying_color
/// </remarks>
type CssColorProperty =
    /// <summary> a CSS color property name </summary>
    | ColorPropertyBackground
    /// <summary> a CSS color property name </summary>
    | ColorProperty
    /// <summary> a CSS color property name </summary>
    | ColorPropertyBorder
    /// <summary> a CSS color property name </summary>
    | ColorPropertyOutline
    /// <summary> a CSS color property name </summary>
    | ColorPropertyTextDecoration
    /// <summary> a CSS color property name </summary>
    | ColorPropertyTextEmphasis
    /// <summary> a CSS color property name </summary>
    | ColorPropertyTextShadow
    /// <summary> a CSS color property name </summary>
    | ColorPropertyCaret
    /// <summary> a CSS color property name </summary>
    | ColorPropertyColumnRule

///<summary>
/// Enumerates a subset of the CSS inheritance values.
///</summary>
///<remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/inheritance
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/inherit
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/initial
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/revert
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/revert-layer
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/unset
///</remarks>
type CssInheritance =
    ///<summary>
    /// “The inherit CSS keyword causes the element to take the computed value
    /// of the property from its parent element.”
    ///</summary>
    | Inherit
    ///<summary>
    /// “The initial CSS keyword applies the initial (or default) value of a property to an element.”
    ///</summary>
    | Initial
    ///<summary>
    /// “The revert CSS keyword reverts the cascaded value of the property from its current value
    /// to the value the property would have had if no changes had been made
    /// by the current style origin to the current element.
    /// Thus, it resets the property to its inherited value if it inherits
    /// from its parent or to the default value established
    /// by the user agent's stylesheet (or by user styles, if any exist).”
    ///</summary>
    | Revert
    ///<summary>
    /// “The revert-layer CSS keyword rolls back the value of a property
    /// in a cascade layer to the value of the property
    /// in a CSS rule matching the element in a previous cascade layer.”
    ///</summary>
    | RevertLayer
    ///<summary>
    /// “The unset CSS keyword resets a property to its inherited value
    /// if the property naturally inherits from its parent,
    /// and to its initial value if not.”
    ///</summary>
    | Unset

    ///<summary>Returns the <see cref="string" /> representation of the inheritance name.</summary>
    member this.Value =
        match this with
        | RevertLayer -> "revert-layer"
        | _ -> this.ToString().ToLowerInvariant()

///<summary>
/// Defines CSS Box Alignment module names.
///</summary>
///<remarks>
/// “The CSS Box Alignment module specifies CSS features that relate to the alignment
/// of boxes in the various CSS box layout models:
/// block layout, table layout, flex layout, and grid layout…”
/// https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Box_Alignment
///</remarks>
type CssBoxAlignment =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritBoxAlignment of CssInheritance
    ///<summary> alignment name for <c>text-align</c> </summary>
    | Start
    ///<summary> alignment name for <c>text-align</c> </summary>
    | End
    ///<summary> alignment name for <c>text-align</c> </summary>
    | Left
    ///<summary> alignment name for <c>text-align</c> </summary>
    | Right
    ///<summary> alignment name for <c>text-align</c> </summary>
    | Center
    ///<summary> alignment name for <c>text-align</c> </summary>
    | Justify
    ///<summary> alignment name for <c>text-align</c> </summary>
    | JustifyAll
    ///<summary> alignment name for <c>text-align</c> </summary>
    | MatchParent
    ///<summary> alignment name for <c>vertical-align</c> </summary>
    | BaseLine
    ///<summary> alignment name for <c>vertical-align</c> </summary>
    | Top
    ///<summary> alignment name for <c>vertical-align</c> </summary>
    | Middle
    ///<summary> alignment name for <c>vertical-align</c> </summary>
    | Bottom
    ///<summary> alignment name for <c>vertical-align</c> </summary>
    | Sub
    ///<summary> alignment name for <c>vertical-align</c> </summary>
    | TextTop
    ///<summary> alignment name for <c>justify-content</c> </summary>
    | SpaceBetween
    ///<summary> alignment name for <c>justify-content</c> </summary>
    | SpaceAround
    ///<summary> alignment name for <c>justify-content</c> </summary>
    | SpaceEvenly
    ///<summary> alignment name for <c>align-items</c> (Flexbox, Grid Layout) </summary>
    | Normal
    ///<summary> alignment name for <c>align-items</c> (Flexbox, Grid Layout) </summary>
    | Stretch
    ///<summary> alignment name for <c>align-items</c> (Flexbox, Grid Layout) </summary>
    | FlexStart
    ///<summary> alignment name for <c>align-items</c> (Flexbox, Grid Layout) </summary>
    | FlexEnd
    ///<summary> alignment name for <c>align-items</c> (Flexbox, Grid Layout) </summary>
    | SelfStart
    ///<summary> alignment name for <c>align-items</c> (Flexbox, Grid Layout) </summary>
    | SelfEnd

    ///<summary>Returns the <see cref="string" /> representation of the alignment name.</summary>
    member this.Value =
        match this with
        | InheritBoxAlignment i -> i.Value
        // text-align
        | Start -> "start"
        | End -> "end"
        | Left -> "left"
        | Right -> "right"
        | Center -> "center"
        | Justify -> "justify"
        | JustifyAll -> "justify-all"
        | MatchParent -> "match-parent"
        // vertical-align
        | BaseLine -> "baseline"
        | Top -> "top"
        | Middle -> "middle"
        | Bottom -> "bottom"
        | Sub -> "sub"
        | TextTop -> "text-top"
        // justify-content
        | SpaceBetween -> "space-between"
        | SpaceAround -> "space-around"
        | SpaceEvenly -> "space-evenly"
        // align-items (Flexbox, Grid Layout)
        | Normal -> "normal"
        | Stretch -> "stretch"
        | FlexStart -> "flex-start"
        | FlexEnd -> "flex-end"
        | SelfStart -> "self-start"
        | SelfEnd -> "self-end"

///<summary>
/// Enumerates the CSS Box Model
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Learn/CSS/Building_blocks/The_box_model#parts_of_a_box
/// </remarks>
type CssBoxModel =
    ///<summary>all margins, paddings or borders</summary>
    | All
    ///<summary>left margin, padding or border</summary>
    | L
    ///<summary>right margin, padding or border</summary>
    | R
    ///<summary>left and right margin, padding or border</summary>
    | LR
    ///<summary>top margin, padding or border</summary>
    | T
    ///<summary>bottom margin, padding or border</summary>
    | B
    ///<summary>top and bottom margin, padding or border</summary>
    | TB

    ///<summary>Returns the <see cref="string" /> representation of the Box Model instance.</summary>
    member this.Value =
        match this with
        | All -> System.String.Empty
        | LR -> "x"
        | TB -> "y"
        | _ -> this.ToString().ToLowerInvariant()

///<summary>
/// Names the typical aspect-ratio numbers of contemporary, still photography.
/// </summary>
/// <remarks>
/// “Common aspect ratios in still photography include:
///  - 1:1
///  - 5:4 (1.25:1)
///  - 4:3 (1.3:1)
///  - 3:2 (1.5:1)
///  - 5:3 (1.6:1)
///  - 16:9 (1.7:1)
///  - 3:1
/// ”
/// — https://en.wikipedia.org/wiki/Aspect_ratio_%28image%29#Still_photography
/// </remarks>
type CssCommonImageAspectRatioNumber =
    ///<summary> typical aspect-ratio number </summary>
    | One
    ///<summary> typical aspect-ratio number </summary>
    | Two
    ///<summary> typical aspect-ratio number </summary>
    | Three
    ///<summary> typical aspect-ratio number </summary>
    | Four
    ///<summary> typical aspect-ratio number </summary>
    | Five
    ///<summary> typical aspect-ratio number </summary>
    | Nine
    ///<summary> typical aspect-ratio number </summary>
    | Sixteen

    ///<summary>Returns the <see cref="string" /> representation of the aspect-ratio number.</summary>
    member this.Value =
        match this with
        | One -> "1"
        | Two -> "2"
        | Three -> "3"
        | Four -> "4"
        | Five -> "5"
        | Nine -> "9"
        | Sixteen -> "16"

///<summary>
/// The <c>flex-direction</c> CSS property sets how flex items are placed in the flex container
/// defining the main axis and the direction (normal or reversed).
///</summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/flex-direction
/// </remarks>
type CssFlexDirection =
    ///<summary> a CSS <c>flex-direction</c> </summary>
    | Row
    ///<summary> a CSS <c>flex-direction</c> </summary>
    | RowReverse
    ///<summary> a CSS <c>flex-direction</c> </summary>
    | Column
    ///<summary> a CSS <c>flex-direction</c> </summary>
    | ColumnReverse

    ///<summary>Returns the <see cref="string" /> representation of the <c>flex-direction</c> property.</summary>
    member this.Value = this.ToString() |> toKabobCase |> Option.get

///<summary>
/// The <c>flex-wrap</c> CSS property sets whether flex items are forced onto one line or can wrap onto multiple lines.
///</summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/flex-wrap
/// </remarks>
type CssFlexWrap =
    ///<summary> a CSS <c>flex-wrap</c> property </summary>
    | NoWrap
    ///<summary> a CSS <c>flex-wrap</c> property </summary>
    | Wrap
    ///<summary> a CSS <c>flex-wrap</c> property </summary>
    | WrapReverse

    ///<summary>Returns the <see cref="string" /> representation of the <c>flex-wrap</c> property.</summary>
    member this.Value =
        match this with
        | WrapReverse -> this.ToString() |> toKabobCase |> Option.get
        | _ -> this.ToString() |> toLowerInvariant |> Option.get

///<summary>
/// Enumerates a subset of the CSS font families
/// and names font classifications typically in CSS frameworks.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-family#syntax
/// </remarks>
type CssFontFamily =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontFamily of CssInheritance
    ///<summary> a CSS font family </summary>
    | SansSerif
    ///<summary> a CSS font family </summary>
    | Monospace
    ///<summary> a typical font classification </summary>
    | Primary
    ///<summary> a typical font classification </summary>
    | Secondary
    ///<summary> a CSS font family </summary>
    | Emoji
    ///<summary> a CSS font family </summary>
    | Math

    ///<summary>Returns the <see cref="string" /> representation of the font family/classification.</summary>
    member this.Value =
        match this with
        | InheritFontFamily i -> i.Value
        | SansSerif -> "sans-serif"
        | _ -> this.ToString().ToLowerInvariant()

///<summary>
/// Specifies the values of the <c>font-variant-alternates</c> property.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-variant-alternates
/// </remarks>
type CssFontVariantAlternates =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontVariantAlternates of CssInheritance
    ///<summary> a value of the <c>font-variant-alternates</c> property </summary>
    | FontVariantAlternatesNormal
    ///<summary> a value of the <c>font-variant-alternates</c> property </summary>
    | FontVariantAlternatesHistoricalForms

    ///<summary>Returns the <see cref="string" /> representation of the <c>font-variant-alternates</c> property.</summary>
    member this.Value =
        match this with
        | InheritFontVariantAlternates i -> i.Value
        | FontVariantAlternatesNormal -> "normal"
        | FontVariantAlternatesHistoricalForms -> "historical-forms"

///<summary>
/// Specifies the values of the <c>font-variant-caps</c> property.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-variant-caps
/// </remarks>
type CssFontVariantCaps =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontVariantCaps of CssInheritance
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsNormal
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsSmall
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsAllSmall
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsPetite
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsAllPetite
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsUnicase
    ///<summary> a value of the <c>font-variant-caps</c> property </summary>
    | FontVariantCapsTitling

    ///<summary>Returns the <see cref="string" /> representation of the <c>font-variant-caps</c> property.</summary>
    member this.Value =
        match this with
        | InheritFontVariantCaps i -> i.Value
        | FontVariantCapsNormal -> "normal"
        | FontVariantCapsSmall -> "small-caps"
        | FontVariantCapsAllSmall -> "all-small-caps"
        | FontVariantCapsPetite -> "petite-caps"
        | FontVariantCapsAllPetite -> "all-petite-caps"
        | FontVariantCapsUnicase -> "unicase"
        | FontVariantCapsTitling -> "titling-caps"

///<summary>
/// Specifies the values of the <c>font-variant-ligatures</c> property.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-variant-ligatures
/// </remarks>
type CssFontVariantLigatures =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontVariantLigatures of CssInheritance
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesNormal
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesNone
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesCommon
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesNoCommon
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesDiscretionary
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesNoDiscretionary
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesHistorical
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesNoHistorical
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesContextual
    ///<summary> a value of the <c>font-variant-ligatures</c> property </summary>
    | FontVariantLigaturesNoContextual

    ///<summary>Returns the <see cref="string" /> representation of the <c>font-variant-ligatures</c> property.</summary>
    member this.Value =
        match this with
        | InheritFontVariantLigatures i -> i.Value
        | FontVariantLigaturesNormal -> "normal"
        | FontVariantLigaturesNone -> "none"
        | FontVariantLigaturesCommon -> "common-ligatures"
        | FontVariantLigaturesNoCommon -> "no-common-ligatures"
        | FontVariantLigaturesDiscretionary -> "discretionary-ligatures"
        | FontVariantLigaturesNoDiscretionary -> "no-discretionary-ligatures"
        | FontVariantLigaturesHistorical -> "historical-ligatures"
        | FontVariantLigaturesNoHistorical -> "no-historical-ligatures"
        | FontVariantLigaturesContextual -> "contextual"
        | FontVariantLigaturesNoContextual -> "no-contextual"

///<summary>
/// Specifies the values of the <c>font-variant-numeric</c> property.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-variant-numeric
/// </remarks>
type CssFontVariantNumeric =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontVariantNumeric of CssInheritance
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantNumericNormal
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantNumericOrdinal
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantNumericSlashedZero
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantNumericLining
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantOldStyle
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantProportional
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantTabular
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantDiagonalFractions
    ///<summary> a value of the <c>font-variant-numeric</c> property </summary>
    | FontVariantStackedFractions

    ///<summary>Returns the <see cref="string" /> representation of the <c>font-variant-numeric</c> property.</summary>
    member this.Value =
        match this with
        | InheritFontVariantNumeric i -> i.Value
        | FontVariantNumericNormal -> "normal"
        | FontVariantNumericOrdinal -> "ordinal"
        | FontVariantNumericSlashedZero -> "slashed-zero"
        | FontVariantNumericLining -> "lining-nums"
        | FontVariantOldStyle -> "oldstyle-nums"
        | FontVariantProportional -> "proportional-nums"
        | FontVariantTabular -> "tabular-nums"
        | FontVariantDiagonalFractions -> "diagonal-fractions"
        | FontVariantStackedFractions -> "stacked-fractions"

///<summary>
/// Specifies a subset of the values of the <c>font-variant</c> shorthand property
/// and the values of all of its constituent properties.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-variant#constituent_properties
/// </remarks>
type CssFontVariant =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontVariant of CssInheritance
    ///<summary> a value of the <c>font-variant</c> property </summary>
    | VariantNone
    ///<summary> a value of the <c>font-variant</c> property </summary>
    | SmallCaps
    ///<summary> a value of the <c>font-variant</c> property </summary>
    | Ligatures of CssFontVariantLigatures
    ///<summary> a value of the <c>font-variant</c> property </summary>
    | FontAlternate of CssFontVariantAlternates
    ///<summary> a value of the <c>font-variant</c> property </summary>
    | Caps of CssFontVariantCaps
    ///<summary> a value of the <c>font-variant</c> property </summary>
    | NumericFontVariant of CssFontVariantNumeric

    ///<summary>Returns the <see cref="string" /> representation of the font variant.</summary>
    member this.Value =
        match this with
        | InheritFontVariant i -> i.Value
        | VariantNone -> "none"
        | SmallCaps -> "small-caps"
        | Ligatures l -> l.Value
        | FontAlternate a -> a.Value
        | Caps c -> c.Value
        | NumericFontVariant n -> n.Value

///<summary>
/// Enumerates a subset of the CSS font weights
/// and names font weights typically in CSS frameworks.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/font-weight#syntax
/// </remarks>
type CssFontWeight =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritFontWeight of CssInheritance
    ///<summary> a typical font weight in place of numeric CSS weights </summary>
    | Light
    ///<summary> a CSS font weight </summary>
    | Normal
    ///<summary> a typical font weight in place of numeric CSS weights </summary>
    | Medium
    ///<summary> a typical font weight in place of numeric CSS weights </summary>
    | Semibold
    ///<summary> a CSS font weight </summary>
    | Bold

    ///<summary>Returns the <see cref="string" /> representation of the font weight.</summary>
    member this.Value =
        match this with
        | InheritFontWeight i -> i.Value
        | _ -> this.ToString().ToLowerInvariant()

///<summary>
/// Enumerates a subset of the CSS text transforms
/// and names text transforms typically in CSS frameworks.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/text-transform#syntax
/// </remarks>
type CssTextTransformation =
    ///<summary> <see cref="CssInheritance" /> </summary>
    | InheritTransformation of CssInheritance
    ///<summary> a CSS text transform </summary>
    | FullWidth
    ///<summary> a CSS text transform </summary>
    | Lowercase
    ///<summary> a typical text transform, referred to as <c>captialize</c> in CSS</summary>
    | TitleCase
    ///<summary> a CSS text transform </summary>
    | UpperCase
    ///<summary> a typical text transform </summary>
    /// <remarks> italicization is not considered a CSS decoration nor a CSS variant </remarks>
    | Italic

    ///<summary>Returns the <see cref="string" /> representation of the transform name.</summary>
    member this.Value =
        match this with
        | InheritTransformation i -> i.Value
        | TitleCase -> "capitalize"
        | FullWidth -> this.ToString() |> toKabobCase |> Option.get
        | _ -> this.ToString().ToLowerInvariant()

/// <summary>
/// Represents the value assigned to <see cref="CssCustomProperty"/>.
/// </summary>
type CssValue =
    ///<summary> the CSS value </summary>
    | CssValue of string

    ///<summary>Returns the <see cref="string" /> representation of the CSS value.</summary>
    member this.Value = let (CssValue v) = this in v

    ///<summary>Returns <see cref="CssValue.Value" />.</summary>
    override this.ToString() = this.Value

/// <summary>
/// Represents a CSS custom property (variable)
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties
/// </remarks>
type CssCustomProperty =
    | CssCustomProperty of string

    /// <summary>
    /// Returns an instance of <see cref="CssCustomProperty"/>
    /// based on the specified input.
    /// </summary>
    static member fromInput (input: string) =
        match input.TrimStart('-') |> toKabobCase with
        | Some s -> CssCustomProperty $"--{s}"
        | None -> CssCustomProperty "--?"

    /// <summary>
    /// Converts this instance to a <c>var()</c> function CSS property.
    /// </summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/var
    /// </remarks>
    member this.toCssPropertyValue = $"var({this.Value})"

    /// <summary>
    /// Converts this instance to a <c>var()</c> function CSS property
    /// with a fallback value.
    /// </summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/CSS/var#using_a_custom_property_as_a_fallback
    /// </remarks>
    member this.toCssPropertyValueWithFallback (fallback: string) = $"var({this.Value}, {fallback})"

    ///<summary>Returns the <see cref="string" /> representation of the CSS value.</summary>
    member this.Value = let (CssCustomProperty v) = this in v

    ///<summary>Returns <see cref="CssCustomProperty.Value" />.</summary>
    override this.ToString() = this.Value

/// <summary>
/// Centralizes <see cref="CssCustomProperty"/> and <see cref="CssValue"/> as a tuple.
/// </summary>
type CssCustomPropertyAndValue =
    | CssCustomPropertyAndValue of CssCustomProperty * CssValue

    /// <summary>
    /// Returns an instance of <see cref="CssCustomPropertyAndValue"/>
    /// based on the specified input.
    /// </summary>
    static member fromInput (name: string, value: string) =
        (CssCustomProperty.fromInput name, CssValue value) |> CssCustomPropertyAndValue

    /// <summary>
    /// Unwraps the underlying tuple of this instance.
    /// </summary>
    member this.Pair = let (CssCustomPropertyAndValue (cssVar, cssVal)) = this in (cssVar, cssVal)

    /// <summary>
    /// Returns a CSS declaration for a CSS ruleset.
    /// </summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Learn/Getting_started_with_the_web/CSS_basics#anatomy_of_a_css_ruleset
    /// </remarks>
    member this.toCssDeclaration = match this with | CssCustomPropertyAndValue (cssVar, cssVal) -> $"{cssVar}: {cssVal};"
