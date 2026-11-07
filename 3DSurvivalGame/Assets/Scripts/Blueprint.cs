using UnityEngine;

[System.Serializable]
public class Blueprint
{
    public string itemName;
    public string Req1;
    public string Req2;
    public int Req1amount;
    public int Req2amount;
    public int numOfRequirements;

    // Constructor dùng từ khóa 'new' thoải mái
    public Blueprint(string name, int reqNUM, string R1, int R1num, string R2, int R2num)
    {
        itemName = name;
        numOfRequirements = reqNUM;
        Req1 = R1;
        Req2 = R2;
        Req1amount = R1num;
        Req2amount = R2num;
    }
}