#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight.Controls.WPF
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="ThemeManager.cs" company="Tethys">
// Copyright  2015 by T. Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft .Net Framework 4.5. 
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.Controls.WPF
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Themes of the modern UI.
    /// </summary>
    public enum ModernUiTheme
    {
        /// <summary>
        /// No specific theme.
        /// </summary>
        None = 0,

        /// <summary>
        /// The light theme.
        /// </summary>
        Light = 1,

        /// <summary>
        /// The dark theme.
        /// </summary>
        Dark = 2,
    } // ModernUiTheme

    /// <summary>
    /// Class to manage WPF themes.
    /// </summary>
    public class ThemeManager : DependencyObject
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The current (= one and only) <see cref="ThemeManager"/>.
        /// </summary>
        private static ThemeManager current;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// The resource key for the accent color.
        /// </summary>
        public const string KeyAccentColor = "AccentColor";

        /// <summary>
        /// The location of the dark theme resource dictionary.
        /// </summary>
        public static readonly Uri DarkThemeSource = new Uri(
            "/Tethys.Silverlight.Controls.WPF;component/Themes/Theme.Dark.xaml",
            UriKind.Relative);
        
        /// <summary>
        /// The location of the light theme resource dictionary.
        /// </summary>
        public static readonly Uri LightThemeSource = new Uri(
            "/Tethys.Silverlight.Controls.WPF;component/Themes/Theme.Light.xaml",
            UriKind.Relative);

        /// <summary>
        /// The default accent color (a dark green)
        /// </summary>
        public static readonly Color DefaultAccentColor 
            = Color.FromArgb(0xff, 0x00, 0xd0, 0x00);

        /// <summary>
        /// Gets the current <see cref="ThemeManager"/> instance.
        /// </summary>
        public static ThemeManager Current
        {
            get
            {
                return current ?? (current = new ThemeManager());
            }
        }

        /// <summary>
        /// This event is raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the current theme source.
        /// </summary>
        public Uri ThemeSource
        {
            get
            {
                return GetThemeSource();
            }

            set
            {
                this.SetThemeSource(value);
            }
        }

        /// <summary>
        /// Gets or sets the accent color.
        /// </summary>
        public Color AccentColor
        {
            get
            {
                return GetAccentColor();
            }

            set
            {
                this.SetAccentColor(value);
            }
        }

        /// <summary>
        /// Gets the light theme command.
        /// </summary>
        public ICommand LightThemeCommand { get; private set; }

        /// <summary>
        /// Gets the dark theme command.
        /// </summary>
        public ICommand DarkThemeCommand { get; private set; }

        /// <summary>
        /// Gets the accent color command.
        /// </summary>
        public ICommand AccentColorCommand { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Prevents a default instance of the <see cref="ThemeManager"/> class from being created.
        /// </summary>
        private ThemeManager()
        {
            this.LightThemeCommand = new DelegateCommand(x => this.SetLightTheme());
            this.DarkThemeCommand = new DelegateCommand(x => this.SetDarkTheme());
            this.AccentColorCommand = new DelegateCommand(
                x =>
                {
                    var nullableColor = x as Color?;
                    if (nullableColor != null)
                    {
                        this.SetAccentColor(nullableColor.Value);
                    }
                    else
                    {
                        var str = x as string;
                        var color = DefaultAccentColor;
                        if (str != null)
                        {
                            var convertFromString = ColorConverter.ConvertFromString(str);
                            if (convertFromString != null)
                            {
                                color = (Color)convertFromString;
                            } // if
                        } // if

                        this.SetAccentColor(color);
                    } // if
                });
        } // ThemeManager()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region DEPENDENCY PROPERTIES
        #region Attached DP - GlobalTheme
        /// <summary>
        /// Attached property that can be used to define the global theme.
        /// </summary>
        public static readonly DependencyProperty GlobalThemeProperty =
            DependencyProperty.RegisterAttached("GlobalTheme",
            typeof(ModernUiTheme), typeof(ThemeManager),
            new UIPropertyMetadata(ModernUiTheme.Light, OnGlobalThemeChanged));

        /// <summary>
        /// Gets the global theme.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <returns>The <see cref="ModernUiTheme"/>.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ModernUiTheme GetGlobalTheme(DependencyObject obj)
        {
            return (ModernUiTheme)obj.GetValue(GlobalThemeProperty);
        } // GetGlobalTheme()

        /// <summary>
        /// Sets the global theme.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetGlobalTheme(DependencyObject obj, ModernUiTheme value)
        {
            obj.SetValue(GlobalThemeProperty, value);
        } // SetGlobalTheme()
        #endregion

        #region Attached DP - GlobalAccentColor
        /// <summary>
        /// Attached property that can be used to define the global accent color.
        /// </summary>
        public static readonly DependencyProperty GlobalAccentColorProperty =
            DependencyProperty.RegisterAttached("GlobalAccentColor",
            typeof(Color), typeof(ThemeManager),
            new UIPropertyMetadata(DefaultAccentColor, OnGlobalAccentColorChanged));

        /// <summary>
        /// Gets the global accent color.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <returns>A <see cref="Color"/> value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Color GetGlobalAccentColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(GlobalAccentColorProperty);
        } // GetGlobalAccentColor()

        /// <summary>
        /// Sets the global accent color.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetGlobalAccentColor(DependencyObject obj, Color value)
        {
            obj.SetValue(GlobalAccentColorProperty, value);
        } // SetGlobalAccentColor()
        #endregion

        #region Attached DP - LocalTheme
        /// <summary>
        /// Attached property that can be used to define a local theme for a control.
        /// </summary>
        public static readonly DependencyProperty LocalThemeProperty =
            DependencyProperty.RegisterAttached("LocalTheme",
            typeof(ModernUiTheme), typeof(ThemeManager),
            new UIPropertyMetadata(ModernUiTheme.None, OnLocalThemeChanged));

        /// <summary>
        /// Gets the local theme.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <returns>The <see cref="ModernUiTheme"/>.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ModernUiTheme GetLocalTheme(DependencyObject obj)
        {
            return (ModernUiTheme)obj.GetValue(LocalThemeProperty);
        } // GetLocalTheme()

        /// <summary>
        /// Sets the local theme.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetLocalTheme(DependencyObject obj, ModernUiTheme value)
        {
            obj.SetValue(LocalThemeProperty, value);
        } // SetLocalTheme()
        #endregion
        #endregion // DEPENDENCY PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Gets the accent color.
        /// </summary>
        /// <returns>The current accent <see cref="Color"/>.</returns>
        public static Color GetAccentColor()
        {
            var accentColor = Application.Current.Resources[KeyAccentColor] as Color?;

            if (accentColor.HasValue)
            {
                return accentColor.Value;
            } // if

            // return the default color
            return DefaultAccentColor;
        } // GetAccentColor()

        /// <summary>
        /// Sets the accent color.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetAccentColor(Color value)
        {
            SetAccentColorInternal(value);

            this.OnPropertyChanged("AccentColor");
        } // SetAccentColor()

        /// <summary>
        /// Sets the dark theme.
        /// </summary>
        public void SetDarkTheme()
        {
            this.SetThemeSource(DarkThemeSource);

            this.OnPropertyChanged("ThemeSource");
        } // SetDarkTheme()

        /// <summary>
        /// Sets the light theme.
        /// </summary>
        public void SetLightTheme()
        {
            this.SetThemeSource(LightThemeSource);

            this.OnPropertyChanged("ThemeSource");
        } // SetLightTheme()

        /// <summary>
        /// Sets the theme.
        /// </summary>
        /// <param name="theme">The theme.</param>
        public void SetTheme(ModernUiTheme theme)
        {
            this.SetThemeSource(GetThemeUri(theme));
        } // SetTheme()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS         
        /// <summary>
        /// Called when the global theme has changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnGlobalThemeChanged(DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            if (obj is FrameworkElement)
            {
                var newtheme = GetGlobalTheme(obj);
                Current.SetTheme(newtheme);
            } // if
        } // OnGlobalThemeChanged()

        /// <summary>
        /// Called when the global accent color has changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnGlobalAccentColorChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            if (obj is FrameworkElement)
            {
                Current.SetAccentColor(GetGlobalAccentColor(obj));
            } // if
        } // OnGlobalAccentColorChanged()

        /// <summary>
        /// Called when the local theme has changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void OnLocalThemeChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            if (obj is FrameworkElement)
            {
                var newtheme = GetLocalTheme(obj);
                ApplyLocalTheme(obj as FrameworkElement, GetThemeUri(newtheme));
            } // if
        } // OnGlobalThemeChanged()

        /// <summary>
        /// Gets the theme URI.
        /// </summary>
        /// <param name="theme">The theme.</param>
        /// <returns>The theme <see cref="Uri"/>.</returns>
        private static Uri GetThemeUri(ModernUiTheme theme)
        {
            if (theme == ModernUiTheme.Dark)
            {
                return DarkThemeSource;
            } // if

            return LightThemeSource;
        } // GetThemeUri()

        /// <summary>
        /// Applies a theme locally for a <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="source">The source.</param>
        private static void ApplyLocalTheme(FrameworkElement element, Uri source)
        {
            if (element == null)
            {
                return;
            } // if

            try
            {
                if (source != null)
                {
                    var themeDictionary = new ThemeResourceDictionary();
                    themeDictionary.Source = source;

                    // add the new dictionary to the collection of merged 
                    // dictionaries of the target object  
                    element.Resources.MergedDictionaries.Insert(0, themeDictionary);

                    // find if the target element already has a theme applied  
                    var existingDictionaries = (from dictionary in element.Resources
                        .MergedDictionaries.OfType<ThemeResourceDictionary>()  
                        select dictionary).ToList();  
  
                    // remove the existing dictionaries  
                    foreach (var dictionary in existingDictionaries)
                    {
                        if (themeDictionary == dictionary)
                        {
                            // don't remove the newly added dictionary  
                            continue;
                        } // if

                        element.Resources.MergedDictionaries.Remove(dictionary);
                    } // foreach
                } // if
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            } // catch
        } // ApplyLocalTheme()

        /// <summary>
        /// Determine the current theme by looking at the app resources and returns
        /// the first dictionary that has the type <see cref="ThemeResourceDictionary"/>.
        /// </summary>
        /// <returns>The theme <see cref="ResourceDictionary"/>.</returns>
        private static ResourceDictionary GetThemeDictionary()
        {
            return GetThemeDictionary(Application.Current.Resources);
        } // GetThemeDictionary()

        /// <summary>
        /// Determine the current theme by looking for a 
        /// <see cref="ResourceDictionary"/> of type <see cref="ThemeResourceDictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>
        /// The theme <see cref="ResourceDictionary" />.
        /// </returns>
        private static ResourceDictionary GetThemeDictionary(ResourceDictionary dictionary)
        {
            foreach (var resourceDictionary in dictionary.MergedDictionaries)
            {
                if (resourceDictionary is ThemeResourceDictionary)
                {
                    return resourceDictionary;
                } // if

                return GetThemeDictionary(resourceDictionary);
            } // foreach

            return null;
        } // GetThemeDictionary()

        /// <summary>
        /// Gets the theme source.
        /// </summary>
        /// <returns>A <see cref="Uri"/>.</returns>
        private static Uri GetThemeSource()
        {
            var dict = GetThemeDictionary();
            if (dict != null)
            {
                return dict.Source;
            } // if

            // could not determine the theme dictionary
            return null;
        } // GetThemeSource()

        /// <summary>
        /// Sets the theme source.
        /// </summary>
        /// <param name="source">The source.</param>
        private void SetThemeSource(Uri source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            } // if

            var oldThemeDict = GetThemeDictionary();
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var themeDict = new ResourceDictionary { Source = source };

            // add new before removing old theme to avoid 'dynamicresource not found' warnings
            dictionaries.Add(themeDict);

            // remove old theme
            if (oldThemeDict != null)
            {
                dictionaries.Remove(oldThemeDict);
            } // if

            this.OnPropertyChanged("ThemeSource");
        } // SetThemeSource()

        /// <summary>
        /// Sets the accent color.
        /// </summary>
        /// <param name="accentColor">Color of the accent.</param>
        private static void SetAccentColorInternal(Color accentColor)
        {
            Application.Current.Resources[KeyAccentColor] = accentColor;
        } // SetAccentColor()

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has
        /// changed.</param>
        /// <remarks>Exists due to legacy reasons.</remarks>
        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            } // if
        } // OnPropertyChanged()
        #endregion // PRIVATE METHODS
    } // ThemeManager
} // Tethys.Silverlight.Controls.WPF
