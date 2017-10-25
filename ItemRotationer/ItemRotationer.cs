using HutongGames.PlayMaker;
using MSCLoader;
using System.IO;
using UnityEngine;
using System;

namespace ItemRotationer
{
    public class ItemRotationer : Mod
    {
        private GameObject _ITEM;
        private GameObject _CAM_HORIZONTAL;
        private GameObject _CAM_VERTICAL;
        private bool _go = true;

        KeyPress keyHorizonVirtical = new KeyPress("hv", "Horizon and Vitical", KeyCode.LeftControl);
        KeyPress keyRoll = new KeyPress("roll", "Rolling", KeyCode.LeftControl, KeyCode.LeftShift);

        public override string ID => "ItemRotationer";
        public override string Name => "ItemRotationer";
        public override string Author => "PigeonBB";
        public override string Version => "0.1";

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, keyHorizonVirtical);
            Keybind.Add(this, keyRoll);
        }

        // Update is called once per frame
        public override void Update()
        {
            if (Application.loadedLevelName != "GAME") return;

            if (!Initialize()) return;

            var isHandEmpty = _ITEM.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("HandEmpty").Value;
            if (isHandEmpty)
            {
                return;
            }

            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");
            if (keyHorizonVirtical.IsPressed() && !keyRoll.IsPressed())
            {
                _CAM_VERTICAL.GetComponent<Behaviour>().enabled = false;
                _CAM_HORIZONTAL.GetComponent<Behaviour>().enabled = false;
                _ITEM.transform.Rotate(_CAM_VERTICAL.transform.up, -x, Space.World);
                _ITEM.transform.Rotate(_CAM_VERTICAL.transform.right, y, Space.World);
                /** 終わったら 0,0,0 に戻す必要あり */
            }
            else if (keyRoll.IsPressed()/* && !Input.anyKeyDown*/)
            {
                _CAM_VERTICAL.GetComponent<Behaviour>().enabled = false;
                _CAM_HORIZONTAL.GetComponent<Behaviour>().enabled = false;
                _ITEM.transform.Rotate(_CAM_HORIZONTAL.transform.forward, -x, Space.World);
                _ITEM.transform.Rotate(_CAM_HORIZONTAL.transform.right, y, Space.World);
            }
            _CAM_VERTICAL.GetComponent<Behaviour>().enabled = true;
            _CAM_HORIZONTAL.GetComponent<Behaviour>().enabled = true;
        }

        private bool Initialize()
        {
            _ITEM = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera/1Hand_Assemble/Hand");
            _CAM_HORIZONTAL = GameObject.Find("PLAYER");
            _CAM_VERTICAL = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera");
            if (_ITEM == null || _CAM_VERTICAL == null || _CAM_HORIZONTAL == null)
            {
                return false;
            }
            return true;
        }
    }

    /*********************
    * for ModLoader ver.0.3.2
    **********************/
    public class KeyPress : Keybind
    {
        public KeyPress(string id, string name, KeyCode key) : base(id, name, key)
        {
            ID = id;
            Name = name;
            Key = key;
            Modifier = KeyCode.None;
        }
        public KeyPress(string id, string name, KeyCode key, KeyCode modifier) : base(id, name, key, modifier)
        {
            ID = id;
            Name = name;
            Key = key;
            Modifier = modifier;
        }
        new public bool IsPressed()
        {
            if (Modifier != KeyCode.None)
            {
                return Input.GetKey(Modifier) && Input.GetKey(Key);
            }
            return Input.GetKey(Key);
        }
    }
}
