using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Identity;
using WebmilioCommons.Inputs;
using WebmilioCommons.Networking;
using WebmilioCommons.Time;

namespace WebmilioCommons
{
    public sealed partial class WebmilioCommonsMod : Mod
    {
        public WebmilioCommonsMod()
        {
            Instance = this;
        }


        public override void Load()
        {
            KeyboardManager.Load();

            if (!Main.dedServ)
                IdentityManager.Load();

            On.Terraria.WorldGen.SaveAndQuit += WorldGenOnSaveAndQuit;

            TimeManagement.Load();
        }

        public override void PostSetupContent()
        {
            // In theory, you don't need to call the 'TryLoad()' method on any singletons since the instance is loaded the second its created.
            // I only put it there to make it obvious.

            if (!Main.dedServ)
                IdentityManager.PostSetupContent();

            NetworkPacketLoader.Instance.TryLoad();

            //PlayerSynchronizationPacket.Construct();
        }

        public override void Unload()
        {
            On.Terraria.WorldGen.SaveAndQuit -= WorldGenOnSaveAndQuit;

            TimeManagement.Unload();

            NetworkPacketLoader.Instance.Unload();
            IdentityManager.Unload();

            Instance = null;
        }

        private void WorldGenOnSaveAndQuit(On.Terraria.WorldGen.orig_SaveAndQuit orig, Action callback)
        {
            orig(callback);

            TimeManagement.ForceUnalter();
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkPacketLoader.Instance.HandlePacket(reader, whoAmI);
        }

        public override void PostUpdateInput()
        {
            KeyboardManager.Update();
        }


        public static WebmilioCommonsMod Instance { get; private set; }
    }
}