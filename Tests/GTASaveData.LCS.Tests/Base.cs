using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.LCS.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T, LCSSaveParams>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.Mobile32) },
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.Mobile64) },
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.PS2) },
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.PSP) },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "MAC3" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "VIC1" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "VIC1 2" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 PackingHeat 1" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 PackingHeat 2" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 FromLaptop" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 AamBaub 1" },    // https://libertycity.net/files/gta-liberty-city-stories/112486-100-save-for-gta-liberty-city-stories-ios-android.html
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 100Percent CP" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "SALH5 AamBaub 9" },    // https://libertycity.net/files/gta-liberty-city-stories/112486-100-save-for-gta-liberty-city-stories-ios-android.html
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "MAC3 CP SetSave" },
            new object[] { LCSSave.FileTypes.Mobile32, ScriptVersion.Android, "VIC1 CartmanKusanagi" },
            new object[] { LCSSave.FileTypes.Mobile64, ScriptVersion.iOS, "MAR4" },
            new object[] { LCSSave.FileTypes.Mobile64, ScriptVersion.iOS, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.Mobile64, ScriptVersion.iOS, "VIC1" },
            new object[] { LCSSave.FileTypes.Mobile64, ScriptVersion.iOS, "VIC1 StarterSave" },
            new object[] { LCSSave.FileTypes.Mobile64, ScriptVersion.iOS, "VIC1 HomeSweetHome Vehicles" },
            new object[] { LCSSave.FileTypes.Mobile64, ScriptVersion.Android, "SALH5 100Percent 2" },  // https://gtaforums.com/topic/961634-gta-liberty-city-stories-iosandroid-100-save-game-updated/
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "NEDS4" },
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 SpecialVehicles" },
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1" },
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 Militia 1" },
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 Militia 2" },
            // Starter Save Pack + Final GAME (libertycity.net)
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 HomeSweetHome" },        // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "RAYS5 MunitionsDump" },       // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SAL7 TroubleWithTriads" },    // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SAL8 DrivingMrLeone" },       // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SAL8 DrivingMrLeone 2" },     // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH4 ShoresideRedemption" }, // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 SicilianGambit" },      // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 SicilianGambit EU" },   // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/16421
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "TOSH4 CashInKazukisChip" },   // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 AVolatileSituation" },   // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 HomeSweetHome 2" },      // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 HomeSweetHome 3" },      // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 HomeSweetHome 4" },      // https://libertycity.net/files/gta-liberty-city-stories/218327-gta-lcs-starter-save-pack-ps2.html
            // CodeBreaker / AR Max saves (GameFAQs)
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 AllSideMissions" },      // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/12185
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 FBICar" },              // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/12227
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 PoliceHeli EU" },       // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/12402
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 ARMax 1" },             // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/12413
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 ARMax 2" },             // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/12413
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "VIC1 MasterSave" },           // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/24852
            new object[] { LCSSave.FileTypes.PS2, ScriptVersion.PS2, "SALH5 MaxAmmo" },             // https://gamefaqs.gamespot.com/ps2/931275-grand-theft-auto-liberty-city-stories/saves/27298
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "MAR3" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "MAR3 BoobyPrize" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "NEDS1 LCConfidential" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SAL8 DrivingMrLeone" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 GlitchedVehicles" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 GlitchedVehicles EU" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 RCBandit" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 Panlantic" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 Rhino" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 TinyInvisibleHeli" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 TinyInvisibleHeli EU" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 Hunter" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 Dodo" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 Dodo EU" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 GangVehicles" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 BPHelicopters" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 BPHelicopters EU" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 Maverick" },
            // PSP percentage / mid-game saves (GameFAQs + libertycity.net)
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 2Percent" },                 // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10261
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 5Percent" },                 // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/25315
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "DONS2 SteeringTheVote" },         // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9689
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "RAYS4 NightOfTheLividDreads" },   // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10346
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 43Percent" },               // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/12292
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 44Percent" },               // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9702
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 50Percent" },                // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10331
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 50Percent" },               // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/12779
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 51Percent" },               // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/32802
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 80Percent" },               // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9920
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU" },           // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10585
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 2" },         // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/12645
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 48Percent" },                // https://libertycity.net/files/gta-liberty-city-stories/233912-17-save-files-with-special-vehicles.html
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 49Percent" },                // https://libertycity.net/files/gta-liberty-city-stories/224671-starter-and-100-saves-for-gta-lcs-pc.html
            // GameFAQs PSP saves (full collection; see URL + description in each comment)
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "DONS1 34Percent 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9701 -- Just got to Shoreside Vale, Money=$53795, 34% Complete. Weapons=Bare Knuckles, Katana, Motolovs,...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 17" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9736 -- 100% Enjoy
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SAL8 PortlandComplete EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9785 -- All missions for Portland completed. Starts from safehouse in Staunton Island ready for all those...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH2 35Percent EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9786 -- Shoreside Vale unlocked. Ready for missions on that island to be completed. No cheats used. 35% c...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 42Percent EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9787 -- All Story Mission Completed , 42% , Day 74 , $131000480. Cheetah Infernus and V8 Ghost stored in...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SAL6 CalmBeforeStorm 3" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9818 -- Saved right before Calm Before the Storm, so you can fly the helicopter
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 18" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9921 -- 100% Completion. Great weapons, lots of ammo, awesome vehicles. Portland: Avenger BP; Staunton: L...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 40Percent EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9936 -- GTA: Liberty City Stories main missions complete about 40 %
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 20" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9977 -- 100%.$956,617.All Mission Completed.All Rampages,Packages and Unique Jumps Completed.Never Busted...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SAL8 19Percent EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/9982 -- Portland Complete 19% Day 21, 2 hidden packages $28793, Armour and health (full) No Staunton Isla...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 19" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10233 -- 100%; 95 game days passed; Never busted or wasted; no cheats used; Criminal Rating: Hitman; Highe...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SAL6 CalmBeforeStorm 2 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10318 -- Never death, Never busted, all stars evaded. Saved right before Clam Before Strom mission, PCJ600...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SAL8 23Percent 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10327 -- 23%, last mission: driving mr. leone, 40 hidden packages, $141,571, some side missions complete....
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 21" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10587 -- 100% Complete, Day 120, $55292225, Criminal Rating: Butcher (16908). No cheats used
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH1 33Percent EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/10696 -- 33% Complete - 1 Shoreside Mission Complete - $24792 Money
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 20" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/11598 -- 100% Complete No cheats used. Enjoy. ' The Associate ' Criminal Rating. Bullet Proof Cars in Stau...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 22" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/11682 -- All Missions Completed-Unlimeted Ammo-All Packages Found-And Rampages(M60)-Manchez in Shorside Va...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 25Percent 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/12221 -- Ultimate Game Save: 25%. Everything that can possibly be completed before the missions begin in P...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 23" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/12802 -- EU Saved Game - 100% completed - Black Cheetah & Infurnus at Staunton Garage - Black Hellenbach G...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 21" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/13092 -- BEST SAVE GAME HERE!!! 100% COMPLETE!!! Bulletproof cars in shorside garage, black cars in staunt...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 47Percent 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/14763 -- Stauton Island ,47%, day 110, $3543136 main missions complete cheats used
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH3 HeliInGarage EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/15351 -- Helicopter saved in garage, (use a bike to push it out) this save is NOT hacked. Enjoy!
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 24" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/16426 -- 100% Complete, no cheats used, for Australian Version
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 22" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/22338 -- This is a eu save and 100% completed with a Hunter Helicopter
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 25" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/23709 -- CHALLENGE SAVE FOR 100%
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 49Percent 3" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/28142 -- All available side missions completed early in the game.Game Completion- 49%Mission attempts - 30...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 23" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/28691 -- 100% completed
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 StarterSave 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/29865 -- Um starter save com todas as missões secundária disponíveis feitas no início do jogo.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 KingOutfit 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/30345 -- 3 savegames actually. Start with $932,000 and "The King" outfit with rare vehicles in every garag...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 26" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/30821 -- 100% Savegame finish, Saint'mark safehouse,M60 AND Molotovs Infinite and good weapons
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 27" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/30901 -- M60 Infinite and Granade Laucher Infinite 100% AND Good weapons safehouse From Saint'mark:)))
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 28" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/31103 -- 100% Completion.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent Japan 5" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/32533 -- 100% Complete!!
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 29" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/32801 -- Hello, here I leave my game at 100% with good weapons and . I have been using this game since Jun...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 24" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/32975 -- Hello, here I leave my game at 100% with good weapons and good empires. I have been using this ga...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 30" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/33812 -- Good Savegame Good Weapons No Cheats Good Money Saint'mark Safehouse
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 25" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/33866 -- Good Savegame Good Weapons No Cheats Good Money Saint'mark Safehouse
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 31" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34189 -- Savegame 100% Unlocked Everyone in finish and Chil Shoreshide Vale Safehouse,Some nice PCJ-600 mo...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent Japan 6" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34190 -- 100% Complete!!Enjoy
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 26" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34265 -- Savegame 100% Good Money Black cars await you in Shoreside Vale, Staunton Island, and Portland. E...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 32" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34273 -- Saint's Mark Safehouse 100% Complete, good money and weapons, enjoy.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "VIC1 49Percent EU 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34380 -- Starter Save- no cheats, 97;100 packages, 25;26 Stunt Jumps, 49% percent completion - Missing Thu...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 27" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34438 -- 100% Complete no Cheats,used,Good Weapons and Good Money Saint's Mark Safehouse and Infernus Yell...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 33" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34448 -- 100% Complete no Cheats,used,Good Weapons and Good Money Shoreshide Vale Safehouse and Infernus Y...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 HideoutVehicles Japan 2" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34471 -- First hideout: LandstalkerSecond hideout: Sentinel,StingerThird hideout: Stretch x3
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 28" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34634 -- Staunton Island Safehouse 100% Complete, good money and weapons, enjoy.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 34" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34668 -- 100% Good weapons and Good Money safehouse From Saint'mark:)))
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 35" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34792 -- Shoreshide Vale Safehouse 100% Complete, good money and weapons, enjoy.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 29" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/34828 -- Good Savegame Good Weapons Good Money Saint'mark Safehouse 100%
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent Japan 7" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/35290 -- 100% Complete!!Enjoy
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 30" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/35332 -- A 100% completion savefile for Grand Theft Auto Liberty City Stories.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent 31" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/35437 -- Good Savegame Good Weapons No Cheats Good Money Saint'mark Safehouse
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent EU 36" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/35445 -- Hello, here I leave my game at 100% with good weapons and . I have been using this game since Jun...
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "SALH5 100Percent Japan 8" },  // https://gamefaqs.gamespot.com/psp/925776-grand-theft-auto-liberty-city-stories/saves/35455 -- Radio is LCFR100% Complete!!Enjoy
            // PPSSPP saves (author's own; US raw + EU decrypted). Confirms 269916 = EU 3.00 Platinum and 270081 = EU 1.05 original.
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULUS10041 S0" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULUS10041 S1" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULUS10041 S2" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULUS10041 S3" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULUS10041 S7" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULES00151 S0" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULES00151 S1" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULES00151 S2" },
            new object[] { LCSSave.FileTypes.PSP, ScriptVersion.PSP, "ULJM05255 S0 Japan" },
        };
    }
}
