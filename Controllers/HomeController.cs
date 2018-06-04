using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using translator.Models;

namespace translator.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.result = " ";
            return View();
        }
        [Route("translate")]
        public IActionResult Translate(TranslatorFormViewModel model)
        {
            Translator translator = new Translator();
            string result = model.amqpText;
            byte[] resultArr = new byte[result.Length/2];

            if (result == null){
                result = " ";
            } else if(result.Length % 2 != 0){
                result = "Invalid amqp";
            } else {
                int currentOutputPosition = 0;
                int i = 0;
                for(; i < result.Length; i++){
                    bool badChar = false;
                    switch (result[i]) {
                        case '0':
                            break;
                        case '1':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)1, i);
                            break;
                        case '2':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)2, i);
                            break;
                        case '3':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)3, i);
                            break;                    
                        case '4':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)4, i);
                            break;
                        case '5':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)5, i);
                            break;
                        case '6':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)6, i);
                            break;
                        case '7':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)7, i);
                            break;
                        case '8':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)8, i);
                            break;
                        case '9':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)9, i);
                            break;
                        case 'a':
                        case 'A':
                             translator.addToArray(ref resultArr[currentOutputPosition], (byte)10, i);                          
                            break;
                        case 'b':
                        case 'B':
                             translator.addToArray(ref resultArr[currentOutputPosition], (byte)11, i);
                            break;                    
                        case 'c':
                        case 'C':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)12, i);
                            break;                    
                        case 'd':
                        case 'D':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)13, i);
                            break;                    
                        case 'e':
                        case 'E':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)14, i);                              
                            break;                    
                        case 'f':
                        case 'F':
                            translator.addToArray(ref resultArr[currentOutputPosition], (byte)15, i);                           
                            break;
                        default:
                            badChar = true;
                            result = "Invalid amqp";
                            break;
                    }
                    if (badChar){
                        break;
                    } else if((i % 2) == 1){
                        currentOutputPosition++;
                    }                       
                }
            }
            List<string> outputList = translator.amqpTranslate(resultArr);
            ViewBag.result = outputList;
            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
