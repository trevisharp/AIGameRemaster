using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

Application.SetHighDpiMode(HighDpiMode.SystemAware);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

List<Player> players = new List<Player>();
List<PointF> foods = new List<PointF>();
List<Bomb> bombs = new List<Bomb>();
int frame = 0;
Random rand = new Random(DateTime.Now.Millisecond);

var form = new Form();
form.WindowState = FormWindowState.Maximized;
form.KeyPreview = true;
form.FormBorderStyle = FormBorderStyle.None;
form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
};

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

Bitmap bmp = null;
Graphics g = null;

Timer tm = new Timer();
tm.Interval = 25;

form.Load += delegate
{
    foreach (var pl in typeof(Player).Assembly.DefinedTypes)
    {
        if (pl.BaseType == typeof(Player))
        {
            players.Add((Player)pl.GetConstructors()[0].Invoke(new object [] {
                new PointF(rand.Next(form.Width), rand.Next(form.Height))
            }));
        }
    }


    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
    g.Clear(Color.White);
    pb.Image = bmp;
    tm.Start();
};

tm.Tick += delegate
{
    g.Clear(Color.White);
    frame++;
    
    foreach (var player in players)
    {
        if (!player.IsBroked)
            player.Loop(g, .025f, players, foods, bombs, frame);
    }
    
    if (foods.Count < 40)
        foods.Add(new PointF(rand.Next(form.Width), rand.Next(form.Height)));
    foreach (var food in foods)
    {
        g.FillRectangle(Brushes.Orange, food.X - 3, food.Y - 3, 6, 6);
    }
    
    for (int i = 0; i < bombs.Count; i++)
    {
        var bomb = bombs[i];
        var dontremove = bomb.Loop(g, .025f, players);
        if (!dontremove)
        {
            bombs.RemoveAt(i);
            i--;
        }
    }

    pb.Refresh();
};

Application.Run(form);