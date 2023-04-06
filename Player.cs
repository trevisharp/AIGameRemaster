using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public abstract class Player
{
    public Player(PointF location, Color primary, Color secundary, string name)
    {
        this.Location = location;
        this.primarycolor = primary;
        this.secundarycolor = secundary;
        this.name = name;
    }

    private readonly Color primarycolor = Color.Blue;
    private readonly Color secundarycolor = Color.Blue;
    private readonly string name = "robo sem nome";
    public Color PrimaryColor => primarycolor;
    public Color SecundaryColor => secundarycolor;
    public string Name => name;
    public bool IsBroked { get; private set; } = false;
    public int Points { get; private set; } = 0;
    public double Energy { get; private set; } = 100;
    public double MaxEnergy { get; private set; } = 100;
    public double EnergyRegeneration { get; private set; } = 2;
    public double Life { get; private set; } = 100;
    public double MaxLife { get; private set; } = 100;
    public double LifeRegeneration { get; private set; } = 2;
    public List<PointF> EntitiesInAccurateSonar { get; private set; } = new List<PointF>();
    public int EntitiesInStrongSonar { get; private set; } = 0;
    public List<PointF> EnemiesInInfraRed { get; private set; } = new List<PointF>();
    public List<PointF> FoodsInInfraRed { get; private set; } = new List<PointF>();
    public PointF Location { get; private set; }
    public SizeF Velocity { get; private set; } = SizeF.Empty;
    public PointF? LastDamage { get; private set; } = null;

    private bool accuratesonaron = false;
    private bool strongsonaron = false;
    private bool infraredsensoron = false;
    private PointF? infraredsensorpoint = null;
    private bool shooting = false;
    private PointF? shootingpoint = null;
    private bool infrareset = false;
    private bool sonarreset = false;
    private bool turbo = false;
    private bool moving = false;

    public void Broke()
    {
        this.IsBroked = true;
    }

    public void AccurateSonar()
    {
        this.accuratesonaron = true;
    }

    public void StrongSonar()
    {
        this.strongsonaron = true;
    }

    public void InfraRedSensor(PointF p)
    {
        this.infraredsensoron = true;
        this.infraredsensorpoint = p;
    }

    public void InfraRedSensor(SizeF direction)
    {
        InfraRedSensor(this.Location + direction);
    }

    public void InfraRedSensor(float angle)
    {
        SizeF direction = new SizeF(
            (float)Math.Cos(angle * (2 * Math.PI) / 360f),
            (float)Math.Sin(angle * (2 * Math.PI) / 360f)
        );
        InfraRedSensor(direction);
    }

    public void Shoot(PointF p)
    {
        shooting = true;
        shootingpoint = p;
    }

    public void Shoot(SizeF direction)
    {
        InfraRedSensor(this.Location + direction);
    }

    public void StartMove(PointF p)
    {
        p = new PointF(p.X - Location.X, p.Y - Location.Y);
        float mod = (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
        if (mod == 0)
            Velocity = SizeF.Empty;
        Velocity = new SizeF(2 * p.X / mod, 2 * p.Y / mod);
        moving = true;
    }

    public void StartMove(SizeF direction)
    {
        StartMove(this.Location + direction);
    }
    
    public void StartMove(float angle)
    {
        SizeF direction = new SizeF(
            (float)Math.Cos(angle * (2 * Math.PI) / 360f),
            (float)Math.Sin(angle * (2 * Math.PI) / 360f)
        );
        StartMove(direction);
    }

    public void StartTurbo()
    {
        turbo = true;
    }

    public void StopTurbo()
    {
        turbo = false;
    }

    public void StopMove()
    {
        Velocity = SizeF.Empty;
        moving = false;
    }

    public void ResetInfraRed()
    {
        infrareset = true;
    }

    public void ResetSonar()
    {
        sonarreset = true;
    }

    public void Draw(Graphics g)
    {
        Color darkpc = Color.FromArgb(
            4 * this.PrimaryColor.R / 10,
            4 * this.PrimaryColor.G / 10,
            4 * this.PrimaryColor.B / 10
        );
        Color darksc = Color.FromArgb(
            4 * this.SecundaryColor.R / 10,
            4 * this.SecundaryColor.G / 10,
            4 * this.SecundaryColor.B / 10
        );
        g.FillEllipse(new SolidBrush(darkpc),
            this.Location.X - 20, this.Location.Y - 20,
            40, 40);
        g.FillPie(new SolidBrush(this.PrimaryColor),
            this.Location.X - 20, this.Location.Y - 20,
            40, 40, 0f, 360f * (float)Life / 100);
        g.FillEllipse(new SolidBrush(darksc), 
            this.Location.X - 10, this.Location.Y - 10,
            20, 20);
        g.FillPie(new SolidBrush(this.SecundaryColor), 
            this.Location.X - 10, this.Location.Y - 10,
            20, 20, 0f, 360f * (float)Energy / 100);
        g.DrawEllipse(Pens.Black, 
            this.Location.X - 20, this.Location.Y - 20,
            40, 40);
        g.DrawString(name, SystemFonts.CaptionFont, Brushes.Black, 
            new RectangleF(this.Location.X - 40, this.Location.Y - 40, 80, 20),
            new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
    }

    public void ReciveDamage(PointF bomb)
    {
        Life -= 15;
        this.LastDamage = bomb;
    }

    public void Loop(Graphics g, float dt, 
        List<Player> allplayers, List<PointF> allfoods, 
        List<Bomb> allbombs, int i)
    {
        allplayers = allplayers
            .Where(p => !p.IsBroked)
            .ToList();
        float dx, dy;
        loop();
        Draw(g);

        if (accuratesonaron)
        {
            accuratesonaron = false;
            EntitiesInAccurateSonar.Clear();
            Energy -= 10 * dt;
            EntitiesInAccurateSonar.Clear();
            foreach (var player in allplayers)
            {
                if (player == this)
                    continue;
                dx = player.Location.X - this.Location.X;
                dy = player.Location.Y - this.Location.Y;
                if (dx * dx + dy * dy < 200 * 200) // 150 pixels
                {
                    EntitiesInAccurateSonar.Add(player.Location);
                }
            }
            foreach (var food in allfoods)
            {
                dx = food.X - this.Location.X;
                dy = food.Y - this.Location.Y;
                if (dx * dx + dy * dy < 200 * 200) // 150 pixels
                {
                    EntitiesInAccurateSonar.Add(food);
                }
            }
            
            g.DrawEllipse(Pens.Blue, this.Location.X - 400 / 2,
                 this.Location.Y - 400 / 2, 400, 400);
        }
        
        if (strongsonaron)
        {
            strongsonaron = false;
            Energy -= 10 * dt;
            EntitiesInStrongSonar = 0;
            foreach (var player in allplayers)
            {
                if (player == this)
                    continue;
                dx = player.Location.X - this.Location.X;
                dy = player.Location.Y - this.Location.Y;
                if (dx * dx + dy * dy < 300*300) // 300 pixels
                {
                    EntitiesInStrongSonar++;
                }
            }
            foreach (var food in allfoods)
            {
                dx = food.X - this.Location.X;
                dy = food.Y - this.Location.Y;
                if (dx * dx + dy * dy < 300*300) // 300 pixels
                {
                    EntitiesInStrongSonar++;
                }
            }
            
            g.DrawEllipse(Pens.DarkBlue, this.Location.X - 600 / 2,
                 this.Location.Y - 600 / 2, 600, 600);
        }

        if (infraredsensoron)
        {
            infraredsensoron = false;
            EnemiesInInfraRed.Clear();
            FoodsInInfraRed.Clear();
            Energy -= 10 * dt;
            float isdx = infraredsensorpoint.Value.X - this.Location.X,
                  isdy = infraredsensorpoint.Value.Y - this.Location.Y;
            SizeF line = new SizeF(infraredsensorpoint.Value.X - this.Location.X,
                infraredsensorpoint.Value.Y - this.Location.Y);
            line = line / (float)Math.Sqrt(isdx * isdx + isdy * isdy);
            
            foreach (var player in allplayers)
            {
                if (player == this)
                    continue;
                var r = player.Location;
                var dist = (float)Math.Sqrt((r.X - this.Location.X) * (r.X - this.Location.X) +
                    (r.Y - this.Location.Y) * (r.Y - this.Location.Y));
                var final = this.Location + line * dist;
                var finaldist = (float)Math.Sqrt((r.X - final.X) * (r.X - final.X) +
                    (r.Y - final.Y) * (r.Y - final.Y));
                if (finaldist < 50f)
                    EnemiesInInfraRed.Add(r);
            }
            
            foreach (var food in allfoods)
            {
                var r = food;
                var dist = (float)Math.Sqrt((r.X - this.Location.X) * (r.X - this.Location.X) +
                    (r.Y - this.Location.Y) * (r.Y - this.Location.Y));
                var final = this.Location + line * dist;
                var finaldist = (float)Math.Sqrt((r.X - final.X) * (r.X - final.X) +
                    (r.Y - final.Y) * (r.Y - final.Y));
                if (finaldist < 50f)
                    FoodsInInfraRed.Add(r);
            }
            g.DrawLine(Pens.Red, this.Location, this.Location + line * 2000f);

            infraredsensorpoint = null;
        }

        if (shooting)
        {
            shooting = false;
            Energy -= 20 * dt;
            SizeF speed = new SizeF(shootingpoint.Value.X -this.Location.X,
                shootingpoint.Value.Y -this.Location.Y);
            speed = 50f * speed / (float)Math.Sqrt(
                speed.Height * speed.Height + speed.Width * speed.Width);
            allbombs.Add(new Bomb()
            {
                Location = this.Location + speed,
                Speed = 3f * speed
            });
        }

        if (infrareset)
        {
            infrareset = false;
            EnemiesInInfraRed.Clear();
            FoodsInInfraRed.Clear();
        }

        if (sonarreset)
        {
            sonarreset = false;
            EntitiesInStrongSonar = 0;
            EntitiesInAccurateSonar.Clear();
        }

        for (int f = 0; f < allfoods.Count; f++)
        {
            var food = allfoods[f];
            float fdx = food.X - this.Location.X,
                  fdy = food.Y - this.Location.Y;
            float fdist = (float)Math.Sqrt(fdx * fdx + fdy * fdy);
            if (fdist < 10f)
            {
                allfoods.RemoveAt(f);
                f--;
                this.Points++;
                this.LifeRegeneration += .5f;
                this.EnergyRegeneration += .5f;
            }
        }

        Location += (turbo ? 2f : 1f) * 50f * Velocity * dt;
        Energy += EnergyRegeneration * dt;
        if (moving)
        {
            Energy -= 2 * dt;
            if (turbo)
                Energy -= 2 * dt;
        }
        if (Energy > MaxEnergy)
            Energy = MaxEnergy;
        Life += LifeRegeneration * dt;
        if (Life > MaxLife)
            Life = MaxLife;
        if (Energy < 0 || Life < 0)
            Broke();
    }
    protected abstract void loop();
}
