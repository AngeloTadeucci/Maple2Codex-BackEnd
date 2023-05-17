using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class AnimationParser
{
    public static readonly Dictionary<string, List<string>> Animations = new();

    static AnimationParser()
    {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("anikeytext.xml")));
        if (xmlFile is null)
        {
            throw new("Failed to load anikeytext.xml");
        }

        XmlNodeList? animationNodes = xmlFile.SelectNodes("/ms2ani/kfm");
        if (animationNodes is null)
        {
            throw new("Failed to load anikeytext.xml");
        }

        foreach (XmlNode kmfNodes in animationNodes)
        {
            string? animationName = kmfNodes.Attributes?["name"]?.Value;
            if (animationName is null)
            {
                throw new("Failed to load anikeytext.xml");
            }

            XmlNodeList? seqNodes = kmfNodes.SelectNodes("seq");
            if (seqNodes is null)
            {
                throw new("Failed to load anikeytext.xml");
            }

            List<string> animations = new();
            foreach (XmlNode keyNode in seqNodes)
            {
                string? keyName = keyNode.Attributes?["name"]?.Value;
                if (keyName is null)
                {
                    throw new("Failed to load anikeytext.xml");
                }

                animations.Add(keyName.ToLower());
            }

            Animations.Add(animationName.ToLower(), animations);
        }
    }
}