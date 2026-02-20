using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

namespace laskupeli;

/// @author gr313125
/// @version 22.01.2026
/// <summary>
/// 
/// </summary>
public class Laskupeli : PhysicsGame
{
    private Image _piirroslaskija = LoadImage("laskija");
    private PhysicsObject _laskija;
    private double _makiveleys = 4500;
    private double _makikorkeus = 600;
    
    public override void Begin()
    {

      Alusta();

       
    }

    public void Alusta()
    {
        ClearAll();
        Luokenttä();
        Näppäintoiminnot();
    }

    public void Luokenttä()
    {
        
        Add(Luoaurinko());
        Add(Luomaki());
        Add(Luolaksija());
        Add(_luohyppyri());
        Gravity = new Vector(100.0, -681.0);


        
    }
    public PhysicsObject Luoaurinko()
    {
        PhysicsObject aurinko = new PhysicsObject(100, 100, Shape.Circle);
        aurinko.Color = Color.Yellow;
        aurinko.X = 300;
        aurinko.Y = 300;
        aurinko.IgnoresGravity = true;
        return aurinko;
        
    }

    public PhysicsObject Luomaki()
    {
        PhysicsObject maki = new PhysicsObject(_makiveleys, _makikorkeus, Shape.Triangle);
        maki.X = -1000;
        maki.Y = -300;
        maki.IgnoresGravity = true;
        maki.MakeStatic();
        
        return maki;

    }

    public PhysicsObject Luolaksija()
    {
        _laskija  = new PhysicsObject(40,100,Shape.Rectangle);
        _laskija.Image = _piirroslaskija;
        _laskija.Add(Luosukset());
        _laskija.Restitution = 0.5;
        return _laskija;

    }

    public PhysicsObject Luosukset()
    {
        PhysicsObject sukset = new PhysicsObject(60, 10, Shape.Rectangle);
        sukset.Y = -50;
        sukset.Color = Color.Black;
        return sukset;
    }

    public void Näppäintoiminnot()
    {
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.S, ButtonState.Down, _laskija.Push, "hidastaa", new Vector(_laskija.Mass*-300,_laskija.Mass*500));
        Keyboard.Listen(Key.R, ButtonState.Pressed, Alusta, "aloittaa alusta");
        Keyboard.Listen(Key.A,ButtonState.Down, Laskijakulmava, "kiertää vasemmalle");
        Keyboard.Listen(Key.D, ButtonState.Down, Laskijakulmaoi, "kiertää oikealle");
        Keyboard.Listen(Key.W, ButtonState.Down, _laskija.Push, "kiihtyy", new Vector(_laskija.Mass * 100, 0));
    }

    public void Laskijakulmava()
    {
        _laskija.ApplyTorque(0.05);
        
    }
    public void Laskijakulmaoi()
    {
        _laskija.ApplyTorque(-0.05);
        
    }
    public PhysicsObject _luohyppyri()
    {
        PhysicsObject hyppyri = new PhysicsObject(100, 50, Shape.Triangle);
        hyppyri.Color = Color.Gray;
        hyppyri.X = 350;
        hyppyri.Y = -340;
        hyppyri.Angle = Angle.ArcTan(_makikorkeus / (_makiveleys / 2)) + Angle.FromDegrees(330);
        hyppyri.MakeStatic();
        hyppyri.IgnoresGravity = true;
        return hyppyri;

    }
}

