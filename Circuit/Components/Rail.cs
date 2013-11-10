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
    [DisplayName("Rail")]
    [DefaultProperty("Voltage")]
    [Description("Single terminal voltage source with the cathode implicitly connected to ground.")] 
    public class Rail : OneTerminal
    {
        /// <summary>
        /// Expression for voltage V.
        /// </summary>
        private Quantity voltage = new Quantity(1, Units.V);
        [Description("Voltage generated by this voltage source.")]
        [Serialize]
        public Quantity Voltage { get { return voltage; } set { if (voltage.Set(value)) NotifyChanged("Voltage"); } }
        
        public Rail() { Name = "V1"; }
        
        public override void Analyze(Analysis Mna)
        {
            // Unknown current.
            Mna.AddTerminal(Terminal, Mna.AddNewUnknown("i" + Name));
            // Set voltage equal to the rail.
            Mna.AddEquation(V, Voltage.Value);
            // Add initial conditions, if necessary.
            Expression V0 = Voltage.Value.Evaluate(t, Constant.Zero);
            if (!(V0 is Constant))
                Mna.AddInitialConditions(Arrow.New(V0, Constant.Zero));
        }

        public override void LayoutSymbol(SymbolLayout Sym)
        {
            base.LayoutSymbol(Sym);

            Sym.InBounds(new Coord(-20, 20), new Coord(20, 0));
            Sym.AddLine(EdgeType.Black, new Coord(-15, 0), new Coord(15, 0));
            Sym.DrawText(() => Voltage.ToString(), new Point(0, 2), Alignment.Center, Alignment.Near);
        }
    }
}

