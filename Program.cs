using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Reflection;

Application.SetHighDpiMode(HighDpiMode.SystemAware);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

List<Player> players = new List<Player>();
List<PointF> foods = new List<PointF>();
List<Bomb> bombs = new List<Bomb>();
int frame = 0;
Random rand = new Random(DateTime.Now.Millisecond);

var form = new Form
{
    WindowState = FormWindowState.Maximized,
    KeyPreview = true,
    FormBorderStyle = FormBorderStyle.None
};
form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
};

var pb = new PictureBox
{
    Dock = DockStyle.Fill
};
form.Controls.Add(pb);

Bitmap bmp = null;
Graphics g = null;

var tm = new Timer
{
    Interval = 25
};

form.Load += delegate
{
    var mode = args.Length < 1 ? "test" : args[0];
    foreach (var pl in typeof(Player).Assembly.DefinedTypes)
    {
        if (pl.BaseType != typeof(Player))
            continue;
        
        var isTest = pl.GetCustomAttribute<TestAttribute>() is not null;
        if (mode == "test" && !isTest)
            continue;
        
        var constructors = pl.GetConstructors();
        if (constructors.Length == 0)
            continue;
        
        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        if (parameters.Length != 1)
            continue;
        
        var parameter = parameters[0];
        if (parameter.ParameterType != typeof(PointF))
            continue;
        
        var rndPoint = new PointF(
            rand.Next(form.Width),
            rand.Next(form.Height)
        );
        
        var player = (Player)constructor.Invoke(new object [] { rndPoint });
        if (player is null)
            continue;
        
        players.Add(player);
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
            player.Loop(g, .025f, players, foods, bombs);
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