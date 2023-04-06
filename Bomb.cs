using System.Collections.Generic;
using System.Drawing;

public class Bomb
{
    public PointF Location { get; set; }
    public SizeF Speed { get; set; }

    public bool Loop(Graphics g, float dt, List<Player> allplayer)
    {
        g.FillEllipse(Brushes.Black, new RectangleF(
            Location.X - 2, Location.Y - 2, 4, 4));
        Location += Speed * dt;
        foreach (var player in allplayer)
        {
            if (player.IsBroked)
                continue;
            var rect = new RectangleF(
                player.Location.X - 25, 
                player.Location.Y - 25,
                50, 50);
            if (rect.Contains(this.Location))
            {
                player.ReciveDamage(this.Location);
                return false;
            }
        }

        return true;
    }
}