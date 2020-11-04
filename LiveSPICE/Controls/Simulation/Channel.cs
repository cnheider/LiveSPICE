﻿using ComputerAlgebra.LinqCompiler;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace LiveSPICE
{
    /// <summary>
    /// Base channel type.
    /// </summary>
    public abstract class Channel : INotifyPropertyChanged
    {
        private string name = "";
        public string Name { get { return name; } set { name = value; NotifyChanged(nameof(Name)); } }

        private Brush signalStatus = Brushes.Transparent;
        public Brush SignalStatus { get { return signalStatus; } set { signalStatus = value; NotifyChanged(nameof(SignalStatus)); } }

        private double signalLevel = 0;
        public double SignalLevel { get { return signalLevel; } }
        /// <summary>
        /// Update the signal level of this channel.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="time"></param>
        public void SampleSignalLevel(double level, double time)
        {
            double a = Frequency.DecayRate(time, 0.25);
            signalLevel = Math.Max(level, level * a + signalLevel * (1 - a));
        }

        public void ResetSignalLevel() { signalLevel = 0; }

        // INotifyPropertyChanged.
        protected void NotifyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Channel of audio data.
    /// </summary>
    public class InputChannel : Channel
    {
        private int index = 0;
        public int Index { get { return index; } }

        public InputChannel(int Index) { index = Index; }
    }

    /// <summary>
    /// Channel generated by a signal expression.
    /// </summary>
    public class SignalChannel : Channel
    {
        private Func<double, double> signal;

        private double[] buffer = null;
        public double[] Buffer(int Count, double t, double dt)
        {
            if (buffer == null || buffer.Length != Count)
                buffer = new double[Count];
            for (int i = 0; i < Count; ++i, t += dt)
                buffer[i] = signal(t);
            return buffer;
        }

        public SignalChannel(ComputerAlgebra.Expression Signal)
        {
            signal = Signal.Compile<Func<double, double>>(Circuit.Component.t);
        }
    }

    /// <summary>
    /// Output audio channel.
    /// </summary>
    public class OutputChannel : Channel
    {
        private int index = 0;
        public int Index { get { return index; } }

        private ComputerAlgebra.Expression signal = 0;
        public ComputerAlgebra.Expression Signal { get { return signal; } set { signal = value; NotifyChanged(nameof(Signal)); } }

        public OutputChannel(int Index) { index = Index; }
    }
}
