using System;
using System.Collections.Generic;
using Avalonia.Input;
using Chip_8.config;
using Chip_8.CustomControls;
using Chip_8.Interfaces;
using Chip_8.Interpreters;
using Chip_8.ViewModels;

namespace Chip_8.Services;

public class InterpreterService(Keyboard keyboardService)
{
    private readonly Dictionary<Key, byte> defaultKeyMap = new()
    {
        { Key.D0, 0x0 },
        { Key.D1, 0x1 },
        { Key.D2, 0x2 },
        { Key.D3, 0x3 },
        { Key.D4, 0x4 },
        { Key.D5, 0x5 },
        { Key.D6, 0x6 },
        { Key.D7, 0x7 },
        { Key.D8, 0x8 },
        { Key.D9, 0x9 },
        { Key.A, 0xa },
        { Key.B, 0xb },
        { Key.C, 0xc },
        { Key.D, 0xd },
        { Key.E, 0xe },
        { Key.F, 0xf }
    };
    
    private readonly Dictionary<Key, byte> qwertzKeyMap = new()
    {
        { Key.X, 0x0 },
        { Key.D1, 0x1 },
        { Key.D2, 0x2 },
        { Key.D3, 0x3 },
        { Key.Q, 0x4 },
        { Key.W, 0x5 },
        { Key.E, 0x6 },
        { Key.A, 0x7 },
        { Key.S, 0x8 },
        { Key.D, 0x9 },
        { Key.Y, 0xa },
        { Key.C, 0xb },
        { Key.D4, 0xc },
        { Key.R, 0xd },
        { Key.F, 0xe },
        { Key.V, 0xf }
    };

    private readonly Dictionary<Key, byte> qwertyKeyMap = new()
    {
        { Key.X, 0x0 },
        { Key.D1, 0x1 },
        { Key.D2, 0x2 },
        { Key.D3, 0x3 },
        { Key.Q, 0x4 },
        { Key.W, 0x5 },
        { Key.E, 0x6 },
        { Key.A, 0x7 },
        { Key.S, 0x8 },
        { Key.D, 0x9 },
        { Key.Z, 0xa },
        { Key.C, 0xb },
        { Key.D4, 0xc },
        { Key.R, 0xd },
        { Key.F, 0xe },
        { Key.V, 0xf }
    };
    
    public IInterpreter? CurrentInstance { get; private set; }

    public IInterpreter CreateInterpreter(InterpreterOptions options, Pixel[] displayBuffer)
    {
        CurrentInstance?.Dispose();
        
        Dictionary<Key, byte> keyMap;

        switch (options.Layout)
        {
            case KeyPadLayouts.Qwertz:
                keyMap = qwertzKeyMap;
                keyboardService.KeyMap = keyMap;
                break;

            case KeyPadLayouts.Qwerty:
                keyMap = qwertyKeyMap;
                keyboardService.KeyMap = keyMap;
                break;
            
            default:
                throw new InvalidOperationException();
        }
        
        switch (options.Version)
        {
            case Chip8Versions.Legacy:
                CurrentInstance = new LegacyInterpreter(displayBuffer, options.Path, keyMap, keyboardService);
                return CurrentInstance;
            
            case Chip8Versions.SuperChip:
                CurrentInstance = new SuperInterpreter();
                return CurrentInstance;
            
            case Chip8Versions.XoChip:
                CurrentInstance = new XoInterpreter();
                return CurrentInstance;
            
            default:
                throw new InvalidOperationException();
        }
    }
}