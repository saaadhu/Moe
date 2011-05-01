using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using EnvDTE80;

namespace SenthilKumarSelvaraj.Moe
{
    static class TracepointManager
    {
        static DTE2 dte;
        public static void Initialize(DTE2 _dte)
        {
            dte = _dte;    
        }

        public static void SetupAllTracepoints()
        {
            RemoveAllExistingMoeTracepoints();
            var tracepointsFilePath = GetTracepointsFilePath();
            Import(tracepointsFilePath);
        }

        static string GetTracepointsFilePath()
        {
            var currentPath = Assembly.GetExecutingAssembly().Location;
            const string fileName = "MoeTracePoints.xml";
            return Path.Combine(Path.GetDirectoryName(currentPath), fileName);
        }

        static void Import(string xmlFilePath)
        {
            var dom = XDocument.Load(xmlFilePath);
            var breakpoints = dom.Document.Descendants("Breakpoint");

            foreach (var breakpoint in breakpoints)
            {
                ImportBreakpoint(breakpoint);
            }
        }

        const string moeBreakpointTag = "MOE";
        static void ImportBreakpoint(XElement breakpointElement)
        {
            var textPosition = breakpointElement.Element("TextPosition");
            var fileName = Path.GetFileName(textPosition.Element("FileName").Value);
            var startline = textPosition.Element("startLine").Value;
            var text = breakpointElement.Element("TracepointText").Value;

            dte.Debugger.Breakpoints.Add(File: fileName, Line: int.Parse(startline) + 1); // The exported line number appears to be zero based.
            var lastBreakpoint = dte.Debugger.Breakpoints.Cast<Breakpoint2>().Last();
            lastBreakpoint.Message = text;
            lastBreakpoint.BreakWhenHit = false;
            lastBreakpoint.Tag = moeBreakpointTag;
        }

        private static void RemoveAllExistingMoeTracepoints()
        {
            var cachedBreakpoints = dte.Debugger.Breakpoints.Cast<Breakpoint2>().ToArray();

            foreach (var breakpoint in cachedBreakpoints)
            {
                if (breakpoint.Tag == moeBreakpointTag)
                    breakpoint.Delete();
            }
        }

    }
}
