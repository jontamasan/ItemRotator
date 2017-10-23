using MSCLoader;
using UnityEngine;

namespace ItemRotationer
{
    public class ItemRotationer : Mod
    {
        public override string ID => "ItemRotationer";
        public override string Name => "ItemRotationer";
        public override string Author => "Your Username";
        public override string Version => "1.0";

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        //Called when mod is loading
        public override void OnLoad()
        {

        }

        // Update is called once per frame
        public override void Update()
        {

        }
    }
}
