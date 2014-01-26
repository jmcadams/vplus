using System;
using System.Globalization;
using System.Linq;
using System.Xml;

public abstract class InputPlugin : IInputPlugin {
    public enum MappingIterator {
        None,
        SingleInput,
        MultiInput
    }

    //private const string AttributeId = "id";
    //private const string AttributeLiveUpdate = "liveUpdate";
    //private const string AttributeMappingId = "mappingId";
    //private const string AttributeName = "name";
    //private const string AttributeRecord = "record";
    //private const string AttributeType = "type";
    //private const string ElementInput = "Input";
    //private const string ElementInputs = "Inputs";
    //private const string ElementIterator = "Iterator";
    //private const string ElementMappingSets = "MappingSets";
    //private const int InvalidId = 0;
    private XmlNode _setupNode;


    // ReSharper disable PublicConstructorInAbstractClass
    public InputPlugin() {
        // ReSharper restore PublicConstructorInAbstractClass
        MappingIteratorType = MappingIterator.None;
        MappingSets = new MappingSets();
    }


    public MappingIterator MappingIteratorType { get; set; }

    internal MappingSets MappingSets { get; set; }

    public Input SingleIterator { get; set; }

    public abstract void Initialize(SetupData setupData, XmlNode setupNode);

    public virtual void Setup() {}

    public virtual void Shutdown() {}

    public virtual void Startup() {}

    public abstract string Author { get; }

    public abstract string Description { get; }

    public abstract HardwareMap[] HardwareMap { get; }

    public abstract Input[] Inputs { get; }

    public bool LiveUpdate { get; set; }

    public abstract string Name { get; }

    public bool Record { get; set; }


    private Input FindInput(ulong id) {
        return id == 0L ? null : Array.Find(Inputs, i => i.Id == id);
    }


    public Input[] GetIterators() {
        return Inputs.Where(input => input.IsMappingIterator).ToArray();
    }


    internal void InitializeInternal(SetupData setupData, XmlNode setupNode) {
        _setupNode = setupNode;
        Initialize(setupData, setupNode);
    }


    internal void IteratorTriggered(Input input) {
        if (MappingIteratorType == MappingIterator.SingleInput) {
            if (input == SingleIterator) {
                MappingSets.StepMapping();
            }
        }
        else {
            MappingSets.CurrentMappingSet = input.AssignedMappingSet;
        }
    }


    internal void PluginToSetupData() {
        MappingSets.WriteData(Xml.GetEmptyNodeAlways(_setupNode, "MappingSets"));
        var emptyNodeAlways = Xml.GetEmptyNodeAlways(_setupNode, "Inputs");
        foreach (var input in Inputs) {
            input.WriteData(emptyNodeAlways);
        }
        var node = Xml.GetEmptyNodeAlways(_setupNode, "Iterator");
        Xml.SetAttribute(node, "type", MappingIteratorType.ToString());
        var iterators = GetIterators();
        if (iterators.Length > 0) {
            if (MappingIteratorType == MappingIterator.SingleInput) {
                Xml.SetAttribute(node, "Input", "id", SingleIterator.Id.ToString(CultureInfo.InvariantCulture));
            }
            else {
                foreach (var input in iterators) {
                    var node2 = Xml.SetNewValue(node, "Input", "");
                    Xml.SetAttribute(node2, "id", input.Id.ToString(CultureInfo.InvariantCulture));
                    Xml.SetAttribute(node2, "mappingId",
                        (input.AssignedMappingSet != null)
                            ? input.AssignedMappingSet.Id.ToString(CultureInfo.InvariantCulture)
                            : 0.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
        Xml.SetAttribute(_setupNode, "liveUpdate", LiveUpdate.ToString());
        Xml.SetAttribute(_setupNode, "record", Record.ToString());
    }


    internal void SetupDataToPlugin() {
        XmlNode node = _setupNode["MappingSets"];
        if (node != null) {
            MappingSets.ReadData(node);
        }
        XmlNode node2 = _setupNode["Inputs"];
        if (node2 != null) {
            foreach (var input in Inputs) {
                var node3 = node2.SelectSingleNode(string.Format("{0}[@{1}=\"{2}\"]", "Input", "name", input.Name));
                if (node3 != null) {
                    input.ReadData(node3);
                }
            }
        }
        XmlNode node4 = _setupNode["Iterator"];
        if (node4 != null) {
            if (node4.Attributes != null) {
                MappingIteratorType = (MappingIterator) Enum.Parse(typeof (MappingIterator), node4.Attributes["type"].Value);
            }
        }
        else {
            MappingIteratorType = MappingIterator.None;
        }
        SingleIterator = null;
        if (node4 != null) {
            switch (MappingIteratorType) {
                case MappingIterator.SingleInput: {
                    XmlNode node5 = node4["Input"];
                    if (node5 != null) {
                        if (node5.Attributes != null) {
                            SingleIterator = FindInput(ulong.Parse(node5.Attributes["id"].Value));
                        }
                    }
                }
                    break;
                case MappingIterator.MultiInput: {
                    var inputNodes = node4.SelectNodes("Input");
                    if (inputNodes != null) {
                        foreach (XmlNode node5 in inputNodes) {
                            if (node5.Attributes == null) {
                                continue;
                            }
                            var input2 = FindInput(ulong.Parse(node5.Attributes["id"].Value));
                            var set = MappingSets.FindMappingSet(ulong.Parse(node5.Attributes["mappingId"].Value));
                            if (input2 != null) {
                                input2.AssignedMappingSet = set;
                            }
                        }
                    }
                }
                    break;
            }
        }
        if (_setupNode.Attributes != null && _setupNode.Attributes["liveUpdate"] != null) {
            LiveUpdate = bool.Parse(_setupNode.Attributes["liveUpdate"].Value);
        }
        if (_setupNode.Attributes != null && _setupNode.Attributes["record"] != null) {
            Record = bool.Parse(_setupNode.Attributes["record"].Value);
        }
    }


    internal void ShutdownInternal() {
        Shutdown();
    }


    internal void StartupInternal() {
        Startup();
    }
}