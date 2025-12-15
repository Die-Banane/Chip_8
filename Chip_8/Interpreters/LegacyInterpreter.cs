using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Input;
using Chip_8.CustomControls;
using Chip_8.Interfaces;
using Chip_8.Services;

namespace Chip_8.Interpreters;

public class LegacyInterpreter : IInterpreter
{
    private readonly Pixel[] _displayBuffer;
    private readonly string  _programPath;
    private readonly Random _random;
    private readonly Keyboard _keyboardService;
    
    private bool _executing;
    
    private ushort pc, i;
    private byte[] v = null!;
    
    private byte[] memory = null!, program = null!;

    private readonly byte[] font =
    [
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
    ];

    private Stack<ushort> stack = null!;

    private byte x, y, n, nn;
    private ushort nnn;
    
    //TODO: Timers
    //TODO: Keypads

    public Dictionary<Key, byte>? KeyMap { get; }

    public LegacyInterpreter(Pixel[] displayBuffer, 
        string programPath, 
        Dictionary<Key, byte>? keyMap,
        Keyboard keyboardService)
    {
        KeyMap = keyMap;
        _keyboardService = keyboardService;
        _programPath = programPath;
        _displayBuffer = displayBuffer;
        _random = new Random();
        
        Initialize();
    }
    
    private void Initialize()
    {
        pc = 0x200;
        v = new byte[16];
        
        memory = new byte[4069];
        stack = new Stack<ushort>();
        
        font.CopyTo(memory, 0x50);

        program = File.ReadAllBytes(_programPath);

        program.CopyTo(memory, 0x200);
        
        _executing = true;
    }

    public void Run()
    {
        while (_executing)
            Step();
    }

    private void Step()
    {
        ushort opCode = FetchAndDecode();

        switch (opCode & 0xf000)
        {
            case 0x0000:
                switch (nn)
                {
                    case 0x00e0:
                        foreach (var pixel in _displayBuffer)
                        {
                            pixel.Clear();
                        }
                        break;
                    
                    case 0x00ee:
                        pc = stack.Pop();
                        break;
                }
                break;
                
            case 0x1000:
                pc = nnn;
                break;
            
            case 0x02000:
                stack.Push(pc);
                pc = nnn;
                break;
            
            case 0x3000:
                pc += (ushort)(v[x] == nn ? 2 : 0);
                break;
            
            case 0x4000:
                pc += (ushort)(v[x] != nn ? 2 : 0);
                break;
            
            case 0x5000:
                pc += (ushort)(v[x] == v[y] ? 2 : 0);
                break;
                
            case 0x6000:
                v[x] = nn;
                break;
                
            case 0x7000:
                v[x] += nn;
                break;
            
            case 0x8000:
                switch (n)
                {
                    case 0x0000:
                        v[x] = v[y];
                        break;

                    case 0x0001:
                        v[x] = (byte)(v[x] | v[y]);
                        break;
                    
                    case 0x0002:
                        v[x] = (byte)(v[x] & v[y]);
                        break;
                    
                    case 0x0003:
                        v[x] = (byte)(v[x] ^ v[y]);
                        break;
                    
                    case 0x0004:
                        v[0xf] = (byte)(v[x] + v[y] > 255 ? 1 : 0);
                        
                        v[x] = (byte)(v[x] + v[y]);
                        break;
                    
                    case 0x0005:
                        v[0xf] = (byte)(v[x] > v[y] ? 1 : 0);
                        
                        v[x] = (byte)(v[x] - v[y]);
                        break;
                    
                    case 0x0006:
                        v[0xf] = (byte)(v[y] & 0x1);
                        
                        v[x] = (byte)(v[y] >> 1);
                        break;
                    
                    case 0x0007:
                        v[0xf] = (byte)(v[y] > v[x] ? 1 : 0);
                        
                        v[x] = (byte)(v[y] - v[x]);
                        break;
                    
                    case 0x000e:
                        v[0xf] = (byte)((v[y] & 0x80) == 0x80 ? 1 : 0);
                        
                        v[x] = (byte)(v[y] << 1);
                        break;
                }
                break;
            
            case 0x9000:
                pc += (ushort)(v[x] != v[y] ? 2 : 0);
                break;
                
            case 0xa000:
                i = nnn;
                break;
            
            case 0xb000:
                pc = (ushort)(nnn + v[0]);
                break;
            
            case 0xc000:
                byte rnd = (byte)_random.NextInt64(0, 255);

                v[x] = (byte)(rnd & nn);
                break;
                
            case 0xd000:
                Draw();
                break;
            
            case 0xe000:
                switch (nn)
                {
                    case 0x009e:
                        if (_keyboardService.IsKeyDown(v[x]))
                            pc += 2;
                        break;
                    
                    case 0x00a1:
                        if (!_keyboardService.IsKeyDown(v[x]))
                            pc += 2;
                        break;
                }
                break;
            
            case 0xf000:
                switch (nn)
                {
                    case 0x0029:
                        i = (ushort)(0x50 + 5 * (v[x] & 0xf));
                        break;
                        
                    case 0x000a:
                        if (_keyboardService.TryConsumeLastPressedKey(out byte key))
                        {
                            v[x] = key;
                        }
                        else
                        {
                            pc -= 2;
                        }
                        break;
                }
                break;
        }
    }

    private void Draw()
    {
        int xPos = v[x] % 64;
        int yPos = v[y] % 32;

        v[0xf] = 0;
        
        for (int j = 0; j < n; j++)
        {
            byte row = memory[i + j];
            
            int tempX = xPos;
            
            for (int k = 7; k >= 0; k--)
            {
                if (tempX >= 64 || yPos >= 32)
                    break;
                
                bool curPixel = (row & 1 << k) != 0;

                if (curPixel)
                {
                    _displayBuffer[yPos * 64 + tempX].Flip(out bool off);
                    v[0xf] = off || v[0xf] == 1 ? (byte)1 : (byte)0;
                }
                tempX++;
            }
            yPos++;
        }
    }

    private ushort FetchAndDecode()
    {
        ushort opCode = (ushort)((memory[pc] << 8) | memory[pc + 1]);

        x = (byte)((opCode & 0x0f00) >> 8);
        y = (byte)((opCode & 0x00f0) >> 4);
        n = (byte)(opCode & 0x000f);
        nn = (byte)(opCode & 0x00ff);
        nnn = (ushort)(opCode & 0xfff);

        pc += 2;
        return opCode;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}