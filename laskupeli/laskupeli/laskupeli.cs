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
    private static Shape _hyppymuoto = Shape.FromImage(_hyppykuva);

    private PhysicsObject _laskija;
    private static double _makiveleys = 4500;
    private static double _makikorkeus = 600;
    private int _makix = -1000;
    private int _makiy = -300;
    private PhysicsObject _maki;
    private PhysicsObject _hyppyri;
    private PhysicsObject _este;
    private double _pyoriminen = 0;
    private double _edellinenKulma = 0;
    

    public override void Begin()
    {
        
        Alusta();
       


    }
/// <summary>
/// aloittaa pelin uudelleen jolloin tekee perus asiat.
/// </summary>
    public void Alusta()
    {
     

        IsFullScreen = true;
        ClearAll();
        
        _maki = new PhysicsObject(_makiveleys, _makikorkeus, Shape.Rectangle);
        _este = new PhysicsObject(1, 1000);
        Luokenttä();
        //Level.CreateBorders();
        Näppäintoiminnot();

        Timer chekki = new Timer(0.1);
        chekki.Timeout += Ilotulitteet;
        chekki.Start();

    }
/// <summary>
/// lisää peliin objektit, kute mäen ja laskijan ja hyppyrin.
/// </summary>
    public void Luokenttä()
    {
        
        Add(Luoaurinko());
        Add(Luomaki());
        Add(Luolaksija());
        Add(Luohyppyri());
        Add(Luoeste());
        Gravity = new Vector(100.0, -681.0);

        AddCollisionHandler(_este, _hyppyri, Hyppyrinpoisto);





        
    }
/// <summary>
/// tekee auringon
/// </summary>
/// <returns>auringon</returns>
    public PhysicsObject Luoaurinko()
    {
        PhysicsObject aurinko = new PhysicsObject(100, 100, Shape.Circle);
        aurinko.Color = Color.Yellow;
        aurinko.X = 300;
        aurinko.Y = 300;
        aurinko.IgnoresGravity = true;
        return aurinko;
        
    }
/// <summary>
/// tekee laskettavan mäen eli pohjan
/// </summary>
/// <returns>mäen</returns>
    public PhysicsObject Luomaki()
    {
        
        _maki.X = _makix;
        _maki.Y = _makiy;
        _maki.IgnoresGravity = true;
        _maki.Angle = Angle.FromDegrees(-15);
        _maki.MakeStatic();
        
        return _maki;

    }
/// <summary>
/// tekee laskijan laskijan muoden, paikan ja fysiikan sekä lisää sukset sille
/// </summary>
/// <returns>laskija</returns>
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
/// <summary>
/// tekee sukset
/// </summary>
/// <returns>sukset</returns>
    public PhysicsObject Luosukset()
    {
        PhysicsObject sukset = new PhysicsObject(100, 50);
        sukset.Shape = _lautaMuoto;
        sukset.Image = _lauta;
        sukset.Y = -_laskija.Height/4;
        
        return sukset;
    }
/// <summary>
/// tekee kaikki mitä tapahtuu kun painettan näppäintä
/// </summary>
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
/// <summary>
/// tekee hauskan räjähdyksen
/// </summary>
    private void CreateExplosion()
    {
        Explosion explosion = new Explosion(500);
        explosion.Position = Mouse.PositionOnScreen;
        Add(explosion);
    }
/// <summary>
/// kääntää laskijaa
/// </summary>
/// <param name="kerroin"> kertoo kumpaan suuntaan laskija kääntyy</param>
    public void Laskijakulmava(int kerroin)
    {
        _laskija.ApplyTorque(0.01 * kerroin);
        
    }


 /// <summary>
 /// hyppyrin paikka ja liike
 /// </summary>
 /// <returns>palauttaa hyppyrin</returns>
    public PhysicsObject Luohyppyri()
    {
        PhysicsObject hyppyri = new PhysicsObject(Hyppyrikoot(RandomGen.NextInt(8)), Hyppyrikoot(RandomGen.NextInt(0,6)));
        
        hyppyri.Image = _hyppykuva;
        hyppyri.X = Screen.Right;
        hyppyri.Y = -505;
        hyppyri.Shape = _hyppymuoto;
        hyppyri.Mass= double.Max(100,100);
        hyppyri.Angle = _maki.Angle;
        hyppyri.MoveTo(new Vector(-1500,168), 100);
        hyppyri.IgnoresGravity = true;
        hyppyri.IgnoresCollisionWith(_maki);
        hyppyri.CanRotate = false;

        _hyppyri = hyppyri;
        return hyppyri;
        
    }
/// <summary>
/// tekee esteen vasempaan reunaan jolla resettaan hyppyrin
/// </summary>
/// <returns> päätöpisteen</returns>
    public PhysicsObject Luoeste()
    {
        _este.X = Screen.Left;
        _este.MakeStatic();
        _este.IgnoresCollisionWith(_laskija);
        return _este;
    }

   
    public void Hyppyrinpoisto(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        
        kohde.Destroy();
        Add(Luohyppyri());
        

    }

    public int Hyppyrikoot(int valinta)
    {
        List<int> koot = new List<int>();
        koot.Add(35);
        koot.Add(40);
        koot.Add(50);
        koot.Add(60);
        koot.Add(70);
        koot.Add(80);
        koot.Add(90);
        koot.Add(100);
        return koot[valinta];
    }

    public void Ilotulitteet()
    {
     
        
            
            double nykyinenKulma = _laskija.Angle.Degrees;

            double muutos = nykyinenKulma - _edellinenKulma;

        
            if (muutos > 180) muutos -= 360;
            if (muutos < -180) muutos += 360;

            _pyoriminen += muutos;

            
            if (_pyoriminen <= -360)
            {
                MessageDisplay.Add("voltti");
                
                for (int i = 0; i < 10; i++)
                {
                    Explosion ilotulitus = new Explosion(10);
                    ilotulitus.X = RandomGen.NextInt(-600, 600);
                    ilotulitus.Y = RandomGen.NextInt(-300, 400);
                    ilotulitus.Force = 0;
                
                    Add(ilotulitus);
                
                
                }
                _pyoriminen = 0;
                
            }
            if (_pyoriminen >= 360)
            {
                MessageDisplay.Add("päkkäri");
                for (int i = 0; i < 10; i++)
                {
                    Explosion ilotulitus = new Explosion(10);
                    ilotulitus.X = RandomGen.NextInt(-600, 600);
                    ilotulitus.Y = RandomGen.NextInt(-300, 400);
                    ilotulitus.Force = 0;
                
                    Add(ilotulitus);
                
                
                }
                _pyoriminen = 0;
            }
            _edellinenKulma = nykyinenKulma;
        
    }
}