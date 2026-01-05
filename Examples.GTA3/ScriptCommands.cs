using System;
using System.Collections.Generic;
using System.IO;

namespace Examples.GTA3
{
    // This is a very crude implementation of specific script commands.
    // Only the bare necessities are implemented for the purposes of Example #3.
    // Should probably do something like the SannyBuilder library with classes
    // and such. Future project idea! :D

    public class Wait : ScriptCommand
    {
        public Wait(int waitTimeMs)
            : base(CommandType.WAIT)
        {
            AddInt(waitTimeMs);
        }
    }

    public class Goto : ScriptCommand
    {
        public Goto(int addr)
            : base(CommandType.GOTO)
        {
            DontResizeArgs = true;
            AddInt(addr);
        }
    }

    public class GotoIfTrue : ScriptCommand
    {
        public GotoIfTrue(int addr)
            : base(CommandType.GOTO_IF_TRUE)
        {
            DontResizeArgs = true;
            AddInt(addr);
        }
    }

    public class GotoIfFalse : ScriptCommand
    {
        public GotoIfFalse(int addr)
            : base(CommandType.GOTO_IF_FALSE)
        {
            DontResizeArgs = true;
            AddInt(addr);
        }
    }

    public class If : ScriptCommand
    {
        public If()
            : base(CommandType.ANDOR)
        {
            AddInt(0);
        }
    }

    public class IfAnd : ScriptCommand
    {
        public IfAnd(int numConditions)
            : base(CommandType.ANDOR)
        {
            AddInt(numConditions - 1);
        }
    }

    public class IfOr : ScriptCommand
    {
        public IfOr(int numConditions)
            : base(CommandType.ANDOR)
        {
            AddInt(20 + (numConditions - 1));
        }
    }

    public class SetLocalInt : ScriptCommand
    {
        public SetLocalInt(int local, int value)
            : base(CommandType.SET_LVAR_INT)
        {
            AddLocal(local);
            AddInt(value);
        }
    }

    public class AddValToLocal : ScriptCommand
    {
        public AddValToLocal(int local, int value)
            : base(CommandType.ADD_VAL_TO_INT_LVAR)
        {
            AddLocal(local);
            AddInt(value);
        }

        public AddValToLocal(int local, float value)
            : base(CommandType.ADD_VAL_TO_FLOAT_LVAR)
        {
            AddLocal(local);
            AddFloat(value);
        }
    }

    public class SubValFromIntLocal : ScriptCommand
    {
        public SubValFromIntLocal(int local, int value)
            : base(CommandType.SUB_VAL_FROM_INT_LVAR)
        {
            AddLocal(local);
            AddInt(value);
        }
    }

    public class IsIntLocalGreaterThanValue : ScriptCommand
    {
        public IsIntLocalGreaterThanValue(int local, int value)
            : base(CommandType.IS_INT_LVAR_GREATER_THAN_NUMBER)
        {
            AddLocal(local);
            AddInt(value);
        }
    }

    public class IsValGreaterThanIntLocal : ScriptCommand
    {
        public IsValGreaterThanIntLocal(int value, int local)
            : base(CommandType.IS_NUMBER_GREATER_THAN_INT_LVAR)
        {
            AddInt(value);
            AddLocal(local);
        }
    }

    public class IsIntLocalEqualToVal : ScriptCommand
    {
        public IsIntLocalEqualToVal(int local, int value)
            : base(CommandType.IS_INT_LVAR_EQUAL_TO_NUMBER)
        {
            AddLocal(local);
            AddInt(value);
        }
    }

    public class IsPlayerPlaying : ScriptCommand
    {
        public IsPlayerPlaying(int playerChar)
            : base(CommandType.IS_PLAYER_PLAYING)
        {
            AddGlobal(playerChar);
        }
    }

    public class CanPlayerStartMission : ScriptCommand
    {
        public CanPlayerStartMission(int playerChar)
            : base(CommandType.CAN_PLAYER_START_MISSION)
        {
            AddGlobal(playerChar);
        }
    }

    public class SetPlayerControl : ScriptCommand
    {
        public SetPlayerControl(int playerChar, bool canControl)
            : base(CommandType.SET_PLAYER_CONTROL)
        {
            AddGlobal(playerChar);
            AddInt((canControl) ? 1 : 0);
        }
    }

    public class SetPlayerHeading : ScriptCommand
    {
        public SetPlayerHeading(int playerChar, float heading)
            : base(CommandType.SET_PLAYER_HEADING)
        {
            AddGlobal(playerChar);
            AddFloat(heading);
        }
    }

    public class SetCameraBehindPlayer : ScriptCommand
    {
        public SetCameraBehindPlayer()
            : base(CommandType.SET_CAMERA_BEHIND_PLAYER)
        { }
    }

    public class GetPlayerCoords : ScriptCommand
    {
        public GetPlayerCoords(int playerChar, int xLocal, int yLocal, int zLocal)
            : base(CommandType.GET_PLAYER_COORDINATES)      // TODO: should also support globals
        {
            AddGlobal(playerChar);
            AddLocal(xLocal);
            AddLocal(yLocal);
            AddLocal(zLocal);
        }
    }

    public class IsButtonPressed : ScriptCommand
    {
        public IsButtonPressed(int pad, Button button)
            : base(CommandType.IS_BUTTON_PRESSED)
        {
            AddInt(pad);
            AddInt((int) button);
        }
    }

    public class PrintBig : ScriptCommand
    {
        public PrintBig(string key, int time, int style)
            : base(CommandType.PRINT_BIG)
        {
            AddText(key);
            AddInt(time);
            AddInt(style);
        }
    }

    public class RequestModel : ScriptCommand
    {
        public RequestModel(int local)          // TODO: should also support global and literal
            : base(CommandType.REQUEST_MODEL)
        {
            AddLocal(local);
        }
    }

    public class HasModelLoaded : ScriptCommand
    {
        public HasModelLoaded(int local)        // TODO: should also support global and literal
            : base(CommandType.HAS_MODEL_LOADED)
        {
            AddLocal(local);
        }
    }

    public class ReleaseModel : ScriptCommand
    {
        public ReleaseModel(int local)
            : base(CommandType.MARK_MODEL_AS_NO_LONGER_NEEDED)
        {
            AddLocal(local);
        }
    }

    public class CreateCar : ScriptCommand
    {
        public CreateCar(int carRefLocal, int modelLocal, int xLocal, int yLocal, int zLocal)
            : base(CommandType.CREATE_CAR)      // TODO: should also support global and literal
        {
            AddLocal(modelLocal);
            AddLocal(xLocal);
            AddLocal(yLocal);
            AddLocal(zLocal);
            AddLocal(carRefLocal);
        }
    }

    public class SetCarHeading : ScriptCommand
    {
        public SetCarHeading(int carRefLocal, float heading)
            : base(CommandType.SET_CAR_HEADING)
        {
            AddLocal(carRefLocal);
            AddFloat(heading);
        }
    }

    public class ReleaseCar : ScriptCommand
    {
        public ReleaseCar(int carRefLocal)
            : base(CommandType.MARK_CAR_AS_NO_LONGER_NEEDED)
        {
            AddLocal(carRefLocal);
        }
    }

    public enum Button
    {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        LeftShoulder1,
        LeftShoulder2,
        RightShoulder1,
        RightShoulder2,
        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,
        Start,
        Select,
        Square,
        Triangle,
        Cross,
        Circle,
        Leftshock,
        Rightshock
    }
}
