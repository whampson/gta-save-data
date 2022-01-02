using GTASaveData;
using GTASaveData.GTA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Examples.GTA3
{
    public static class GTA3Examples
    {
        static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
        }

        static void Example1()
        {
            // --------------------------------------------------------------------------------
            // This example demonstrates some basic editing techniques.
            // --------------------------------------------------------------------------------

            // First, grab a file from our test data suite.
            // This file was saved on Staunton Island after completing 'Casino Calamity'
            // and the next mission available is 'Sayonara Salvatore'.
            string srcPath = Support.GetFileFromTestData("RC3", GTA3Save.FileTypes.PC);

            // Load it!
            // GTA3Save implements IDisposable, so be sure to use the 'using' statement
            // when working with this class.
            using GTA3Save s = GTA3Save.Load(srcPath);


            // ----------------------------- BEGIN MODIFICATIONS ------------------------------


            // 1) Set title and timestamp
            {
                s.Title = "~b~EXAMPLE 1";            // ~b~ sets the color to blue!
                s.TimeStamp = new DateTime(2015, 10, 21, 19, 28, 0);
                // The title may also be set via 'SimpleVars.LastMissionPassedName'
                // and time stamp via 'SimpleVars.TimeStamp'
            }

            // 2) Tweak some game settings
            {
                // Game settings and generic world params are stored in 'SimpleVars'
                var simp = s.SimpleVars;

                simp.GameClockHours = 18;
                simp.GameClockMinutes = 30;
                simp.MillisecondsPerGameMinute = 5000;
                simp.ForcedWeatherType = WeatherType.Sunny;
                simp.CameraModeInCar = CameraMode.Cinematic;
                simp.CameraModeOnFoot = CameraMode.TopDown;

                // On PS2, there is no gta3.set; all the game settings are stored in SimpleVars.
                // On PC, these do nothing.
                if (s.IsPS2)
                {
                    simp.Brightness = 255;
                    simp.MusicVolume = 63;
                    simp.SfxVolume = 127;
                    simp.Trails = true;
                    simp.UseMono = true;
                    simp.ShowSubtitles = false;
                    simp.UseWideScreen = true;
                    simp.UseVibration = true;
                    simp.Language = Language.German;    // this will enable censorship on PAL version,
                                                        // will crash NTSC version since GERMAN.GXT is not on the disk
                }
            }

            // 3) Edit garage vehicles
            {
                // Let's replace the cars in the Staunton Island garage
                var stauntonGarage = s.Garages.StoredCarsStaunton;

                // Mafia Sentinel -> Banshee
                StoredCar car1 = stauntonGarage.First(c => c.Model == 134);
                car1.Model = 119;                   // Banshee
                car1.Color1 = 6;                    // taxi yellow
                car1.Color2 = 0;                    // black
                car1.Heading = 180;                 // south
                car1.Flags = StoredCarFlags.All;    // bulletproof, collision proof, explosion proof, fireproof, meleeproof
                car1.Radio = RadioStation.Lips106;

                // Diablo Stallion -> Stinger
                StoredCar car2 = stauntonGarage.First(c => c.Model == 137);
                car2.Model = 92;                    // Stinger
                car2.Color1 = 7;                    // stirking blue
                car2.Color2 = 78;                   // silver
                car2.Heading = 0;                   // north
                car2.Extra1 = 0;                    // hardtop
                car2.Extra2 = -1;                   // (none)
                car2.Flags = StoredCarFlags.BulletProof;
                car2.Radio = RadioStation.None;
            }

            // 4) Edit player info
            {
                // Player info is stored in two locations
                var pPed = s.PlayerPeds.GetPlayerPed();
                var pInfo = s.PlayerInfo;

                // Health, armor, wanted level, weapons, position, player model, and some other useless stuff
                pPed.Health = 999;                  // can go higher, but game only shows 3 digits well
                pPed.Armor = 999;
                pPed.MaxWantedLevel = 6;
                pPed.MaxChaosLevel = 6400;
                pPed.ModelName = "DARKEL";          // only 'Special' models allowed, see https://gtamods.com/wiki/023C#List_of_valid_models
                pPed.GiveWeapon(WeaponType.M16, 99999);
                pPed.GiveWeapon(WeaponType.RocketLauncher, 100);

                // Money, perks, and some other informational and useless stuff
                pInfo.InfiniteSprint = true;        // awarded after completing Paramedic
                pInfo.GetOutOfJailFree = true;      // awarded after getting 10 consecutive kills on Vigilante, works once then resets
                pInfo.GetOutOfHospitalFree = true;  // not used in vanilla game, works once then resets
                s.Garages.FreeBombs = true;         // you can also get free bombs and resprays...
                s.Garages.FreeResprays = true;      // ...but these are stored in the Garages block

                int totalMoney = pInfo.GiveMoney(5000000, true);     // second param controls the money counter animation

                // Print some info
                Console.WriteLine($"Player money: {totalMoney:C0}");
                Console.WriteLine($"{pInfo.PackagesCollected} out of {pInfo.PackagesTotal} Hidden Packages collected.");
                if (pInfo.WastedBustedTime != 0)
                {
                    Console.WriteLine("Player has been wasted or busted on this save file.");
                }
            }

            // 5) Sayonara Salvatore is a hard mission... let's skip it!
            {
                // Global variables and script addresses vary by SCM version, make sure we've got the right one.
                Debug.Assert(s.Script.MainScriptSize == 108797);        // PC and PS2v2 SCM

                // Mimic the 'Mission Passed' code from the :ASUKA1 thread of MAIN.SCM,
                // that way it looks like the mission was passed normally ;)
                s.Stats.MissionsGiven += 1;                                     // 0317: increment_mission_attempts
                s.Script.SetGlobal(316, 1);                                     // $FLAG_ASUKA_MISSION1_PASSED = 1
                s.PedTypeInfo.AddThreat(PedTypeId.Gang1, PedTypeFlags.Player1); // 03F1: pedtype 7 add_threat 1
                {                                                               // 0237: set_gang 0 primary_weapon_to 2 secondary_weapon_to 4 
                    s.Gangs[GangType.Mafia].Weapon1 = WeaponType.Colt45;
                    s.Gangs[GangType.Mafia].Weapon2 = WeaponType.Shotgun;
                }
                {                                                               // 0318: set_latest_mission_passed 'AM1'
                    s.Stats.MissionsPassed += 1;
                    s.Stats.LastMissionPassedName = "AM1";
                }
                s.Stats.ProgressMade += 1;                                      // 030C: progress_made += 1
                s.Script.StartNewScript(50022);                                 // 004F: start_new_script @NONAME_42
                s.Script.StartNewScript(61388);                                 // 004F: start_new_script @JOE_BUG
                s.Script.StartNewScript(63065);                                 // 004F: start_new_script @YARD_PH
                s.Script.StartNewScript(63327);                                 // 004F: start_new_script @ASUK_DR

                Console.WriteLine("Mission Passed: Sayonara Salvatore");
            }


            // ------------------------------ END MODIFICATIONS -------------------------------


            // Save it!
            string dstPath = Support.GetSaveSlot(1);
            int numWritten = s.Save(dstPath);

            Console.WriteLine($"{numWritten} bytes written to '{dstPath}'.");
        }

        static void Example2()
        {
            // --------------------------------------------------------------------------------
            // This example demonstrates more advanced editing techniques.
            // --------------------------------------------------------------------------------

            // Lets grab another save from our test suite.
            // This one is a starter save with only 'Luigi's Girls' completed.
            string srcPath = Support.GetFileFromTestData("LM1", GTA3Save.FileTypes.PC);
            using GTA3Save s = GTA3Save.Load(srcPath);


            // ----------------------------- BEGIN MODIFICATIONS ------------------------------


            // 1) Set title and timestamp
            {
                s.Title = "~g~EXAMPLE 2";
                s.TimeStamp = DateTime.Now;
            }

            // 2) Inject custom SCM code!
            {
                // It is possible to inject executable SCM code into save files via
                // the 'Script.ScriptSpace' array. This space is normally used for
                // the global variables, but the game does not do any size checking
                // when loading this array (it simply overwrites MAIN.SCM starting
                // from offset 0), making it possible for us to utilize this space
                // for injecting our own SCM code! As far as I know, it works on all
                // versions of GTA3 (even PS2!).

                var scr = s.Script;

                // First, expand the script space a bit to make room for our new code.
                // You can also overwrite any global variables you choose, but if
                // you don't want to mess up the story mode missions, it's best to
                // expand the script space for your code.
                scr.GrowScriptSpace(44);

                // The first 8 bytes of ScriptSpace are reserved for the purpose of
                // telling the game how big the global variable space is (this is why
                // you can't use global variables 0 and 1). Thus, expanding the script
                // space does not affect the size of the global variable space. You can
                // add global variables by expanding the script space, then setting the
                // variable space size via SetSizeOfVariableSpace(int). We're not doing
                // that in this example, instead we will use the end of the variable
                // space as the base address for our custom script.
                int baseAddr = scr.GetGlobalVariableSpaceSize();

                // Now for the fun part: writing raw SCM code!
                // Each command consists of a 2-byte opcode and a predefined number
                // of arguments (usually) depending on the opcode being used. String
                // arguments (e.g. GXT strings) always occupy 8 bytes. For numeric
                // arguments, a 1-byte type specifier is included followed by a
                // 1- to 4-byte argument depending on the type.
                // Type specifiers are as follows:
                //   1 = INT32      (4 bytes)
                //   2 = GLOBALVAR  (2 bytes)
                //   3 = LOCALVAR   (2 bytes)
                //   4 = INT8       (1 byte)
                //   5 = INT16      (2 bytes)
                //   6 = FLOAT*     (2 bytes)
                //
                // *Not really a float; it's a 2-byte fixed-point value with 1/16 precision.
                //
                // This code will print a message to the pager then terminate.
                int addr = baseAddr;
                {                                                               // 0001: wait 5000
                    addr += scr.Write2BytesToScript(addr, 0x0001);
                    addr += scr.Write1ByteToScript(addr, 5);
                    addr += scr.Write2BytesToScript(addr, 5000);
                }
                {                                                               // 014D: text_pager 'YD4_C' 100 2 0
                    addr += scr.Write2BytesToScript(addr, 0x014D);
                    addr += scr.WriteTextLabelToScript(addr, "YD4_C");
                    addr += scr.Write1ByteToScript(addr, 5);
                    addr += scr.Write2BytesToScript(addr, 100);
                    addr += scr.Write1ByteToScript(addr, 4);
                    addr += scr.Write1ByteToScript(addr, 2);
                    addr += scr.Write1ByteToScript(addr, 4);
                    addr += scr.Write1ByteToScript(addr, 0);
                }
                {                                                               // 014D: text_pager 'YD4_D' 100 2 0
                    addr += scr.Write2BytesToScript(addr, 0x014D);
                    addr += scr.WriteTextLabelToScript(addr, "YD4_D");
                    addr += scr.Write1ByteToScript(addr, 5);
                    addr += scr.Write2BytesToScript(addr, 100);
                    addr += scr.Write1ByteToScript(addr, 4);
                    addr += scr.Write1ByteToScript(addr, 2);
                    addr += scr.Write1ByteToScript(addr, 4);
                    addr += scr.Write1ByteToScript(addr, 0);
                }
                addr += scr.Write2BytesToScript(addr, 0x004E);                  // 004E: terminate_this_script

                Console.WriteLine($"{addr - baseAddr} bytes written to script at address 0x{baseAddr:X4}.");

                // NOTE: you may also inject SCM bytecode by writing directly
                // to 'Script.ScriptSpace'. See the next example for details!

                // Last, create a new RunningScript pointing at the
                // base address to kick everything off!
                scr.StartNewScript(baseAddr);
            }

            // 3) Unlock islands
            {
                // We could do this by modifying 'Script.BuildingSwaps' and 'Script.InvisibilitySettings'
                // to fix the bridge, 'Objects.Objects' to destroy the tunnel barriers, and 'Paths.PathNodes'
                // to restore the paths, but this is difficult as a) the handles may not be consistent between
                // saves/GTA3 versions, and b) it's not clear which path nodes stored in the save file
                // correspond to which parts of Liberty City. It's best to just execute the SCM code that unlocks
                // the islands instead. We will do this by writing 'Script.ScriptSpace' directly.

                var scr = s.Script;

                // You could do it this way...
                byte[] code1 =
                {
                    // Fix the Callahan Bridge
                    0x4A,0x03,                                                                                          // 034A: portland_complete 
                    0xB6,0x03,0x06,0x26,0x2C,0x06,0x83,0xC5,0x06,0x31,0x04,0x06,0x40,0x01,0x04,0x83,0x04,0xA2,          // 03B6: replace_model_at 525.3125 -927.0625 71.8125 radius 20.0 from #NBBRIDGFK2 to #NBBRIDGCABLS01 
                    0xB6,0x03,0x06,0xD5,0x20,0x06,0x0F,0xC6,0x06,0x7D,0x04,0x06,0x40,0x01,0x04,0x83,0x04,0xA2,          // 03B6: replace_model_at 706.375 -935.8125 67.0625 radius 20.0 from #NBBRIDGFK2 to #NBBRIDGCABLS01 
                    0xB6,0x03,0x06,0x10,0x21,0x06,0x7F,0xC6,0x06,0xB8,0x02,0x06,0x40,0x01,0x04,0x82,0x04,0xA1,          // 03B6: replace_model_at 529.0 -920.0625 43.5 radius 20.0 from #DAMGBRIDGERDB to #NBBRIDGERDB 
                    0xB6,0x03,0x06,0xEC,0x2B,0x06,0x41,0xC5,0x06,0x6B,0x02,0x06,0x40,0x01,0x04,0x82,0x04,0xA1,          // 03B6: replace_model_at 702.75 -939.9375 38.6875 radius 20.0 from #DAMGBRIDGERDB to #NBBRIDGERDB 
                    0xB6,0x03,0x06,0x10,0x21,0x06,0x11,0xC5,0x06,0xB8,0x02,0x06,0x40,0x01,0x04,0x81,0x04,0xA0,          // 03B6: replace_model_at 529.0 -942.9375 43.5 radius 20.0 from #DAMGBBRIDGERDA to #NBBRIDGERDA 
                    0xB6,0x03,0x06,0xEC,0x2B,0x06,0x81,0xC6,0x06,0x6B,0x02,0x06,0x40,0x01,0x04,0x81,0x04,0xA0,          // 03B6: replace_model_at 702.75 -919.9375 38.6875 radius 20.0 from #DAMGBBRIDGERDA to #NBBRIDGERDA 
                    0xB6,0x03,0x06,0xD5,0x20,0x06,0x0F,0xC6,0x06,0x7D,0x04,0x06,0x40,0x01,0x04,0x80,0x04,0x8E,          // 03B6: replace_model_at 525.3125 -927.0625 71.8125 radius 20.0 from #LODRIDGFK2 to #LODRIDGCABLS01 
                    0xB6,0x03,0x06,0x26,0x2C,0x06,0x83,0xC5,0x06,0x31,0x04,0x06,0x40,0x01,0x04,0x80,0x04,0x8E,          // 03B6: replace_model_at 706.375 -935.8125 67.0625 radius 20.0 from #LODRIDGFK2 to #LODRIDGCABLS01 
                    0xB6,0x03,0x06,0x92,0x20,0x06,0x51,0xC6,0x06,0xB8,0x02,0x06,0x40,0x01,0x05,0x7F,0xFF,0x04,0x8F,     // 03B6: replace_model_at 521.125 -922.9375 43.5 radius 20.0 from #LODGBRIDGERDB to #LODRIDGERDB 
                    0xB6,0x03,0x06,0xEC,0x2B,0x06,0x41,0xC5,0x06,0x6B,0x02,0x06,0x40,0x01,0x05,0x7F,0xFF,0x04,0x8F,     // 03B6: replace_model_at 702.75 -939.9375 38.6875 radius 20.0 from #LODGBRIDGERDB to #LODRIDGERDB 
                    0xB6,0x03,0x06,0x10,0x21,0x06,0x3F,0xC5,0x06,0xB8,0x02,0x06,0x40,0x01,0x05,0x7E,0xFF,0x04,0x90,     // 03B6: replace_model_at 529.0 -940.0625 43.5 radius 20.0 from #LODGBBRIDGERDA to #LODRIDGERDA 
                    0xB6,0x03,0x06,0xEC,0x2B,0x06,0x81,0xC6,0x06,0x6B,0x02,0x06,0x40,0x01,0x05,0x7E,0xFF,0x04,0x90,     // 03B6: replace_model_at 702.75 -919.9375 38.6875 radius 20.0 from #LODGBBRIDGERDA to #LODRIDGERDA 
                    0x63,0x03,0x06,0x34,0x40,0x06,0xA4,0xC5,0x06,0xF0,0x00,0x06,0x20,0x03,0x04,0xE7,0x04,0x00,          // 0363: toggle_model_render_at 1027.25 -933.75 15.0 radius 50.0 object #INDHELIX_BARRIER 0 
                    0xB6,0x03,0x06,0x34,0x40,0x06,0xA4,0xC5,0x06,0xF0,0x00,0x06,0x20,0x03,0x04,0xE7,0x04,0xE6,          // 03B6: replace_model_at 1027.25 -933.75 15.0 radius 50.0 from #INDHELIX_BARRIER to #LOD_LAND014
                };

                // Inject the code
                int baseAddr = scr.GetScriptSpaceSize();
                int limit = scr.GrowScriptSpace(DataBuffer.Align4(code1.Length));   // align4 not required but it keeps some editing tools happy
                scr.ScriptSpace.CopyFrom(code1, baseAddr);

                // ...but this is ugly and hard to read.
                // Here's a more readable and less error-prone approach for the rest of the code
                // (see Script.cs for simple GTAScript "compiler")

                using MemoryStream buf = new MemoryStream();
                using BinaryWriter w = new BinaryWriter(buf);

                // Fix the Shoreside Lift Bridge
                w.Write(new ScriptCommand(0x034B).Compile());                       // 034B: staunton_complete

                // Restore paths
                w.Write(new ScriptCommand(0x01E7)                                   // 01E7: remove_forbidden_for_cars_cube 619.5625 -911.5 45.0 834.25 -954.5 32.0
                    .AddFloat(619.5625f).AddFloat(-911.5f).AddFloat(45.0f)          //          callahan bridge
                    .AddFloat(834.25f).AddFloat(-954.5f).AddFloat(32.0f)
                    .Compile());
                w.Write(new ScriptCommand(0x01E7)                                   // 01E7: remove_forbidden_for_cars_cube 659.5625 200.0 -20.0 945.75 147.5 5.0
                    .AddFloat(659.5625f).AddFloat(200.0f).AddFloat(-20.0f)          //          tunnel road
                    .AddFloat(945.75f).AddFloat(147.5f).AddFloat(5.0f)
                    .Compile());
                w.Write(new ScriptCommand(0x01E7)                                   // 01E7: remove_forbidden_for_cars_cube 529.5625 106.5 -30.0 581.375 65.6875 0.0 
                    .AddFloat(529.5625f).AddFloat(106.5f).AddFloat(-30.0f)          //          staunton road to shoreside vale/portland intersection
                    .AddFloat(581.375f).AddFloat(65.6875f).AddFloat(0.0f)
                    .Compile());
                w.Write(new ScriptCommand(0x01E7)                                   // 01E7: remove_forbidden_for_cars_cube 496.6875 75.5 -30.0 484.0 44.1875 0.0 
                    .AddFloat(496.6875f).AddFloat(75.5f).AddFloat(-30.0f)           //          tunnel to shoreside vale
                    .AddFloat(484.0f).AddFloat(44.1875f).AddFloat(0.0f)
                    .Compile());
                w.Write(new ScriptCommand(0x01E7)                                   // 01E7: remove_forbidden_for_cars_cube -46.75 -648.0 39.0 -69.0625 -614.0 50.0
                    .AddFloat(-46.75f).AddFloat(-648.0f).AddFloat(39.0f)            //          shoreside bridge
                    .AddFloat(-69.0625f).AddFloat(-614.0f).AddFloat(50.0f)
                    .Compile());

                // Remove barriers
                w.Write(new ScriptCommand(0x0108).AddGlobal(34).Compile());         // 0108: destroy_object $SUBWAY_GATE_INDUSTRIAL
                w.Write(new ScriptCommand(0x0108).AddGlobal(35).Compile());         // 0108: destroy_object $TUNNEL_GATE_INDUSTRIAL 
                w.Write(new ScriptCommand(0x0108).AddGlobal(37).Compile());         // 0108: destroy_object $SUBWAY_GATE_SUBURBAN1
                w.Write(new ScriptCommand(0x0108).AddGlobal(38).Compile());         // 0108: destroy_object $SUBWAY_GATE_SUBURBAN2
                w.Write(new ScriptCommand(0x0108).AddGlobal(39).Compile());         // 0108: destroy_object $TUNNEL_GATE_SUBURBAN
                w.Write(new ScriptCommand(0x0108).AddGlobal(51).Compile());         // 0108: destroy_object $HELIX_BARRIER

                // Make sure the Shoreside Vale barriers don't get created after completing 'Last Requests'
                w.Write(new ScriptCommand(0x0004).AddGlobal(428).AddInt(1)          // 0004: $BARRIERS_BEEN_ADDED = 1
                    .Compile());

                // Don't forget to terminate the script!
                w.Write(new ScriptCommand(0x004E).Compile());                       // 004E: terminate_this_script

                // Inject the code
                byte[] code2 = buf.ToArray();
                int offset = scr.GetScriptSpaceSize();
                limit = scr.GrowScriptSpace(DataBuffer.Align4(code2.Length));
                scr.ScriptSpace.CopyFrom(code2, offset);

                // Lastly, kick off the new script!
                scr.StartNewScript(baseAddr);

                Console.WriteLine($"{limit - baseAddr} bytes written to script at address 0x{baseAddr:X4}.");
            }


            // ------------------------------ END MODIFICATIONS -------------------------------


            string dstPath = Support.GetSaveSlot(2);
            int numWritten = s.Save(dstPath);

            Console.WriteLine($"{numWritten} bytes written to '{dstPath}'.");
        }

        static void Example3()
        {
            // --------------------------------------------------------------------------------
            // The last example demonstrated injecting a run-once script into the save; when
            // the game is saved again, the injected script will be lost. This example will
            // show how to inject a permanent script that will persist across saves.
            // --------------------------------------------------------------------------------

            // Lets grab the 'Luigi's Girls' save again.
            string srcPath = Support.GetFileFromTestData("LM1", GTA3Save.FileTypes.PC);
            using GTA3Save s = GTA3Save.Load(srcPath);


            // ----------------------------- BEGIN MODIFICATIONS ------------------------------


            // 1) Set title and timestamp
            {
                s.Title = "~p~EXAMPLE 3";
                s.TimeStamp = DateTime.Now;
            }

            // 2) Inject persistent script
            {
                // Let's create a car spawner script that will remain in the game forever.
                // To do this, we will inject the script into the Global Variable Space after
                // resizing it to accomodate our code. Because the global variable space is
                // stored each time the game is saved (and the game doesn't do any size checking),
                // any modifications to the global variable space will persist.

                // First, let's write our script code! This example shows how we can override the
                // ScriptCommand class to support specific command types. It also demonstrates one
                // way to deal with jump address calculation.

                // We will divide our code into blocks; the start of each block is essentially a jump label.
                // We will address each block relatively when writing GOTO statements and resolve the block
                // addresses at the end while we create the final bytecode.

                // Let's get to it!

                const int PlayerChar = 132;
                const int ModelBanshee = 119;
                const int ModelRhino = 122;
                const int ModelBfInjection = 114;
                const int ModelBorgnine = 148;
                const int ModelFbiCar = 107;

                ScriptCommand[][] codeBlocks = new ScriptCommand[][]
                {
                    new ScriptCommand[]
                    {                                                       // :IDLE_LOOP
                        new Wait(0),                                        // 0001: wait 0
                        new IfAnd(2),                                       // 00D6: if and
                            new IsPlayerPlaying(PlayerChar),                // 0256:    player $PLAYER_CHAR defined
                            new CanPlayerStartMission(PlayerChar),          // 03EE:    player $PLAYER_CHAR controllable
                        new GotoIfFalse(0),                                 // 004D: goto_if_false @IDLE_LOOP
                        new IfAnd(2),                                       // 00D6: if and
                            new IsButtonPressed(0, Button.LeftShoulder1),   // 00E1:    pad 0 button_pressed 4
                            new IsButtonPressed(0, Button.DPadDown),        // 00E1:    pad 0 button_pressed 9
                        new GotoIfFalse(0),                                 // 004D: goto_if_false @IDLE_LOOP
                        new SetPlayerControl(PlayerChar, false),            // 01B4: set_player $PLAYER_CHAR can_move 0
                        new SetPlayerHeading(PlayerChar, 0.0f),             // 0171: set_player $PLAYER_CHAR z_angle_to 0.0
                        new SetCameraBehindPlayer(),                        // 0373: set_camera_directly_behind_player
                        new Goto(4),                                        // 0002: goto @MENU_MAIN_LOOP
                    },
                    new ScriptCommand[]
                    {                                                       // :SPAWN_CAR
                        new RequestModel(1),                                // 0247: request_model 1@
                    },
                    new ScriptCommand[]
                    {                                                       // :SPAWN_CAR_LOOP
                        new Wait(0),                                        // 0000: wait 0
                        new If(),                                           // 00D6: if
                            new HasModelLoaded(1),                          // 0248:    model 1@ available
                        new GotoIfFalse(0),                                 // 004D: goto_if_false @SPAWN_CAR_LOOP
                        new GetPlayerCoords(PlayerChar, 3, 4, 5),           // 0054: store_player $PLAYER_CHAR position_to 3@ 4@ 5@
                        new AddValToLocal(4, 5.0f),                         // 000B: 4@ += 5.0
                        new AddValToLocal(5, 0.5f),                         // 000B: 5@ += 0.5
                        new CreateCar(6, 1, 3, 4, 5),                       // 00A5: 6@ = create_car 1@ at 3@ 4@ 5@
                        new SetCarHeading(6, 90.0f),                        // 0175: set_car 6@ z_angle_to 90.0
                        new ReleaseModel(1),                                // 0249: release_model 1@
                        new ReleaseCar(6),                                  // 01C3: mark_car_as_no_longer_needed 6@
                    },
                    new ScriptCommand[]
                    {                                                       // :MENU_EXIT
                        new SetPlayerControl(PlayerChar, true),             // 01B4: set_player $PLAYER_CHAR can_move 1
                        new Goto(-3)                                        // 0002: goto @IDLE_LOOP
                    },
                    new ScriptCommand[]
                    {                                                       // :MENU_MAIN_LOOP
                        new Wait(0),                                        // 0001: wait 0
                        new If(),                                           // 00D6: if
                            new IsButtonPressed(0, Button.Cross),           // 00E1:    pad 0 button_pressed 16
                        new GotoIfTrue(-3),                                 // 004C: goto_if_true @SPAWN_CAR
                        new If(),                                           // 00D6: if
                            new IsButtonPressed(0, Button.Triangle),        // 00E1:    pad 0 button_pressed 15
                        new GotoIfTrue(-1),                                 // 004C: goto_if_true @MENU_EXIT
                        new If(),                                           // 00D6: if
                            new IsButtonPressed(0, Button.DPadLeft),        // 00E1:    pad 0 button_pressed 10
                        new GotoIfTrue(1),                                  // 004C: goto_if_true @CAR_SELECT_DEC
                        new If(),                                           // 00D6: if
                            new IsButtonPressed(0, Button.DPadRight),       // 00E1:    pad 0 button_pressed 11
                        new GotoIfTrue(2),                                  // 004C: goto_if_true @CAR_SELECT_INC
                        new Goto(3),                                        // 0002: goto @CAR_SELECT_BOUNDS
                    },
                    new ScriptCommand[]
                    {                                                       // :CAR_SELECT_DEC
                        new SubValFromIntLocal(0, 1),                       // 000E: 0@ -= 1
                        new Goto(2),                                        // 0002: goto @CAR_SELECT_BOUNDS
                    },
                    new ScriptCommand[]
                    {                                                       // :CAR_SELECT_INC
                        new SubValFromIntLocal(0, 1),                       // 000A: 0@ += 1
                        new Goto(1),                                        // 0002: goto @CAR_SELECT_BOUNDS
                    },
                    new ScriptCommand[]                                     // :CAR_SELECT_BOUNDS
                    {                                                       // :CAR_SELECT_BOUNDS_MAX
                        new If(),                                           // 00D6: if
                            new IsIntLocalGreaterThanValue(0, 4),           // 0019:    0@ > 4
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @CAR_SELECT_BOUNDS_MIN
                        new SetLocalInt(0, 0),                              // 0006: 0@ = 0
                    },
                    new ScriptCommand[]
                    {                                                       // :CAR_SELECT_BOUNDS_MIN
                        new If(),                                           // 00D6: if
                            new IsValGreaterThanIntLocal(0, 0),             // 001B:    0 > 0@
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @VEHICLE_0
                        new SetLocalInt(0, 4)                               // 0006: 0@ = 4
                    },
                    new ScriptCommand[]
                    {                                                       // :VEHICLE_0
                        new If(),                                           // 00D6: if
                            new IsIntLocalEqualToVal(0, 0),                 // 0039:    0@ == 0
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @VEHICLE_1
                        new PrintBig("BANSHEE", 250, 6),                    // 00BA: text_styled 'BANSHEE' 250 ms 6
                        new SetLocalInt(1, ModelBanshee),                   // 0006: 1@ = #BANSHEE
                    },
                    new ScriptCommand[]
                    {                                                       // :VEHICLE_1
                        new If(),                                           // 00D6: if
                            new IsIntLocalEqualToVal(0, 1),                 // 0039:    0@ == 1
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @VEHICLE_2
                        new PrintBig("BORGNIN", 250, 6),                    // 00BA: text_styled 'BORGNIN' 250 ms 6
                        new SetLocalInt(1, ModelBorgnine),                  // 0006: 1@ = #BORGNINE
                    },
                    new ScriptCommand[]
                    {                                                       // :VEHICLE_2
                        new If(),                                           // 00D6: if
                            new IsIntLocalEqualToVal(0, 2),                 // 0039:    0@ == 2
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @VEHICLE_3
                        new PrintBig("BFINJC", 250, 6),                     // 00BA: text_styled 'BFINJC' 250 ms 6
                        new SetLocalInt(1, ModelBfInjection),               // 0006: 1@ = #BFINJECT
                    },
                    new ScriptCommand[]
                    {                                                       // :VEHICLE_3
                        new If(),                                           // 00D6: if
                            new IsIntLocalEqualToVal(0, 3),                 // 0039:    0@ == 3
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @VEHICLE_4
                        new PrintBig("FBICAR", 250, 6),                     // 00BA: text_styled 'FBICAR' 250 ms 6
                        new SetLocalInt(1, ModelFbiCar),                    // 0006: 1@ = #FBICAR
                    },
                    new ScriptCommand[]
                    {                                                       // :VEHICLE_4
                        new If(),                                           // 00D6: if
                            new IsIntLocalEqualToVal(0, 4),                 // 0039:    0@ == 4
                        new GotoIfFalse(1),                                 // 004D: goto_if_false @KEYUP_LOOP
                        new PrintBig("RHINO", 250, 6),                      // 00BA: text_styled 'RHINO' 250 ms 6
                        new SetLocalInt(1, ModelRhino),                     // 0006: 1@ = #RHINO
                    },
                    // You could add more cars here, just make sure you
                    // keep track of your GOTO offsets!!
                    new ScriptCommand[]
                    {                                                       // :KEYUP_LOOP
                        new IfOr(2),                                        // 00D6: if or
                            new IsButtonPressed(0, Button.DPadLeft),        // 00E1:    pad 0 key_pressed 10
                            new IsButtonPressed(0, Button.DPadRight),       // 00E1:    pad 0 key_pressed 11
                        new GotoIfFalse(-10),                               // 004D: goto_if_false @MENU_MAIN_LOOP
                        new Wait(75),                                       // 0001: wait 75
                        new Goto(0)                                         // 0002: goto @KEYUP_LOOP
                    }
                };

                var scr = s.Script;

                // Now it's time to resolve the block addresses.
                // First, run through the code and compute the absolute address
                // of each block.

                int baseAddr = scr.GetGlobalVariableSpaceSize();
                int addr = baseAddr;
                var blockAddrs = new List<int>();

                foreach (var block in codeBlocks)
                {
                    blockAddrs.Add(addr);
                    foreach (var cmd in block)
                    {
                        addr += cmd.Compile().Length;
                    }
                }

                using MemoryStream buf = new MemoryStream();
                using BinaryWriter w = new BinaryWriter(buf);

                // Now we go back through the code and replace each relative GOTO address
                // with the absolute address we computed in the last step. This is required
                // because GTA3s script processor does not support relative addressing for
                // non-mission scripts. (and we can't inject non-mission scripts due to the
                // way the game loads them from MAIN.SCM).

                addr = baseAddr;
                for (int i = 0; i < codeBlocks.Length; i++)
                {
                    foreach (var cmd in codeBlocks[i])
                    {
                        if (cmd.IsJump())
                        {
                            var arg = cmd.Args[0];
                            int blockOffset = arg.Value;
                            arg.Value = blockAddrs[i + blockOffset];
                        }

                        byte[] code = cmd.Compile();
                        Console.WriteLine($"{addr}:  {cmd}");
                        w.Write(code);
                        addr += code.Length;
                    }
                }

                // We've got our final bytecode! All that's left is to inject it into the
                // script space and start a new RunningScript to kick it off!

                byte[] bytecode = buf.ToArray();
                int newSize = scr.GrowScriptSpace(DataBuffer.Align4(bytecode.Length));
                scr.ScriptSpace.CopyFrom(bytecode, baseAddr);
                scr.SetGlobalVariableSpaceSize(newSize);        // This is how we make the script persist.

                Console.WriteLine($"{addr - baseAddr} bytes written to script at address 0x{baseAddr:X4}.");

                var newScript = scr.StartNewScript(baseAddr);
                newScript.Name = "carspwn";
            }

            // ------------------------------ END MODIFICATIONS -------------------------------


            string dstPath = Support.GetSaveSlot(3);
            int numWritten = s.Save(dstPath);

            Console.WriteLine($"{numWritten} bytes written to '{dstPath}'.");
        }
    }
}
