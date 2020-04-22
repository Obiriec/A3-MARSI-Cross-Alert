/* -------------------------------------------------------------------------------
 * 
 *    A3 MARSI Cross Alert
 *
 *    This indicator derives from the famous RSI-O-MA which is located at this link (https://ctrader.com/algos/indicators/show/2004)
 *    to which I added a sound "alert" that warns at each intersection of the two lines .
 *    The indicator is obtained with an RSI applied to an exponential moving average, with trigger.
 *
 *    The signals of this indicator are not to be confused with the classic RSI, they are very different and give us the opportunity
 *    to evaluate the formation of new minimums or maximums in the price trend.
 *
 *    This indicator was born from my idea of being able to have a very simple sound signal even if I am not right in front of the graph!
 *
 *    Happy trading everyone and ... like if you liked this idea, thank you !!!
 *    
 *    Author & Developer: Armando Brecciaroli (email: a.brecciaroli@me.com - Telegram: https://t.me/Obiriec)
 *    
 *    Changelog:
 *      v.1.0.0 (April 09, 2020) First release
 *      
 *          
 * -------------------------------------------------------------------------------
 */


using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Levels(20, 30, 70, 80)]

    [Indicator(IsOverlay = false, ScalePrecision = 0, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class A3_MARSI_Cross_Alert : Indicator
    {

// Obiriec -  Condition for sound alert
        bool b = true;
        bool a = true;
        [Parameter("Sound ON", DefaultValue = true)]
        public bool PlaySound { get; set; }
// Obiriec - Path where the sound file to be executed for the alert must be copied
        [Parameter("Media File Location", DefaultValue = "c:\\windows\\media\\Obiriec - A3 MARSI Cross Alert - Sounds\\Alert-1-Obiriec MARSI Cross.mp3")]
        public string MediaFile { get; set; }
// Obiriec - Logic parameters
        [Parameter("RSI Periods", Group = "RSI", DefaultValue = 20)]
        public int RSI_Periods { get; set; }

        [Parameter("Trigger Period", Group = "RSI", DefaultValue = 10)]
        public int Trigger_Periods { get; set; }

        [Parameter("MA Period", Group = "Best settings is Exponential MA", DefaultValue = 20)]
        public int EMA_Periods { get; set; }

        [Parameter("MA Type", Group = "Best settings is Exponential MA", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MaType { get; set; }

        [Parameter("MA Source", Group = "Best settings is Exponential MA")]
        public DataSeries Source { get; set; }

        [Output("Rsi", LineColor = "CornflowerBlue")]
        public IndicatorDataSeries Rsi { get; set; }

        [Output("Trigger", LineColor = "DarkRed", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries Trigger { get; set; }

        private RelativeStrengthIndex _rsi;
        private MovingAverage _ma;
        private ExponentialMovingAverage _ema;

        protected override void Initialize()
        {
            _ma = Indicators.MovingAverage(Source, EMA_Periods, MaType);
            _rsi = Indicators.RelativeStrengthIndex(_ma.Result, RSI_Periods);
            _ema = Indicators.ExponentialMovingAverage(_rsi.Result, Trigger_Periods);

        }

        public override void Calculate(int index)
        {
            Rsi[index] = _rsi.Result[index];
            Trigger[index] = _ema.Result[index];

            if (_rsi.Result[index] > _ema.Result[index] && a == true && PlaySound == true)
            {

                Notifications.PlaySound(MediaFile);

                a = false;
                b = true;
            }

            if (_ema.Result[index] > _rsi.Result[index] && b == true && PlaySound == true)
            {

                Notifications.PlaySound(MediaFile);
                b = false;
                a = true;
            }
        }
    }
}






