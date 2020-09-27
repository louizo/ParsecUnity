﻿using UnityEngine.InputSystem;
using System.Collections.Generic;
using ParsecGaming;

public static class ParsecInputSystemMapping
{
    public static readonly Dictionary<Parsec.ParsecKeycode, Key> Keys = new Dictionary<Parsec.ParsecKeycode, Key>()
    {
        { Parsec.ParsecKeycode.KEY_A, Key.A },
        { Parsec.ParsecKeycode.KEY_B, Key.B },
        { Parsec.ParsecKeycode.KEY_C, Key.C },
        { Parsec.ParsecKeycode.KEY_D, Key.D },
        { Parsec.ParsecKeycode.KEY_E, Key.E },
        { Parsec.ParsecKeycode.KEY_F, Key.F },
        { Parsec.ParsecKeycode.KEY_G, Key.G },
        { Parsec.ParsecKeycode.KEY_H, Key.H },
        { Parsec.ParsecKeycode.KEY_I, Key.I },
        { Parsec.ParsecKeycode.KEY_J, Key.J },
        { Parsec.ParsecKeycode.KEY_K, Key.K },
        { Parsec.ParsecKeycode.KEY_L, Key.L },
        { Parsec.ParsecKeycode.KEY_M, Key.M },
        { Parsec.ParsecKeycode.KEY_N, Key.N },
        { Parsec.ParsecKeycode.KEY_O, Key.O },
        { Parsec.ParsecKeycode.KEY_P, Key.P },
        { Parsec.ParsecKeycode.KEY_Q, Key.Q },
        { Parsec.ParsecKeycode.KEY_R, Key.R },
        { Parsec.ParsecKeycode.KEY_S, Key.S },
        { Parsec.ParsecKeycode.KEY_T, Key.T },
        { Parsec.ParsecKeycode.KEY_U, Key.U },
        { Parsec.ParsecKeycode.KEY_V, Key.V },
        { Parsec.ParsecKeycode.KEY_W, Key.W },
        { Parsec.ParsecKeycode.KEY_X, Key.X },
        { Parsec.ParsecKeycode.KEY_Y, Key.Y },
        { Parsec.ParsecKeycode.KEY_Z, Key.Z },

        { Parsec.ParsecKeycode.KEY_1, Key.Digit1 },
        { Parsec.ParsecKeycode.KEY_2, Key.Digit2 },
        { Parsec.ParsecKeycode.KEY_3, Key.Digit3 },
        { Parsec.ParsecKeycode.KEY_4, Key.Digit4 },
        { Parsec.ParsecKeycode.KEY_5, Key.Digit5 },
        { Parsec.ParsecKeycode.KEY_6, Key.Digit6 },
        { Parsec.ParsecKeycode.KEY_7, Key.Digit7 },
        { Parsec.ParsecKeycode.KEY_8, Key.Digit8 },
        { Parsec.ParsecKeycode.KEY_9, Key.Digit9 },
        { Parsec.ParsecKeycode.KEY_0, Key.Digit0 },

        { Parsec.ParsecKeycode.KEY_SPACE, Key.Space },
        { Parsec.ParsecKeycode.KEY_ENTER, Key.Enter },
        { Parsec.ParsecKeycode.KEY_TAB, Key.Tab },
        { Parsec.ParsecKeycode.KEY_LSHIFT, Key.LeftShift },
        { Parsec.ParsecKeycode.KEY_RSHIFT, Key.RightShift },
        { Parsec.ParsecKeycode.KEY_LALT, Key.LeftAlt },
        { Parsec.ParsecKeycode.KEY_RALT, Key.RightAlt },
        { Parsec.ParsecKeycode.KEY_LCTRL, Key.LeftCtrl },
        { Parsec.ParsecKeycode.KEY_RCTRL, Key.RightCtrl }
    };
}