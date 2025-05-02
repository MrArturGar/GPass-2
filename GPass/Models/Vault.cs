using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Models;

public class Vault
{
    public byte[] MagicNumber { get; set; } = { 0x47, 0x50, 0x41, 0x53 };
    public double Version { get; set; } = 1.0;
    public byte[] Salt { get; set; }
    public List<Entry> Entries { get; set; } = new();
}