﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyMath;

namespace Circuit
{
    /// <summary>
    /// Ideal voltage source.
    /// </summary>
    [Category("Standard")]
    [DisplayName("Input Voltage Source")]
    [DefaultProperty("Voltage")]
    [Description("Ideal voltage source representing an input port.")]
    public class Input : TwoTerminal
    {
        public Input() { Name = "V1"; }
        
        public override void Analyze(Analysis Mna) { VoltageSource.Analyze(Mna, "", Anode, Cathode, DependentVariable(Name, t)); }

        public override void LayoutSymbol(SymbolLayout Sym)
        {
            base.LayoutSymbol(Sym);

            int w = 10;
            Sym.AddLine(EdgeType.Black, new Coord(-w, 20), new Coord(w, 20));
            Sym.DrawPositive(EdgeType.Black, new Coord(0, 15));
            Sym.AddLine(EdgeType.Black, new Coord(-w, -20), new Coord(w, -20));
            Sym.DrawNegative(EdgeType.Black, new Coord(0, -15));

            Sym.DrawText(() => Name, new Point(0, 0), Alignment.Center, Alignment.Center);
        }
    }
}

