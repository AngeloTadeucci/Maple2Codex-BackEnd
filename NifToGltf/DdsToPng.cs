using System.Diagnostics;
using MaplePacketLib2.Tools;
public class DdsToPng {

    public static void Convert() {
        const char quote = '"';
        string ddsPath = Path.Combine("D:", "Projetos", "Maple2_Codex", "MapleStory2-Handbook", "static", "resource", "image", "map", "bg");
        string outputPath = Path.Combine("D:", "Projetos", "Maple2_Codex", "MapleStory2-Handbook", "static", "resource", "image", "map", "bg", "output");

        foreach (string file in Directory.GetFileSystemEntries(ddsPath, "*.dds", SearchOption.AllDirectories)) {
            string fileName = Path.GetFileName(file).Replace(".dds", ".png");
            string destFileName = Path.Combine(outputPath, fileName);

            Directory.CreateDirectory(outputPath);

            string strCmdText2 = @$"/C noesis.exe ?cmode {quote}{file}{quote} {quote}{destFileName}{quote}";
            Process? process2 = Process.Start("CMD.exe", strCmdText2);
            // wait for process to finish
            process2.WaitForExit();
        }

    }
}
