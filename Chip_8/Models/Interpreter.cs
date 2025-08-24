using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Timers;

namespace Chip_8.Models;

internal class Interpreter
{
    private Timer _timer;
    
    ushort i, pc; // Index Register, Program Counter

    byte v0, v1, v2, v3, v4, v5, v6, v7, v8, v9, vA, vB, vC, vD, vE, vF; // general purpose Registers
    byte delay, sound; 

    byte[] memory = new byte[4096];

    Stack<byte> stack;

    private readonly byte[] font =
    {
        0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
        0x20, 0x60, 0x20, 0x20, 0x70, // 1
        0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
        0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
        0x90, 0x90, 0xF0, 0x10, 0x10, // 4
        0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
        0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
        0xF0, 0x10, 0x20, 0x40, 0x40, // 7
        0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
        0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
        0xF0, 0x90, 0xF0, 0x90, 0x90, // A
        0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
        0xF0, 0x80, 0x80, 0x80, 0xF0, // C
        0xE0, 0x90, 0x90, 0x90, 0xE0, // D
        0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
        0xF0, 0x80, 0xF0, 0x80, 0x80  // F
    };

    public readonly Dictionary<Key, byte> keyMap = new()
    {
        {Key.D1, 0x0},
        {Key.D2, 0x1},
        {Key.D3, 0x2},
        {Key.D4, 0x3},
        {Key.Q, 0x4},
        {Key.W, 0x5},
        {Key.E, 0x6},
        {Key.R, 0x7},
        {Key.A, 0x8},
        {Key.S, 0x9},
        {Key.D, 0xa},
        {Key.F, 0xb},
        {Key.Z, 0xc},
        {Key.X, 0xd},
        {Key.C, 0xe},
        {Key.V, 0xf},

    };

    public void Initialize()
    {
        _timer = new(16.6);
        _timer.Elapsed += DecrementTimer;
        _timer.Start();
        
        pc = 0x200;

        stack = new();
        
        for (int i = 0; i < font.Length; i++)
        {
            memory[0x50 + i] = font[i];
        }
    }

    private void loop()
    {
        ushort instruction = Fetch();

        switch (instruction & 0xf000) // get first nibble
        {
            case 0x0000:
                break;

            case 0x1000:
                break;

            case 0x2000:
                break;

            case 0x3000:
                break;

            case 0x4000:
                break;

            case 0x5000:
                break;

            case 0x6000:
                break;

            case 0x7000:
                break;

            case 0x8000:
                break;

            case 0x9000:
                break;

            case 0xa000:
                break;

            case 0xb000:
                break;

            case 0xc000:
                break;

            case 0xd000:
                break;

            case 0xe000:
                break;

            case 0xf000:
                break;
        }
    }

    private ushort Fetch()
    {
        ushort instruction = (ushort)(memory[pc] << 8 | memory[pc + 1]);
        pc += 2;
        return instruction;
    }

    private void DecrementTimer(object sender, ElapsedEventArgs e)
    {
        delay = delay > 0 ? (byte)(delay - 1) : (byte)0;
        sound = sound > 0 ? (byte)(sound - 1) : (byte)0;
    }
}