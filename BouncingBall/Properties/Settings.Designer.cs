﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BouncingBall.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("localhost")]
        public string sBrokerUrl {
            get {
                return ((string)(this["sBrokerUrl"]));
            }
            set {
                this["sBrokerUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int iCameraId {
            get {
                return ((int)(this["iCameraId"]));
            }
            set {
                this["iCameraId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6000")]
        public int iTimeToLive {
            get {
                return ((int)(this["iTimeToLive"]));
            }
            set {
                this["iTimeToLive"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int iGameTick {
            get {
                return ((int)(this["iGameTick"]));
            }
            set {
                this["iGameTick"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int iBallSpeed {
            get {
                return ((int)(this["iBallSpeed"]));
            }
            set {
                this["iBallSpeed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int iCannyThresholdLow {
            get {
                return ((int)(this["iCannyThresholdLow"]));
            }
            set {
                this["iCannyThresholdLow"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("90")]
        public int iCannyThresholdHight {
            get {
                return ((int)(this["iCannyThresholdHight"]));
            }
            set {
                this["iCannyThresholdHight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>Spiderman</string>
  <string>Batman</string>
  <string>Superman</string>
  <string>Bob Lenon</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection sAvailableIds {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["sAvailableIds"]));
            }
            set {
                this["sAvailableIds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("calibration.txt")]
        public string sCalibrationFile {
            get {
                return ((string)(this["sCalibrationFile"]));
            }
            set {
                this["sCalibrationFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.15")]
        public float fMaxGoalSize {
            get {
                return ((float)(this["fMaxGoalSize"]));
            }
            set {
                this["fMaxGoalSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.05")]
        public float fMinGoalSize {
            get {
                return ((float)(this["fMinGoalSize"]));
            }
            set {
                this["fMinGoalSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("400")]
        public int iRoomWidth {
            get {
                return ((int)(this["iRoomWidth"]));
            }
            set {
                this["iRoomWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("400")]
        public int iRoomHeight {
            get {
                return ((int)(this["iRoomHeight"]));
            }
            set {
                this["iRoomHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public int iMarkerRealLength {
            get {
                return ((int)(this["iMarkerRealLength"]));
            }
            set {
                this["iMarkerRealLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public int iBallDiameter {
            get {
                return ((int)(this["iBallDiameter"]));
            }
            set {
                this["iBallDiameter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool bIsFrontCamera {
            get {
                return ((bool)(this["bIsFrontCamera"]));
            }
            set {
                this["bIsFrontCamera"] = value;
            }
        }
    }
}
