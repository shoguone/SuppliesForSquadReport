//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuppliesForSquadReport.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class LOG_P
    {
        public int ID { get; set; }
        public Nullable<int> KOD { get; set; }
        public string O_N_KOM { get; set; }
        public string N_N_KOM { get; set; }
        public Nullable<System.DateTime> D_IZM { get; set; }
        public Nullable<System.TimeSpan> T_IZM { get; set; }
    }
}