using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimKhoa
{
    struct S_PhuToiThieu
    {
        public List<string> trai;

        public List<string> phai;
    }

    class C_ThuatToan
    {
        /// <summary>
        /// Tìm bao đóng
        /// </summary>
        /// <param name="baoDong">Bao đóng cần tìm</param>
        /// <param name="trai">tập phục thuộc hàm bên trái</param>
        /// <param name="phai">tập phục thuộc hàm bên phải</param>
        /// <param name="n">số phục thuộc hàm</param>
        /// <returns>Bao đóng</returns>
        public string TimBaoDong(string baoDong, List<string> trai, List<string> phai)
        {
            int doDaiBaoDong = baoDong.Length-1;

            while (doDaiBaoDong != baoDong.Length)
            {
                doDaiBaoDong = baoDong.Length;

                for (int i = 0; i < trai.Count; i++)
                {
                    if (SoSanhChuoi(trai[i], baoDong))
                    {
                        for (int j = 0; j < phai[i].Length;j++ )
                            if (!SoSanhChuoi(phai[i][j].ToString(), baoDong))
                                baoDong += phai[i][j].ToString();
                    }
                }
    
            }

            return baoDong;
        }

        /// <summary>
        /// So sánh chuỗi A có nằm trong chuỗi B không
        /// </summary>
        /// <param name="con">A</param>
        /// <param name="cha">B</param>
        /// <returns>true nếu nằm trong, ngược lại trả về false</returns>
        private bool SoSanhChuoi(string con, string cha)
        {
            int ChuoiCon=0;

            if (cha.Length < con.Length)
                return false;

            for (int i = 0; i < con.Length; i++)
                for (int j = 0; j < cha.Length; j++)
                {
                    if (con[i] == cha[j])
                    {
                        ChuoiCon++;
                        break;
                    }
                }

                    if (ChuoiCon == con.Length)
                        return true;

            return false;
        }

        /// <summary>
        /// tìm tất cả các khóa của lược đồ
        /// </summary>
        /// <param name="tapThuocTinh">tập thuộc tính của lược đồ</param>
        /// <param name="trai">danh sách phụ thuộc hàm bên trái</param>
        /// <param name="phai">danh sách phụ thuộc hàm bên phải</param>
        /// <returns>trả về danh sách các khóa</returns>
        public List<string> TimKhoa(string tapThuocTinh, List<string> trai, List<string> phai)
        {
            List<string> listKhoa = new List<string>();

            string L="";
            string R="";
            string TN="";
            string TG="";

            //lấy tập L (chỉ xuất hiện vế trái, ko xh vế phải)
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                for (int t = 0; t < trai.Count; t++)
                    if (SoSanhChuoi(tapThuocTinh[i].ToString(), trai[t]) && !SoSanhChuoi(tapThuocTinh[i].ToString(), phai[t]))
                    {
                        L+=tapThuocTinh[i].ToString();
                        break;
                    }
            }
            //lấy tập R (chỉ xuất hiện vế phải, ko xh vế trái)
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                for (int t = 0; t < trai.Count; t++)
                    if (SoSanhChuoi(tapThuocTinh[i].ToString(), phai[t]) && !SoSanhChuoi(tapThuocTinh[i].ToString(), trai[t]))
                    {
                        R+=tapThuocTinh[i].ToString();
                        break;
                    }
            }

            /*lấy TN thuộc tính chỉ xuất hiện ở vế trái, không xuất hiện ở vế phải và
             * các thuộc tính không xuất hiện ở cả vế trái và vế phải của F*/
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                if (!SoSanhChuoi(tapThuocTinh[i].ToString(), R))
                {
                    TN += tapThuocTinh[i].ToString();
                }
            }
            //lấy TG (giao giữa 2 tập L và R)
            for (int i = 0; i < L.Length; i++)
            {
                if (SoSanhChuoi(L[i].ToString(), R))
                {
                    TG += L[i].ToString();
                }
            }

            //nếu tập TG rỗng thì khóa chính là TN
            if (TG == "")
            {
                listKhoa.Add(TN);
                return listKhoa;
            }
            else
            {
                List<string> TapConTG = new List<string>();
                //sinh tập con của TG
                TapConTG = TimTapCon(TG);

                List<string> SieuKhoa = new List<string>();

                //kiểm tra từng tập con của TG hợp với TN có là siêu khóa không
                for (int n = 0; n < TapConTG.Count; n++)
                {
                    //lấy giao tập nguồn(TN) và từng con của TG 
                    string temp = TN + TapConTG[n];
                    //nếu giao tập nguồn(TN) và từng con của TG tất cả lấy bao đóng mà bằng tập thuộc tính thì là siêu khóa
                    if (SoSanhChuoi(tapThuocTinh, TimBaoDong(temp, trai, phai)))
                    {
                        SieuKhoa.Add(temp);
                    }
                }

                //tìm siêu khóa tối thiểu
                for (int i = 0; i < SieuKhoa.Count; i++)
                {
                    for (int j = i + 1; j < SieuKhoa.Count; j++)
                    {
                        if (SoSanhChuoi(SieuKhoa[i], SieuKhoa[j]))
                        {
                            SieuKhoa.Remove(SieuKhoa[j]);
                            j--;
                        }
                    }
                }

                listKhoa = SieuKhoa;
            }

            return listKhoa;
        }

        /// <summary>
        /// tìm con con bằng phương pháp sinh
        /// </summary>
        /// <param name="str">chuỗi cần sinh tập con</param>
        /// <returns>trả về danh sách tập con</returns>
        List<string> TimTapCon(string str)
        {

            List<string> TapCon = new List<string>();

            int[] a = new int[str.Length];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 0;
            }

            int t = str.Length - 1;

            TapCon.Add("");
            while (t >= 0)
            {

                t = str.Length - 1;
                while (t >= 0 && a[t] == 1)
                    t--;

                if (t >= 0)
                {
                    a[t] = 1;
                    for (int i = t + 1; i < str.Length; i++)
                        a[i] = 0;

                    string temp = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (a[i] == 1)
                        {
                            temp += str[i];
                        }
                    }

                    TapCon.Add(temp);
                }
            }

            return TapCon;
        }

        public S_PhuToiThieu TimPhuToiThieu(List<string> trai, List<string> phai)
        {
            S_PhuToiThieu ptt = new S_PhuToiThieu();

            int n = phai.Count;
            //tách phụ thuộc hàm vế phải có hơn 1 thuộc tính
            for (int i = 0; i < n; i++)
            {
                if (phai[i].Length > 1)
                {
                    string tempPhai = phai[i];
                    string temTrai = trai[i];

                    trai.Remove(trai[i]);
                    phai.Remove(phai[i]);

                    for (int j = 0; j < tempPhai.Length; j++)
                    {
                        trai.Add(temTrai);
                        phai.Add(tempPhai[j].ToString());
                    }

                    i--;
                }
            }

            //loại bỏ thuộc tính dư thừa bên vế trái có hơn 1 thuộc tính
            for (int i = 0; i < trai.Count; i++)
            {
                if (trai[i].Length > 1)
                {

                    for (int j = 0; j < trai[i].Length; j++)
                    {
                        if (trai[i].Length > 1)
                        {
                            string temp = trai[i];
                            temp = CatKiTu(temp, j);

                            if (SoSanhChuoi(phai[i], TimBaoDong(temp, trai, phai)))
                            {
                                trai[i] = temp;
                                j--;
                            }

                        }
                    }
                }
            }

            //loại bỏ thuộc tính dư thừa
            List<string> TempTrai = new List<string>();
            List<string> TempPhai = new List<string>();

            for (int i = 0; i < trai.Count; i++)
            {
                TempTrai.Add(trai[i]);
                TempPhai.Add(phai[i]);
            }

            for (int i = 0; i < trai.Count; i++)
            {

                TempTrai.RemoveAt(i);
                TempPhai.RemoveAt(i);

                if (SoSanhChuoi(phai[i], TimBaoDong(trai[i], TempTrai, TempPhai)))
                {
                    trai.Clear();
                    phai.Clear();

                    for (int t = 0; t < TempTrai.Count; t++)
                    {
                        trai.Add(TempTrai[t]);
                        phai.Add(TempPhai[t]);
                    }

                    i--;
                }
                else
                {
                    TempTrai.Clear();
                    TempPhai.Clear();

                    for (int t = 0; t < trai.Count; t++)
                    {
                        TempTrai.Add(trai[t]);
                        TempPhai.Add(phai[t]);
                    }
                }
            }

            ptt.trai = trai;
            ptt.phai = phai;

            return ptt;
        }

        string CatKiTu(string str, int vitri)
        {
            string ok="";
            for (int i = 0; i < str.Length; i++)
            {
                if(vitri != i)
                    ok += str[i].ToString();
            }

            return ok;
        }
    }
}
