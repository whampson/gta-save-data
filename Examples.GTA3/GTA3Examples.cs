using GTASaveData.GTA3;
using System;

namespace Examples.GTA3
{
    public static class GTA3Examples
    {
        static void Main(string[] args)
        {
            // Grab a file from our test data suite.
            string srcPath = Support.GetFileFromTestData("RC3", GTA3Save.FileTypes.PC);


            // Load it!
            using GTA3Save save = GTA3Save.Load(srcPath);   // GTA3Save implements IDisposable,
                                                            // so be sure to use the 'using' statement
                                                            // when working with it.
            // Modify some stuff.
            SetSaveTitleAndTimeStamp(save);
            TweakGameSettings(save);
            PassMissions(save);
            InjectTestScript(save);


            // Save it!
            string dstPath = Support.GetSaveSlot(1);
            int numWritten = save.Save(dstPath);

            Console.WriteLine($"{numWritten} bytes written to '{dstPath}'.");
        }

        static void SetSaveTitleAndTimeStamp(GTA3Save s)
        {
            s.Title = "~b~EXAMPLE";             // ~b~ sets the color to blue!
            s.TimeStamp = new DateTime(2015, 10, 21, 19, 28, 0);

            // The title may also be set via 'SimpleVars.LastMissionPassedName'
            // and time stamp via 'SimpleVars.TimeStamp'
        }

        static void TweakGameSettings(GTA3Save s)
        {
            // Game settings and generic world params are stored in 'SimpleVars'
            var simp = s.SimpleVars;

            simp.GameClockHours = 18;
            simp.GameClockMinutes = 30;
            simp.MillisecondsPerGameMinute = 5000;
            simp.ForcedWeatherType = WeatherType.Sunny;
            simp.CameraModeInCar = CameraMode.Far;
            simp.CameraModeOnFoot = CameraMode.Far;

            // On PS2, there is no gta3.set; all the game settings are stored in SimpleVars.
            // On PC, these do nothing.
            if (s.IsPS2)
            {
                simp.Brightness = 255;
                simp.BlurOn = true;                 // trails!
                simp.MusicVolume = 63;
                simp.SfxVolume = 127;
                simp.ShowSubtitles = false;
                simp.UseWideScreen = true;
                simp.UseVibration = true;
                simp.Language = Language.German;    // this will enable censorship on PAL version,
                                                    // will crash NTSC version since GERMAN.GXT is not on the disk
            }
        }

        static void PassMissions(GTA3Save s)
        {
            if (s.Scripts.MainScriptSize == 108797)         // PC/PS2 (v2) SCM
            {
                // Sayonara Salvatore is hard, let's skip it!
                // Keep it as transparent as possible so it looks like it was done normally.
                {
                    s.Stats.MissionsGiven += 1;                                     // 0317: increment_mission_attempts
                    s.Scripts.SetGlobal(316, 1);                                    // $FLAG_ASUKA_MISSION1_PASSED = 1
                    s.PlayerInfo.GiveMoney(25000);                                  // 0109: player $PLAYER_CHAR money += 25000
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
                    s.Scripts.StartNewScript(50022);                                // 004F: start_new_script @NONAME_42
                    s.Scripts.StartNewScript(61388);                                // 004F: start_new_script @JOE_BUG
                    s.Scripts.StartNewScript(63065);                                // 004F: start_new_script @YARD_PH
                    s.Scripts.StartNewScript(63327);                                // 004F: start_new_script @ASUK_DR
                }
            }
        }

        static void InjectTestScript(GTA3Save s)
        {
            // It is possible to executable inject SCM code into save files via
            // the 'Scripts.ScriptSpace' array. This space is normally used for
            // the global variables, but the game does not to any size checking
            // when loading this array (it simply overwrites MAIN.SCM starting
            // from offset 0), making it possible for us to utilize this space
            // for injecting our own SCM code! As far as I know, it works on all
            // versions of GTA3 (even PS2!).

            var scr = s.Scripts;

            // First, grow the script space a bit to make room for our new code.
            // You can also overwrite any global variables you choose, but if
            // you don't want to mess up the story mode missions, it's best to
            // expand the script space for your code.
            scr.ResizeScriptSpace(44);

            // The first 8 bytes of ScriptSpace are reserved for the purpose of
            // telling the game how big the global variable space is (this is why
            // you can't use global variables 0 and 1). Thus, expanding the script
            // space does not affect the size of the global variable space. You can
            // add global variables by expanding the script space, then setting the
            // variable space size via SetSizeOfVariableSpace(int). We're not doing
            // that in this example, instead we will use the end of the variable
            // space as the base address for our custom script.
            int baseAddr = scr.GetSizeOfVariableSpace();

            // Now for the fun part: writing raw SCM code!
            // Each command consists of a 2-byte opcode and a predefined number
            // of arguments (usually) depending on the opcode being used. String
            // arguments (e.g. GXT strings) always occupy 8 bytes. For numeric
            // arguments, a 1-byte type specifier is included followed by a 1- to
            // 4-byte argument depending on the type.
            // Type specifiers are as follows:
            //   1 = INT32      (4 bytes)
            //   2 = GLOBALVAR  (2 bytes)
            //   3 = LOCALVAR   (2 bytes)
            //   4 = INT8       (1 bytes)
            //   5 = INT16      (2 bytes)
            //   6 = FLOAT      (2 bytes)
            int addr = baseAddr;
            {                                                               // 0001: wait 10000
                addr += scr.Write2BytesToScript(addr, 0x0001);
                addr += scr.Write1ByteToScript(addr, 5);
                addr += scr.Write2BytesToScript(addr, 10000);
            }
            {                                                               // 00BA: text_styled 'YD4_C' 10000 ms 4
                addr += scr.Write2BytesToScript(addr, 0x00BA);
                addr += scr.WriteTextLabelToScript(addr, "YD4_C");
                addr += scr.Write1ByteToScript(addr, 5);
                addr += scr.Write2BytesToScript(addr, 10000);
                addr += scr.Write1ByteToScript(addr, 4);
                addr += scr.Write1ByteToScript(addr, 4);
            }
            {                                                               // 0001: wait 10000
                addr += scr.Write2BytesToScript(addr, 0x0001);
                addr += scr.Write1ByteToScript(addr, 5);
                addr += scr.Write2BytesToScript(addr, 10000);
            }
            {                                                               // 00BA: text_styled 'YD4_D' 5000 ms 4
                addr += scr.Write2BytesToScript(addr, 0x00BA);
                addr += scr.WriteTextLabelToScript(addr, "YD4_D");
                addr += scr.Write1ByteToScript(addr, 5);
                addr += scr.Write2BytesToScript(addr, 5000);
                addr += scr.Write1ByteToScript(addr, 4);
                addr += scr.Write1ByteToScript(addr, 4);
            }
            addr += scr.Write2BytesToScript(addr, 0x004E);                  // 004E: terminate_this_script

            Console.WriteLine($"{addr - baseAddr} bytes written to script at address 0x{baseAddr:X4}.");

            // NOTE: you may also inject SCM bytecode by directly
            // writing to 'Scripts.ScriptSpace'

            // Last, create a new RunningScript pointing at the
            // base address to kick everything off!
            scr.StartNewScript(baseAddr);
        }
    }
}
