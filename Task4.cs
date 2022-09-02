using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Actions
{
    
    public class Task4:Form
    {
        #region Consts
        private const int INDENT = 15;
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
        #endregion


        public Task4()
        {
            points = new List<Point>();// { new Point(38,28), new Point(479, 128), new Point(397, 256), new Point(38, 128) };
            flags = new List<bool>();
            p = new PictureBox();

            timer = new Timer();
            timer.Interval = 30;
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
            move.Click += (o, e) => { timer.Enabled = !timer.Enabled; };


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
            #endregion

            #region Мальберт
            p.SetBounds(addPoint.Right + INDENT, INDENT, ClientSize.Width - addPoint.Width - 3 * INDENT, ClientSize.Height - 2 * INDENT);
            p.BorderStyle = BorderStyle.FixedSingle;
            p.Paint += DrawSomethings;
            p.MouseClick += AddPoint;
            baseBackColor = p.BackColor;
            #endregion


            #region Обработчики событий

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
            this.Controls.Add(p);
            #endregion
        }

        private void MoveFigure(object sender, EventArgs e)
        {
            int easyStep = 10;
            var square = p.ClientSize;
            Random rnd = new Random();
            //TODO обдумать над способом генерирования новых координат у точек
            if (points.Count > 0)
            {
                Point t = new Point();
                for (int i = 0; i < points.Count; i++)
                {
                    t.X = points[i].X;
                    t.Y = points[i].Y;

                    if (flags[i])
                    {
                        //t.X += easyStep;
                        //t.Y += easyStep;


                        t.X += rnd.Next(0, 50);
                        t.Y += rnd.Next(0, 50);

                        points[i] = t;
                        flags[i] = points[i].X <= square.Width && points[i].Y <= square.Height;
                    }
                    else
                    {
                        //t.X = t.X - easyStep;
                        //t.Y = t.Y - easyStep;

                        t.X -= rnd.Next(0, 50);
                        t.Y -= rnd.Next(0, 50);
                        points[i] = t;
                        flags[i] = points[i].X <= 0 || points[i].Y <= 0;
                    }
                    p.Refresh();
                }
            }
        }

        private void AddPoint(object sender, MouseEventArgs e)
        {
            if (editPoints)
            {
                points.Add(e.Location);
                flags.Add(true);
            }
            (sender as PictureBox).Refresh();
        }

        private void DrawSomethings(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            if (points.Count > 0)
            {
                foreach (var point in points)
                {
                    g.FillEllipse(Brushes.Green, point.X, point.Y, 8,8);
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
                            g.DrawClosedCurve(Pens.Blue, points.ToArray());
                        }
                        //TODO ввести подсказку для пользователя, если точек мало
                        break;
                    case LineType.Bezier:
                        // 4 points need
                        if (points.Count % 3 - 1 == 0)
                        {
                            g.DrawBeziers(Pens.Red, points.ToArray());
                        }
                        //TODO если точек больше 4ех
                        break;
                    case LineType.Polygon:
                        // 2 points need
                        if (points.Count > 3)
                        {
                            g.DrawPolygon(Pens.Green, points.ToArray());
                        }
                        break;
                    case LineType.FilledCurve:
                        // 3 points need
                          
                        if (points.Count > 2)
                        {
                            g.FillClosedCurve(Brushes.Green, points.ToArray());
                        }
                        break;
                    default:
                        break;
                }
            }
            


        }

        /// <summary>
        /// Отрисовка объекта 
        /// </summary>
        /// <param name="sender">Объект создавший событие.</param>
        /// <param name="e">Параметры.</param>
        private void DrawLineType(object sender, EventArgs e)
        {
            PictureBox temp;
            switch ((LineType)(((int, PictureBox))(sender as Button).Tag).Item1)
            {
                case LineType.None:
                    temp = (((int, PictureBox))(sender as Button).Tag).Item2;
                    temp.Tag = (LineType)(((int, PictureBox))(sender as Button).Tag).Item1;
                    temp.Refresh();
                    break;
                case LineType.Curve:
                    temp = (((int, PictureBox))(sender as Button).Tag).Item2;
                    temp.Tag = (LineType)(((int, PictureBox))(sender as Button).Tag).Item1;
                    temp.Refresh();
                    break;
                case LineType.Bezier:
                    temp = (((int, PictureBox))(sender as Button).Tag).Item2;
                    temp.Tag = (LineType)(((int, PictureBox))(sender as Button).Tag).Item1;
                    temp.Refresh();
                    break;
                case LineType.Polygon:
                    temp = (((int, PictureBox))(sender as Button).Tag).Item2;
                    temp.Tag = (LineType)(((int, PictureBox))(sender as Button).Tag).Item1;
                    temp.Refresh();
                    break;
                case LineType.FilledCurve:
                    temp = (((int, PictureBox))(sender as Button).Tag).Item2;
                    temp.Tag = (LineType)(((int, PictureBox))(sender as Button).Tag).Item1;
                    temp.Refresh();
                    break;
                default:
                    break;
            }
        }
    }
}
