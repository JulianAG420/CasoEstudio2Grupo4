﻿namespace CasoEstudio2G4.Models
{
    public class CasaModel
    {
        public long IdCasa { get; set; }
        public string DescripcionCasa { get; set; }
        public decimal PrecioCasa { get; set; }
        public string UsuarioAlquiler { get; set; }
        public string Estado { get; set; }
        public string FechaAlquiler { get; set; } 
    }
}