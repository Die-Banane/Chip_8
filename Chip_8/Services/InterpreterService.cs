using System;
using Chip_8.config;
using Chip_8.CustomControls;
using Chip_8.Interfaces;
using Chip_8.Interpreters;
using Chip_8.ViewModels;

namespace Chip_8.Services;

public class InterpreterService
{
    private IInterpreter? _currentInstance;

    public IInterpreter CreateInterpreter(InterpreterOptions options, Pixel[] displayBuffer)
    {
        _currentInstance?.Dispose();
        
        switch (options.Version)
        {
            case Chip8Versions.Legacy:
                _currentInstance = new LegacyInterpreter(displayBuffer, options.Path);
                return _currentInstance;
            
            case Chip8Versions.SuperChip:
                _currentInstance = new SuperInterpreter();
                return _currentInstance;
            
            case Chip8Versions.XoChip:
                _currentInstance = new XoInterpreter();
                return _currentInstance;
            
            default:
                throw new InvalidOperationException();
        }
    }
}