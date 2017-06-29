using System.Windows.Forms;
using WindowsInput;
using System.Collections.Generic;
using System.Text;
using WindowsInput.Native;
using System.Linq;

namespace BoGoViet.TiengViet
{
    struct MyKey
    {
        public Keys key;
        public bool isUpper;
        public MyKey(Keys k, bool i)
        {
            key = k;
            isUpper = i;
        }
    };

    class TiengViet
    {
        private InputSimulator sim;

        private List<Keys> phuAmDau = new List<Keys>();
        private List<Keys> nguyenAmGiua = new List<Keys>();
        private List<MyKey> phuAmCuoi = new List<MyKey>();

        // neu viet tieng anh
        private List<Keys> nguyenAmGiuaThuHai = new List<Keys>();
        private Keys dauCuoiCau = Keys.None;
        private Keys dauPhuAm = Keys.None;
        private Keys doubleCuoiCau = Keys.None;
        private bool vekepPress = false;
        private bool PrintWW = false;
        private StringBuilder nguyenAmGiuaBienDoi = new StringBuilder();
        private StringBuilder nguyenAmGiuaBienDoiCoDau = new StringBuilder();
        private bool layNguyenAmGiua = true;
        private int rememberDauCuoiCau = -1;
        private bool pressLeftShift = false;
        private bool pressRightShift = false;
        private bool[] upperNguyenAm;
        private bool isCapsLockOn = false;

        private bool isCtrlAltWinPress = false;
        private bool isDoubleD = false;
        private bool isPhuAmDUpper = false;

        private string kieuGo; 
        private TiengvietUtil tvu = new TiengvietUtil();

        public TiengViet()
        {
            sim = new InputSimulator();
            upperNguyenAm = new bool[] { false, false, false };
        }

        public void SetKieuGo(string _kieugo)
        {
            this.kieuGo = _kieugo;
            tvu.SetKieuGo(_kieugo);
        }

        public void OnKeyDown(ref object sender, ref KeyEventArgs e)
        {
            
            isCapsLockOn = sim.InputDeviceState.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
            
            if (e.KeyCode == Keys.LShiftKey)
            {
                pressLeftShift = true;
            }
            else if (e.KeyCode == Keys.RShiftKey)
            {
                pressRightShift = true;
            }
            else if (e.KeyCode == Keys.LControlKey
              || e.KeyCode == Keys.RControlKey
              || e.KeyCode == Keys.LWin
              || e.KeyCode == Keys.RWin
              || e.KeyCode == Keys.RMenu
              || e.KeyCode == Keys.LMenu)
            {
                isCtrlAltWinPress = true;
            }
            if (isCtrlAltWinPress)
            {
                Reset();
                return;
            }
            bool toShiftPress = isShiftPress();
            // kiem tra gi
            if (phuAmDau.Count == 1 && phuAmDau.ToArray()[0] == Keys.G &&
                nguyenAmGiua.Count == 1 && nguyenAmGiua.ToArray()[0] == Keys.I
                && tvu.CheckDau(e.KeyCode, isShiftPress()) == -1)
            {
                // PrintWW = false;
                phuAmDau.Add(Keys.I);
                nguyenAmGiua.Clear();
                nguyenAmGiuaThuHai.Clear();
                nguyenAmGiuaBienDoi = new StringBuilder();
                nguyenAmGiuaBienDoiCoDau = new StringBuilder();
                upperNguyenAm[0] = false;
                upperNguyenAm[1] = false;
                upperNguyenAm[2] = false;
            }

            if (tvu.CheckKeyEndWord(e.KeyCode))
            {
                Reset();
                return;
            }
            else if (nguyenAmGiuaThuHai.Count > 0)
            {
                return;
            }
            else if (!IsAZKey(e.KeyCode))
            {
                return;
            }
            // kiem tra la QU
            else if (phuAmDau.Count == 1 && e.KeyCode == Keys.U && phuAmDau.ToArray()[0] == Keys.Q)
            {
                phuAmDau.Add(e.KeyCode);
                layNguyenAmGiua = false;
            }
            else
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.O || e.KeyCode == Keys.U || 
                e.KeyCode == Keys.E || e.KeyCode == Keys.I || e.KeyCode == Keys.Y)
            {
                layNguyenAmGiua = true;
                if (tvu.IsTelexDouble(e.KeyCode)) { 
                    doubleCuoiCau = Keys.None;
                    Keys[] nguyenAmGiuaKeys = nguyenAmGiua.ToArray();
                    bool checkCanDoubleKey = false;
                    for (int i = 0; i < nguyenAmGiua.Count; i++)
                    {
                        if (nguyenAmGiuaKeys[i] == e.KeyCode)
                        {
                            checkCanDoubleKey = true;
                            break;
                        }
                    }
                    if (checkCanDoubleKey)
                    {
                        doubleCuoiCau = e.KeyCode;
                    }
                }
                if (phuAmCuoi.Count == 0 && doubleCuoiCau == Keys.None)
                {
                    nguyenAmGiua.Add(e.KeyCode);

                    // luu nguyen am chu hoa vo mang
                    if (isUpperCase() && nguyenAmGiua.Count < 4)
                    {
                        upperNguyenAm[nguyenAmGiua.Count - 1] = true;
                    }
                }
                else if (doubleCuoiCau == Keys.None)
                {
                    nguyenAmGiuaThuHai.Add(e.KeyCode);
                }
            }
            // VNI 
            else if (tvu.IsVNIDouble(e.KeyCode))
            {
                doubleCuoiCau = Keys.None;
                Keys[] nguyenAmGiuaKeys = nguyenAmGiua.ToArray();
                bool checkCanDoubleKey = false;
                for (int i = 0; i < nguyenAmGiua.Count; i++)
                {
                    if (nguyenAmGiuaKeys[i] == Keys.A || nguyenAmGiuaKeys[i] == Keys.E || nguyenAmGiuaKeys[i] == Keys.O)
                    {
                        checkCanDoubleKey = true;
                        break;
                    }
                }
                if (checkCanDoubleKey)
                {
                    doubleCuoiCau = e.KeyCode;
                }
            }
            else if (phuAmDau.Count == 1 && tvu.IsDGach(e.KeyCode))
            {
                dauPhuAm = e.KeyCode;
                layNguyenAmGiua = true;
            }
            else if (nguyenAmGiua.Count == 0)
            {
                /*
                if (e.KeyCode == Keys.W) {
                    nguyenAmGiua.Add(Keys.U);

                    // luu nguyen am chu hoa vo mang
                    if (isUpperCase() && nguyenAmGiua.Count < 3)
                    {
                        upperNguyenAm[nguyenAmGiua.Count - 1] = true;
                    }
                    PrintWW = true;
                } else
                {
                    phuAmDau.Add(e.KeyCode);
                }
                */
                if (!IsControlKeyCode(e.KeyCode))
                {
                    if (e.KeyCode == Keys.D && isUpperCase())
                    {
                        isPhuAmDUpper = true;
                    }
                    phuAmDau.Add(e.KeyCode);
                }
                layNguyenAmGiua = false;
            }
            /*
            else if (nguyenAmGiua.Count == 1 && 
                nguyenAmGiuaBienDoi.ToString() == "ư")
            {
                e.Handled = true;
                nguyenAmGiuaBienDoi = new StringBuilder("w");
                sim.Keyboard.KeyPress(VirtualKeyCode.BACK);
                SendText("w", 0);
                phuAmDau.Add(Keys.W);
                return;
            }
            */
            else if (tvu.CheckDau(e.KeyCode, isShiftPress()) != -1)
            {
                dauCuoiCau = e.KeyCode;
                layNguyenAmGiua = false;
            }
            else if (tvu.IsTelexDauMoc(e.KeyCode) || 
                tvu.IsVNIDauMocA(e.KeyCode) || tvu.IsVNIDauMocO(e.KeyCode))
            {
                layNguyenAmGiua = false;
                vekepPress = true;
            }
            else
            {
                layNguyenAmGiua = false;
                if (!IsControlKeyCode(e.KeyCode))
                {
                    phuAmCuoi.Add(new MyKey(e.KeyCode, isUpperCase()));
                }
            }
            
            // prevent send key
            if (doubleCuoiCau != Keys.None || dauCuoiCau != Keys.None || vekepPress == true)
            {
                layNguyenAmGiua = false;
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
            // all thing to write
            Run(ref e);
        }

        private void Run(ref KeyEventArgs e)
        {
            int numberKeysPhuAmCuoiCau = phuAmCuoi.Count;
            //  Fix for thuơ, huơ
            if (tvu.CheckDau(e.KeyCode, isShiftPress()) == -1 && !tvu.IsDGach(e.KeyCode) &&
                !tvu.IsDauMoc(e.KeyCode) && (
                nguyenAmGiuaBienDoi.ToString() == "uơ" || nguyenAmGiuaBienDoi.ToString() == "ưo" ||
                nguyenAmGiuaBienDoi.ToString() == "oa" || nguyenAmGiuaBienDoi.ToString() == "uy"))
            {
                for (int i = 0; i < nguyenAmGiua.Count - 3; i++)
                {
                    sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                }
                if (nguyenAmGiuaBienDoi.ToString() == "uơ" || nguyenAmGiuaBienDoi.ToString() == "ưo")
                {
                    nguyenAmGiuaBienDoi = new StringBuilder("ươ");
                }
                string sendtext = ThayDoiNguyenAmGiua();
                SendText(sendtext);
                
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.O || e.KeyCode == Keys.U
                        || e.KeyCode == Keys.E || e.KeyCode == Keys.I || e.KeyCode == Keys.Y)
                {
                    nguyenAmGiuaBienDoi = nguyenAmGiuaBienDoi.Append(e.KeyCode.ToString().ToLower()[0]);
                }
                for (int i = 0; i < nguyenAmGiua.Count - 3; i++)
                {
                    sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                }
                return;
            }
            
            bool printCurrentPressKey = false;
            if (doubleCuoiCau == Keys.None && dauCuoiCau == Keys.None && vekepPress == false
                && PrintWW == false && dauPhuAm == Keys.None)
            {

                if (nguyenAmGiuaBienDoi.Length > 0)
                {
                    if (e.KeyCode == Keys.A || e.KeyCode == Keys.O || e.KeyCode == Keys.U
                        || e.KeyCode == Keys.E || e.KeyCode == Keys.I || e.KeyCode == Keys.Y)
                    {
                        nguyenAmGiuaBienDoi = nguyenAmGiuaBienDoi.Append(e.KeyCode.ToString().ToLower()[0]);
                    }
                }
                else if (nguyenAmGiuaBienDoi.Length == 0 && layNguyenAmGiua)
                {
                    if (e.KeyCode == Keys.A || e.KeyCode == Keys.O || e.KeyCode == Keys.U
                        || e.KeyCode == Keys.E || e.KeyCode == Keys.I || e.KeyCode == Keys.Y)
                    {
                        nguyenAmGiuaBienDoi = ConvertKeysToString(nguyenAmGiua);
                    }
                }
                if (IsAZKey(e.KeyCode))
                {

                }
                return;
            }

            if (tvu.CheckKeyEndWord(e.KeyCode))
            {
                e.Handled = false;
                Reset();
                return;
            }
            else
            {
                e.Handled = true;
            }

            //m_Events.KeyPress -= HookManager_KeyPress;

            
            
            if (doubleCuoiCau != Keys.None)
            {
                // TODO thay doi truong hop co dau
                string textSend = "";

                if (tvu.IsTelexDouble(e.KeyCode) || tvu.IsVNIDouble(e.KeyCode))
                {

                    int checkDau = KiemTraDau(ref textSend, e.KeyCode);
                    if (textSend != "")
                    {
                        if (rememberDauCuoiCau != -1)
                        {
                            textSend = ThayDoiDoubleCoDau(textSend);
                        }
                        SendText(textSend);
                    }
                    if (checkDau == 1)
                    {
                        printCurrentPressKey = true;
                        nguyenAmGiuaBienDoi = new StringBuilder();
                    }
                    if (checkDau == 2)
                    {
                        e.Handled = false;
                    }
                }
                doubleCuoiCau = Keys.None;
            }
            /*
            else if (PrintWW == true)
            {
                e.Handled = true;
                nguyenAmGiuaBienDoi = new StringBuilder("ư");
                SendText("ư", 0);
                PrintWW = false;
            }
            */
            else if (vekepPress == true)
            {
                string textSend = "";
                bool check = KiemTraVekep(ref textSend, e.KeyCode);

                if (textSend != "")
                {
                    if (rememberDauCuoiCau != -1)
                    {
                        textSend = ThayDoiDoubleCoDau(textSend);
                    }
                    SendText(textSend);
                    vekepPress = false;
                }
                if (textSend != "" && check == false)
                {
                    printCurrentPressKey = true;
                }
                if (textSend == "")
                {
                    e.Handled = false;
                }
                vekepPress = false;
            }
            else if (tvu.IsDGach(e.KeyCode))
            {
                if (phuAmDau.Count == 1 && phuAmDau.ToArray()[0] == Keys.D && isDoubleD == false)
                {
                    for (int i = 0; i < nguyenAmGiuaBienDoi.Length; i++)
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                    }
                    SendText("đ", 0);
                    for (int i = 0; i < nguyenAmGiuaBienDoi.Length; i++)
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                    }
                    isDoubleD = true;
                }
                else if (phuAmDau.Count == 1 && phuAmDau.ToArray()[0] == Keys.D && isDoubleD == true)
                {
                    for (int i = 0; i < nguyenAmGiuaBienDoi.Length; i++)
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                    }
                    SendText("d", 0);
                    for (int i = 0; i < nguyenAmGiuaBienDoi.Length; i++)
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                    }
                    e.Handled = false;
                }
                else
                {
                    e.Handled = false;
                }
                dauPhuAm = Keys.None;
            }
            else if (dauCuoiCau != Keys.None)
            {

                rememberDauCuoiCau = tvu.CheckDau(dauCuoiCau, isShiftPress());
                string textSend = "";
                bool check = KiemTraDauCuoiCau(ref textSend, dauCuoiCau);
                if (textSend != "")
                {
                    SendText(textSend);
                    vekepPress = false;
                    dauCuoiCau = Keys.None;
                }
                if (!check)
                {
                    e.Handled = false;
                }
                dauCuoiCau = Keys.None;
            }

            if (e.Handled == false)
            {
                phuAmCuoi.Add(new MyKey(e.KeyCode, isUpperCase()));
                nguyenAmGiuaThuHai.Add(Keys.NoName);
            }
            
            if (printCurrentPressKey)
            {
                e.Handled = false;
                Reset();
            }
        }

        public void OnKeyUp(ref object sender, ref KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey)
            {
                pressLeftShift = false;
            }
            else if (e.KeyCode == Keys.RShiftKey)
            {
                pressRightShift = false;
            }
            else if (e.KeyCode == Keys.LControlKey
             || e.KeyCode == Keys.RControlKey
             || e.KeyCode == Keys.LWin
             || e.KeyCode == Keys.RWin
             || e.KeyCode == Keys.LMenu
             || e.KeyCode == Keys.RMenu)
            {
                isCtrlAltWinPress = false;
            }
        }

        public void OnMouseClick(ref object sender, ref MouseEventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            phuAmDau.Clear();
            nguyenAmGiua.Clear();
            nguyenAmGiuaThuHai.Clear();
            phuAmCuoi.Clear();
            dauCuoiCau = Keys.None;
            doubleCuoiCau = Keys.None;
            nguyenAmGiuaBienDoi = new StringBuilder();
            nguyenAmGiuaBienDoiCoDau = new StringBuilder();
            rememberDauCuoiCau = -1;
            // PrintWW = false;
            upperNguyenAm[0] = false;
            upperNguyenAm[1] = false;
            upperNguyenAm[2] = false;
            dauPhuAm = Keys.None;
            isDoubleD = false;
            isPhuAmDUpper = false;
        }

        private StringBuilder ConvertKeysToString(List<Keys> s)
        {
            Keys[] keys = s.ToArray();
            string result = "";
            for (int i = 0; i < s.Count; i++)
            {
                result += keys[i].ToString().ToLower();
            }
            return new StringBuilder(result);
        }

        private int KiemTraDau(ref string textSend, Keys key)
        {
            int result = 2;
            Dictionary<string, string> tDouble = tvu.cDoubleA;
            Dictionary<string, string> tDoubleInvert = tvu.cDoubleAInvert;

            if (key == Keys.O)
            {
                tDouble = tvu.cDoubleO;
                tDoubleInvert = tvu.cDoubleOInvert;
            }
            else if (key == Keys.E)
            {
                tDouble = tvu.cDoubleE;
                tDoubleInvert = tvu.cDoubleEInvert;
            } else if (tvu.IsVNIDouble(key))
            {
                tDouble = tvu.cDouble;
                tDoubleInvert = tvu.cDoubleInvert;
            }
            string nguyenAmGiuaBienDoiString = nguyenAmGiuaBienDoi.ToString().ToLower();
            if (tDouble.ContainsKey(nguyenAmGiuaBienDoiString))
            {
                textSend = tDouble[nguyenAmGiuaBienDoiString];
                nguyenAmGiuaBienDoi = new StringBuilder(textSend);
                result = 0;
            }
            else if (tDoubleInvert.ContainsKey(nguyenAmGiuaBienDoiString))
            {
                textSend = tDoubleInvert[nguyenAmGiuaBienDoiString];
                nguyenAmGiuaBienDoi = new StringBuilder(textSend);
                result = 1;
            }
            return result;
        }

        private bool KiemTraVekep(ref string textSend, Keys key)
        {
            bool result = false;
            Dictionary<string,string> cVeKep = tvu.cVekep;
            Dictionary<string, string> cVekepInvert = tvu.cVekepInvert;
            if (tvu.IsVNIDauMocO(key))
            {
                cVeKep = tvu.cVekepOU;
                cVekepInvert = tvu.cVekepInvertOU;
            }
            else if (tvu.IsVNIDauMocA(key))
            {
                cVeKep = tvu.cVekepA;
                cVekepInvert = tvu.cVekepInvertA;
            }
            string nguyenAmGiuaBienDoiString = nguyenAmGiuaBienDoi.ToString().ToLower();
            if (cVeKep.ContainsKey(nguyenAmGiuaBienDoiString))
            {
                textSend = cVeKep[nguyenAmGiuaBienDoiString];
                // Fix for huơ, thuở
                Keys phuamdaucuoi = Keys.None;
                if (phuAmDau.Count > 0) { 
                    phuamdaucuoi = phuAmDau.Last();
                }
                if (phuAmCuoi.Count == 0 && nguyenAmGiuaBienDoiString == "uo" && phuamdaucuoi == Keys.H)
                {
                    textSend = "uơ";
                }
                nguyenAmGiuaBienDoi = new StringBuilder(textSend);
                result = true;
            }
            else if (cVekepInvert.ContainsKey(nguyenAmGiuaBienDoiString))
            {
                textSend = cVekepInvert[nguyenAmGiuaBienDoiString];
                nguyenAmGiuaBienDoi = new StringBuilder(textSend);
                result = false;
            }
            return result;
        }

        private bool KiemTraDauCuoiCau(ref string textSend, Keys key_press)
        {
            string nguyenAmGiuaBienDoiString = nguyenAmGiuaBienDoi.ToString().ToLower();
            bool check = false;

            int position = 0;
            if (tvu.cDauNguyenAm.ContainsKey(nguyenAmGiuaBienDoiString))
            {
                position = tvu.cDauNguyenAm[nguyenAmGiuaBienDoiString];

                // fix for oa
                if ((nguyenAmGiuaBienDoiString == "oa" || nguyenAmGiuaBienDoiString == "uy")
                    && phuAmCuoi.Count > 0)
                {
                    position = 1;
                }

                string position_char = nguyenAmGiuaBienDoiString[position].ToString();
                int postision_dau = tvu.CheckDau(key_press, isShiftPress());
                if (tvu.cChar.ContainsKey(position_char))
                {
                    if (postision_dau != -1)
                    {

                        if (nguyenAmGiuaBienDoiCoDau.Length != nguyenAmGiuaBienDoi.Length)
                        {
                            nguyenAmGiuaBienDoiCoDau = new StringBuilder(nguyenAmGiuaBienDoi.ToString());
                        }
                        if (nguyenAmGiuaBienDoiCoDau.Length > position &&
                            nguyenAmGiuaBienDoiCoDau[position] == tvu.cChar[position_char][postision_dau][0])
                        {
                            nguyenAmGiuaBienDoiCoDau[position] = position_char[0];
                            textSend = nguyenAmGiuaBienDoiCoDau.ToString();
                            check = false;
                        }
                        else
                        {
                            nguyenAmGiuaBienDoiCoDau[position] = tvu.cChar[position_char][postision_dau][0];
                            textSend = nguyenAmGiuaBienDoiCoDau.ToString();
                            check = true;
                        }
                    }
                    else
                    {
                        // some error
                        check = false;
                    }
                }
            }
            else
            {
                check = false;
            }
            return check;
        }

        private string ThayDoiNguyenAmGiua()
        {
            string char1 = nguyenAmGiuaBienDoi[0].ToString();
            string char2 = nguyenAmGiuaBienDoi[1].ToString();

            if (rememberDauCuoiCau == -1)
            {
                return nguyenAmGiuaBienDoi.ToString();
            }
            int position = rememberDauCuoiCau;
            if (tvu.cCharInvert.ContainsKey(char1)) { 
                char1 = tvu.cCharInvert[char1];
            }
            if (tvu.cCharInvert.ContainsKey(char2))
            {
                char2 = tvu.cCharInvert[char2];
            }
            
            char2 = tvu.cChar[char2][position];
            return char1 + char2;
        }

        // text: ôi
        // dau: Key.J
        // return ội
        private string ThayDoiDoubleCoDau(string text)
        {
            int postision_dau = rememberDauCuoiCau;
            StringBuilder result = new StringBuilder(text);

            if (tvu.cDauNguyenAm.ContainsKey(text))
            {
                int pos = tvu.cDauNguyenAm[text];
                string c = text[pos].ToString();
                if (tvu.cChar.ContainsKey(c.ToString()))
                {
                    result[pos] = tvu.cChar[c][postision_dau][0];
                }
            }
            return result.ToString();
        }

        // type = 0 normal
        // type = 1 text la nguyen am giua
        public string SendText(string text, int type = 1)
        {
            int numberKeysPhuAmCuoiCau = phuAmCuoi.Count;
            if (pressLeftShift)
            {
                sim.Keyboard.KeyUp(VirtualKeyCode.LSHIFT);
            }
            if (pressRightShift)
            {
                sim.Keyboard.KeyUp(VirtualKeyCode.RSHIFT);
            }
            for (int i = 0; i < numberKeysPhuAmCuoiCau; i++)
            {
                sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
            }
            
            // ^ is xor logic
            bool toUpper = isUpperCase();
            if (text == "đ")
            {
                toUpper = isPhuAmDUpper;
            }
            string textsend = "";
            if (type == 1)
            {
                StringBuilder textBuilder = new StringBuilder(text);
                for (int i = 0; i < textBuilder.Length && i < 3; i++)
                {
                    if (upperNguyenAm[i])
                    {
                        textBuilder[i] = textBuilder[i].ToString().ToUpper()[0];
                    }
                }
                textsend = textBuilder.ToString();
            }
            else if (toUpper)
            {
                text = text.ToUpper();
                textsend = text;
            }
            else
            {
                textsend = text;
            }
            
            for (int i = 0; i < text.Length; i++)
            {
                sim.Keyboard.KeyPress(VirtualKeyCode.BACK);
            }
            sim.Keyboard.TextEntry(textsend);
            
            oldsendtext = textsend;
            for (int i = 0; i < numberKeysPhuAmCuoiCau; i++)
            {
                sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
            }
            if (pressLeftShift)
            {
                sim.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
            }
            if (pressRightShift)
            {
                sim.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
            }
            return text;
        }

        private bool isUpperCase()
        {
            return isCapsLockOn ^ (pressLeftShift || pressRightShift);
        }

        private bool isShiftPress()
        {
            return pressLeftShift || pressRightShift;
        }

        private bool kiegoKhacTelex()
        {
            return kieuGo == "VNI" || kieuGo == "VIQR";
        }

        private bool IsAZKey(Keys key)
        {
            return ! tvu.CheckKeyEndWord(key);
        }

        private bool IsControlKeyCode(Keys key)
        {
            Keys[] listControllKey = new Keys[] {
                Keys.Enter, Keys.LControlKey
                , Keys.RControlKey, Keys.LWin, Keys.RWin, Keys.RMenu, Keys.LMenu
                , Keys.LShiftKey, Keys.RShiftKey, Keys.Back, Keys.Tab


                , Keys.Left , Keys.Right
                , Keys.Down , Keys.Up

                , Keys.F10 , Keys.F1
                , Keys.F2 , Keys.F3
                , Keys.F4 , Keys.F5
                , Keys.F6 , Keys.F7
                , Keys.F8 , Keys.F9
                , Keys.F11 , Keys.F12

                , Keys.Escape , Keys.Delete

                , Keys.PageDown , Keys.Home
                , Keys.PageUp , Keys.End
                , Keys.Scroll , Keys.Pause
                , Keys.Insert , Keys.Next

                , Keys.CapsLock , Keys.Capital
                , Keys.Apps
            };
            if (listControllKey.Contains(key))
            {
                return true;
            }
            return false;
        }
    }
}
