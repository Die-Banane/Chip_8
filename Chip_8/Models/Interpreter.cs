using System.IO;
using Avalonia.Input;
using System.Collections.Generic;
using System.Timers;
using System;

namespace Chip_8.Models;

internal class Interpreter
{
    private Timer _timer;

    private byte x, y, n, nn; // the nibbles
    private ushort nnn;
    
    ushort i, pc; // Index Register, Program Counter

    byte[] v = new byte[16]; // general purpose Registers
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

    public void Initialize(string program)
    {
        LoadProgram(program);

        for (int i = 0; i < font.Length; i++) // loads the font in memory
        {
            memory[0x50 + i] = font[i];
        }

        pc = 0x200;

        stack = new();

        _timer = new(16.6);
        _timer.Elapsed += DecrementTimers;
        _timer.Start();
    }

    public void Execute()
    {
        while (true)
        {
            ushort instruction = Fetch();

            switch (instruction & 0xf000) // get first nibble
            {
                case 0x0000:
                    switch (nn)
                    {
                        case 0x00e0:
                            Array.Fill<byte>(Display.Buffer, 0);
                            break;

                        case 0x00ee:
                            //subroutines
                            break;
                    }
                    break;

                case 0x1000:
                    pc = nnn;
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
                    v[x] = nn;
                    break;

                case 0x7000:
                    v[x] += nn;
                    break;

                case 0x8000:
                    break;

                case 0x9000:
                    break;

                case 0xa000:
                    i = nnn;
                    break;

                case 0xb000:
                    break;

                case 0xc000:
                    break;

                case 0xd000:
                    Draw();
                    break;

                case 0xe000:
                    break;

                case 0xf000:
                    break;
            }
        }
    }

    private ushort Fetch()
    {
        ushort instruction = (ushort)(memory[pc] << 8 | memory[pc + 1]);
        pc += 2;

        x = (byte)((instruction & 0x0f00) >> 8);
        y = (byte)((instruction & 0x00f0) >> 4);
        n = (byte)(instruction & 0x000f);
        nn = (byte)(instruction & 0x00ff);
        nnn = (ushort)(instruction & 0x0fff);

        return instruction;
    }

    private void DecrementTimers(object? sender, ElapsedEventArgs e)
    {
        delay = delay > 0 ? (byte)(delay - 1) : (byte)0;
        sound = sound > 0 ? (byte)(sound - 1) : (byte)0;
    }

    private void LoadProgram(string program)
    {
        byte[] instructions = File.ReadAllBytes(program);

        for (int i = 0; i < instructions.Length; i++)
        {
            memory[0x200 + i] = instructions[i];
        }
    }

    private void Draw()
    {
        int xCoord = v[x] & 63;
        int yCoord = v[y] & 31;

        v[0xf] = 0;

        for (int j = 0; j < n; j++)
        {
            byte row = memory[i + j];

            foreach (var bit in ByteToBooleans(row))
            {
                if (xCoord < 64)
                {
                    if (Display.Buffer[yCoord * Display.Width + xCoord] == 255 && bit)
                    {
                        Display.Buffer[yCoord * Display.Width + xCoord] = 0;
                        v[0xf] = 1;
                    }
                    else if (Display.Buffer[yCoord * Display.Width + xCoord] == 0 && bit)
                    {
                        Display.Buffer[yCoord * Display.Width + xCoord] = 255;
                    }
                }

                xCoord++;
            }

            xCoord -= 8;
            yCoord++;
        }
    }

    private bool[] ByteToBooleans(byte value)
    {
        bool[] result = new bool[8];

        result[7] = (value & 0x01) == 0x01;
        result[6] = (value & 0x02) == 0x02;
        result[5] = (value & 0x04) == 0x04;
        result[4] = (value & 0x08) == 0x08;
        result[3] = (value & 0x10) == 0x10;
        result[2] = (value & 0x20) == 0x20;
        result[1] = (value & 0x40) == 0x40;
        result[0] = (value & 0x80) == 0x80;

        return result;
    }
}