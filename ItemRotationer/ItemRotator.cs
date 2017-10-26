﻿using HutongGames.PlayMaker;
using MSCLoader;
using System;
using UnityEngine;

namespace ItemRotator
{
    public class ItemRotator : Mod
    {
        public override string ID => "ItemRotator";
        public override string Name => "ItemRotator";
        public override string Author => "PigeonBB";
        public override string Version => "0.1";

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        private const float HORIZONTAL_FACTOR = 2f;
        private const float VERTICAL_FACTOR = 2f;
        private const float MOVE_FACTOR = 1f/4f;
        private GameObject _HAND;
        private GameObject _CAM_HORIZONTAL;
        private GameObject _CAM_VERTICAL;
        private PlayMakerFSM _fsmHand = null;
        private struct Angle
        {
            public float x, y, z;
            private Angle(float p1, float p2, float p3)
            {
                x = p1;
                y = p2;
                z = p3;
            }
        }

        Keybind keyHorizonVirtical = new Keybind("hv", "Horizon and Vitical", KeyCode.LeftControl);
        Keybind keyRoll = new Keybind("roll", "Rolling", KeyCode.LeftControl, KeyCode.LeftShift);

        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, keyHorizonVirtical);
            Keybind.Add(this, keyRoll);
        }

        // Update is called once per frame
        public override void Update()
        {
            if (Application.loadedLevelName != "GAME")
                return;

            if (!Initialize())
                return;

            var isHnadEmpty = _HAND.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("HandEmpty").Value;
            var isInMenu = FsmVariables.GlobalVariables.FindFsmBool("PlayerInMenu").Value;
            if (isHnadEmpty)
            {
                return;
            }
            else if (isInMenu)
            {
                return;
            }

            Angle angle;
            angle.x = Input.GetAxis("Mouse X");
            angle.y = Input.GetAxis("Mouse Y");
            angle.z = Input.GetAxis("Mouse ScrollWheel");
            if (keyHorizonVirtical.IsPressed() && !keyRoll.IsPressed())
            {
                _fsmHand.enabled = false;
                _CAM_VERTICAL.GetComponent<Behaviour>().enabled = false;
                _CAM_HORIZONTAL.GetComponent<Behaviour>().enabled = false;
                Rotator(_HAND.transform, _CAM_VERTICAL.transform ,angle, true);
            }
            else if (keyRoll.IsPressed())
            {
                _fsmHand.enabled = false;
                _CAM_VERTICAL.GetComponent<Behaviour>().enabled = false;
                _CAM_HORIZONTAL.GetComponent<Behaviour>().enabled = false;
                Rotator(_HAND.transform, _CAM_HORIZONTAL.transform, angle, false);
            }
            else if (Input.GetKeyUp(keyHorizonVirtical.Key) || Input.GetKeyUp(keyRoll.Key))
            {
                _fsmHand.enabled = true;
                _CAM_VERTICAL.GetComponent<Behaviour>().enabled = true;
                _CAM_HORIZONTAL.GetComponent<Behaviour>().enabled = true;
            }
        }

        // rotation
        private void Rotator(Transform target, Transform axis ,Angle angle, bool horizontal)
        {
            if (horizontal)
            {
                target.Rotate(axis.up, -angle.x * HORIZONTAL_FACTOR, Space.World); // horizontal rotating
            }
            else
            {
                target.Rotate(axis.forward, -angle.x * HORIZONTAL_FACTOR, Space.World); // rolling
            }
            target.Rotate(axis.right, angle.y * VERTICAL_FACTOR, Space.World); // vertical rotating

            if (target.localPosition.z > 0.1f)
            {
                target.localPosition += new Vector3(0, 0, angle.z * MOVE_FACTOR); // back and forward
            }
            else
            {
                target.localPosition += new Vector3(0, 0, 0.1f); // limited near position
            }
        }

        // init
        private bool Initialize()
        {
            _HAND = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera/1Hand_Assemble/Hand");
            _CAM_HORIZONTAL = GameObject.Find("PLAYER");
            _CAM_VERTICAL = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera");
            if (_HAND == null || _CAM_VERTICAL == null || _CAM_HORIZONTAL == null)
            {
                return false;
            }
            foreach (var e in _HAND.GetComponentsInChildren<PlayMakerFSM>())
            {
                foreach (var a in e.FsmVariables.GetAllNamedVariables())
                {
                    if (a.Name == "RotationX")
                    {
                        _fsmHand = e;
                        break;
                    }
                }
                if (_fsmHand != null) break;
            }

            return true;
        }
    }

    /*********************
    * for ModLoader ver.0.3.2
    **********************
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
    */
}