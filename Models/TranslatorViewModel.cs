using System;
using System.ComponentModel.DataAnnotations;

namespace translator.Models{
    public class TranslatorFormViewModel{
        [Display(Name="amqp Text")]
        [Required]
        public string amqpText {get; set;}
    }
}
