using System;
using System.Globalization;
using System.Windows.Forms;
using Languages.Implementation;
using Languages.Interfaces;
using Timer = System.Timers.Timer;

namespace KWAnzeige
{
    public partial class Main : Form
    {
        private readonly ILanguageManager _lm = new LanguageManager();
        private readonly Timer _timerCw = new Timer();

        public Main()
        {
            InitializeComponent();
            InitializeLanguageManager();
            LoadLanguagesToCombo();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            textBoxCW.Text = GetIso8601WeekOfYear(DateTime.Now).ToString();
            _timerCw.Elapsed += TimerCW_Tick;
            _timerCw.Interval = 1000;
            _timerCw.Start();
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void TimerCW_Tick(object sender, EventArgs e)
        {
            textBoxCW.Text = GetIso8601WeekOfYear(DateTime.Now).ToString();
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                time = time.AddDays(3);
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }

        private void InitializeLanguageManager()
        {
            _lm.SetCurrentLanguage("de-DE");
            _lm.OnLanguageChanged += OnLanguageChanged;
        }

        private void LoadLanguagesToCombo()
        {
            foreach (var lang in _lm.GetLanguages())
                comboBoxLanguage.Items.Add(lang.Name);
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxLanguage.SelectedItem.ToString())
            {
                case "Deutsch":
                    _lm.SetCurrentLanguage("de-DE");
                    break;
                case "English (US)":
                    _lm.SetCurrentLanguage("en-US");
                    break;
            }
        }

        private void OnLanguageChanged(object sender, EventArgs eventArgs)
        {
            labelCW.Text = _lm.GetCurrentLanguage().GetWord("TodaysCW");
            Text = _lm.GetCurrentLanguage().GetWord("Title");
            buttonMinimize.Text = _lm.GetCurrentLanguage().GetWord("Minimize");
        }
    }
}