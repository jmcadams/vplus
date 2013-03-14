namespace LedTriks
{
    using ScriptEngine;
    using System;
    using System.Text;
    using Vixen;

    public class LedTriks : IScriptModule
    {
        private string[] m_imports = new string[] { "LedTriksUtil", "System.Drawing" };
        private string[] m_references = new string[] { @"\Plugins\Output\LedTriksUtil.dll", "System.Drawing.dll" };

        public string GenerateSequenceCode(EventSequence sequence)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("internal LedTriks.Context ScriptContext = null;");
            builder.AppendLine("public Vixen.HardwareUpdateDelegate HardwareUpdate {");
            builder.AppendLine("set {  }");
            builder.AppendLine("}");
            builder.AppendLine("public Vixen.EventSequence Sequence {");
            builder.AppendLine("set { ScriptContext.Sequence = value; }");
            builder.AppendLine("}");
            builder.AppendLine("public void ScriptStarting() {");
            builder.AppendLine("ScriptContext = new LedTriks.Context();");
            builder.AppendLine("}");
            builder.AppendLine("public void ScriptEnded() {");
            builder.AppendLine("ScriptContext.Stop();");
            builder.AppendLine("ScriptContext.Dispose();");
            builder.AppendLine("ScriptContext = null;");
            builder.AppendLine("}");
            builder.AppendLine("public void ForceStop() {");
            builder.AppendLine("ScriptContext.Stop();");
            builder.AppendLine("while(ScriptContext.State != Context.RunState.Stopped) System.Threading.Thread.Sleep(10);");
            builder.AppendLine("}");
            builder.AppendLine("public bool Running {");
            builder.AppendLine("get { return ScriptContext != null && ScriptContext.State != Context.RunState.Stopped; }");
            builder.AppendLine("}");
            builder.AppendLine("public void Dispose() {");
            builder.AppendLine("GC.SuppressFinalize(this);");
            builder.AppendLine("}");
            builder.AppendLine("internal string FontName {");
            builder.AppendLine("    set { ScriptContext.FontName = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal FontStyle FontStyle {");
            builder.AppendLine("    set { ScriptContext.FontStyle = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal int FrameLength {");
            builder.AppendLine("    set { ScriptContext.FrameLength = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal int TextHeight {");
            builder.AppendLine("    set { ScriptContext.TextHeight = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.VertPosition TextVertPosition {");
            builder.AppendLine("    set { ScriptContext.TextVertPosition = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.HorzPosition TextHorzPosition {");
            builder.AppendLine("    set { ScriptContext.TextHorzPosition = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal int TextVertPositionValue {");
            builder.AppendLine("    set { ScriptContext.TextVertPositionValue = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal int TextHorzPositionValue {");
            builder.AppendLine("    set { ScriptContext.TextHorzPositionValue = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal Size BoardLayout {");
            builder.AppendLine("    set { ");
            builder.AppendLine("    ScriptContext.BoardLayout = value;");
            builder.AppendLine("    }");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.ScrollDirection TextScrollDirection {");
            builder.AppendLine("    set { ScriptContext.TextScrollDirection = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.ScrollExtent TextScrollExtent {");
            builder.AppendLine("    set { ScriptContext.TextScrollExtent = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal bool IgnoreFontDescent {");
            builder.AppendLine("    set { ScriptContext.IgnoreFontDescent = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.Frame[] GenerateText(string text) {");
            builder.AppendLine("    return Actions.GenerateText(ScriptContext, text);");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.Frame[] GenerateText(string text, TimeSpan staticTextTimeLength) {");
            builder.AppendLine("    return Actions.GenerateText(ScriptContext, text, staticTextTimeLength);");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.Frame[] MergeFrames(LedTriksUtil.Frame[] sourceFrames, LedTriksUtil.Frame[] destFrames) {");
            builder.AppendLine("    return LedTriks.Actions.MergeFrames(sourceFrames, destFrames);");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.Frame[] MergeFrames(LedTriksUtil.Frame[] sourceFrames, LedTriksUtil.Frame[] destFrames, int count) {");
            builder.AppendLine("    return LedTriks.Actions.MergeFrames(sourceFrames, destFrames, count);");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.Frame[] MergeFrames(LedTriksUtil.Frame[] sourceFrames, int sourceIndex, LedTriksUtil.Frame[] destFrames, int destIndex, int count) {");
            builder.AppendLine("    return LedTriks.Actions.MergeFrames(sourceFrames, sourceIndex, destFrames, destIndex, count);");
            builder.AppendLine("}");
            builder.AppendLine("internal void OutputFrames(LedTriksUtil.Frame[] frames) {");
            builder.AppendLine("    LedTriks.Actions.OutputFrames(ScriptContext, frames);");
            builder.AppendLine("}");
            builder.AppendLine("internal void GenerateTextAndOutput(string text) {");
            builder.AppendLine("    LedTriks.Actions.GenerateTextAndOutput(ScriptContext, text);");
            builder.AppendLine("}");
            builder.AppendLine("internal void GenerateTextAndOutput(string text, TimeSpan staticTextTimeLength) {");
            builder.AppendLine("    LedTriks.Actions.GenerateTextAndOutput(ScriptContext, text, staticTextTimeLength);");
            builder.AppendLine("}");
            builder.AppendLine("internal void WaitForEnd() {");
            builder.AppendLine("    LedTriks.Actions.WaitForEnd(ScriptContext);");
            builder.AppendLine("}");
            builder.AppendLine("internal LedTriksUtil.Frame[] LoadFramesFromSequence(string sequenceFileName) {");
            builder.AppendLine("    return LedTriks.Actions.LoadFramesFromSequence(sequenceFileName);");
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string[] Imports
        {
            get
            {
                return this.m_imports;
            }
        }

        public string[] References
        {
            get
            {
                return this.m_references;
            }
        }
    }
}

