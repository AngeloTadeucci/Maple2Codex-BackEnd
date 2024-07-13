
namespace Maple2Storage.Types;

public class CustomStat {
    public long Str { get; set; }
    public long Dex { get; set; }
    public long Int { get; set; }
    public long Luk { get; set; }
    public long Hp { get; set; }
    public long HpRgp { get; set; }
    public long HpInv { get; set; }
    public long Sp { get; set; }
    public long SpRgp { get; set; }
    public long SpInv { get; set; }
    public long Ep { get; set; }
    public long EpRgp { get; set; }
    public long EpInv { get; set; }
    public long Asp { get; set; }
    public long Msp { get; set; }
    public long Atp { get; set; }
    public long Evp { get; set; }
    public long Cap { get; set; }
    public long Cad { get; set; }
    public long Car { get; set; }
    public long Ndd { get; set; }
    public long Abp { get; set; }
    public long Jmp { get; set; }
    public long Pap { get; set; }
    public long Map { get; set; }
    public long Par { get; set; }
    public long Mar { get; set; }
    public long Wapmin { get; set; }
    public long Wapmax { get; set; }
    public long Dmg { get; set; }
    public long Pen { get; set; }
    public long Rmsp { get; set; }
    public long Bap { get; set; }
    public long BapPet { get; set; }
    public long Hiddenhpadd { get; set; }
    public long Hiddennddadd { get; set; }
    public long Hiddenwapadd { get; set; }
    public long Hiddenhpadd03 { get; set; }
    public long Hiddennddadd03 { get; set; }
    public long Hiddenwapadd03 { get; set; }
    public long Hiddenhpadd04 { get; set; }
    public long Hiddennddadd04 { get; set; }
    public long Hiddenwapadd04 { get; set; }
    public float ScaleStatRate1 { get; set; }
    public float ScaleStatRate2 { get; set; }
    public float ScaleStatRate3 { get; set; }
    public float ScaleStatRate4 { get; set; }
    public long ScaleBaseTap1 { get; set; }
    public long ScaleBaseTap2 { get; set; }
    public long ScaleBaseTap3 { get; set; }
    public long ScaleBaseTap4 { get; set; }
    public long ScaleBaseDef1 { get; set; }
    public long ScaleBaseDef2 { get; set; }
    public long ScaleBaseDef3 { get; set; }
    public long ScaleBaseDef4 { get; set; }
    public float ScaleBaseSpaRate1 { get; set; }
    public float ScaleBaseSpaRate2 { get; set; }
    public float ScaleBaseSpaRate3 { get; set; }
    public float ScaleBaseSpaRate4 { get; set; }

    public CustomStat(Maple2.File.Parser.Xml.Npc.Stat stat) {
        Str = stat.str;
        Dex = stat.dex;
        Int = stat.@int;
        Luk = stat.luk;
        Hp = stat.hp;
        HpRgp = stat.hp_rgp;
        HpInv = stat.hp_inv;
        Sp = stat.sp;
        SpRgp = stat.sp_rgp;
        SpInv = stat.sp_inv;
        Ep = stat.ep;
        EpRgp = stat.ep_rgp;
        EpInv = stat.ep_inv;
        Asp = stat.asp;
        Msp = stat.msp;
        Atp = stat.atp;
        Evp = stat.evp;
        Cap = stat.cap;
        Cad = stat.cad;
        Car = stat.car;
        Ndd = stat.ndd;
        Abp = stat.abp;
        Jmp = stat.jmp;
        Pap = stat.pap;
        Map = stat.map;
        Par = stat.par;
        Mar = stat.mar;
        Wapmin = stat.wapmin;
        Wapmax = stat.wapmax;
        Dmg = stat.dmg;
        Pen = stat.pen;
        Rmsp = stat.rmsp;
        Bap = stat.bap;
        BapPet = stat.bap_pet;
        Hiddenhpadd = stat.hiddenhpadd;
        Hiddennddadd = stat.hiddennddadd;
        Hiddenwapadd = stat.hiddenwapadd;
        Hiddenhpadd03 = stat.hiddenhpadd03;
        Hiddennddadd03 = stat.hiddennddadd03;
        Hiddenwapadd03 = stat.hiddenwapadd03;
        Hiddenhpadd04 = stat.hiddenhpadd04;
        Hiddennddadd04 = stat.hiddennddadd04;
        Hiddenwapadd04 = stat.hiddenwapadd04;
        ScaleStatRate1 = stat.scaleStatRate_1;
        ScaleStatRate2 = stat.scaleStatRate_2;
        ScaleStatRate3 = stat.scaleStatRate_3;
        ScaleStatRate4 = stat.scaleStatRate_4;
        ScaleBaseTap1 = stat.scaleBaseTap_1;
        ScaleBaseTap2 = stat.scaleBaseTap_2;
        ScaleBaseTap3 = stat.scaleBaseTap_3;
        ScaleBaseTap4 = stat.scaleBaseTap_4;
        ScaleBaseDef1 = stat.scaleBaseDef_1;
        ScaleBaseDef2 = stat.scaleBaseDef_2;
        ScaleBaseDef3 = stat.scaleBaseDef_3;
        ScaleBaseDef4 = stat.scaleBaseDef_4;
        ScaleBaseSpaRate1 = stat.scaleBaseSpaRate_1;
        ScaleBaseSpaRate2 = stat.scaleBaseSpaRate_2;
        ScaleBaseSpaRate3 = stat.scaleBaseSpaRate_3;
        ScaleBaseSpaRate4 = stat.scaleBaseSpaRate_4;
    }
}
