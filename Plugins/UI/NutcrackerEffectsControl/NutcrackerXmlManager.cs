
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using VixenPlus;

namespace NutcrackerEffectsControl {
    public class NutcrackerXmlManager {

        private readonly XElement _doc;


        public NutcrackerXmlManager() {
            if (File.Exists(Paths.NutcrackerDataFile)) {
                _doc = XElement.Load(Paths.NutcrackerDataFile);
            }
            else {
                _doc = new XElement("xrgb",
                                    new XElement("models"),
                                    new XElement("effects"),
                                    new XElement("palettes",
                                                 new XElement("palette",
                                                              new XAttribute("name", "default"),
                                                              new XAttribute("color1", "#FF0000"),
                                                              new XAttribute("color2", "#00FF00"),
                                                              new XAttribute("color3", "#0000FF"),
                                                              new XAttribute("color4", "#FFFF00"),
                                                              new XAttribute("color5", "#FFFFFF"),
                                                              new XAttribute("color6", "#000000")
                                                     )
                                        )
                    );
            }
        }


        public Color[] GetColors(string paletteName) {
            var palette = GetPaletteByName(paletteName);

            var colors = new Color[6];

            for (var i = 0; i < 6; i++) {
                var attr = "color" + (i + 1);
                var colorAttr = palette.Attribute(attr);
                if (colorAttr != null) {
                    colors[i] = ColorTranslator.FromHtml(colorAttr.Value);
                }
                else {
                    colors[i] = Color.White;
                }
            }

            return colors;
        }


        private XElement GetPaletteByName(string paletteName) {
            var palette = _doc.Descendants("palettes").Descendants("palette").Where(e => {
                var attr = e.Attribute("name");
                return attr != null && attr.Value == paletteName;
            }).First();
            return palette;
        }

        public List<XElement> GetModels() {
            return _doc.Descendants("models").Descendants("model").ToList();
        }


        public List<XElement> GetPresets() {
            return _doc.Descendants("effects").Descendants("effect").ToList();
        }
 
        public List<XElement> AddPreset(XElement newPreset) {
            var effects = GetPresets();
            effects.Add(newPreset);
            SaveDoc();

            return effects;
        } 

        public string[] GetPalettes() {
            return _doc.Descendants("palettes").Descendants("palette").Select(e => {
                var attr = e.Attribute("name");
                return attr != null ? attr.Value : null;
            }).ToArray();
        }


        public void SavePalette(string response, string[] colors) {
            var pal = new XElement("palette", new XAttribute("name", response), new XAttribute("color1", colors[0]),
                                   new XAttribute("color2", colors[1]), new XAttribute("color3", colors[2]), new XAttribute("color4", colors[3]),
                                   new XAttribute("color5", colors[4]), new XAttribute("color6", colors[5]));

            var pals = _doc.Element(XName.Get("palettes"));
            if (pals == null) {
                return;
            }

            pals.Add(pal);
            SaveDoc();
        }


        public void RemovePalette(string palette) {
            GetPaletteByName(palette).Remove();
            SaveDoc();
        }


        public string GetDataForEffect(string settingName) {
            var effectData = _doc.Descendants("effects").Descendants("effect").Where(e => {
                var attr = e.Attribute("name");
                return attr != null && attr.Value == settingName;
            }).First().Attribute("settings");
            return effectData != null ? effectData.Value : null;
        }

        private void SaveDoc() {
            _doc.Save(Paths.NutcrackerDataFile);
        }
    }
}
