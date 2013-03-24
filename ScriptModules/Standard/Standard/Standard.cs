namespace Standard
{
    using ScriptEngine;
    using System;
    using System.Text;
    using VixenPlus;

    public class Standard : IScriptModule
    {
        private string[] m_imports = new string[0];
        private string[] m_references = new string[0];

        public string GenerateSequenceCode(EventSequence sequence)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("internal static Standard.Context ScriptContext = null;");
            builder.AppendLine("public Vixen.HardwareUpdateDelegate HardwareUpdate {");
            builder.AppendLine("set { ScriptContext.HardwareUpdate = value; }");
            builder.AppendLine("}");
            builder.AppendLine("public Vixen.EventSequence Sequence {");
            builder.AppendLine("set { ScriptContext.Sequence = value; }");
            builder.AppendLine("}");
            builder.AppendLine("public void ScriptStarting() {");
            builder.AppendLine("ScriptContext = new Standard.Context();");
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
            builder.AppendLine("get { return ScriptContext.State != Context.RunState.Stopped; }");
            builder.AppendLine("}");
            builder.AppendLine("public void Dispose() {");
            builder.AppendLine("GC.SuppressFinalize(this);");
            builder.AppendLine("}");
            builder.AppendLine("internal static IChannelEnumerable All {");
            builder.AppendLine("get { return ScriptContext.m_channels; }");
            builder.AppendLine("}");
            int num = 0;
            foreach (Channel channel in sequence.Channels)
            {
                builder.AppendFormat("internal static IChannelEnumerable {0} {{\r\n", channel.Name);
                builder.AppendFormat("get {{ return ScriptContext.m_channels[{0}]; }}\r\n", num++);
                builder.AppendLine("}");
            }
            builder.AppendLine("internal ChannelCollection ChannelRange(IChannelEnumerable startChannel, IChannelEnumerable endChannel) {");
            builder.AppendLine("return ScriptContext.m_channels.ChannelRange(startChannel, endChannel);");
            builder.AppendLine("}");
            builder.AppendLine("internal ChannelCollection ChannelRange(IChannelEnumerable startChannel, int count) {");
            builder.AppendLine("return ScriptContext.m_channels.ChannelRange(startChannel, count);");
            builder.AppendLine("}");
            builder.AppendLine("internal ChannelCollection ChannelRange(int start, int count) {");
            builder.AppendLine("return ScriptContext.m_channels.ChannelRange(start, count);");
            builder.AppendLine("}");
            builder.AppendLine("internal ChannelCollection Channels(params IChannelEnumerable[] channels) {");
            builder.AppendLine("ChannelCollection collection = new ChannelCollection();");
            builder.AppendLine("collection.Add(channels);");
            builder.AppendLine("return collection;");
            builder.AppendLine("}");
            builder.AppendLine("internal uint At(int level) {");
            builder.AppendLine("return new Standard.At(level).TypeValue;");
            builder.AppendLine("}");
            builder.AppendLine("internal ITimeModifier For(int time) {");
            builder.AppendLine("return new Standard.For(time);");
            builder.AppendLine("}");
            builder.AppendLine("internal ITimeModifier Over(int time) {");
            builder.AppendLine("return new Standard.Over(time);");
            builder.AppendLine("}");
            builder.AppendLine("internal uint Wait {");
            builder.AppendLine("get { return new Standard.Wait().TypeValue; }");
            builder.AppendLine("}");
            builder.AppendLine("internal ITimeModifier Every(int time) {");
            builder.AppendLine("return new Standard.Every(time);");
            builder.AppendLine("}");
            builder.AppendLine("internal void On(IChannelEnumerable channels, params uint[] modifiers) {");
            builder.AppendLine("Standard.Actions.On(ScriptContext, channels, modifiers);");
            builder.AppendLine("}");
            builder.AppendLine("internal void Off(IChannelEnumerable channels, params uint[] modifiers) {");
            builder.AppendLine("Standard.Actions.Off(ScriptContext, channels, modifiers);");
            builder.AppendLine("}");
            builder.AppendLine("internal void Ramp(IChannelEnumerable channels, int startIntensity, int endIntensity, params uint[] modifiers) {");
            builder.AppendLine("Standard.Actions.Ramp(ScriptContext, channels, startIntensity, endIntensity, modifiers);");
            builder.AppendLine("}");
            builder.AppendLine("internal void Random(IChannelEnumerable channels, int saturationLevel, params uint[] modifiers) {");
            builder.AppendLine("Standard.Actions.Random(ScriptContext, channels, saturationLevel, modifiers);");
            builder.AppendLine("}");
            builder.AppendLine("internal void Chase(IChannelEnumerable channels, params uint[] modifiers) {");
            builder.AppendLine("Standard.Actions.Chase(ScriptContext, channels, modifiers);");
            builder.AppendLine("}");
            builder.AppendLine("internal void WaitOnTimers() {");
            builder.AppendLine("Standard.Actions.WaitOnTimers(ScriptContext);");
            builder.AppendLine("}");
            builder.AppendLine("internal int EventPeriod {");
            builder.AppendLine("get { return ScriptContext.m_eventPeriodLength; }");
            builder.AppendLine("set { ScriptContext.m_eventPeriodLength = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal bool ClearAfterCommand {");
            builder.AppendLine("get { return ScriptContext.m_clearAfterCommand; }");
            builder.AppendLine("set { ScriptContext.m_clearAfterCommand = value; }");
            builder.AppendLine("}");
            builder.AppendLine("internal void BeginUpdate() {");
            builder.AppendLine("ScriptContext.BeginUpdate();");
            builder.AppendLine("}");
            builder.AppendLine("internal void EndUpdate() {");
            builder.AppendLine("ScriptContext.EndUpdate();");
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

