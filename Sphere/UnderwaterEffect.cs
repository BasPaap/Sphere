﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace Bas.Sphere
{

    /// <summary>Applies water defraction effect.</summary>
    public class UnderwaterEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(UnderwaterEffect), 0);
        public static readonly DependencyProperty TimerProperty = DependencyProperty.Register("Timer", typeof(double), typeof(UnderwaterEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty RefractonProperty = DependencyProperty.Register("Refracton", typeof(double), typeof(UnderwaterEffect), new UIPropertyMetadata(((double)(50D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty VerticalTroughWidthProperty = DependencyProperty.Register("VerticalTroughWidth", typeof(double), typeof(UnderwaterEffect), new UIPropertyMetadata(((double)(23D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty Wobble2Property = DependencyProperty.Register("Wobble2", typeof(double), typeof(UnderwaterEffect), new UIPropertyMetadata(((double)(23D)), PixelShaderConstantCallback(3)));
        public UnderwaterEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/Bas.Sphere;component/UnderwaterEffect.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(TimerProperty);
            this.UpdateShaderValue(RefractonProperty);
            this.UpdateShaderValue(VerticalTroughWidthProperty);
            this.UpdateShaderValue(Wobble2Property);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        public double Timer
        {
            get
            {
                return ((double)(this.GetValue(TimerProperty)));
            }
            set
            {
                this.SetValue(TimerProperty, value);
            }
        }
        /// <summary>Refraction Amount.</summary>
        public double Refracton
        {
            get
            {
                return ((double)(this.GetValue(RefractonProperty)));
            }
            set
            {
                this.SetValue(RefractonProperty, value);
            }
        }
        /// <summary>Vertical trough</summary>
        public double VerticalTroughWidth
        {
            get
            {
                return ((double)(this.GetValue(VerticalTroughWidthProperty)));
            }
            set
            {
                this.SetValue(VerticalTroughWidthProperty, value);
            }
        }
        /// <summary>Center X of the Zoom.</summary>
        public double Wobble2
        {
            get
            {
                return ((double)(this.GetValue(Wobble2Property)));
            }
            set
            {
                this.SetValue(Wobble2Property, value);
            }
        }
    }
}
