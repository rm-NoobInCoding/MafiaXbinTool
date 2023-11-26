using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MafiaXbinTool
{
    internal class Program
    {
        public static ulong FileID = 6792254151303881171;
        public static ulong FileVersion = 4;
        public static int EntrySize = 16;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    if (Path.GetExtension(args[0]) == ".xbin")
                    {
                        List<string> strs = new List<string>();
                        List<string> ids = new List<string>();
                        BinaryReader br = new BinaryReader(File.OpenRead(args[0]));
                        br.ReadInt64(); //FileID
                        br.ReadInt64(); //FileVersion
                        br.ReadInt32(); //always -1
                        br.ReadInt32(); //EntrySize
                        br.ReadInt32(); //count of strings but int32
                        long count = br.ReadInt64();
                        for (int i = 0; i < count; i++)
                        {
                            ids.Add(br.ReadUInt64().ToString());
                            ulong offset = br.ReadUInt64();
                            long pos = br.BaseStream.Position;
                            br.BaseStream.Seek((long)offset - 8, SeekOrigin.Current);
                            strs.Add(br.ReadNullTerminated());
                            br.BaseStream.Seek(pos, SeekOrigin.Begin);

                        }
                        File.WriteAllLines(Path.ChangeExtension(args[0], ".txt"), strs.ToArray());
                        File.WriteAllLines(Path.ChangeExtension(args[0], ".ids"), ids.ToArray());
                    }
                    else if (Path.GetExtension(args[0]) == ".txt" && File.Exists(Path.ChangeExtension(args[0], ".ids")))
                    {
                        List<string> strs = new List<string>(File.ReadAllLines(args[0]));
                        List<string> ids = new List<string>(File.ReadAllLines(Path.ChangeExtension(args[0], ".ids")));
                        if (strs.Count != ids.Count) Console.WriteLine("count of texts and ids are not equal!");
                        BinaryWriter bw = new BinaryWriter(File.OpenWrite(Path.ChangeExtension(args[0], ".xbin")));
                        bw.Write(FileID);
                        bw.Write(FileVersion);
                        bw.Write(-1);
                        bw.Write(EntrySize);
                        bw.Write(strs.Count);
                        bw.Write((ulong)strs.Count);
                        MemoryStream StrChunk = new MemoryStream();
                        for(int i = 0; i < strs.Count; i++)
                        {
                            bw.Write(ulong.Parse(ids[i]));
                            bw.Write((ulong)((strs.Count - i) * EntrySize + StrChunk.Length - 8));
                            StrChunk.WriteStr(strs[i]);
                        }
                        bw.Write(StrChunk.ToArray());
                        bw.Close();
                        Console.WriteLine("Done!");
                    }
                    else
                    {
                        Console.WriteLine("Unknown file extension : " + Path.GetExtension(args[0]));
                    }

                }
                else
                {
                    Console.WriteLine("File does not exist!");
                }

            }
        }
    }
}
