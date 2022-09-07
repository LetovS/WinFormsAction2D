using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using static Actions.Parametrs;

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
        public enum LineType { Point = 1, Curve = 2, Bezier = 3, Polygon =4 , FilledCurve = 5, None =6 }
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
        private (Timer, EventArgs) moverSet;
        private DrawSetting[] pointsSet;
        private Keys keys;
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

        private Timer timer;
        enum TypeMover { Auto = 1, Handle, None }
        /// <summary>
        /// Флаг редактирования положения точки.
        /// </summary>
        private bool flagMOves = false;
        
        
        /// <summary>
        /// Оповещение пользователя.
        /// </summary>
        private Label msg;
        /// <summary>
        /// Координаты перемещаемой точки.
        /// </summary>
        private Point movepoint;
        /// <summary>
        /// Использование буферизации.
        /// </summary>
        private CheckBox bufferDraw;
        /// <summary>
        /// Статусы клавишь
        /// </summary>
        Dictionary<LineType, bool> status =
                        new Dictionary<LineType, bool>
                        {
                            { LineType.Point, false },
                            { LineType.Curve, false},
                            { LineType.Bezier, false},
                            { LineType.Polygon, false},
                            { LineType.FilledCurve, false}
                        };
        #endregion
        public Task4()
        {
            pointsSet = new Parametrs().GetSettingsForDrawing();
            points = new List<Point>();
            flags = new List<bool>();
            PictureBox p = new PictureBox();

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
            addPoint.Tag = (LineType.Point,p);
            addPoint.Click += DrawLineType;
            

            Button param = new Button() { Text = "Параметры" };
            param.SetBounds(INDENT, addPoint.Bottom + INDENT, btnWidth, btnHeight);
            param.Click += SetParams;

            Button move = new Button() { Text = "Движение" };
            move.SetBounds(INDENT, param.Bottom + INDENT, btnWidth, btnHeight);
            move.Click += (o,e) => { timer.Enabled = !timer.Enabled; };


            Button clear = new Button() { Text = "Очистить"};
            clear.SetBounds(INDENT, move.Bottom + INDENT, btnWidth, btnHeight);
            
            clear.Click += (o, e) => { GetDrowObj().CreateGraphics().Clear(baseBackColor); points.Clear(); };
            
            Button curve = new Button() { Text = "Кривая"};
            curve.SetBounds(INDENT, clear.Bottom + INDENT, btnWidth, btnHeight);
            curve.Tag = (LineType.Curve, p);
            curve.Click += DrawLineType;

            Button bezier = new Button() { Text = "Безье"};
            bezier.Tag = (LineType.Bezier, p);
            bezier.Click += DrawLineType;
            bezier.SetBounds(INDENT, curve.Bottom + INDENT, btnWidth, btnHeight);

            Button polygon = new Button() { Text = "Ломанная"};
            polygon.Tag = (LineType.Polygon, p);
            polygon.Click += DrawLineType;
            polygon.SetBounds(INDENT, bezier.Bottom + INDENT, btnWidth, btnHeight);

            Button fiilCurve = new Button() { Text = "Закрашенная"};
            fiilCurve.Tag = (LineType.FilledCurve, p);
            fiilCurve.Click += DrawLineType;
            fiilCurve.SetBounds(INDENT, polygon.Bottom + INDENT, btnWidth, btnHeight);


            msg = new Label();
            msg.SetBounds(INDENT, fiilCurve.Bottom + INDENT, btnWidth, btnHeight - 10);
            msg.TextAlign = ContentAlignment.MiddleCenter;
            
            bufferDraw = new CheckBox() { Text = "Buffer выкл" };
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
            p.Tag = LineType.None;
            baseBackColor = p.BackColor;
            #endregion

            #region Обработчики событий
            this.Load += Task4_Load;
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

        private void Task4_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            KeyDown += PushKeys;
            foreach (var item in (sender as Task4).Controls)
            {
                if (item is PictureBox p)
                {
                    p.Paint += DrawFigures;
                    p.MouseDown += AddPoint;
                    p.MouseMove += Mover;
                    p.MouseUp += EndMover;
                    return;
                }
            }
            
        }

        /// <summary>
        /// Получение настроек для отрисовки.
        /// </summary>
        private void SetParams(object sender, EventArgs e)
        {
            Parametrs param = new Parametrs();
            
            this.Visible = false;
            if (param.ShowDialog() == DialogResult.OK)
            {
                pointsSet = param.GetSettingsForDrawing();
                
            }
            this.Visible = true;
        }
        /// <summary>
        /// Редактирование положения точки.
        /// </summary>
        private void Mover(object sender, MouseEventArgs e)
        {
            if (flagMOves)
            {
                (sender as PictureBox).CreateGraphics().FillEllipse(Brushes.Red, e.X, e.Y, 10, 10);
                movepoint = new Point(e.X, e.Y);
                (sender as PictureBox).Refresh();
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
                points[(int)msg.Tag] = movepoint;
                movepoint = new Point(0,0);
                msg.Text = "Перемещена";
                (sender as PictureBox).Refresh();
            }
        }
        /// <summary>
        /// Добавление точек с флагами.
        /// </summary>
        private void AddPoint(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (points.Count > 0 && CheckPoint(e, out int index) && status[LineType.Point])
                {
                    msg.Tag = index;
                    msg.Text = "Перемещается " + (index + 1) + " точка";
                    movepoint = new Point(e.X, e.Y);
                    flagMOves = true;
                }
                else
                {
                    if (status[LineType.Point])
                    {
                        points.Add(new Point(e.X, e.Y));
                        flags.Add(true);
                    }
                    
                }
                (sender as PictureBox).Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CheckPoint(e, out int index))
                {
                    points.RemoveAt(index);
                    flags.RemoveAt(index);
                    msg.Text = "Удалена " + (index + 1) + " точка";
                }
                (sender as PictureBox).Refresh();

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
                case Keys.Escape:
                    PictureBox drawObj = GetDrowObj();
                    if (drawObj!=null)
                    {
                        drawObj.CreateGraphics().Clear(baseBackColor);
                        points.Clear();
                        e.Handled = true;
                    }
                    break;
                case Keys.Space:
                    timer.Tag = TypeMover.Auto;
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
                    timer.Interval = (timer.Interval - 10 > 0) ? timer.Interval -= 10 : 1;
                    msg.Text = (timer.Interval <= 1) ? "Макс. скорость" : $" Скорость: {timer.Interval} мс";
                    e.Handled = true;
                    return;
                case Keys.B:
                    bufferDraw.Checked = !bufferDraw.Checked;
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Реализация движения.
        /// </summary>
        private void MoveFigure(object sender, EventArgs e)
        {
            moverSet = (sender as Timer, e);
            TypeMover type = (TypeMover)(sender as Timer).Tag;
            PictureBox p = GetDrowObj();    

            if (p != null)
            {
                var square = p.ClientSize;
                switch (type)
                {
                    case TypeMover.Auto:
                        Random rnd = new Random();
                        int xStep = rnd.Next(0, 50);
                        int yStep = rnd.Next(0, 50);

                        if (points.Count > 0)
                        {
                            Point t = new Point();
                            for (int i = 0; i < points.Count; i++)
                            {
                                t.X = points[i].X;
                                t.Y = points[i].Y;
                                CheckReflection(xStep, yStep, square, ref t, i);

                                points[i] = t;
                                p.Refresh();
                            }
                        }
                        break;
                    case TypeMover.Handle:
                        var k = (TypeMover)(sender as Timer).Tag;
                        HandMoveObj(keys);
                        timer.Enabled = !timer.Enabled;
                        break;
                }
            }   
        }
        /// <summary>
        /// Проверка на достижение границ.
        /// </summary>
        /// <param name="xStep">Шаг по оси Х.</param>
        /// <param name="yStep">Шаг по оси Y.</param>
        /// <param name="square"></param>
        /// <param name="t"></param>
        /// <param name="i"></param>
        private Point CheckReflection(int xStep, int yStep, Size square, ref Point t, int i)
        {
            if (flags[i])
            {
                if (t.X + xStep <= square.Width || t.Y + yStep <= square.Height)
                {
                    if (t.X + xStep <= square.Width)
                    {
                        t.X += xStep;
                    }
                    else
                    {
                        t.X = square.Width - 3;
                        flags[i] = false;
                    }

                    if (t.Y + yStep <= square.Height)
                    {
                        t.Y += yStep;
                    }
                    else
                    {
                        t.Y = square.Height - 3;
                        flags[i] = false;
                    }

                }
                
            }
            else
            {
                if (t.X - xStep >= 0 || t.Y - yStep >= 0)
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
            return new Point(t.X, t.Y);
        }

        /// <summary>
        /// Отрисовка фигур.
        /// </summary>
        private void DrawFigures(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            foreach (KeyValuePair<LineType, bool> modeDrawing in status)
            {
                if (modeDrawing.Value)
                {
                    DrawingObjects(modeDrawing.Key, g);// Метод отрисовки по требуемого типу LineType
                }
            }
        }
        /// <summary>
        /// Отрисовка объектов
        /// </summary>
        private void DrawingObjects(LineType key, Graphics g)
        {
            
            switch (key)
            {
                case LineType.Point:
                    // Если статус тру то дабавляем точки
                    foreach (var item in points)
                    {
                        g.FillEllipse(new SolidBrush(pointsSet[0].color), item.X - pointsSet[0].size / 2, item.Y - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                    }
                    break;
                case LineType.Curve:
                    
                    if (points.Count > 2)
                    {
                        g.DrawClosedCurve(new Pen(pointsSet[1].color, pointsSet[1].size), points.ToArray());
                        msg.Text = "";
                    }
                    else
                    {
                        msg.Text = $"Добавте {3 - points.Count} точек";
                    }
                    break;
                case LineType.Bezier:
                    if (points.Count % 3 - 1 == 0)
                    {
                        msg.Text = "";
                        //g.Clear(baseBackColor);
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (i % 3 == 0)
                            {
                                g.FillEllipse(new SolidBrush(pointsSet[0].color), points[i].X - pointsSet[0].size / 2, points[i].Y - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                            }
                            else
                                g.FillEllipse(new SolidBrush(Color.Green), points[i].X - pointsSet[0].size / 2, points[i].Y - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                        }

                        g.DrawBeziers(new Pen(pointsSet[2].color, pointsSet[2].size), points.ToArray());
                    }
                    else
                    {
                        int val = points.Count % 3 - 1;
                        if (val == 1 || val == -1)
                        {
                            if (val == 1)
                            {
                                for (int i = 0; i < points.Count; i++)
                                {
                                    if (i < points.Count - 1)
                                    {
                                        g.FillEllipse(new SolidBrush(pointsSet[0].color), points[i].X - pointsSet[0].size / 2, points[i].Y - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                                    }
                                    else
                                    {
                                        g.FillEllipse(new SolidBrush(Color.Red), points[i].X - pointsSet[0].size / 2, points[i].Y - pointsSet[0].size / 2, pointsSet[0].size + 2, pointsSet[0].size + 2);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < points.Count; i++)
                                {
                                    if (i < points.Count - 2)
                                    {
                                        g.FillEllipse(new SolidBrush(pointsSet[0].color), points[i].X - pointsSet[0].size / 2, points[i].Y - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                                    }
                                    else
                                    {
                                        g.FillEllipse(new SolidBrush(Color.Red), points[i].X - pointsSet[0].size / 2, points[i].Y - pointsSet[0].size / 2, pointsSet[0].size + 2, pointsSet[0].size + 2);
                                    }
                                }
                            }

                        }

                        //TODO подумать как задавать надпись в информ метку
                        msg.Text = $"Error 404";
                    }
                    
                    break;
                case LineType.Polygon:
                    if (points.Count > 2)
                    {
                        g.DrawPolygon(new Pen(pointsSet[3].color, pointsSet[3].size), points.ToArray());
                        msg.Text = "";
                    }
                    else
                    {
                        msg.Text = $"Добавте {3 - points.Count} точек";
                    }
                    break;
                case LineType.FilledCurve:
                    if (points.Count > 2)
                    {
                        g.FillClosedCurve(new SolidBrush(pointsSet[4].color), points.ToArray());
                        msg.Text = "";
                    }
                    else
                    {
                        msg.Text = $"Добавте {3 - points.Count} точек";
                    }
                    break;
                case LineType.None:
                    //рисуем точки всегда
                    break;
               
            }
        }
        /// <summary>
        /// Обработка нажатий кнопок. 
        /// </summary>
        private void DrawLineType(object sender, EventArgs e)
        {
            var currentObj = sender as Button;

            LineType typeObj;
            PictureBox drawObj;

            (typeObj, drawObj) = (ValueTuple<LineType, PictureBox>)currentObj.Tag;
            
            switch (typeObj)
            {
                case LineType.Point:
                case LineType.Curve:
                case LineType.Bezier:
                case LineType.Polygon:                    
                case LineType.FilledCurve:
                    // Добавляем в мальберт объект лперечисления LineType
                    status[typeObj] = !status[typeObj];
                    //TODO Подумать как взаимно изменять щелчки клавиш и отображение фигур
                    //if (typeObj == LineType.FilledCurve)
                    //{
                    //    for (int i = 2; i < 5; i++)
                    //    {
                    //        LineType type = (LineType)i;
                    //        status[type] = (status[type]) ? !status[type] : false;
                    //    }
                    //}
                    currentObj.ForeColor = (status[typeObj]) ? Color.Red : Color.Black;
                    drawObj.Tag = (status[typeObj]) ? typeObj: LineType.None;
                    drawObj.Refresh();
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
                    timer.Tag = TypeMover.Handle;
                    timer.Enabled = !timer.Enabled;
                    keys = keyData;
                    MoveFigure(timer, moverSet.Item2);
                    return true;
                    
                case Keys.Space:
                case Keys.Escape:
                case Keys.Subtract:
                case Keys.OemMinus:
                case Keys.Add:
                case Keys.Oemplus:
                case Keys.B:
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
            //TODO с кооперировать с методом MoveFigure и реализовать выбор по типу движения ручками вс автоматически
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
            //TODO Ввести проверку положения точек при движении
            int delta = 10;
            Point temp;
            PictureBox p = GetDrowObj();
            if (p != null)
            {
                var area = p.ClientSize;

                for (int i = 0; i < points.Count; i++)
                {
                    temp = points[i];
                    switch (yU)
                    {
                        case TypeHandChangePositionObject.xR:

                            temp = CheckReflection(delta, 0, area, ref temp, i);

                            //if (temp.X + delta < area.Width)
                            //{
                            //    temp.X += delta;
                            //}
                            //else
                            //    temp.X = area.Width - 5;

                            break;
                        case TypeHandChangePositionObject.xL:
                            temp = CheckReflection(-delta, 0, area, ref temp, i);
                            //if (temp.X - delta > 0)
                            //{
                            //    temp.X -= delta;
                            //}
                            //else
                            //    temp.X = 0;
                            break;
                        case TypeHandChangePositionObject.yU:
                            temp = CheckReflection(0, -delta, area, ref temp, i);
                            //if (temp.Y - delta > 0)
                            //{
                            //    temp.Y -= delta;
                            //}
                            //else
                            //    temp.Y = 0;
                            break;
                        case TypeHandChangePositionObject.yD:
                            temp = CheckReflection(0, delta, area, ref temp, i);
                            //if (temp.Y + delta < area.Height)
                            //{
                            //    temp.Y += delta;
                            //}
                            //else
                            //    temp.Y = area.Height - 5;
                            break;
                    }
                    points[i] = temp;
                    p.Refresh();
                }
            }
            
        }
        /// <summary>
        /// Получение объекта отрисовки
        /// </summary>
        /// <returns>Graphics g</returns>
        private PictureBox GetDrowObj()
        {
            
            foreach (var item in this.Controls)
            {
                if (item is PictureBox drawObj)
                {
                    return drawObj;
                }
            }
            return null;
        }
        static void Main()
        {
            Application.Run(new Task4());
        }
    }
}
