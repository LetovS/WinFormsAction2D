using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Actions
{
    
    public class Task4:Form
    {
        #region Consts
        private const int INDENT = 10;
        private const int btnWidth = 90;
        private const int btnHeight = 40;
        #endregion

        #region Fields
        /// <summary>
        /// Список рисуемых объектов.
        /// </summary>
        enum LineType { None = 1, Curve = 2, Bezier = 3, Polygon =4 , FilledCurve =5 }
        /// <summary>
        /// Коллекция точек.
        /// </summary>
        private List<Point> points;
        private List<bool> flags;
        private Color baseBackColor;
        private bool editPoints = false;
        private PictureBox p;
        private Timer timer;
        private Label msg;
        #endregion


        public Task4()
        {

            points = new List<Point>();// { new Point(38,28), new Point(479, 128), new Point(397, 256), new Point(38, 128) };
            flags = new List<bool>();
            p = new PictureBox();
            timer = new Timer();
            timer.Interval = 300;
            timer.Tick += MoveFigure;
            // Динамическое создание формы с кнопками 
            this.Size = new Size(800, 500);
            this.Text = "Draw figures";
            this.MaximizeBox = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Font = new Font("Time New Romans", 8, FontStyle.Regular);

            #region Кнопоки
            Button addPoint = new Button() { Text = "Точки" };
            addPoint.SetBounds(INDENT, INDENT, btnWidth, btnHeight);
            addPoint.Tag = 1;
            addPoint.Click += (o, e) => { editPoints = !editPoints; (o as Button).FlatStyle = (editPoints) ? FlatStyle.Popup : FlatStyle.Standard; };

            Button param = new Button() { Text = "Параметры" };
            param.SetBounds(INDENT, addPoint.Bottom + INDENT, btnWidth, btnHeight);

            Button move = new Button() { Text = "Движение" };
            move.SetBounds(INDENT, param.Bottom + INDENT, btnWidth, btnHeight);
            move.Click += (o,e) => { timer.Enabled = !timer.Enabled; };


            Button clear = new Button() { Text = "Очистить"};
            clear.SetBounds(INDENT, move.Bottom + INDENT, btnWidth, btnHeight);
            clear.Tag = p;
            clear.Click += (o, e) => { ((o as Button).Tag as PictureBox).CreateGraphics().Clear(baseBackColor); points.Clear(); };
            Button curve = new Button() { Text = "Кривая"};
            curve.SetBounds(INDENT, clear.Bottom + INDENT, btnWidth, btnHeight);
            curve.Tag = (item1: 2, item2:p);
            curve.Click += DrawLineType;

            Button bezier = new Button() { Text = "Безье"};
            bezier.Tag = (3, p);
            bezier.Click += DrawLineType;
            bezier.SetBounds(INDENT, curve.Bottom + INDENT, btnWidth, btnHeight);

            Button polygon = new Button() { Text = "Ломанная"};
            polygon.Tag = (4, p);
            polygon.Click += DrawLineType;
            polygon.SetBounds(INDENT, bezier.Bottom + INDENT, btnWidth, btnHeight);

            Button fiilCurve = new Button() { Text = "Закрашенная"};
            fiilCurve.Tag = (5, p);
            fiilCurve.Click += DrawLineType;
            fiilCurve.SetBounds(INDENT, polygon.Bottom + INDENT, btnWidth, btnHeight);


            msg = new Label();
            msg.SetBounds(INDENT, fiilCurve.Bottom + INDENT, btnWidth, btnHeight);
            msg.TextAlign = ContentAlignment.MiddleCenter;
            #endregion

            #region Мальберт
            p.SetBounds(addPoint.Right + INDENT, INDENT, ClientSize.Width - addPoint.Width - 3 * INDENT, ClientSize.Height - 2 * INDENT);
            p.BorderStyle = BorderStyle.FixedSingle;
            p.Paint += DrawSomethings;
            p.MouseClick += AddPoint;
            
            baseBackColor = p.BackColor;
            #endregion


            #region Обработчики событий
            KeyPreview = true;
            KeyDown += PushKeys;
            
            #endregion




            #region Добавление кнопок в форму
            this.Controls.Add(addPoint);
            this.Controls.Add(param);
            this.Controls.Add(move);
            this.Controls.Add(clear);
            this.Controls.Add(curve);
            this.Controls.Add(bezier);
            this.Controls.Add(polygon);
            this.Controls.Add(fiilCurve);
            this.Controls.Add(msg);
            this.Controls.Add(p);
            #endregion
        }

        /// <summary>
        /// Добавление точек с флагами.
        /// </summary>
        private void AddPoint(object sender, MouseEventArgs e)
        {
            
            var t = e.Clicks;
            if (editPoints)
            {
                points.Add(e.Location);
                flags.Add(true);
            }
            else
            {
                if (points.Count > 0)
                {
                    bool flag = false;
                    int epsilone = 15;
                    var w = e.Location;
                    for (int i = 0; i < points.Count; i++)
                    {
                        
                        if ((points[i].X + epsilone > e.X && points[i].X - epsilone < e.X) && (points[i].Y + epsilone > e.Y && points[i].Y - epsilone < e.Y))
                        {
                            int index = -1;
                            Point item = points[i];
                            p.MouseDown += (o1,e1) => { flag = true; index = points.FindIndex(x => x.X == points[i].X && x.Y == points[i].Y); points.Remove(points[i]); };
                            p.MouseMove += (o2, e2) =>
                            {
                                if (flag)
                                {
                                    p.CreateGraphics().DrawEllipse(Pens.Red, e2.X, e2.Y, 10,10);
                                    p.Refresh();
                                }
                            };
                            p.MouseDown += (o3,e3) => { flag = false;  points.Insert(index, item); };
                        }
                    }

                    
                    
                }
                
            }
            p.Refresh();
        }


        private void PushKeys(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                    
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    (sender as Task4).p.CreateGraphics().Clear(baseBackColor);
                    points.Clear();
                    e.Handled = true;
                    break;
                case Keys.Space:
                    timer.Enabled = !timer.Enabled;
                    e.Handled = true;
                    break;
                case Keys.Add:
                case Keys.Oemplus:
                    if (timer.Enabled)
                    {
                        timer.Interval += 10;
                        msg.Text = $" Скорость: {timer.Interval} мс";
                       
                    }
                    e.Handled = true;
                    return;
                case Keys.Subtract:
                case Keys.OemMinus:
                    //timer.Interval -= 10;
                    //e.Handled = true;
                    timer.Interval = (timer.Interval - 10 > 0) ? timer.Interval -= 10 : 1;
                    msg.Text = (timer.Interval <= 1) ? "Макс. скорость" : $" Скорость: {timer.Interval} мс";
                    e.Handled = true;
                    return;
                default:
                    break;
            }
        }

        /// <summary>
        /// Реализация движения.
        /// </summary>
        private void MoveFigure(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int xStep = rnd.Next(0, 50);
            int yStep = rnd.Next(0, 50);
            var square = p.ClientSize;
            
            if (points.Count > 0)
            {
                Point t = new Point();
                for (int i = 0; i < points.Count; i++)
                {
                    t.X = points[i].X;
                    t.Y = points[i].Y;

                    if (flags[i])
                    {
                        
                        if (t.X + xStep <= square.Width  || t.Y  + yStep <= square.Height )
                        {
                            if (t.X + xStep <= square.Width)
                            {
                                t.X += xStep;
                            }
                            else
                            {
                                t.X = square.Width- 3;
                                flags[i] = false;
                            }
                                
                            if (t.Y + yStep <= square.Height)
                            {
                                t.Y += yStep;
                            }
                            else
                            {
                                t.Y = square.Height-3;
                                flags[i] = false;

                            }
                            
                        }
                    }
                    else
                    {
                        if (t.X - xStep  >= 0 || t.Y - yStep  >= 0)
                        {
                            if (t.X - xStep >= 0)
                            {
                                t.X -= xStep;
                            }
                            else
                            {
                                t.X = 2;
                                flags[i] = true;
                            }
                                
                            if (t.Y - yStep >= 0)
                            {
                                t.Y -= yStep;
                            }
                            else
                            {
                                t.Y = 2;
                                flags[i] = true;
                            }
                        }
                    }
                    points[i] = t;
                    p.Refresh();
                }
            }
        }
        
        /// <summary>
        /// Отрисовка фигур.
        /// </summary>
        private void DrawSomethings(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            if (points.Count > 0)
            {
                foreach (var point in points)
                {
                    g.FillEllipse(Brushes.Green, point.X, point.Y, 8, 8);
                }
            }
            if ((sender as PictureBox).Tag != null)
            {
                switch ((LineType)(sender as PictureBox).Tag)
                {
                    case LineType.None:
                        //TODO проработать,если не нужно постоянное отображение точек.
                        break;
                    case LineType.Curve:
                        if (points.Count > 2)
                        {
                            //foreach (var point in points)
                            //{
                            //    g.FillEllipse(Brushes.Green, point.X, point.Y, 8, 8);
                            //}
                            g.DrawClosedCurve(Pens.Blue, points.ToArray());
                            msg.Text = "";
                        }
                        else
                        {
                            msg.Text = $"Добавте {3 - points.Count} точек";
                        }
                        break;
                    case LineType.Bezier:
                        // 4 points need
                        
                        if (points.Count % 3 - 1 == 0)
                        {
                            msg.Text = "";
                            g.Clear(baseBackColor);
                            for (int i = 0; i < points.Count; i++)
                            {
                                if (i % 3 == 0)
                                {
                                    g.FillEllipse(Brushes.Green, points[i].X, points[i].Y, 8, 8);
                                }
                            }

                            g.DrawBeziers(Pens.Red, points.ToArray());
                        }
                        else
                        {
                            msg.Text = $"Error 404";
                        }
                        //TODO если точек больше 4ех
                        break;
                    case LineType.Polygon:
                        // 2 points need
                        if (points.Count > 2)
                        {
                            //foreach (var point in points)
                            //{
                            //    g.FillEllipse(Brushes.Green, point.X, point.Y, 8, 8);
                            //}
                            g.DrawPolygon(Pens.Green, points.ToArray());
                            msg.Text = "";
                        }
                        else
                        {
                            msg.Text = $"Добавте {3 - points.Count} точек";
                            
                        }
                        break;
                    case LineType.FilledCurve:
                        // 3 points need
                        if (points.Count > 2)
                        {
                            //foreach (var point in points)
                            //{
                            //    g.FillEllipse(Brushes.Green, point.X, point.Y, 8, 8);
                            //}
                            g.FillClosedCurve(Brushes.Green, points.ToArray());
                            msg.Text = "";
                        }
                        else
                        {
                            msg.Text = $"Добавте {3 - points.Count} точек";
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Обработка нажатий кнопок. 
        /// </summary>
        private void DrawLineType(object sender, EventArgs e)
        {
            switch ((LineType)(((int, PictureBox))(sender as Button).Tag).Item1)
            {
                case LineType.None:
                case LineType.Curve:
                case LineType.Bezier:
                case LineType.Polygon:                    
                case LineType.FilledCurve:
                    p.Tag = (LineType)(((int, PictureBox))(sender as Button).Tag).Item1;
                    p.Refresh();
                    break;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:

                    return true;
                    
                case Keys.Space:
                case Keys.Escape:
                case Keys.Subtract:
                case Keys.OemMinus:
                case Keys.Add:
                case Keys.Oemplus:
                    this.Text = "Нажали "+ keyData;
                    return false;
                default:
                    return true;
            }
        }

    }
}
