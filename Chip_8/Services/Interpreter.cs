using System.Collections.Generic;
using System.IO;
using Chip_8.ViewModels;

namespace Chip_8.Services;

public class Interpreter
{
    private readonly InterpreterOptions _options;
    
    private bool _executing;
    
    private ushort pc, i;
    private byte[] v;
    
    private byte[] memory, program;

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

    private Stack<ushort> stack;

    private byte x, y, n, nn;
    private ushort nnn;
    
    //TODO: Timers
    //TODO: Keypads

    public Interpreter(InterpreterOptions options)
    {
        _options = options;
        Initalize();
    }
    
    private void Initalize()
    {
        pc = 0x200;
        v = new byte[16];
        
        memory = new byte[4069];
        stack = new Stack<ushort>();

        program = File.ReadAllBytes(_options.Path);

        for (int j = 0; j < program.Length; j++)
        {
            memory[j + 0x200] = program[j];
        }
        
        _executing = true;
    }

    private void MainLoop()
    {
        while (_executing)
        {
            ushort opCode = FetchAndDecode();

            switch (opCode & 0xf000)
            {
                case 0x0000:
                    switch (nn)
                    {
                        case 0x00e0:
                            //_screen = new ObservableCollection<bool>(Enumerable.Repeat(false, 2048));
                            break;
                    }
                    break;
                
                case 0x1000:
                    pc = nnn;
                    break;
                
                case 0x6000:
                    v[x] = nn;
                    break;
                
                case 0x7000:
                    v[x] += nn;
                    break;
                
                case 0xa000:
                    i = nnn;
                    break;
            }
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

    private void Draw()
    {
        
    }
}