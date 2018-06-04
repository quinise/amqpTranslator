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
            void addToArray(ref byte destination, byte source, int index){
                if (index % 2 == 0){
                    source = (byte)(source << 4);
                }
                destination |= source;  
            }

            int getInt32(byte[] source){
                byte[] destination = new Byte[4];
                Array.Copy(source, destination, 4);
                Array.Reverse(destination);
                return BitConverter.ToInt32(destination, 0);
            }

            int getInt16(byte[] source){
                byte[] destination = new Byte[2];
                Array.Copy(source, destination, 2);
                Array.Reverse(destination);
                return BitConverter.ToInt16(destination, 0);
            }            
            
            List<string> amqpTranslate(byte[] source){
                List<string> currentOutputList = new List<string>(new string[] {" "});
                int i = 0;
                int lengthOfFrame = getInt32(source);
                i = i+4;
                if(lengthOfFrame != source.Length){
                    currentOutputList.Add("Frame length does not match provided frame");
                    return currentOutputList;
                } else {
                    currentOutputList.Add("Frame Length: "+ lengthOfFrame +" bytes ");
                    int dOff = 4 * source[i]; 
                    currentOutputList.Add("Data offset at position: " + dOff);
                    i++;
                    if (source[i] == 0) {
                        currentOutputList.Add("Frame type: AMQP");
                        i++;
                    } else if (source[i]== 1) {
                        currentOutputList.Add("Frame type: SASL");
                        i++;
                    } else if (source[i] != 1) {
                        currentOutputList.Add("Type not found");
                    }

                    int channel = getInt16(source.Skip(i).Take(2).ToArray());
                    currentOutputList.Add("Channel: " + channel);
                    i = i + 2;

                    if (dOff == i) {
                        currentOutputList.Add("No extended header");
                    } else {
                        currentOutputList.Add("Extended header present");
                        i = dOff;
                    }
                    
                    return currentOutputList;
                }
            }

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
                            addToArray(ref resultArr[currentOutputPosition], (byte)1, i);
                            break;
                        case '2':
                            addToArray(ref resultArr[currentOutputPosition], (byte)2, i);
                            break;
                        case '3':
                             addToArray(ref resultArr[currentOutputPosition], (byte)3, i);
                            break;                    
                        case '4':
                            addToArray(ref resultArr[currentOutputPosition], (byte)4, i);
                            break;
                        case '5':
                          addToArray(ref resultArr[currentOutputPosition], (byte)5, i);
                            break;
                        case '6':
                            addToArray(ref resultArr[currentOutputPosition], (byte)6, i);
                            break;
                        case '7':
                            addToArray(ref resultArr[currentOutputPosition], (byte)7, i);
                            break;
                        case '8':
                            addToArray(ref resultArr[currentOutputPosition], (byte)8, i);
                            break;
                        case '9':
                            addToArray(ref resultArr[currentOutputPosition], (byte)9, i);
                            break;
                        case 'a':
                        case 'A':
                             addToArray(ref resultArr[currentOutputPosition], (byte)10, i);                          
                            break;
                        case 'b':
                        case 'B':
                             addToArray(ref resultArr[currentOutputPosition], (byte)11, i);
                            break;                    
                        case 'c':
                        case 'C':
                            addToArray(ref resultArr[currentOutputPosition], (byte)12, i);
                            break;                    
                        case 'd':
                        case 'D':
                            addToArray(ref resultArr[currentOutputPosition], (byte)13, i);
                            break;                    
                        case 'e':
                        case 'E':
                            addToArray(ref resultArr[currentOutputPosition], (byte)14, i);                              
                            break;                    
                        case 'f':
                        case 'F':
                            addToArray(ref resultArr[currentOutputPosition], (byte)15, i);                           
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
            List<string> outputList = amqpTranslate(resultArr);
            ViewBag.result = outputList;
            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
