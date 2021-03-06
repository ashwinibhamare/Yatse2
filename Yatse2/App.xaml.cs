﻿// ------------------------------------------------------------------------
//    YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2010  Tolriq (http://yatse.leetzone.org)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Setup;
using Yatse2.Libs;

[assembly: CLSCompliant(false)]
namespace Yatse2
{
    public partial class App
    {

        static Assembly AssemblyLoader(object sender, ResolveEventArgs args)
        {
            var system = Helper.SystemPath + args.Name.Substring(0, args.Name.IndexOf(",",StringComparison.OrdinalIgnoreCase)) + ".dll";
            if (File.Exists(system))
            {
                var myAssembly = Assembly.LoadFrom(system);
                return myAssembly;
            }

            var plugin = Helper.PluginPath + args.Name.Substring(0, args.Name.IndexOf(",", StringComparison.OrdinalIgnoreCase)) + ".dll";
            if (File.Exists(plugin))
            {
                var myAssembly = Assembly.LoadFrom(plugin);
                return myAssembly;
            }
            return null;
        }

        private static void InitLog()
        {
            Logger.Instance().LogFile = Helper.LogPath + "Yatse2.log";
            Logger.Instance().RotateLogFile();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            /*var processes = Process.GetProcessesByName("Yatse2.exe");
            if (processes.Length != 1)
            {
                MessageBox.Show("Yatse2 is already running. Closing.");
                Shutdown();
            }*/ // TODO : Reactivate

            Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                CultureInfo.CurrentCulture.IetfLanguageTag)));

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += AssemblyLoader;

            InitLog();

            base.OnStartup(e);
        }

        static void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Instance().LogException("YatseApp", e.Exception );
            e.Handled = true;
        }
    }
}
