using speedrun;
using TMPro;
using UnityEngine;

public class SL_MatchRow : MonoBehaviour
{
    [SerializeField] private TMP_Text _day              = null;
    [SerializeField] private TMP_Text _date             = null;
    [SerializeField] private TMP_Text _month            = null;
    [SerializeField] private TMP_Text _time             = null;
    [SerializeField] private TMP_Text _game_title       = null;
    [SerializeField] private TMP_Text _vs_runner_name   = null;

    [SerializeField] private UnityEngine.UI.Image _game_image       = null;
    [SerializeField] private UnityEngine.UI.Image _vs_runner_image  = null;

    private SpeedrunningLeagueAPI.MatchModel _model;
    public SpeedrunningLeagueAPI.MatchModel model {
        set {
            _model = value;
            _day.text = _model.start_time.DayOfWeek.ToString();
            _date.text = _model.start_time.Day.ToString();
            _month.text = _model.start_time.ToString("MMMM");

            _time.text = _model.start_time.ToString("h:mm tt");
            _game_title.text = _model.game_name;
            string vs_name = PlayerPrefs.GetString("userid").Equals(_model.home_srid) ? _model.away : _model.home;
            _vs_runner_name.text = vs_name;

            SpeedrunAPI.instance.fetch_game_image(_model.game_srid, speedrun.GameImageTypes.thumb, (Sprite s) => {
                _game_image.sprite = s;
            });

            TwitchAPI.TwitchAPI.instance.fetch_runner_image(vs_name, (Sprite s) => {
                _vs_runner_image.sprite = s;
            });
        }
    }


}
