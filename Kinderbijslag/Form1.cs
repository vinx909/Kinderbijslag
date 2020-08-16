using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinderbijslag
{
    public partial class Form1 : Form
    {
        private const int youngAgeUpperLimit = 12;
        private const int olderAgeUpperLimit = 18;
        private const int youngAgeAmount = 150;
        private const int oldAgeAmount = 235;
        private static readonly double[,] persentagGrowth = { { 3, 2 }, { 5, 3 }, { 6, 3.5 } };
        private const double totalPersentage = 100;

        private const string currentDatelabelText = "current date:";
        private const string addChildButtonText = "add child";
        private const string calculateChildBenefitsButtonText = "calculate Child Benefits";
        private const string parentalContributionMessageBoxText = "Child Benefits: ";

        private const int widthMargin = 10;
        private const int heightMargin = 10;
        private const int rowHeight = 30;

        private Date currentDate;
        private List<ChildBirthDate> childBirthDates;
        private Button addChildButton;
        private Button calculateChildBenefitsButton;

        public static double[,] PersentagGrowth => persentagGrowth;

        public Form1()
        {
            InitializeComponent();
            CurrentDateInitialize();
            ChildBirthDatesInitialize();
            AddChildButtonInitialize();
            CalculateChildBenefitsButtonInitialize();
            ResetPositions();
        }

        private void CurrentDateInitialize()
        {
            currentDate = new Date(this, currentDatelabelText);
        }

        private void ChildBirthDatesInitialize()
        {
            childBirthDates = new List<ChildBirthDate>();
            ChildBirthDatesAddNew();
        }
        private void ChildBirthDatesAddNew()
        {
            childBirthDates.Add(new ChildBirthDate(this));
        }
        private void AddChildButtonInitialize()
        {
            addChildButton = new Button();
            addChildButton.Text = addChildButtonText;
            addChildButton.Click += new EventHandler(ButtonFunctionAddChild);
            Controls.Add(addChildButton);
        }
        private void CalculateChildBenefitsButtonInitialize()
        {
            calculateChildBenefitsButton = new Button();
            calculateChildBenefitsButton.Text = calculateChildBenefitsButtonText;
            calculateChildBenefitsButton.Click += new EventHandler(ButtonFunctionCalculateParentalContribution);
            Controls.Add(calculateChildBenefitsButton);
        }
        internal void ChildBirthDatesRemove(ChildBirthDate toRemove)
        {
            childBirthDates.Remove(toRemove);
            ResetPositions();
        }

        private double CalculateChildBenefits()
        {
            double total = 0;
            int numberOfChilderen = 0;
            int[] currentDate = this.currentDate.GetDate();
            int yearIndex = Date.GetYearIndex();
            int monthIndex = Date.GetMonthIndex();
            int dayIndex = Date.GetDayIndex();
            foreach (ChildBirthDate childBirthDate in childBirthDates)
            {
                int[] birthDate = childBirthDate.GetDate();
                if (birthDate[yearIndex] > currentDate[yearIndex] - youngAgeUpperLimit || (birthDate[yearIndex] >= currentDate[yearIndex] - youngAgeUpperLimit && birthDate[monthIndex] < currentDate[monthIndex]) || (birthDate[yearIndex] >= currentDate[yearIndex] - youngAgeUpperLimit && birthDate[monthIndex] <= currentDate[monthIndex] && birthDate[dayIndex] <= currentDate[dayIndex]))
                {
                    total += youngAgeAmount;
                    numberOfChilderen++;
                }
                else if (birthDate[yearIndex] > currentDate[yearIndex] - olderAgeUpperLimit || (birthDate[yearIndex] >= currentDate[yearIndex] - olderAgeUpperLimit && birthDate[monthIndex] < currentDate[monthIndex]) || (birthDate[yearIndex] >= currentDate[yearIndex] - olderAgeUpperLimit && birthDate[monthIndex] <= currentDate[monthIndex] && birthDate[dayIndex] <= currentDate[dayIndex]))
                {
                    total += oldAgeAmount;
                    numberOfChilderen++;
                }
            }
            double maxPersentagePart = 0;
            double persentageIncrease = 0;
            for(int i=0;i< persentagGrowth.GetLength(0); i++)
            {
                if(persentagGrowth[i,0]<= numberOfChilderen&& persentagGrowth[i, 0] > maxPersentagePart)
                {
                    maxPersentagePart = persentagGrowth[i, 0];
                    persentageIncrease = persentagGrowth[i, 1];
                }
            }
            return total / totalPersentage * (totalPersentage + persentageIncrease);
        }

        private void ResetPositions()
        {
            int numberOfRowsDown = 0;
            currentDate.ChangePosition(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
            numberOfRowsDown++;
            foreach (ChildBirthDate childBirthDate in childBirthDates)
            {
                childBirthDate.ChangePosition(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
                numberOfRowsDown++;
            }
            addChildButton.Location = new Point(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
            numberOfRowsDown++;
            calculateChildBenefitsButton.Location = new Point(widthMargin, heightMargin + rowHeight * numberOfRowsDown);
        }

        private void ButtonFunctionAddChild(object sender, EventArgs e)
        {
            ChildBirthDatesAddNew();
            ResetPositions();
        }
        private void ButtonFunctionCalculateParentalContribution(object sender, EventArgs e)
        {
            MessageBox.Show(parentalContributionMessageBoxText + CalculateChildBenefits());
        }

        internal class Date
        {
            protected const int textBoxOfset = 100;
            protected const int textBoxBetweenOfset = 30;
            protected const int textBoxWidth = 25;

            protected const int dateArrayLength = 3;
            protected const int dateDayIndex = 0;
            protected const int dateMonthIndex = 1;
            protected const int dateYearIndex = 2;

            protected Label label;
            protected TextBox textBoxDay;
            protected TextBox textBoxMonth;
            protected TextBox textBoxYear;

            protected Form1 form;

            internal Date(Form1 form, string labelText)
            {
                this.form = form;

                label = new Label();
                label.Text = labelText;
                form.Controls.Add(label);

                textBoxDay = new TextBox();
                textBoxDay.Width = textBoxWidth;
                form.Controls.Add(textBoxDay);

                textBoxMonth = new TextBox();
                textBoxMonth.Width = textBoxWidth;
                form.Controls.Add(textBoxMonth);

                textBoxYear = new TextBox();
                textBoxYear.Width = textBoxWidth;
                form.Controls.Add(textBoxYear);
            }

            internal void ChangePosition(int widthOfset, int heightOfset)
            {
                CorrectPosition(widthOfset, heightOfset);
            }
            protected void CorrectPosition(int widthOfset, int heightOfset)
            {
                label.Location = new Point(widthOfset, heightOfset);
                textBoxDay.Location = new Point(widthOfset + textBoxOfset, heightOfset);
                textBoxMonth.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset, heightOfset);
                textBoxYear.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset * 2, heightOfset);
            }
            internal int[] GetDate()
            {
                int[] date = new int[dateArrayLength];
                date[dateDayIndex] = int.Parse(textBoxDay.Text);
                date[dateMonthIndex] = int.Parse(textBoxMonth.Text);
                date[dateYearIndex] = int.Parse(textBoxYear.Text);
                return date;
            }
            internal static int GetYearIndex()
            {
                return dateYearIndex;
            }
            internal static int GetMonthIndex()
            {
                return dateMonthIndex;
            }
            internal static int GetDayIndex()
            {
                return dateDayIndex;
            }
        }
        internal class ChildBirthDate : Date
        {
            private const string labelText = "date of childs birth (day-month-year):";
            private const string buttonText = "remove";

            private Button button;

            internal ChildBirthDate(Form1 form) : base(form, labelText)
            {
                label.Text = labelText;

                button = new Button();
                button.Text = buttonText;
                button.Click += new EventHandler(ButtonFunction);
                form.Controls.Add(button);
            }
            private void ButtonFunction(object sender, EventArgs e)
            {
                form.Controls.Remove(label);
                form.Controls.Remove(textBoxDay);
                form.Controls.Remove(textBoxMonth);
                form.Controls.Remove(textBoxYear);
                form.Controls.Remove(button);
                form.ChildBirthDatesRemove(this);
            }
            internal void ChangePosition(int widthOfset, int heightOfset)
            {
                CorrectPosition(widthOfset, heightOfset);
                button.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset * 3, heightOfset);
            }
        }
    }
}
