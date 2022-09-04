using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Actions
{
    
    public class Task4:Form
    {
        #region Consts
        private const int INDENT = 9;
        private const int btnWidth = 90;
        private const int btnHeight = 40;
        #endregion

        #region Fields
        /// <summary>
        /// Список рисуемых объектов.
        /// </summary>
        public enum LineType { Point = 1, Curve = 2, Bezier = 3, Polygon =4 , FilledCurve =5 }
        /// <summary>
        /// Тип ручного перемещения объекта.
        /// </summary>
        enum TypeHandChangePositionObject
        { 
            /// <summary>
            /// Смещение вправо.
            /// </summary>
            xR = 1,
            /// <summary>
            /// Смещение влево.
            /// </summary>
            xL,
            /// <summary>
            /// Смещение вверх.
            /// </summary>
            yU,
            /// <summary>
            /// Смещение вниз.
            /// </summary>
            yD
        }

        /// <summary>
        /// Коллекция точек.
        /// </summary>
        private List<Point> points;
        /// <summary>
        /// Состояние отражения точек.
        /// </summary>
        private List<bool> flags;
        /// <summary>
        /// Базовый цвет фона плоскости отрисовки.
        /// </summary>
        private Color baseBackColor;
        /// <summary>
        /// Флаг режима добавления точек.
        /// </summary>
        private bool editPoints = false;
        /// <summary>
        /// Флаг редактирования положения точки.
        /// </summary>
        private bool flagMOves = false;
        /// <summary>
        /// Плоскость рисования.
        /// </summary>
        private PictureBox p;
        /// <summary>
        /// Прорисовка объектов.
        /// </summary>
        private Timer timer;
        /// <summary>
        /// Оповещение пользователя.
        /// </summary>
        private Label msg;
        #endregion
        public Task4()
        {
            //TODO сделать реализацию параметров, добавить там флорму для показа и изменения координат всех точек, там же сделать сохранение и залгрузку точек.

            points = new List<Point>();
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
            param.Click += SetParams;





            Button move = new Button() { Text = "Движение" };
            move.SetBounds(INDENT, param.Bottom + INDENT, btnWidth, btnHeight);
            move.Click += (o,e) => { timer.Enabled = !timer.Enabled; };


            Button clear = new Button() { Text = "Очистить"};
            clear.SetBounds(INDENT, move.Bottom + INDENT, btnWidth, btnHeight);
            clear.Tag = p;
            clear.Click += (o, e) => { ((o as Button).Tag as PictureBox).CreateGraphics().Clear(baseBackColor); points.Clear(); };
            
            Button curve = new Button() { Text = "Кривая"};
            curve.SetBounds(INDENT, clear.Bottom + INDENT, btnWidth, btnHeight);
            curve.Tag = 2;
            curve.Click += DrawLineType;

            Button bezier = new Button() { Text = "Безье"};
            bezier.Tag = 3;
            bezier.Click += DrawLineType;
            bezier.SetBounds(INDENT, curve.Bottom + INDENT, btnWidth, btnHeight);

            Button polygon = new Button() { Text = "Ломанная"};
            polygon.Tag = 4;
            polygon.Click += DrawLineType;
            polygon.SetBounds(INDENT, bezier.Bottom + INDENT, btnWidth, btnHeight);

            Button fiilCurve = new Button() { Text = "Закрашенная"};
            fiilCurve.Tag = 5;
            fiilCurve.Click += DrawLineType;
            fiilCurve.SetBounds(INDENT, polygon.Bottom + INDENT, btnWidth, btnHeight);


            msg = new Label();
            msg.SetBounds(INDENT, fiilCurve.Bottom + INDENT, btnWidth, btnHeight - 10);
            msg.TextAlign = ContentAlignment.MiddleCenter;
            
            CheckBox bufferDraw = new CheckBox() { Text = "Buffer выкл" };
            bufferDraw.SetBounds(INDENT, msg.Bottom + 4, btnWidth, 20);
            
            bufferDraw.Click += (o, e) =>
            {
                (o as CheckBox).Checked = (o as CheckBox).Checked;
                (o as CheckBox).Text = ((o as CheckBox).Checked) ? "Buffer вкл" : "Buffer выкл";
                DoubleBuffered = (o as CheckBox).Checked;
            };
            
            #endregion

            #region Мальберт
            p.SetBounds(addPoint.Right + INDENT, INDENT, ClientSize.Width - addPoint.Width - 3 * INDENT, ClientSize.Height - 2 * INDENT);
            p.BorderStyle = BorderStyle.FixedSingle;
            baseBackColor = p.BackColor;
            #endregion

            #region Обработчики событий
            KeyPreview = true;
            KeyDown += PushKeys;
            p.Paint += DrawSomethings;
            p.MouseDown += AddPoint;
            p.MouseMove += Mover;
            p.MouseUp += EndMover;

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
            this.Controls.Add(bufferDraw);
            #endregion
        }

        private void SetParams(object sender, EventArgs e)
        {
            Parametrs param = new Parametrs();
            if (param.ShowDialog() == DialogResult.OK)
            {
                
                p.Refresh();
            }
        }

        /// <summary>
        /// Редактирование положения точки.
        /// </summary>
        private void Mover(object sender, MouseEventArgs e)
        {
            if (flagMOves)
            {
                p.CreateGraphics().FillEllipse(Brushes.Red, e.X, e.Y, 10, 10);
                Point temp = points[(int)msg.Tag];
                temp.X += e.X;
                temp.Y += e.Y;                
                points[(int)msg.Tag] = temp;
                p.Refresh();
            }
        }
        /// <summary>
        /// Завершение редактироания положения точки.
        /// </summary>
        private void EndMover(object sender, MouseEventArgs e)
        {
            if (flagMOves)
            {
                flagMOves = false;
                Point temp = points[(int)msg.Tag];
                temp.X = e.X;
                temp.Y = e.Y;
                points[(int)msg.Tag] = temp;
                msg.Text = "Перемещена";
                p.Refresh();
            }
        }
        /// <summary>
        /// Добавление точек с флагами.
        /// </summary>
        private void AddPoint(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (points.Count > 0 && CheckPoint(e, out int index))
                {
                    msg.Tag = index;
                    msg.Text = "Перемещается " + (index + 1) + " точка";
                    flagMOves = true;
                }
                else
                {
                    if (editPoints)
                    {
                        points.Add(new Point(e.X, e.Y));
                        flags.Add(true);
                        p.Refresh();
                    }
                }
                
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CheckPoint(e, out int index))
                {
                    points.RemoveAt(index);
                    flags.RemoveAt(index);
                    msg.Text = "Удалена " + (index + 1) + " точка";
                }
                p.Refresh();

            }
            
            
        }
        /// <summary>
        /// Проверка точек.
        /// </summary>
        private bool CheckPoint(MouseEventArgs e, out int index)
        {
            for (int i = 0; i < points.Count; i++)
            {
                int epsilone = 25;
                if ((
                    points[i].X + epsilone > e.X
                    && points[i].X - epsilone < e.X)
                    && (points[i].Y + epsilone > e.Y &&
                    points[i].Y - epsilone < e.Y))
                {
                    index = points.FindIndex(x => x.X == points[i].X && x.Y == points[i].Y);
                    return true;
                }
            }
            index = -1;
            return false;
        }
        /// <summary>
        /// Обработчик клавиш.
        /// </summary>
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
            Pen pen = Pens.Green;
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
                    case LineType.Point:
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
                            g.DrawPolygon(pen, points.ToArray());
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
            switch ((LineType)(sender as Button).Tag)
            {
                case LineType.Point:
                case LineType.Curve:
                case LineType.Bezier:
                case LineType.Polygon:                    
                case LineType.FilledCurve:
                    // Добавляем в мальберт объект лперечисления LineType
                    p.Tag = (LineType)(sender as Button).Tag;
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
                    HandMoveObj(keyData);
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
        /// <summary>
        /// Смещение объектов ручками.
        /// </summary>
        private void HandMoveObj(Keys keyData)
        {
            
            switch (keyData)
            {
                case Keys.Up:
                    NewMethod(TypeHandChangePositionObject.yU);
                    break;
                case Keys.Down:
                    NewMethod(TypeHandChangePositionObject.yD);
                    break;
                case Keys.Right:
                    NewMethod(TypeHandChangePositionObject.xR);
                    break;
                case Keys.Left:
                    NewMethod(TypeHandChangePositionObject.xL);
                    break;
                default:
                    break;
            }
            
        }
        /// <summary>
        /// Сдвиг объекта стрелками.
        /// </summary>
        private void NewMethod(TypeHandChangePositionObject yU)
        {
            int delta = 10;
            Point temp;
            var area = p.ClientSize;
            for (int i = 0; i < points.Count; i++)
            {
                temp = points[i];
                switch (yU)
                {
                    case TypeHandChangePositionObject.xR:
                        if (temp.X + delta < area.Width)
                        {
                            temp.X += delta;
                        }
                        else
                            temp.X = area.Width - 5;

                        break;
                    case TypeHandChangePositionObject.xL:
                        if (temp.X - delta > 0)
                        {
                            temp.X -= delta;
                        }
                        else
                            temp.X = 0;
                        break;
                    case TypeHandChangePositionObject.yU:

                        if (temp.Y - delta > 0)
                        {
                            temp.Y -= delta;
                        }
                        else
                            temp.Y = 0;
                        break;
                    case TypeHandChangePositionObject.yD:
                        if (temp.Y + delta < area.Height)
                        {
                            temp.Y += delta;
                        }
                        else
                            temp.Y = area.Height - 5;
                        break;
                }
                points[i] = temp;
                p.Refresh();
            }
        }
    }
}
