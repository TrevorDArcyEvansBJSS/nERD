// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using NClass.Translations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NClass.DiagramEditor
{
  public enum GradientStyle
  {
    None,
    Horizontal,
    Vertical,
    Diagonal
  }

  [Serializable]
  [DefaultProperty("AttributeColor")]
  public sealed class Style : IDisposable
  {
    public static event EventHandler CurrentStyleChanged;

    private static readonly SortedList<string, Style> Styles = new SortedList<string, Style>();
    private static readonly string SettingsDir = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NClass");
    private static readonly string UserStylePath = Path.Combine(SettingsDir, "style.dst");

    static Style()
    {
      Directory.CreateDirectory(StylesDirectory);
      LoadStyles();
      if (!LoadCurrentStyle())
      {
        CurrentStyle = new Style();
        SaveCurrentStyle();
      }
    }

    public Style()
    {
      _abstractNameFont = new Font(_nameFont, _nameFont.Style | FontStyle.Italic);
      _staticMemberFont = new Font(_memberFont, _memberFont.Style | FontStyle.Underline);
      _abstractMemberFont = new Font(_memberFont, _memberFont.Style | FontStyle.Italic);
    }

    public static IEnumerable<Style> AvailableStyles
    {
      get { return Styles.Values; }
    }

    private static Style _currentStyle;
    public static Style CurrentStyle
    {
      get
      {
        return _currentStyle;
      }
      set
      {
        if (value != null && _currentStyle != value)
        {
          if (_currentStyle != null)
            _currentStyle.Dispose();
          _currentStyle = (Style)value.Clone();
          SaveCurrentStyle();
          if (CurrentStyleChanged != null)
            CurrentStyleChanged(null, EventArgs.Empty);
        }
      }
    }

    public static string StylesDirectory { get; } = Path.Combine(SettingsDir, "UserStyles");

    #region Style Information

    [Browsable(false)]
    public bool IsUntitled
    {
      get
      {
        return string.IsNullOrEmpty(_name);
      }
    }

    private string _name = null;
    [DisplayName("Style Name"), Category("(Style Information)")]
    [Description("The name of the current style.")]
    public string Name
    {
      get
      {
        if (string.IsNullOrEmpty(_name))
          return Strings.Untitled;
        else
          return _name;
      }
      set
      {
        if (value == Strings.Untitled)
          _name = null;
        else
          _name = value;
      }
    }

    private string _author = null;
    [DisplayName("Author"), Category("(Style Information)")]
    [Description("The author of the current style.")]
    public string Author
    {
      get
      {
        if (string.IsNullOrEmpty(_author))
          return Strings.Unknown;
        else
          return _author;
      }
      set
      {
        if (value == Strings.Unknown)
          _author = null;
        else
          _author = value;
      }
    }

    #endregion

    #region Background properties

    [DisplayName("Background Color"), Category("(Background)")]
    [Description("The background color for the diagram.")]
    [DefaultValue(typeof(Color), "White")]
    public Color BackgroundColor { get; set; } = Color.White;

    #endregion

    #region All Types properties

    [DisplayName("Attribute Color"), Category("(All Types)")]
    [Description("The text color of a type's attributes.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color AttributeColor { get; set; } = Color.Black;

    [DisplayName("Background Color"), Category("(All Types)")]
    [Description("The background color for all types.")]
    [DefaultValue(typeof(Color), "White")]
    public Color TypeBackgroundColor
    {
      get
      {
        Color color = ClassBackgroundColor;

        if (StructureBackgroundColor == color &&
          InterfaceBackgroundColor == color &&
          EnumBackgroundColor == color &&
          DelegateBackgroundColor == color)
        {
          return color;
        }
        else
        {
          return Color.Empty;
        }
      }
      set
      {
        if (value != Color.Empty)
        {
          ClassBackgroundColor = value;
          StructureBackgroundColor = value;
          InterfaceBackgroundColor = value;
          EnumBackgroundColor = value;
          DelegateBackgroundColor = value;
        }
      }
    }

    [DisplayName("Border Color"), Category("(All Types)")]
    [Description("The border color for all types.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color BorderColor
    {
      get
      {
        Color color = ClassBorderColor;

        if (StructureBorderColor == color &&
          InterfaceBorderColor == color &&
          EnumBorderColor == color &&
          DelegateBorderColor == color)
        {
          return color;
        }
        else
        {
          return Color.Empty;
        }
      }
      set
      {
        if (value != Color.Empty)
        {
          ClassBorderColor = value;
          StructureBorderColor = value;
          InterfaceBorderColor = value;
          EnumBorderColor = value;
          DelegateBorderColor = value;
        }
      }
    }

    private int _classBorderWidth = 1;
    [DisplayName("Border Width"), Category("(All Types)")]
    [Description("The border width for all types.")]
    [DefaultValue(1)]
    public int? BorderWidth
    {
      get
      {
        int width = _classBorderWidth;

        if (_structureBorderWidth == width &&
          _interfaceBorderWidth == width &&
          _enumBorderWidth == width &&
          _delegateBorderWidth == width &&
          _abstractClassBorderWidth == width &&
          _sealedClassBorderWidth == width &&
          _staticClassBorderWidth == width)
        {
          return width;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (value.HasValue)
        {
          int width = (value.Value < 1) ? 1 : value.Value;

          _classBorderWidth = width;
          _structureBorderWidth = width;
          _interfaceBorderWidth = width;
          _enumBorderWidth = width;
          _delegateBorderWidth = width;
          _abstractClassBorderWidth = width;
          _sealedClassBorderWidth = width;
          _staticClassBorderWidth = width;
        }
      }
    }

    [DisplayName("Dashed Border"), Category("(All Types)")]
    [Description("Whether the border for all types will be dashed.")]
    [DefaultValue(false)]
    public bool? IsBorderDashed
    {
      get
      {
        bool dashed = IsClassBorderDashed;

        if (IsStructureBorderDashed == dashed &&
          IsInterfaceBorderDashed == dashed &&
          IsEnumBorderDashed == dashed &&
          IsDelegateBorderDashed == dashed &&
          IsAbstractClassBorderDashed == dashed &&
          IsSealedClassBorderDashed == dashed &&
          IsStaticClassBorderDashed == dashed)
        {
          return dashed;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (value.HasValue)
        {
          IsClassBorderDashed = value.Value;
          IsStructureBorderDashed = value.Value;
          IsInterfaceBorderDashed = value.Value;
          IsEnumBorderDashed = value.Value;
          IsDelegateBorderDashed = value.Value;
          IsAbstractClassBorderDashed = value.Value;
          IsSealedClassBorderDashed = value.Value;
          IsStaticClassBorderDashed = value.Value;
        }
      }
    }

    [DisplayName("Header Alignment"), Category("(All Types)")]
    [Description("Specifies text alignment within the header compartment.")]
    [DefaultValue(ContentAlignment.MiddleCenter)]
    public ContentAlignment HeaderAlignment { get; set; } = ContentAlignment.MiddleCenter;

    [DisplayName("Header Color"), Category("(All Types)")]
    [Description("The background color of the header compartment for all types.")]
    [DefaultValue(typeof(Color), "White")]
    public Color HeaderColor
    {
      get
      {
        Color color = ClassHeaderColor;

        if (StructureHeaderColor == color &&
          InterfaceHeaderColor == color &&
          EnumHeaderColor == color &&
          DelegateHeaderColor == color)
        {
          return color;
        }
        else
        {
          return Color.Empty;
        }
      }
      set
      {
        if (value != Color.Empty)
        {
          ClassHeaderColor = value;
          StructureHeaderColor = value;
          InterfaceHeaderColor = value;
          EnumHeaderColor = value;
          DelegateHeaderColor = value;
        }
      }
    }

    [DisplayName("Identifier Color"), Category("(All Types)")]
    [Description("The text color of the secondary text beside the type's name.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color IdentifierColor { get; set; } = Color.Black;

    private Font _identifierFont = new Font("Tahoma", 6.75F);
    [DisplayName("Identifier Font"), Category("(All Types)")]
    [Description("The font of the secondary text beside the type's name.")]
    public Font IdentifierFont
    {
      get
      {
        return _identifierFont;
      }
      set
      {
        if (value != null && _identifierFont != value)
        {
          _identifierFont.Dispose();
          _identifierFont = value;
        }
      }
    }

    private Font _memberFont = new Font("Tahoma", 8.25F);
    [DisplayName("Member Font"), Category("(All Types)")]
    [Description("The font of the type's members.")]
    public Font MemberFont
    {
      get
      {
        return _memberFont;
      }
      set
      {
        if (value != null && _memberFont != value)
        {
          _memberFont.Dispose();
          _staticMemberFont.Dispose();
          _abstractMemberFont.Dispose();

          _memberFont = value;
          _staticMemberFont = new Font(_memberFont, _memberFont.Style | FontStyle.Underline);
          _abstractMemberFont = new Font(_memberFont, _memberFont.Style | FontStyle.Italic);
        }
      }
    }

    [NonSerialized]
    private Font _staticMemberFont;
    [Browsable(false)]
    public Font StaticMemberFont
    {
      get { return _staticMemberFont; }
    }

    [NonSerialized]
    private Font _abstractMemberFont;
    [Browsable(false)]
    public Font AbstractMemberFont
    {
      get { return _abstractMemberFont; }
    }

    [DisplayName("Name Color"), Category("(All Types)")]
    [Description("The text color of the type's name.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color NameColor { get; set; } = Color.Black;

    private Font _nameFont = new Font("Arial", 9.75F, FontStyle.Bold);
    [DisplayName("Name Font"), Category("(All Types)")]
    [Description("The font of the type's name.")]
    public Font NameFont
    {
      get
      {
        return _nameFont;
      }
      set
      {
        if (value != null && _nameFont != value)
        {
          _nameFont.Dispose();
          _abstractNameFont.Dispose();

          _nameFont = value;
          _abstractNameFont = new Font(_nameFont, _nameFont.Style | FontStyle.Italic);
        }
      }
    }

    [NonSerialized]
    private Font _abstractNameFont;
    [Browsable(false)]
    public Font AbstractNameFont
    {
      get { return _abstractNameFont; }
    }

    [DisplayName("Operation Color"), Category("(All Types)")]
    [Description("The text color of a type's operations.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color OperationColor { get; set; } = Color.Black;

    private int _classRoundingSize = 0;
    [DisplayName("Rounding Size"), Category("(All Types)")]
    [Description("The rounding size of the corners for all types.")]
    [DefaultValue(0)]
    public int? RoundingSize
    {
      get
      {
        int size = _classRoundingSize;

        if (_structureRoundingSize == size &&
          _interfaceRoundingSize == size &&
          _enumRoundingSize == size &&
          _delegateRoundingSize == size)
        {
          return size;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (value.HasValue)
        {
          int size = (value.Value < 0) ? 0 : value.Value;

          _classRoundingSize = size;
          _structureRoundingSize = size;
          _interfaceRoundingSize = size;
          _enumRoundingSize = size;
          _delegateRoundingSize = size;
        }
      }
    }

    private Size _shadowOffset = new Size(4, 4);
    [DisplayName("Shadow"), Category("(All Types)")]
    [Description("The offset of the shadow for all entities.")]
    [DefaultValue(typeof(Size), "4, 4")]
    public Size ShadowOffset
    {
      get
      {
        return _shadowOffset;
      }
      set
      {
        if (value.Width < 0)
          value.Width = 0;
        if (value.Height < 0)
          value.Height = 0;

        _shadowOffset = value;
      }
    }

    [DisplayName("Shadow Color"), Category("(All Types)")]
    [Description("The color of the shadow.")]
    [DefaultValue(typeof(Color), "70,0,0,0")]
    public Color ShadowColor { get; set; } = Color.FromArgb(70, Color.Black);

    private bool _showSignature = false;
    [DisplayName("Show Signature"), Category("(All Types)")]
    [Description("Whether to show detailed type description within the header compartment.")]
    [DefaultValue(false)]
    public bool ShowSignature
    {
      get
      {
        return _showSignature;
      }
      set
      {
        if (value && ShowStereotype)
          ShowStereotype = false;
        _showSignature = value;
      }
    }

    private bool _showStereotypes = true;
    [DisplayName("Show Stereotype"), Category("(All Types)")]
    [Description("Whether to show stereotype within the header compartment.")]
    [DefaultValue(true)]
    public bool ShowStereotype
    {
      get
      {
        return _showStereotypes;
      }
      set
      {
        if (value && ShowSignature)
          ShowSignature = false;
        _showStereotypes = value;
      }
    }

    [DisplayName("Header Gradient Style"), Category("(All Types)")]
    [Description("The direction of the gradient header color in all types.")]
    [DefaultValue(GradientStyle.None)]
    public GradientStyle? HeaderGradientStyle
    {
      get
      {
        GradientStyle value = ClassGradientHeaderStyle;

        if (StructureGradientHeaderStyle == value &&
          InterfaceGradientHeaderStyle == value &&
          EnumGradientHeaderStyle == value &&
          DelegateGradientHeaderStyle == value)
        {
          return value;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (value.HasValue)
        {
          ClassGradientHeaderStyle = value.Value;
          StructureGradientHeaderStyle = value.Value;
          InterfaceGradientHeaderStyle = value.Value;
          EnumGradientHeaderStyle = value.Value;
          DelegateGradientHeaderStyle = value.Value;
        }
      }
    }

    [DisplayName("Use Icons"), Category("(All Types)")]
    [Description("Whether to use member type icons in the " +
      "attributes and operations compartments.")]
    [DefaultValue(false)]
    public bool UseIcons { get; set; } = false;

    #endregion

    #region Class properties

    [DisplayName("Background Color"), Category("Class")]
    [Description("The background color for the class type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color ClassBackgroundColor { get; set; } = Color.White;

    [DisplayName("Border Color"), Category("Class")]
    [Description("The border color for the class type.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color ClassBorderColor { get; set; } = Color.Black;

    [DisplayName("Border Width"), Category("Class")]
    [Description("The border width for the class type.")]
    [DefaultValue(1)]
    public int ClassBorderWidth
    {
      get
      {
        return _classBorderWidth;
      }
      set
      {
        if (value < 1)
          _classBorderWidth = 1;
        else
          _classBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Class")]
    [Description("Whether the border for the class type will be dashed.")]
    [DefaultValue(false)]
    public bool IsClassBorderDashed { get; set; } = false;

    [DisplayName("Header Color"), Category("Class")]
    [Description("The background color of the header compartment for the class type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color ClassHeaderColor { get; set; } = Color.White;

    [DisplayName("Rounding Size"), Category("Class")]
    [Description("The rounding size of the corners for the class type.")]
    [DefaultValue(0)]
    public int ClassRoundingSize
    {
      get
      {
        return _classRoundingSize;
      }
      set
      {
        if (value < 0)
          _classRoundingSize = 0;
        else
          _classRoundingSize = value;
      }
    }

    [DisplayName("Header Gradient Style"), Category("Class")]
    [Description("The direction of the gradient header color in class types.")]
    [DefaultValue(GradientStyle.None)]
    public GradientStyle ClassGradientHeaderStyle { get; set; } = GradientStyle.None;

    #endregion

    #region Modified class properties

    private int _abstractClassBorderWidth = 1;
    [DisplayName("Border Width"), Category("Abstract Class")]
    [Description("The border width for abstract classes.")]
    [DefaultValue(1)]
    public int AbstractClassBorderWidth
    {
      get
      {
        return _abstractClassBorderWidth;
      }
      set
      {
        if (value < 1)
          _abstractClassBorderWidth = 1;
        else
          _abstractClassBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Abstract Class")]
    [Description("Whether the border for abstract classes will be dashed.")]
    [DefaultValue(false)]
    public bool IsAbstractClassBorderDashed { get; set; } = false;

    private int _sealedClassBorderWidth = 1;
    [DisplayName("Border Width"), Category("Sealed Class")]
    [Description("The border width for sealed classes.")]
    [DefaultValue(1)]
    public int SealedClassBorderWidth
    {
      get
      {
        return _sealedClassBorderWidth;
      }
      set
      {
        if (value < 1)
          _sealedClassBorderWidth = 1;
        else
          _sealedClassBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Sealed Class")]
    [Description("Whether the border for sealed classes will be dashed.")]
    [DefaultValue(false)]
    public bool IsSealedClassBorderDashed { get; set; } = false;

    private int _staticClassBorderWidth = 1;
    [DisplayName("Border Width"), Category("Static Class")]
    [Description("The border width for static classes.")]
    [DefaultValue(1)]
    public int StaticClassBorderWidth
    {
      get
      {
        return _staticClassBorderWidth;
      }
      set
      {
        if (value < 1)
          _staticClassBorderWidth = 1;
        else
          _staticClassBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Static Class")]
    [Description("Whether the border for static classes will be dashed.")]
    [DefaultValue(false)]
    public bool IsStaticClassBorderDashed { get; set; } = false;

    #endregion

    #region Structure properties

    [DisplayName("Background Color"), Category("Structure")]
    [Description("The background color for the structure type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color StructureBackgroundColor { get; set; } = Color.White;

    [DisplayName("Border Color"), Category("Structure")]
    [Description("The border color for the structure type.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color StructureBorderColor { get; set; } = Color.Black;

    private int _structureBorderWidth = 1;
    [DisplayName("Border Width"), Category("Structure")]
    [Description("The border width for the structure type.")]
    [DefaultValue(1)]
    public int StructureBorderWidth
    {
      get
      {
        return _structureBorderWidth;
      }
      set
      {
        if (value < 1)
          _structureBorderWidth = 1;
        else
          _structureBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Structure")]
    [Description("Whether the border for the structure type will be dashed.")]
    [DefaultValue(false)]
    public bool IsStructureBorderDashed { get; set; } = false;

    [DisplayName("Header Color"), Category("Structure")]
    [Description("The background color of the header compartment for the structure type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color StructureHeaderColor { get; set; } = Color.White;

    private int _structureRoundingSize = 0;
    [DisplayName("Rounding Size"), Category("Structure")]
    [Description("The rounding size of the corners for the structure type.")]
    [DefaultValue(0)]
    public int StructureRoundingSize
    {
      get
      {
        return _structureRoundingSize;
      }
      set
      {
        if (value < 0)
          _structureRoundingSize = 0;
        else
          _structureRoundingSize = value;
      }
    }

    [DisplayName("Header Gradient Style"), Category("Structure")]
    [Description("The direction of the gradient header color in structure types.")]
    [DefaultValue(GradientStyle.None)]
    public GradientStyle StructureGradientHeaderStyle { get; set; } = GradientStyle.None;

    #endregion

    #region Interface properties

    [DisplayName("Background Color"), Category("Interface")]
    [Description("The background color for the interface type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color InterfaceBackgroundColor { get; set; } = Color.White;

    [DisplayName("Border Color"), Category("Interface")]
    [Description("The border color for the interface type.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color InterfaceBorderColor { get; set; } = Color.Black;

    private int _interfaceBorderWidth = 1;
    [DisplayName("Border Width"), Category("Interface")]
    [Description("The border width for the interface type.")]
    [DefaultValue(1)]
    public int InterfaceBorderWidth
    {
      get
      {
        return _interfaceBorderWidth;
      }
      set
      {
        if (value < 1)
          _interfaceBorderWidth = 1;
        else
          _interfaceBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Interface")]
    [Description("Whether the border for the interface type will be dashed.")]
    [DefaultValue(false)]
    public bool IsInterfaceBorderDashed { get; set; } = false;

    [DisplayName("Header Color"), Category("Interface")]
    [Description("The background color of the header compartment for the interface type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color InterfaceHeaderColor { get; set; } = Color.White;

    private int _interfaceRoundingSize = 0;
    [DisplayName("Rounding Size"), Category("Interface")]
    [Description("The rounding size of the corners for the interface type.")]
    [DefaultValue(0)]
    public int InterfaceRoundingSize
    {
      get
      {
        return _interfaceRoundingSize;
      }
      set
      {
        if (value < 0)
          _interfaceRoundingSize = 0;
        else
          _interfaceRoundingSize = value;
      }
    }

    [DisplayName("Header Gradient Style"), Category("Interface")]
    [Description("The direction of the gradient header color in interface types.")]
    [DefaultValue(GradientStyle.None)]
    public GradientStyle InterfaceGradientHeaderStyle { get; set; } = GradientStyle.None;

    #endregion

    #region Enum properties

    [DisplayName("Background Color"), Category("Enum")]
    [Description("The background color for the enum type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color EnumBackgroundColor { get; set; } = Color.White;

    [DisplayName("Border Color"), Category("Enum")]
    [Description("The border color for the enum type.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color EnumBorderColor { get; set; } = Color.Black;

    private int _enumBorderWidth = 1;
    [DisplayName("Border Width"), Category("Enum")]
    [Description("The border width for the enum type")]
    [DefaultValue(1)]
    public int EnumBorderWidth
    {
      get
      {
        return _enumBorderWidth;
      }
      set
      {
        if (value < 1)
          _enumBorderWidth = 1;
        else
          _enumBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Enum")]
    [Description("Whether the border for the enum type will be dashed.")]
    [DefaultValue(false)]
    public bool IsEnumBorderDashed { get; set; } = false;

    [DisplayName("Header Color"), Category("Enum")]
    [Description("The background color of the header compartment for the enum type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color EnumHeaderColor { get; set; } = Color.White;

    [DisplayName("Item Color"), Category("Enum")]
    [Description("The text color of an enumerator item.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color EnumItemColor { get; set; } = Color.Black;

    private int _enumRoundingSize = 0;
    [DisplayName("Rounding Size"), Category("Enum")]
    [Description("The rounding size of the corners for the enum type.")]
    [DefaultValue(0)]
    public int EnumRoundingSize
    {
      get
      {
        return _enumRoundingSize;
      }
      set
      {
        if (value < 0)
          _enumRoundingSize = 0;
        else
          _enumRoundingSize = value;
      }
    }

    [DisplayName("Header Gradient Style"), Category("Enum")]
    [Description("The direction of the gradient header color in enum types.")]
    [DefaultValue(GradientStyle.None)]
    public GradientStyle EnumGradientHeaderStyle { get; set; } = GradientStyle.None;

    #endregion

    #region Delegate properties

    [DisplayName("Background Color"), Category("Delegate")]
    [Description("The background color for the delegate type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color DelegateBackgroundColor { get; set; } = Color.White;

    [DisplayName("Border Color"), Category("Delegate")]
    [Description("The border color for the delegate type.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color DelegateBorderColor { get; set; } = Color.Black;

    private int _delegateBorderWidth = 1;
    [DisplayName("Border Width"), Category("Delegate")]
    [Description("The border width for the delegate type.")]
    [DefaultValue(1)]
    public int DelegateBorderWidth
    {
      get
      {
        return _delegateBorderWidth;
      }
      set
      {
        if (value < 1)
          _delegateBorderWidth = 1;
        else
          _delegateBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Delegate")]
    [Description("Whether the border for the delegate type will be dashed.")]
    [DefaultValue(false)]
    public bool IsDelegateBorderDashed { get; set; } = false;

    [DisplayName("Header Color"), Category("Delegate")]
    [Description("The background color of the header compartment for the delegate type.")]
    [DefaultValue(typeof(Color), "White")]
    public Color DelegateHeaderColor { get; set; } = Color.White;

    [DisplayName("Parameter Color"), Category("Delegate")]
    [Description("The text color of a delegate's parameters.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color DelegateParameterColor { get; set; } = Color.Black;

    private int _delegateRoundingSize = 0;
    [DisplayName("Rounding Size"), Category("Delegate")]
    [Description("The rounding size of the corners for the delegate type.")]
    [DefaultValue(0)]
    public int DelegateRoundingSize
    {
      get
      {
        return _delegateRoundingSize;
      }
      set
      {
        if (value < 0)
          _delegateRoundingSize = 0;
        else
          _delegateRoundingSize = value;
      }
    }

    [DisplayName("Header Gradient Style"), Category("Delegate")]
    [Description("The direction of the gradient header color in delegate types.")]
    [DefaultValue(GradientStyle.None)]
    public GradientStyle DelegateGradientHeaderStyle { get; set; } = GradientStyle.None;

    #endregion

    #region Comment properties

    [DisplayName("Background Color"), Category("Comment")]
    [Description("The background color for the comment.")]
    [DefaultValue(typeof(Color), "White")]
    public Color CommentBackColor { get; set; } = Color.White;

    [DisplayName("Border Color"), Category("Comment")]
    [Description("The border color for the comment.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color CommentBorderColor { get; set; } = Color.Black;

    private int _commentBorderWidth = 1;
    [DisplayName("Border Width"), Category("Comment")]
    [Description("The border width for the comment.")]
    [DefaultValue(1)]
    public int CommentBorderWidth
    {
      get
      {
        return _commentBorderWidth;
      }
      set
      {
        if (value < 1)
          _commentBorderWidth = 1;
        else
          _commentBorderWidth = value;
      }
    }

    [DisplayName("Dashed Border"), Category("Comment")]
    [Description("Whether the border for the comment will be dashed.")]
    [DefaultValue(false)]
    public bool IsCommentBorderDashed { get; set; } = false;

    private Font _commentFont = new Font("Tahoma", 8.25F);
    [DisplayName("Font"), Category("Comment")]
    [Description("The font of the displayed text for the comment.")]
    public Font CommentFont
    {
      get
      {
        return _commentFont;
      }
      set
      {
        if (value != null && _commentFont != value)
        {
          _commentFont.Dispose();
          _commentFont = value;
        }
      }
    }

    [DisplayName("Text Color"), Category("Comment")]
    [Description("The text color for the comment.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color CommentTextColor { get; set; } = Color.Black;

    #endregion

    #region State properties

    [DisplayName("Background Color"), Category("State")]
    [Description("The background color for the State.")]
    [DefaultValue(typeof(Color), "White")]
    public Color StateBackColor { get; set; }

    [DisplayName("Border Color"), Category("State")]
    [Description("The border color for the State.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color StateBorderColor { get; set; }

    private int _stateBorderWidth = 1;
    [DisplayName("Border Width"), Category("State")]
    [Description("The border width for the State.")]
    [DefaultValue(1)]
    public int StateBorderWidth
    {
      get
      {
        return _stateBorderWidth;
      }
      set
      {
        if (value < 1)
          _stateBorderWidth = 1;
        else
          _stateBorderWidth = value;
      }
    }

    #endregion

    #region Relationship properties

    private int _relationshipDashSize = 5;
    [DisplayName("Dash Size"), Category("(Relationship)")]
    [Description("The lengths of alternating dashes and spaces in dashed lines.")]
    [DefaultValue(5)]
    public int RelationshipDashSize
    {
      get
      {
        return _relationshipDashSize;
      }
      set
      {
        if (value < 1)
          _relationshipDashSize = 1;
        else
          _relationshipDashSize = value;
      }
    }

    [DisplayName("Color"), Category("(Relationship)")]
    [Description("The line color for the relationship.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color RelationshipColor { get; set; } = Color.Black;

    [DisplayName("Font Color"), Category("(Relationship)")]
    [Description("The color of relationship texts.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color RelationshipTextColor { get; set; } = Color.Black;

    private Font _relationshipTextFont = new Font("Tahoma", 8.25F);
    [DisplayName("Font"), Category("(Relationship)")]
    [Description("The font of the type's name.")]
    public Font RelationshipTextFont
    {
      get
      {
        return _relationshipTextFont;
      }
      set
      {
        if (value != null && _relationshipTextFont != value)
        {
          _relationshipTextFont.Dispose();
          _relationshipTextFont = value;
        }
      }
    }

    private int _relationshipWidth = 1;
    [DisplayName("Width"), Category("(Relationship)")]
    [Description("The width of the relationship line.")]
    [DefaultValue(1)]
    public int RelationshipWidth
    {
      get
      {
        return _relationshipWidth;
      }
      set
      {
        if (value < 1)
          _relationshipWidth = 1;
        else
          _relationshipWidth = value;
      }
    }

    #endregion

    #region Transition

    [DisplayName("Color"), Category("(Transition)")]
    [Description("The line color for the Transition.")]
    [DefaultValue(typeof(Color), "Black")]
    public Color TransitionColor { get; set; }

    private int _transitionWidth = 1;
    [DisplayName("Width"), Category("(Transition)")]
    [Description("The width of the Transition line.")]
    [DefaultValue(1)]
    public int TransitionWidth
    {
      get
      {
        return _transitionWidth;
      }
      set
      {
        if (value < 1)
          _transitionWidth = 1;
        else
          _transitionWidth = value;
      }
    }

    #endregion

    public Style Clone()
    {
      Style newStyle = (Style)MemberwiseClone();

      newStyle._nameFont = (Font)NameFont.Clone();
      newStyle._abstractNameFont = (Font)AbstractNameFont.Clone();
      newStyle._identifierFont = (Font)IdentifierFont.Clone();
      newStyle._memberFont = (Font)MemberFont.Clone();
      newStyle._staticMemberFont = (Font)StaticMemberFont.Clone();
      newStyle._abstractMemberFont = (Font)AbstractMemberFont.Clone();
      newStyle._commentFont = (Font)CommentFont.Clone();
      newStyle._relationshipTextFont = (Font)RelationshipTextFont.Clone();

      return newStyle;
    }

    public void Dispose()
    {
      _nameFont.Dispose();
      _abstractNameFont.Dispose();
      _identifierFont.Dispose();
      _memberFont.Dispose();
      _staticMemberFont.Dispose();
      _abstractMemberFont.Dispose();
      _commentFont.Dispose();
      _relationshipTextFont.Dispose();
    }

    private static bool LoadStyles()
    {
      try
      {
        if (Directory.Exists(StylesDirectory))
        {
          string[] files = Directory.GetFiles(StylesDirectory, "*.dst");
          foreach (string file in files)
            Load(file);

          return true;
        }
        else
        {
          return false;
        }
      }
      catch
      {
        return false;
      }
    }

    private static bool LoadCurrentStyle()
    {
      return ((_currentStyle = Load(UserStylePath, false)) != null);
    }

    public static bool SaveCurrentStyle()
    {
      return CurrentStyle.Save(UserStylePath, false);
    }

    public static Style Load(string path)
    {
      return Load(path, true);
    }

    private static Style Load(string path, bool addToList)
    {
      try
      {
        using (Stream stream = new FileStream(path, FileMode.Open))
        {
          BinaryFormatter formatter = new BinaryFormatter();
          Style result = (Style)formatter.Deserialize(stream);

          if (addToList && result != null)
            AddToList(result, path);

          return result;
        }
      }
      catch
      {
        return null;
      }
    }

    [OnDeserialized]
    private void SetFonts(StreamingContext context)
    {
      _abstractNameFont = new Font(_nameFont, _nameFont.Style | FontStyle.Italic);
      _staticMemberFont = new Font(_memberFont, _memberFont.Style | FontStyle.Underline);
      _abstractMemberFont = new Font(_memberFont, _memberFont.Style | FontStyle.Italic);
    }

    public bool Save(string filePath)
    {
      return Save(filePath, true);
    }

    private bool Save(string path, bool addToList)
    {
      try
      {
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
          BinaryFormatter formatter = new BinaryFormatter();
          formatter.Serialize(stream, this);
        }

        if (addToList)
          AddToList(Clone(), path);

        return true;
      }
      catch
      {
        return false;
      }
    }

    private static void AddToList(Style style, string stylePath)
    {
      if (!Styles.ContainsKey(stylePath))
      {
        Styles.Add(stylePath, style);
      }
      else // Replace the old style
      {
        Styles.Remove(stylePath);
        Styles.Add(stylePath, style);
      }
    }

    public override string ToString()
    {
      return Name;
    }
  }
}