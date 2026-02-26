using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Avalonia.Input;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Services;

namespace Chip_8.Interpreters;

public class LegacyInterpreter : IInterpreter
{
    private readonly DisplayBuffer _displayBuffer;
    private readonly string _programPath;
    private readonly Random _random;
    private readonly Keyboard _keyboard;
    private readonly int _frequency;

    private bool _allowDraw;
    private bool _executing;
    
    private ushort _pc, _index;
    private byte[] _v = null!;

    private byte[] _memory = null!, program = null!;

    private readonly byte[] _font =
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

    private Stack<ushort> _stack = null!;

    private byte x, y, n, nn, _soundTimer, _delayTimer;
    private ushort nnn;

    public Dictionary<Key, byte>? KeyMap { get; }

    public LegacyInterpreter(DisplayBuffer displayBuffer,
        string programPath,
        Dictionary<Key, byte>? keyMap,
        Keyboard keyboard,
        int frequency)
    {
          KeyMap = keyMap;
          _keyboard = keyboard;
          _programPath = programPath;
          _displayBuffer = displayBuffer;
          _random = new Random();
          _frequency = frequency;

          InitializeCpu();
    }

    private void InitializeCpu()
    { 
        _pc = 0x200;
        _v = new byte[16];

        _memory = new byte[4096];
        _stack = new Stack<ushort>();

        _font.CopyTo(_memory, 0x50);

        program = File.ReadAllBytes(_programPath);

        program.CopyTo(_memory, 0x200);

        _allowDraw = true;
        _executing = true;
    }

    public void Run()
    {
        var sw = Stopwatch.StartNew();
        
        long ticksPerInstruction = Stopwatch.Frequency / _frequency;
        long ticksPerTimerTick = Stopwatch.Frequency / 60;
        
        long nextClockTick = sw.ElapsedTicks;
        long nextTimerTick = nextClockTick;

        while (_executing)
        {
            long now = sw.ElapsedTicks;
            
            if (now >= nextTimerTick)
            {
                Tick();
                nextTimerTick += ticksPerTimerTick;
                
                if (now - nextTimerTick > ticksPerTimerTick)
                    nextTimerTick = now;
            }
            
            //timings for the CPU step
            if (now >= nextClockTick) //step the CPU when enough time has passed
            {
                Step();
                nextClockTick += ticksPerInstruction; //calculate when the next tick should happen

                if (now - nextClockTick > ticksPerInstruction) //correct possible drift
                    nextClockTick = now;
            }
            else //halt the cpu if we need to
                Thread.SpinWait(20);
        }
        
        sw.Stop();
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
                        _displayBuffer.Clear();
                        break;

                    case 0x00ee:
                        _pc = _stack.Pop();
                        break;
                }
                break;

                case 0x1000:
                    _pc = nnn;
                    break;

                case 0x2000:
                    _stack.Push(_pc);
                    _pc = nnn;
                    break;

                case 0x3000:
                    _pc += (ushort)(_v[x] == nn ? 2 : 0);
                    break;

                case 0x4000:
                    _pc += (ushort)(_v[x] != nn ? 2 : 0);
                    break;

                case 0x5000:
                    _pc += (ushort)(_v[x] == _v[y] ? 2 : 0);
                    break;

                case 0x6000:
                    _v[x] = nn;
                    break;

                case 0x7000:
                    _v[x] += nn;
                    break;

                case 0x8000:
                    switch (n)
                    {
                        case 0x0000:
                            _v[x] = _v[y];
                            break;

                        case 0x0001:
                            _v[x] = (byte)(_v[x] | _v[y]);
                            _v[0xf] = 0;
                            break;

                        case 0x0002:
                            _v[x] = (byte)(_v[x] & _v[y]);
                            _v[0xf] = 0;
                            break;

                        case 0x0003:
                            _v[x] = (byte)(_v[x] ^ _v[y]);
                            _v[0xf] = 0;
                            break;

                        case 0x0004:
                            byte vX = _v[x];
                            byte vY = _v[y];
                            byte sum = (byte)(vX + vY);

                            _v[x] = sum;

                            _v[0xf] = (byte)(vX + vY > 255 ? 1 : 0);
                            break;

                        case 0x0005:
                            sum = (byte)(_v[x] - _v[y]);
                            byte carry = (byte)(_v[x] >= _v[y] ? 1 : 0);

                            _v[x] = sum;
                            _v[0xf] = carry;
                            break;

                        case 0x0006:
                            vY = _v[y];

                            _v[x] = (byte)(vY >> 1);
                            _v[0xf] = (byte)(vY & 0x1);
                            break;

                        case 0x0007:
                            sum = (byte)(_v[y] - _v[x]);
                            carry = (byte)(_v[y] >= _v[x] ? 1 : 0);

                            _v[x] = sum;
                            _v[0xf] = carry;
                            break;

                        case 0x000e:
                            vY = _v[y];

                            _v[x] = (byte)(vY << 1);
                            _v[0xf] = (byte)((vY & 0x80) == 0x80 ? 1 : 0);
                            break;
                    }
                    break;

            case 0x9000:
                _pc += (ushort)(_v[x] != _v[y] ? 2 : 0);
                break;

            case 0xa000:
                  _index = nnn;
                  break;

            case 0xb000:
                  _pc = (ushort)(nnn + _v[0]);
                  break;

            case 0xc000:
                byte rnd = (byte)_random.NextInt64(0, 256);

                _v[x] = (byte)(rnd & nn);
                break;

            case 0xd000:
                if (!_allowDraw) // wait till the cpu is allowed to draw again
                {
                    _pc -= 2;
                    break;
                }
                Draw();
                break;

            case 0xe000: 
                switch (nn)
                { 
                    case 0x009e:
                        if (_keyboard.IsKeyDown(_v[x]))
                            _pc += 2;
                        break;

                    case 0x00a1:
                        if (!_keyboard.IsKeyDown(_v[x])) 
                            _pc += 2;
                        break;
                }
                break;

            case 0xf000: 
                switch (nn)
                {
                    case 0x0007:
                        _v[x] = _delayTimer;
                        break;

                    case 0x000a:
                        if (!_keyboard.WaitingForKey)
                        {
                            _keyboard.WaitingForKey = true;
                            _keyboard.PendingKey = Keyboard.InvalidKey;
                        }

                        if (_keyboard.PendingKey == Keyboard.InvalidKey)
                            _pc -= 2;
                        else
                        {
                            _v[x] = _keyboard.PendingKey;
                            _keyboard.PendingKey = Keyboard.InvalidKey;
                            _keyboard.WaitingForKey = false;
                        }
                        break;

                    case 0x0015:
                        _delayTimer = _v[x];
                        break;

                    case 0x0018:
                        _soundTimer = _v[x];
                        break;

                    case 0x001e:
                        byte vX = _v[x];
                        _v[0xf] = (byte)(_index + _v[x] > 0xfff ? 1 : 0);

                        _index += vX;
                        break;

                    case 0x0029:
                        _index = (ushort)(0x50 + 5 * (_v[x] & 0xf));
                        break;

                    case 0x0033:
                        _memory[_index + 2] = (byte)(_v[x] % 10);
                        _memory[_index + 1] = (byte)(_v[x] / 10 % 10);
                        _memory[_index] = (byte)(_v[x] / 10 / 10);
                        break;

                    case 0x0055:
                        for (int j = 0; j <= x; j++)
                        {
                            _memory[_index + j] = _v[j];
                        }

                        _index = (ushort)(_index + x + 1);
                        break;

                    case 0x0065:
                        for (int j = 0; j <= x; j++)
                        {
                            _v[j] = _memory[_index + j];
                        }

                        _index = (ushort)(_index + x + 1);
                        break;
                }
                break;

            default:
                Debug.WriteLine("unknown OpCode");
                break;
        }
    }

    private void Draw()
    {
        _allowDraw = false;
        
        int xPos = _v[x] % 64;
        int yPos = _v[y] % 32;

        _v[0xf] = 0;

        for (int j = 0; j < n; j++)
        {
            byte row = _memory[_index + j];

            int tempX = xPos;

            for (int k = 7; k >= 0; k--)
            {
                if (tempX >= 64 || yPos >= 32)
                  break;

                bool curPixel = (row & 1 << k) != 0;

                if (curPixel)
                    _v[0xf] = (byte)(_displayBuffer.XorPixel(tempX, yPos) || _v[0xf] == 1 ? 1 : 0);
                
                tempX++;
            }
            yPos++;
        }
    }

    private ushort FetchAndDecode()
    {
        ushort opCode = (ushort)((_memory[_pc] << 8) | _memory[_pc + 1]);

        x = (byte)((opCode & 0x0f00) >> 8);
        y = (byte)((opCode & 0x00f0) >> 4);
        n = (byte)(opCode & 0x000f);
        nn = (byte)(opCode & 0x00ff);
        nnn = (ushort)(opCode & 0xfff);

        _pc += 2;
        return opCode;
    }

    private void Tick()
    {
        if (_soundTimer > 0)
          _soundTimer--;

        if (_delayTimer > 0)
          _delayTimer--;
        
        _allowDraw = true; // only allow drawing 60 times per second
    }
    
    public void Dispose() => _executing = false;
}