namespace CDR.API.Api.Model
{
    public class DashboardTotalGraphModel
    {
        public List<string> dates { get; set; }
        public List<long> calls { get; set; }
        public List<long> inbound { get; set; }
        public List<long> outbound { get; set; }
        public List<long> missed { get; set; }
        public List<long> abandoned { get; set; }
        public List<long> ext2ext { get; set; }

        public string callspercent
        {
            get
            {
                string _percent = string.Empty;

                try
                {
                    string _dateNow = string.Format("{0:dd.MM.yyyy}", DateTime.Now);
                    string _dateNowMinus1 = string.Format("{0:dd.MM.yyyy}", DateTime.Now.AddDays(-1));

                    int _dateNowIndex = dates.IndexOf(_dateNow);
                    int _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                    {
                        _dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        _dateNowMinus1 = string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

                        _dateNowIndex = dates.IndexOf(_dateNow);
                        _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);
                    }

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                        return _percent;

                    var _nowCalls = calls[_dateNowIndex];
                    var _nowMinus1Calls = calls[_dateNowMinus1Index];

                    if (_nowCalls > _nowMinus1Calls)
                    {
                        if (_nowMinus1Calls == 0)
                        {
                            _percent = "<h6 class=\"success darken-4\">100% <i class=\"la la-arrow-up\"></i></h6>";
                            return _percent;
                        }

                        var _fark = _nowCalls - _nowMinus1Calls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"success darken-4\">" + ((int)_carp > 100 ? 100 : (int)_carp) + "% <i class=\"la la-arrow-up\"></i></h6>";
                    }
                    else if (_nowCalls < _nowMinus1Calls)
                    {

                        var _fark = _nowMinus1Calls - _nowCalls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"danger\"'>" + (int)_carp + "% <i class=\"la la-arrow-down\"></i></h6>";
                    }
                }
                catch (Exception)
                {
                }

                return _percent;
            }
        }
        public string inboundpercent
        {
            get
            {
                string _percent = string.Empty;

                try
                {
                    string _dateNow = string.Format("{0:dd.MM.yyyy}", DateTime.Now);
                    string _dateNowMinus1 = string.Format("{0:dd.MM.yyyy}", DateTime.Now.AddDays(-1));

                    int _dateNowIndex = dates.IndexOf(_dateNow);
                    int _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                    {
                        _dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        _dateNowMinus1 = string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

                        _dateNowIndex = dates.IndexOf(_dateNow);
                        _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);
                    }

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                        return _percent;

                    var _nowCalls = inbound[_dateNowIndex];
                    var _nowMinus1Calls = inbound[_dateNowMinus1Index];

                    if (_nowCalls > _nowMinus1Calls)
                    {
                        if (_nowMinus1Calls == 0)
                        {
                            _percent = "<h6 class=\"success darken-4\">100% <i class=\"la la-arrow-up\"></i></h6>";
                            return _percent;
                        }

                        var _fark = _nowCalls - _nowMinus1Calls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"success darken-4\">" + ((int)_carp > 100 ? 100 : (int)_carp) + "% <i class=\"la la-arrow-up\"></i></h6>";
                    }
                    else if (_nowCalls < _nowMinus1Calls)
                    {

                        var _fark = _nowMinus1Calls - _nowCalls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"danger\"'>" + (int)_carp + "% <i class=\"la la-arrow-down\"></i></h6>";
                    }
                }
                catch (Exception)
                {
                }

                return _percent;
            }
        }
        public string outboundpercent
        {
            get
            {
                string _percent = string.Empty;

                try
                {
                    string _dateNow = string.Format("{0:dd.MM.yyyy}", DateTime.Now);
                    string _dateNowMinus1 = string.Format("{0:dd.MM.yyyy}", DateTime.Now.AddDays(-1));

                    int _dateNowIndex = dates.IndexOf(_dateNow);
                    int _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                    {
                        _dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        _dateNowMinus1 = string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

                        _dateNowIndex = dates.IndexOf(_dateNow);
                        _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);
                    }

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                        return _percent;

                    var _nowCalls = outbound[_dateNowIndex];
                    var _nowMinus1Calls = outbound[_dateNowMinus1Index];

                    if (_nowCalls > _nowMinus1Calls)
                    {
                        if (_nowMinus1Calls == 0)
                        {
                            _percent = "<h6 class=\"success darken-4\">100% <i class=\"la la-arrow-up\"></i></h6>";
                            return _percent;
                        }

                        var _fark = _nowCalls - _nowMinus1Calls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"success darken-4\">" + ((int)_carp > 100 ? 100 : (int)_carp) + "% <i class=\"la la-arrow-up\"></i></h6>";
                    }
                    else if (_nowCalls < _nowMinus1Calls)
                    {

                        var _fark = _nowMinus1Calls - _nowCalls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"danger\"'>" + (int)_carp + "% <i class=\"la la-arrow-down\"></i></h6>";
                    }
                }
                catch (Exception)
                {
                }

                return _percent;
            }
        }
        public string missedpercent
        {
            get
            {
                string _percent = string.Empty;

                try
                {
                    string _dateNow = string.Format("{0:dd.MM.yyyy}", DateTime.Now);
                    string _dateNowMinus1 = string.Format("{0:dd.MM.yyyy}", DateTime.Now.AddDays(-1));

                    int _dateNowIndex = dates.IndexOf(_dateNow);
                    int _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                    {
                        _dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        _dateNowMinus1 = string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

                        _dateNowIndex = dates.IndexOf(_dateNow);
                        _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);
                    }

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                        return _percent;

                    var _nowCalls = missed[_dateNowIndex];
                    var _nowMinus1Calls = missed[_dateNowMinus1Index];

                    if (_nowCalls > _nowMinus1Calls)
                    {
                        if (_nowMinus1Calls == 0)
                        {
                            _percent = "<h6 class=\"success darken-4\">100% <i class=\"la la-arrow-up\"></i></h6>";
                            return _percent;
                        }

                        var _fark = _nowCalls - _nowMinus1Calls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"success darken-4\">" + ((int)_carp > 100 ? 100 : (int)_carp) + "% <i class=\"la la-arrow-up\"></i></h6>";
                    }
                    else if (_nowCalls < _nowMinus1Calls)
                    {

                        var _fark = _nowMinus1Calls - _nowCalls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"danger\"'>" + (int)_carp + "% <i class=\"la la-arrow-down\"></i></h6>";
                    }
                }
                catch (Exception)
                {
                }

                return _percent;
            }
        }
        public string abandonedpercent
        {
            get
            {
                string _percent = string.Empty;

                try
                {
                    string _dateNow = string.Format("{0:dd.MM.yyyy}", DateTime.Now);
                    string _dateNowMinus1 = string.Format("{0:dd.MM.yyyy}", DateTime.Now.AddDays(-1));

                    int _dateNowIndex = dates.IndexOf(_dateNow);
                    int _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                    {
                        _dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        _dateNowMinus1 = string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

                        _dateNowIndex = dates.IndexOf(_dateNow);
                        _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);
                    }

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                        return _percent;

                    var _nowCalls = abandoned[_dateNowIndex];
                    var _nowMinus1Calls = abandoned[_dateNowMinus1Index];

                    if (_nowCalls > _nowMinus1Calls)
                    {
                        if (_nowMinus1Calls == 0)
                        {
                            _percent = "<h6 class=\"success darken-4\">100% <i class=\"la la-arrow-up\"></i></h6>";
                            return _percent;
                        }

                        var _fark = _nowCalls - _nowMinus1Calls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"success darken-4\">" + ((int)_carp > 100 ? 100 : (int)_carp) + "% <i class=\"la la-arrow-up\"></i></h6>";
                    }
                    else if (_nowCalls < _nowMinus1Calls)
                    {

                        var _fark = _nowMinus1Calls - _nowCalls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"danger\"'>" + (int)_carp + "% <i class=\"la la-arrow-down\"></i></h6>";
                    }
                }
                catch (Exception)
                {
                }

                return _percent;
            }
        }
        public string ext2extpercent
        {
            get
            {
                string _percent = string.Empty;

                try
                {
                    string _dateNow = string.Format("{0:dd.MM.yyyy}", DateTime.Now);
                    string _dateNowMinus1 = string.Format("{0:dd.MM.yyyy}", DateTime.Now.AddDays(-1));

                    int _dateNowIndex = dates.IndexOf(_dateNow);
                    int _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                    {
                        _dateNow = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        _dateNowMinus1 = string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(-1));

                        _dateNowIndex = dates.IndexOf(_dateNow);
                        _dateNowMinus1Index = dates.IndexOf(_dateNowMinus1);
                    }

                    if (_dateNowIndex == -1 || _dateNowMinus1Index == -1)
                        return _percent;

                    var _nowCalls = ext2ext[_dateNowIndex];
                    var _nowMinus1Calls = ext2ext[_dateNowMinus1Index];

                    if (_nowCalls > _nowMinus1Calls)
                    {
                        if (_nowMinus1Calls == 0)
                        {
                            _percent = "<h6 class=\"success darken-4\">100% <i class=\"la la-arrow-up\"></i></h6>";
                            return _percent;
                        }

                        var _fark = _nowCalls - _nowMinus1Calls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"success darken-4\">" + ((int)_carp > 100 ? 100 : (int)_carp) + "% <i class=\"la la-arrow-up\"></i></h6>";
                    }
                    else if (_nowCalls < _nowMinus1Calls)
                    {

                        var _fark = _nowMinus1Calls - _nowCalls;
                        double _bol = (double)_fark / (double)_nowMinus1Calls;
                        var _carp = _bol * 100;

                        _percent = "<h6 class=\"danger\"'>" + (int)_carp + "% <i class=\"la la-arrow-down\"></i></h6>";
                    }
                }
                catch (Exception)
                {
                }

                return _percent;
            }
        }
    }
}
