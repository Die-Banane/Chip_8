using System;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Interpreters;
using Chip_8.ViewModels;

namespace Chip_8.Services;

public class InterpreterService(Keyboard keyboardService)
{
    public IInterpreter BuildInterpreter(InterpreterOptions options, DisplayBuffer displayBuffer)
    {
        switch (options.Layout)
        {
            case KeyPadLayouts.Azerty:
                keyboardService.KeyMap = Chip8.AzertyKeyMap;
                break;
            
            case KeyPadLayouts.Qwertz:
                keyboardService.KeyMap = Chip8.QwertzKeyMap;
                break;

            case KeyPadLayouts.Qwerty:
                keyboardService.KeyMap = Chip8.QwertyKeyMap;
                break;
            
            default:
                throw new InvalidOperationException();
        }
        
        switch (options.Version)
        {
            case Chip8Versions.Legacy:
                return new LegacyInterpreter(displayBuffer, options.Path, keyboardService, options.Frequency);
            
            case Chip8Versions.SuperChip:
                return new SuperInterpreter();
            
            case Chip8Versions.XoChip:
                return new XoInterpreter();
            
            default:
                throw new InvalidOperationException();
        }
    }
}