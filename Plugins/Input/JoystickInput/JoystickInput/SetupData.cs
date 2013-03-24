namespace JoystickInput
{
    using System;
    using System.Xml;
    using VixenPlus;

    internal static class SetupData
    {
        private const string ATTR_IS_ITERATOR = "isIterator";
        private const string ATTR_NAME = "name";
        private const string DIGITAL_POV = "DigitalPOV";
        private const string ELEMENT_INPUT = "Input";
        private const string ELEMENT_INPUTS = "Inputs";
        private static XmlNode m_inputsNode;
        private static XmlNode m_setupNode;

        public static bool GetIsIterator(string inputName)
        {
            XmlNode node = m_inputsNode.SelectSingleNode(string.Format("{0}[@{1}=\"{2}\"]", "Input", "name", inputName));
            return ((node != null) && bool.Parse(node.Attributes["isIterator"].Value));
        }

        public static void LoadFrom(XmlNode setupNode)
        {
            m_setupNode = setupNode;
            m_inputsNode = Xml.GetNodeAlways(m_setupNode, "Inputs");
        }

        public static void SetIsIterator(string inputName, bool value)
        {
            XmlNode node = m_inputsNode.SelectSingleNode(string.Format("{0}[@{1}=\"{2}\"]", "Input", "name", inputName));
            if (node == null)
            {
                node = Xml.SetNewValue(m_inputsNode, "Input", "");
            }
            Xml.SetAttribute(node, "name", inputName);
            Xml.SetAttribute(node, "isIterator", value.ToString());
        }

        public static bool IsQuadDigitalPOV
        {
            get
            {
                return bool.Parse(Xml.GetNodeAlways(m_setupNode, "DigitalPOV", "True").InnerText);
            }
            set
            {
                Xml.SetValue(m_setupNode, "DigitalPOV", value ? bool.TrueString : bool.FalseString);
            }
        }
    }
}

