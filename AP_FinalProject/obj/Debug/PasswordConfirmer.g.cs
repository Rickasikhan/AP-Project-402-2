﻿#pragma checksum "..\..\PasswordConfirmer.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "90163AD20B6365A5A6C9F1C0D0E7DA0941B9AE9CD0FE0F414081C67EBE552884"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AP_FinalProject;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AP_FinalProject {
    
    
    /// <summary>
    /// PasswordConfirmer
    /// </summary>
    public partial class PasswordConfirmer : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 101 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox FirstPassBox;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox SecondPassBox;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ProceedButton;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PasswordMatchTB;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame ContentFrame;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PassRepeatTB;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\PasswordConfirmer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PassRegexTB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AP_FinalProject;component/passwordconfirmer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PasswordConfirmer.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.FirstPassBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 2:
            this.SecondPassBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            this.ProceedButton = ((System.Windows.Controls.Button)(target));
            
            #line 103 "..\..\PasswordConfirmer.xaml"
            this.ProceedButton.Click += new System.Windows.RoutedEventHandler(this.ProceedButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PasswordMatchTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.ContentFrame = ((System.Windows.Controls.Frame)(target));
            
            #line 105 "..\..\PasswordConfirmer.xaml"
            this.ContentFrame.Navigated += new System.Windows.Navigation.NavigatedEventHandler(this.ContentFrame_Navigated);
            
            #line default
            #line hidden
            return;
            case 6:
            this.PassRepeatTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.PassRegexTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

