using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiileApp.Pages;

public class EuroopaRiik
{
    public string Nimi { get; set; }
    public string Pealinn { get; set; }
    public int Rahvaarv { get; set; }
    public string Lipp { get; set; }
    public string Kirjaldus { get; set; }

    public override bool Equals(object obj) =>
        obj is EuroopaRiik other && other.Nimi == this.Nimi;

    public override int GetHashCode() => Nimi.GetHashCode();
}
