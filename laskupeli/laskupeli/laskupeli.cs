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
    private static Image _piirrosLaskija = LoadImage("lauta.png");
    private static Shape _laskijanMuoto = Shape.FromImage(_piirrosLaskija);
    private static Image _lauta = LoadImage("lumilauta.png");
    private static Shape _lautaMuoto = Shape.FromImage(_lauta);
    private static Image _hyppykuva = LoadImage("hyppyri");

    private PhysicsObject _laskija;
    private static double _makiveleys = 4500;
    private static double _makikorkeus = 600;
    private int _makix = -1000;
    private int _makiy = -300;
    private PhysicsObject maki;
    private PhysicsObject hyppyri;
    private PhysicsObject este;
    

    public override void Begin()
    {
        
        Alusta();
       


    }

    public void Alusta()
    {
     

        //IsFullScreen = true;
        ClearAll();
        hyppyri = new PhysicsObject(RandomGen.NextInt(100, 200), RandomGen.NextInt(20, 100));
        maki = new PhysicsObject(_makiveleys, _makikorkeus, Shape.Rectangle);
        este = new PhysicsObject(1, 1000);
        Luokenttä();
        //Level.CreateBorders();
        Näppäintoiminnot();
       
    }

    public void Luokenttä()
    {
        
        Add(Luoaurinko());
        Add(Luomaki());
        Add(Luolaksija());
        Add(luohyppyri());
        Add(luoeste());
        Gravity = new Vector(100.0, -681.0);
        AddCollisionHandler(este,hyppyri, hyppyrinpoisto);


        
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
        
        maki.X = _makix;
        maki.Y = _makiy;
        maki.IgnoresGravity = true;
        maki.Angle = Angle.FromDegrees(-15);
        maki.MakeStatic();
        
        return maki;

    }

    public PhysicsObject Luolaksija()
    {
        _laskija = new PhysicsObject(50, 100);
        _laskija.Image = _piirrosLaskija;
        _laskija.Shape = _laskijanMuoto;
        _laskija.Add(Luosukset());
        _laskija.X = Screen.Left+40;
        _laskija.Restitution = 0.5;
        return _laskija;

    }

    public PhysicsObject Luosukset()
    {
        PhysicsObject sukset = new PhysicsObject(100, 50);
        sukset.Shape = _lautaMuoto;
        sukset.Image = _lauta;
        sukset.Y = -_laskija.Height/4;
        
        return sukset;
    }

    public void Näppäintoiminnot()
    {
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.S, ButtonState.Down, _laskija.Push, "hidastaa", new Vector(_laskija.Mass*-300,_laskija.Mass*500));
        Keyboard.Listen(Key.R, ButtonState.Pressed, Alusta, "aloittaa alusta");
        Keyboard.Listen(Key.A,ButtonState.Down, Laskijakulmava, "kiertää vasemmalle", 1);
        Keyboard.Listen(Key.D, ButtonState.Down, Laskijakulmava , "kiertää oikealle", -1);
        Keyboard.Listen(Key.W, ButtonState.Down, _laskija.Push, "kiihtyy", new Vector(_laskija.Mass * 50, 0));
        Mouse.Listen(MouseButton.Left, ButtonState.Pressed, CreateExplosion, "");
    }

    private void CreateExplosion()
    {
        Explosion explosion = new Explosion(500);
        explosion.Position = Mouse.PositionOnScreen;
        Add(explosion);
    }

    public void Laskijakulmava(int kerroin)
    {
        _laskija.ApplyTorque(0.01 * kerroin);
        
    }

    public void valitsehyppyri()
    {
        List<PhysicsObject> hyppyrit = new List<PhysicsObject>();
        hyppyrit.Add(luohyppyri());
        
    }
 
    public PhysicsObject luohyppyri()
    {
        
        hyppyri.Image = _hyppykuva;
        hyppyri.X = Screen.Right;
        hyppyri.Y = -505;
        hyppyri.Shape = Shape.FromImage(_hyppykuva);    
        hyppyri.Mass= double.Max(100,100);
        hyppyri.Angle = maki.Angle;
        hyppyri.MoveTo(new Vector(-1000,-200), 100);
        hyppyri.IgnoresGravity = true;
        hyppyri.IgnoresCollisionWith(maki);
        hyppyri.CanRotate = false;
        return hyppyri;
        
    }

    public PhysicsObject luoeste()
    {
        este.X = Screen.Left;
        este.MakeStatic();
        este.IgnoresCollisionWith(_laskija);
        return este;
    }

   
    public void hyppyrinpoisto(PhysicsObject tormaaja, PhysicsObject kohde)
    {
       Remove(hyppyri);


    }
    

    
    
}

