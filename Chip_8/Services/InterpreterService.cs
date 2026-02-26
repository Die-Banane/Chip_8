using System;
using System.Collections.Generic;
using Avalonia.Input;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Interpreters;
using Chip_8.ViewModels;

namespace Chip_8.Services;

public class InterpreterService(Keyboard keyboardService)
{
    private readonly Dictionary<Key, byte> _azertyKeyMap = new()
    {
        { Key.X, 0x0 },
        { Key.D1, 0x1 },
        { Key.D2, 0x2 },
        { Key.D3, 0x3 },
        { Key.A, 0x4 },
        { Key.Z, 0x5 },
        { Key.E, 0x6 },
        { Key.Q, 0x7 },
        { Key.S, 0x8 },
        { Key.D, 0x9 },
        { Key.W, 0xa },
        { Key.C, 0xb },
        { Key.D4, 0xc },
        { Key.R, 0xd },
        { Key.F, 0xe },
        { Key.V, 0xf }
    };
    
    private readonly Dictionary<Key, byte> _qwertzKeyMap = new()
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

    private readonly Dictionary<Key, byte> _qwertyKeyMap = new()
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

    public IInterpreter CreateInterpreter(InterpreterOptions options, DisplayBuffer displayBuffer)
    {
        CurrentInstance?.Dispose();
        
        Dictionary<Key, byte> keyMap;

        switch (options.Layout)
        {
            case KeyPadLayouts.Azerty:
                keyMap = _azertyKeyMap;
                keyboardService.KeyMap = keyMap;
                break;
            
            case KeyPadLayouts.Qwertz:
                keyMap = _qwertzKeyMap;
                keyboardService.KeyMap = keyMap;
                break;

            case KeyPadLayouts.Qwerty:
                keyMap = _qwertyKeyMap;
                keyboardService.KeyMap = keyMap;
                break;
            
            default:
                throw new InvalidOperationException();
        }
        
        switch (options.Version)
        {
            case Chip8Versions.Legacy:
                CurrentInstance = new LegacyInterpreter(displayBuffer, options.Path, keyMap, keyboardService, options.Frequency);
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