// I use this to convert the nif files to gltf
// Noesis is required to run this
// No, I won't make this readable, maybe in the future, uwu

// Quickest tutorial I can give:
// 1. Download Noesis
// 2. Export the nif files to Resources/Model
// 3. Use the app to extract all the textures from the models
// 4. Use the app to convert the nif files to gltf
// 5. glhf

using System.Diagnostics;
using MaplePacketLib2.Tools;

string solutionDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
string modelDir = new(Path.Combine(solutionDir, "Maple2Storage", "Resources", "Model"));
string texturesDir = new(Path.Combine(solutionDir, "Maple2Storage", "Resources", "Textures", "Textures"));
string output = new(Path.Combine(solutionDir, "Maple2Storage", "Resources", "GLTF"));
const string quote = "\"";

string[] textures = Directory.GetFileSystemEntries(texturesDir, "*.dds", SearchOption.AllDirectories);
string[] models = Directory.GetFileSystemEntries(modelDir, "*.nif", SearchOption.AllDirectories);

foreach (string modelPath in models) {
    string fileName = Path.GetFileNameWithoutExtension(modelPath);
    string outputFolder = new(Path.Combine(output, fileName));
    Directory.CreateDirectory(outputFolder);
    string directoryName = Path.GetDirectoryName(modelPath)!;

    // try
    // {
    //     GetTexturesFromNif();
    // }
    // catch
    // {
    //     Console.WriteLine($"Error reading {modelPath}");
    //     throw;
    // }
    //
    // continue;

    try {
        Console.WriteLine($"Converting {fileName} nif to gltf");

        // check if gltf already exists
        if (File.Exists(@$"{output}\{fileName}\{fileName}.gltf")) {
            Console.WriteLine("Skipping " + fileName + " base model.");
        } else {
            string strCmdText2 = @$"/C noesis.exe ?cmode {quote}{modelPath}{quote} {quote}{output}\{fileName}\{fileName}.gltf{quote}";
            Process? process2 = Process.Start("CMD.exe", strCmdText2);
            // wait for process to finish
            process2.WaitForExit();

            Console.WriteLine("Finished converting " + fileName + " base model.");
        }

        // Only do animations for NPC folder
        if (!directoryName.Contains("Npc")) {
            continue;
        }

        // get output textures .png
        string[] baseTextures = Directory.GetFileSystemEntries(outputFolder, "*.png", SearchOption.TopDirectoryOnly);

        // find kf files that match file name
        string[] animations = Directory.GetFileSystemEntries(directoryName, "*.kf", SearchOption.TopDirectoryOnly);
        foreach (string animation in animations) {
            string animationName = Path.GetFileNameWithoutExtension(animation);

            Console.WriteLine($"Converting {fileName} kf to gltf");
            // check if gltf dont exists
            if (!File.Exists(@$"{output}\{fileName}\{animationName}.gltf")) {
                // CD into input directory
                string cdCmdText = $"/C cd /d {quote}{directoryName}{quote}";
                string strCmdText = @$"{cdCmdText} && noesis.exe ?cmode {quote}{animation}{quote} {quote}{output}\{fileName}\{animationName}.gltf{quote}";
                Process? process = Process.Start("CMD.exe", strCmdText);
                // wait for process to finish
                process.WaitForExit();

                // get output animation textures .png
                string[] pngs = Directory.GetFileSystemEntries(outputFolder, "*.png", SearchOption.TopDirectoryOnly);
                string[] animationTextures = pngs.Where(
                    x => Path.GetFileName(x).StartsWith(animationName)
                ).ToArray();

                // check if animations and base count match if not continue
                if (animationTextures.Length != baseTextures.Length) {
                    Console.WriteLine("Skipping cleanup of " + fileName + " animation.");
                    continue;
                }

                // open gltf file, rename textures to use base texture for all animations
                string gltfPath = @$"{output}\{fileName}\{animationName}.gltf";
                string gltf = File.ReadAllText(gltfPath);
                for (int i = 0; i < animationTextures.Length; i++) {
                    string animationTexture = Path.GetFileName(animationTextures[i]);
                    string baseTexture = Path.GetFileName(baseTextures[i]);
                    gltf = gltf.Replace(animationTexture, baseTexture);
                }

                File.WriteAllText(gltfPath, gltf);

                // delete animation textures
                foreach (string animationTexture in animationTextures) {
                    File.Delete(animationTexture);
                }

                Console.WriteLine("Finished converting " + animationName + " animation.");
            } else {
                Console.WriteLine("Skipping " + fileName + " animation.");
            }
        }
    } catch (Exception e) {
        Console.WriteLine(e);
        throw;
    }

    void GetTexturesFromNif() {
        byte[] fileBytes = File.ReadAllBytes(modelPath);
        PacketReader reader = new(fileBytes);
        reader.Skip(39);
        reader.Skip(4); // version
        reader.Skip(1); // endianness

        reader.Skip(4); // user version
        int numBlocks = reader.ReadInt(); // num blocks
        int metadataSize = reader.ReadInt(); // metadata size
        reader.Skip(metadataSize);

        short count = reader.ReadShort();
        for (int i = 0; i < count; i++) {
            reader.ReadString();
        }

        reader.Skip(numBlocks * 2);
        reader.Skip(numBlocks * 4);
        int count2 = reader.ReadInt();
        reader.Skip(4);
        List<string> ddsFiles = new();
        for (int i = 0; i < count2; i++) {
            string values = reader.ReadString();
            if (values.Contains(".dds", StringComparison.OrdinalIgnoreCase)) {
                ddsFiles.Add(values);
            }
        }

        foreach (string ddsFile in ddsFiles) {
            string? file = textures.FirstOrDefault(x => x.Contains(ddsFile, StringComparison.OrdinalIgnoreCase));
            if (file is null) {
                continue;
            }

            // get path without filename
            string path = directoryName;

            string destFileName = path + '\\' + file.Split('\\').Last();
            if (File.Exists(destFileName)) {
                continue;
            }

            Console.WriteLine($"Copying {ddsFile} to {destFileName}");
            File.Copy(file, destFileName);
        }
    }
}