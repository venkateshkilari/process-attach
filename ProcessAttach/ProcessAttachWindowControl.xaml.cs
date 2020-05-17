namespace ProcessAttach
{
    using EnvDTE;
    using Microsoft.Web.Administration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ProcessAttachWindowControl.
    /// </summary>
    public partial class ProcessAttachWindowControl : UserControl
    {
        public EnvDTE.Process process;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessAttachWindowControl"/> class.
        /// </summary>
        public ProcessAttachWindowControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
       
            var solutionBuild = ProcessAttachPackage.DTE.Solution.SolutionBuild;
            var startupProjects = (object[])solutionBuild.StartupProjects;
            foreach(var project in startupProjects) {
                string projName = project.ToString().Substring(0, project.ToString().LastIndexOf("."));
                ProcessAttachPackage.DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate();
                ProcessAttachPackage.DTE.ToolWindows.SolutionExplorer.GetItem(projName).Select(EnvDTE.vsUISelectionType.vsUISelectionTypeSelect);
                ProcessAttachPackage.DTE.ExecuteCommand("Debug.StartWithoutDebugging");
            }

            var Processes = ProcessAttachPackage.DTE.Debugger.LocalProcesses;
           
                MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Started"),
                "ProcessAttachWindow");
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            StringBuilder stringBuilder = new StringBuilder();
            var Processes = ProcessAttachPackage.DTE.Debugger.LocalProcesses;
            foreach (EnvDTE.Process processInfo in Processes)
            {
                if (processInfo.Name.Contains("iisexpress"))
                {
                    stringBuilder.Append($"ProcessName:{processInfo.Name},ProcessId:{processInfo.ProcessID}");                   
                     processInfo.Attach();
                }
            }
            MessageBox.Show(
            string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", stringBuilder.ToString()),
            "ProcessAttachWindow");
       
        }
    }
}