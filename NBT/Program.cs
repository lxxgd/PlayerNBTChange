using fNbt;

namespace NBT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("请输入存档文件夹路径:");
            string? path = Console.ReadLine();
            if (Directory.Exists(path))
            {
                string levelData = Path.Combine(path, "level.dat");
                NbtFile? levleNBT = null;
                if (File.Exists(levelData))
                {
                    levleNBT = new NbtFile(levelData);  
                }
                if (levleNBT == null) 
                { 
                    Console.WriteLine("level.dat文件不存在");
                    return;
                }
                string playerDataPath = Path.Combine(path, "playerdata");
                Dictionary<string,NbtFile> playerNBTs = new Dictionary<string,NbtFile>();
                if (Directory.Exists(playerDataPath))
                {
                    foreach (string file in Directory.GetFiles(playerDataPath))
                    {
                        if(file.EndsWith(".dat"))
                        {
                            NbtFile nbt = new NbtFile(file);
                            playerNBTs.Add(file.Split('\\').Last().Split('.')[0].Replace("-",""),nbt);
                        }
                    }
                }
                Console.WriteLine("要根据什么读取玩家数据？0.玩家名 1.UUID");
                int readType = int.Parse(Console.ReadLine());

                if (readType == 1)
                {
                    Console.WriteLine("请输入UUID（不要带“-”，例如bb3175b8a2b643b3978575515c67e1f6是正确的，cd97120b-035c-4461-a663-440185404da3是错误的）:");
                    string? uuid = Console.ReadLine();
                    if (!string.IsNullOrEmpty(uuid))
                    {
                        if (playerNBTs.TryGetValue(uuid, out NbtFile? value))
                        {
                            NbtFile playerNBT = value;
                            NbtCompound playerData = (NbtCompound)playerNBT.RootTag;
                            playerData.Name = "Player";
                            levleNBT.RootTag["Data"]["Player"] = playerData;
                        }
                        else
                        {
                            Console.WriteLine("该玩家没有nbt文件");
                        }
                    }
                }
                else if (readType == 0)
                {
                    Console.WriteLine("请输入玩家名称:");
                    string? playerName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        string uuid = UUIDUtils.PlayerNameToUUID(playerName);
                        Console.WriteLine(playerName + " uuid: " + uuid);
                        if (playerNBTs.TryGetValue(uuid, out NbtFile? value))
                        {
                            NbtFile playerNBT = value;
                            NbtCompound playerData = (NbtCompound)playerNBT.RootTag;
                            playerData.Name = "Player";
                            levleNBT.RootTag["Data"]["Player"] = playerData;
                        }
                        else
                        {
                            Console.WriteLine("该玩家没有nbt文件");
                        }
                    }
                }
                levleNBT.SaveToFile(levelData,levleNBT.FileCompression);
                Console.WriteLine("操作完成");
            }
            else
            {
                Console.WriteLine("不是有效的文件夹");
            }
            Console.ReadLine();
        }
    }
}
